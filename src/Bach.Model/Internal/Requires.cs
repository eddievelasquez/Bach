// Module Name: Requires.cs
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

#nullable enable

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using NoEnumeration = JetBrains.Annotations.NoEnumerationAttribute;

namespace Bach.Model.Internal;

internal static class Requires
{
#region Public Methods

  public static void Between<T>(
    T value,
    T lowerBound,
    T upperBound,
    string? message = null,
    [CallerArgumentExpression( nameof( value ) )] string? callerArgExpr = null )
    where T: IComparable
  {
    if( value.CompareTo( lowerBound ) >= 0 && value.CompareTo( upperBound ) <= 0 )
    {
      return;
    }

    callerArgExpr ??= "argument";
    throw new ArgumentOutOfRangeException( callerArgExpr,
                                           value,
                                           message ?? $"Value must be between {lowerBound} and {upperBound}" );
  }

  public static void Condition<TException>(
    bool predicate,
    string? message = null,
    [CallerArgumentExpression( nameof( predicate ) )] string? callerArgExpr
      = null )
    where TException: Exception, new()
  {
    if( predicate )
    {
      return;
    }

    // https://stackoverflow.com/questions/41397/asking-a-generic-method-to-throw-specific-exception-type-on-fail/41450#41450
    TException? exception;

    try
    {
      message ??= $"Expectation not met: {callerArgExpr ?? "<unknown>"}";
      exception = Activator.CreateInstance( typeof( TException ), message ) as TException;
    }
    catch( MissingMethodException )
    {
      exception = new TException();
    }

    Debug.Assert( exception != null, nameof( exception ) + " != null" );
    throw exception;
  }

  public static void ExactCount<T>(
    [NotNull] ICollection<T>? value,
    int expectedCount,
    string? message = null,
    [CallerArgumentExpression( nameof( value ) )] string? callerArgExpr = null )
  {
    if( value == null )
    {
      ThrowArgumentNull( message, callerArgExpr );
    }

    if( value.Count != expectedCount )
    {
      ThrowArgumentInvalidCount( expectedCount, message, callerArgExpr );
    }
  }

  public static void ExactCount<T>(
    [NotNull] IReadOnlyCollection<T>? value,
    int expectedCount,
    string? message = null,
    [CallerArgumentExpression( nameof( value ) )] string? callerArgExpr = null )
  {
    if( value == null )
    {
      ThrowArgumentNull( message, callerArgExpr );
    }

    if( value.Count != expectedCount )
    {
      ThrowArgumentInvalidCount( expectedCount, message, callerArgExpr );
    }
  }

  public static void GreaterThan<T>(
    T value,
    T lowerBound,
    string? message = null,
    [CallerArgumentExpression( nameof( value ) )] string? callerArgExpr = null )
    where T: IComparable
  {
    if( value.CompareTo( lowerBound ) > 0 )
    {
      return;
    }

    callerArgExpr ??= "argument";
    throw new ArgumentOutOfRangeException( callerArgExpr,
                                           value,
                                           message ?? $"Value must be greater than {lowerBound}" );
  }

  public static void LessThan<T>(
    T value,
    T upperBound,
    string? message = null,
    [CallerArgumentExpression( nameof( value ) )] string? callerArgExpr = null )
    where T: IComparable
  {
    if( value.CompareTo( upperBound ) < 0 )
    {
      return;
    }

    callerArgExpr ??= "argument";
    throw new ArgumentOutOfRangeException( callerArgExpr, value, message ?? $"Value must be less than {upperBound}" );
  }

  public static void NotEmpty<T>(
    ReadOnlySpan<T> value,
    string? message = null,
    [CallerArgumentExpression( nameof( value ) )] string? callerArgExpr = null )
  {
    if( value.IsEmpty )
    {
      ThrowArgumentEmpty( message, callerArgExpr );
    }
  }

  public static void NotNull<T>(
    [NoEnumeration] [NotNull] T? value,
    string? message = null,
    [CallerArgumentExpression( nameof( value ) )] string? callerArgExpr = null )
  {
    if( value == null )
    {
      ThrowArgumentNull( message, callerArgExpr );
    }
  }

  public static void NotNullOrEmpty(
    [NotNull] string? value,
    string? message = null,
    [CallerArgumentExpression( nameof( value ) )] string? callerArgExpr = null )
  {
    if( value == null )
    {
      ThrowArgumentNull( message, callerArgExpr );
    }

    if( value.Length == 0 )
    {
      ThrowArgumentEmpty( message, callerArgExpr );
    }
  }

  public static void NotNullOrEmpty<T>(
    [NotNull] ICollection<T>? value,
    string? message = null,
    [CallerArgumentExpression( nameof( value ) )] string? callerArgExpr = null )
  {
    if( value == null )
    {
      ThrowArgumentNull( message, callerArgExpr );
    }

    if( value.Count == 0 )
    {
      ThrowArgumentEmpty( message, callerArgExpr );
    }
  }

#endregion

#region Implementation

  [DoesNotReturn]
  private static void ThrowArgumentNull(
    string? message,
    string? callerArgExpr )
  {
    callerArgExpr ??= "argument";

    throw new ArgumentNullException( callerArgExpr, message ?? $"{callerArgExpr} cannot be null" );
  }

  [DoesNotReturn]
  private static void ThrowArgumentEmpty(
    string? message,
    string? callerArgExpr )
  {
    callerArgExpr ??= "argument";
    throw new ArgumentException( callerArgExpr, message ?? $"{callerArgExpr} cannot be empty" );
  }

  [DoesNotReturn]
  private static void ThrowArgumentInvalidCount(
    int expectedCount,
    string? message,
    string? callerArgExpr )
  {
    callerArgExpr ??= "argument";
    throw new ArgumentException( callerArgExpr,
                                 message ?? $"{callerArgExpr} must have exactly {expectedCount} elements" );
  }

#endregion
}
