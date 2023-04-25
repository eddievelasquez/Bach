// Module Name: Accidental.cs
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
///   An Accidental represents a modification to a <see cref="Accidental" />
///   by raising or lowering its relative pitch.
/// </summary>
public readonly struct Accidental
  : IEquatable<Accidental>,
    IComparable<Accidental>,
    IComparable
{
#region Constants

  /// <summary>
  ///   Double flat (𝄫)
  /// </summary>
  public static readonly Accidental DoubleFlat = new( -2 );

  /// <summary>
  ///   Flat (♭)
  /// </summary>
  public static readonly Accidental Flat = new( -1 );

  /// <summary>
  ///   Natural (♮)
  /// </summary>
  public static readonly Accidental Natural = new( 0 );

  /// <summary>
  ///   Sharp (♯)
  /// </summary>
  public static readonly Accidental Sharp = new( 1 );

  /// <summary>
  ///   Double Sharp (♯♯)
  /// </summary>
  public static readonly Accidental DoubleSharp = new( 2 );

  private static readonly string[] s_symbols = { "bb", "b", "", "#", "##" };
  private static readonly string[] s_names = { "DoubleFlat", "Flat", "Natural", "Sharp", "DoubleSharp" };
  private static readonly int s_doubleFlatOffset = Math.Abs( (int) DoubleFlat );

#endregion

#region Fields

  private readonly int _value;

#endregion

#region Constructors

  private Accidental( int value )
  {
    Requires.Between( value, -2, 2 );
    _value = value;
  }

#endregion

#region Public Methods

  /// <summary>Adds a number of steps to a pitch class name.</summary>
  /// <param name="steps">The number of steps to add.</param>
  /// <returns>A Accidental.</returns>
  [Pure]
  public Accidental Add( int steps )
  {
    var result = new Accidental( _value + steps );
    return result;
  }

  /// <inheritdoc />
  public int CompareTo( object? obj )
  {
    if( obj is null )
    {
      return 1;
    }

    return obj is Accidental other
             ? CompareTo( other )
             : throw new ArgumentException( $"Object must be of type {nameof( Accidental )}" );
  }

  /// <inheritdoc />
  public int CompareTo( Accidental other )
  {
    return _value.CompareTo( other._value );
  }

  /// <inheritdoc />
  public bool Equals( Accidental other )
  {
    return _value == other._value;
  }

  /// <inheritdoc />
  public override bool Equals( object? obj )
  {
    return obj is Accidental other && Equals( other );
  }

  /// <inheritdoc />
  public override int GetHashCode()
  {
    return _value;
  }

  /// <summary>
  ///   Converts the specified string representation of an accidental to its <see cref="Accidental" /> equivalent.
  /// </summary>
  /// <param name="value">A string containing the accidental to convert.</param>
  /// <returns>An object that is equivalent to the accidental contained in value.</returns>
  /// <exception cref="FormatException">value does not contain a valid string representation of an accidental.</exception>
  public static Accidental Parse( string value )
  {
    if( !TryParse( value, out var accidental ) )
    {
      throw new FormatException( $"{value} is not a valid accidental" );
    }

    return accidental;
  }

  /// <summary>Subtracts a number of steps from a pitch class name.</summary>
  /// <param name="steps">The number of steps to subtract.</param>
  /// <returns>A Accidental.</returns>
  [Pure]
  public Accidental Subtract( int steps )
  {
    return Add( -steps );
  }

  /// <inheritdoc />
  public override string ToString()
  {
    return s_names[_value + s_doubleFlatOffset];
  }

  /// <summary>
  ///   Returns this instance's symbolic representation.
  /// </summary>
  /// <returns>String representation of the accidental.</returns>
  [Pure]
  public string ToSymbol()
  {
    return s_symbols[_value + s_doubleFlatOffset];
  }

  /// <summary>
  ///   Converts the specified string representation of an accidental to its <see cref="Accidental" /> equivalent
  ///   and returns a value that indicates whether the conversion succeeded.
  /// </summary>
  /// <param name="value">A string containing the accidental to convert.</param>
  /// <param name="accidental">
  ///   When this method returns, contains the Accidental value equivalent to accidental
  ///   contained in value, if the conversion succeeded, or Natural if the conversion failed.
  ///   The conversion fails if the s parameter is longer than 2 characters or does not contain a valid string
  ///   representation of an accidental. This parameter is passed uninitialized.
  /// </param>
  /// <returns>
  ///   <see langword="true" /> if the value parameter was converted successfully; otherwise, <see langword="false" />
  ///   .
  /// </returns>
  public static bool TryParse(
    string value,
    out Accidental accidental )
  {
    accidental = Natural;
    if( string.IsNullOrEmpty( value ) )
    {
      return true;
    }

    if( value.Length > 2 )
    {
      return false;
    }

    var accidentalValue = 0;
    foreach( var c in value )
    {
      switch( c )
      {
        case '♮':
          // The accidental can only be a valid natural if it's the single character
          return value.Length == 1;

        case 'b':
        case 'B':
        case '♭':
          --accidentalValue;
          break;

        case '#':
        case '♯':
          ++accidentalValue;
          break;

        default:
          return false;
      }
    }

    // Cannot be natural unless the "b#" or "#b" combinations are found
    accidental = new Accidental( accidentalValue );
    return accidental != Natural;
  }

#endregion

#region Operators

  /// <summary>Explicit cast that converts the given Accidental to an int.</summary>
  /// <param name="accidental">The pitch class name.</param>
  /// <returns>The result of the operation.</returns>
  public static explicit operator int( Accidental accidental )
  {
    return accidental._value;
  }

  /// <summary>Explicit cast that converts the given int to a Accidental.</summary>
  /// <param name="value">The value.</param>
  /// <returns>The result of the operation.</returns>
  public static explicit operator Accidental( int value )
  {
    return new Accidental( value );
  }

  /// <summary>Equality operator.</summary>
  /// <param name="left">The first instance to compare.</param>
  /// <param name="right">The second instance to compare.</param>
  /// <returns>The result of the operation.</returns>
  public static bool operator ==(
    Accidental left,
    Accidental right )
  {
    return left.Equals( right );
  }

  /// <summary>Inequality operator.</summary>
  /// <param name="left">The first instance to compare.</param>
  /// <param name="right">The second instance to compare.</param>
  /// <returns>The result of the operation.</returns>
  public static bool operator !=(
    Accidental left,
    Accidental right )
  {
    return !left.Equals( right );
  }

  /// <summary>Lesser-than comparison operator.</summary>
  /// <param name="left">The first instance to compare.</param>
  /// <param name="right">The second instance to compare.</param>
  /// <returns>The result of the operation.</returns>
  public static bool operator <(
    Accidental left,
    Accidental right )
  {
    return left.CompareTo( right ) < 0;
  }

  /// <summary>Greater-than comparison operator.</summary>
  /// <param name="left">The first instance to compare.</param>
  /// <param name="right">The second instance to compare.</param>
  /// <returns>The result of the operation.</returns>
  public static bool operator >(
    Accidental left,
    Accidental right )
  {
    return left.CompareTo( right ) > 0;
  }

  /// <summary>Lesser-than-or-equal comparison operator.</summary>
  /// <param name="left">The first instance to compare.</param>
  /// <param name="right">The second instance to compare.</param>
  /// <returns>The result of the operation.</returns>
  public static bool operator <=(
    Accidental left,
    Accidental right )
  {
    return left.CompareTo( right ) <= 0;
  }

  /// <summary>Greater-than-or-equal comparison operator.</summary>
  /// <param name="left">The first instance to compare.</param>
  /// <param name="right">The second instance to compare.</param>
  /// <returns>The result of the operation.</returns>
  public static bool operator >=(
    Accidental left,
    Accidental right )
  {
    return left.CompareTo( right ) >= 0;
  }

  /// <summary>Addition operator.</summary>
  /// <param name="accidental">The first value.</param>
  /// <param name="semitoneCount">A number of semitones to add to it.</param>
  /// <returns>The result of the operation.</returns>
  public static Accidental operator +(
    Accidental accidental,
    int semitoneCount )
  {
    return accidental.Add( semitoneCount );
  }

  /// <summary>Increment operator.</summary>
  /// <param name="accidental">The pitch class.</param>
  /// <returns>The result of the operation.</returns>
  public static Accidental operator ++( Accidental accidental )
  {
    return accidental.Add( 1 );
  }

  /// <summary>Subtraction operator.</summary>
  /// <param name="accidental">The first value.</param>
  /// <param name="semitoneCount">A number of semitones to subtract from it.</param>
  /// <returns>The result of the operation.</returns>
  public static Accidental operator -(
    Accidental accidental,
    int semitoneCount )
  {
    return accidental.Subtract( semitoneCount );
  }

  /// <summary>Decrement operator.</summary>
  /// <param name="accidental">The pitch class.</param>
  /// <returns>The result of the operation.</returns>
  public static Accidental operator --( Accidental accidental )
  {
    return accidental.Subtract( 1 );
  }

#endregion
}
