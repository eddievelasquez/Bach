// Module Name: IntervalQuality.cs
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

/// <summary>Values that represent interval qualities.</summary>
public readonly struct IntervalQuality
  : IEquatable<IntervalQuality>,
    IComparable<IntervalQuality>,
    IComparable
{
#region Constants

  /// <summary>A  constant representing a diminished interval.</summary>
  public static readonly IntervalQuality Diminished = new( 0 );

  /// <summary>A  constant representing a minor interval.</summary>
  public static readonly IntervalQuality Minor = new( 1 );

  /// <summary>A  constant representing a perfect interval.</summary>
  public static readonly IntervalQuality Perfect = new( 2 );

  /// <summary>A  constant representing a major interval.</summary>
  public static readonly IntervalQuality Major = new( 3 );

  /// <summary>A  constant representing an augmented interval.</summary>
  public static readonly IntervalQuality Augmented = new( 4 );

  private static readonly string[] s_symbols = { "d", "m", "P", "M", "A" };
  private static readonly string[] s_short = { "dim", "min", "Perf", "Maj", "Aug" };
  private static readonly string[] s_long = { "Diminished", "Minor", "Perfect", "Major", "Augmented" };

#endregion

#region Fields

  private readonly int _value;

#endregion

#region Constructors

  private IntervalQuality( int value )
  {
    Requires.Between( value, 0, 4 );
    _value = value;
  }

#endregion

#region Properties

  /// <summary>Returns the symbol for the given interval quality.</summary>
  /// <value>A string.</value>
  public string Symbol => s_symbols[_value];

  /// <summary>Returns the short name for the given interval quality.</summary>
  /// <value>A string.</value>
  public string ShortName => s_short[_value];

  /// <summary>Returns the long name for the given interval quality.</summary>
  /// <value>A string.</value>
  public string LongName => s_long[_value];

#endregion

#region Public Methods

  /// <summary>Adds a number of semitones to a pitch class name.</summary>
  /// <param name="semitones">The number of semitones to add.</param>
  /// <returns>A IntervalQuality.</returns>
  [Pure]
  public IntervalQuality Add( int semitones )
  {
    var result = new IntervalQuality( _value + semitones );
    return result;
  }

  /// <inheritdoc />
  public int CompareTo( object? obj )
  {
    if( obj is null )
    {
      return 1;
    }

    return obj is IntervalQuality other
             ? CompareTo( other )
             : throw new ArgumentException( $"Object must be of type {nameof( IntervalQuality )}" );
  }

  /// <inheritdoc />
  public int CompareTo( IntervalQuality other )
  {
    return _value.CompareTo( other._value );
  }

  /// <inheritdoc />
  public bool Equals( IntervalQuality other )
  {
    return _value == other._value;
  }

  /// <inheritdoc />
  public override bool Equals( object? obj )
  {
    return obj is IntervalQuality other && Equals( other );
  }

  /// <inheritdoc />
  public override int GetHashCode()
  {
    return _value;
  }

  /// <summary>
  ///   Converts the specified string representation of an interval quality to its <see cref="IntervalQuality" />
  ///   equivalent.
  /// </summary>
  /// <param name="value">A string containing the interval quality to convert.</param>
  /// <returns>An object that is equivalent to the interval quality contained in value.</returns>
  /// <exception cref="FormatException">value does not contain a valid string representation of an interval quality.</exception>
  public static IntervalQuality Parse( string value )
  {
    if( !TryParse( value, out var quality ) )
    {
      throw new FormatException( $"\"{value}\" is not a valid interval quality" );
    }

    return quality;
  }

  /// <summary>Subtracts a number of semitones from a pitch class name.</summary>
  /// <param name="semitones">The number of semitones to subtract.</param>
  /// <returns>A IntervalQuality.</returns>
  [Pure]
  public IntervalQuality Subtract( int semitones )
  {
    return Add( -semitones );
  }

  /// <inheritdoc />
  public override string ToString()
  {
    return LongName;
  }

  /// <summary>
  ///   Converts the specified string representation of an interval quality to its <see cref="IntervalQuality" />
  ///   equivalent
  ///   and returns a value that indicates whether the conversion succeeded.
  /// </summary>
  /// <param name="value">A string containing the interval quality to convert.</param>
  /// <param name="quality">
  ///   When this method returns, contains the IntervalQuality value equivalent to the interval quality
  ///   contained in value, if the conversion succeeded; otherwise, the value is undefined if the conversion failed.
  ///   The conversion fails if the value parameter is null or empty  or does not contain a valid string
  ///   representation of an interval quality. This parameter is passed uninitialized.
  /// </param>
  /// <returns>
  ///   <see langword="true" /> if the value parameter was converted successfully; otherwise, <see langword="false" />
  ///   .
  /// </returns>
  public static bool TryParse(
    string value,
    out IntervalQuality quality )
  {
    if( !string.IsNullOrEmpty( value ) )
    {
      return TryParse( value[0], out quality );
    }

    quality = Perfect;
    return false;
  }

  /// <summary>
  ///   Converts the specified character representation of an interval quality to its <see cref="IntervalQuality" />
  ///   equivalent
  ///   and returns a value that indicates whether the conversion succeeded.
  /// </summary>
  /// <param name="value">A character containing the interval quality to convert.</param>
  /// <param name="quality">
  ///   When this method returns, contains the IntervalQuality value equivalent to the interval quality
  ///   contained in value, if the conversion succeeded; otherwise, the value is undefined if the conversion failed.
  ///   The conversion fails if the value parameter is null or empty  or does not contain a valid string
  ///   representation of an interval quality. This parameter is passed uninitialized.
  /// </param>
  /// <returns>
  ///   <see langword="true" /> if the value parameter was converted successfully; otherwise, <see langword="false" />
  ///   .
  /// </returns>
  public static bool TryParse(
    char value,
    out IntervalQuality quality )
  {
    if( value != '\0' )
    {
      for( var i = 0; i < s_symbols.Length; i++ )
      {
        if( s_symbols[i][0] != value )
        {
          continue;
        }

        quality = new IntervalQuality( i );
        return true;
      }
    }

    quality = Perfect;
    return false;
  }

#endregion

#region Operators

  /// <summary>Explicit cast that converts the given IntervalQuality to an int.</summary>
  /// <param name="quality">The pitch class name.</param>
  /// <returns>The result of the operation.</returns>
  public static explicit operator int( IntervalQuality quality )
  {
    return quality._value;
  }

  /// <summary>Explicit cast that converts the given int to a IntervalQuality.</summary>
  /// <param name="value">The value.</param>
  /// <returns>The result of the operation.</returns>
  public static explicit operator IntervalQuality( int value )
  {
    return new IntervalQuality( value );
  }

  /// <summary>Equality operator.</summary>
  /// <param name="left">The first instance to compare.</param>
  /// <param name="right">The second instance to compare.</param>
  /// <returns>The result of the operation.</returns>
  public static bool operator ==(
    IntervalQuality left,
    IntervalQuality right )
  {
    return left.Equals( right );
  }

  /// <summary>Inequality operator.</summary>
  /// <param name="left">The first instance to compare.</param>
  /// <param name="right">The second instance to compare.</param>
  /// <returns>The result of the operation.</returns>
  public static bool operator !=(
    IntervalQuality left,
    IntervalQuality right )
  {
    return !left.Equals( right );
  }

  /// <summary>Lesser-than comparison operator.</summary>
  /// <param name="left">The first instance to compare.</param>
  /// <param name="right">The second instance to compare.</param>
  /// <returns>The result of the operation.</returns>
  public static bool operator <(
    IntervalQuality left,
    IntervalQuality right )
  {
    return left.CompareTo( right ) < 0;
  }

  /// <summary>Greater-than comparison operator.</summary>
  /// <param name="left">The first instance to compare.</param>
  /// <param name="right">The second instance to compare.</param>
  /// <returns>The result of the operation.</returns>
  public static bool operator >(
    IntervalQuality left,
    IntervalQuality right )
  {
    return left.CompareTo( right ) > 0;
  }

  /// <summary>Lesser-than-or-equal comparison operator.</summary>
  /// <param name="left">The first instance to compare.</param>
  /// <param name="right">The second instance to compare.</param>
  /// <returns>The result of the operation.</returns>
  public static bool operator <=(
    IntervalQuality left,
    IntervalQuality right )
  {
    return left.CompareTo( right ) <= 0;
  }

  /// <summary>Greater-than-or-equal comparison operator.</summary>
  /// <param name="left">The first instance to compare.</param>
  /// <param name="right">The second instance to compare.</param>
  /// <returns>The result of the operation.</returns>
  public static bool operator >=(
    IntervalQuality left,
    IntervalQuality right )
  {
    return left.CompareTo( right ) >= 0;
  }

  /// <summary>Addition operator.</summary>
  /// <param name="quality">The first value.</param>
  /// <param name="semitones">A number of semitones to add to it.</param>
  /// <returns>The result of the operation.</returns>
  public static IntervalQuality operator +(
    IntervalQuality quality,
    int semitones )
  {
    return quality.Add( semitones );
  }

  /// <summary>Increment operator.</summary>
  /// <param name="quality">The interval quality to increment.</param>
  /// <returns>The result of the operation.</returns>
  public static IntervalQuality operator ++( IntervalQuality quality )
  {
    return quality.Add( 1 );
  }

  /// <summary>Subtraction operator.</summary>
  /// <param name="quality">The first value.</param>
  /// <param name="semitones">A number of semitones to subtract from it.</param>
  /// <returns>The result of the operation.</returns>
  public static IntervalQuality operator -(
    IntervalQuality quality,
    int semitones )
  {
    return quality.Subtract( semitones );
  }

  /// <summary>Decrement operator.</summary>
  /// <param name="quality">The interval quality to decrement.</param>
  /// <returns>The result of the operation.</returns>
  public static IntervalQuality operator --( IntervalQuality quality )
  {
    return quality.Subtract( 1 );
  }

#endregion
}
