// Module Name: TonalEvaluator.cs
// Project:     Bach.Model
// Copyright (c) 2012, 2026  Eddie Velasquez.
//
// This source is subject to the MIT License.
// See http://opensource.org/licenses/MIT.
// All other rights reserved.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software
// and associated documentation files (the "Software"), to deal in the Software without restriction,
// including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense,
// and/or sell copies of the Software, and to permit persons to whom the Software is furnished to
// do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial
// portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A
// PARTICULAR PURPOSE AND NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE
// OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System.Collections.Generic;
using System.Linq;
using Bach.Model.Analysis.Internal;
using Bach.Model.Internal;

namespace Bach.Model.Analysis;

/// <summary>
///   Evaluates a <see cref="PartEventScope"/> and produces ranked tonal candidates.
/// </summary>
/// <remarks>
///   This evaluator is duration-free and uses spelling-sensitive keys discovered from the registry by
///   default. Callers may supply additional <see cref="Key"/> candidates. The evaluator records simple
///   scale-degree and set membership evidence and uses the supplied <see cref="RepertoireProfile"/>
///   to filter registry candidates. Evidence providers run in descending priority order and all
///   providers contribute; priority does not short-circuit lower-priority musical observations.
/// </remarks>
public sealed class TonalEvaluator
{
  #region Nested Types

  /// <summary>
  ///   Represents a scored candidate key with associated evidence counts.
  /// </summary>
  /// <param name="Key">The candidate key.</param>
  /// <param name="Confidence">The confidence score of the candidate.</param>
  /// <param name="MatchedCount">The number of matched pitch classes.</param>
  /// <param name="HarmonicEvidenceCount">The amount of harmonic evidence supporting the candidate.</param>
  /// <param name="Evidence">The list of tonal evidence.</param>
  private sealed record CandidateScore(
    Key Key,
    double Confidence,
    int MatchedCount,
    int HarmonicEvidenceCount,
    IReadOnlyList<TonalEvidence> Evidence );

  #endregion

  #region Fields

  private readonly RepertoireProfile _profile;
  private readonly TonalEvidenceEvaluatorPipeline _evaluatorPipeline;

  #endregion

  #region Constructors

  /// <summary>
  ///   Initializes a new evaluator with an optional repertoire profile and evidence providers.
  /// </summary>
  /// <param name="profile">The repertoire profile used to filter registry candidates.</param>
  /// <param name="pipeline">The pipeline of tonal evidence evaluators.</param>
  public TonalEvaluator(
    RepertoireProfile? profile = null,
    TonalEvidenceEvaluatorPipeline? pipeline = null )
  {
    _profile = profile ?? RepertoireProfile.Default;
    _evaluatorPipeline = pipeline ?? new TonalEvidenceEvaluatorPipelineBuilder().AddDefaultEvaluators().Build();
  }

  #endregion

  #region Public Methods

  /// <summary>
  ///   Evaluates the supplied scope and returns a ranked result set or an inconclusive result.
  /// </summary>
  /// <param name="scope">The scope of events to evaluate.</param>
  /// <param name="additionalCandidates">Optional additional key candidates to consider.</param>
  /// <param name="options">Options that control which ranked candidates are returned.</param>
  /// <returns>A <see cref="TonalAnalysisResult"/> containing the evaluation results.</returns>
  public TonalAnalysisResult Evaluate(
    PartEventScope scope,
    IEnumerable<Key>? additionalCandidates = null,
    TonalEvaluationOptions? options = null )
  {
    ArgumentNullException.ThrowIfNull( scope );

    var evaluationOptions = options ?? TonalEvaluationOptions.Default;
    evaluationOptions.Validate();

    // Extract events from the scope and ensure there are events to evaluate.
    var events = scope.Events.ToArray();

    if( events.Length == 0 )
    {
      return new TonalAnalysisResultBuilder( scope ).Inconclusive( "No events in scope." )
                                                    .Build();
    }

    // Extract unique pitch classes from the events and ensure there are pitch classes to evaluate.
    var uniquePitchClasses = GetUniquePitchClasses( events );

    if( uniquePitchClasses.Length == 0 )
    {
      return new TonalAnalysisResultBuilder( scope ).Inconclusive( "No pitch classes in scope." )
                                                    .Build();
    }

    // Get the default candidates from the registry and concatenate any additional candidates provided by the caller.
    var candidates = TonalCandidateSource.GetDefaultCandidates( _profile )
                                         .Concat( additionalCandidates ?? Array.Empty<Key>() );

    // Score the candidates against the events and unique pitch classes.
    var scored = ScoreCandidates( scope, events, uniquePitchClasses, candidates, evaluationOptions )
      .ToArray();

    if( scored.Length == 0 )
    {
      return new TonalAnalysisResultBuilder( scope ).Inconclusive( "No candidate matched the observed pitch classes." )
                                                    .Build();
    }

    // Order the scored candidates by confidence, matched count, harmonic evidence count, and other criteria.
    var ordered = scored.OrderByDescending( s => s.Confidence )
                        .ThenByDescending( s => s.MatchedCount )
                        .ThenByDescending( s => s.HarmonicEvidenceCount )
                        .ThenBy( s => s.Key.ScaleDefinition.FormulaId == ScaleDefinition.Major.FormulaId ? 0 : 1 )
                        .ThenBy( s => s.Key.Scale.Formula.Id, Comparer.IdComparer )
                        .ThenBy( s => s.Key.Tonic )
                        .ToArray();

    return BuildResultSet(
      scope,
      uniquePitchClasses,
      SelectCandidates( ordered, evaluationOptions )
    );
  }

  /// <summary>
  ///   Convenience overload that accepts a part and range.
  /// </summary>
  /// <param name="source">The part to evaluate.</param>
  /// <param name="range">The range of the part to evaluate.</param>
  /// <param name="additionalCandidates">Optional additional key candidates to consider.</param>
  /// <param name="options">Options that control which ranked candidates are returned.</param>
  /// <returns>A <see cref="TonalAnalysisResult"/> containing the evaluation results.</returns>
  public TonalAnalysisResult Evaluate(
    Part source,
    Range range,
    IEnumerable<Key>? additionalCandidates = null,
    TonalEvaluationOptions? options = null )
  {
    return Evaluate( new PartEventScope( source, range ), additionalCandidates, options );
  }

  #endregion

  #region Implementation

  /// <summary>
  ///   Extracts unique pitch classes from the provided events.
  /// </summary>
  /// <param name="events">The events from which to extract unique pitch classes.</param>
  /// <returns>An array of unique pitch classes.</returns>
  private static PitchClass[] GetUniquePitchClasses(
    IEnumerable<IPartEvent> events )
  {
    return
    [
      .. events.SelectMany( e => e.PitchClasses )
               .Distinct()
    ];
  }

  /// <summary>
  ///   Scores the candidate keys against the provided events and unique pitch classes.
  /// </summary>
  /// <param name="scope">The part event scope being evaluated.</param>
  /// <param name="events">The events to evaluate.</param>
  /// <param name="uniquePitchClasses">The unique pitch classes extracted from the events.</param>
  /// <param name="candidates">The candidate keys to score.</param>
  /// <param name="options">Options that control which ranked candidates are returned.</param>
  /// <returns>
  ///   A <see cref="IEnumerable{CandidateScore}"/> representing the scores of the candidates.
  /// </returns>
  private IEnumerable<CandidateScore> ScoreCandidates(
    PartEventScope scope,
    IPartEvent[] events,
    PitchClass[] uniquePitchClasses,
    IEnumerable<Key> candidates,
    TonalEvaluationOptions options )
  {
    return candidates.Select( candidate => ScoreCandidate(
                                new TonalEvidenceContext(
                                  candidate,
                                  scope,
                                  events,
                                  uniquePitchClasses,
                                  options
                                )
                              ) )
                     .OfType<CandidateScore>();
  }

  /// <summary>
  ///   Scores a single candidate key against the provided events and unique pitch classes.
  /// </summary>
  /// <param name="context">The immutable context for the candidate key.</param>
  /// <returns>
  ///   A <see cref="CandidateScore"/> representing the score of the candidate key, or null if no pitch classes match.
  /// </returns>
  private CandidateScore? ScoreCandidate(
    TonalEvidenceContext context )
  {
    var matched = context.UniquePitchClasses.Count( context.CandidateKey.Scale.Contains );

    // If no pitch classes match the candidate scale, we can skip further evaluation.
    if( matched == 0 )
    {
      return null;
    }

    var evidences = new List<TonalEvidence>();
    var tonalCenterScore = 0.0;

    // Evaluate the candidate key against the events and unique pitch classes using the evidence evaluators.
    foreach( var (evidence, weightedScore) in _evaluatorPipeline.Evaluate( context))
    {
      evidences.Add( evidence );
      tonalCenterScore += weightedScore;
    }

    var supportingEvidenceCount = evidences.Count( evidence => evidence.Supports );
    var conflictingEvidenceCount = evidences.Count( evidence => !evidence.Supports );

    // Calculate confidence based on matched pitch classes and harmonic evidence.
    var confidence = CalculateConfidence(
      context,
      matched,
      supportingEvidenceCount,
      conflictingEvidenceCount,
      tonalCenterScore
    );

    return new CandidateScore(
      context.CandidateKey,
      confidence,
      matched,
      supportingEvidenceCount,
      evidences
    );
  }

  /// <summary>
  ///   Calculates a confidence score for a candidate key based on matched pitch classes and harmonic evidence.
  /// </summary>
  /// <param name="context">The immutable context for the candidate key.</param>
  /// <param name="matchedCount">The number of pitch classes that match the candidate scale.</param>
  /// <param name="supportingEvidenceCount">The number of supporting harmonic evidence items.</param>
  /// <param name="conflictingEvidenceCount">The number of conflicting harmonic evidence items.</param>
  /// <param name="tonalCenterScore">The score representing the strength of the tonal center.</param>
  /// <returns>A confidence score between 0.0 and 1.0.</returns>
  private double CalculateConfidence(
    TonalEvidenceContext context,
    int matchedCount,
    int supportingEvidenceCount,
    int conflictingEvidenceCount,
    double tonalCenterScore )
  {
    // Calculate the profile weight based on the candidate key's scale formula and the repertoire profile.
    // The profile weight is the maximum weight of the tags associated with the candidate key's scale formula.
    var profileWeight = context.CandidateKey.Scale.Formula.Classification.RepertoireTags
                                    .Select( _profile.GetWeight )
                                    .DefaultIfEmpty( 0.0 )
                                    .Max();

    // Calculate the base confidence as the ratio of matched pitch classes to unique pitch classes.
    var baseConfidence = (double) matchedCount / context.UniquePitchClasses.Count;

    // Adjust the confidence based on supporting harmonic evidence.
    var harmonicAdjustment = supportingEvidenceCount == 0
      ? 0.0
      : Math.Min( 0.2, supportingEvidenceCount * 0.025 );

    // Adjust the confidence based on conflicting harmonic evidence.
    var conflictAdjustment = Math.Min( 0.2, conflictingEvidenceCount * 0.025 );
    var profileAdjustment = Math.Min( 0.1, profileWeight * 0.1 );

    // Tonal-center adjustment is provided by the ordered-event analysis and is already bounded by the analyzer.
    var tonalAdjustment = Math.Clamp( tonalCenterScore, 0.0, 0.35 );

    // Return the final confidence score, clamped between 0.0 and 1.0.
    return Math.Clamp(
      baseConfidence + harmonicAdjustment + profileAdjustment + tonalAdjustment - conflictAdjustment,
      0.0,
      1.0
    );
  }

  /// <summary>
  ///   Builds a <see cref="TonalAnalysisResult"/> from the scored candidates and their evidence.
  /// </summary>
  /// <param name="scope">The scope of the part events.</param>
  /// <param name="uniquePitchClasses">The unique pitch classes in the part events.</param>
  /// <param name="ordered">The scored candidate keys, ordered by confidence.</param>
  /// <returns>A <see cref="TonalAnalysisResult"/> representing the analysis results.</returns>
  private static TonalAnalysisResult BuildResultSet(
    PartEventScope scope,
    PitchClass[] uniquePitchClasses,
    IEnumerable<CandidateScore> ordered )
  {
    var setBuilder = new TonalAnalysisResultSetBuilder( scope );
    var rank = 1;

    foreach( var item in ordered )
    {
      var supporting = new List<EvidenceReason>
      {
        new(
          EvidenceReasonCategory.ScaleContext,
          $"Matches {item.MatchedCount} of {uniquePitchClasses.Length} pitch classes."
        )
      };

      var conflicting = new List<EvidenceReason>();

      foreach( var evidence in item.Evidence )
      {
        var target = evidence.Supports ? supporting : conflicting;
        target.Add( evidence.Reason );
      }

      var candidateRecord = new RankedTonalCandidateResult(
        rank,
        item.Key.Tonic,
        item.Key,
        item.Confidence,
        supporting,
        conflicting,
        scope
      );

      setBuilder.AddCandidate( candidateRecord );
      ++rank;
    }

    return setBuilder.Build();
  }

  /// <summary>
  ///   Selects candidates from the ordered list based on the provided evaluation options.
  /// </summary>
  /// <param name="ordered">The ordered list of candidate scores.</param>
  /// <param name="options">The evaluation options to apply.</param>
  /// <returns>The selected candidates based on the evaluation options.</returns>
  private static IEnumerable<CandidateScore> SelectCandidates(
    IReadOnlyList<CandidateScore> ordered,
    TonalEvaluationOptions options )
  {
    if( options.IncludeAllCandidates )
    {
      return ordered;
    }

    var confidenceFloor = ordered[0].Confidence - options.ConfidenceDelta;

    return ordered.Where( candidate => candidate.Confidence >= confidenceFloor )
                  .Take( options.MaximumCandidates );
  }

  #endregion
}
