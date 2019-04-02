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
  using System;
  using System.Collections.ObjectModel;

  /// <summary>A scale formula defines how the notes of a scale relate to each other.</summary>
  public class ScaleFormula: Formula
  {
    #region Nested type: Category

    [Flags]
    private enum Category
    {
      None = 0,
      Diatonic = 1,
      Major = 2,
      Minor = 4
    }

    #endregion

    #region Data Members

    private readonly Category _categories;

    #endregion

    #region Constructors

    /// <summary>Constructor.</summary>
    /// <param name="key">The language-neutral key of the scale.</param>
    /// <param name="name">The localizable name of the scale.</param>
    /// <param name="intervals">
    ///   The intervals that describe the relationship between the notes that
    ///   compose the scale.
    /// </param>
    public ScaleFormula(string key,
                        string name,
                        params Interval[] intervals)
      : base(key, name, intervals)
    {
      _categories = Categorize(this);
    }

    /// <summary>Constructor.</summary>
    /// <param name="key">The language-neutral key of the scale.</param>
    /// <param name="name">The localizable name of the scale.</param>
    /// <param name="formula">
    ///   The string representation of the formula for the scale. The formula is a
    ///   sequence of comma-separated intervals. See
    ///   <see cref="Interval.ToString" /> for the format of an interval.
    /// </param>
    public ScaleFormula(string key,
                        string name,
                        string formula)
      : base(key, name, formula)
    {
      _categories = Categorize(this);
    }

    #endregion

    #region Properties

    /// <summary>Determines if this formula describes a diatonic scale.</summary>
    /// <notes>A diatonic scale is one that includes 5 whole steps and 2 semitones.</notes>
    /// <value>True if diatonic, false if not.</value>
    public bool Diatonic => ( _categories & Category.Diatonic ) != 0;

    /// <summary>Determines if this formula describes a major scale.</summary>
    /// <notes>A major scale is one in which the root, third and fifth form a major triad (R,M3,5).</notes>
    /// <value>True if major, false if not.</value>
    public bool Major => ( _categories & Category.Major ) != 0;

    /// <summary>Determines if this formula describes a minor scale.</summary>
    /// <notes>A minor scale is one in which the root, third and fifth form a minor triad (R,m3,5).</notes>
    /// <value>True if minor, false if not.</value>
    public bool Minor => ( _categories & Category.Minor ) != 0;

    #endregion

    #region  Implementation

    private static Category Categorize(ScaleFormula formula)
    {
      var category = Category.None;
      if( IsDiatonic(formula) )
      {
        category |= Category.Diatonic;
      }

      if( IsMajor(formula) )
      {
        category |= Category.Major;
      }

      if( IsMinor(formula) )
      {
        category |= Category.Minor;
      }

      return category;
    }

    private static bool IsDiatonic(ScaleFormula formula)
    {
      if( formula.Intervals.Count != 7 )
      {
        return false;
      }

      var wholeSteps = 0;
      var halfSteps = 0;

      foreach( int step in formula.GetRelativeSteps() )
      {
        if( step == 2 )
        {
          ++wholeSteps;
        }
        else if( step == 1 )
        {
          ++halfSteps;
        }
      }

      return wholeSteps == 5 && halfSteps == 2;
    }

    private static bool IsMajor(ScaleFormula formula)
    {
      // Scale is minor when the root, third and fifth form a major triad (R,M3,5).
      ReadOnlyCollection<Interval> intervals = formula.Intervals;
      return intervals[0] == Interval.Unison && intervals.Contains(Interval.MajorThird) && intervals.Contains(Interval.Fifth);
    }

    private static bool IsMinor(ScaleFormula formula)
    {
      // Scale is minor when the root, third and fifth form a minor triad (R,m3,5).
      ReadOnlyCollection<Interval> intervals = formula.Intervals;
      return intervals[0] == Interval.Unison && intervals.Contains(Interval.MinorThird) && intervals.Contains(Interval.Fifth);
    }

    #endregion
  }
}
