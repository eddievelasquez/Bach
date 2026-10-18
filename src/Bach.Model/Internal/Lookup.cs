// Module Name: Lookup.cs
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

using System.Collections.Frozen;
using System.Collections.Generic;

namespace Bach.Model.Internal;

/// <summary>
/// Represents a lookup collection that allows for efficient retrieval of items based on string keys, supporting both
/// string and ReadOnlySpan&lt;char&gt; lookups. The collection is frozen after being initialized in the constructor.
/// </summary>
/// <typeparam name="T">The type of items stored in the lookup.</typeparam>
internal sealed class Lookup<T>
{
  #region Fields

  private readonly FrozenDictionary<string, T> _primary;
  private readonly FrozenDictionary<string, T>.AlternateLookup<ReadOnlySpan<char>> _alternate;

  #endregion

  #region Constructors

  /// <summary>
  ///   Initializes a new instance of the <see cref="Lookup{T}"/> class with the specified items, key extraction function,
  /// </summary>
  /// <param name="items">The items to include in the lookup.</param>
  /// <param name="keyFunc">A function to extract the key from each item.</param>
  /// <param name="comparer">
  ///   An optional string comparer to use for key comparisons. Defaults to <see cref="StringComparer.OrdinalIgnoreCase"/> if
  ///   not provided.
  /// </param>
  public Lookup(
    IEnumerable<T> items,
    Func<T, string> keyFunc,
    StringComparer? comparer = null )
  {
    comparer ??= StringComparer.OrdinalIgnoreCase;

    Dictionary<string, T> tmp = new( comparer );

    foreach( var item in items )
    {
      var key = keyFunc( item );
      tmp.Add( key, item );
    }

    _primary = tmp.ToFrozenDictionary( comparer );
    _alternate = _primary.GetAlternateLookup<ReadOnlySpan<char>>();
  }

  #endregion

  #region Public Methods

  /// <summary>
  ///   Tries to get the value associated with the specified string key.
  /// </summary>
  /// <param name="key">The key to locate.</param>
  /// <param name="value">
  ///   When this method returns, the value associated with the specified key, if the key is found; otherwise, the default
  ///   value for the type of the value parameter.
  /// </param>
  /// <returns>true if the lookup contains an element with the specified key; otherwise, false.</returns>
  public bool TryGetValue(
    string key,
    out T? value )
  {
    return _primary.TryGetValue( key, out value );
  }

  /// <summary>
  ///   Tries to get the value associated with the specified ReadOnlySpan&lt;char&gt; key.
  /// </summary>
  /// <param name="key">The key to locate.</param>
  /// <param name="value">
  ///   When this method returns, the value associated with the specified key, if the key is found; otherwise, the default
  ///   value for the type of the value parameter.
  /// </param>
  /// <returns>true if the lookup contains an element with the specified key; otherwise, false.</returns>
  public bool TryGetValue(
    ReadOnlySpan<char> key,
    out T? value )
  {
    return _alternate.TryGetValue( key, out value );
  }

  #endregion
}
