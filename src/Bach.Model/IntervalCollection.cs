// Module Name: IntervalCollection.cs
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
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;
  using Internal;

  public class IntervalCollection
    : IReadOnlyList<Interval>,
      IEquatable<IEnumerable<Interval>>
  {
    #region Data Members

    private readonly Interval[] _intervals;

    #endregion

    #region Constructors

    internal IntervalCollection(Interval[] intervals)
    {
      Contract.Requires<ArgumentNullException>(intervals != null);
      Contract.Requires<ArgumentException>(intervals.Length > 0);
      Contract.Requires<ArgumentException>(intervals.IsSortedUnique());

      _intervals = intervals;
    }

    #endregion

    #region IEquatable<IEnumerable<Interval>> Members

    /// <inheritdoc />
    public bool Equals(IEnumerable<Interval> other)
    {
      if( ReferenceEquals(this, other) )
      {
        return true;
      }

      if( other is null )
      {
        return false;
      }

      return _intervals.SequenceEqual(other);
    }

    #endregion

    #region IReadOnlyList<Interval> Members

    /// <inheritdoc />
    public IEnumerator<Interval> GetEnumerator() => ( (IEnumerable<Interval>)_intervals ).GetEnumerator();

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <inheritdoc />
    public int Count => _intervals.Length;

    /// <inheritdoc />
    public Interval this[int index] => _intervals[index];

    #endregion

    #region Public Methods

    /// <summary>
    ///   Searches for the specified interval using the optional provided comparer and returns the index of its
    ///   occurence in the collection.
    /// </summary>
    /// <param name="interval">The interval to locate</param>
    /// <param name="comparer">
    ///   The optional <see cref="IComparer&lt;Interval&gt;" />  implementation to use when comparing intervals. If no
    ///   comparer is provided, the intervals will be compared using an exact match.
    /// </param>
    /// <returns>The index of the occurence of <paramref name="interval" /> in the collection, if found; otherwise, -1.</returns>
    public int IndexOf(Interval interval,
                       IComparer<Interval> comparer = null)
      => Array.BinarySearch(_intervals, interval, comparer ?? Comparer<Interval>.Default);

    #endregion

    #region Overrides

    /// <inheritdoc />
    public override bool Equals(object obj)
    {
      if( ReferenceEquals(this, obj) )
      {
        return true;
      }

      return obj is IntervalCollection other && Equals(other);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
      unchecked
      {
        var hash = 17;
        foreach( Interval interval in _intervals )
        {
          hash = ( hash * 23 ) + interval.GetHashCode();
        }

        return hash;
      }
    }

    /// <inheritdoc />
    public override string ToString() => string.Join(",", _intervals);

    #endregion
  }
}
