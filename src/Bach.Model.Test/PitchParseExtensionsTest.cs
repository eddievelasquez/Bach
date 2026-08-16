// Module Name: PitchParseExtensionsTest.cs
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

public sealed class PitchParseExtensionsTest
{
  #region Properties

  public static TheoryData<string, Type> UnparsablePitchValues =>
    new()
    {
      { null!, typeof( ArgumentNullException ) },
      { "", typeof( ArgumentException ) },
      { "C$4,Z5", typeof( FormatException ) }
    };

  #endregion

  #region Public Methods

  [Fact]
  public void ParsePitchClasses_Span_ShouldThrowFormatException_WhenInvalid()
  {
    Action act = () => _ = "X".AsSpan()
                              .ParsePitchClasses();

    act.Should()
       .Throw<FormatException>()
       .Which.Message.Should()
       .Contain( "contains invalid pitchClasses" );
  }

  [Fact]
  public void ParsePitchClasses_String_ShouldReturnExpectedList_WhenValid()
  {
    var result = "C,Eb,G".ParsePitchClasses();

    result.Should()
          .HaveCount( 3 );

    result[0]
      .Should()
      .Be( PitchClass.C );

    result[1]
      .Should()
      .Be( PitchClass.EFlat );

    result[2]
      .Should()
      .Be( PitchClass.G );
  }

  [Fact]
  public void ParsePitchClasses_String_ShouldThrow_WhenNullOrEmpty()
  {
    Action actNull = () => _ = ( (string) null! ).ParsePitchClasses();

    actNull.Should()
           .Throw<ArgumentNullException>();

    Action actEmpty = () => _ = string.Empty.ParsePitchClasses();

    actEmpty.Should()
            .Throw<ArgumentException>();
  }

  [Fact]
  public void ParsePitches_ShouldReturnExpectedValue_WhenInputIsMidiNotes()
  {
    Pitch[] expected = [Pitch.Parse( "C4" ), Pitch.Parse( "C5" )];

    "60,72".ParsePitches()
           .Should()
           .BeEquivalentTo( expected ); // Using midi
  }

  [Fact]
  public void ParsePitches_ShouldReturnExpectedValue_WhenInputIsPitchClasses()
  {
    Pitch[] expected = [Pitch.Parse( "C4" ), Pitch.Parse( "C5" )];

    "C4,C5".ParsePitches()
           .Should()
           .BeEquivalentTo( expected ); // Using pitches
  }

  [Theory]
  [MemberData( nameof( UnparsablePitchValues ) )]
  public void ParsePitches_ShouldThrow_WhenInputIsInvalid(
    string input,
    Type expectedExceptionType )
  {
    Action act = () => _ = input.ParsePitches();

    act.Should()
       .Throw<Exception>()
       .Which.Should()
       .BeOfType( expectedExceptionType );
  }

  [Fact]
  public void ParsePitches_Span_ShouldThrowFormatException_WhenInvalid()
  {
    Action act = () => _ = "Z".AsSpan()
                              .ParsePitches();

    act.Should()
       .Throw<FormatException>()
       .Which.Message.Should()
       .Contain( "contains invalid pitches" );
  }

  [Fact]
  public void ParsePitches_String_ShouldReturnExpectedList_WhenValid()
  {
    var result = "C4,C5".ParsePitches();

    result.Should()
          .HaveCount( 2 );

    result[0]
      .Should()
      .Be( Pitch.Parse( "C4" ) );

    result[1]
      .Should()
      .Be( Pitch.Parse( "C5" ) );
  }

  [Fact]
  public void TryParsePitchClasses_ShouldReturnFalseAndResultNotNull_WhenTrailingCharactersRemain()
  {
    var success = "C,".AsSpan()
                      .TryParsePitchClasses( null, out var result );

    success.Should()
           .BeFalse();

    result.Should()
          .NotBeNull();

    result.Count.Should()
          .Be( 1 );
  }

  [Fact]
  public void TryParsePitchClasses_ShouldReturnFalseAndResultNull_WhenSpanEmpty()
  {
    var success = string.Empty.AsSpan()
                        .TryParsePitchClasses( null, out var result );

    success.Should()
           .BeFalse();

    result.Should()
          .BeNull();
  }

  [Fact]
  public void TryParsePitchClasses_ShouldReturnTrueAndResult_WhenFullyConsumed()
  {
    var success = "C,E".AsSpan()
                       .TryParsePitchClasses( null, out var result );

    success.Should()
           .BeTrue();

    result.Should()
          .NotBeNull();

    result.Count.Should()
          .Be( 2 );
  }

  [Fact]
  public void TryParsePitchClasses_WithTail_ShouldReturnFalseAndNull_WhenSpanEmpty()
  {
    var success = string.Empty.AsSpan()
                        .TryParsePitchClasses( null, out var result, out var tail );

    success.Should()
           .BeFalse();

    result.Should()
          .BeNull();

    tail.IsEmpty.Should()
        .BeTrue();
  }

  [Fact]
  public void TryParsePitchClasses_WithTail_ShouldReturnFalseAndTailUnchanged_WhenInvalidElement()
  {
    var success = "C,X".AsSpan()
                       .TryParsePitchClasses( null, out var result, out var tail );

    success.Should()
           .BeFalse();

    result.Should()
          .BeNull();

    tail.ToString()
        .Should()
        .Be( "C,X" );
  }

  [Fact]
  public void TryParsePitchClasses_WithTail_ShouldReturnTrueAndTail_WhenTrailingCharactersRemain()
  {
    var success = "C,E,".AsSpan()
                        .TryParsePitchClasses( null, out var result, out var tail );

    success.Should()
           .BeTrue();

    result.Should()
          .NotBeNull();

    result.Count.Should()
          .Be( 2 );

    tail.ToString()
        .Should()
        .Be( "C,E," );
  }

  [Fact]
  public void TryParsePitches_ShouldReturnFalseAndResultNotNull_WhenTrailingCharactersRemain_Pitch()
  {
    var success = "C4,".AsSpan()
                       .TryParsePitches( null, out var result );

    success.Should()
           .BeFalse();

    result.Should()
          .NotBeNull();

    result.Count.Should()
          .Be( 1 );

    result[0]
      .Should()
      .Be( Pitch.Parse( "C4" ) );
  }

  [Fact]
  public void TryParsePitches_ShouldReturnFalseAndResultNull_WhenSpanEmpty_Pitch()
  {
    var success = string.Empty.AsSpan()
                        .TryParsePitches( null, out var result );

    success.Should()
           .BeFalse();

    result.Should()
          .BeNull();
  }

  [Fact]
  public void TryParsePitches_ShouldReturnTrueAndResult_WhenFullyConsumed_Pitch()
  {
    var success = "C4,C5".AsSpan()
                         .TryParsePitches( null, out var result );

    success.Should()
           .BeTrue();

    result.Should()
          .NotBeNull();

    result.Count.Should()
          .Be( 2 );

    result[0]
      .Should()
      .Be( Pitch.Parse( "C4" ) );

    result[1]
      .Should()
      .Be( Pitch.Parse( "C5" ) );
  }

  [Fact]
  public void TryParsePitches_WithTail_ShouldReturnFalseAndNull_WhenSpanEmpty_Pitch()
  {
    var success = string.Empty.AsSpan()
                        .TryParsePitches( null, out var result, out var tail );

    success.Should()
           .BeFalse();

    result.Should()
          .BeNull();

    tail.IsEmpty.Should()
        .BeTrue();
  }

  [Fact]
  public void TryParsePitches_WithTail_ShouldReturnFalseAndTailUnchanged_WhenInvalidElement_Pitch()
  {
    var success = "C4,X".AsSpan()
                        .TryParsePitches( null, out var result, out var tail );

    success.Should()
           .BeFalse();

    result.Should()
          .BeNull();

    tail.ToString()
        .Should()
        .Be( "C4,X" );
  }

  [Fact]
  public void TryParsePitches_WithTail_ShouldReturnTrueAndTail_WhenTrailingCharactersRemain_Pitch()
  {
    var success = "C4,C5,".AsSpan()
                          .TryParsePitches( null, out var result, out var tail );

    success.Should()
           .BeTrue();

    result.Should()
          .NotBeNull();

    result.Count.Should()
          .Be( 2 );

    tail.ToString()
        .Should()
        .Be( "C4,C5," );
  }

  #endregion
}
