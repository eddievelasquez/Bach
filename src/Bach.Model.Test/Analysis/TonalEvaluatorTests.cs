// Module Name: TonalEvaluatorTests.cs
// Project:     Bach.Model.Test
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
using System.IO;
using System.Linq;

namespace Bach.Model.Analysis.Test;

public class TonalEvaluatorTests
{
  #region Nested Types

  private sealed class EmptyEvent: IPartEvent
  {
    #region Properties

    public IEnumerable<PitchClass> PitchClasses => Array.Empty<PitchClass>();

    #endregion
  }

  #endregion

  #region Public Methods

  [Fact]
  public void PartEventScope_ShouldStoreAppliedFunctionsImmutably()
  {
    var part = new Part( [Pitch.Create( PitchClass.D, 4 )] );
    var evidence = new EvidenceReason( EvidenceReasonCategory.AnalystObservation, "The target is tonicized." );

    var appliedFunction = new AppliedFunction(
      AnalysisTarget.ForEvent( part[0] ),
      ScaleDegree.Dominant,
      AppliedTriadFunction.Dominant,
      evidence
    );
    var input = new List<AppliedFunction> { appliedFunction };

    var scope = new PartEventScope( part, .., input );
    input.Clear();

    scope.AppliedFunctions.Should()
         .ContainSingle()
         .Which.Should()
         .Be( appliedFunction );
  }

  [Fact]
  public void PartEventScope_ShouldRejectAppliedFunctionTargetOutsideScope()
  {
    var part = new Part( [Pitch.Create( PitchClass.C, 4 ), Pitch.Create( PitchClass.D, 4 )] );

    var appliedFunction = new AppliedFunction(
      AnalysisTarget.ForEvent( part[1] ),
      ScaleDegree.Dominant,
      AppliedTriadFunction.Dominant,
      new EvidenceReason( EvidenceReasonCategory.AnalystObservation, "The target is tonicized." )
    );

    Action action = () => new PartEventScope( part, ..1, [appliedFunction] );

    action.Should()
          .Throw<ArgumentException>()
          .WithMessage( "*target must belong*" );
  }

  [Fact]
  public void Evaluate_ShouldPreserveAppliedDominantEvidence()
  {
    var part = new Part( [Pitch.Create( PitchClass.D, 4 )] );

    var scope = new PartEventScope(
      part,
      ..,
      [
        new AppliedFunction(
          AnalysisTarget.ForEvent( part[0] ),
          ScaleDegree.Dominant,
          AppliedTriadFunction.Dominant,
          new EvidenceReason( EvidenceReasonCategory.AnalystObservation, "The dominant tonicizes V." )
        )
      ]
    );

    var result = (TonalAnalysisResultSet) new TonalEvaluator().Evaluate(
      scope,
      [new Key( PitchClass.C, ScaleDefinition.Major )],
      TonalEvaluationOptions.AllCandidates
    );

    result.Candidates.Should()
          .Contain( candidate => candidate.SupportingEvidence.Any( evidence =>
                                                                     evidence.Category
                                                                     == EvidenceReasonCategory.HarmonicContext
                                                                     && evidence.Explanation.Contains(
                                                                       "V/V",
                                                                       StringComparison.Ordinal
                                                                     )
                                                                     && evidence.Explanation.Contains(
                                                                       "The dominant tonicizes V.",
                                                                       StringComparison.Ordinal
                                                                     )
                    )
          );
  }

  [Fact]
  public void Evaluate_ShouldPreserveAppliedLeadingToneEvidenceAndRankDeterministically()
  {
    var part = new Part( [Pitch.Create( PitchClass.D, 4 )] );

    var scope = new PartEventScope(
      part,
      ..,
      [
        new AppliedFunction(
          AnalysisTarget.ForEvent( part[0] ),
          ScaleDegree.Dominant,
          AppliedTriadFunction.LeadingTone,
          new EvidenceReason( EvidenceReasonCategory.AnalystObservation, "The leading-tone triad tonicizes V." )
        )
      ]
    );
    var evaluator = new TonalEvaluator();

    var candidates = new[]
    {
      new Key( PitchClass.C, ScaleDefinition.Major ), new Key( PitchClass.G, ScaleDefinition.Major )
    };

    var first = evaluator.Evaluate( scope, candidates, TonalEvaluationOptions.AllCandidates );
    var second = evaluator.Evaluate( scope, candidates, TonalEvaluationOptions.AllCandidates );

    var firstResult = (TonalAnalysisResultSet) first;
    var secondResult = (TonalAnalysisResultSet) second;

    firstResult.Candidates.Select( candidate =>
                                     $"{candidate.Rank}:{candidate.Tonic}:{candidate.Key.ScaleDefinition.FormulaId}:{candidate.Confidence:R}"
               )
               .Should()
               .Equal(
                 secondResult.Candidates.Select( candidate =>
                                                   $"{candidate.Rank}:{candidate.Tonic}:{candidate.Key.ScaleDefinition.FormulaId}:{candidate.Confidence:R}"
                 )
               );

    var result = firstResult;

    result.Candidates.Should()
          .Contain( candidate => candidate.SupportingEvidence.Any( evidence =>
                                                                     evidence.Category
                                                                     == EvidenceReasonCategory.HarmonicContext
                                                                     && evidence.Explanation.Contains(
                                                                       "vii°/V",
                                                                       StringComparison.Ordinal
                                                                     )
                                                                     && evidence.Explanation.Contains(
                                                                       "The leading-tone triad tonicizes V.",
                                                                       StringComparison.Ordinal
                                                                     )
                    )
          );

    result.Candidates.Should()
          .OnlyContain( candidate => candidate.Confidence >= 0.0 && candidate.Confidence <= 1.0 );
  }

  /// <summary>
  ///   Verifies the duration-free harmonic reduction of the opening 16 measures of J. S. Bach's
  ///   Prelude in C major, BWV 846, from Book I of The Well-Tempered Clavier.
  /// </summary>
  /// <remarks>
  ///   The fixture is a compact piano reduction. It contains one parsed chord event per measure.
  ///   It does not represent duration, rhythm, or meter.
  /// </remarks>
  [Fact]
  public void Evaluate_ShouldReturnCMajorForBachPreludeOpeningReduction()
  {
    var part = Part.Parse( LoadFixture( "BachPreludeInCMajor-BWV846-Opening16Measures.txt" ) );

    part.Should()
        .HaveCount( 16 )
        .And.OnlyContain( partEvent => partEvent is IChordEvent );

    var scope = new PartEventScope( part );
    var evaluator = new TonalEvaluator();
    var resultSet = (TonalAnalysisResultSet) evaluator.Evaluate( scope, options: TonalEvaluationOptions.AllCandidates );

    var candidate = resultSet.Candidates.Should()
                             .Contain( result => result.Tonic == PitchClass.C
                                                 && result.Key.ScaleDefinition.FormulaId == ScaleDefinition.Major.FormulaId
                             )
                             .Which;

    candidate.Tonic.Should()
             .Be( PitchClass.C );

    candidate.Key.ScaleDefinition.FormulaId.Should()
             .Be( ScaleDefinition.Major.FormulaId );

    candidate.Confidence.Should()
             .BeInRange( 0.0, 1.0 );

    candidate.SupportingEvidence.Should()
             .Contain( evidence => evidence.Category == EvidenceReasonCategory.HarmonicContext
                                   && evidence.Explanation.Contains( "root", StringComparison.OrdinalIgnoreCase )
             )
             .And.Contain( evidence => evidence.Category == EvidenceReasonCategory.HarmonicContext
                                       && evidence.Explanation.Contains( "bass", StringComparison.OrdinalIgnoreCase )
             )
             .And.Contain( evidence => evidence.Category == EvidenceReasonCategory.HarmonicContext
                                       && evidence.Explanation.Contains( "formula", StringComparison.OrdinalIgnoreCase )
             );
  }

  /// <summary>
  ///   Verifies the duration-free mixed pitch and chord reduction of the opening of Mozart's
  ///   Piano Sonata No. 16 in C major, K. 545, first movement.
  /// </summary>
  /// <remarks>
  ///   The fixture contains sixteen representative pitch and chord events. It does not represent
  ///   duration, rhythm, or meter.
  /// </remarks>
  [Fact]
  public void Evaluate_ShouldReturnCMajorForMozartSonataOpeningReduction()
  {
    var part = Part.Parse( LoadFixture( "MozartSonataK545-OpeningMixedPitchesAndChords.txt" ) );

    part.Should()
        .HaveCount( 16 )
        .And.Contain( partEvent => partEvent is Pitch )
        .And.Contain( partEvent => partEvent is IChordEvent );

    var scope = new PartEventScope( part );
    var evaluator = new TonalEvaluator();
    var resultSet = (TonalAnalysisResultSet) evaluator.Evaluate( scope, options: TonalEvaluationOptions.AllCandidates );

    var candidate = resultSet.Candidates.Should()
                             .Contain( result => result.Tonic == PitchClass.C
                                                 && result.Key.ScaleDefinition.FormulaId == ScaleDefinition.Major.FormulaId
                             )
                             .Which;

    candidate.Confidence.Should()
             .BeInRange( 0.0, 1.0 );

    candidate.SupportingEvidence.Should()
             .Contain( evidence => evidence.Category == EvidenceReasonCategory.ScaleContext
                                   && evidence.Explanation.Contains( "scale degree", StringComparison.OrdinalIgnoreCase )
             )
             .And.Contain( evidence => evidence.Category == EvidenceReasonCategory.HarmonicContext
                                       && evidence.Explanation.Contains( "root", StringComparison.OrdinalIgnoreCase )
             )
             .And.Contain( evidence => evidence.Category == EvidenceReasonCategory.HarmonicContext
                                       && evidence.Explanation.Contains( "formula", StringComparison.OrdinalIgnoreCase )
             );
  }

  [Fact]
  public void Evaluate_NoEvents_ReturnsInconclusive()
  {
    var part = new Part();
    var scope = new PartEventScope( part );
    var evaluator = new TonalEvaluator();

    var result = evaluator.Evaluate( scope );

    result.Should()
          .BeOfType<InconclusiveTonalAnalysisResult>();
  }

  [Fact]
  public void Evaluate_ShouldReturnInconclusive_WhenEventsContainNoPitchClasses()
  {
    var scope = new PartEventScope( new Part( [new EmptyEvent()] ) );

    var result = new TonalEvaluator().Evaluate( scope );

    result.Should()
          .BeOfType<InconclusiveTonalAnalysisResult>();

    ( (InconclusiveTonalAnalysisResult) result ).Reason.Should()
                                                .Be( "No pitch classes in scope." );
  }

  [Fact]
  public void Evaluate_ShouldIncludeAdditionalKey_WhenFormulaIsNotARegistryCandidate()
  {
    var additionalKey = new Key( PitchClass.C, ScaleDefinition.FromFormulaId( "BebopDominant" ) );
    var scope = new PartEventScope( Part.Parse( "C4,E4,G4" ) );

    var result = new TonalEvaluator().Evaluate( scope, [additionalKey], TonalEvaluationOptions.AllCandidates );

    result.Should()
          .BeOfType<TonalAnalysisResultSet>();

    ( (TonalAnalysisResultSet) result ).Candidates.Should()
                                       .Contain( candidate => candidate.Key.ScaleDefinition.FormulaId == "BebopDominant"
                                                              && candidate.Tonic == PitchClass.C
                                       );
  }

  [Fact]
  public void Evaluate_ShouldPreserveSpellingSensitiveAdditionalCandidates()
  {
    var cSharp = new Key( PitchClass.CSharp, ScaleDefinition.Major );
    var dFlat = new Key( PitchClass.DFlat, ScaleDefinition.Major );

    var scope = new PartEventScope(
      new Part(
        [Pitch.Create( PitchClass.CSharp, 4 ), Pitch.Create( PitchClass.F, 4 ), Pitch.Create( PitchClass.GSharp, 4 )]
      )
    );

    var result = new TonalEvaluator().Evaluate( scope, [cSharp, dFlat], TonalEvaluationOptions.AllCandidates );

    result.Should()
          .BeOfType<TonalAnalysisResultSet>();

    ( (TonalAnalysisResultSet) result ).Candidates.Should()
                                       .Contain( candidate => candidate.Key.Tonic == PitchClass.CSharp )
                                       .And.Contain( candidate => candidate.Key.Tonic == PitchClass.DFlat );
  }

  [Fact]
  public void Evaluate_ShouldIncludeScaleContextEvidence_WhenPitchClassesMatch()
  {
    var scope = new PartEventScope( Part.Parse( "C4,E4,G4" ) );

    var result = (TonalAnalysisResultSet) new TonalEvaluator().Evaluate(
      scope,
      options: TonalEvaluationOptions.AllCandidates
    );

    var cMajor = result.Candidates.Should()
                       .Contain( candidate => candidate.Key.Tonic == PitchClass.C
                                              && candidate.Key.ScaleDefinition.FormulaId == ScaleDefinition.Major.FormulaId
                       )
                       .Which;

    cMajor.SupportingEvidence.Should()
          .Contain( evidence => evidence.Category == EvidenceReasonCategory.ScaleContext
                                && evidence.Explanation.Contains( "pitch classes", StringComparison.OrdinalIgnoreCase )
          );

    cMajor.SupportingEvidence.Should()
          .Contain( evidence => evidence.Explanation.Contains( "scale degree", StringComparison.OrdinalIgnoreCase ) );
  }

  [Fact]
  public void Evaluate_ShouldIncludeConflictingScaleEvidence_WhenPitchClassIsOutsideCandidateScale()
  {
    var scope = new PartEventScope( new Part( [Pitch.Create( PitchClass.C, 4 ), Pitch.Create( PitchClass.CSharp, 4 )] ) );

    var result = (TonalAnalysisResultSet) new TonalEvaluator().Evaluate(
      scope,
      options: TonalEvaluationOptions.AllCandidates
    );

    var cMajor = result.Candidates.Should()
                       .Contain( candidate => candidate.Key.Tonic == PitchClass.C
                                              && candidate.Key.ScaleDefinition.FormulaId == ScaleDefinition.Major.FormulaId
                       )
                       .Which;

    cMajor.ConflictingEvidence.Should()
          .Contain( evidence => evidence.Category == EvidenceReasonCategory.ScaleContext
                                && evidence.Explanation.Contains( PitchClass.CSharp.ToString(), StringComparison.Ordinal )
          );
  }

  [Fact]
  public void CandidateSource_DefaultCandidates_NotEmpty()
  {
    var candidates = TonalCandidateSource.GetDefaultCandidates()
                                         .Take( 5 )
                                         .ToArray();

    candidates.Should()
              .NotBeEmpty();

    candidates.All( k => k is not null )
              .Should()
              .BeTrue();
  }

  [Fact]
  public void Evaluate_InvertedChord_ShouldExposeRootBassInversionAndFormulaEvidence()
  {
    var chord = PitchChord.Create( PitchClass.C[4], ChordFormula.Major, 1 );
    var scope = new PartEventScope( new Part( [Pitch.Create( PitchClass.C, 4 ), chord] ) );

    var result = (TonalAnalysisResultSet) new TonalEvaluator().Evaluate(
      scope,
      options: TonalEvaluationOptions.AllCandidates
    );
    var candidate = result.Candidates.First();

    candidate.SupportingEvidence.Should()
             .Contain( evidence => evidence.Category == EvidenceReasonCategory.HarmonicContext );

    candidate.SupportingEvidence.Select( evidence => evidence.Explanation )
             .Should()
             .Contain( explanation => explanation.Contains( "root", StringComparison.OrdinalIgnoreCase ) );

    candidate.SupportingEvidence.Select( evidence => evidence.Explanation )
             .Should()
             .Contain( explanation => explanation.Contains( "bass", StringComparison.OrdinalIgnoreCase ) );

    candidate.SupportingEvidence.Select( evidence => evidence.Explanation )
             .Should()
             .Contain( explanation => explanation.Contains( "formula", StringComparison.OrdinalIgnoreCase ) );

    candidate.SupportingEvidence.Select( evidence => evidence.Explanation )
             .Should()
             .Contain( explanation => explanation.Contains( "inversion", StringComparison.OrdinalIgnoreCase ) );
  }

  [Fact]
  public void Evaluate_ShouldIncludeConflictingHarmonicEvidence_WhenChordToneIsOutsideCandidateScale()
  {
    var chord = PitchChord.Create( PitchClass.FSharp[4], ChordFormula.Major );
    var scope = new PartEventScope( new Part( [Pitch.Create( PitchClass.C, 4 ), chord] ) );

    var result = (TonalAnalysisResultSet) new TonalEvaluator().Evaluate(
      scope,
      options: TonalEvaluationOptions.AllCandidates
    );

    var cMajor = result.Candidates.Should()
                       .Contain( candidate => candidate.Key.Tonic == PitchClass.C
                                              && candidate.Key.ScaleDefinition.FormulaId == ScaleDefinition.Major.FormulaId
                       )
                       .Which;

    cMajor.ConflictingEvidence.Should()
          .Contain( evidence => evidence.Category == EvidenceReasonCategory.HarmonicContext
                                && evidence.Explanation.Contains( "conflicts", StringComparison.OrdinalIgnoreCase )
          );
  }

  [Fact]
  public void RepertoireProfile_CommonPractice_EnablesCommonPracticeTags()
  {
    var profile = RepertoireProfile.CommonPractice;

    profile.GetWeight( ScaleTag.CommonPractice )
           .Should()
           .Be( 1.0 );

    profile.GetWeight( ScaleTag.Modal )
           .Should()
           .Be( 0.0 );
  }

  [Fact]
  public void Evaluate_ChordEvent_IncludesHarmonicEvidence()
  {
    var chord = PitchChord.Create( PitchClass.C, ChordFormula.Major );
    var scope = new PartEventScope( new Part( new IPartEvent[] { chord } ) );

    var result = new TonalEvaluator( RepertoireProfile.CommonPractice ).Evaluate(
      scope,
      options: TonalEvaluationOptions.AllCandidates
    );

    result.Should()
          .BeOfType<TonalAnalysisResultSet>();
    var candidates = (TonalAnalysisResultSet) result;

    candidates.Candidates.Should()
              .Contain( candidate =>
                          candidate.SupportingEvidence.Any( e => e.Category == EvidenceReasonCategory.HarmonicContext )
              );
  }

  [Fact]
  public void Evaluate_ChordEvent_KeepsConfidenceInRange()
  {
    var chord = PitchChord.Create( PitchClass.C, ChordFormula.Major, inversion: 1 );
    var scope = new PartEventScope( new Part( new IPartEvent[] { chord } ) );

    var result = new TonalEvaluator().Evaluate( scope );

    result.Should()
          .BeOfType<TonalAnalysisResultSet>();

    ( (TonalAnalysisResultSet) result ).Candidates.Should()
                                       .OnlyContain( candidate => candidate.Confidence >= 0.0 && candidate.Confidence <= 1.0
                                       );
  }

  [Fact]
  public void Evaluate_ShouldReturnDeterministicRanksAndConfidence()
  {
    var scope = new PartEventScope( Part.Parse( "C4,E4,G4" ) );
    var evaluator = new TonalEvaluator();

    var first = (TonalAnalysisResultSet) evaluator.Evaluate( scope );
    var second = (TonalAnalysisResultSet) evaluator.Evaluate( scope );

    first.Candidates.Select( candidate => ( candidate.Rank, candidate.Tonic, candidate.Key.ScaleDefinition.FormulaId,
                               candidate.Confidence )
         )
         .Should()
         .Equal(
           second.Candidates.Select( candidate => ( candidate.Rank, candidate.Tonic, candidate.Key.ScaleDefinition.FormulaId,
                                       candidate.Confidence )
           )
         );

    first.Candidates.Select( candidate => candidate.Rank )
         .Should()
         .Equal( Enumerable.Range( 1, first.Count ) );
  }

  [Fact]
  public void Evaluate_ShouldReturnSingleBestCandidate_ByDefault()
  {
    var scope = new PartEventScope( Part.Parse( "C4" ) );

    var result = new TonalEvaluator().Evaluate( scope );

    result.Should()
          .BeOfType<TonalAnalysisResultSet>();

    ( (TonalAnalysisResultSet) result ).Candidates.Should()
                                       .ContainSingle()
                                       .Which.Rank.Should()
                                       .Be( 1 );
  }

  [Fact]
  public void Evaluate_ShouldReturnMultipleCandidates_WhenAllCandidatesAreRequested()
  {
    var scope = new PartEventScope( Part.Parse( "C4" ) );

    var result = new TonalEvaluator().Evaluate( scope, options: TonalEvaluationOptions.AllCandidates );

    result.Should()
          .BeOfType<TonalAnalysisResultSet>();

    ( (TonalAnalysisResultSet) result ).Candidates.Should()
                                       .HaveCountGreaterThan( 1 );
  }

  [Fact]
  public void Evaluate_ShouldWidenResults_ByMaximumCandidatesAndConfidenceDelta()
  {
    var scope = new PartEventScope( Part.Parse( "C4" ) );

    var result = new TonalEvaluator().Evaluate(
      scope,
      options: new TonalEvaluationOptions
      {
        MaximumCandidates = 3,
        ConfidenceDelta = 0.1
      }
    );

    ( (TonalAnalysisResultSet) result ).Candidates.Should()
                                       .HaveCount( 3 );
  }

  #endregion

  #region Implementation

  private static string LoadFixture(
    string fixtureName )
  {
    var fixturePath = Path.Combine(
      AppContext.BaseDirectory,
      "Fixtures",
      fixtureName
    );

    return File.ReadAllText( fixturePath )
               .Trim();
  }

  #endregion
}
