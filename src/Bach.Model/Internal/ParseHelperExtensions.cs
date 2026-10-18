// Module Name: ParseHelperExtensions.cs
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

namespace Bach.Model.Internal;

internal static class ParseHelperExtensions
{
  #region Implementation

  /// <param name="c">The character to check.</param>
  extension(
    char c )
  {
    #region Public Methods

    /// <summary>
    ///   Determines whether the specified character is a valid chord symbol character.
    /// </summary>
    /// <returns>true if the character is a valid chord symbol character; otherwise, false.</returns>
    public bool IsChordSymbolChar()
    {
      // Digits
      if( (uint) ( c - '0' ) <= 9 )
      {
        return true;
      }

      // ASCII letters
      if( (uint) ( ( c | 0x20 ) - 'a' ) <= 25 )
      {
        return true;
      }

      // Common ASCII symbols
      return c switch
      {
        Constants.ClassicalAugmentedIntervalQualitySymbol
          or Constants.AsciiSharpAccidentalSymbol
          or Constants.AsciiFlatAccidentalSymbol
          or '('
          or ')' => true,

        // Unicode accidentals + half-diminished
        _ => c switch
        {
          Constants.UnicodeSharpAccidentalSymbol => true,
          Constants.UnicodeFlatAccidentalSymbol  => true,
          Constants.UnicodeDiminishedChordSymbol => true,
          _                                      => false
        }
      };
    }

    #endregion
  }

  /// <param name="span">The span to search.</param>
  extension(
    ReadOnlySpan<char> span )
  {
    #region Public Methods

    /// <summary>
    ///   Finds the index of the first character in the span that is not a valid chord symbol character.
    /// </summary>
    /// <returns>The index of the first non-chord symbol character, or -1 if none is found.</returns>
    public int IndexOfNonChordSymbol()
    {
      for( var i = 0; i < span.Length; i++ )
      {
        if( !span[i]
             .IsChordSymbolChar() )
        {
          return i;
        }
      }

      return -1;
    }

    #endregion
  }

  #endregion
}
