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

using System;
using System.Diagnostics.Contracts;
using Bach.Model.Internal;

namespace Bach.Model;

/// <summary>
///   A NoteName represents the traditional note name
///   according to International Pitch Notation convention.
/// </summary>
public readonly struct NoteName
  : IEquatable<NoteName>,
    IComparable<NoteName>
{
#region Constants

  private const int NoteNameCount = 7;
  public static readonly NoteName C = new( 0 );
  public static readonly NoteName D = new( 1 );
  public static readonly NoteName E = new( 2 );
  public static readonly NoteName F = new( 3 );
  public static readonly NoteName G = new( 4 );
  public static readonly NoteName A = new( 5 );
  public static readonly NoteName B = new( 6 );

  // ReSharper disable once StringLiteralTypo
  private static readonly string s_names = "CDEFGAB";

#endregion

#region Fields

  private readonly int _value;

#endregion

#region Constructors

  private NoteName( int value )
  {
    Requires.Between( value, 0, NoteNameCount - 1 );
    _value = value;
  }

#endregion

#region Public Methods

  /// <summary>Adds a number of steps to a note name.</summary>
  /// <param name="steps">The number of steps to add.</param>
  /// <returns>A NoteName.</returns>
  [Pure]
  public NoteName Add( int steps )
  {
    var result = (NoteName) ArrayExtensions.WrapIndex( NoteNameCount, _value + steps );
    return result;
  }

  /// <inheritdoc />
  public int CompareTo( NoteName other )
  {
    return _value.CompareTo( other._value );
  }

  /// <inheritdoc />
  public bool Equals( NoteName other )
  {
    return _value == other._value;
  }

  /// <inheritdoc />
  public override bool Equals( object? obj )
  {
    return obj is NoteName other && Equals( other );
  }

  /// <inheritdoc />
  public override int GetHashCode()
  {
    return _value;
  }

  /// <summary>Parses the provided string.</summary>
  /// <exception cref="FormatException">Thrown when the provided string doesn't represent a a NoteName.</exception>
  /// <exception cref="ArgumentNullException">Thrown when a null string is provided.</exception>
  /// <exception cref="ArgumentException">Thrown when an empty string is provided.</exception>
  /// <param name="value">The value to parse.</param>
  /// <returns>A PitchClass.</returns>
  public static NoteName Parse( string value )
  {
    Requires.NotNullOrEmpty( value );

    if( !TryParse( value, out var result ) )
    {
      throw new FormatException( $"{value} is not a valid note name" );
    }

    return result;
  }

  /// <summary>Subtracts a number of steps from a note name.</summary>
  /// <param name="steps">The number of steps to subtract.</param>
  /// <returns>A NoteName.</returns>
  [Pure]
  public NoteName Subtract( int steps )
  {
    return Add( -steps );
  }

  /// <summary>Returns to number of note names between two note names.</summary>
  /// <param name="name">The last note name.</param>
  /// <returns>A NoteName.</returns>
  [Pure]
  public int Subtract( NoteName name )
  {
    return (int) Add( -(int) name );
  }

  /// <inheritdoc />
  public override string ToString()
  {
    return s_names[_value].ToString();
  }

  /// <summary>Attempts to parse a NoteName from the given string.</summary>
  /// <param name="s">The value to parse.</param>
  /// <param name="noteName">[out] The note name.</param>
  /// <returns>True if it succeeds, false if it fails.</returns>
  public static bool TryParse(
    string s,
    out NoteName noteName )
  {
    if( string.IsNullOrWhiteSpace( s ) )
    {
      noteName = C;
      return false;
    }

    var value = s_names.IndexOf( char.ToUpperInvariant( s[0] ) );
    if( value == -1 )
    {
      noteName = C;
      return false;
    }

    noteName = new NoteName( value );
    return true;
  }

#endregion

#region Operators

  /// <summary>Explicit cast that converts the given NoteName to an int.</summary>
  /// <param name="noteName">The note name.</param>
  /// <returns>The result of the operation.</returns>
  public static explicit operator int( NoteName noteName )
  {
    return noteName._value;
  }

  /// <summary>Explicit cast that converts the given int to a NoteName.</summary>
  /// <param name="value">The value.</param>
  /// <returns>The result of the operation.</returns>
  public static explicit operator NoteName( int value )
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
  public static NoteName operator ++( NoteName noteName )
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
  public static NoteName operator --( NoteName noteName )
  {
    return noteName.Subtract( 1 );
  }

#endregion
}
