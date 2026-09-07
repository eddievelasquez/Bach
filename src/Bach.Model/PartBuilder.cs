// Module Name: PartBuilder.cs
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

using System.Collections.Generic;

namespace Bach.Model;

/// <summary>
///   Builds immutable <see cref="Part"/> instances from an ordered sequence of part events.
/// </summary>
public sealed class PartBuilder
{
  #region Fields

  private readonly List<IPartEvent> _events;

  #endregion

  #region Constructors

  /// <summary>
  ///   Initializes an empty part builder.
  /// </summary>
  public PartBuilder()
  {
    _events = [];
  }

  /// <summary>
  ///   Initializes a part builder with the specified initial capacity.
  /// </summary>
  /// <param name="capacity">The initial capacity of the builder.</param>
  public PartBuilder(
    int capacity )
  {
    _events = new List<IPartEvent>( capacity );
  }

  /// <summary>
  ///   Initializes a part builder with the specified events.
  /// </summary>
  /// <param name="events">The events to add to the builder.</param>
  /// <exception cref="ArgumentNullException">Thrown when <paramref name="events"/> is null.</exception>
  /// <exception cref="ArgumentException">Thrown when the collection contains a null event.</exception>
  public PartBuilder(
    IEnumerable<IPartEvent> events )
    : this()
  {
    AddRange( events );
  }

  #endregion

  #region Public Methods

  /// <summary>
  ///   Adds one event to the end of the builder.
  /// </summary>
  /// <param name="partEvent">The event to add.</param>
  /// <returns>This builder.</returns>
  /// <exception cref="ArgumentNullException">Thrown when <paramref name="partEvent"/> is null.</exception>
  public PartBuilder Add(
    IPartEvent partEvent )
  {
    ArgumentNullException.ThrowIfNull( partEvent );
    _events.Add( partEvent );
    return this;
  }

  /// <summary>
  ///   Adds multiple events to the end of the builder.
  /// </summary>
  /// <param name="events">The events to add.</param>
  /// <returns>This builder.</returns>
  /// <exception cref="ArgumentNullException">Thrown when <paramref name="events"/> is null.</exception>
  /// <exception cref="ArgumentException">Thrown when the collection contains a null event.</exception>
  public PartBuilder AddRange(
    IEnumerable<IPartEvent> events )
  {
    ArgumentNullException.ThrowIfNull( events );

    foreach( var partEvent in events )
    {
      Add( partEvent );
    }

    return this;
  }

  /// <summary>
  ///   Inserts an event at the specified position.
  /// </summary>
  /// <param name="index">The zero-based insertion index.</param>
  /// <param name="partEvent">The event to insert.</param>
  /// <returns>This builder.</returns>
  /// <exception cref="ArgumentNullException">Thrown when <paramref name="partEvent"/> is null.</exception>
  /// <exception cref="ArgumentOutOfRangeException">
  ///   Thrown when <paramref name="index"/> is outside the valid insertion
  ///   range.
  /// </exception>
  public PartBuilder Insert(
    int index,
    IPartEvent partEvent )
  {
    ArgumentNullException.ThrowIfNull( partEvent );
    _events.Insert( index, partEvent );
    return this;
  }

  /// <summary>
  ///   Creates an immutable part from the events currently in the builder.
  /// </summary>
  /// <returns>A new immutable <see cref="Part"/>.</returns>
  public Part Build()
  {
    return new Part( _events );
  }

  #endregion
}
