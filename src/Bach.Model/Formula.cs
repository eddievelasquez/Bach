// Module Name: Formula.cs
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

  /// <summary>A formula is a base class for constructing a sequence of notes based on a series of intervals.</summary>
  public abstract class Formula
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

    /// <summary>Specialized constructor for use only by derived classes.</summary>
    /// <exception cref="ArgumentNullException">Thrown when either the key, name or interval arguments are null.</exception>
    /// <exception cref="ArgumentException">Thrown when teh key or name are empty.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when interval array is empty.</exception>
    /// <param name="key">
    ///   The language-neutral key for the formula. The key is used as the unique identifier for a formula in
    ///   the registry.
    /// </param>
    /// <param name="name">The localizable name for the formula.</param>
    /// <param name="intervals">The intervals that compose the formula.</param>
    protected Formula(string key,
                      string name,
                      Interval[] intervals)
    {
      Contract.RequiresNotNullOrEmpty(key, "Must provide a key");
      Contract.RequiresNotNullOrEmpty(name, "Must provide a name");
      Contract.Requires<ArgumentNullException>(intervals != null, "Must provide an interval array");
      Contract.Requires<ArgumentOutOfRangeException>(intervals.Length > 0, "Must provide at least one interval");
      Contract.Requires<ArgumentException>(intervals.IsSortedWithoutDuplicates());

      Key = key;
      Name = name;
      _intervals = intervals;
    }

    /// <summary>Specialized constructor for use only by derived classed.</summary>
    /// <param name="key">
    ///   The language-neutral key for the formula. The key is used as the unique
    ///   identifier for a formula in the registry.
    /// </param>
    /// <param name="name">The localizable name for the formula.</param>
    /// <param name="formula">
    ///   The string representation of the formula for the scale. The formula is a
    ///   sequence of comma-separated intervals. See
    ///   <see cref="Interval.ToString" /> for the format of an interval.
    /// </param>
    protected Formula(string key,
                      string name,
                      string formula)
      : this(key, name, ParseIntervals(formula))
    {
    }

    #endregion

    #region Properties

    /// <summary>Gets the intervals that compose this formula.</summary>
    /// <value>The intervals.</value>
    public ReadOnlyCollection<Interval> Intervals => new ReadOnlyCollection<Interval>(_intervals);

    /// <summary>Gets the localizable name for the formula.</summary>
    /// <value>The name.</value>
    public string Name { get; }

    #endregion

    #region IEquatable<Formula> Members

    /// <inheritdoc />
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

    /// <summary>Returns the language-neutral key for the formula.</summary>
    /// <value>The key.</value>
    public string Key { get; }

    #endregion

    #region Public Methods

    /// <summary>Generates a sequence of pitches based on the formula's intervals.</summary>
    /// <param name="root">The root pitch.</param>
    /// <param name="skipCount">(Optional) Number of pitches to skip.</param>
    /// <returns> An enumerator for a sequence of pitches.</returns>
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

        if( pitch > Pitch.MaxValue )
        {
          yield break;
        }

        yield return pitch;

        ++index;
      }
    }

    /// <summary>Generates a sequence of notes based on the formula's intervals.</summary>
    /// <notes>Warning! By design, this enumerator never ends.</notes>
    /// <param name="root">The root note.</param>
    /// <returns> An enumerator for a sequence of notes.</returns>
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

    /// <inheritdoc />
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

    /// <inheritdoc />
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

    /// <inheritdoc />
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
