// Module Name: Part.cs
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

namespace Bach.Model;

using System.Collections;
using System.Collections.Generic;

/// <summary>A sequential collection of musical events that can contain either pitches or pitch chords.</summary>
public sealed class Part
  : IList<IPartEvent>
{
  #region Fields

  private readonly List<IPartEvent> _events;

  #endregion

  #region Constructors

  /// <summary>
  ///   Initializes a new instance of the <see cref="Part" /> class.
  /// </summary>
  public Part()
  {
    _events = [];
  }

  /// <summary>
  ///   Initializes a new instance of the <see cref="Part" /> class with the specified collection of part events.
  /// </summary>
  /// <param name="events">The collection of part events to initialize the part with.</param>
  public Part(
    IEnumerable<IPartEvent> events )
  {
    ArgumentNullException.ThrowIfNull( events );
    _events = new List<IPartEvent>( events );
  }

  /// <summary>
  ///   Initializes a new instance of the <see cref="Part" /> class with the specified initial capacity.
  /// </summary>
  /// <param name="capacity">The initial capacity of the part.</param>
  public Part(
    int capacity )
  {
    _events = new List<IPartEvent>( capacity );
  }

  #endregion

  #region Properties

  /// <inheritdoc />
  public int Count => _events.Count;

  /// <inheritdoc />
  public bool IsReadOnly => false;

  /// <inheritdoc />
  public IPartEvent this[
    int index ]
  {
    get => _events[index];
    set
    {
      ArgumentNullException.ThrowIfNull( value );
      _events[index] = value;
    }
  }

  #endregion

  #region Public Methods

  /// <summary>
  ///   Adds a musical event to the part.
  /// </summary>
  /// <param name="partEvent">The musical event to add.</param>
  public void Add(
    IPartEvent partEvent )
  {
    ArgumentNullException.ThrowIfNull( partEvent );
    _events.Add( partEvent );
  }

  /// <inheritdoc />
  public void Clear()
  {
    _events.Clear();
  }

  /// <inheritdoc />
  public bool Contains(
    IPartEvent item )
  {
    ArgumentNullException.ThrowIfNull( item );
    return _events.Contains( item );
  }

  /// <inheritdoc />
  public void CopyTo(
    IPartEvent[] array,
    int arrayIndex )
  {
    ArgumentNullException.ThrowIfNull( array );
    _events.CopyTo( array, arrayIndex );
  }

  /// <inheritdoc />
  public IEnumerator<IPartEvent> GetEnumerator()
  {
    return _events.GetEnumerator();
  }

  /// <inheritdoc />
  IEnumerator IEnumerable.GetEnumerator()
  {
    return GetEnumerator();
  }

  /// <inheritdoc />
  public int IndexOf(
    IPartEvent item )
  {
    ArgumentNullException.ThrowIfNull( item );
    return _events.IndexOf( item );
  }

  /// <inheritdoc />
  public void Insert(
    int index,
    IPartEvent item )
  {
    ArgumentNullException.ThrowIfNull( item );
    _events.Insert( index, item );
  }

  /// <inheritdoc />
  public bool Remove(
    IPartEvent item )
  {
    ArgumentNullException.ThrowIfNull( item );
    return _events.Remove( item );
  }

  /// <inheritdoc />
  public void RemoveAt(
    int index )
  {
    _events.RemoveAt( index );
  }

  #endregion
}
