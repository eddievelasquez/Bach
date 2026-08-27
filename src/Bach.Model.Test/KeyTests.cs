// Module Name: KeyTest.cs
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

public sealed class KeyTests
{
  #region Properties

  public static TheoryData<string, PitchClass, ModeType> ModeNames =>
    new()
    {
      { "Am", PitchClass.A, ModeType.Minor },
      { "A", PitchClass.A, ModeType.Major },
      { "AM", PitchClass.A, ModeType.Major }
    };

  #endregion

  #region Public Methods

  [Fact]
  public void Constructor_ShouldCreateScaleWithCorrectRoot_WhenCalled()
  {
    // Arrange
    var pitchClass = PitchClass.BFlat;
    var mode = ModeType.Major;

    // Act
    var key = new Key( pitchClass, mode );

    // Assert
    key.Scale.Root.Should()
       .Be( pitchClass );
  }

  [Fact]
  public void Constructor_ShouldCreateScaleWithMajorFormula_WhenModeIsMajor()
  {
    // Arrange & Act
    var key = new Key( PitchClass.D, ModeType.Major );

    // Assert
    key.Scale.Formula.Name.Should()
       .Be( "Major" );
  }

  [Fact]
  public void Constructor_ShouldCreateScaleWithNaturalMinorFormula_WhenModeIsMinor()
  {
    // Arrange & Act
    var key = new Key( PitchClass.D, ModeType.Minor );

    // Assert
    key.Scale.Formula.Name.Should()
       .Be( "Natural Minor" );
  }

  [Fact]
  public void Constructor_ShouldInitializeAllProperties_WhenCalledWithAnyMode()
  {
    // Arrange
    var pitchClass = PitchClass.E;
    var mode = ModeType.Major;

    // Act
    var key = new Key( pitchClass, mode );

    // Assert
    key.Tonic.Should()
       .Be( pitchClass );

    key.Mode.Should()
       .Be( mode );

    key.Scale.Should()
       .NotBeNull();

    key.KeySignature.Should()
       .NotBeNull();
  }

  [Fact]
  public void Constructor_ShouldInitializeCorrectly_ForMajorKey()
  {
    var key = new Key( PitchClass.G, ModeType.Major );

    key.Tonic.Should()
       .Be( PitchClass.G );

    key.Mode.Should()
       .Be( ModeType.Major );

    key.KeySignature.AccidentalCount.Should()
       .Be( 1 );

    key.KeySignature.Accidental.Should()
       .Be( Accidental.Sharp );

    key.Scale.Should()
       .NotBeNull();

    key.Scale.Root.Should()
       .Be( PitchClass.G );

    key.Scale.Formula.Name.Should()
       .Be( "Major" );
  }

  [Fact]
  public void Constructor_ShouldInitializeCorrectly_ForMinorKey()
  {
    var key = new Key( PitchClass.E, ModeType.Minor );

    key.Tonic.Should()
       .Be( PitchClass.E );

    key.Mode.Should()
       .Be( ModeType.Minor );

    key.KeySignature.AccidentalCount.Should()
       .Be( 1 );

    key.KeySignature.Accidental.Should()
       .Be( Accidental.Sharp );

    key.Scale.Root.Should()
       .Be( PitchClass.E );
  }

  [Fact]
  public void Constructor_ShouldInitializeProperties_WhenCalledWithMajorMode()
  {
    // Arrange
    var pitchClass = PitchClass.C;
    var mode = ModeType.Major;

    // Act
    var key = new Key( pitchClass, mode );

    // Assert
    key.Tonic.Should()
       .Be( pitchClass );

    key.Mode.Should()
       .Be( mode );

    key.Scale.Should()
       .NotBeNull();

    key.Scale.Root.Should()
       .Be( pitchClass );

    key.KeySignature.Should()
       .NotBeNull();
  }

  [Fact]
  public void Constructor_ShouldInitializeProperties_WhenCalledWithMinorMode()
  {
    // Arrange
    var pitchClass = PitchClass.A;
    var mode = ModeType.Minor;

    // Act
    var key = new Key( pitchClass, mode );

    // Assert
    key.Tonic.Should()
       .Be( pitchClass );

    key.Mode.Should()
       .Be( mode );

    key.Scale.Should()
       .NotBeNull();

    key.Scale.Root.Should()
       .Be( pitchClass );

    key.KeySignature.Should()
       .NotBeNull();
  }

  [Fact]
  public void Constructor_ShouldSetKeySignatureFromTable_WhenKeyIsInTable()
  {
    // Arrange & Act
    var keyC = new Key( PitchClass.C, ModeType.Major );
    var keyG = new Key( PitchClass.G, ModeType.Major );
    var keyAm = new Key( PitchClass.A, ModeType.Minor );

    // Assert
    keyC.KeySignature.Should()
        .NotBeNull();

    keyG.KeySignature.Should()
        .NotBeNull();

    keyAm.KeySignature.Should()
         .NotBeNull();
  }

  [Fact]
  public void Constructor_ShouldSetKeySignatureFromTable_WhenKeyIsInTableDirectly()
  {
    // Arrange & Act
    var key = new Key( PitchClass.DFlat, ModeType.Major );

    // Assert
    key.KeySignature.Should()
       .NotBeNull();

    key.KeySignature.Should()
       .NotBe( KeySignature.Empty );
  }

  [Fact]
  public void Constructor_ShouldSetKeySignature_WhenKeyIsInTable()
  {
    // Arrange & Act
    var key = new Key( PitchClass.G, ModeType.Major );

    // Assert
    key.KeySignature.Should()
       .NotBe( KeySignature.Empty );
  }

  [Fact]
  public void ParseSpan_ShouldReturnKey_WhenSpanHasFlat()
  {
    // Arrange
    var span = "Ab".AsSpan();

    // Act
    var key = Key.Parse( span, null );

    // Assert
    key.Should()
       .NotBeNull();

    key.Tonic.Should()
       .Be( PitchClass.AFlat );

    key.Mode.Should()
       .Be( ModeType.Major );
  }

  [Fact]
  public void ParseSpan_ShouldReturnKey_WhenSpanHasSharp()
  {
    // Arrange
    var span = "C#".AsSpan();

    // Act
    var key = Key.Parse( span, null );

    // Assert
    key.Should()
       .NotBeNull();

    key.Tonic.Should()
       .Be( PitchClass.CSharp );

    key.Mode.Should()
       .Be( ModeType.Major );
  }

  [Fact]
  public void ParseSpan_ShouldReturnKey_WhenSpanHasWhitespace()
  {
    // Arrange
    var span = "  F  ".AsSpan();

    // Act
    var key = Key.Parse( span, null );

    // Assert
    key.Should()
       .NotBeNull();

    key.Tonic.Should()
       .Be( PitchClass.F );

    key.Mode.Should()
       .Be( ModeType.Major );
  }

  [Fact]
  public void ParseSpan_ShouldReturnKey_WhenSpanIsValid()
  {
    // Arrange
    var span = "E".AsSpan();

    // Act
    var key = Key.Parse( span, null );

    // Assert
    key.Should()
       .NotBeNull();

    key.Tonic.Should()
       .Be( PitchClass.E );

    key.Mode.Should()
       .Be( ModeType.Major );
  }

  [Fact]
  public void ParseSpan_ShouldReturnMinorKey_WhenSpanEndsWithM()
  {
    // Arrange
    var span = "Em".AsSpan();

    // Act
    var key = Key.Parse( span, null );

    // Assert
    key.Should()
       .NotBeNull();

    key.Tonic.Should()
       .Be( PitchClass.E );

    key.Mode.Should()
       .Be( ModeType.Minor );
  }

  [Fact]
  public void ParseSpan_ShouldThrowFormatException_WhenSpanIsEmpty()
  {
    // Arrange & Act
    var exception = Record.Exception( () => Key.Parse( "".AsSpan(), null ) );

    // Assert
    exception.Should()
             .BeOfType<FormatException>();
  }

  [Fact]
  public void ParseSpan_ShouldThrowFormatException_WhenSpanIsInvalid()
  {
    // Arrange & Act
    var exception = Record.Exception( () => Key.Parse( "Invalid".AsSpan(), null ) );

    // Assert
    exception.Should()
             .BeOfType<FormatException>();
  }

  [Fact]
  public void ParseWithProvider_ShouldReturnKey_WhenValueHasFlat()
  {
    // Arrange
    var value = "Eb";

    // Act
    var key = Key.Parse( value, null );

    // Assert
    key.Should()
       .NotBeNull();

    key.Tonic.Should()
       .Be( PitchClass.EFlat );

    key.Mode.Should()
       .Be( ModeType.Major );
  }

  [Fact]
  public void ParseWithProvider_ShouldReturnKey_WhenValueHasSharp()
  {
    // Arrange
    var value = "G#";

    // Act
    var key = Key.Parse( value, null );

    // Assert
    key.Should()
       .NotBeNull();

    key.Tonic.Should()
       .Be( PitchClass.GSharp );

    key.Mode.Should()
       .Be( ModeType.Major );
  }

  [Fact]
  public void ParseWithProvider_ShouldReturnKey_WhenValueIsValid()
  {
    // Arrange
    var value = "D";

    // Act
    var key = Key.Parse( value, null );

    // Assert
    key.Should()
       .NotBeNull();

    key.Tonic.Should()
       .Be( PitchClass.D );

    key.Mode.Should()
       .Be( ModeType.Major );
  }

  [Fact]
  public void ParseWithProvider_ShouldReturnMinorKey_WhenValueEndsWithM()
  {
    // Arrange
    var value = "Dm";

    // Act
    var key = Key.Parse( value, null );

    // Assert
    key.Should()
       .NotBeNull();

    key.Tonic.Should()
       .Be( PitchClass.D );

    key.Mode.Should()
       .Be( ModeType.Minor );
  }

  [Fact]
  public void ParseWithProvider_ShouldThrowArgumentNullException_WhenValueIsNull()
  {
    // Arrange
    string value = null!;

    // Act
    Action act = () => Key.Parse( value, null );

    // Assert
    act.Should()
       .Throw<ArgumentNullException>();
  }

  [Fact]
  public void ParseWithProvider_ShouldThrowFormatException_WhenValueIsEmpty()
  {
    // Arrange
    var value = "";

    // Act
    Action act = () => Key.Parse( value, null );

    // Assert
    act.Should()
       .Throw<FormatException>();
  }

  [Fact]
  public void ParseWithProvider_ShouldThrowFormatException_WhenValueIsInvalid()
  {
    // Arrange
    var value = "Invalid";

    // Act
    Action act = () => Key.Parse( value, null );

    // Assert
    act.Should()
       .Throw<FormatException>();
  }

  [Fact]
  public void Parse_ShouldReturnKey_WhenValueHasAccidental()
  {
    // Arrange
    var value = "F#";

    // Act
    var key = Key.Parse( value );

    // Assert
    key.Should()
       .NotBeNull();

    key.Tonic.Should()
       .Be( PitchClass.FSharp );

    key.Mode.Should()
       .Be( ModeType.Major );
  }

  [Fact]
  public void Parse_ShouldReturnKey_WhenValueHasWhitespace()
  {
    // Arrange
    var value = "  Bb  ";

    // Act
    var key = Key.Parse( value );

    // Assert
    key.Should()
       .NotBeNull();

    key.Tonic.Should()
       .Be( PitchClass.BFlat );

    key.Mode.Should()
       .Be( ModeType.Major );
  }

  [Fact]
  public void Parse_ShouldReturnKey_WhenValueIsValid()
  {
    // Arrange
    var value = "C";

    // Act
    var key = Key.Parse( value );

    // Assert
    key.Should()
       .NotBeNull();

    key.Tonic.Should()
       .Be( PitchClass.C );

    key.Mode.Should()
       .Be( ModeType.Major );
  }

  [Fact]
  public void Parse_ShouldReturnKey_WhenValueIsValidSingleNote()
  {
    // Arrange
    var value = "D";

    // Act
    var key = Key.Parse( value );

    // Assert
    key.Should()
       .NotBeNull();

    key.Tonic.Should()
       .Be( PitchClass.D );

    key.Mode.Should()
       .Be( ModeType.Major );
  }

  [Theory]
  [MemberData( nameof( ModeNames ) )]
  public void Parse_ShouldReturnMinorKey_WhenValueEndsWithM(
    string value,
    PitchClass tonic,
    ModeType mode )
  {
    // Act
    var key = Key.Parse( value );

    // Assert
    key.Should()
       .NotBeNull();

    key.Tonic.Should()
       .Be( tonic );

    key.Mode.Should()
       .Be( mode );
  }

  [Fact]
  public void Parse_ShouldThrowArgumentNullException_WhenValueIsNull()
  {
    // Arrange
    string value = null!;

    // Act
    Action act = () => Key.Parse( value );

    // Assert
    act.Should()
       .Throw<ArgumentNullException>();
  }

  [Fact]
  public void Parse_ShouldThrowFormatException_WhenValueIsEmpty()
  {
    // Arrange
    var value = "";

    // Act
    Action act = () => Key.Parse( value );

    // Assert
    act.Should()
       .Throw<FormatException>();
  }

  [Fact]
  public void Parse_ShouldThrowFormatException_WhenValueIsInvalid()
  {
    // Arrange
    var value = "Invalid";

    // Act
    Action act = () => Key.Parse( value );

    // Assert
    act.Should()
       .Throw<FormatException>();
  }

  [Fact]
  public void ToString_ShouldRenderTonicAndMode()
  {
    var key = new Key( PitchClass.C, ModeType.Major );

    key.ToString()
       .Should()
       .Be( "C Major" );
  }

  [Fact]
  public void ToString_ShouldReturnFormattedString_WhenKeyHasFlatTonic()
  {
    // Arrange
    var key = new Key( PitchClass.BFlat, ModeType.Major );

    // Act
    var result = key.ToString();

    // Assert
    result.Should()
          .Be( "Bb Major" );
  }

  [Fact]
  public void ToString_ShouldReturnFormattedString_WhenKeyIsMajor()
  {
    // Arrange
    var key = new Key( PitchClass.G, ModeType.Major );

    // Act
    var result = key.ToString();

    // Assert
    result.Should()
          .Be( "G Major" );
  }

  [Fact]
  public void ToString_ShouldReturnFormattedString_WhenKeyIsMinor()
  {
    // Arrange
    var key = new Key( PitchClass.A, ModeType.Minor );

    // Act
    var result = key.ToString();

    // Assert
    result.Should()
          .Be( "A Minor" );
  }

  [Fact]
  public void ToString_ShouldReturnFormattedString_WhenKeyIsMinorWithSharp()
  {
    // Arrange
    var key = new Key( PitchClass.CSharp, ModeType.Minor );

    // Act
    var result = key.ToString();

    // Assert
    result.Should()
          .Be( "C# Minor" );
  }

  [Fact]
  public void ToString_ShouldReturnFormattedString_WhenTonicHasAccidental()
  {
    // Arrange
    var key = new Key( PitchClass.FSharp, ModeType.Major );

    // Act
    var result = key.ToString();

    // Assert
    result.Should()
          .Be( "F# Major" );
  }

  [Fact]
  public void TryParseSpan_ShouldHandleNullResult_WhenPitchClassParsingFails()
  {
    // Arrange
    var span = "XYZ".AsSpan();

    // Act
    var result = Key.TryParse( span, null, out var key );

    // Assert
    result.Should()
          .BeFalse();

    key.Should()
       .BeNull();
  }

  [Fact]
  public void TryParseSpan_ShouldHandleNullResult_WhenSpanIsEmpty()
  {
    // Arrange
    var span = "".AsSpan();

    // Act
    var result = Key.TryParse( span, null, out var key );

    // Assert
    result.Should()
          .BeFalse();

    key.Should()
       .BeNull();
  }

  [Fact]
  public void TryParseSpan_ShouldReturnFalse_WhenPitchClassIsInvalid()
  {
    // Arrange
    var span = "Z".AsSpan();

    // Act
    var result = Key.TryParse( span, null, out var key );

    // Assert
    result.Should()
          .BeFalse();

    key.Should()
       .BeNull();
  }

  [Fact]
  public void TryParseSpan_ShouldReturnFalse_WhenSpanIsEmpty()
  {
    // Arrange
    var span = "".AsSpan();

    // Act
    var result = Key.TryParse( span, null, out var key );

    // Assert
    result.Should()
          .BeFalse();

    key.Should()
       .BeNull();
  }

  [Fact]
  public void TryParseSpan_ShouldReturnFalse_WhenSpanIsInvalid()
  {
    // Arrange
    var span = "Invalid".AsSpan();

    // Act
    var result = Key.TryParse( span, null, out var key );

    // Assert
    result.Should()
          .BeFalse();

    key.Should()
       .BeNull();
  }

  [Fact]
  public void TryParseSpan_ShouldReturnFalse_WhenSpanIsWhitespace()
  {
    // Arrange
    var span = "   ".AsSpan();

    // Act
    var result = Key.TryParse( span, null, out var key );

    // Assert
    result.Should()
          .BeFalse();

    key.Should()
       .BeNull();
  }

  [Fact]
  public void TryParseSpan_ShouldReturnTrue_WhenSpanEndsWithM()
  {
    // Arrange
    var span = "Em".AsSpan();

    // Act
    var result = Key.TryParse( span, null, out var key );

    // Assert
    result.Should()
          .BeTrue();

    key.Should()
       .NotBeNull();

    key!.Tonic.Should()
        .Be( PitchClass.E );

    key.Mode.Should()
       .Be( ModeType.Minor );
  }

  [Fact]
  public void TryParseSpan_ShouldReturnTrue_WhenSpanHasFlat()
  {
    // Arrange
    var span = "Bb".AsSpan();

    // Act
    var result = Key.TryParse( span, null, out var key );

    // Assert
    result.Should()
          .BeTrue();

    key.Should()
       .NotBeNull();

    key!.Tonic.Should()
        .Be( PitchClass.BFlat );

    key.Mode.Should()
       .Be( ModeType.Major );
  }

  [Fact]
  public void TryParseSpan_ShouldReturnTrue_WhenSpanHasSharp()
  {
    // Arrange
    var span = "C#".AsSpan();

    // Act
    var result = Key.TryParse( span, null, out var key );

    // Assert
    result.Should()
          .BeTrue();

    key.Should()
       .NotBeNull();

    key!.Tonic.Should()
        .Be( PitchClass.CSharp );

    key.Mode.Should()
       .Be( ModeType.Major );
  }

  [Fact]
  public void TryParseSpan_ShouldReturnTrue_WhenSpanHasWhitespace()
  {
    // Arrange
    var span = "  F  ".AsSpan();

    // Act
    var result = Key.TryParse( span, null, out var key );

    // Assert
    result.Should()
          .BeTrue();

    key.Should()
       .NotBeNull();

    key!.Tonic.Should()
        .Be( PitchClass.F );

    key.Mode.Should()
       .Be( ModeType.Major );
  }

  [Fact]
  public void TryParseSpan_ShouldReturnTrue_WhenSpanIsValid()
  {
    // Arrange
    var span = "E".AsSpan();

    // Act
    var result = Key.TryParse( span, null, out var key );

    // Assert
    result.Should()
          .BeTrue();

    key.Should()
       .NotBeNull();

    key!.Tonic.Should()
        .Be( PitchClass.E );

    key.Mode.Should()
       .Be( ModeType.Major );
  }

  [Fact]
  public void TryParseSpan_ShouldReturnTrue_WhenSpanIsValidNoteWithSharp()
  {
    // Arrange
    var span = "D#".AsSpan();

    // Act
    var result = Key.TryParse( span, null, out var key );

    // Assert
    result.Should()
          .BeTrue();

    key.Should()
       .NotBeNull();

    key!.Tonic.Should()
        .Be( PitchClass.DSharp );

    key.Mode.Should()
       .Be( ModeType.Major );
  }

  [Fact]
  public void TryParseSpan_ShouldReturnTrue_WhenSpanIsValidNoteWithoutAccidental()
  {
    // Arrange
    var span = "G".AsSpan();

    // Act
    var result = Key.TryParse( span, null, out var key );

    // Assert
    result.Should()
          .BeTrue();

    key.Should()
       .NotBeNull();

    key!.Tonic.Should()
        .Be( PitchClass.G );

    key.Mode.Should()
       .Be( ModeType.Major );
  }

  [Fact]
  public void TryParseSpan_ShouldSetModeToMajor_WhenSpanDoesNotEndWithM()
  {
    // Arrange
    var span = "Ab".AsSpan();

    // Act
    var result = Key.TryParse( span, null, out var key );

    // Assert
    result.Should()
          .BeTrue();

    key.Should()
       .NotBeNull();

    key!.Mode.Should()
        .Be( ModeType.Major );
  }

  [Fact]
  public void TryParseSpan_ShouldTrimWhitespace_WhenSpanHasLeadingAndTrailingSpaces()
  {
    // Arrange
    var span = "   Eb   ".AsSpan();

    // Act
    var result = Key.TryParse( span, null, out var key );

    // Assert
    result.Should()
          .BeTrue();

    key.Should()
       .NotBeNull();

    key!.Tonic.Should()
        .Be( PitchClass.EFlat );

    key.Mode.Should()
       .Be( ModeType.Major );
  }

  [Fact]
  public void TryParseWithProvider_ShouldHandleNullResult_WhenValueIsEmpty()
  {
    // Arrange
    var value = "";

    // Act
    var result = Key.TryParse( value, null, out var key );

    // Assert
    result.Should()
          .BeFalse();

    key.Should()
       .BeNull();
  }

  [Fact]
  public void TryParseWithProvider_ShouldReturnFalse_WhenPitchClassIsInvalid()
  {
    // Arrange
    var value = "X";

    // Act
    var result = Key.TryParse( value, null, out var key );

    // Assert
    result.Should()
          .BeFalse();

    key.Should()
       .BeNull();
  }

  [Fact]
  public void TryParseWithProvider_ShouldReturnFalse_WhenValueIsEmpty()
  {
    // Arrange
    var value = "";

    // Act
    var result = Key.TryParse( value, null, out var key );

    // Assert
    result.Should()
          .BeFalse();

    key.Should()
       .BeNull();
  }

  [Fact]
  public void TryParseWithProvider_ShouldReturnFalse_WhenValueIsInvalid()
  {
    // Arrange
    var value = "Invalid";

    // Act
    var result = Key.TryParse( value, null, out var key );

    // Assert
    result.Should()
          .BeFalse();

    key.Should()
       .BeNull();
  }

  [Fact]
  public void TryParseWithProvider_ShouldReturnFalse_WhenValueIsNull()
  {
    // Arrange
    string? value = null;

    // Act
    var result = Key.TryParse( value, null, out var key );

    // Assert
    result.Should()
          .BeFalse();

    key.Should()
       .BeNull();
  }

  [Fact]
  public void TryParseWithProvider_ShouldReturnFalse_WhenValueIsWhitespace()
  {
    // Arrange
    var value = "   ";

    // Act
    var result = Key.TryParse( value, null, out var key );

    // Assert
    result.Should()
          .BeFalse();

    key.Should()
       .BeNull();
  }

  [Fact]
  public void TryParseWithProvider_ShouldReturnTrue_WhenValueEndsWithM()
  {
    // Arrange
    var value = "Dm";

    // Act
    var result = Key.TryParse( value, null, out var key );

    // Assert
    result.Should()
          .BeTrue();

    key.Should()
       .NotBeNull();

    key!.Tonic.Should()
        .Be( PitchClass.D );

    key.Mode.Should()
       .Be( ModeType.Minor );
  }

  [Fact]
  public void TryParseWithProvider_ShouldReturnTrue_WhenValueHasFlat()
  {
    // Arrange
    var value = "Eb";

    // Act
    var result = Key.TryParse( value, null, out var key );

    // Assert
    result.Should()
          .BeTrue();

    key.Should()
       .NotBeNull();

    key!.Tonic.Should()
        .Be( PitchClass.EFlat );

    key.Mode.Should()
       .Be( ModeType.Major );
  }

  [Fact]
  public void TryParseWithProvider_ShouldReturnTrue_WhenValueHasWhitespace()
  {
    // Arrange
    var value = "  G  ";

    // Act
    var result = Key.TryParse( value, null, out var key );

    // Assert
    result.Should()
          .BeTrue();

    key.Should()
       .NotBeNull();

    key!.Tonic.Should()
        .Be( PitchClass.G );

    key.Mode.Should()
       .Be( ModeType.Major );
  }

  [Fact]
  public void TryParseWithProvider_ShouldReturnTrue_WhenValueIsValid()
  {
    // Arrange
    var value = "D";

    // Act
    var result = Key.TryParse( value, null, out var key );

    // Assert
    result.Should()
          .BeTrue();

    key.Should()
       .NotBeNull();

    key!.Tonic.Should()
        .Be( PitchClass.D );

    key.Mode.Should()
       .Be( ModeType.Major );
  }

  [Fact]
  public void TryParseWithProvider_ShouldReturnTrue_WhenValueIsValidWithSharp()
  {
    // Arrange
    var value = "A#";

    // Act
    var result = Key.TryParse( value, null, out var key );

    // Assert
    result.Should()
          .BeTrue();

    key.Should()
       .NotBeNull();

    key!.Tonic.Should()
        .Be( PitchClass.ASharp );

    key.Mode.Should()
       .Be( ModeType.Major );
  }

  [Fact]
  public void TryParseWithProvider_ShouldTrimWhitespace_WhenValueHasLeadingAndTrailingSpaces()
  {
    // Arrange
    var value = "   D#   ";

    // Act
    var result = Key.TryParse( value, null, out var key );

    // Assert
    result.Should()
          .BeTrue();

    key.Should()
       .NotBeNull();

    key!.Tonic.Should()
        .Be( PitchClass.DSharp );

    key.Mode.Should()
       .Be( ModeType.Major );
  }

  [Fact]
  public void TryParse_ShouldDelegateToSpanOverload_WhenCalled()
  {
    // Arrange
    var value = "G#";

    // Act
    var result = Key.TryParse( value, out var key );

    // Assert
    result.Should()
          .BeTrue();

    key.Should()
       .NotBeNull();

    key!.Tonic.Should()
        .Be( PitchClass.GSharp );

    key.Mode.Should()
       .Be( ModeType.Major );
  }

  [Fact]
  public void TryParse_ShouldReturnFalse_WhenValueIsEmpty()
  {
    // Arrange
    var value = "";

    // Act
    var result = Key.TryParse( value, out var key );

    // Assert
    result.Should()
          .BeFalse();

    key.Should()
       .BeNull();
  }

  [Fact]
  public void TryParse_ShouldReturnFalse_WhenValueIsInvalid()
  {
    // Arrange
    var value = "Invalid";

    // Act
    var result = Key.TryParse( value, out var key );

    // Assert
    result.Should()
          .BeFalse();

    key.Should()
       .BeNull();
  }

  [Fact]
  public void TryParse_ShouldReturnFalse_WhenValueIsNull()
  {
    // Arrange
    string? value = null;

    // Act
    var result = Key.TryParse( value, out var key );

    // Assert
    result.Should()
          .BeFalse();

    key.Should()
       .BeNull();
  }

  [Fact]
  public void TryParse_ShouldReturnFalse_WhenValueIsWhitespace()
  {
    // Arrange
    var value = "   ";

    // Act
    var result = Key.TryParse( value, out var key );

    // Assert
    result.Should()
          .BeFalse();

    key.Should()
       .BeNull();
  }

  [Fact]
  public void TryParse_ShouldReturnTrue_WhenValueEndsWithM()
  {
    // Arrange
    var value = "Am";

    // Act
    var result = Key.TryParse( value, out var key );

    // Assert
    result.Should()
          .BeTrue();

    key.Should()
       .NotBeNull();

    key!.Tonic.Should()
        .Be( PitchClass.A );

    key.Mode.Should()
       .Be( ModeType.Minor );
  }

  [Fact]
  public void TryParse_ShouldReturnTrue_WhenValueHasAccidental()
  {
    // Arrange
    var value = "F#";

    // Act
    var result = Key.TryParse( value, out var key );

    // Assert
    result.Should()
          .BeTrue();

    key.Should()
       .NotBeNull();

    key!.Tonic.Should()
        .Be( PitchClass.FSharp );

    key.Mode.Should()
       .Be( ModeType.Major );
  }

  [Fact]
  public void TryParse_ShouldReturnTrue_WhenValueHasFlat()
  {
    // Arrange
    var value = "Bb";

    // Act
    var result = Key.TryParse( value, out var key );

    // Assert
    result.Should()
          .BeTrue();

    key.Should()
       .NotBeNull();

    key!.Tonic.Should()
        .Be( PitchClass.BFlat );

    key.Mode.Should()
       .Be( ModeType.Major );
  }

  [Fact]
  public void TryParse_ShouldReturnTrue_WhenValueIsValid()
  {
    // Arrange
    var value = "C";

    // Act
    var result = Key.TryParse( value, out var key );

    // Assert
    result.Should()
          .BeTrue();

    key.Should()
       .NotBeNull();

    key!.Tonic.Should()
        .Be( PitchClass.C );

    key.Mode.Should()
       .Be( ModeType.Major );
  }

  #endregion
}
