// Module Name: IntervalQualityTest.cs
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

public sealed class IntervalQualityTests
{
  #region Properties

  public static TheoryData<IntervalQuantity, IntervalQuality> InvalidIntervalCombinations =>
    IntervalTests.InvalidIntervalCombinations;

  public static TheoryData<IntervalQuality, int, IntervalQuality> ValidAdditionData { get; } = new()
  {
    { IntervalQuality.Diminished, 0, IntervalQuality.Diminished },
    { IntervalQuality.Diminished, 1, IntervalQuality.Minor },
    { IntervalQuality.Diminished, 2, IntervalQuality.Perfect },
    { IntervalQuality.Diminished, 3, IntervalQuality.Major },
    { IntervalQuality.Diminished, 4, IntervalQuality.Augmented },
    { IntervalQuality.Minor, 1, IntervalQuality.Perfect },
    { IntervalQuality.Minor, 2, IntervalQuality.Major },
    { IntervalQuality.Minor, 3, IntervalQuality.Augmented },
    { IntervalQuality.Perfect, 1, IntervalQuality.Major },
    { IntervalQuality.Perfect, 2, IntervalQuality.Augmented },
    { IntervalQuality.Major, 1, IntervalQuality.Augmented }
  };

  public static TheoryData<IntervalQuality, int> InvalidAdditionData { get; } = new()
  {
    { IntervalQuality.Diminished, 5 },
    { IntervalQuality.Minor, 4 },
    { IntervalQuality.Perfect, 3 },
    { IntervalQuality.Major, 2 },
    { IntervalQuality.Augmented, 1 },
    { IntervalQuality.Diminished, -1 },
    { IntervalQuality.Minor, -2 },
    { IntervalQuality.Perfect, -3 },
    { IntervalQuality.Major, -4 },
    { IntervalQuality.Augmented, -5 }
  };

  public static TheoryData<IntervalQuality, int, IntervalQuality> ValidSubtractionData { get; } = new()
  {
    { IntervalQuality.Augmented, 0, IntervalQuality.Augmented },
    { IntervalQuality.Augmented, 1, IntervalQuality.Major },
    { IntervalQuality.Augmented, 2, IntervalQuality.Perfect },
    { IntervalQuality.Augmented, 3, IntervalQuality.Minor },
    { IntervalQuality.Augmented, 4, IntervalQuality.Diminished },
    { IntervalQuality.Major, 1, IntervalQuality.Perfect },
    { IntervalQuality.Major, 2, IntervalQuality.Minor },
    { IntervalQuality.Major, 3, IntervalQuality.Diminished },
    { IntervalQuality.Perfect, 1, IntervalQuality.Minor },
    { IntervalQuality.Perfect, 2, IntervalQuality.Diminished },
    { IntervalQuality.Minor, 1, IntervalQuality.Diminished }
  };

  public static TheoryData<IntervalQuality, int> InvalidSubtractionData { get; } = new()
  {
    { IntervalQuality.Augmented, 5 },
    { IntervalQuality.Major, 4 },
    { IntervalQuality.Perfect, 3 },
    { IntervalQuality.Minor, 2 },
    { IntervalQuality.Diminished, 1 }
  };

  public static TheoryData<IntervalQuality, string> LongNameData { get; } = new()
  {
    { IntervalQuality.Diminished, "Diminished" },
    { IntervalQuality.Minor, "Minor" },
    { IntervalQuality.Perfect, "Perfect" },
    { IntervalQuality.Major, "Major" },
    { IntervalQuality.Augmented, "Augmented" }
  };

  public static TheoryData<IntervalQuality, string> ShortNameData { get; } = new()
  {
    { IntervalQuality.Diminished, "dim" },
    { IntervalQuality.Minor, "min" },
    { IntervalQuality.Perfect, "Perf" },
    { IntervalQuality.Major, "Maj" },
    { IntervalQuality.Augmented, "Aug" }
  };

  public static TheoryData<IntervalQuality, string> ValidQualityNames { get; } = new()
  {
    { IntervalQuality.Diminished, "Diminished" },
    { IntervalQuality.Minor, "Minor" },
    { IntervalQuality.Perfect, "Perfect" },
    { IntervalQuality.Major, "Major" },
    { IntervalQuality.Augmented, "Augmented" }
  };

  public static TheoryData<string, IntervalQuality> ValidQualityStrings { get; } = new()
  {
    { "d", IntervalQuality.Diminished },
    { "m", IntervalQuality.Minor },
    { "P", IntervalQuality.Perfect },
    { "M", IntervalQuality.Major },
    { "A", IntervalQuality.Augmented }
  };

  public static TheoryData<IntervalQuality, string> ClassicalSymbolData => new()
  {
    { IntervalQuality.Diminished, "d" },
    { IntervalQuality.Minor, "m" },
    { IntervalQuality.Perfect, "P" },
    { IntervalQuality.Major, "M" },
    { IntervalQuality.Augmented, "A" }
  };

  public static TheoryData<IntervalQuality, string> ModernSymbolData => new()
  {
    { IntervalQuality.Diminished, "°" },
    { IntervalQuality.Minor, "m" },
    { IntervalQuality.Perfect, "P" },
    { IntervalQuality.Major, "M" },
    { IntervalQuality.Augmented, "+" }
  };

  public static TheoryData<IntervalQuality, int> AddOutOfRangeData => new()
  {
    { IntervalQuality.Augmented, 1 },
    { IntervalQuality.Diminished, -1 }
  };

  #endregion

  #region Public Methods

  [Theory]
  [MemberData( nameof( AddOutOfRangeData ) )]
  public void Add_ResultOutOfRange_ThrowsArgumentOutOfRange(
    IntervalQuality quality,
    int semitones )
  {
    // Act
    Action act = () => quality.Add( semitones );

    // Assert
    act.Should()
       .Throw<ArgumentOutOfRangeException>();
  }

  [Theory]
  [MemberData( nameof( ValidAdditionData ) )]
  public void Add_ShouldSucceed(
    IntervalQuality quality,
    int increment,
    IntervalQuality expectedQuality )
  {
    quality.Add( increment )
           .Should()
           .Be( expectedQuality );
  }

  [Theory]
  [MemberData( nameof( InvalidAdditionData ) )]
  public void Add_ShouldThrowArgumentOutOfRange(
    IntervalQuality quality,
    int increment )
  {
    var act = () => quality.Add( increment );

    act.Should()
       .Throw<ArgumentOutOfRangeException>();
  }

  [Fact]
  public void Add_WithNegativeSemitones_ReturnsExpectedQuality()
  {
    // Arrange
    var start = IntervalQuality.Major; // value = 3

    // Act
    var result = start.Add( -2 ); // 3 - 2 = 1 -> Minor

    // Assert
    result.Should()
          .Be( IntervalQuality.Minor );
  }

  [Fact]
  public void Add_WithPositiveSemitones_ReturnsExpectedQuality()
  {
    // Arrange
    var start = IntervalQuality.Minor; // value = 1

    // Act
    var result = start.Add( 2 ); // 1 + 2 = 3 -> Major

    // Assert
    result.Should()
          .Be( IntervalQuality.Major );
  }

  [Theory]
  [MemberData( nameof( InvalidIntervalCombinations ) )]
  public void IsValidFor_ShouldReturnFalse_WhenInvalidIntervalCombinationOccurs(
    IntervalQuantity quantity,
    IntervalQuality quality )
  {
    quality.IsValidFor( quantity )
           .Should()
           .BeFalse();
  }

  [Theory]
  [MemberData( nameof( LongNameData ) )]
  public void LongName_ShouldReturnName(
    IntervalQuality quality,
    string expectedName )
  {
    quality.LongName.Should()
           .Be( expectedName );
  }

  [Theory]
  [MemberData( nameof( ValidQualityStrings ) )]
  public void Parse_ShouldSucceed_WhenValueIsValid(
    string input,
    IntervalQuality expected )
  {
    IntervalQuality.Parse( input )
                   .Should()
                   .Be( expected );
  }

  [Fact]
  public void Parse_ShouldThrowFormatException_WhenValueIsInvalid()
  {
    var act = () => IntervalQuality.Parse( "X" );

    act.Should()
       .Throw<FormatException>();
  }

  [Theory]
  [MemberData( nameof( ShortNameData ) )]
  public void ShortName_ShouldReturnName(
    IntervalQuality quality,
    string expectedShortName )
  {
    quality.ShortName.Should()
           .Be( expectedShortName );
  }

  [Theory]
  [MemberData( nameof( ValidSubtractionData ) )]
  public void Subtract_ShouldSucceed(
    IntervalQuality quality,
    int decrement,
    IntervalQuality expectedQuality )
  {
    quality.Subtract( decrement )
           .Should()
           .Be( expectedQuality );
  }

  [Theory]
  [MemberData( nameof( InvalidSubtractionData ) )]
  public void Subtract_ShouldThrowArgumentOutOfRange(
    IntervalQuality quality,
    int decrement )
  {
    var act = () => quality.Subtract( decrement );

    act.Should()
       .Throw<ArgumentOutOfRangeException>();
  }

  [Theory]
  [MemberData( nameof( ClassicalSymbolData ) )]
  public void ClassicalSymbol_QualityProvided_ReturnsExpected(
    IntervalQuality quality,
    string expected )
  {
    // Arrange & Act
    var result = quality.ClassicalSymbol;

    // Assert
    result.Should()
          .Be( expected );
  }

  [Theory]
  [MemberData( nameof( ModernSymbolData ) )]
  public void ModernSymbol_QualityProvided_ReturnsExpected(
    IntervalQuality quality,
    string expected )
  {
    // Arrange & Act
    var result = quality.ModernSymbol;

    // Assert
    result.Should()
          .Be( expected );
  }

  [Theory]
  [MemberData( nameof( ValidQualityNames ) )]
  public void ToString_ShouldReturnName(
    IntervalQuality quality,
    string expectedName )
  {
    quality.ToString()
           .Should()
           .Be( expectedName );
  }

  #endregion
}
