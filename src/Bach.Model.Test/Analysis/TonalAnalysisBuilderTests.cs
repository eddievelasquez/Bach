// Module Name: TonalAnalysisBuilderTests.cs
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

public class TonalAnalysisBuilderTests
{
  #region Public Methods

  [Fact]
  public void Builder_Should_BuildRankedCandidate()
  {
    var pitch = Pitch.Create( PitchClass.C, 4 );
    var part = new Part( new IPartEvent[] { pitch } );
    var key = new Key( PitchClass.C, ScaleDefinition.Major );
    var evidence = new EvidenceReason( EvidenceReasonCategory.ScaleContext, "matches C major" );

    var result = new TonalAnalysisResultBuilder( part, .. )
                 .For( PitchClass.C, key )
                 .WithRank( 1 )
                 .WithConfidence( 0.92 )
                 .Supporting( evidence )
                 .Build();

    result.Should()
          .BeOfType<RankedTonalCandidateResult>();
    var candidate = (RankedTonalCandidateResult) result;

    candidate.Tonic.Should()
             .Be( PitchClass.C );

    candidate.Confidence.Should()
             .BeApproximately( 0.92, 1e-12 );

    ReferenceEquals( candidate.Scope.Source, part )
      .Should()
      .BeTrue();

    candidate.Scope.Range.Should()
             .Be( .. );

    candidate.SupportingEvidence.Should()
             .ContainSingle()
             .Which.Should()
             .Be( evidence );
  }

  [Fact]
  public void Builder_Should_BuildInconclusiveResult()
  {
    var pitch = Pitch.Create( PitchClass.C, 4 );
    var part = new Part( new IPartEvent[] { pitch } );

    var result = new TonalAnalysisResultBuilder( part, ..1 )
                 .Inconclusive( "The evidence is ambiguous." )
                 .Build();

    result.Should()
          .BeOfType<InconclusiveTonalAnalysisResult>();

    ( (InconclusiveTonalAnalysisResult) result ).Reason.Should()
                                                .Be( "The evidence is ambiguous." );
  }

  [Fact]
  public void ResultSetBuilder_Should_AddCandidatesThroughCallback()
  {
    var pitch = Pitch.Create( PitchClass.C, 4 );
    var part = new Part( new IPartEvent[] { pitch } );
    var key = new Key( PitchClass.C, ScaleDefinition.Major );

    var result = new TonalAnalysisResultSetBuilder( part, .. )
                 .AddCandidate( candidate => candidate
                                             .For( PitchClass.C, key )
                                             .WithRank( 1 )
                                             .WithConfidence( 0.9 )
                 )
                 .Build();

    result.Count.Should()
          .Be( 1 );

    result.Candidates[0]
          .Key.Should()
          .Be( key );
  }

  [Fact]
  public void ResultSetBuilder_Should_RejectUnorderedRanks()
  {
    var pitch = Pitch.Create( PitchClass.C, 4 );
    var part = new Part( new IPartEvent[] { pitch } );
    var key = new Key( PitchClass.C, ScaleDefinition.Major );

    var builder = new TonalAnalysisResultSetBuilder( part, .. )
      .AddCandidate( candidate => candidate
                                  .For( PitchClass.C, key )
                                  .WithRank( 2 )
                                  .WithConfidence( 0.9 )
      );

    Action action = () => builder.AddCandidate( candidate => candidate
                                                             .For( PitchClass.C, key )
                                                             .WithRank( 1 )
                                                             .WithConfidence( 0.8 )
    );

    action.Should()
          .Throw<InvalidOperationException>();
  }

  [Fact]
  public void ResultSetBuilder_Should_BuildInconclusiveResult()
  {
    var pitch = Pitch.Create( PitchClass.C, 4 );
    var part = new Part( new IPartEvent[] { pitch } );

    var result = new TonalAnalysisResultSetBuilder( part, .. )
      .BuildInconclusive( "No candidate reached the threshold." );

    result.Reason.Should()
          .Be( "No candidate reached the threshold." );
  }

  [Fact]
  public void Builders_Should_RejectNullCallbacks()
  {
    var pitch = Pitch.Create( PitchClass.C, 4 );
    var part = new Part( new IPartEvent[] { pitch } );
    var builder = new TonalAnalysisResultSetBuilder( part, .. );

    Action action = () => builder.AddCandidate( (Action<TonalAnalysisResultBuilder>) null! );

    action.Should()
          .Throw<ArgumentNullException>();
  }

  [Fact]
  public void ResultSet_Should_AddExistingCandidate()
  {
    var pitch = Pitch.Create( PitchClass.C, 4 );
    var part = new Part( new IPartEvent[] { pitch } );
    var scope = new PartEventScope( part );
    var key = new Key( PitchClass.C, ScaleDefinition.Major );

    var candidate = new RankedTonalCandidateResult(
      1,
      PitchClass.C,
      key,
      0.9,
      null,
      null,
      scope
    );

    var result = new TonalAnalysisResultSetBuilder( scope )
                 .AddCandidate( candidate )
                 .Build();

    result.Candidates.Should()
          .ContainSingle()
          .Which.Should()
          .Be( candidate );
  }

  #endregion
}
