//
// Module Name: AccidentalExtensions.cs
// Project:     Bach.Model
// Copyright (c) 2013, 2016  Eddie Velasquez.
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
//  portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A
// PARTICULAR PURPOSE AND NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE
// OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

namespace Bach.Model
{
  using System;

  /// <summary>
  /// Provides extensions for <see cref="Accidental"/>.
  /// </summary>
  public static class AccidentalExtensions
  {
    private static readonly string[] s_symbols = { "bb", "b", "", "#", "##" };
    private static readonly int s_doubleFlatOffset = Math.Abs((int) Accidental.DoubleFlat);

    /// <summary>
    /// Converts an <see cref="Accidental"/> to its string representation.
    /// </summary>
    /// <param name="accidental">The accidental</param>
    /// <returns>String representation of the accidental.</returns>
    public static string ToSymbol(this Accidental accidental)
    {
      Contract.Requires(accidental >= Accidental.DoubleFlat && accidental <= Accidental.DoubleSharp);
      return s_symbols[(int) accidental + s_doubleFlatOffset];
    }

    /// <summary>
    /// Converts the specified string representation of an accidental to its <see cref="Accidental"/> equivalent
    /// and returns a value that indicates whether the conversion succeeded.
    /// </summary>
    /// <param name="value">A string containing the accidental to convert.</param>
    /// <param name="accidental">When this method returns, contains the Accidental value equivalent to accidental
    /// contained in value, if the conversion succeeded, or Natural if the conversion failed.
    /// The conversion fails if the s parameter is longer than 2 characters or does not contain a valid string
    /// representation of an accidental. This parameter is passed uninitialized.</param>
    /// <returns><see langword="true"/> if the value parameter was converted successfully; otherwise, <see langword="false"/>.</returns>
    public static bool TryParse(string value,
                                out Accidental accidental)
    {
      accidental = Accidental.Natural;
      if( string.IsNullOrEmpty(value) )
      {
        return true;
      }

      if( value.Length > 2 )
      {
        return false;
      }

      foreach( char c in value )
      {
        switch( c )
        {
          case 'b':
          case 'B':
            --accidental;
            break;

          case '#':
            ++accidental;
            break;

          default:
            return false;
        }
      }

      // Cannot be natural unless the "b#" or "#b" combinations are found
      return accidental != Accidental.Natural;
    }

    /// <summary>
    /// Converts the specified string representation of an accidental to its <see cref="Accidental"/> equivalent.
    /// </summary>
    /// <param name="value">A string containing the accidental to convert.</param>
    /// <returns>An object that is equivalent to the accidental contained in value.</returns>
    /// <exception cref="FormatException">value does not contain a valid string representation of an accidental.</exception>
    public static Accidental Parse(string value)
    {
      if( !TryParse(value, out Accidental accidental) )
      {
        throw new FormatException($"{value} is not a valid accidental");
      }

      return accidental;
    }
  }
}
