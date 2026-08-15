// Module Name: NoteName.cs
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

namespace Bach.Model;

using System.Diagnostics.CodeAnalysis;
using Bach.Model.Internal;
using System.Diagnostics.Contracts;

/// <summary>
///   A NoteName represents the traditional note name
///   according to International Pitch Notation convention.
/// </summary>
public readonly struct NoteName
  : IEquatable<NoteName>,
    IComparable<NoteName>,
    ISpanConsumingParsable<NoteName>
{
  #region Constants

  /// <summary>
  /// The total number of note names. C, D, E, F, G, A, B
  /// </summary>
  public const int TotalCount = 7;

  /// <summary>
  ///   The C (Do) note
  /// </summary>
  public static readonly NoteName C = new( 0 );

  /// <summary>
  ///   The D (Re) note
  /// </summary>
  public static readonly NoteName D = new( 1 );

  /// <summary>
  ///   The E (Mi) note
  /// </summary>
  public static readonly NoteName E = new( 2 );

  /// <summary>
  ///   The F (Fa) note
  /// </summary>
  public static readonly NoteName F = new( 3 );

  /// <summary>
  ///   The G (Sol) note
  /// </summary>
  public static readonly NoteName G = new( 4 );

  /// <summary>
  ///   The A (La) note
  /// </summary>
  public static readonly NoteName A = new( 5 );

  /// <summary>
  ///   The B (Ti) note
  /// </summary>
  public static readonly NoteName B = new( 6 );

  // ReSharper disable once StringLiteralTypo
  private static readonly string s_names = "CDEFGAB";

  #endregion

  #region Fields

  private readonly int _value;

  #endregion

  #region Constructors

  private NoteName(
    int value )
  {
    ArgumentOutOfRangeException.ThrowIfLessThan( value, 0 );
    ArgumentOutOfRangeException.ThrowIfGreaterThan( value, TotalCount - 1 );
    _value = value;
  }

  #endregion

  #region Public Methods

  /// <summary>Adds a number of steps to a note name.</summary>
  /// <param name="steps">The number of steps to add.</param>
  /// <returns>A NoteName.</returns>
  [Pure]
  public NoteName Add(
    int steps )
  {
    var result = (NoteName) (_value + steps).Wrap( TotalCount );
    return result;
  }

  /// <inheritdoc />
  public int CompareTo(
    NoteName other )
  {
    return _value.CompareTo( other._value );
  }

  /// <inheritdoc />
  public bool Equals(
    NoteName other )
  {
    return _value == other._value;
  }

  /// <inheritdoc />
  public override bool Equals(
    object? obj )
  {
    return obj is NoteName other && Equals( other );
  }

  /// <inheritdoc />
  public override int GetHashCode()
  {
    return _value;
  }

  /// <summary>Parses the provided string.</summary>
  /// <param name="value">The value to parse.</param>
  /// <returns>A PitchClass.</returns>
  /// <exception cref="FormatException">Thrown when the provided string doesn't represent a <see cref="NoteName"/>.</exception>
  /// <exception cref="ArgumentNullException">Thrown when a null string is provided.</exception>
  /// <exception cref="ArgumentException">Thrown when an empty string is provided.</exception>
  public static NoteName Parse(
    string value )
  {
    return Parse( value, null );
  }

  /// <summary>
  /// Parses the provided string.
  /// </summary>
  /// <param name="value">The string to parse.</param>
  /// <param name="provider">The format provider.</param>
  /// <returns>A NoteName.</returns>
  /// <exception cref="FormatException">Thrown when the provided string doesn't represent a <see cref="NoteName"/>.</exception>
  /// <exception cref="ArgumentNullException">Thrown when a null string is provided.</exception>
  /// <exception cref="ArgumentException">Thrown when an empty string is provided.</exception>
  public static NoteName Parse(
    string value,
    IFormatProvider? provider )
  {
    ArgumentException.ThrowIfNullOrEmpty( value );
    return Parse( value.AsSpan(), provider );
  }

  /// <summary>
  /// Parses the provided string.
  /// </summary>
  /// <param name="value">The string to parse.</param>
  /// <param name="provider">The format provider.</param>
  /// <returns>A NoteName.</returns>
  /// <exception cref="FormatException">Thrown when the provided string doesn't represent a <see cref="NoteName"/>.</exception>
  public static NoteName Parse(
    ReadOnlySpan<char> value,
    IFormatProvider? provider )
  {
    return TryParse( value, provider, out var result ) ? result : throw new FormatException( $"{value} is not a valid note name" );
  }

  /// <summary>Subtracts a number of steps from a note name.</summary>
  /// <param name="steps">The number of steps to subtract.</param>
  /// <returns>A NoteName.</returns>
  [Pure]
  public NoteName Subtract(
    int steps )
  {
    return Add( -steps );
  }

  /// <summary>Returns to number of note names between two note names.</summary>
  /// <param name="name">The last note name.</param>
  /// <returns>A NoteName.</returns>
  [Pure]
  public int Subtract(
    NoteName name )
  {
    return (int) Add( -(int) name );
  }

  /// <inheritdoc />
  public override string ToString()
  {
    return s_names[_value]
      .ToString();
  }

  /// <summary>Attempts to parse a NoteName from the given string.</summary>
  /// <param name="value">The value to parse.</param>
  /// <param name="noteName">[out] The note name.</param>
  /// <returns>True if it succeeds, false if it fails.</returns>
  public static bool TryParse(
    string? value,
    out NoteName noteName )
  {
    return TryParse( value.AsSpan(), null, out noteName );
  }

  /// <summary>
  /// Attempts to parse a NoteName from the given string.
  /// </summary>
  /// <param name="value">The value to parse.</param>
  /// <param name="provider">The format provider.</param>
  /// <param name="noteName">[out] The note name.</param>
  /// <returns>True if it succeeds, false if it fails.</returns>
  public static bool TryParse(
    [NotNullWhen( true )] string? value,
    IFormatProvider? provider,
    out NoteName noteName )
  {
    return TryParse( value.AsSpan(), provider, out noteName );
  }

  /// <summary>Attempts to parse a NoteName from the given span.</summary>
  /// <param name="span">The span to parse.</param>
  /// <param name="noteName">[out] The note name.</param>
  /// <returns>True if it succeeds, false if it fails.</returns>
  public static bool TryParse(
    ReadOnlySpan<char> span,
    out NoteName noteName )
  {
    return TryParse( span, null, out noteName );
  }

  /// <summary>
  /// Attempts to parse a NoteName from the given span.
  /// </summary>
  /// <param name="span">The span to parse.</param>
  /// <param name="provider">The format provider.</param>
  /// <param name="noteName">[out] The note name.</param>
  /// <returns>True if it succeeds, false if it fails.</returns>
  public static bool TryParse(
    ReadOnlySpan<char> span,
    IFormatProvider? provider,
    out NoteName noteName )
  {
    // We want to make sure that the entire span is consumed, so we call the
    // overload that returns the tail and check if it's empty.
    return TryParse(span, provider, out noteName, out var tail ) && tail.IsEmpty;
  }

  /// <summary>
  /// Attempts to parse a NoteName from the given span.
  /// </summary>
  /// <param name="span">The span to parse.</param>
  /// <param name="provider">The format provider.</param>
  /// <param name="noteName">[out] The note name.</param>
  /// <param name="tail">[out] The remaining span after parsing the note name.</param>
  /// <returns>True if it succeeds, false if it fails.</returns>
  public static bool TryParse(
    ReadOnlySpan<char> span,
    IFormatProvider? provider,
    out NoteName noteName,
    out ReadOnlySpan<char> tail )
  {
    span = span.TrimStart();
    if( span.IsEmpty )
    {
      noteName = C;
      tail = ReadOnlySpan<char>.Empty;
      return false;
    }

    var value = s_names.IndexOf( char.ToUpperInvariant( span[0] ) );
    if( value == -1 )
    {
      noteName = C;
      tail = span;
      return false;
    }

    noteName = new NoteName( value );
    tail = span[1..];
    return true;
  }

  #endregion

  #region Operators

  /// <summary>Explicit cast that converts the given NoteName to an int.</summary>
  /// <param name="noteName">The note name.</param>
  /// <returns>The result of the operation.</returns>
  public static explicit operator int(
    NoteName noteName )
  {
    return noteName._value;
  }

  /// <summary>Explicit cast that converts the given int to a NoteName.</summary>
  /// <param name="value">The value.</param>
  /// <returns>The result of the operation.</returns>
  public static explicit operator NoteName(
    int value )
  {
    return new NoteName( value );
  }

  /// <summary>Equality operator.</summary>
  /// <param name="left">The first instance to compare.</param>
  /// <param name="right">The second instance to compare.</param>
  /// <returns>The result of the operation.</returns>
  public static bool operator ==(
    NoteName left,
    NoteName right )
  {
    return left.Equals( right );
  }

  /// <summary>Inequality operator.</summary>
  /// <param name="left">The first instance to compare.</param>
  /// <param name="right">The second instance to compare.</param>
  /// <returns>The result of the operation.</returns>
  public static bool operator !=(
    NoteName left,
    NoteName right )
  {
    return !left.Equals( right );
  }

  /// <summary>Lesser-than comparison operator.</summary>
  /// <param name="left">The first instance to compare.</param>
  /// <param name="right">The second instance to compare.</param>
  /// <returns>The result of the operation.</returns>
  public static bool operator <(
    NoteName left,
    NoteName right )
  {
    return left.CompareTo( right ) < 0;
  }

  /// <summary>Greater-than comparison operator.</summary>
  /// <param name="left">The first instance to compare.</param>
  /// <param name="right">The second instance to compare.</param>
  /// <returns>The result of the operation.</returns>
  public static bool operator >(
    NoteName left,
    NoteName right )
  {
    return left.CompareTo( right ) > 0;
  }

  /// <summary>Lesser-than-or-equal comparison operator.</summary>
  /// <param name="left">The first instance to compare.</param>
  /// <param name="right">The second instance to compare.</param>
  /// <returns>The result of the operation.</returns>
  public static bool operator <=(
    NoteName left,
    NoteName right )
  {
    return left.CompareTo( right ) <= 0;
  }

  /// <summary>Greater-than-or-equal comparison operator.</summary>
  /// <param name="left">The first instance to compare.</param>
  /// <param name="right">The second instance to compare.</param>
  /// <returns>The result of the operation.</returns>
  public static bool operator >=(
    NoteName left,
    NoteName right )
  {
    return left.CompareTo( right ) >= 0;
  }

  /// <summary>Subtraction operator.</summary>
  /// <param name="a">The first value.</param>
  /// <param name="b">The second value.</param>
  /// <returns>The result of the operation.</returns>
  public static int operator -(
    NoteName a,
    NoteName b )
  {
    return a.Subtract( b );
  }

  /// <summary>Addition operator.</summary>
  /// <param name="noteName">The first value.</param>
  /// <param name="semitoneCount">A number of semitones to add to it.</param>
  /// <returns>The result of the operation.</returns>
  public static NoteName operator +(
    NoteName noteName,
    int semitoneCount )
  {
    return noteName.Add( semitoneCount );
  }

  /// <summary>Increment operator.</summary>
  /// <param name="noteName">The pitch class.</param>
  /// <returns>The result of the operation.</returns>
  public static NoteName operator ++(
    NoteName noteName )
  {
    return noteName.Add( 1 );
  }

  /// <summary>Subtraction operator.</summary>
  /// <param name="noteName">The first value.</param>
  /// <param name="semitoneCount">A number of semitones to subtract from it.</param>
  /// <returns>The result of the operation.</returns>
  public static NoteName operator -(
    NoteName noteName,
    int semitoneCount )
  {
    return noteName.Subtract( semitoneCount );
  }

  /// <summary>Decrement operator.</summary>
  /// <param name="noteName">The pitch class.</param>
  /// <returns>The result of the operation.</returns>
  public static NoteName operator --(
    NoteName noteName )
  {
    return noteName.Subtract( 1 );
  }

  #endregion
}
