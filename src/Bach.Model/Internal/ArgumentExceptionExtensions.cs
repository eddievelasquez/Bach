// Module Name: ArgumentExceptionExtensions.cs
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

using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Bach.Model.Internal;

/// <summary>
///   Provides extension methods for <see cref="ArgumentException"/> to enhance argument validation.
/// </summary>
internal static class ArgumentExceptionExtensions
{
  #region Implementation

  extension(
    ArgumentException )
  {
    #region Public Methods

    /// <summary>
    ///   Throws an <see cref="ArgumentNullException"/> if the specified source is null, or an
    ///   <see cref="ArgumentException"/> if the source is empty.
    /// </summary>
    /// <typeparam name="T">The type of elements in the source sequence.</typeparam>
    /// <param name="source">The source sequence to check.</param>
    /// <param name="message">The message to include in the exception.</param>
    /// <param name="paramName">The name of the parameter.</param>
    /// <exception cref="ArgumentNullException">Thrown if the source is null.</exception>
    /// <exception cref="ArgumentException">Thrown if the source is empty.</exception>
    public static void ThrowIfNullOrEmpty<T>(
      IEnumerable<T>? source,
      string? message = null,
      [CallerArgumentExpression( nameof( source ) )] string? paramName = null )
    {
      if( source is null )
      {
        throw new ArgumentNullException( paramName, message );
      }

      if( !source.TryGetNonEnumeratedCount( out var count ) )
      {
        if( !source.Any() )
        {
          throw new ArgumentException( message ?? "Sequence must not be empty.", paramName );
        }
      }
      else if( count == 0 )
      {
        throw new ArgumentException( message ?? "Sequence must not be empty.", paramName );
      }
    }

    /// <summary>
    ///   Throws an <see cref="ArgumentException"/> if the specified source contains any null elements.
    /// </summary>
    /// <typeparam name="T">The type of elements in the source sequence.</typeparam>
    /// <param name="source">The source sequence to check.</param>
    /// <param name="message">The message to include in the exception.</param>
    /// <param name="paramName">The name of the parameter.</param>
    /// <exception cref="ArgumentException">Thrown if the source contains any null elements.</exception>
    public static void ThrowIfContainsNulls<T>(
      IEnumerable<T> source,
      string? message = null,
      [CallerArgumentExpression( nameof( source ) )] string? paramName = null )
    {
      if( source.Any( e => e is null ) )
      {
        throw new ArgumentException( message ?? "Sequence contains a null.", paramName );
      }
    }

    /// <summary>
    ///   Throws an <see cref="ArgumentException"/> if the specified value is not equal to the expected value.
    /// </summary>
    /// <typeparam name="T">The type of the values to compare.</typeparam>
    /// <param name="value">The value to check.</param>
    /// <param name="expected">The expected value.</param>
    /// <param name="message">The message to include in the exception.</param>
    /// <param name="paramName">The name of the parameter.</param>
    /// <exception cref="ArgumentException">Thrown if the value is not equal to the expected value.</exception>
    public static void ThrowIfNotEqualTo<T>(
      T value,
      T expected,
      string? message = null,
      [CallerArgumentExpression( nameof( value ) )] string? paramName = null )
      where T: IComparable<T>
    {
      if( value.CompareTo( expected ) != 0 )
      {
        throw new ArgumentException( message ?? $"Value must be equal to {expected}.", paramName );
      }
    }

    #endregion
  }

  extension(
    ArgumentOutOfRangeException )
  {
    #region Public Methods

    /// <summary>
    ///   Throws an <see cref="ArgumentOutOfRangeException"/> if the specified value is outside the specified range.
    /// </summary>
    /// <param name="value">The value to check.</param>
    /// <param name="lowerBound">The lower bound of the range.</param>
    /// <param name="upperBound">The upper bound of the range.</param>
    /// <param name="message">The message to include in the exception.</param>
    /// <param name="paramName">The name of the parameter.</param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static void ThrowIfOutOfRange<T>(
      T value,
      T lowerBound,
      T upperBound,
      string? message = null,
      [CallerArgumentExpression( nameof( value ) )] string? paramName = null )
      where T: IComparable<T>
    {
      if( value.CompareTo( lowerBound ) < 0 || value.CompareTo( upperBound ) > 0 )
      {
        throw new ArgumentOutOfRangeException( paramName, value, message );
      }
    }

    #endregion
  }

  #endregion
}
