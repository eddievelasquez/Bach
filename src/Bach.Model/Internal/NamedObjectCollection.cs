// Module Name: NamedObjectCollection.cs
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

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Bach.Model.Internal;

/// <summary>Collection of key-name objects.</summary>
/// <typeparam name="T">Type parameter for the keyed object.</typeparam>
[DebuggerDisplay( "Count = {Count}" )]
public sealed class NamedObjectCollection<T>: Collection<T>
  where T: INamedObject
{
#region Fields

  private readonly Dictionary<string, T> _byId;
  private readonly Dictionary<string, T> _byName;

#endregion

#region Constructors

  /// <inheritdoc />
  internal NamedObjectCollection()
    : base( new List<T>() )
  {
    _byId = new Dictionary<string, T>( Comparer.IdComparer );
    _byName = new Dictionary<string, T>( Comparer.NameComparer );
  }

#endregion

#region Properties

  private new List<T> Items
  {
    get
    {
      Debug.Assert( base.Items is List<T> );
      return (List<T>) base.Items;
    }
  }

  public T this[ string idOrName ]
  {
    get
    {
      if( TryGetValue( idOrName, out var item ) )
      {
        return item;
      }

      throw new KeyNotFoundException( string.Format( $"Id or name not found: {idOrName}" ) );
    }
  }

#endregion

#region Public Methods

  public bool TryGetValue(
    string idOrName,
    [MaybeNullWhen( false )] out T item )
  {
    Requires.NotNullOrEmpty( idOrName );

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

#endregion

#region Implementation

  /// <inheritdoc />
  protected override void ClearItems()
  {
    base.ClearItems();

    _byId.Clear();
    _byName.Clear();
  }

  /// <inheritdoc />
  protected override void InsertItem(
    int index,
    T item )
  {
    Requires.NotNull( item );

    _byId.Add( item.Id, item );
    _byName.Add( item.Name, item );

    base.InsertItem( index, item );
  }

  /// <inheritdoc />
  protected override void RemoveItem( int index )
  {
    var item = Items[index];
    _byId.Remove( item.Id );
    _byName.Remove( item.Name );

    base.RemoveItem( index );
  }

  /// <inheritdoc />
  protected override void SetItem(
    int index,
    T item )
  {
    Requires.NotNull( item );

    var existingItem = Items[index];
    SetItem( _byId, item.Id, existingItem.Id, item );
    SetItem( _byName, item.Name, existingItem.Name, item );

    base.SetItem( index, item );
  }

  private static void SetItem(
    Dictionary<string, T> dictionary,
    string newId,
    string existingId,
    T item )
  {
    Debug.Assert( dictionary != null );
    Debug.Assert( !string.IsNullOrEmpty( newId ) );
    Debug.Assert( !string.IsNullOrEmpty( existingId ) );

    if( dictionary.Comparer.Equals( newId, existingId ) )
    {
      dictionary[newId] = item;
    }
    else
    {
      dictionary.Add( newId, item );
      dictionary.Remove( existingId );
    }
  }

#endregion
}
