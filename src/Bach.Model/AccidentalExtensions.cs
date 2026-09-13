// Module Name: AccidentalExtensions.cs
// Project:     Bach.Model
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

using Bach.Model.Internal;

namespace Bach.Model;

/// <summary>
///   Provides operations for <see cref="Accidental"/> values.
/// </summary>
public static class AccidentalExtensions
{
  #region Constants

  private static readonly string[] s_symbols =
  [
    "bb", "b", "",
    "#", "##"
  ];

  private static readonly string[] s_extendedSymbols =
  [
    Constants.UnicodeDoubleFlatAccidentalSymbol, $"{Constants.UnicodeFlatAccidentalSymbol}", "",
    $"{Constants.UnicodeSharpAccidentalSymbol}", Constants.UnicodeDoubleSharpAccidentalSymbol
  ];

  #endregion

  #region Implementation

  extension(
    Accidental accidental )
  {
    #region Public Methods

    /// <summary>Adds a number of steps to an accidental.</summary>
    /// <param name="steps">The number of steps to add.</param>
    /// <returns>An accidental.</returns>
    public Accidental Add(
      int steps )
    {
      return Validate( accidental + steps );
    }

    /// <summary>Subtracts a number of steps from an accidental.</summary>
    /// <param name="steps">The number of steps to subtract.</param>
    /// <returns>An accidental.</returns>
    public Accidental Subtract(
      int steps )
    {
      return accidental.Add( -steps );
    }

    /// <summary>Returns the ASCII symbolic representation of an accidental.</summary>
    /// <returns>The accidental symbol.</returns>
    public string ToSymbol()
    {
      Validate( accidental );

      // The index is offset by 2 to account for the negative values of double flat and flat.
      return s_symbols[(int) accidental + 2];
    }

    /// <summary>Returns the Unicode symbolic representation of an accidental.</summary>
    /// <returns>The accidental symbol.</returns>
    public string ToExtendedSymbol()
    {
      Validate( accidental );

      // The index is offset by 2 to account for the negative values of double flat and flat.
      return s_extendedSymbols[(int) accidental + 2];
    }

    #endregion
  }

  extension(
    Accidental )
  {
    #region Public Methods

    /// <summary>
    ///   Parses an accidental.
    /// </summary>
    /// <param name="value">The string representation of the accidental.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <returns>The parsed accidental.</returns>
    /// <exception cref="FormatException">Thrown when the input value is not a valid accidental.</exception>
    public static Accidental Parse(
      string? value,
      IFormatProvider? provider = null )
    {
      return Accidental.Parse( value.AsSpan(), provider );
    }

    /// <summary>
    ///   Parses an accidental.
    /// </summary>
    /// <param name="value">The string representation of the accidental.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <returns>The parsed accidental.</returns>
    /// <exception cref="FormatException">Thrown when the input value is not a valid accidental.</exception>
    public static Accidental Parse(
      ReadOnlySpan<char> value,
      IFormatProvider? provider = null )
    {
      return Accidental.TryParse( value, provider, out var accidental )
        ? accidental
        : throw new FormatException( $"{value} is not a valid accidental" );
    }

    /// <summary>
    ///   Attempts to parse an accidental.
    /// </summary>
    /// <param name="value">The string representation of the accidental.</param>
    /// <param name="accidental">The parsed accidental.</param>
    /// <returns>True if the accidental was successfully parsed; otherwise, false.</returns>
    public static bool TryParse(
      string? value,
      out Accidental accidental )
    {
      return Accidental.TryParse( value, null, out accidental );
    }

    /// <summary>
    ///   Attempts to parse an accidental.
    /// </summary>
    /// <param name="value">The string representation of the accidental.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <param name="accidental">The parsed accidental.</param>
    /// <returns>True if the accidental was successfully parsed; otherwise, false.</returns>
    public static bool TryParse(
      string? value,
      IFormatProvider? provider,
      out Accidental accidental )
    {
      return Accidental.TryParse( value.AsSpan(), provider, out accidental );
    }

    /// <summary>
    ///   Attempts to parse an accidental.
    /// </summary>
    /// <param name="value">The string representation of the accidental.</param>
    /// <param name="accidental">The parsed accidental.</param>
    /// <returns>True if the accidental was successfully parsed; otherwise, false.</returns>
    public static bool TryParse(
      ReadOnlySpan<char> value,
      out Accidental accidental )
    {
      return Accidental.TryParse( value, null, out accidental );
    }

    /// <summary>
    ///   Attempts to parse an accidental.
    /// </summary>
    /// <param name="value">The string representation of the accidental.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <param name="accidental">The parsed accidental.</param>
    /// <returns>True if the accidental was successfully parsed; otherwise, false.</returns>
    public static bool TryParse(
      ReadOnlySpan<char> value,
      IFormatProvider? provider,
      out Accidental accidental )
    {
      // Ensure the accidental was successfully parsed and the entire input was consumed.
      return Accidental.TryParse( value, provider, out accidental, out var tail ) && tail.IsEmpty;
    }

    /// <summary>
    ///   Attempts to parse an accidental and returns the remaining unparsed characters.
    /// </summary>
    /// <param name="value">The string representation of the accidental.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <param name="accidental">The parsed accidental.</param>
    /// <param name="tail">The remaining unparsed characters.</param>
    /// <returns>True if the accidental was successfully parsed; otherwise, false.</returns>
    public static bool TryParse(
      ReadOnlySpan<char> value,
      IFormatProvider? provider,
      out Accidental accidental,
      out ReadOnlySpan<char> tail )
    {
      // If the input is empty, default to natural and return true.
      if( value.IsEmpty )
      {
        accidental = Accidental.Natural;
        tail = ReadOnlySpan<char>.Empty;
        return true;
      }

      switch( value[0] )
      {
        case Constants.UnicodeNaturalAccidentalSymbol:
          accidental = Accidental.Natural;
          tail = value[1..];
          return true;

        case Constants.AsciiFlatAccidentalSymbol:
        case 'B':
        case Constants.UnicodeFlatAccidentalSymbol:
          if( value.Length > 1 && IsFlat( value[1] ) )
          {
            accidental = Accidental.DoubleFlat;
            tail = value[2..];
            return true;
          }

          accidental = Accidental.Flat;
          tail = value[1..];
          return value.Length == 1;

        case Constants.AsciiSharpAccidentalSymbol:
        case Constants.UnicodeSharpAccidentalSymbol:
          if( value.Length > 1 && IsSharp( value[1] ) )
          {
            accidental = Accidental.DoubleSharp;
            tail = value[2..];
            return true;
          }

          accidental = Accidental.Sharp;
          tail = value[1..];
          return value.Length == 1;

        case Constants.UnicodeDoubleAccidentalStartSurrogatePair:
          if( value.Length > 1 )
          {
            switch( value[1] )
            {
              // Check for double flat surrogate pair.
              case Constants.UnicodeDoubleAccidentalFlatSurrogatePair:
                accidental = Accidental.DoubleFlat;
                tail = value[2..];
                return true;

              // Check for double sharp surrogate pair.
              case Constants.UnicodeDoubleAccidentalSharpSurrogatePair:
                accidental = Accidental.DoubleSharp;
                tail = value[2..];
                return true;
            }
          }

          break;
      }

      accidental = Accidental.Natural;
      tail = value;
      return false;
    }

    #endregion
  }

  /// <summary>
  ///   Determines whether the specified character represents a flat accidental.
  /// </summary>
  /// <param name="value">The character to evaluate.</param>
  /// <returns>True if the character represents a flat accidental; otherwise, false.</returns>
  private static bool IsFlat(
    char value )
  {
    return value == Constants.AsciiFlatAccidentalSymbol || value == 'B' || value == Constants.UnicodeFlatAccidentalSymbol;
  }

  /// <summary>
  ///   Determines whether the specified character represents a sharp accidental.
  /// </summary>
  /// <param name="value">The character to evaluate.</param>
  /// <returns>True if the character represents a sharp accidental; otherwise, false.</returns>
  private static bool IsSharp(
    char value )
  {
    return value == Constants.AsciiSharpAccidentalSymbol || value == Constants.UnicodeSharpAccidentalSymbol;
  }

  /// <summary>
  ///   Validates that the specified accidental is within the defined range of accidental values.
  /// </summary>
  /// <param name="accidental">The accidental to validate.</param>
  /// <returns>The validated accidental.</returns>
  /// <exception cref="ArgumentOutOfRangeException">Thrown if the accidental is not within the defined range.</exception>
  private static Accidental Validate(
    Accidental accidental )
  {
    if( accidental is < Accidental.DoubleFlat or > Accidental.DoubleSharp )
    {
      throw new ArgumentOutOfRangeException( nameof( accidental ), accidental, "The accidental is not defined." );
    }

    return accidental;
  }

  #endregion
}
