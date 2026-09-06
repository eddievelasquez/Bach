// Module Name: RootlessJazzVoicingTests.cs
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

using System.Linq;

namespace Bach.Model.Analysis.Test;

public class RootlessJazzVoicingTests
{
  #region Public Methods

  [Fact]
  public void Ctor_Should_CreateRecord()
  {
    var observed = new[] { PitchClass.C, PitchClass.E, PitchClass.G }; // triad-like voicing
    var evidence = new EvidenceReason( EvidenceReasonCategory.HarmonicContext, "implied root C" );

    var v = new RootlessJazzVoicing( observed, null, PitchClass.C, 0.9, evidence );

    v.ObservedPitchClasses.Should()
     .BeEquivalentTo( observed );

    v.ImpliedRoot.Should()
     .Be( PitchClass.C );

    v.Confidence.Should()
     .Be( 0.9 );

    v.Evidence.Should()
     .Be( evidence );
  }

  [Fact]
  public void Ctor_Throws_OnEmptyObserved()
  {
    var evidence = new EvidenceReason( EvidenceReasonCategory.AnalystObservation, "x" );
    Action a = () => new RootlessJazzVoicing( Enumerable.Empty<PitchClass>(), null, PitchClass.C, 0.5, evidence );

    a.Should()
     .Throw<ArgumentException>();
  }

  [Fact]
  public void Ctor_Throws_OnConfidenceOutOfRange()
  {
    var observed = new[] { PitchClass.C };
    var evidence = new EvidenceReason( EvidenceReasonCategory.AnalystObservation, "x" );
    Action a = () => new RootlessJazzVoicing( observed, null, PitchClass.C, -0.1, evidence );

    a.Should()
     .Throw<ArgumentOutOfRangeException>();
  }

  #endregion
}
