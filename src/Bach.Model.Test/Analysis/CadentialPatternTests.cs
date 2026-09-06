// Module Name: CadentialPatternTests.cs
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

public class CadentialPatternTests
{
  #region Public Methods

  [Fact]
  public void Ctor_Should_CreateRecord()
  {
    var target = AnalysisTarget.ForPitch( Pitch.Create( PitchClass.C, 4 ) );
    var evidence = new EvidenceReason( EvidenceReasonCategory.Resolution, "descending step to tonic" );

    var record = new CadentialPattern( target, InterpretationKinds.CadenceKind.Authentic, evidence );

    record.Target.Should()
          .Be( target );

    record.Kind.Should()
          .Be( InterpretationKinds.CadenceKind.Authentic );

    record.Evidence.Should()
          .Be( evidence );
  }

  [Fact]
  public void Ctor_Throws_OnNulls()
  {
    var evidence = new EvidenceReason( EvidenceReasonCategory.AnalystObservation, "x" );
    Action a = () => new CadentialPattern( null!, InterpretationKinds.CadenceKind.Half, evidence );

    a.Should()
     .Throw<ArgumentNullException>();
  }

  #endregion
}
