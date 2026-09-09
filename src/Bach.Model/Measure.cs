// Module Name: Measure.cs
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

using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Bach.Model;

/// <summary>
///   An ordered immutable collection of <see cref="IPartEvent"/> values representing one measure.
/// </summary>
public sealed class Measure: IReadOnlyList<IPartEvent>
{
  #region Fields

  private readonly IReadOnlyList<IPartEvent> _events;

  #endregion

  #region Constructors

  /// <summary>
  ///   Initializes a new instance of the <see cref="Measure"/> class.
  /// </summary>
  /// <param name="events">The events belonging to the measure.</param>
  public Measure(
    IEnumerable<IPartEvent> events )
  {
    ArgumentNullException.ThrowIfNull( events );

    var array = events.ToArray();

    if( array.Length == 0 )
    {
      throw new InvalidOperationException( "A measure must contain at least one event." );
    }

    if( array.Any( e => e is null ) )
    {
      throw new ArgumentException( "The event collection contains a null event.", nameof( events ) );
    }

    _events = Array.AsReadOnly( array );
  }

  #endregion

  #region Properties

  /// <summary>
  ///   Gets the number of events in the measure.
  /// </summary>
  public int Count => _events.Count;

  /// <summary>
  ///   Gets the pitch classes contained in the measure, in event order.
  /// </summary>
  public IEnumerable<PitchClass> PitchClasses => _events.SelectMany( e => e.PitchClasses );

  /// <inheritdoc/>
  public IPartEvent this[
    int index ] => _events[index];

  #endregion

  #region Public Methods

  /// <inheritdoc/>
  public IEnumerator<IPartEvent> GetEnumerator() => _events.GetEnumerator();

  /// <inheritdoc/>
  IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

  #endregion
}
