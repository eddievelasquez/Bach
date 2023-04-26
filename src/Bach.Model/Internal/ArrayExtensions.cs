// Module Name: ArrayExtensions.cs
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
using System.Collections.Generic;

namespace Bach.Model.Internal;

internal static class ArrayExtensions
{
#region Public Methods

  /// <summary>Determines whether the provided list is sorted and only contains unique elements</summary>
  /// <typeparam name="T">Type of the list elements.</typeparam>
  /// <param name="values">The values.</param>
  /// <returns>
  ///   <c>true</c> if the list is sorted and only contains unique elements; otherwise, <c>false</c>.
  /// </returns>
  public static bool IsSortedUnique<T>( this IReadOnlyList<T> values )
    where T: IComparable<T>
  {
    for( var i = 1; i < values.Count; ++i )
    {
      var result = values[i - 1].CompareTo( values[i] );
      if( result >= 0 )
      {
        return false;
      }
    }

    return true;
  }

  /// <summary>  Handles underflow and overflow of the provided index given a size.</summary>
  /// <param name="size">The size.</param>
  /// <param name="index">The index.</param>
  /// <returns>An index that is ensured to be in the range zero to size - 1.</returns>
  public static int WrapIndex(
    int size,
    int index )
  {
    return ( ( index % size ) + size ) % size;
  }

  /// <summary>  Handles underflow and overflow of the provided index within the given collection.</summary>
  /// <param name="collection">The collection.</param>
  /// <param name="index">The index.</param>
  /// <returns>An index that is ensured to be within the range of elements of the list.</returns>
  public static int WrapIndex<T>(
    this ICollection<T> collection,
    int index )
  {
    return WrapIndex( collection.Count, index );
  }

  /// <summary>  Handles underflow and overflow of the provided index within the given collection.</summary>
  /// <param name="collection">The collection.</param>
  /// <param name="index">The index.</param>
  /// <returns>An index that is ensured to be within the range of elements of the list.</returns>
  public static int WrapIndex<T>(
    this IReadOnlyCollection<T> collection,
    int index )
  {
    return WrapIndex( collection.Count, index );
  }

  /// <summary>  Handles underflow and overflow of the provided index within the dimension of the given array.</summary>
  /// <param name="array">An array.</param>
  /// <param name="dimension">The array's dimension to evaluate.</param>
  /// <param name="index">The index.</param>
  /// <returns>An index that is ensured to be within the range of elements of the dimension of the array.</returns>
  public static int WrapIndex(
    this Array array,
    int dimension,
    int index )
  {
    return WrapIndex( array.GetLength( dimension ), index );
  }

#endregion
}
