// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

// See: https://github.com/dotnet/runtime/issues/934

namespace System;

internal static class MemoryExtensions
{
#region Public Methods

  public static SpanSplitEnumerator<char> Split( this ReadOnlySpan<char> span )
  {
    return new SpanSplitEnumerator<char>( span, ' ' );
  }

  public static SpanSplitEnumerator<char> Split(
    this ReadOnlySpan<char> span,
    char separator )
  {
    return new SpanSplitEnumerator<char>( span, separator );
  }

  public static SpanSplitSequenceEnumerator<char> Split(
    this ReadOnlySpan<char> span,
    string separator )
  {
    return new SpanSplitSequenceEnumerator<char>( span, separator );
  }

#endregion
}

internal ref struct SpanSplitEnumerator<T>
  where T: IEquatable<T>
{
#region Fields

  private readonly ReadOnlySpan<T> _sequence;
  private readonly T _separator;
  private int _offset;
  private int _index;

#endregion

#region Constructors

  internal SpanSplitEnumerator(
    ReadOnlySpan<T> span,
    T separator )
  {
    _sequence = span;
    _separator = separator;
    _index = 0;
    _offset = 0;
  }

#endregion

#region Properties

  public Range Current => new( _offset, ( _offset + _index ) - 1 );

#endregion

#region Public Methods

  public SpanSplitEnumerator<T> GetEnumerator()
  {
    return this;
  }

  public bool MoveNext()
  {
    if( _sequence.Length - _offset < _index )
    {
      return false;
    }

    var slice = _sequence.Slice( _offset += _index );

    var nextIdx = slice.IndexOf( _separator );
    _index = ( nextIdx != -1 ? nextIdx : slice.Length ) + 1;
    return true;
  }

#endregion
}

internal ref struct SpanSplitSequenceEnumerator<T>
  where T: IEquatable<T>
{
#region Fields

  private readonly ReadOnlySpan<T> _sequence;
  private readonly ReadOnlySpan<T> _separator;
  private int _offset;
  private int _index;

#endregion

#region Constructors

  internal SpanSplitSequenceEnumerator(
    ReadOnlySpan<T> span,
    ReadOnlySpan<T> separator )
  {
    _sequence = span;
    _separator = separator;
    _index = 0;
    _offset = 0;
  }

#endregion

#region Properties

  public Range Current => new( _offset, ( _offset + _index ) - 1 );

#endregion

#region Public Methods

  public SpanSplitSequenceEnumerator<T> GetEnumerator()
  {
    return this;
  }

  public bool MoveNext()
  {
    if( _sequence.Length - _offset < _index )
    {
      return false;
    }

    var slice = _sequence.Slice( _offset += _index );

    var nextIdx = slice.IndexOf( _separator );
    _index = ( nextIdx != -1 ? nextIdx : slice.Length ) + 1;
    return true;
  }

#endregion
}
