// Module Name: AccidentalTests.cs
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

namespace Bach.Model.Test;

public sealed class AccidentalTests
{
  #region Properties

  public static TheoryData<string?, Accidental, bool> AllAccidentals
  {
    get
    {
      var data = new TheoryData<string?, Accidental, bool>();

      foreach( var row in ValidAccidentals )
      {
        var (value, accidental, _, _, _) = row.Data;
        data.Add( value, accidental, true );
      }

      foreach( var row in InvalidAccidentals )
      {
        var (value, accidental) = row.Data;
        data.Add( value, accidental, false );
      }

      return data;
    }
  }

  public static TheoryData<string?, Accidental, string, string, string> ValidAccidentals
  {
    get
    {
      var data = new TheoryData<string?, Accidental, string, string, string>
      {
        { null, Accidental.Natural, "Natural", "", "" },
        { "", Accidental.Natural, "Natural", "", "" },
        { "♮", Accidental.Natural, "Natural", "", "" },
        { "bb", Accidental.DoubleFlat, "DoubleFlat", "bb", "𝄫" },
        { "𝄫", Accidental.DoubleFlat, "DoubleFlat", "bb", "𝄫" },
        { "b", Accidental.Flat, "Flat", "b", "♭" },
        { "♭", Accidental.Flat, "Flat", "b", "♭" },
        { "#", Accidental.Sharp, "Sharp", "#", "♯" },
        { "♯", Accidental.Sharp, "Sharp", "#", "♯" },
        { "##", Accidental.DoubleSharp, "DoubleSharp", "##", "𝄪" },
        { "𝄪", Accidental.DoubleSharp, "DoubleSharp", "##", "𝄪" }
      };

      return data;
    }
  }

  public static TheoryData<string?, Accidental> InvalidAccidentals
  {
    get
    {
      var data = new TheoryData<string?, Accidental>
      {
        { "b#", Accidental.Flat },
        { "#b", Accidental.Sharp },
        { "bbb", Accidental.DoubleFlat },
        { "###", Accidental.DoubleSharp },
        { "$", Accidental.Natural }
      };

      return data;
    }
  }

  public static TheoryData<Accidental, int, Accidental> ValidAdditionData { get; } = new()
  {
    { Accidental.DoubleFlat, 0, Accidental.DoubleFlat },
    { Accidental.DoubleFlat, 1, Accidental.Flat },
    { Accidental.DoubleFlat, 2, Accidental.Natural },
    { Accidental.DoubleFlat, 3, Accidental.Sharp },
    { Accidental.DoubleFlat, 4, Accidental.DoubleSharp },
    { Accidental.Flat, 0, Accidental.Flat },
    { Accidental.Flat, 1, Accidental.Natural },
    { Accidental.Flat, 2, Accidental.Sharp },
    { Accidental.Flat, 3, Accidental.DoubleSharp },
    { Accidental.Natural, 0, Accidental.Natural },
    { Accidental.Natural, 1, Accidental.Sharp },
    { Accidental.Natural, 2, Accidental.DoubleSharp },
    { Accidental.Sharp, 0, Accidental.Sharp },
    { Accidental.Sharp, 1, Accidental.DoubleSharp },
    { Accidental.DoubleSharp, 0, Accidental.DoubleSharp },
    { Accidental.DoubleSharp, -1, Accidental.Sharp },
    { Accidental.DoubleSharp, -2, Accidental.Natural },
    { Accidental.DoubleSharp, -3, Accidental.Flat },
    { Accidental.DoubleSharp, -4, Accidental.DoubleFlat },
    { Accidental.Sharp, -1, Accidental.Natural },
    { Accidental.Sharp, -2, Accidental.Flat },
    { Accidental.Sharp, -3, Accidental.DoubleFlat },
    { Accidental.Natural, -1, Accidental.Flat },
    { Accidental.Natural, -2, Accidental.DoubleFlat },
    { Accidental.Flat, -1, Accidental.DoubleFlat }
  };

  public static TheoryData<Accidental, int> InvalidAdditionData { get; } = new()
  {
    { Accidental.DoubleFlat, 5 },
    { Accidental.Flat, 4 },
    { Accidental.Natural, 3 },
    { Accidental.Sharp, 2 },
    { Accidental.DoubleSharp, 1 },
    { Accidental.DoubleSharp, -5 },
    { Accidental.Sharp, -4 },
    { Accidental.Natural, -3 },
    { Accidental.Flat, -2 },
    { Accidental.DoubleFlat, -1 }
  };

  public static TheoryData<Accidental, int, Accidental> ValidSubtractionData { get; } = new()
  {
    { Accidental.DoubleFlat, 0, Accidental.DoubleFlat },
    { Accidental.DoubleFlat, -1, Accidental.Flat },
    { Accidental.DoubleFlat, -2, Accidental.Natural },
    { Accidental.DoubleFlat, -3, Accidental.Sharp },
    { Accidental.DoubleFlat, -4, Accidental.DoubleSharp },
    { Accidental.Flat, 0, Accidental.Flat },
    { Accidental.Flat, -1, Accidental.Natural },
    { Accidental.Flat, -2, Accidental.Sharp },
    { Accidental.Flat, -3, Accidental.DoubleSharp },
    { Accidental.Natural, 0, Accidental.Natural },
    { Accidental.Natural, -1, Accidental.Sharp },
    { Accidental.Natural, -2, Accidental.DoubleSharp },
    { Accidental.Sharp, 0, Accidental.Sharp },
    { Accidental.Sharp, -1, Accidental.DoubleSharp },
    { Accidental.DoubleSharp, 0, Accidental.DoubleSharp },
    { Accidental.DoubleSharp, 1, Accidental.Sharp },
    { Accidental.DoubleSharp, 2, Accidental.Natural },
    { Accidental.DoubleSharp, 3, Accidental.Flat },
    { Accidental.DoubleSharp, 4, Accidental.DoubleFlat },
    { Accidental.Sharp, 1, Accidental.Natural },
    { Accidental.Sharp, 2, Accidental.Flat },
    { Accidental.Sharp, 3, Accidental.DoubleFlat },
    { Accidental.Natural, 1, Accidental.Flat },
    { Accidental.Natural, 2, Accidental.DoubleFlat },
    { Accidental.Flat, 1, Accidental.DoubleFlat }
  };

  public static TheoryData<Accidental, int> InvalidSubtractionData { get; } = new()
  {
    { Accidental.DoubleFlat, -5 },
    { Accidental.Flat, -4 },
    { Accidental.Natural, -3 },
    { Accidental.Sharp, -2 },
    { Accidental.DoubleSharp, -1 },
    { Accidental.DoubleSharp, 5 },
    { Accidental.Sharp, 4 },
    { Accidental.Natural, 3 },
    { Accidental.Flat, 2 },
    { Accidental.DoubleFlat, 1 }
  };

  public static TheoryData<string?, Accidental> ParseStringData { get; } = new()
  {
    { null, Accidental.Natural },
    { "", Accidental.Natural },
    { "b", Accidental.Flat },
    { "bb", Accidental.DoubleFlat },
    { "#", Accidental.Sharp },
    { "##", Accidental.DoubleSharp },
    { "♭", Accidental.Flat },
    { "♯", Accidental.Sharp },
    { "𝄫", Accidental.DoubleFlat },
    { "𝄪", Accidental.DoubleSharp }
  };

  public static TheoryData<string, Accidental> ParseStringWithProviderData { get; } = new()
  {
    { "", Accidental.Natural },
    { "b", Accidental.Flat },
    { "bb", Accidental.DoubleFlat },
    { "#", Accidental.Sharp },
    { "##", Accidental.DoubleSharp }
  };

  public static TheoryData<Accidental, string> SymbolData { get; } = new()
  {
    { Accidental.DoubleFlat, "bb" },
    { Accidental.Flat, "b" },
    { Accidental.Natural, "" },
    { Accidental.Sharp, "#" },
    { Accidental.DoubleSharp, "##" }
  };

  public static TheoryData<Accidental, string> NameData { get; } = new()
  {
    { Accidental.DoubleFlat, "DoubleFlat" },
    { Accidental.Flat, "Flat" },
    { Accidental.Natural, "Natural" },
    { Accidental.Sharp, "Sharp" },
    { Accidental.DoubleSharp, "DoubleSharp" }
  };

  public static TheoryData<string?, Accidental> TryParseSuccessData { get; } = new()
  {
    { null, Accidental.Natural },
    { "", Accidental.Natural },
    { "b", Accidental.Flat },
    { "bb", Accidental.DoubleFlat },
    { "#", Accidental.Sharp },
    { "##", Accidental.DoubleSharp },
    { "♭", Accidental.Flat },
    { "♯", Accidental.Sharp },
    { "𝄫", Accidental.DoubleFlat },
    { "𝄪", Accidental.DoubleSharp }
  };

  public static TheoryData<Accidental, string> ExtendedSymbolData { get; } = new()
  {
    { Accidental.DoubleFlat, "𝄫" },
    { Accidental.Flat, "♭" },
    { Accidental.Natural, "" },
    { Accidental.Sharp, "♯" },
    { Accidental.DoubleSharp, "𝄪" }
  };

  #endregion

  #region Public Methods

  [Theory]
  [MemberData( nameof( ValidAdditionData ) )]
  public void Add_ShouldSucceed(
    Accidental accidental,
    int increment,
    Accidental expectedAccidental )
  {
    accidental.Add( increment )
              .Should()
              .Be( expectedAccidental );
  }

  [Theory]
  [MemberData( nameof( InvalidAdditionData ) )]
  public void Add_ShouldThrowArgumentOutOfRange(
    Accidental accidental,
    int increment )
  {
    var act = () => accidental.Add( increment );

    act.Should()
       .Throw<ArgumentOutOfRangeException>();
  }

  [Theory]
  [MemberData( nameof( ValidSubtractionData ) )]
  public void Subtract_ShouldSucceed(
    Accidental accidental,
    int value,
    Accidental expectedAccidental )
  {
    accidental.Subtract( value )
              .Should()
              .Be( expectedAccidental );
  }

  [Fact]
  public void Parse_ReadOnlySpan_WithInvalidValue_ThrowsFormatException_WithMessageContainingValue()
  {
    // Arrange
    var input = "x";

    // Act
    Action act = () => Accidental.Parse( input.AsSpan() );

    // Assert
    act.Should()
       .Throw<FormatException>()
       .WithMessage( $"{input} is not a valid accidental" );
  }

  [Theory]
  [MemberData( nameof( ValidAccidentals ) )]
  public void Parse_ShouldSucceed_WhenValueIsValid(
    string? value,
    Accidental accidental,
    string name,
    string symbol,
    string extendedSymbol )
  {
    _ = name; // Unused parameter, but kept for clarity in the test data
    _ = symbol; // Unused parameter, but kept for clarity in the test data
    _ = extendedSymbol; // Unused parameter, but kept for clarity in the test data

    Accidental.Parse( value )
              .Should()
              .Be( accidental );
  }

  [Theory]
  [MemberData( nameof( InvalidAccidentals ) )]
  public void Parse_ShouldThrowFormatException_WhenValueIsInvalid(
    string? value,
    Accidental _ )
  {
    var act = () => Accidental.Parse( value );

    act.Should()
       .Throw<FormatException>();
  }

  [Theory]
  [MemberData( nameof( ParseStringData ) )]
  public void Parse_StringNullable_ShouldReturnExpected_WhenValid(
    string? input,
    Accidental expected )
  {
    // Act
    var result = Accidental.Parse( input );

    // Assert
    result.Should()
          .Be( expected );
  }

  [Theory]
  [MemberData( nameof( ParseStringWithProviderData ) )]
  public void Parse_StringWithProvider_ShouldReturnExpected_WhenValid(
    string input,
    Accidental expected )
  {
    // Act
    var result = Accidental.Parse( input );

    // Assert
    result.Should()
          .Be( expected );
  }

  [Theory]
  [MemberData( nameof( InvalidSubtractionData ) )]
  public void Subtract_ShouldThrowArgumentOutOfRange(
    Accidental accidental,
    int decrement )
  {
    var act = () => accidental.Subtract( decrement );

    act.Should()
       .Throw<ArgumentOutOfRangeException>();
  }

  [Theory]
  [MemberData( nameof( ExtendedSymbolData ) )]
  public void ToExtendedSymbol_ShouldReturnExpectedSymbol_ForVariousAccidentals(
    Accidental accidental,
    string expected )
  {
    // Act
    var result = accidental.ToExtendedSymbol();

    // Assert
    result.Should()
          .Be( expected );
  }

  [Theory]
  [MemberData( nameof( ValidAccidentals ) )]
  public void ToExtendedSymbol_ShouldReturnSymbol(
    string? value,
    Accidental accidental,
    string name,
    string symbol,
    string extendedSymbol )
  {
    _ = value; // Unused parameter, but kept for clarity in the test data
    _ = name; // Unused parameter, but kept for clarity in the test data
    _ = symbol; // Unused parameter, but kept for clarity in the test data

    accidental.ToExtendedSymbol()
              .Should()
              .Be( extendedSymbol );
  }

  [Theory]
  [MemberData( nameof( NameData ) )]
  public void ToString_ShouldReturnExpectedName_ForVariousAccidentals(
    Accidental accidental,
    string expected )
  {
    // Act
    var result = accidental.ToString();

    // Assert
    result.Should()
          .Be( expected );
  }

  [Theory]
  [MemberData( nameof( ValidAccidentals ) )]
  public void ToString_ShouldReturnName(
    string? value,
    Accidental accidental,
    string name,
    string symbol,
    string extendedSymbol )
  {
    _ = value; // Unused parameter, but kept for clarity in the test data
    _ = symbol; // Unused parameter, but kept for clarity in the test data
    _ = extendedSymbol; // Unused parameter, but kept for clarity in the test data

    accidental.ToString()
              .Should()
              .Be( name );
  }

  [Theory]
  [MemberData( nameof( SymbolData ) )]
  public void ToSymbol_ShouldReturnExpectedSymbol_ForVariousAccidentals(
    Accidental accidental,
    string expected )
  {
    // Act
    var result = accidental.ToSymbol();

    // Assert
    result.Should()
          .Be( expected );
  }

  [Theory]
  [MemberData( nameof( ValidAccidentals ) )]
  public void ToSymbol_ShouldReturnSymbol(
    string? value,
    Accidental accidental,
    string name,
    string symbol,
    string extendedSymbol )
  {
    _ = value; // Unused parameter, but kept for clarity in the test data
    _ = name; // Unused parameter, but kept for clarity in the test data
    _ = extendedSymbol; // Unused parameter, but kept for clarity in the test data

    accidental.ToSymbol()
              .Should()
              .Be( symbol );
  }

  [Fact]
  public void TryParse_ReadOnlySpanOut_ShouldReturnFalseAndFlat_WhenPartialFlat()
  {
    // Act
    var ok = Accidental.TryParse( "bX".AsSpan(), out var accidental );

    // Assert
    ok.Should()
      .BeFalse();

    accidental.Should()
              .Be( Accidental.Flat );
  }

  [Fact]
  public void TryParse_ReadOnlySpanOut_ShouldReturnTrueAndNatural_WhenEmpty()
  {
    // Act
    var ok = Accidental.TryParse( ReadOnlySpan<char>.Empty, out var accidental );

    // Assert
    ok.Should()
      .BeTrue();

    accidental.Should()
              .Be( Accidental.Natural );
  }

  [Fact]
  public void TryParse_ReadOnlySpanWithTail_ShouldHandleSurrogateDoubleAccidentals_WhenPresent()
  {
    // Arrange
    var doubleFlat = "\uD834\uDD2BZ"; // U+1D12B (high surrogate U+D834, low U+DD2B)
    var doubleSharp = "\uD834\uDD2AW"; // U+1D12A (high surrogate U+D834, low U+DD2A)

    // Act
    var okDF = Accidental.TryParse( doubleFlat.AsSpan(), null, out var accDF, out var tailDF );
    var okDS = Accidental.TryParse( doubleSharp.AsSpan(), null, out var accDS, out var tailDS );

    // Assert
    okDF.Should()
        .BeTrue();

    accDF.Should()
         .Be( Accidental.DoubleFlat );

    tailDF.ToString()
          .Should()
          .Be( "Z" );

    okDS.Should()
        .BeTrue();

    accDS.Should()
         .Be( Accidental.DoubleSharp );

    tailDS.ToString()
          .Should()
          .Be( "W" );
  }

  [Fact]
  public void TryParse_ReadOnlySpanWithTail_ShouldReturnFalseAndNatural_WhenNoMatch()
  {
    // Arrange
    var input = "x";

    // Act
    var ok = Accidental.TryParse( input.AsSpan(), null, out var accidental, out var tail );

    // Assert
    ok.Should()
      .BeFalse();

    accidental.Should()
              .Be( Accidental.Natural );

    tail.ToString()
        .Should()
        .Be( input );
  }

  [Fact]
  public void TryParse_ReadOnlySpanWithTail_ShouldReturnFalseAndSharp_WhenPartialSharp()
  {
    // Act
    var ok = Accidental.TryParse( "#X".AsSpan(), null, out var accidental, out var tail );

    // Assert
    ok.Should()
      .BeFalse();

    accidental.Should()
              .Be( Accidental.Sharp );

    tail.ToString()
        .Should()
        .Be( "X" );
  }

  [Fact]
  public void TryParse_ReadOnlySpanWithTail_ShouldReturnTrueAndDoubleFlat_WhenAsciiDoubleFlat()
  {
    // Act
    var ok = Accidental.TryParse( "bbY".AsSpan(), null, out var accidental, out var tail );

    // Assert
    ok.Should()
      .BeTrue();

    accidental.Should()
              .Be( Accidental.DoubleFlat );

    tail.ToString()
        .Should()
        .Be( "Y" );
  }

  [Theory]
  [MemberData( nameof( AllAccidentals ) )]
  public void TryParse_ShouldProcessAnyValue(
    string? input,
    Accidental expectedAccidental,
    bool expectedResult )
  {
    var result = Accidental.TryParse( input, out var accidental );

    result.Should()
          .Be( expectedResult );

    accidental.Should()
              .Be( expectedAccidental );
  }

  [Theory]
  [MemberData( nameof( TryParseSuccessData ) )]
  public void TryParse_StringNullableOut_ShouldReturnTrueAndExpectedAccidental_WhenValid(
    string? input,
    Accidental expected )
  {
    // Act
    var ok = Accidental.TryParse( input, out var accidental );

    // Assert
    ok.Should()
      .BeTrue();

    accidental.Should()
              .Be( expected );
  }

  [Fact]
  public void TryParse_StringOut_ShouldReturnFalseAndNatural_WhenInvalid()
  {
    // Act
    var ok = Accidental.TryParse( "x", out var accidental );

    // Assert
    ok.Should()
      .BeFalse();

    accidental.Should()
              .Be( Accidental.Natural );
  }

  [Fact]
  public void TryParse_StringOut_ShouldReturnFalseAndPartialAccidental_WhenPartialMatch()
  {
    // Act
    var okFlat = Accidental.TryParse( "bX", out var accidentalFlat );
    var okSharp = Accidental.TryParse( "#X", out var accidentalSharp );

    // Assert
    okFlat.Should()
          .BeFalse();

    accidentalFlat.Should()
                  .Be( Accidental.Flat );

    okSharp.Should()
           .BeFalse();

    accidentalSharp.Should()
                   .Be( Accidental.Sharp );
  }

  [Fact]
  public void TryParse_StringWithProviderOut_ShouldRespectValueAndIgnoreProvider()
  {
    // Arrange
    var provider = CultureInfo.InvariantCulture;

    // Act
    var ok = Accidental.TryParse( "##", provider, out var accidental );

    // Assert
    ok.Should()
      .BeTrue();

    accidental.Should()
              .Be( Accidental.DoubleSharp );
  }

  [Fact]
  public void ToSymbol_ShouldThrowArgumentOutOfRange_WhenValueIsUndefined()
  {
    var act = () => ( (Accidental) 99 ).ToSymbol();

    act.Should()
       .Throw<ArgumentOutOfRangeException>();
  }

  #endregion
}
