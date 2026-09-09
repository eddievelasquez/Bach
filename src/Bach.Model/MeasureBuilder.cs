// Module Name: MeasureBuilder.cs
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
///   Builds immutable <see cref="Measure"/> instances.
/// </summary>
public sealed class MeasureBuilder
{
  #region Fields

  private readonly List<IPartEvent> _events;

  #endregion

  #region Constructors

  /// <summary>
  ///   Initializes an empty measure builder.
  /// </summary>
  public MeasureBuilder()
  {
    _events = [];
  }

  /// <summary>
  ///   Initializes a measure builder with the specified initial capacity.
  /// </summary>
  /// <param name="capacity">The initial capacity.</param>
  public MeasureBuilder(
    int capacity )
  {
    _events = new List<IPartEvent>( capacity );
  }

  /// <summary>
  ///   Initializes a measure builder with the specified events.
  /// </summary>
  /// <param name="events">The initial events.</param>
  public MeasureBuilder(
    IEnumerable<IPartEvent> events )
  {
    ArgumentNullException.ThrowIfNull( events );
    _events = [.. events];
  }

  #endregion

  #region Properties

  internal int Count => _events.Count;

  #endregion

  #region Public Methods

  /// <summary>
  ///   Adds one event to the measure.
  /// </summary>
  public MeasureBuilder Add(
    IPartEvent partEvent )
  {
    ArgumentNullException.ThrowIfNull( partEvent );
    _events.Add( partEvent );
    return this;
  }

  /// <summary>
  ///   Adds multiple events to the measure.
  /// </summary>
  public MeasureBuilder AddRange(
    IEnumerable<IPartEvent> events )
  {
    ArgumentNullException.ThrowIfNull( events );

    foreach( var e in events )
    {
      Add( e );
    }

    return this;
  }

  /// <summary>
  ///   Builds an immutable <see cref="Measure"/>.
  /// </summary>
  public Measure Build()
  {
    return new Measure( _events );
  }

  #endregion
}
