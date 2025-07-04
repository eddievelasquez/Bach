// Module Name: Interval.cs
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

using System.Diagnostics;
using System.Text;

/// <summary>An interval.</summary>
public readonly struct Interval
  : IEquatable<Interval>,
    IComparable<Interval>
{
  #region Constants

  private const string SYMBOL_QUANTITY_TO_STRING_FORMAT = "sq";

  private static readonly int[][] s_quantitySemitones =
  [
    // Diminished, Minor, Perfect, Major, Augmented
    [-1, -1, 00, -1, 01], // First
    [00, 01, -1, 02, 03], // Second
    [02, 03, -1, 04, 05], // Third
    [04, -1, 05, -1, 06], // Fourth
    [06, -1, 07, -1, 08], // Fifth
    [07, 08, -1, 09, 10], // Sixth
    [09, 10, -1, 11, 12], // Seventh
    [11, -1, 12, -1, 13], // Octave
    [12, 13, -1, 14, 15], // Ninth (Second)
    [14, 15, -1, 16, 17], // Tenth (Third)
    [16, -1, 17, -1, 18], // Eleventh (Fourth)
    [18, -1, 19, -1, 20], // Twelfth (Fifth)
    [19, 20, -1, 21, 22], // Thirteenth (Sixth)
    [21, 22, -1, 23, 24] // Fourteenth (Seventh)
  ];

  /// <summary>
  ///   The Unison interval
  /// </summary>
  public static readonly Interval Unison = new( IntervalQuantity.Unison, IntervalQuality.Perfect );

  /// <summary>
  ///   The augmented Unison interval
  /// </summary>
  public static readonly Interval AugmentedFirst = new( IntervalQuantity.Unison, IntervalQuality.Augmented );

  /// <summary>
  ///   The diminished Second interval
  /// </summary>
  public static readonly Interval DiminishedSecond = new( IntervalQuantity.Second, IntervalQuality.Diminished );

  /// <summary>
  ///   The minor Second interval
  /// </summary>
  public static readonly Interval MinorSecond = new( IntervalQuantity.Second, IntervalQuality.Minor );

  /// <summary>
  ///   The major Second interval
  /// </summary>
  public static readonly Interval MajorSecond = new( IntervalQuantity.Second, IntervalQuality.Major );

  /// <summary>
  ///   The augmented Second interval
  /// </summary>
  public static readonly Interval AugmentedSecond = new( IntervalQuantity.Second, IntervalQuality.Augmented );

  /// <summary>
  ///   The diminished Third interval
  /// </summary>
  public static readonly Interval DiminishedThird = new( IntervalQuantity.Third, IntervalQuality.Diminished );

  /// <summary>
  ///   The minor Third interval
  /// </summary>
  public static readonly Interval MinorThird = new( IntervalQuantity.Third, IntervalQuality.Minor );

  /// <summary>
  ///   The major Third interval
  /// </summary>
  public static readonly Interval MajorThird = new( IntervalQuantity.Third, IntervalQuality.Major );

  /// <summary>
  ///   The augmented Third interval
  /// </summary>
  public static readonly Interval AugmentedThird = new( IntervalQuantity.Third, IntervalQuality.Augmented );

  /// <summary>
  ///   The diminished Fourth interval
  /// </summary>
  public static readonly Interval DiminishedFourth = new( IntervalQuantity.Fourth, IntervalQuality.Diminished );

  /// <summary>
  ///   The perfect Fourth interval
  /// </summary>
  public static readonly Interval Fourth = new( IntervalQuantity.Fourth, IntervalQuality.Perfect );

  /// <summary>
  ///   The augmented Fourth interval
  /// </summary>
  public static readonly Interval AugmentedFourth = new( IntervalQuantity.Fourth, IntervalQuality.Augmented );

  /// <summary>
  ///   The diminished Fifth interval
  /// </summary>
  public static readonly Interval DiminishedFifth = new( IntervalQuantity.Fifth, IntervalQuality.Diminished );

  /// <summary>
  ///   The perfect Fifth interval
  /// </summary>
  public static readonly Interval Fifth = new( IntervalQuantity.Fifth, IntervalQuality.Perfect );

  /// <summary>
  ///   The augmented Fifth interval
  /// </summary>
  public static readonly Interval AugmentedFifth = new( IntervalQuantity.Fifth, IntervalQuality.Augmented );

  /// <summary>
  ///   The diminished Sixth interval
  /// </summary>
  public static readonly Interval DiminishedSixth = new( IntervalQuantity.Sixth, IntervalQuality.Diminished );

  /// <summary>
  ///   The minor Sixth interval
  /// </summary>
  public static readonly Interval MinorSixth = new( IntervalQuantity.Sixth, IntervalQuality.Minor );

  /// <summary>
  ///   The major Sixth interval
  /// </summary>
  public static readonly Interval MajorSixth = new( IntervalQuantity.Sixth, IntervalQuality.Major );

  /// <summary>
  ///   The augmented Sixth interval
  /// </summary>
  public static readonly Interval AugmentedSixth = new( IntervalQuantity.Sixth, IntervalQuality.Augmented );

  /// <summary>
  ///   The diminished Seventh interval
  /// </summary>
  public static readonly Interval DiminishedSeventh = new( IntervalQuantity.Seventh, IntervalQuality.Diminished );

  /// <summary>
  ///   The minor Seventh interval
  /// </summary>
  public static readonly Interval MinorSeventh = new( IntervalQuantity.Seventh, IntervalQuality.Minor );

  /// <summary>
  ///   The major Seventh interval
  /// </summary>
  public static readonly Interval MajorSeventh = new( IntervalQuantity.Seventh, IntervalQuality.Major );

  /// <summary>
  ///   The augmented Seventh interval
  /// </summary>
  public static readonly Interval AugmentedSeventh = new( IntervalQuantity.Seventh, IntervalQuality.Augmented );

  /// <summary>
  ///   The diminished Octave interval
  /// </summary>
  public static readonly Interval DiminishedOctave = new( IntervalQuantity.Octave, IntervalQuality.Diminished );

  /// <summary>
  ///   The Octave interval
  /// </summary>
  public static readonly Interval Octave = new( IntervalQuantity.Octave, IntervalQuality.Perfect );

  #endregion

  #region Fields

  private readonly byte _quality;
  private readonly byte _quantity;
  private readonly byte _semitoneCount;

  #endregion

  #region Constructors

  /// <summary>
  ///   Initializes a new instance of the Interval class.
  /// </summary>
  /// <param name="quantity">The quantity of the interval.</param>
  /// <param name="quality">The quality of the interval.</param>
  public Interval(
    IntervalQuantity quantity,
    IntervalQuality quality )
  {
    Debug.Assert( quantity >= IntervalQuantity.Unison );
    Debug.Assert( quantity <= IntervalQuantity.Fourteenth );
    Debug.Assert( quality >= IntervalQuality.Diminished );
    Debug.Assert( quality <= IntervalQuality.Augmented );

    _semitoneCount = (byte) GetSemitoneCount( quantity, quality );
    _quantity = (byte) quantity;
    _quality = (byte) quality;
  }

  /// <summary>Constructor.</summary>
  /// <param name="quantity">The interval quantity.</param>
  /// <param name="semitoneCount">The number of semitones in the interval.</param>
  internal Interval(
    IntervalQuantity quantity,
    int semitoneCount )
  {
    var pos = Array.IndexOf( s_quantitySemitones[(int) quantity], semitoneCount );
    Debug.Assert( pos != -1, $"A {quantity} with {semitoneCount} is not a valid interval" );
    Debug.Assert( pos is >= 0 and <= 24 );

    _quantity = (byte) quantity;
    _quality = (byte) pos;
    _semitoneCount = (byte) semitoneCount;
  }

  #endregion

  #region Properties

  /// <summary>Gets the interval's quantity.</summary>
  /// <value>The quantity.</value>
  public IntervalQuantity Quantity => (IntervalQuantity) _quantity;

  /// <summary>Gets the interval's quality.</summary>
  /// <value>The quality.</value>
  public IntervalQuality Quality => (IntervalQuality) _quality;

  /// <summary>
  ///   Gets the interval's inversion. An interval and its inversion always add up to an octave.
  /// </summary>
  public Interval Inversion
  {
    get
    {
      var newQuantity = (IntervalQuantity) ( 8 - (int) Quantity - 1 );
      var newQuality = IntervalQuality.Augmented - (int) Quality;

      var result = new Interval( newQuantity, newQuality );
      return result;
    }
  }

  /// <summary>Gets the number of semitones in the interval.</summary>
  /// <value>The number of semitones.</value>
  public int SemitoneCount => _semitoneCount;

  #endregion

  #region Public Methods

  /// <inheritdoc />
  public int CompareTo(
    Interval other )
  {
    var result = _quantity - other._quantity;
    if( result != 0 )
    {
      return result;
    }

    result = _quality - other._quality;
    return result;
  }

  /// <inheritdoc />
  public bool Equals(
    Interval other )
  {
    return other.Quantity == Quantity && other.Quality == Quality;
  }

  /// <inheritdoc />
  public override bool Equals(
    object? obj )
  {
    return obj is Interval other && Equals( other );
  }

  /// <inheritdoc />
  public override int GetHashCode()
  {
    var hashCode = ( _quality << 8 ) | _quantity;
    return hashCode;
  }

  /// <summary>Gets semitone count of the interval.</summary>
  /// <exception cref="ArgumentOutOfRangeException">
  ///   Thrown when one or more arguments are outside the
  ///   required range.
  /// </exception>
  /// <exception cref="ArgumentException">
  ///   Thrown when the interval quantity and quality combination doesn't refer to a valid
  ///   interval.
  /// </exception>
  /// <param name="quantity">The interval quantity.</param>
  /// <param name="quality">The interval quality.</param>
  /// <returns>The semitone count.</returns>
  public static int GetSemitoneCount(
    IntervalQuantity quantity,
    IntervalQuality quality )
  {
    ArgumentOutOfRangeException.ThrowIfLessThan( (int) quantity, (int) IntervalQuantity.Unison );
    ArgumentOutOfRangeException.ThrowIfGreaterThan( (int) quantity, (int) IntervalQuantity.Fourteenth );
    ArgumentOutOfRangeException.ThrowIfLessThan( quality, IntervalQuality.Diminished );
    ArgumentOutOfRangeException.ThrowIfGreaterThan( quality, IntervalQuality.Augmented );

    var steps = s_quantitySemitones[(int) quantity][(int) quality];
    if( steps == -1 )
    {
      throw new ArgumentException( $"{quantity}{quality} is not a valid interval" );
    }

    return steps;
  }

  /// <summary>Determine if the interval quantity and quality refer to a valid interval.</summary>
  /// <param name="quantity">The interval quantity.</param>
  /// <param name="quality">The interval quality.</param>
  /// <returns>True if valid, false if not.</returns>
  public static bool IsValid(
    IntervalQuantity quantity,
    IntervalQuality quality )
  {
    if( quantity < IntervalQuantity.Unison
        || quantity > IntervalQuantity.Fourteenth
        || quality < IntervalQuality.Diminished
        || quality > IntervalQuality.Augmented )
    {
      return false;
    }

    var steps = s_quantitySemitones[(int) quantity][(int) quality];
    return steps != -1;
  }

  /// <summary>
  ///   Converts the specified string representation of an interval to its <see cref="Interval" /> equivalent.
  /// </summary>
  /// <param name="value">A string containing the interval to convert.</param>
  /// <returns>An object that is equivalent to the interval contained in value.</returns>
  /// <exception cref="FormatException">value does not contain a valid string representation of an interval.</exception>
  public static Interval Parse(
    string value )
  {
    if( !TryParse( value, out var interval ) )
    {
      throw new FormatException( value + " is not a valid interval" );
    }

    return interval;
  }

  /// <summary>Returns the fully qualified type name of this instance.</summary>
  /// <returns>The fully qualified type name.</returns>
  public override string ToString()
  {
    return ToString( SYMBOL_QUANTITY_TO_STRING_FORMAT, null );
  }

  /// <summary>
  ///   Returns a string representation of the value of this <see cref="Interval" /> instance, according to the
  ///   provided format specifier.
  /// </summary>
  /// <param name="format">A custom format string.</param>
  /// <returns>
  ///   A string representation of the value of the current <see cref="Interval" /> object as specified by
  ///   <paramref name="format" />.
  /// </returns>
  /// <remarks>
  ///   <para>"s": Symbol pattern. e.g. (m)minor, (d)diminished, (A)augmented. Excludes perfect and major.</para>
  ///   <para>"S": Symbol pattern. e.g. (P)perfect, (M)major, (m)minor, (d)diminished, (A)augmented.</para>
  ///   <para>"q": Numeric quantity pattern. e.g. 1, 2, 3, etc.</para>
  ///   <para>"Q": Ordinal quantity pattern. e.g. First, Second, Third.</para>
  /// </remarks>
  public string ToString(
    string format )
  {
    return ToString( format, null );
  }

  /// <summary>
  ///   Returns a string representation of the value of this <see cref="Interval" /> instance, according to the
  ///   provided format specifier and format provider.
  /// </summary>
  /// <param name="format">A custom format string.</param>
  /// <param name="provider">The format provider. (Currently unused)</param>
  /// <returns>
  ///   A string representation of the value of the current <see cref="Interval" /> object as specified by
  ///   <paramref name="format" />.
  /// </returns>
  /// <remarks>
  ///   <para>"s": Symbol pattern. e.g. (m)minor, (d)diminished, (A)augmented. Excludes perfect and major.</para>
  ///   <para>"S": Symbol pattern. e.g. (P)perfect, (M)major, (m)minor, (d)diminished, (A)augmented.</para>
  ///   <para>"q": Numeric quantity pattern. e.g. 1, 2, 3, etc.</para>
  ///   <para>"Q": Ordinal quantity pattern. e.g. First, Second, Third.</para>
  /// </remarks>
  public string ToString(
    string format,
    IFormatProvider? provider )
  {
    var buf = new StringBuilder();
    foreach( var f in format )
    {
      switch( f )
      {
        case 's':
        {
          if( Quality != IntervalQuality.Perfect && Quality != IntervalQuality.Major )
          {
            buf.Append( Quality.Symbol );
          }

          break;
        }

        case 'S':
          buf.Append( Quality.Symbol );
          break;

        case 'q':
          buf.Append( (int) ( Quantity + 1 ) );
          break;

        case 'Q':
          buf.Append( Quantity );
          break;

        default:
          buf.Append( f );
          break;
      }
    }

    return buf.ToString();
  }

  /// <summary>
  ///   Converts the specified string representation of an interval to its <see cref="Interval" /> equivalent
  ///   and returns a value that indicates whether the conversion succeeded.
  /// </summary>
  /// <param name="value">A string containing the interval quality to convert.</param>
  /// <param name="interval">
  ///   When this method returns, contains the Interval value equivalent to the interval
  ///   contained in value, if the conversion succeeded; otherwise, the value is undefined if the conversion failed.
  ///   The conversion fails if the value parameter is null or empty or does not contain a valid string
  ///   representation of an interval. This parameter is passed uninitialized.
  /// </param>
  /// <returns>
  ///   <see langword="true" /> if the value parameter was converted successfully; otherwise, <see langword="false" />
  ///   .
  /// </returns>
  public static bool TryParse(
    string? value,
    out Interval interval )
  {
    if( !string.IsNullOrWhiteSpace( value ) )
    {
      return TryParse( value.AsSpan(), out interval );
    }

    interval = Unison;
    return false;
  }

  /// <summary>
  ///   Converts the specified character span representation of an interval to its <see cref="Interval" /> equivalent
  ///   and returns a value that indicates whether the conversion succeeded.
  /// </summary>
  /// <param name="value">A read-only character span containing the interval quality to convert.</param>
  /// <param name="interval">
  ///   When this method returns, contains the Interval value equivalent to the interval
  ///   contained in value, if the conversion succeeded; otherwise, the value is undefined if the conversion failed.
  ///   The conversion fails if the value parameter is null or empty or does not contain a valid string
  ///   representation of an interval. This parameter is passed uninitialized.
  /// </param>
  /// <returns>
  ///   <see langword="true" /> if the value parameter was converted successfully; otherwise, <see langword="false" />.
  /// </returns>
  public static bool TryParse(
    ReadOnlySpan<char> value,
    out Interval interval )
  {
    interval = Unison;

    if( value.IsEmpty )
    {
      return false;
    }

    var i = 0;

    // skip whitespaces
    while( i < value.Length && char.IsWhiteSpace( value[i] ) )
    {
      ++i;
    }

    var quality = IntervalQuality.Perfect;
    var hasExplicitQuality = char.IsLetter( value[i] );

    if( hasExplicitQuality )
    {
      if( value[i] == 'R' )
      {
        interval = Unison;
        return true;
      }

      if( !IntervalQuality.TryParse( value[i], out quality ) )
      {
        return false;
      }

      ++i;
    }

    // Check if we have a double-digit compound interval and adjust the length accordingly
    var intervalSpan = value[i..];
    var intervalNumberLength = 1;

    if( intervalSpan.Length > 1 && char.IsDigit( value[1] ) )
    {
      ++intervalNumberLength;
    }

    if( !int.TryParse( value.Slice( i, intervalNumberLength ), out var number ) )
    {
      return false;
    }

    if( !hasExplicitQuality )
    {
      var temp = ( number - 1 ) % 7 + 1;
      quality = temp is 1 or 4 or 5 ? IntervalQuality.Perfect : IntervalQuality.Major;
    }

    var quantity = (IntervalQuantity) ( number - 1 );
    if( !IsValid( quantity, quality ) )
    {
      return false;
    }

    interval = new Interval( quantity, quality );
    return true;
  }

  #endregion

  #region Operators

  /// <summary>Explicit cast that converts the given Interval to an int.</summary>
  /// <param name="interval">The interval.</param>
  /// <returns>The result of the operation.</returns>
  public static explicit operator int(
    Interval interval )
  {
    return ( interval._quality << 8 ) | interval._quantity;
  }

  /// <summary>Explicit cast that converts the given int to an Interval.</summary>
  /// <param name="value">The value.</param>
  /// <returns>The result of the operation.</returns>
  public static explicit operator Interval(
    int value )
  {
    var quantity = (IntervalQuantity) ( value & 0xFF );
    var quality = (IntervalQuality) ( ( value >> 8 ) & 0xFF );
    return new Interval( quantity, quality );
  }

  /// <summary>Equality operator.</summary>
  /// <param name="lhs">The first instance to compare.</param>
  /// <param name="rhs">The second instance to compare.</param>
  /// <returns>The result of the operation.</returns>
  public static bool operator ==(
    Interval lhs,
    Interval rhs )
  {
    return Equals( lhs, rhs );
  }

  /// <summary>Inequality operator.</summary>
  /// <param name="lhs">The first instance to compare.</param>
  /// <param name="rhs">The second instance to compare.</param>
  /// <returns>The result of the operation.</returns>
  public static bool operator !=(
    Interval lhs,
    Interval rhs )
  {
    return !Equals( lhs, rhs );
  }

  /// <summary>Lesser-than comparison operator.</summary>
  /// <param name="lhs">The first instance to compare.</param>
  /// <param name="rhs">The second instance to compare.</param>
  /// <returns>The result of the operation.</returns>
  public static bool operator <(
    Interval lhs,
    Interval rhs )
  {
    return lhs.CompareTo( rhs ) < 0;
  }

  /// <summary>Lesser-than-or-equal comparison operator.</summary>
  /// <param name="lhs">The first instance to compare.</param>
  /// <param name="rhs">The second instance to compare.</param>
  /// <returns>The result of the operation.</returns>
  public static bool operator <=(
    Interval lhs,
    Interval rhs )
  {
    return lhs.CompareTo( rhs ) <= 0;
  }

  /// <summary>Greater-than comparison operator.</summary>
  /// <param name="lhs">The first instance to compare.</param>
  /// <param name="rhs">The second instance to compare.</param>
  /// <returns>The result of the operation.</returns>
  public static bool operator >(
    Interval lhs,
    Interval rhs )
  {
    return lhs.CompareTo( rhs ) > 0;
  }

  /// <summary>Greater-than-or-equal comparison operator.</summary>
  /// <param name="lhs">The first instance to compare.</param>
  /// <param name="rhs">The second instance to compare.</param>
  /// <returns>The result of the operation.</returns>
  public static bool operator >=(
    Interval lhs,
    Interval rhs )
  {
    return lhs.CompareTo( rhs ) >= 0;
  }

  #endregion
}
