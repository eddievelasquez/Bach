// Module Name: NamedObjectCollection.cs
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
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Bach.Model.Internal;

/// <summary>
///   Represents a collection of named objects indexed by both their identifier and display name.
/// </summary>
/// <typeparam name="T">
///   The type of named object stored in the collection. The type must implement <see cref="INamedObject"/>.
/// </typeparam>
/// <remarks>
///   The collection preserves insertion order and supports lookups by either the object identifier or its name.
/// </remarks>
[DebuggerDisplay( "Count = {Count}" )]
internal sealed class NamedObjectCollection<T>: INamedObjectCollection<T>
  where T: INamedObject
{
  #region Fields

  private readonly FrozenDictionary<string, T> _byKeyOrName;
  private readonly FrozenDictionary<string, T>.AlternateLookup<ReadOnlySpan<char>> _byKeyOrNameAltLookup;
  private readonly T[] _values;

  #endregion

  #region Constructors

  /// <summary>
  ///   Initializes a new instance of the <see cref="NamedObjectCollection{T}"/> class with the specified items.
  /// </summary>
  /// <param name="items">The items to initialize the collection with.</param>
  public NamedObjectCollection(
    IEnumerable<T> items )
  {
    _values = [.. items];

    var dict = new Dictionary<string, T>( Comparer.NameComparer );

    foreach( var namedObject in _values )
    {
      // Add the id, an exception will be thrown if the id already exists in the dictionary.
      dict.Add( namedObject.Id, namedObject );

      // Names are unique as well, so no other entry should exist unless the name is the same as the id.
      ref var existingValueRef = ref CollectionsMarshal.GetValueRefOrAddDefault(
        dict,
        namedObject.Name,
        out var exists
      );

      // The name doesn't exist, so we can add it to the dictionary.
      if( !exists )
      {
        existingValueRef = namedObject;
        continue;
      }

      // If the existing value is not equal to the new value, throw an exception; otherwise,
      // the id and name are the same so there's no need to add it again.
      if( !EqualityComparer<T>.Default.Equals( existingValueRef, namedObject ) )
      {
        throw new ArgumentException( $"A different object with the id or name '{namedObject.Name}' already exists." );
      }
    }

    _byKeyOrName = dict.ToFrozenDictionary( Comparer.NameComparer );
    _byKeyOrNameAltLookup = _byKeyOrName.GetAlternateLookup<ReadOnlySpan<char>>();
  }

  #endregion

  #region Properties

  /// <summary>
  ///   Gets the value associated with the specified ID or name.
  /// </summary>
  /// <param name="idOrName">The ID or name of the object.</param>
  /// <exception cref="KeyNotFoundException">If the object was not found.</exception>
  public T this[
    string idOrName ]
  {
    get
    {
      if( TryGetValue( idOrName, out var item ) )
      {
        return item;
      }

      throw new KeyNotFoundException(
        string.Format( $"{Humanize( GetType() )} with id or name '{idOrName}' was not found." )
      );
    }
  }

  #endregion

  #region Public Methods

  /// <summary>
  ///   Tries to get the value associated with the specified ID or name.
  /// </summary>
  /// <param name="idOrName">The ID or name of the object.</param>
  /// <param name="item">
  ///   When this method returns, contains the value associated with the specified ID or name, if the ID or
  ///   name is found; otherwise, the default value for the type of the value parameter. This parameter is passed
  ///   uninitialized.
  /// </param>
  /// <returns>true if the ID or name is found; otherwise, false.</returns>
  public bool TryGetValue(
    string idOrName,
    [MaybeNullWhen( false )] out T item )
  {
    ArgumentException.ThrowIfNullOrEmpty( idOrName );

    return _byKeyOrName.TryGetValue( idOrName, out item );
  }

  /// <summary>
  ///   Tries to get the value associated with the specified ID or name.
  /// </summary>
  /// <param name="idOrName">The ID or name of the object.</param>
  /// <param name="item">
  ///   When this method returns, contains the value associated with the specified ID or name, if the ID or
  ///   name is found; otherwise, the default value for the type of the value parameter. This parameter is passed
  ///   uninitialized.
  /// </param>
  /// <returns>true if the ID or name is found; otherwise, false.</returns>
  public bool TryGetValue(
    ReadOnlySpan<char> idOrName,
    [MaybeNullWhen( false )] out T item )
  {
    if( idOrName.IsEmpty )
    {
      item = default;
      return false;
    }

    return _byKeyOrNameAltLookup.TryGetValue( idOrName, out item );
  }

  /// <summary>
  ///   Returns an enumerator that iterates through the collection.
  /// </summary>
  /// <returns>An enumerator that can be used to iterate through the collection.</returns>
  public IEnumerator<T> GetEnumerator()
  {
    return _values.AsEnumerable()
                  .GetEnumerator();
  }

  #endregion

  #region IEnumerable Implementation

  /// <summary>
  ///   Returns an enumerator that iterates through the collection.
  /// </summary>
  /// <returns>An enumerator that can be used to iterate through the collection.</returns>
  IEnumerator IEnumerable.GetEnumerator()
  {
    return GetEnumerator();
  }

  #endregion

  #region IReadOnlyCollection<T> Implementation

  /// <summary>
  ///   Gets the number of items in the collection.
  /// </summary>
  public int Count => _values.Length;

  #endregion

  #region Implementation

  private static string Humanize(
    Type type )
  {
    if( !type.IsGenericType )
    {
      return Humanize( type.Name );
    }

    var gt = type.GetGenericArguments()
                 .First();

    return Humanize( gt.Name );
  }

  private static string Humanize(
    string idOrName )
  {
    // Split a pascal-case or camel-case string into words and join them with spaces
    var words = Regex.Matches( idOrName, @"[A-Z][a-z]*|[a-z]+|\d+" )
                     .Select( m => m.Value )
                     .ToArray();

    var result = string.Join( " ", words );
    return result;
  }

  #endregion
}
