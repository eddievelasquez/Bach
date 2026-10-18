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

using System.Linq;

namespace Bach.Model.Scales.Test;

public sealed class StepCollectionTests
{
  #region Properties

  public static TheoryData<string, int[]> SupportedTokens =>
    new()
    {
      {
        "H-W-3-4-1", [
          1, 2, 3,
          4, 1
        ]
      },
      {
        "h-w-3-4-1", [
          1, 2, 3,
          4, 1
        ]
      },
      {
        "1-2-3-4-1", [
          1, 2, 3,
          4, 1
        ]
      },
      {
        " W - h - 2 - 1 - 3 ", [
          2, 1, 2,
          1, 3
        ]
      }
    };

  public static TheoryData<string> InvalidStepCollections =>
    new()
    {
      "1-1-1-1",
      "1-1-1-1-1-1-1-1-1-1-1-1-1",
      "1-1-1-1-X",
      "1-1-1-1-10"
    };

  #endregion

  #region Public Methods

  [Theory]
  [MemberData( nameof( SupportedTokens ) )]
  public void Parse_ShouldReturnExpectedSteps_WhenTokensAreSupported(
    string input,
    int[] expectedSteps )
  {
    StepCollection.Parse( input )
                  .Should()
                  .Equal( expectedSteps );
  }

  [Fact]
  public void Parse_ShouldReturnExpectedSteps_WhenInputIsSpan()
  {
    StepCollection.Parse( "W-H-2-1-3".AsSpan() )
                  .Should()
                  .Equal( 2, 1, 2, 1, 3 );
  }

  [Theory]
  [InlineData( "1-1-1-1-1", 5 )]
  [InlineData( "1-1-1-1-1-1-1-1-1-1-1-1", 12 )]
  public void TryParse_ShouldReturnExpectedSteps_WhenCountIsAtSupportedBoundary(
    string input,
    int expectedCount )
  {
    var parsed = StepCollection.TryParse( input, out var steps );

    parsed.Should()
          .BeTrue();

    steps.Should()
         .Equal( Enumerable.Repeat( 1, expectedCount ) );
  }

  [Theory]
  [MemberData( nameof( InvalidStepCollections ) )]
  public void TryParse_ShouldReturnFalseAndNullSteps_WhenInputIsInvalid(
    string input )
  {
    var parsed = StepCollection.TryParse( input, out var steps );

    parsed.Should()
          .BeFalse();

    steps.Should()
         .BeNull();
  }

  [Fact]
  public void TryParse_ShouldReturnFalseAndNullSteps_WhenInputIsNull()
  {
    var parsed = StepCollection.TryParse( null, out var steps );

    parsed.Should()
          .BeFalse();

    steps.Should()
         .BeNull();
  }

  [Fact]
  public void TryParse_ShouldReturnFalseAndNullSteps_WhenSpanCountIsTooSmall()
  {
    var parsed = StepCollection.TryParse( "1-1-1-1".AsSpan(), out var steps );

    parsed.Should()
          .BeFalse();

    steps.Should()
         .BeNull();
  }

  [Fact]
  public void TryParse_ShouldReturnFalseAndNullSteps_WhenSpanContainsInvalidToken()
  {
    var parsed = StepCollection.TryParse( "1-1-1-1-X".AsSpan(), out var steps );

    parsed.Should()
          .BeFalse();

    steps.Should()
         .BeNull();
  }

  [Fact]
  public void Parse_ShouldThrowArgumentException_WhenInputIsEmpty()
  {
    Action action = () => StepCollection.Parse( string.Empty );

    action.Should()
          .Throw<ArgumentException>();
  }

  [Fact]
  public void Parse_ShouldThrowArgumentException_WhenSpanIsEmpty()
  {
    Action action = () => StepCollection.Parse( ReadOnlySpan<char>.Empty );

    action.Should()
          .Throw<ArgumentException>();
  }

  [Fact]
  public void Parse_ShouldThrowFormatException_WhenSpanContainsInvalidToken()
  {
    Action action = () => StepCollection.Parse( "1-1-1-1-X".AsSpan() );

    action.Should()
          .Throw<FormatException>();
  }

  [Theory]
  [MemberData( nameof( InvalidStepCollections ) )]
  public void Parse_ShouldThrowFormatException_WhenInputIsInvalid(
    string input )
  {
    Action action = () => StepCollection.Parse( input );

    action.Should()
          .Throw<FormatException>();
  }

  #endregion
}
