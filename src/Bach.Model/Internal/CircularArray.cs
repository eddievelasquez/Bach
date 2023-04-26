// Module Name: CircularArray.cs
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
using System.Diagnostics;
using System.Linq;

namespace Bach.Model.Internal;

/// <summary>
///   Represents buffer that is circular.
/// </summary>
/// <typeparam name="T">The type of the buffer.</typeparam>
[DebuggerDisplay( "Count = {Length}" )]
internal sealed class CircularArray<T>: IEnumerable<T>
{
#region Fields

  private readonly IList<T> _items;
  private int _head;

#endregion

#region Constructors

  /// <summary>
  ///   Initializes a new instance <see cref="CircularArray{T}" /> class
  ///   that contains the provided items.
  /// </summary>
  public CircularArray( IEnumerable<T> items )
  {
    Requires.NotNull( items );
    _items = items.ToArray();
    _head = 0;
  }

  /// <summary>
  ///   Initializes a new instance <see cref="CircularArray{T}" /> class
  ///   that contains the provided items.
  /// </summary>
  public CircularArray( IList<T> items )
  {
    Requires.NotNull( items );
    _items = items;
    _head = 0;
  }

#endregion

#region Properties

  public int Head
  {
    get => _head;
    set => _head = _items.WrapIndex( value );
  }

  /// <summary>
  ///   Gets the number of elements actually contained in the
  ///   <see cref="CircularArray{T}" />.
  /// </summary>
  public int Length => _items.Count;

  /// <summary>
  ///   Gets or sets the element at the specified index.
  /// </summary>
  /// <value></value>
  public T this[ int index ]
  {
    get
    {
      Requires.Between( index, 0, Length - 1 );
      return GetElement( index );
    }
    set
    {
      Requires.Between( index, 0, Length - 1 );
      SetElement( index, value );
    }
  }

#endregion

#region Public Methods

  /// <inheritdoc />
  public IEnumerator<T> GetEnumerator()
  {
    for( var index = 0; index < Length; ++index )
    {
      var value = GetElement( index );
      yield return value;
    }
  }

  /// <inheritdoc />
  IEnumerator IEnumerable.GetEnumerator()
  {
    return GetEnumerator();
  }

#endregion

#region Implementation

  private T GetElement( int index )
  {
    var position = ( _head + index ) % Length;
    if( position < 0 )
    {
      throw new ArgumentOutOfRangeException( nameof( index ) );
    }

    return _items[position];
  }

  private void SetElement(
    int index,
    T value )
  {
    var position = ( _head + index ) % Length;
    if( position < 0 )
    {
      throw new ArgumentOutOfRangeException( nameof( index ) );
    }

    _items[position] = value;
  }

#endregion
}
