//  
// Module Name: Formula.cs
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
  using System.Collections.Generic;
  using System.Collections.ObjectModel;
  using System.Diagnostics.Contracts;
  using System.Linq;
  using System.Text;

  public class Formula: INamedObject,
                        IEquatable<Formula>
  {
    #region Data Members

    private static readonly StringComparer s_comparer = StringComparer.CurrentCultureIgnoreCase;

    private readonly Interval[] _intervals;

    #endregion

    #region Construction/Destruction

    public Formula(string name, params Interval[] intervals)
    {
      Contract.Requires<ArgumentNullException>(name != null);
      Contract.Requires<ArgumentException>(name.Length > 0);
      Contract.Requires<ArgumentOutOfRangeException>(intervals.Length > 0);

      Name = name;
      _intervals = intervals;
    }

    public Formula(string name, string formula)
      : this(name, ParseIntervals(formula))
    {
    }

    #endregion

    #region Properties

    public ReadOnlyCollection<Interval> Intervals => new ReadOnlyCollection<Interval>(_intervals);

    public int Count => _intervals.Length;

    #endregion

    #region Public Methods

    public override string ToString()
    {
      var buf = new StringBuilder();
      buf.Append(Name);
      buf.Append(": ");

      var needComma = false;

      foreach( Interval interval in _intervals )
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

    public override bool Equals(object other)
    {
      if( ReferenceEquals(other, this) )
      {
        return true;
      }

      if( ReferenceEquals(other, null) || other.GetType() != GetType() )
      {
        return false;
      }

      return Equals((Formula) other);
    }

    public override int GetHashCode()
    {
      return s_comparer.GetHashCode(Name);
    }

    public IEnumerable<AbsoluteNote> Generate(AbsoluteNote root, int skipCount = 0)
    {
      int intervalCount = _intervals.Length;
      int index = skipCount;

      while( true )
      {
        Interval interval = _intervals[index % intervalCount];

        var accidentalMode = AccidentalMode.FavorSharps;
        if( interval.Quality == IntervalQuality.Diminished || interval.Quality == IntervalQuality.Minor )
        {
          accidentalMode = AccidentalMode.FavorFlats;
        }

        int octaveAdd = index / intervalCount;

        // TODO: Must ensure that enharmonic intervals are choosing the appropriate note
        AbsoluteNote note = root.Add(interval.Steps + octaveAdd * AbsoluteNote.IntervalsPerOctave, accidentalMode);
        yield return note;

        ++index;
      }
    }

    public IEnumerable<Note> Generate(Note root)
    {
      int intervalCount = _intervals.Length;
      var index = 0;

      while( true )
      {
        Interval interval = _intervals[index % intervalCount];

        var accidentalMode = AccidentalMode.FavorSharps;
        if( interval.Quality == IntervalQuality.Diminished || interval.Quality == IntervalQuality.Minor )
        {
          accidentalMode = AccidentalMode.FavorFlats;
        }

        // TODO: Must ensure that enharmonic intervals are choosing the appropriate note
        Note note = root.Add(interval.Steps, accidentalMode);
        yield return note;

        ++index;
      }
    }

    #endregion

    #region IEquatable<Formula> Members

    public bool Equals(Formula other)
    {
      if( ReferenceEquals(other, this) )
      {
        return true;
      }

      if( ReferenceEquals(other, null) )
      {
        return false;
      }

      return s_comparer.Equals(Name, other.Name) && _intervals.SequenceEqual(other.Intervals);
    }

    #endregion

    #region INamedObject Members

    public string Name { get; }

    #endregion

    #region Implementation

    private static Interval[] ParseIntervals(string formula)
    {
      Contract.Requires<ArgumentNullException>(formula != null);
      Contract.Requires<ArgumentException>(formula.Length > 0);

      var values = formula.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
      return values.Select(Interval.Parse).ToArray();
    }

    #endregion
  }
}
