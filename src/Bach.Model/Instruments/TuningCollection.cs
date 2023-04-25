// Module Name: TuningCollection.cs
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
using System.Diagnostics.CodeAnalysis;
using Bach.Model.Internal;
using Comparer = Bach.Model.Internal.Comparer;

namespace Bach.Model.Instruments;

/// <summary>Collection of tunings.</summary>
public sealed class TuningCollection: IReadOnlyDictionary<string, Tuning>
{
#region Fields

  private readonly string _instrumentId;
  private readonly Dictionary<string, Tuning> _tunings = new( Comparer.IdComparer );

#endregion

#region Constructors

  internal TuningCollection( string instrumentId )
  {
    _instrumentId = instrumentId;
  }

#endregion

#region Properties

  /// <summary>Gets the standard tuning.</summary>
  /// <exception cref="ArgumentOutOfRangeException">Thrown when the tuning collection is empty.</exception>
  /// <value>The standard.</value>
  public Tuning Standard
  {
    get
    {
      Requires.GreaterThan( Count, 0 );
      return this["Standard"];
    }
  }

  /// <summary>Gets the number of tunings in the collection.</summary>
  /// <value>The count.</value>
  public int Count => _tunings.Count;

  /// <inheritdoc />
  /// <summary>Gets the tuning that has the specified language-neutral id.</summary>
  /// <returns>The tuning that has the specified id in the read-only dictionary.</returns>
  public Tuning this[ string id ] => _tunings[id];

  /// <summary>
  ///   Gets an enumerable collection that contains the keys for all the tunings in the collection.
  /// </summary>
  /// <value>An enumerable collection that contains the keys.</value>
  public IEnumerable<string> Keys => _tunings.Keys;

  /// <summary>
  ///   Gets an enumerable collection that contains all the tunings in the collection.
  /// </summary>
  /// <value>An enumerable collection that contains the tunings.</value>
  public IEnumerable<Tuning> Values => _tunings.Values;

#endregion

#region Public Methods

  /// <summary>
  ///   Determines whether the collection contains a tuning that has the specified language-neutral id.
  /// </summary>
  /// <param name="id">The id to locate.</param>
  /// <returns>
  ///   true if the collection contains an tuning that has the specified id; otherwise,
  ///   false.
  /// </returns>
  public bool ContainsKey( string id )
  {
    return _tunings.ContainsKey( id );
  }

  /// <inheritdoc />
  public IEnumerator<KeyValuePair<string, Tuning>> GetEnumerator()
  {
    return _tunings.GetEnumerator();
  }

  /// <inheritdoc />
  IEnumerator IEnumerable.GetEnumerator()
  {
    return GetEnumerator();
  }

  /// <summary>Gets the value that is associated with the specified id.</summary>
  /// <param name="id">The language-neutral id to locate.</param>
  /// <param name="value">
  ///   [out] When this method returns, the tuning associated with the specified id,
  ///   if the id is found; otherwise, it returns null. This parameter is passed uninitialized.
  /// </param>
  /// <returns>
  ///   true if a tuning with the given id is found; otherwise, false.
  /// </returns>
  public bool TryGetValue(
    string id,
    [MaybeNullWhen( false )] out Tuning value )
  {
    return _tunings.TryGetValue( id, out value );
  }

#endregion

#region Implementation

  internal void Add( Tuning tuning )
  {
    Requires.NotNull( tuning );

    Debug.Assert( _instrumentId.Equals( tuning.InstrumentDefinition.Id ) );
    _tunings.Add( tuning.Id, tuning );
  }

#endregion
}
