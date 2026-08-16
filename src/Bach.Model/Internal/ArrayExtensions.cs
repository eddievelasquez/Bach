// Module Name: ArrayExtensions.cs
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

namespace Bach.Model.Internal;

using System.Collections.Generic;

internal static class ArrayExtensions
{
  #region Implementation

  /// <param name="values">The values.</param>
  /// <typeparam name="T">Type of the list elements.</typeparam>
  extension<T>(
    IReadOnlyList<T> values )
    where T: IComparable<T>
  {
    #region Public Methods

    /// <summary>Determines whether the provided list is sorted and only contains unique elements</summary>
    /// <returns>
    ///   <c>true</c> if the list is sorted and only contains unique elements; otherwise, <c>false</c>.
    /// </returns>
    public bool IsSortedUnique()
    {
      for( var i = 1; i < values.Count; ++i )
      {
        var result = values[i - 1]
          .CompareTo( values[i] );

        if( result >= 0 )
        {
          return false;
        }
      }

      return true;
    }

    #endregion
  }

  /// <param name="index">The index.</param>
  extension(
    int index )
  {
    #region Public Methods

    /// <summary>  Handles underflow and overflow of the provided index given a length.</summary>
    /// <param name="length">The length.</param>
    /// <returns>An index that is ensured to be in the range zero to length - 1.</returns>
    public int Wrap(
      int length )
    {
      // Handle underflow and overflow of the provided index by using the modulo operator.
      // The result of the modulo operator can be negative, so we add length to ensure a
      // positive result, and then take modulo length again to ensure it is within the range.
      return ( ( index % length ) + length ) % length;
    }

    #endregion
  }

  /// <param name="collection">The collection.</param>
  extension<T>(
    ICollection<T> collection )
  {
    #region Public Methods

    /// <summary>  Handles underflow and overflow of the provided index within the given collection.</summary>
    /// <param name="index">The index.</param>
    /// <returns>An index that is ensured to be within the range of elements of the list.</returns>
    public int WrapIndex(
      int index )
    {
      return index.Wrap( collection.Count );
    }

    #endregion
  }

  /// <param name="collection">The collection.</param>
  extension<T>(
    IReadOnlyCollection<T> collection )
  {
    #region Public Methods

    /// <summary>  Handles underflow and overflow of the provided index within the given collection.</summary>
    /// <param name="index">The index.</param>
    /// <returns>An index that is ensured to be within the range of elements of the list.</returns>
    public int WrapIndex(
      int index )
    {
      return index.Wrap( collection.Count );
    }

    #endregion
  }

  /// <param name="array">An array.</param>
  extension(
    Array array )
  {
    #region Public Methods

    /// <summary>  Handles underflow and overflow of the provided index within the dimension of the given array.</summary>
    /// <param name="dimension">The array's dimension to evaluate.</param>
    /// <param name="index">The index.</param>
    /// <returns>An index that is ensured to be within the range of elements of the dimension of the array.</returns>
    public int WrapIndex(
      int dimension,
      int index )
    {
      return index.Wrap( array.GetLength( dimension ) );
    }

    #endregion
  }

  #endregion
}
