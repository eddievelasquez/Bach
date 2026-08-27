// Module Name: ScaleDegreeTest.cs
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

namespace Bach.Model.Test;

public sealed class ScaleDegreeTests
{
  #region Public Methods

  [Fact]
  public void Parse_ShouldThrowArgumentException_WhenValueIsNullOrWhitespace()
  {
    var act = () => ScaleDegree.Parse( "   " );

    act.Should()
       .Throw<ArgumentException>();
  }

  [Fact]
  public void Parse_ShouldThrowFormatException_WhenValueIsInvalid()
  {
    var act = () => ScaleDegree.Parse( "X" );

    act.Should()
       .Throw<FormatException>()
       .WithMessage( "The value 'X' is not a valid scale degree." );
  }

  [Fact]
  public void ResolveDiatonicTriad_ShouldReturnExpectedQuality_ForMajorMode()
  {
    var degree = ScaleDegree.Tonic;
    var key = new Key( PitchClass.C, ModeType.Major );

    var triad = degree.ResolveDiatonicTriad( key );

    triad.Root.Should()
         .Be( PitchClass.C );

    triad.Quality.Should()
         .Be( TriadQuality.Major );
  }

  [Fact]
  public void Resolve_ShouldReturnExpectedPitchClass_ForMinorKey()
  {
    var key = new Key( PitchClass.A, ModeType.Minor );
    var degree = ScaleDegree.Mediant;

    degree.Resolve( key )
          .Should()
          .Be( PitchClass.C );
  }

  [Fact]
  public void Resolve_ShouldReturnTonicPitchClass_ForMajorKey()
  {
    var key = new Key( PitchClass.C, ModeType.Major );
    var degree = ScaleDegree.Tonic;

    degree.Resolve( key )
          .Should()
          .Be( PitchClass.C );
  }

  [Fact]
  public void Resolve_ShouldThrowArgumentNullException_WhenKeyIsNull()
  {
    var degree = ScaleDegree.Dominant;

    var act = () => degree.Resolve( null! );

    act.Should()
       .Throw<ArgumentNullException>();
  }

  [Fact]
  public void ToString_ShouldReturnRomanNumeralSymbol()
  {
    ScaleDegree.Tonic.ToString()
               .Should()
               .Be( "I" );

    ScaleDegree.Dominant.ToString()
               .Should()
               .Be( "V" );
  }

  [Fact]
  public void TryParse_ShouldReturnExpectedValue_ForNashvilleAndRomanNumeralInput()
  {
    ScaleDegree.TryParse( "5", out var nashville )
               .Should()
               .BeTrue();

    nashville.Should()
             .Be( ScaleDegree.Dominant );

    ScaleDegree.TryParse( "vi", out var roman )
               .Should()
               .BeTrue();

    roman.Should()
         .Be( ScaleDegree.Submediant );
  }

  #endregion
}
