//
// Module Name: TuningCollection.cs
// Project:     Bach.Model
// Copyright (c) 2016  Eddie Velasquez.
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
//  portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A
// PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE
// OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

namespace Bach.Model.Instruments
{
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Diagnostics;
  using System.Diagnostics.Contracts;

  public class TuningCollection: IReadOnlyDictionary<string, Tuning>
  {
    #region Data Members

    private Guid _instrumentId;

    private readonly Dictionary<string, Tuning> _tunings =
      new Dictionary<string, Tuning>(StringComparer.CurrentCultureIgnoreCase);

    #endregion

    #region Construction/Destruction

    internal TuningCollection(Guid instrumentId)
    {
      _instrumentId = instrumentId;
    }

    #endregion

    #region Properties

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

    public IEnumerator<KeyValuePair<string, Tuning>> GetEnumerator() => _tunings.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public int Count => _tunings.Count;

    public bool ContainsKey(string key) => _tunings.ContainsKey(key);

    public bool TryGetValue(string key,
                            out Tuning value) => _tunings.TryGetValue(key, out value);

    public Tuning this[string key] => _tunings[key];

    public IEnumerable<string> Keys => _tunings.Keys;

    public IEnumerable<Tuning> Values => _tunings.Values;

    #endregion

    internal void Add(Tuning tuning)
    {
      Contract.Requires<ArgumentNullException>(tuning != null);

      Debug.Assert(_instrumentId.Equals(tuning.InstrumentDefinition.InstrumentId));
      _tunings.Add(tuning.Name, tuning);
    }
  }
}
