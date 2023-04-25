// Module Name: IntervalCollection.cs
// Project:     Bach.Model
// Copyright (c) 2012, 2023  Eddie Velasquez.
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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Bach.Model.Internal;

namespace Bach.Model;

public sealed class IntervalCollection
  : IReadOnlyList<Interval>,
    IEquatable<IEnumerable<Interval>>
{
#region Fields

  private readonly Interval[] _intervals;

#endregion

#region Constructors

  internal IntervalCollection( Interval[] intervals )
  {
    Requires.NotNullOrEmpty( intervals );
    Requires.Condition<ArgumentException>( intervals.IsSortedUnique(),
                                           "Intervals must be sorted and contain no duplicates" );

    _intervals = intervals;
  }

#endregion

#region Properties

  /// <inheritdoc />
  public int Count => _intervals.Length;

  /// <inheritdoc />
  public Interval this[ int index ] => _intervals[index];

#endregion

#region Public Methods

  /// <inheritdoc />
  public bool Equals( IEnumerable<Interval>? other )
  {
    if( ReferenceEquals( this, other ) )
    {
      return true;
    }

    return other is not null && _intervals.SequenceEqual( other );
  }

  /// <inheritdoc />
  public override bool Equals( object? obj )
  {
    if( ReferenceEquals( this, obj ) )
    {
      return true;
    }

    return obj is IntervalCollection other && Equals( other );
  }

  /// <inheritdoc />
  public IEnumerator<Interval> GetEnumerator()
  {
    return ( (IEnumerable<Interval>) _intervals ).GetEnumerator();
  }

  /// <inheritdoc />
  IEnumerator IEnumerable.GetEnumerator()
  {
    return GetEnumerator();
  }

  /// <inheritdoc />
  public override int GetHashCode()
  {
    var hash = new HashCode();
    foreach( var interval in _intervals )
    {
      hash.Add( interval );
    }

    return hash.ToHashCode();
  }

  /// <summary>
  ///   Searches for the specified interval using the optional provided comparer and returns the index of its
  ///   occurrence in the collection.
  /// </summary>
  /// <param name="interval">The interval to locate</param>
  /// <param name="comparer">
  ///   The optional <see cref="IComparer&lt;Interval&gt;" />  implementation to use when comparing intervals. If no
  ///   comparer is provided, the intervals will be compared using an exact match.
  /// </param>
  /// <returns>The index of the occurrence of <paramref name="interval" /> in the collection, if found; otherwise, -1.</returns>
  public int IndexOf(
    Interval interval,
    IComparer<Interval>? comparer = null )
  {
    return Array.BinarySearch( _intervals, interval, comparer ?? Comparer<Interval>.Default );
  }

  /// <inheritdoc />
  public override string ToString()
  {
    return string.Join( ",", _intervals );
  }

#endregion
}
