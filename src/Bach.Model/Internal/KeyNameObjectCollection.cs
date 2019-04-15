// Module Name: KeyNameObjectCollection.cs
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

namespace Bach.Model.Internal
{
  using System;
  using System.Collections.Generic;
  using System.Collections.ObjectModel;
  using System.Diagnostics;

  /// <summary>Collection of key-name objects.</summary>
  /// <typeparam name="T">Type parameter for the keyed object.</typeparam>
  [DebuggerDisplay("Count = {Count}")]
  public class KeyNameObjectCollection<T>: Collection<T>
    where T: IKeyNameObject
  {
    #region Data Members

    private readonly Dictionary<string, T> _byKey;
    private readonly Dictionary<string, T> _byName;

    #endregion

    #region Constructors

    /// <inheritdoc />
    internal KeyNameObjectCollection()
      : base(new List<T>())
    {
      _byKey = new Dictionary<string, T>(KeyComparer);
      _byName = new Dictionary<string, T>(NameComparer);
    }

    #endregion

    #region Properties

    public StringComparer KeyComparer { get; } = StringComparer.InvariantCultureIgnoreCase;
    public StringComparer NameComparer { get; } = StringComparer.CurrentCultureIgnoreCase;

    private new List<T> Items
    {
      get
      {
        Debug.Assert(base.Items is List<T>);
        return (List<T>)base.Items;
      }
    }

    public T this[string key]
    {
      get
      {
        if( TryGetValue(key, out T item) )
        {
          return item;
        }

        throw new KeyNotFoundException(string.Format($"Key or name not found: {key}"));
      }
    }

    #endregion

    #region Public Methods

    public bool TryGetValue(string keyOrName,
                            out T item)
    {
      Contract.Requires<ArgumentNullException>(keyOrName != null);

      if( _byKey.TryGetValue(keyOrName, out item) )
      {
        return true;
      }

      if( _byName.TryGetValue(keyOrName, out item) )
      {
        return true;
      }

      item = default;
      return false;
    }

    #endregion

    #region Overrides

    /// <inheritdoc />
    protected override void ClearItems()
    {
      base.ClearItems();

      _byKey?.Clear();
      _byName?.Clear();
    }

    /// <inheritdoc />
    protected override void InsertItem(int index,
                                       T item)
    {
      _byKey.Add(item.Key, item);
      _byName.Add(item.Name, item);

      base.InsertItem(index, item);
    }

    /// <inheritdoc />
    protected override void RemoveItem(int index)
    {
      T item = Items[index];
      _byKey.Remove(item.Key);
      _byName.Remove(item.Name);

      base.RemoveItem(index);
    }

    /// <inheritdoc />
    protected override void SetItem(int index,
                                    T item)
    {
      T existingItem = Items[index];
      SetItem(_byKey, KeyComparer, item.Key, existingItem.Key, item);
      SetItem(_byName, NameComparer, item.Name, existingItem.Name, item);

      base.SetItem(index, item);
    }

    #endregion

    #region  Implementation

    private static void SetItem(IDictionary<string, T> dictionary,
                                StringComparer comparer,
                                string newKey,
                                string oldKey,
                                T item)
    {
      Debug.Assert(dictionary != null);
      Debug.Assert(comparer != null);
      Debug.Assert(!string.IsNullOrEmpty(newKey));
      Debug.Assert(!string.IsNullOrEmpty(oldKey));

      if( comparer.Equals(oldKey, newKey) )
      {
        dictionary[newKey] = item;
      }
      else
      {
        dictionary.Add(newKey, item);
        dictionary.Remove(oldKey);
      }
    }

    #endregion
  }
}
