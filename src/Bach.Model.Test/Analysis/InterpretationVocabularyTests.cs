// Module Name: InterpretationVocabularyTests.cs
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

using Bach.Model.Analysis;

namespace Bach.Model.Test.Analysis;

public sealed class InterpretationVocabularyTests
{
  #region Public Methods

  [Fact]
  public void EvidenceReason_ShouldStoreCategoryAndExplanation()
  {
    var reason = new EvidenceReason( EvidenceReasonCategory.MelodicMotion, "The tone moves by step between chord tones." );

    reason.Category.Should()
          .Be( EvidenceReasonCategory.MelodicMotion );

    reason.Explanation.Should()
          .Be( "The tone moves by step between chord tones." );

    reason.Should()
          .Be( new EvidenceReason( EvidenceReasonCategory.MelodicMotion, "The tone moves by step between chord tones." ) );
  }

  [Fact]
  public void EvidenceReason_ShouldRejectEmptyExplanation()
  {
    var act = () => new EvidenceReason( EvidenceReasonCategory.AnalystObservation, " " );

    act.Should()
       .Throw<ArgumentException>();
  }

  [Fact]
  public void AnalysisTarget_ShouldReferencePartEventOrPitch()
  {
    var chord = PitchChord.Create( PitchClass.C, ChordFormula.Major );
    var eventTarget = AnalysisTarget.ForEvent( chord );
    var pitch = PitchClass.E[4];
    var pitchTarget = AnalysisTarget.ForPitch( pitch );

    eventTarget.PartEvent.Should()
               .BeSameAs( chord );

    eventTarget.Pitch.Should()
               .BeNull();

    pitchTarget.PartEvent.Should()
               .Be( pitch );

    pitchTarget.Pitch.Should()
               .Be( pitch );
  }

  [Fact]
  public void AnalysisTarget_ShouldRejectInvalidPitch()
  {
    var act = () => AnalysisTarget.ForPitch( Pitch.Empty );

    act.Should()
       .Throw<ArgumentException>();
  }

  [Fact]
  public void NonChordToneTypes_ShouldStoreTheirMusicalRelationships()
  {
    var target = AnalysisTarget.ForPitch( PitchClass.F[4] );
    var preparation = AnalysisTarget.ForPitch( PitchClass.F[4] );
    var resolution = AnalysisTarget.ForPitch( PitchClass.E[4] );
    var anticipatedTarget = AnalysisTarget.ForPitch( PitchClass.F[4] );
    var evidence = new EvidenceReason( EvidenceReasonCategory.Resolution, "The tone resolves by step." );

    var passing = new PassingTone( target, MelodicDirection.Ascending, evidence );
    var neighbor = new NeighborTone( target, NeighborToneDirection.Upper, evidence );
    var suspension = new Suspension( target, preparation, resolution, evidence );
    var anticipation = new Anticipation( target, anticipatedTarget, evidence );
    var pedal = new PedalTone( target, PitchClass.F, evidence );

    passing.Direction.Should()
           .Be( MelodicDirection.Ascending );

    neighbor.Direction.Should()
            .Be( NeighborToneDirection.Upper );

    suspension.Preparation.Should()
              .Be( preparation );

    suspension.Resolution.Should()
              .Be( resolution );

    anticipation.AnticipatedTarget.Should()
                .Be( anticipatedTarget );

    pedal.PedalPitch.Should()
         .Be( PitchClass.F );

    passing.Should()
           .BeAssignableTo<NonChordTone>();

    neighbor.Should()
            .BeAssignableTo<NonChordTone>();
  }

  [Fact]
  public void AlteredDegree_ShouldPreserveSpelledAlterationAndKind()
  {
    var target = AnalysisTarget.ForPitch( PitchClass.B[4] );
    var alteration = new DegreeAlteration( ScaleDegree.LeadingTone, Interval.MajorSeventh );

    var evidence = new EvidenceReason(
      EvidenceReasonCategory.ScaleContext,
      "The leading tone is raised in harmonic minor."
    );

    var alteredDegree = new AlteredDegree( target, alteration, AlteredDegreeKind.Chromatic, evidence );

    alteredDegree.Alteration.Should()
                 .Be( alteration );

    alteredDegree.Alteration.Degree.Should()
                 .Be( ScaleDegree.LeadingTone );

    alteredDegree.Alteration.AlteredInterval.Should()
                 .Be( Interval.MajorSeventh );

    alteredDegree.Kind.Should()
                 .Be( AlteredDegreeKind.Chromatic );

    alteredDegree.Evidence.Should()
                 .Be( evidence );
  }

  [Fact]
  public void AppliedFunction_ShouldFormatTypedFunctionAsConventionalLabel()
  {
    var target = AnalysisTarget.ForEvent( PitchChord.Create( PitchClass.D, ChordFormula.Major ) );
    var evidence = new EvidenceReason( EvidenceReasonCategory.HarmonicContext, "The chord resolves to the dominant." );

    var dominant = new AppliedFunction( target, ScaleDegree.Dominant, AppliedTriadFunction.Dominant, evidence );
    var leadingTone = new AppliedFunction( target, ScaleDegree.Dominant, AppliedTriadFunction.LeadingTone, evidence );

    dominant.Label.Should()
            .Be( "V/V" );

    dominant.ToString()
            .Should()
            .Be( "V/V" );

    leadingTone.Label.Should()
               .Be( "vii°/V" );

    dominant.TargetDegree.Should()
            .Be( ScaleDegree.Dominant );
  }

  #endregion
}
