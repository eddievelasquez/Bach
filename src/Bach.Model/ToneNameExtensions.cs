//
// Module Name: ToneNameExtensions.cs
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
  public static class ToneNameExtensions
  {
    #region Data Members

    private static readonly int[] s_intervals = { 2, // C-D
      2, // D-E
      1, // E-F
      2, // F-G
      2, // G-A
      2, // A-B
      1 // B-C
    };

    #endregion

    #region Public Methods

    public static ToneName Next(this ToneName toneName)
    {
      if( toneName == ToneName.B )
      {
        return ToneName.C;
      }

      return toneName + 1;
    }

    public static ToneName Previous(this ToneName toneName)
    {
      if( toneName == ToneName.C )
      {
        return ToneName.B;
      }

      return toneName - 1;
    }

    public static int IntervalBetween(this ToneName toneName, ToneName end)
    {
      if( toneName == end )
      {
        return 0;
      }

      var interval = 0;
      while( toneName != end )
      {
        interval += s_intervals[(int) toneName];
        toneName = (ToneName) (((int) toneName + 1) % s_intervals.Length);
      }

      return interval;
    }

    #endregion
  }
}
