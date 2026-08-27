// Module Name: ChordProgressionTest.cs
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

using System.Globalization;
using System.Linq;

namespace Bach.Model.Test;

public sealed class ChordProgressionTests
{
  #region Public Methods

  [Fact]
  public void Constructor_ShouldCreateIndependentCopy_WhenScaleDegreesProvided()
  {
    var scaleDegrees = new[] { ScaleDegree.Tonic, ScaleDegree.Dominant };
    var progression = new ChordProgression( scaleDegrees );

    progression.ScaleDegrees.Should()
               .NotBeSameAs( scaleDegrees );
  }

  [Fact]
  public void Constructor_ShouldInitializeScaleDegrees_WhenValidListProvided()
  {
    var scaleDegrees = new[] { ScaleDegree.Tonic, ScaleDegree.Dominant, ScaleDegree.Subdominant, ScaleDegree.Tonic };

    var progression = new ChordProgression( scaleDegrees );

    progression.ScaleDegrees.Should()
               .NotBeNull()
               .And.HaveCount( 4 )
               .And.ContainInOrder( scaleDegrees );
  }

  [Fact]
  public void Constructor_ShouldThrowArgumentException_WhenScaleDegreesIsEmpty()
  {
    var act = () => new ChordProgression();

    act.Should()
       .Throw<ArgumentException>();
  }

  [Fact]
  public void Constructor_ShouldThrowArgumentException_WhenScaleDegreesIsNull()
  {
    var act = () => new ChordProgression( null! );

    act.Should()
       .Throw<ArgumentException>();
  }

  [Theory]
  [InlineData( "1-4-5", new[] { 1, 4, 5 } )]
  [InlineData( "7 6 5", new[] { 7, 6, 5 } )]
  [InlineData( "2-3-4-5", new[] { 2, 3, 4, 5 } )]
  public void Parse_ShouldReturnChordProgression_WhenNashvilleStringIsProvided(
    string input,
    int[] expectedDegrees )
  {
    var actual = ChordProgression.Parse( input );

    actual.ScaleDegrees.Should()
          .Equal( expectedDegrees.Select( degree => ScaleDegree.ScaleDegrees[degree - 1] ) );
  }

  [Theory]
  [InlineData( "I IV V", new[] { 1, 4, 5 } )]
  [InlineData( "ii-V-I", new[] { 2, 5, 1 } )]
  [InlineData( "III VI VII", new[] { 3, 6, 7 } )]
  public void Parse_ShouldReturnChordProgression_WhenRomanNumeralStringIsProvided(
    string input,
    int[] expectedDegrees )
  {
    var actual = ChordProgression.Parse( input );

    actual.ScaleDegrees.Should()
          .Equal( expectedDegrees.Select( degree => ScaleDegree.ScaleDegrees[degree - 1] ) );
  }

  [Theory]
  [InlineData( "I-4-V" )]
  [InlineData( "1 IV 5" )]
  [InlineData( "I 4" )]
  [InlineData( "1-ii" )]
  public void Parse_ShouldThrowFormatException_WhenMixedSystemsOrInvalidTokensAreProvided(
    string input )
  {
    var act = () => ChordProgression.Parse( input );

    act.Should()
       .Throw<FormatException>();
  }

  [Fact]
  public void Parse_WithFormatProvider_ShouldReturnChordProgression_WhenRomanNumeralStringIsProvided()
  {
    var actual = ChordProgression.Parse( "I IV V", CultureInfo.InvariantCulture );

    actual.ScaleDegrees.Should()
          .Equal( ScaleDegree.Tonic, ScaleDegree.Subdominant, ScaleDegree.Dominant );
  }

  [Fact]
  public void ToStringWithFormatAndProvider_ShouldReturnNumericRepresentation_WhenFormatIsN()
  {
    var scaleDegrees = new[] { ScaleDegree.Tonic, ScaleDegree.Subdominant, ScaleDegree.Dominant, ScaleDegree.Tonic };
    var progression = new ChordProgression( scaleDegrees );

    var result = progression.ToString( "N", null );

    result.Should()
          .Be( "1-4-5-1" );
  }

  [Fact]
  public void ToStringWithFormatAndProvider_ShouldReturnSymbolRepresentation_WhenFormatIsG()
  {
    var scaleDegrees = new[] { ScaleDegree.Tonic, ScaleDegree.Dominant, ScaleDegree.Tonic };
    var progression = new ChordProgression( scaleDegrees );

    var result = progression.ToString( "G", null );

    result.Should()
          .Be( "I-V-I" );
  }

  [Fact]
  public void ToStringWithFormatAndProvider_ShouldReturnSymbolRepresentation_WhenFormatIsNull()
  {
    var scaleDegrees = new[] { ScaleDegree.Mediant, ScaleDegree.Submediant, ScaleDegree.Supertonic, ScaleDegree.Dominant };
    var progression = new ChordProgression( scaleDegrees );

    var result = progression.ToString( null, null );

    result.Should()
          .Be( "iii-vi-ii-V" );
  }

  [Fact]
  public void ToStringWithFormatAndProvider_ShouldReturnSymbolRepresentation_WhenFormatIsR()
  {
    var scaleDegrees = new[] { ScaleDegree.Tonic, ScaleDegree.Subdominant, ScaleDegree.Dominant, ScaleDegree.Tonic };
    var progression = new ChordProgression( scaleDegrees );

    var result = progression.ToString( "R", null );

    result.Should()
          .Be( "I-IV-V-I" );
  }

  [Fact]
  public void ToStringWithFormatAndProvider_ShouldThrowFormatException_WhenFormatIsInvalid()
  {
    var scaleDegrees = new[] { ScaleDegree.Tonic, ScaleDegree.Dominant };
    var progression = new ChordProgression( scaleDegrees );

    var act = () => progression.ToString( "X", null );

    act.Should()
       .Throw<FormatException>()
       .WithMessage( "The format string 'X' is not supported." );
  }

  [Fact]
  public void ToStringWithFormatProvider_ShouldReturnSymbolRepresentation_WhenFormatProviderProvided()
  {
    var scaleDegrees = new[] { ScaleDegree.Tonic, ScaleDegree.Subdominant, ScaleDegree.Dominant };
    var progression = new ChordProgression( scaleDegrees );

    var result = progression.ToString( CultureInfo.InvariantCulture );

    result.Should()
          .Be( "I-IV-V" );
  }

  [Fact]
  public void ToStringWithFormat_ShouldReturnSymbolRepresentation_WhenFormatIsNull()
  {
    var scaleDegrees = new[] { ScaleDegree.Supertonic, ScaleDegree.Dominant, ScaleDegree.Tonic };
    var progression = new ChordProgression( scaleDegrees );

    var result = progression.ToString( (string?) null );

    result.Should()
          .Be( "ii-V-I" );
  }

  [Fact]
  public void ToString_ShouldReturnSymbolRepresentation_WhenNoParametersProvided()
  {
    var scaleDegrees = new[] { ScaleDegree.Tonic, ScaleDegree.Subdominant, ScaleDegree.Dominant, ScaleDegree.Tonic };
    var progression = new ChordProgression( scaleDegrees );

    var result = progression.ToString();

    result.Should()
          .Be( "I-IV-V-I" );
  }

  [Theory]
  [InlineData( "I 4", false )]
  [InlineData( "1-4-5", true )]
  [InlineData( "I IV V", true )]
  [InlineData( "0-1-2", false )]
  [InlineData( "I-IX", false )]
  [InlineData( "1-2-3-4-5-6-7", true )]
  public void TryParse_ShouldReturnExpectedResult_WhenInputIsProvided(
    string input,
    bool expected )
  {
    ChordProgression.TryParse( input, out var actual )
                    .Should()
                    .Be( expected );

    if( expected )
    {
      actual.Should()
            .NotBeNull();
    }
  }

  [Theory]
  [InlineData( null )]
  [InlineData( "" )]
  [InlineData( "   " )]
  [InlineData( "\t" )]
  public void TryParse_ShouldReturnFalse_WhenInputIsNullOrWhitespace(
    string? input )
  {
    ChordProgression.TryParse( input, out _ )
                    .Should()
                    .BeFalse();
  }

  [Fact]
  public void TryParse_WithFormatProvider_ShouldReturnTrue_WhenNashvilleStringIsProvided()
  {
    ChordProgression.TryParse( "1-4-5", CultureInfo.InvariantCulture, out var actual )
                    .Should()
                    .BeTrue();

    actual.Should()
          .NotBeNull();
  }

  #endregion
}
