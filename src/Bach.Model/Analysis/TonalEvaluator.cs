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

namespace Bach.Model.Analysis;

/// <summary>
///   Evaluates a <see cref="PartEventScope"/> and produces ranked tonal candidates.
/// </summary>
/// <remarks>
///   This evaluator is duration-free and uses spelling-sensitive keys discovered from the registry by
///   default. Callers may supply additional <see cref="Key"/> candidates. The evaluator records simple
///   scale-degree and set membership evidence and uses the supplied <see cref="RepertoireProfile"/>
///   to filter registry candidates. Results are constructed using the existing result builders.
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
  /// <param name="SupportingHarmonicEvidence">A list of supporting harmonic evidence.</param>
  /// <param name="ConflictingHarmonicEvidence">A list of conflicting harmonic evidence.</param>
  private sealed record CandidateScore(
    Key Key,
    double Confidence,
    int MatchedCount,
    int HarmonicEvidenceCount,
    IReadOnlyList<string> SupportingHarmonicEvidence,
    IReadOnlyList<string> ConflictingHarmonicEvidence );

  #endregion

  #region Fields

  private readonly RepertoireProfile _profile;

  #endregion

  #region Constructors

  /// <summary>
  ///   Initializes a new evaluator with an optional repertoire profile.
  /// </summary>
  public TonalEvaluator(
    RepertoireProfile? profile = null )
  {
    _profile = profile ?? RepertoireProfile.Default;
  }

  #endregion

  #region Public Methods

  /// <summary>
  ///   Evaluates the supplied scope and returns a ranked result set or an inconclusive result.
  /// </summary>
  /// <param name="scope">The scope of events to evaluate.</param>
  /// <param name="additionalCandidates">Optional additional key candidates to consider.</param>
  /// <returns>A <see cref="TonalAnalysisResult"/> containing the evaluation results.</returns>
  public TonalAnalysisResult Evaluate(
    PartEventScope scope,
    IEnumerable<Key>? additionalCandidates = null )
  {
    ArgumentNullException.ThrowIfNull( scope );

    var events = scope.Events.ToArray();

    if( events.Length == 0 )
    {
      return new TonalAnalysisResultBuilder( scope ).Inconclusive( "No events in scope." )
                                                    .Build();
    }

    var uniquePitchClasses = GetUniquePitchClasses( events );

    if( uniquePitchClasses.Length == 0 )
    {
      return new TonalAnalysisResultBuilder( scope ).Inconclusive( "No pitch classes in scope." )
                                                    .Build();
    }

    var candidates = TonalCandidateSource.GetDefaultCandidates( _profile )
                                         .Concat( additionalCandidates ?? Array.Empty<Key>() );

    var scored = ScoreCandidates( events, uniquePitchClasses, candidates )
      .ToArray();

    if( scored.Length == 0 )
    {
      return new TonalAnalysisResultBuilder( scope ).Inconclusive( "No candidate matched the observed pitch classes." )
                                                    .Build();
    }

    return BuildResultSet(
      scope,
      events,
      uniquePitchClasses,
      scored.OrderByDescending( s => s.Confidence )
            .ThenByDescending( s => s.MatchedCount )
            .ThenByDescending( s => s.HarmonicEvidenceCount )
            .ThenBy( s => s.Key.Scale.Formula.Id, StringComparer.Ordinal )
            .ThenBy( s => s.Key.Tonic )
    );
  }

  /// <summary>
  ///   Convenience overload that accepts a part and range.
  /// </summary>
  /// <param name="source">The part to evaluate.</param>
  /// <param name="range">The range of the part to evaluate.</param>
  /// <param name="additionalCandidates">Optional additional key candidates to consider.</param>
  /// <returns>A <see cref="TonalAnalysisResult"/> containing the evaluation results.</returns>
  public TonalAnalysisResult Evaluate(
    Part source,
    Range range,
    IEnumerable<Key>? additionalCandidates = null )
  {
    return Evaluate( new PartEventScope( source, range ), additionalCandidates );
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
  /// <param name="events">The events to evaluate.</param>
  /// <param name="uniquePitchClasses">The unique pitch classes extracted from the events.</param>
  /// <param name="candidates">The candidate keys to score.</param>
  /// <returns>
  ///   A <see cref="IEnumerable{CandidateScore}"/> representing the scores of the candidates.
  /// </returns>
  private IEnumerable<CandidateScore> ScoreCandidates(
    IPartEvent[] events,
    PitchClass[] uniquePitchClasses,
    IEnumerable<Key> candidates )
  {
    return candidates.Select( candidate => ScoreCandidate( candidate, events, uniquePitchClasses ) )
                     .OfType<CandidateScore>();
  }

  /// <summary>
  ///   Scores a single candidate key against the provided events and unique pitch classes.
  /// </summary>
  /// <param name="candidateKey">The candidate key to score.</param>
  /// <param name="events">The events to evaluate.</param>
  /// <param name="uniquePitchClasses">The unique pitch classes extracted from the events.</param>
  /// <returns>
  ///   A <see cref="CandidateScore"/> representing the score of the candidate key, or null if no pitch classes match.
  /// </returns>
  private CandidateScore? ScoreCandidate(
    Key candidateKey,
    IPartEvent[] events,
    PitchClass[] uniquePitchClasses )
  {
    var scale = candidateKey.Scale;
    var matched = uniquePitchClasses.Count( scale.Contains );

    // If no pitch classes match the candidate scale, we can skip further evaluation.
    if( matched == 0 )
    {
      return null;
    }

    List<string> supportingHarmonicEvidence = [];
    List<string> conflictingHarmonicEvidence = [];

    // Evaluate harmonic evidence from chord events.
    foreach( var chordEvent in events.OfType<IChordEvent>() )
    {
      AddHarmonicEvidence( scale, chordEvent, supportingHarmonicEvidence, conflictingHarmonicEvidence );
    }

    // Calculate confidence based on matched pitch classes and harmonic evidence.
    var confidence = CalculateConfidence(
      candidateKey,
      matched,
      uniquePitchClasses.Length,
      supportingHarmonicEvidence.Count,
      conflictingHarmonicEvidence.Count
    );

    return new CandidateScore(
      candidateKey,
      confidence,
      matched,
      supportingHarmonicEvidence.Count,
      supportingHarmonicEvidence,
      conflictingHarmonicEvidence
    );
  }

  /// <summary>
  ///   Calculates a confidence score for a candidate key based on matched pitch classes and harmonic evidence.
  /// </summary>
  /// <param name="candidateKey">The candidate key to evaluate.</param>
  /// <param name="matchedCount">The number of pitch classes that match the candidate scale.</param>
  /// <param name="uniquePitchClassCount">The total number of unique pitch classes in the events.</param>
  /// <param name="supportingEvidenceCount">The number of supporting harmonic evidence items.</param>
  /// <param name="conflictingEvidenceCount">The number of conflicting harmonic evidence items.</param>
  /// <returns>A confidence score between 0.0 and 1.0.</returns>
  private double CalculateConfidence(
    Key candidateKey,
    int matchedCount,
    int uniquePitchClassCount,
    int supportingEvidenceCount,
    int conflictingEvidenceCount )
  {
    // Calculate the profile weight based on the candidate key's scale formula and the repertoire profile.
    // The profile weight is the maximum weight of the tags associated with the candidate key's scale formula.
    var profileWeight = candidateKey.Scale.Formula.Classification.RepertoireTags
                                    .Select( _profile.GetWeight )
                                    .DefaultIfEmpty( 0.0 )
                                    .Max();

    // Calculate the base confidence as the ratio of matched pitch classes to unique pitch classes.
    var baseConfidence = (double) matchedCount / uniquePitchClassCount;

    // Adjust the confidence based on supporting and conflicting harmonic evidence.
    var harmonicAdjustment = supportingEvidenceCount == 0
      ? 0.0
      : Math.Min( 0.2, supportingEvidenceCount * 0.025 );

    // Adjust the confidence based on conflicting harmonic evidence.
    var conflictAdjustment = Math.Min( 0.2, conflictingEvidenceCount * 0.025 );
    var profileAdjustment = Math.Min( 0.1, profileWeight * 0.1 );

    // Return the final confidence score, clamped between 0.0 and 1.0.
    return Math.Clamp(
      baseConfidence + harmonicAdjustment + profileAdjustment - conflictAdjustment,
      0.0,
      1.0
    );
  }

  /// <summary>
  ///   Adds harmonic evidence for a chord event against a candidate scale.
  /// </summary>
  /// <param name="scale">The candidate scale to evaluate against.</param>
  /// <param name="chordEvent">The chord event to evaluate.</param>
  /// <param name="supportingEvidence">A list to which supporting evidence strings will be added.</param>
  /// <param name="conflictingEvidence">A list to which conflicting evidence strings will be added.</param>
  private static void AddHarmonicEvidence(
    Scale scale,
    IChordEvent chordEvent,
    List<string> supportingEvidence,
    List<string> conflictingEvidence )
  {
    // Evaluate the chord's root, bass, and pitch classes against the candidate scale.
    var rootMatches = scale.Contains( new[] { chordEvent.Root.PitchClass } );
    var bassMatches = scale.Contains( new[] { chordEvent.Bass.PitchClass } );
    var chordMatches = chordEvent.PitchClasses.All( scale.Contains );

    AddEvidence( rootMatches, $"Chord root {chordEvent.Root.PitchClass} is in the candidate scale.", $"Chord root {chordEvent.Root.PitchClass} is outside the candidate scale." );
    AddEvidence( bassMatches, $"Chord bass {chordEvent.Bass.PitchClass} is in the candidate scale.", $"Chord bass {chordEvent.Bass.PitchClass} is outside the candidate scale." );
    AddEvidence( chordMatches, $"Chord formula {chordEvent.Formula.Name} is contained by the candidate scale.", $"Chord formula {chordEvent.Formula.Name} has tones outside the candidate scale." );

    switch (chordEvent.Inversion)
    {
      case 0 when chordEvent.Bass.PitchClass == chordEvent.Root.PitchClass:
        supportingEvidence.Add( "The chord is in root position." );
        break;

      case > 0:
        supportingEvidence.Add( $"The chord inversion is {chordEvent.Inversion}." );
        break;
    }

    return;

    void AddEvidence(
      bool supporting,
      string supportingReason,
      string conflictingReason )
    {
      if( supporting )
      {
        supportingEvidence.Add( supportingReason );
      }
      else
      {
        conflictingEvidence.Add( conflictingReason );
      }
    }
  }

  /// <summary>
  ///   Builds a <see cref="TonalAnalysisResult"/> from the scored candidates and their evidence.
  /// </summary>
  /// <param name="scope">The scope of the part events.</param>
  /// <param name="events">The part events to analyze.</param>
  /// <param name="uniquePitchClasses">The unique pitch classes in the part events.</param>
  /// <param name="ordered">The scored candidate keys, ordered by confidence.</param>
  /// <returns>A <see cref="TonalAnalysisResult"/> representing the analysis results.</returns>
  private static TonalAnalysisResult BuildResultSet(
    PartEventScope scope,
    IPartEvent[] events,
    PitchClass[] uniquePitchClasses,
    IEnumerable<CandidateScore> ordered )
  {
    var setBuilder = new TonalAnalysisResultSetBuilder( scope );
    var rank = 1;

    foreach( var item in ordered )
    {
      // Add scale context evidence for the candidate.
      var supporting = new List<EvidenceReason>
      {
        new(
          EvidenceReasonCategory.ScaleContext,
          $"Matches {item.MatchedCount} of {uniquePitchClasses.Length} pitch classes."
        )
      };

      var conflicting = new List<EvidenceReason>();

      // Add harmonic context evidence for the candidate based on chord events.
      foreach( var chordEvent in events.OfType<IChordEvent>() )
      {
        AddChordCandidateEvidence( item.Key.Scale, chordEvent, supporting, conflicting );
      }

      // Create a ranked tonal candidate result with the accumulated evidence.
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
  ///   Adds evidence for a chord event against a candidate scale, categorizing it as supporting or conflicting.
  /// </summary>
  /// <param name="scale">The candidate scale.</param>
  /// <param name="chordEvent">The chord event to evaluate.</param>
  /// <param name="supporting">The list to which supporting evidence will be added.</param>
  /// <param name="conflicting">The list to which conflicting evidence will be added.</param>
  private static void AddChordCandidateEvidence(
    Scale scale,
    IChordEvent chordEvent,
    List<EvidenceReason> supporting,
    List<EvidenceReason> conflicting )
  {
    // Evaluate the chord's root against the candidate scale and add evidence accordingly.
    if( scale.Contains( new[] { chordEvent.Root.PitchClass } ) )
    {
      AddEvidenceReason(
        supporting,
        EvidenceReasonCategory.HarmonicContext,
        $"Chord root {chordEvent.Root.PitchClass} supports the candidate."
      );
    }
    else
    {
      AddEvidenceReason(
        conflicting,
        EvidenceReasonCategory.HarmonicContext,
        $"Chord root {chordEvent.Root.PitchClass} conflicts with the candidate."
      );
    }

    // Evaluate the chord's bass against the candidate scale and add evidence accordingly.
    if( scale.Contains( new[] { chordEvent.Bass.PitchClass } ) )
    {
      AddEvidenceReason(
        supporting,
        EvidenceReasonCategory.HarmonicContext,
        $"Chord bass {chordEvent.Bass.PitchClass} supports the candidate."
      );
    }
    else
    {
      AddEvidenceReason(
        conflicting,
        EvidenceReasonCategory.HarmonicContext,
        $"Chord bass {chordEvent.Bass.PitchClass} conflicts with the candidate."
      );
    }

    // Evaluate the chord's pitch classes against the candidate scale and add evidence accordingly.
    if( chordEvent.PitchClasses.All( pc => scale.Contains( new[] { pc } ) ) )
    {
      AddEvidenceReason(
        supporting,
        EvidenceReasonCategory.HarmonicContext,
        $"Chord formula {chordEvent.Formula.Name} is contained by the candidate scale."
      );
    }
    else
    {
      AddEvidenceReason(
        conflicting,
        EvidenceReasonCategory.HarmonicContext,
        $"Chord formula {chordEvent.Formula.Name} has tones outside the candidate scale."
      );
    }
  }

  private static void AddEvidenceReason(
    List<EvidenceReason> list,
    EvidenceReasonCategory category,
    string message )
  {
    list.Add( new EvidenceReason( category, message ) );
  }

  #endregion
}
