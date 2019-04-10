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
    #region Nested type: IntervalComparer

    private class IntervalComparer: IComparer<Interval>
    {
      #region IComparer<Interval> Members

      public int Compare(Interval x,
                         Interval y)
        => x.CompareTo(y);

      #endregion
    }

    #endregion

    #region Nested type: SemitoneCountIntervalComparer

    private class SemitoneCountIntervalComparer: IComparer<Interval>
    {
      #region IComparer<Interval> Members

      public int Compare(Interval x,
                         Interval y)
        => x.SemitoneCount - y.SemitoneCount;

      #endregion
    }

    #endregion

    #region Constants

    private const string DefaultToStringFormat = "N: I";

    private static readonly StringComparer s_comparer = StringComparer.CurrentCultureIgnoreCase;
    private static readonly IntervalComparer s_intervalComparer = new IntervalComparer();
    private static readonly SemitoneCountIntervalComparer s_semitoneComparer = new SemitoneCountIntervalComparer();

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

    /// <summary>Determines whether this instance contains the provided intervals.</summary>
    /// <param name="intervals">The intervals to evaluate.</param>
    /// <param name="match">Interval matching strategy.</param>
    /// <returns>
    ///   <c>true</c> if the formula contains the specified intervals; otherwise, <c>false</c>.
    /// </returns>
    public bool Contains(IEnumerable<Interval> intervals,
                         IntervalMatch match = IntervalMatch.Exact)
    {
      IComparer<Interval> comparer = ( match == IntervalMatch.Exact ) ? (IComparer<Interval>)s_intervalComparer : s_semitoneComparer;
      return intervals.All(interval => Array.BinarySearch(_intervals, interval, comparer) >= 0);
    }

    /// <summary>Generates a sequence of pitches based on the formula's intervals.</summary>
    /// <param name="root">The root pitch.</param>
    /// <returns> An enumerator for a sequence of pitches.</returns>
    public IEnumerable<Pitch> Generate(Pitch root)
    {
      int intervalCount = _intervals.Length;
      var index = 0;

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

    /// <summary>Gets the relative steps in terms of semitones between the intervals that compose the formula.</summary>
    /// <returns>An array of integral semitone counts.</returns>
    public int[] GetRelativeSteps()
    {
      var steps = new int[_intervals.Length];
      var lastCount = 0;

      for( var i = 1; i < _intervals.Length; i++ )
      {
        int semitoneCount = _intervals[i].SemitoneCount;
        int step = semitoneCount - lastCount;
        steps[i - 1] = step;
        lastCount = semitoneCount;
      }

      // Add last step between the root octave and the
      // last interval
      steps[steps.Length - 1] = 12 - lastCount;
      return steps;
    }

    /// <summary>
    ///   Returns a string representation of the value of this <see cref="Formula" /> instance, according to the
    ///   provided format specifier.
    /// </summary>
    /// <param name="format">A custom format string.</param>
    /// <returns>
    ///   A string representation of the value of the current <see cref="Formula" /> object as specified by
    ///   <paramref name="format" />.
    /// </returns>
    /// <remarks>
    ///   <para>Format specifiers:</para>
    ///   <para>"N": Name pattern. e.g. "Major".</para>
    ///   <para>"I": Intervals pattern. e.g. "P1,M3,P5".</para>
    /// </remarks>
    public string ToString(string format) => ToString(format, null);

    /// <summary>
    ///   Returns a string representation of the value of this <see cref="Formula" /> instance, according to the
    ///   provided format specifier and format provider.
    /// </summary>
    /// <param name="format">A custom format string.</param>
    /// <param name="provider">The format provider. (Currently unused)</param>
    /// <returns>
    ///   A string representation of the value of the current <see cref="Formula" /> object as specified by
    ///   <paramref name="format" />.
    /// </returns>
    /// <remarks>
    ///   <para>Format specifiers:</para>
    ///   <para>"N": Name pattern. e.g. "Major".</para>
    ///   <para>"I": Intervals pattern. e.g. "P1,M3,P5".</para>
    /// </remarks>
    public string ToString(string format,
                           IFormatProvider provider)
    {
      if( string.IsNullOrEmpty(format) )
      {
        format = DefaultToStringFormat;
      }

      var buf = new StringBuilder();
      foreach( char f in format )
      {
        switch( f )
        {
          case 'N':
            buf.Append(Name);
            break;

          case 'I':
            buf.Append(IEnumerableExtensions.ToString(_intervals));
            break;

          default:
            buf.Append(f);
            break;
        }
      }

      return buf.ToString();
    }

    #endregion

    #region Overrides

    /// <inheritdoc />
    public override string ToString() => ToString(DefaultToStringFormat, null);

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
