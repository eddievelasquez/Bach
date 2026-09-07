// Module Name: TonalAnalysisResultTests.cs
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

namespace Bach.Model.Analysis.Test;

public class TonalAnalysisResultTests
{
  #region Public Methods

  [Fact]
  public void RankedCtor_Should_CreateRecord()
  {
    var pitch = Pitch.Create( PitchClass.C, 4 );
    var part = new Part( new IPartEvent[] { pitch } );
    var scope = new PartEventScope( part );
    var key = new Key( PitchClass.C, ScaleDefinition.Major );
    var supporting = new[] { new EvidenceReason( EvidenceReasonCategory.ScaleContext, "matches scale degrees" ) };
    var conflicting = new[] { new EvidenceReason( EvidenceReasonCategory.HarmonicContext, "contradicting chord" ) };

    var record = new RankedTonalCandidateResult(
      1,
      PitchClass.C,
      key,
      0.85,
      supporting,
      conflicting,
      scope
    );

    record.Scope.Should()
          .Be( scope );

    record.Scope.Events.Should()
          .ContainSingle()
          .Which.Should()
          .Be( pitch );

    record.Rank.Should()
          .Be( 1 );

    record.Tonic.Should()
          .Be( PitchClass.C );

    record.Key.Should()
          .Be( key );

    record.Confidence.Should()
          .BeApproximately( 0.85, 1e-12 );

    record.SupportingEvidence.Should()
          .ContainSingle()
          .Which.Explanation.Should()
          .Be( "matches scale degrees" );
  }

  [Fact]
  public void RankedCtor_Validates_Arguments()
  {
    var pitch = Pitch.Create( PitchClass.C, 4 );
    var scope = new PartEventScope( new Part( new IPartEvent[] { pitch } ) );
    var key = new Key( PitchClass.C, ScaleDefinition.Major );

    Action a1 = () => new RankedTonalCandidateResult(
      0,
      PitchClass.C,
      key,
      0.5,
      null,
      null,
      scope
    );

    a1.Should()
      .Throw<ArgumentOutOfRangeException>();

    Action a2 = () => new RankedTonalCandidateResult(
      1,
      PitchClass.C,
      key,
      -0.1,
      null,
      null,
      scope
    );

    a2.Should()
      .Throw<ArgumentOutOfRangeException>();

    Action a3 = () => new RankedTonalCandidateResult(
      1,
      PitchClass.D,
      key,
      0.5,
      null,
      null,
      scope
    );

    a3.Should()
      .Throw<ArgumentException>();
  }

  [Fact]
  public void InconclusiveCtor_Should_CreateRecord()
  {
    var pitch = Pitch.Create( PitchClass.C, 4 );
    var scope = new PartEventScope( new Part( new IPartEvent[] { pitch } ) );
    var conflicting = new[] { new EvidenceReason( EvidenceReasonCategory.AnalystObservation, "ambiguous set" ) };

    var record = new InconclusiveTonalAnalysisResult( scope, "insufficient evidence", conflicting );

    record.Scope.Should()
          .Be( scope );

    record.Reason.Should()
          .Be( "insufficient evidence" );

    record.ConflictingEvidence.Should()
          .ContainSingle()
          .Which.Explanation.Should()
          .Be( "ambiguous set" );
  }

  #endregion
}
