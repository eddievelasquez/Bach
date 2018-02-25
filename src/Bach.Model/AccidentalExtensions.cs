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
// PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE
// OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

namespace Bach.Model
{
  using System;

  public static class AccidentalExtensions
  {
    #region Data Members

    private static readonly string[] s_symbols = { "bb", "b", "", "#", "##" };
    private static readonly int s_doubleFlatOffset = Math.Abs((int)Accidental.DoubleFlat);

    #endregion

    #region Public Methods

    public static string ToSymbol(this Accidental accidental)
    {
      Contract.Requires(accidental >= Accidental.DoubleFlat && accidental <= Accidental.DoubleSharp);
      return s_symbols[(int)accidental + s_doubleFlatOffset];
    }

    public static bool TryParse(string value,
                                out Accidental accidental)
    {
      accidental = Accidental.Natural;
      if (string.IsNullOrEmpty(value))
      {
        return true;
      }

      if (value.Length > 2)
      {
        return false;
      }

      foreach (char c in value)
      {
        switch (c)
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

    public static Accidental Parse(string value)
    {
      Accidental accidental;
      if (!TryParse(value, out accidental))
      {
        throw new FormatException($"{value} is not a valid accidental");
      }

      return accidental;
    }

    #endregion
  }
}
