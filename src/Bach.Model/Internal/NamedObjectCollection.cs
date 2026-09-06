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
public sealed class NamedObjectCollection<T>: IReadOnlyCollection<T>
  where T: INamedObject
{
  #region Fields

  private readonly FrozenDictionary<string, T> _byId;
  private readonly FrozenDictionary<string, T> _byName;

  #endregion

  #region Constructors

  /// <summary>
  ///   Initializes a new instance of the <see cref="NamedObjectCollection{T}"/> class with the specified items.
  /// </summary>
  /// <param name="items">The items to initialize the collection with.</param>
  public NamedObjectCollection(
    IEnumerable<T> items )
  {
    var byId = new Dictionary<string, T>( Comparer.IdComparer );
    var byName = new Dictionary<string, T>( Comparer.NameComparer );

    foreach( var namedObject in items )
    {
      byId.Add( namedObject.Id, namedObject );
      byName.Add( namedObject.Name, namedObject );
    }

    _byId = byId.ToFrozenDictionary( Comparer.IdComparer );
    _byName = byName.ToFrozenDictionary( Comparer.NameComparer );
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

  /// <summary>
  ///   Gets the number of items in the collection.
  /// </summary>
  public int Count => _byId.Count;

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

    if( _byId.TryGetValue( idOrName, out item ) )
    {
      return true;
    }

    if( _byName.TryGetValue( idOrName, out item ) )
    {
      return true;
    }

    item = default;
    return false;
  }

  /// <summary>
  ///   Returns an enumerator that iterates through the collection.
  /// </summary>
  /// <returns>An enumerator that can be used to iterate through the collection.</returns>
  public IEnumerator<T> GetEnumerator()
  {
    return _byId.Values.AsEnumerable()
                .GetEnumerator();
  }

  /// <summary>
  ///   Returns an enumerator that iterates through the collection.
  /// </summary>
  /// <returns>An enumerator that can be used to iterate through the collection.</returns>
  IEnumerator IEnumerable.GetEnumerator()
  {
    return GetEnumerator();
  }

  #endregion

  #region Implementation

  private static string Humanize(
    Type type )
  {
    if (!type.IsGenericType )
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
