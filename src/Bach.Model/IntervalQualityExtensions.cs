//
// Module Name: IntervalQualityExtensions.cs
// Project:     Bach.Model
// Copyright (c) 2016  Eddie Velasquez.
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

  public static class IntervalQualityExtensions
  {
    #region Data Members

    private static readonly string[] s_symbols = { "d", "m", "P", "M", "A" };
    private static readonly string[] s_short = { "dim", "min", "Perf", "Maj", "Aug" };
    private static readonly string[] s_long = { "diminished", "minor", "perfect", "major", "augmented" };

    #endregion

    #region Public Methods

    public static IntervalQuality Parse(string value)
    {
      if( !TryParse(value, out IntervalQuality quality) )
      {
        throw new FormatException($"\"{value}\" is not a valid interval quality");
      }

      return quality;
    }

    public static bool TryParse(string value,
                                out IntervalQuality quality)
    {
      if( !string.IsNullOrEmpty(value) )
      {
        for( var i = 0; i < s_symbols.Length; i++ )
        {
          if( !s_symbols[i].Equals(value) )
          {
            continue;
          }

          quality = (IntervalQuality) i;
          return true;
        }
      }

      quality = IntervalQuality.Unknown;
      return false;
    }

    public static string Symbol(this IntervalQuality quality)
    {
      Contract.Requires<ArgumentOutOfRangeException>(quality >= IntervalQuality.Diminished);
      Contract.Requires<ArgumentOutOfRangeException>(quality <= IntervalQuality.Augmented);

      return s_symbols[(int) quality];
    }

    public static string ShortName(this IntervalQuality quality)
    {
      Contract.Requires<ArgumentOutOfRangeException>(quality >= IntervalQuality.Diminished);
      Contract.Requires<ArgumentOutOfRangeException>(quality <= IntervalQuality.Augmented);

      return s_short[(int) quality];
    }

    public static string LongName(this IntervalQuality quality)
    {
      Contract.Requires<ArgumentOutOfRangeException>(quality >= IntervalQuality.Diminished);
      Contract.Requires<ArgumentOutOfRangeException>(quality <= IntervalQuality.Augmented);

      return s_long[(int) quality];
    }

    #endregion
  }
}
