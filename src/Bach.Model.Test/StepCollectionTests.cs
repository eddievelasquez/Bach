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
using System.Linq;

namespace Bach.Model.Test;

public class StepCollectionTests
{
  #region Public Methods

  [Theory]
  [InlineData( new[] { 1, 1, 1, 1 } )] // Less than minimum steps
  [InlineData( new[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 } )] // More than maximum steps
  [InlineData( new[] { 0, 1, 1, 1, 1, 1 } )] // Step less than minimum size
  [InlineData( new[] { 4, 1, 1, 1, 1, 1 } )] // Step greater than maximum size
  [InlineData( new[] { 2, 2, 2, 2, 2, 1 } )] // Sum not equal to 12
  public void Constructor_ShouldThrowArgumentOutOfRangeException_WhenStepsAreInvalid(
int[] steps )
  {
    // Act
    Action act = () => new StepCollection( steps );

    // Assert
    act.Should()
       .Throw<ArgumentOutOfRangeException>();
  }

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
  [InlineData( "W-W-H-W-W-W-H", new[] { "P1", "M2", "M3", "P4", "P5", "M6", "M7" } )]
  [InlineData( "2-2-3-2-3", new[] { "P1", "M2", "M3", "P5", "M6" } )]
  [InlineData( "3-2-2-3-2", new[] { "P1", "m3", "P4", "P5", "m7" } )]
  [InlineData( "3-2-1-1-3-2", new[] { "P1", "m3", "P4", "A4", "P5", "m7" } )]
  [InlineData( "2-1-2-2-1-2-2", new[] { "P1", "M2", "m3", "P4", "P5", "m6", "m7" } )]
  public void ToIntervals_ShouldReturnIntervalsStartingAtUnison(
    string input,
    string[] expected )
  {
    // Arrange
    var stepCollection = StepCollection.Parse( input );

    // Act
    var intervals = stepCollection.ToIntervals();

    // Assert
    intervals.Should()
             .ContainInOrder( expected.Select( Interval.Parse ) );
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
  [InlineData( "4-2-2-2-2-2" )] // Invalid number 4
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

  [Theory]
  [InlineData( "2-2-2-2-2-1" )] // Sum is 11, not 12
  [InlineData( "3-3-3-3-3" )] // Sum is 15, not 12
  public void TryParse_WithInvalidSum_ShouldReturnFalse(
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

  #endregion
}
