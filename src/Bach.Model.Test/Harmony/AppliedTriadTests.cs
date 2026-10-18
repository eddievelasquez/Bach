// Module Name: AppliedTriadTests.cs
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

namespace Bach.Model.Harmony.Test;

public sealed class AppliedTriadTests
{
  #region Public Methods

  [Fact]
  public void Ctor_ShouldPreserveValues_WhenTargetIsTonic()
  {
    var triad = new Triad( PitchClass.C, TriadQuality.Major );
    var appliedTriad = new AppliedTriad( triad, ScaleDegree.Tonic, AppliedTriadFunction.Dominant );

    appliedTriad.Triad.Should()
                .BeSameAs( triad );

    appliedTriad.TargetDegree.Should()
                .Be( ScaleDegree.Tonic );

    appliedTriad.Function.Should()
                .Be( AppliedTriadFunction.Dominant );
  }

  [Fact]
  public void Ctor_ShouldAcceptLeadingToneTarget_WhenDegreeIsSeven()
  {
    var appliedTriad = new AppliedTriad(
      new Triad( PitchClass.B, TriadQuality.Diminished ),
      ScaleDegree.LeadingTone,
      AppliedTriadFunction.LeadingTone
    );

    appliedTriad.TargetDegree.Should()
                .Be( ScaleDegree.LeadingTone );

    appliedTriad.Function.Should()
                .Be( AppliedTriadFunction.LeadingTone );
  }

  [Fact]
  public void Ctor_ShouldThrowArgumentOutOfRangeException_WhenTargetDegreeIsDefault()
  {
    Action action = () => new AppliedTriad(
      new Triad( PitchClass.C, TriadQuality.Major ),
      default,
      AppliedTriadFunction.Dominant
    );

    action.Should()
          .Throw<ArgumentOutOfRangeException>();
  }

  [Fact]
  public void Ctor_ShouldThrowArgumentNullException_WhenTriadIsNull()
  {
    Action action = () => new AppliedTriad( null!, ScaleDegree.Tonic, AppliedTriadFunction.Dominant );

    action.Should()
          .Throw<ArgumentNullException>();
  }

  #endregion
}
