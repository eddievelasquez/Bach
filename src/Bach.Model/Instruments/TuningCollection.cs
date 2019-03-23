// Module Name: TuningCollection.cs
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

namespace Bach.Model.Instruments
{
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Diagnostics;
  using Model.Internal;

  /// <summary>Collection of tunings.</summary>
  public class TuningCollection: IReadOnlyDictionary<string, Tuning>
  {
    #region Data Members

    private readonly Guid _instrumentId;

    private readonly Dictionary<string, Tuning> _tunings = new Dictionary<string, Tuning>(StringComparer.CurrentCultureIgnoreCase);

    #endregion

    #region Constructors

    internal TuningCollection(Guid instrumentId)
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
        Contract.Requires<ArgumentOutOfRangeException>(Count > 0);
        return this["Standard"];
      }
    }

    #endregion

    #region IReadOnlyDictionary<string,Tuning> Members

    /// <inheritdoc />
    public IEnumerator<KeyValuePair<string, Tuning>> GetEnumerator() => _tunings.GetEnumerator();

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <summary>Gets the number of tunings in the collection.</summary>
    /// <value>The count.</value>
    public int Count => _tunings.Count;

    /// <summary>
    ///   Determines whether the collection contains a tuning that has the specified language-neutral key.
    /// </summary>
    /// <param name="key">The key to locate.</param>
    /// <returns>
    ///   true if the collection contains an tuning that has the specified key; otherwise,
    ///   false.
    /// </returns>
    public bool ContainsKey(string key) => _tunings.ContainsKey(key);

    /// <summary>Gets the value that is associated with the specified key.</summary>
    /// <param name="key">The language-neutral key to locate.</param>
    /// <param name="value">
    ///   [out] When this method returns, the tuning associated with the specified key,
    ///   if the key is found; otherwise, it returns null. This parameter is passed uninitialized.
    /// </param>
    /// <returns>
    ///   true if a tuning with the given key is found; otherwise, false.
    /// </returns>
    public bool TryGetValue(string key,
                            out Tuning value)
      => _tunings.TryGetValue(key, out value);

    /// <inheritdoc />
    /// <summary>Gets the tuning that has the specified language-neutral key.</summary>
    /// <returns>The tuning that has the specified key in the read-only dictionary.</returns>
    public Tuning this[string key] => _tunings[key];

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

    #region  Implementation

    internal void Add(Tuning tuning)
    {
      Contract.Requires<ArgumentNullException>(tuning != null);

      Debug.Assert(_instrumentId.Equals(tuning.InstrumentDefinition.InstrumentId));
      _tunings.Add(tuning.Name, tuning);
    }

    #endregion
  }
}
