// Module Name: IEnumerableExtensions.cs
// Project:     Bach.Model
// Copyright (c) 2012, 2019  Eddie Velasquez.
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

namespace Bach.Model
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using Internal;

  /// <summary>Provides extensions to IEnumerables</summary>
  public static class IEnumerableExtensions
  {
    #region Public Methods

    /// <summary>Returns the intervals that separate the provided notes.</summary>
    /// <param name="notes">The notes.</param>
    /// <returns>An intervals iterator.</returns>
    public static IEnumerable<Interval> Intervals(this IEnumerable<Note> notes)
    {
      Contract.Requires<ArgumentNullException>(notes != null);
      using( IEnumerator<Note> e = notes.GetEnumerator() )
      {
        if( !e.MoveNext() )
        {
          yield break;
        }

        Note root = e.Current;
        if( !e.MoveNext() )
        {
          yield break;
        }

        do
        {
          Note note = e.Current;
          Interval interval = root - note;
          yield return interval;
        } while( e.MoveNext() );
      }
    }

    /// <summary>Converts a sequence of intervals into a comma separated string</summary>
    /// <param name="intervals">The intervals.</param>
    /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
    public static string ToString(this IEnumerable<Interval> intervals)
    {
      var buf = new StringBuilder();
      var needComma = false;

      foreach( Interval interval in intervals )
      {
        if( needComma )
        {
          buf.Append(',');
        }
        else
        {
          needComma = true;
        }

        buf.Append(interval);
      }

      return buf.ToString();
    }

    #endregion
  }
}
