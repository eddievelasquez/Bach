// Module Name: StepCollectionTests.cs
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

using System.Collections;
using System.Collections.Generic;

namespace Bach.Model.Test;

public class StepCollectionTests
{
  #region Public Methods

  [Theory]
  [InlineData( new[] { 2, 2, 2, 2, 2, 2 }, 6 )] // Valid case
  [InlineData( new[] { 3, 3, 3, 2, 1 }, 5 )] // Valid case
  [InlineData( new[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, 12 )] // Valid case
  public void Count_ShouldReturnCorrectNumberOfSteps(
    int[] steps,
    int expectedCount )
  {
    // Arrange
    var stepCollection = new StepCollection( steps );

    // Act
    var count = stepCollection.Count;

    // Assert
    count.Should()
         .Be( expectedCount );
  }

  [Fact]
  public void GetEnumerator_NonGeneric_ShouldEnumerateAllSteps()
  {
    // Arrange
    var steps = new[] { 2, 2, 2, 2, 2, 2 };
    var stepCollection = new StepCollection( steps );

    // Act
    var enumerator = ( (IEnumerable) stepCollection ).GetEnumerator();
    var enumeratedSteps = new List<int>();

    while( enumerator.MoveNext() )
    {
      enumeratedSteps.Add( (int) enumerator.Current );
    }

    // Assert
    enumeratedSteps.Should()
                   .BeEquivalentTo( steps );
  }

  [Theory]
  [InlineData( new[] { 2, 2, 2, 2, 2, 2 } )] // Valid case
  [InlineData( new[] { 3, 3, 3, 2, 1 } )] // Valid case
  [InlineData( new[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 } )] // Valid case
  public void GetEnumerator_ShouldEnumerateAllSteps(
    int[] steps )
  {
    // Arrange
    var stepCollection = new StepCollection( steps );

    // Act
    var enumeratedSteps = new List<int>();

    foreach( var step in stepCollection )
    {
      enumeratedSteps.Add( step );
    }

    // Assert
    enumeratedSteps.Should()
                   .BeEquivalentTo( steps );
  }

  [Fact]
  public void Constructor_ShouldCreateInstance_WhenStepsAreValid()
  {
    // Arrange
    var steps = new[] { 2, 2, 1, 2, 2, 2, 1 };

    // Act
    var stepCollection = new StepCollection( steps );

    // Assert
    stepCollection.Should()
                  .NotBeNull();
    stepCollection.Count.Should()
                  .Be( 7 );
  }

  [Theory]
  [InlineData( "W-W-H-W-W-W-H", 7 )] // Major scale
  [InlineData( "2-2-1-2-2-2-1", 7 )] // Major scale with numbers
  [InlineData( "w-w-h-w-w-w-h", 7 )] // Lowercase
  [InlineData( "2-2-2-2-2-2", 6 )] // Whole tone scale
  [InlineData( "3-3-3-2-1", 5 )] // Pentatonic-like
  public void Parse_ShouldReturnStepCollection_WhenStringIsValid(
    string input,
    int expectedCount )
  {
    // Act
    var result = StepCollection.Parse( input );

    // Assert
    result.Should()
          .NotBeNull();
    result.Count.Should()
          .Be( expectedCount );
  }

  [Fact]
  public void Parse_ShouldThrowArgumentException_WhenStringIsEmpty()
  {
    // Act
    Action act = () => StepCollection.Parse( string.Empty );

    // Assert
    act.Should()
       .Throw<ArgumentException>();
  }

  [Theory]
  [InlineData( "invalid" )]
  [InlineData( "X-Y-Z-A-B" )]
  [InlineData( "1,2,3" )] // Too few steps (also triggers bug)
  public void Parse_ShouldThrowFormatException_WhenStringIsInvalid(
    string input )
  {
    // Act
    Action act = () => StepCollection.Parse( input );

    // Assert
    act.Should()
       .Throw<FormatException>();
  }

  [Fact]
  public void Parse_WithNullString_ShouldThrowArgumentNullException()
  {
    // Act
    Action act = () => StepCollection.Parse( (string) null!, null );

    // Assert
    act.Should()
       .Throw<ArgumentNullException>();
  }

  [Fact]
  public void Parse_WithEmptySpan_ShouldThrowArgumentException()
  {
    // Arrange
    var input = ReadOnlySpan<char>.Empty;

    // Act & Assert
    try
    {
      StepCollection.Parse( input, null );
      Assert.Fail( "Expected ArgumentException was not thrown" );
    }
    catch( ArgumentException exception )
    {
      exception.Message.Should()
               .Contain( "cannot be empty" );
    }
  }

  [Theory]
  [InlineData( "W-W-H-W-W-W-H" )]
  [InlineData( "2-2-1-2-2-2-1" )]
  public void Parse_WithValidSpan_ShouldReturnStepCollection(
    string input )
  {
    // Act
    var result = StepCollection.Parse( input.AsSpan(), null );

    // Assert
    result.Should()
          .NotBeNull();
    result.Count.Should()
          .Be( 7 );
  }

  [Fact]
  public void Parse_WithInvalidSpan_ShouldThrowFormatException()
  {
    // Arrange
    var input = "invalid".AsSpan();

    // Act & Assert
    try
    {
      StepCollection.Parse( input, null );
      Assert.Fail( "Expected FormatException was not thrown" );
    }
    catch( FormatException exception )
    {
      exception.Message.Should()
               .Contain( "not in a valid format" );
    }
  }

  [Fact]
  public void TryParse_WithNullString_ShouldReturnFalse()
  {
    // Act
    var result = StepCollection.TryParse( (string?) null, out var steps );

    // Assert
    result.Should()
          .BeFalse();
    steps.Should()
         .BeNull();
  }

  [Theory]
  [InlineData( "W-W-H-W-W-W-H", 7 )]
  [InlineData( "2-2-1-2-2-2-1", 7 )]
  public void TryParse_WithValidString_ShouldReturnTrueAndStepCollection(
    string input,
    int expectedCount )
  {
    // Act
    var result = StepCollection.TryParse( input, out var steps );

    // Assert
    result.Should()
          .BeTrue();
    steps.Should()
         .NotBeNull();
    steps!.Count.Should()
          .Be( expectedCount );
  }

  [Theory]
  [InlineData( "invalid" )]
  [InlineData( "X-Y-Z" )]
  public void TryParse_WithInvalidString_ShouldReturnFalse(
    string input )
  {
    // Act
    var result = StepCollection.TryParse( input, out var steps );

    // Assert
    result.Should()
          .BeFalse();
    steps.Should()
         .BeNull();
  }

  [Fact]
  public void TryParse_WithProvider_AndNullString_ShouldReturnFalse()
  {
    // Act
    var result = StepCollection.TryParse( (string?) null, null, out var steps );

    // Assert
    result.Should()
          .BeFalse();
    steps.Should()
         .BeNull();
  }

  [Theory]
  [InlineData( "W-W-H-W-W-W-H", 7 )]
  [InlineData( "w-w-h-w-w-w-h", 7 )]
  public void TryParse_WithProvider_AndValidString_ShouldReturnTrueAndStepCollection(
    string input,
    int expectedCount )
  {
    // Act
    var result = StepCollection.TryParse( input, null, out var steps );

    // Assert
    result.Should()
          .BeTrue();
    steps.Should()
         .NotBeNull();
    steps!.Count.Should()
          .Be( expectedCount );
  }

  [Theory]
  [InlineData( "invalid" )]
  [InlineData( "X-Y-Z" )]
  public void TryParse_WithProvider_AndInvalidString_ShouldReturnFalse(
    string input )
  {
    // Act
    var result = StepCollection.TryParse( input, null, out var steps );

    // Assert
    result.Should()
          .BeFalse();
    steps.Should()
         .BeNull();
  }

  [Fact]
  public void TryParse_WithEmptySpan_ShouldReturnFalse()
  {
    // Act
    var result = StepCollection.TryParse( ReadOnlySpan<char>.Empty, null, out var steps );

    // Assert
    result.Should()
          .BeFalse();
    steps.Should()
         .BeNull();
  }

  [Theory]
  [InlineData( "W-W-W" )] // Only 3 steps (need at least 5)
  [InlineData( "1-2-3-1" )] // Only 4 steps (need at least 5)
  public void TryParse_WithTooFewSeparators_ShouldReturnFalse(
    string input )
  {
    // Act
    var result = StepCollection.TryParse( input.AsSpan(), null, out var steps );

    // Assert
    result.Should()
          .BeFalse();
    steps.Should()
         .BeNull();
  }

  [Fact]
  public void TryParse_WithTooManySeparators_ShouldReturnFalse()
  {
    // Arrange - 13 steps (need at most 12)
    var input = "1-1-1-1-1-1-1-1-1-1-1-1-1".AsSpan();

    // Act
    var result = StepCollection.TryParse( input, null, out var steps );

    // Assert
    result.Should()
          .BeFalse();
    steps.Should()
         .BeNull();
  }

  [Theory]
  [InlineData( "WW-H-W-W-W-H" )] // Multi-character range
  [InlineData( "W-HH-W-W-W-H" )] // Multi-character range
  public void TryParse_WithMultiCharacterRange_ShouldReturnFalse(
    string input )
  {
    // Act
    var result = StepCollection.TryParse( input.AsSpan(), null, out var steps );

    // Assert
    result.Should()
          .BeFalse();
    steps.Should()
         .BeNull();
  }

  [Theory]
  [InlineData( "X-Y-Z-A-B-C-D" )] // Invalid characters
  [InlineData( "5-2-2-2-2-2" )] // Invalid number 4
  [InlineData( "0-2-2-2-2-2" )] // Invalid number 0
  public void TryParse_WithInvalidCharacters_ShouldReturnFalse(
    string input )
  {
    // Act
    var result = StepCollection.TryParse( input.AsSpan(), null, out var steps );

    // Assert
    result.Should()
          .BeFalse();
    steps.Should()
         .BeNull();
  }

  [Theory]
  [InlineData( "H-H-H-H-H-H-H-H-H-H-H-H" )] // All H (1)
  [InlineData( "W-W-W-W-W-W" )] // All W (2)
  [InlineData( "3-3-3-2-1" )] // Valid with 3
  public void TryParse_WithValidCharacters_ShouldReturnTrue(
    string input )
  {
    // Act
    var result = StepCollection.TryParse( input.AsSpan(), null, out var steps );

    // Assert
    result.Should()
          .BeTrue();
    steps.Should()
         .NotBeNull();
  }

  [Theory]
  [InlineData( "h-h-h-h-h-h-h-h-h-h-h-h" )] // Lowercase h
  [InlineData( "w-w-w-w-w-w" )] // Lowercase w
  public void TryParse_WithLowercaseCharacters_ShouldReturnTrue(
    string input )
  {
    // Act
    var result = StepCollection.TryParse( input.AsSpan(), null, out var steps );

    // Assert
    result.Should()
          .BeTrue();
    steps.Should()
         .NotBeNull();
  }

  [Fact]
  public void ToString_ShouldReturnUppercaseFormat_WhenFormatIsNull()
  {
    // Arrange
    var stepCollection = new StepCollection( new[] { 2, 2, 1, 2, 2, 2, 1 } );

    // Act
    var result = stepCollection.ToString( null, null );

    // Assert
    result.Should()
          .Be( "W-W-H-W-W-W-H" );
  }

  [Fact]
  public void ToString_ShouldReturnNumericFormat_WhenFormatIsN()
  {
    // Arrange
    var stepCollection = new StepCollection( new[] { 2, 2, 1, 2, 2, 2, 1 } );

    // Act
    var result = stepCollection.ToString( "N", null );

    // Assert
    result.Should()
          .Be( "2-2-1-2-2-2-1" );
  }

  [Fact]
  public void ToString_ShouldReturnUppercaseFormat_WhenFormatIsS()
  {
    // Arrange
    var stepCollection = new StepCollection( new[] { 2, 2, 1, 2, 2, 2, 1 } );

    // Act
    var result = stepCollection.ToString( "S", null );

    // Assert
    result.Should()
          .Be( "W-W-H-W-W-W-H" );
  }

  [Fact]
  public void ToString_ShouldReturnLowercaseFormat_WhenFormatIsLowerS()
  {
    // Arrange
    var stepCollection = new StepCollection( new[] { 2, 2, 1, 2, 2, 2, 1 } );

    // Act
    var result = stepCollection.ToString( "s", null );

    // Assert
    result.Should()
          .Be( "w-w-h-w-w-w-h" );
  }

  [Fact]
  public void ToString_ShouldReturnEmptyString_WhenFormatIsEmpty()
  {
    // Arrange
    var stepCollection = new StepCollection( new[] { 2, 2, 1, 2, 2, 2, 1 } );

    // Act
    var result = stepCollection.ToString( "", null );

    // Assert
    result.Should()
          .Be( "" );
  }

  [Fact]
  public void ToString_ShouldIncludeLiteralCharacters_WhenFormatContainsNonFormatCharacters()
  {
    // Arrange
    var stepCollection = new StepCollection( new[] { 2, 2, 1, 2, 2, 2, 1 } );

    // Act
    var result = stepCollection.ToString( "(N)", null );

    // Assert
    result.Should()
          .Be( "(2-2-1-2-2-2-1)" );
  }

  [Fact]
  public void ToString_ShouldHandleMixedFormatCodes_WhenFormatContainsMultipleFormatCharacters()
  {
    // Arrange
    var stepCollection = new StepCollection( new[] { 2, 2, 1, 2, 2, 2, 1 } );

    // Act
    var result = stepCollection.ToString( "N=S", null );

    // Assert
    result.Should()
          .Be( "2-2-1-2-2-2-1=W-W-H-W-W-W-H" );
  }

  [Fact]
  public void ToString_ShouldHandleAllStepValues_WhenStepsInclude1And2And3()
  {
    // Arrange
    var stepCollection = new StepCollection( new[] { 3, 3, 3, 2, 1 } );

    // Act
    var resultN = stepCollection.ToString( "N", null );
    var resultS = stepCollection.ToString( "S", null );
    var results = stepCollection.ToString( "s", null );

    // Assert
    resultN.Should()
           .Be( "3-3-3-2-1" );
    resultS.Should()
           .Be( "3-3-3-W-H" );
    results.Should()
           .Be( "3-3-3-w-h" );
  }

  [Fact]
  public void ToString_ShouldReturnLiteralText_WhenFormatContainsOnlyLiterals()
  {
    // Arrange
    var stepCollection = new StepCollection( new[] { 2, 2, 1, 2, 2, 2, 1 } );

    // Act
    var result = stepCollection.ToString( "()[]", null );

    // Assert
    result.Should()
          .Be( "()[]" );
  }

  [Fact]
  public void ToString_ShouldIgnoreFormatProvider_WhenFormatProviderIsSpecified()
  {
    // Arrange
    var stepCollection = new StepCollection( new[] { 2, 2, 1, 2, 2, 2, 1 } );
    var formatProvider = System.Globalization.CultureInfo.InvariantCulture;

    // Act
    var result = stepCollection.ToString( "S", formatProvider );

    // Assert
    result.Should()
          .Be( "W-W-H-W-W-W-H" );
  }

  [Fact]
  public void ToString_ShouldHandleAllHalfSteps_WhenAllStepsAre1()
  {
    // Arrange
    var stepCollection = new StepCollection( new[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 } );

    // Act
    var resultS = stepCollection.ToString( "S", null );
    var results = stepCollection.ToString( "s", null );
    var resultN = stepCollection.ToString( "N", null );

    // Assert
    resultS.Should()
           .Be( "H-H-H-H-H-H-H-H-H-H-H-H" );
    results.Should()
           .Be( "h-h-h-h-h-h-h-h-h-h-h-h" );
    resultN.Should()
           .Be( "1-1-1-1-1-1-1-1-1-1-1-1" );
  }

  [Fact]
  public void ToString_ShouldHandleAllWholeSteps_WhenAllStepsAre2()
  {
    // Arrange
    var stepCollection = new StepCollection( new[] { 2, 2, 2, 2, 2, 2 } );

    // Act
    var resultS = stepCollection.ToString( "S", null );
    var results = stepCollection.ToString( "s", null );
    var resultN = stepCollection.ToString( "N", null );

    // Assert
    resultS.Should()
           .Be( "W-W-W-W-W-W" );
    results.Should()
           .Be( "w-w-w-w-w-w" );
    resultN.Should()
           .Be( "2-2-2-2-2-2" );
  }

  #endregion
}
