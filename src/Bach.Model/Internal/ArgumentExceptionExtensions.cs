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
    /// <param name="paramName">The name of the parameter.</param>
    /// <exception cref="ArgumentNullException">Thrown if the source is null.</exception>
    /// <exception cref="ArgumentException">Thrown if the source is empty.</exception>
    public static void ThrowIfNullOrEmpty<T>(
      IEnumerable<T>? source,
      string? paramName = null )
    {
      if( source is null )
      {
        throw new ArgumentNullException( paramName );
      }

      if( !source.TryGetNonEnumeratedCount( out var count ) )
      {
        if( !source.Any() )
        {
          throw new ArgumentException( "Sequence must not be empty.", paramName );
        }
      }
      else if( count == 0 )
      {
        throw new ArgumentException( "Sequence must not be empty.", paramName );
      }
    }

    #endregion
  }

  #endregion
}
