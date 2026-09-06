// Module Name: IntervalExtensions.cs
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

using System.Collections.Generic;
using System.Linq;

namespace Bach.Model;

/// <summary>Provides common extensions.</summary>
public static class IntervalExtensions
{
  #region Implementation

  /// <param name="notes">The notes to parse.</param>
  extension(
    IEnumerable<string> notes )
  {
    #region Public Methods

    /// <summary>
    ///   Returns the intervals that separate the provided notes.
    /// </summary>
    /// <returns>An interval iterator.</returns>
    public IEnumerable<Interval> ParseIntervals()
    {
      var list = notes.Select( PitchClass.Parse );
      return list.CalculateIntervals();
    }

    #endregion
  }

  /// <param name="pitchClasses">The pitch classes.</param>
  extension(
    IEnumerable<PitchClass> pitchClasses )
  {
    #region Public Methods

    /// <summary>Returns the intervals that separate the provided pitch classes.</summary>
    /// <returns>An interval iterator.</returns>
    public IEnumerable<Interval> CalculateIntervals()
    {
      ArgumentNullException.ThrowIfNull( pitchClasses );

      // Use the enumerator directly to avoid multiple enumerations of the source collection.
      using var e = pitchClasses.GetEnumerator();

      // Unison is always the first interval, as it represents the distance from a note to itself.
      yield return Interval.Unison;

      // If there are no pitch classes, or only one, we cannot calculate any intervals, so we exit early.
      if( !e.MoveNext() )
      {
        yield break;
      }

      // The first pitch class is the root. Calculate all later intervals relative to this root.
      var root = e.Current;

      // If there are no more pitch classes after the root, we cannot calculate any intervals, so we exit early.
      if( !e.MoveNext() )
      {
        yield break;
      }

      do
      {
        // Calculate the interval from the root to the current pitch class and yield it.
        var pitchClass = e.Current;
        var interval = root.GetIntervalTo( pitchClass );
        yield return interval;
      } while( e.MoveNext() );
    }

    #endregion
  }

  #endregion
}
