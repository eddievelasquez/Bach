// Module Name: ScaleFormula.cs
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
  /// <summary>A scale formula defines how the notes of a scale relate to each other.</summary>
  public class ScaleFormula: Formula
  {
    #region Constructors

    /// <summary>Constructor.</summary>
    /// <param name="key">The language-neutral key of the scale.</param>
    /// <param name="name">The localizable name of the scale.</param>
    /// <param name="intervals">The intervals that describe the relationship between the notes that
    ///                         compose the scale.</param>
    public ScaleFormula(string key,
                        string name,
                        Interval[] intervals)
      : base(key, name, intervals)
    {
    }

    /// <summary>Constructor.</summary>
    /// <param name="key">The language-neutral key of the scale.</param>
    /// <param name="name">The localizable name of the scale.</param>
    /// <param name="formula">The string representation of the formula for the scale. The formula is a
    ///                       sequence of comma-separated intervals. See
    ///                       <see cref="Interval.ToString" /> for the format of an interval.</param>
    public ScaleFormula(string key,
                        string name,
                        string formula)
      : base(key, name, formula)
    {
    }

    #endregion

    #region Public Methods

    /// <summary>Gets the relative steps in terms of semitones between the intervals that compose the scale.</summary>
    /// <returns>An array of integral semitone counts.</returns>
    public int[] GetRelativeSteps()
    {
      var steps = new int[Intervals.Count];
      var lastStep = 0;

      for( var i = 1; i < Intervals.Count; i++ )
      {
        int currentIntervalSteps = Intervals[i].SemitoneCount;
        int step = currentIntervalSteps - lastStep;
        steps[i - 1] = step;
        lastStep = currentIntervalSteps;
      }

      // Add last step between the root octave and the
      // last interval
      steps[steps.Length - 1] = 12 - lastStep;
      return steps;
    }

    #endregion
  }
}
