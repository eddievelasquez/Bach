// Module Name: CollectionExtensions.cs
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

namespace Bach.Model.Internal;

/// <summary>
/// Provides extension methods for collections of intervals and steps.
/// </summary>
public static class CollectionExtensions
{
  #region Implementation

  extension(
    IEnumerable<Interval> intervals )
  {
    #region Public Methods

    /// <summary>
    /// Converts a collection of intervals to a collection of steps.
    /// </summary>
    /// <returns></returns>
    public IEnumerable<int> ToSteps()
    {
      ArgumentNullException.ThrowIfNull( intervals );

      // Must call a core method to enable checking for null before the first yield
      return intervals.ToStepsCore();
    }

    private IEnumerable<int> ToStepsCore()
    {
      var lastCount = 0;

      // Skip the first interval since it is always 0, and we don't want to include it in the steps
      foreach( var interval in intervals.Skip( 1 ) )
      {
        var semitoneCount = interval.SemitoneCount;
        var step = semitoneCount - lastCount;
        yield return step;

        lastCount = semitoneCount;
      }

      // Add the final step to complete the octave
      yield return Constants.OctaveSemitoneCount - lastCount;
    }

    #endregion
  }

  extension(
    IEnumerable<int> steps )
  {
    /// <summary>
    /// Converts a collection of steps to a collection of intervals.
    /// </summary>
    /// <returns>A collection of intervals.</returns>
    public IEnumerable<Interval> ToIntervals()
    {
      ArgumentNullException.ThrowIfNull(steps);

      // Must call a core method to enable checking for null before the first yield
      return steps.ToIntervalsCore();
    }

    private IEnumerable<Interval> ToIntervalsCore()
    {
      // Unison is always the first interval
      var previous = Interval.Unison;
      yield return previous;

      // Calculate the cumulative semitone count for each step and yield the corresponding interval
      var semitones = 0;
      foreach (var step in steps)
      {
        semitones += step;
        var interval = Interval.FromSemitones( semitones );

        // If the interval is the same as the previous one, get its enharmonic equivalent
        // to avoid duplicate interval quantities
        if( interval.Quantity == previous.Quantity )
        {
          interval = interval.GetEnharmonicEquivalent();
        }

        // If the interval is an octave or greater, stop yielding intervals
        // because we only want intervals within a single octave
        if( interval >= Interval.Octave )
        {
          yield break;
        }

        yield return interval;

        previous = interval;
      }
    }
  }

  #endregion
}
