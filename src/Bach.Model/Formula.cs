﻿// Module Name: Formula.cs
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
  using System.Collections.ObjectModel;
  using System.Linq;
  using System.Text;
  using Internal;

  public class Formula
    : IKeyedObject,
      IEquatable<Formula>
  {
    #region Constants

    private static readonly StringComparer s_comparer = StringComparer.CurrentCultureIgnoreCase;

    #endregion

    #region Data Members

    private readonly Interval[] _intervals;

    #endregion

    #region Constructors

    public Formula(string key,
                   string name,
                   Interval[] intervals)
    {
      Contract.RequiresNotNullOrEmpty(key, "Must provide a key");
      Contract.RequiresNotNullOrEmpty(name, "Must provide a name");
      Contract.Requires<ArgumentNullException>(intervals != null, "Must provide an interval array");
      Contract.Requires<ArgumentOutOfRangeException>(intervals.Length > 0, "Must provide at least one interval");

      Key = key;
      Name = name;
      _intervals = intervals;
    }

    public Formula(string key,
                   string name,
                   string formula)
      : this(key, name, ParseIntervals(formula))
    {
    }

    #endregion

    #region Properties

    public ReadOnlyCollection<Interval> Intervals => new ReadOnlyCollection<Interval>(_intervals);

    public int IntervalCount => _intervals.Length;

    public string Name { get; }

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

      return s_comparer.Equals(Key, other.Key) && s_comparer.Equals(Name, other.Name) && _intervals.SequenceEqual(other.Intervals);
    }

    #endregion

    #region IKeyedObject Members

    public string Key { get; }

    #endregion

    #region Public Methods

    public IEnumerable<Pitch> Generate(Pitch root,
                                       int skipCount = 0)
    {
      int intervalCount = _intervals.Length;
      int index = skipCount;

      while( true )
      {
        Interval interval = _intervals[index % intervalCount];
        Pitch pitch = root + interval;

        // Do we need to change the pitch's octave?
        int octaveAdd = index / intervalCount;
        if( octaveAdd > 0 )
        {
          pitch += octaveAdd * Pitch.IntervalsPerOctave;
        }

        if (pitch > Pitch.MaxValue)
        {
          yield break;
        }

        yield return pitch;
        ++index;
      }
    }

    public IEnumerable<Note> Generate(Note root)
    {
      int intervalCount = _intervals.Length;
      var index = 0;

      // NOTE: By design, this iterator never returns.
      while( true )
      {
        Interval interval = _intervals[index % intervalCount];
        Note note = root + interval;
        yield return note;

        ++index;
      }

      // ReSharper disable once IteratorNeverReturns
    }

    #endregion

    #region Overrides

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

      return Equals((Formula)other);
    }

    public override int GetHashCode() => s_comparer.GetHashCode(Key);

    #endregion

    #region  Implementation

    private static Interval[] ParseIntervals(string formula)
    {
      Contract.RequiresNotNullOrEmpty(formula, "Must provide a formula");

      string[] values = formula.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
      return values.Select(Interval.Parse).ToArray();
    }

    #endregion
  }
}
