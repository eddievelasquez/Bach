// Module Name: Interval.cs
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

using System.Text;

namespace Bach.Model;

/// <summary>An interval.</summary>
public readonly struct Interval
  : IEquatable<Interval>,
    IComparable<Interval>,
    ISpanParsable<Interval>,
    IFormattable
{
  #region Constants

  private const string SYMBOL_QUANTITY_TO_STRING_FORMAT = "sq";

  private const int SHIFT_SEMITONES = 10;
  private const int SHIFT_QUANTITY = 6;
  private const int SHIFT_QUALITY = 3;
  private const int SHIFT_DISPLACEMENT = 0;

  private const ushort MASK_SEMITONES = 0b111111;
  private const ushort MASK_QUANTITY = 0b1111;
  private const ushort MASK_QUALITY = 0b111;
  private const ushort MASK_DISPLACEMENT = 0b111;

  // Base semitone count for each interval quantity (unison, second, third, ..., fourteenth)
  // without considering quality.
  private static readonly int[] s_quantitySemitones = [0, 2, 4, 5, 7, 9, 11, 12, 14, 16, 17, 19, 21, 23];

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

  // Bit layout
  // +---------------------------------------------------------------+
  // | 15            10 | 9        6 | 5        3 | 2        0       |
  // +---------------------------------------------------------------+
  // | Semitone Distance | Quantity  | Quality   | Displacement      |
  // +---------------------------------------------------------------+
  // | signed (−26…+26)  | 0–13      | 0–4       | encoded 0–6       |
  // |        6 bits     | 4 bits    | 3 bits    | 3 bits            |
  // +---------------------------------------------------------------+
  private readonly ushort _value;

  #endregion

  #region Constructors

  /// <summary>
  ///   Initializes a new instance of the Interval class.
  /// </summary>
  /// <param name="quantity">The quantity of the interval.</param>
  /// <param name="quality">The quality of the interval.</param>
  /// <param name="descending">Whether the interval is descending.</param>
  public Interval(
    IntervalQuantity quantity,
    IntervalQuality quality,
    bool descending = false )
    : this( Pack( quantity, quality, GetDisplacement( quantity, quality ), descending ) )
  {
  }

  /// <summary>
  ///   Initializes a new instance of the Interval class with a packed ushort value.
  /// </summary>
  /// <param name="value">The packed <c>ushort</c> value.</param>
  private Interval(
    ushort value )
  {
    _value = value;
  }

  #endregion

  #region Properties

  /// <summary>
  ///   Gets the interval's semitone distance from the unison. This value can be negative for descending intervals.
  /// </summary>
  public int SemitoneCount
  {
    get
    {
      var raw = ( _value >> SHIFT_SEMITONES ) & MASK_SEMITONES;
      return ( raw >= 32 ) ? raw - 64 : raw; // Sign extend
    }
  }

  /// <summary>Gets the interval's quantity.</summary>
  /// <value>The quantity.</value>
  public IntervalQuantity Quantity => (IntervalQuantity) ( ( _value >> SHIFT_QUANTITY ) & MASK_QUANTITY );

  /// <summary>Gets the interval's quality.</summary>
  /// <value>The quality.</value>
  public IntervalQuality Quality => (IntervalQuality) ( ( _value >> SHIFT_QUALITY ) & MASK_QUALITY );

  /// <summary>
  ///   Gets the interval's quality displacement. This is the number of semitones the interval is displaced from its base
  ///   quality. For perfect quantities (unison, fourth, fifth, etc.), the base quality is perfect. For major quantities
  ///   (second, third, sixth, seventh), the base quality is major. The displacement can be negative (diminished) or
  ///   positive (augmented). Up to triple diminished or augmented intervals are supported, hence the range of -3 to +3.
  /// </summary>
  public int Displacement => ( ( _value >> SHIFT_DISPLACEMENT ) & MASK_DISPLACEMENT ) - 3;

  /// <summary>Gets a value indicating whether the interval is ascending.</summary>
  /// <value>True if ascending, false if descending.</value>
  public bool IsAscending => SemitoneCount >= 0;

  /// <summary>Gets a value indicating whether the interval is descending.</summary>
  /// <value>True if descending, false if ascending.</value>
  public bool IsDescending => SemitoneCount < 0;

  /// <summary>
  ///   Gets the interval's inversion. An interval and its inversion always add up to an octave.
  /// </summary>
  public Interval Inversion => new( Quantity.Inverse, Quality.Inverse, !IsDescending );

  #endregion

  #region Public Methods

  /// <inheritdoc/>
  public int CompareTo(
    Interval other )
  {
    return SemitoneCount.CompareTo( other.SemitoneCount );
  }

  /// <inheritdoc/>
  public bool Equals(
    Interval other )
  {
    return other._value == _value;
  }

  /// <inheritdoc/>
  public override bool Equals(
    object? obj )
  {
    return obj is Interval other && Equals( other );
  }

  /// <summary>
  ///   Flips the direction of the interval making an ascending interval descending and vice versa.
  /// </summary>
  /// <returns>The interval with its direction flipped.</returns>
  public Interval FlipDirection()
  {
    // To flip the direction of the interval, we need to negate the semitone count while keeping the other properties intact.
    var qqd = (ushort) ( _value & 0x03FF ); // keep low 10 bits (Quantity, Quality, Displacement)
    var packed = (ushort) ( ( ( -SemitoneCount & MASK_SEMITONES ) << SHIFT_SEMITONES ) | qqd );
    return new Interval( packed );
  }

  /// <inheritdoc/>
  public override int GetHashCode()
  {
    return _value;
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
  /// <param name="descending">True if it is a descending interval; otherwise false.</param>
  /// <returns>The semitone count.</returns>
  public static int GetSemitoneCount(
    IntervalQuantity quantity,
    IntervalQuality quality,
    bool descending = false )
  {
    ArgumentOutOfRangeException.ThrowIfLessThan( (int) quantity, (int) IntervalQuantity.Unison );
    ArgumentOutOfRangeException.ThrowIfGreaterThan( (int) quantity, (int) IntervalQuantity.Fourteenth );
    ArgumentOutOfRangeException.ThrowIfLessThan( (int) quality, (int) IntervalQuality.Diminished );
    ArgumentOutOfRangeException.ThrowIfGreaterThan( (int) quality, (int) IntervalQuality.Augmented );

    var displacement = GetDisplacement( quantity, quality );
    var semitones = s_quantitySemitones[(int) quantity - 1] + displacement;
    return descending ? -semitones : semitones;
  }

  /// <summary>
  ///   Converts the specified string representation of an interval to its <see cref="Interval"/> equivalent.
  /// </summary>
  /// <param name="value">A string containing the interval to convert.</param>
  /// <returns>An object that is equivalent to the interval contained in value.</returns>
  /// <exception cref="FormatException">value does not contain a valid string representation of an interval.</exception>
  public static Interval Parse(
    string value )
  {
    ArgumentNullException.ThrowIfNull( value );
    return Parse( value.AsSpan(), null );
  }

  /// <summary>
  ///   Converts the specified string representation of an interval to its <see cref="Interval"/> equivalent
  ///   using the specified format provider.
  /// </summary>
  /// <param name="value">A string containing the interval to convert.</param>
  /// <param name="provider">The format provider.</param>
  /// <returns>An object that is equivalent to the interval contained in value.</returns>
  /// <exception cref="FormatException">value does not contain a valid string representation of an interval.</exception>
  public static Interval Parse(
    string value,
    IFormatProvider? provider )
  {
    ArgumentNullException.ThrowIfNull( value );
    return Parse( value.AsSpan(), provider );
  }

  /// <summary>
  ///   Converts the specified span representation of an interval to its <see cref="Interval"/> equivalent
  ///   using the specified format provider.
  /// </summary>
  /// <param name="value">A read-only character span containing the interval to convert.</param>
  /// <param name="provider">The format provider.</param>
  /// <returns>An object that is equivalent to the interval contained in value.</returns>
  /// <exception cref="FormatException">value does not contain a valid string representation of an interval.</exception>
  public static Interval Parse(
    ReadOnlySpan<char> value,
    IFormatProvider? provider )
  {
    return TryParse( value, provider, out var interval )
      ? interval
      : throw new FormatException( $"{value} is not a valid interval" );
  }

  /// <summary>Returns the fully qualified type name of this instance.</summary>
  /// <returns>The fully qualified type name.</returns>
  public override string ToString()
  {
    return ToString( SYMBOL_QUANTITY_TO_STRING_FORMAT, null );
  }

  /// <summary>
  ///   Returns a string representation of the value of this <see cref="Interval"/> instance, according to the
  ///   provided format specifier.
  /// </summary>
  /// <param name="format">A custom format string.</param>
  /// <returns>
  ///   A string representation of the value of the current <see cref="Interval"/> object as specified by
  ///   <paramref name="format"/>.
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
  ///   Returns a string representation of the value of this <see cref="Interval"/> instance, according to the
  ///   provided format specifier and format provider.
  /// </summary>
  /// <param name="format">A custom format string.</param>
  /// <param name="provider">The format provider. (Currently unused)</param>
  /// <returns>
  ///   A string representation of the value of the current <see cref="Interval"/> object as specified by
  ///   <paramref name="format"/>.
  /// </returns>
  /// <remarks>
  ///   <para>"s": Symbol pattern. e.g. (m)minor, (d)diminished, (A)augmented. Excludes perfect and major.</para>
  ///   <para>"S": Symbol pattern. e.g. (P)perfect, (M)major, (m)minor, (d)diminished, (A)augmented.</para>
  ///   <para>"q": Numeric quantity pattern. e.g. 1, 2, 3, etc.</para>
  ///   <para>"Q": Ordinal quantity pattern. e.g. First, Second, Third.</para>
  /// </remarks>
  public string ToString(
    string? format,
    IFormatProvider? provider )
  {
    if( string.IsNullOrWhiteSpace( format ) )
    {
      format = SYMBOL_QUANTITY_TO_STRING_FORMAT;
    }

    var buf = new StringBuilder();

    foreach( var f in format )
    {
      switch( f )
      {
        case 'd':
          if( IsDescending )
          {
            buf.Append( '-' );
          }

          break;

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
          buf.Append( (int) Quantity );
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
  ///   Converts the specified string representation of an interval to its <see cref="Interval"/> equivalent
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
  ///   <see langword="true"/> if the value parameter was converted successfully; otherwise, <see langword="false"/>
  ///   .
  /// </returns>
  public static bool TryParse(
    string? value,
    out Interval interval )
  {
    return TryParse( value.AsSpan(), null, out interval );
  }

  /// <summary>
  ///   Converts the specified string representation of an interval to its <see cref="Interval"/> equivalent
  ///   and returns a value that indicates whether the conversion succeeded.
  /// </summary>
  /// <param name="value">A string containing the interval quality to convert.</param>
  /// <param name="provider">The format provider.</param>
  /// <param name="interval">
  ///   When this method returns, contains the Interval value equivalent to the interval contained in
  ///   value.
  /// </param>
  /// <returns>
  ///   <see langword="true"/> if the value parameter was converted successfully; otherwise, <see langword="false"/>
  ///   .
  /// </returns>
  public static bool TryParse(
    string? value,
    IFormatProvider? provider,
    out Interval interval )
  {
    return TryParse( value.AsSpan(), provider, out interval );
  }

  /// <summary>
  ///   Converts the specified character span representation of an interval to its <see cref="Interval"/> equivalent
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
  ///   <see langword="true"/> if the value parameter was converted successfully; otherwise, <see langword="false"/>.
  /// </returns>
  public static bool TryParse(
    ReadOnlySpan<char> value,
    out Interval interval )
  {
    return TryParse( value, null, out interval );
  }

  /// <summary>
  ///   Converts the specified character span representation of an interval to its <see cref="Interval"/> equivalent
  ///   and returns a value that indicates whether the conversion succeeded.
  /// </summary>
  /// <param name="value">A read-only character span containing the interval quality to convert.</param>
  /// <param name="provider">The format provider.</param>
  /// <param name="interval">
  ///   When this method returns, contains the Interval value equivalent to the interval contained in
  ///   value.
  /// </param>
  /// <returns>
  ///   <see langword="true"/> if the value parameter was converted successfully; otherwise, <see langword="false"/>
  ///   .
  /// </returns>
  public static bool TryParse(
    ReadOnlySpan<char> value,
    IFormatProvider? provider,
    out Interval interval )
  {
    interval = Unison;
    value = value.TrimStart();

    if( value.IsEmpty )
    {
      return false;
    }

    var isDescending = false;

    if( value[0] == '-' )
    {
      isDescending = true;
      value = value[1..];
    }
    else if( value[0] == '+' )
    {
      value = value[1..];
    }

    if( value.IsEmpty )
    {
      return false;
    }

    var i = 0;
    var quality = IntervalQuality.Perfect;
    var hasExplicitQuality = char.IsLetter( value[i] );

    if( hasExplicitQuality )
    {
      if( value[i] == 'R' )
      {
        interval = !isDescending ? Unison : -Unison;
        return true;
      }

      if( !IntervalQuality.TryParse( value[i..], null, out quality, out _ ) )
      {
        return false;
      }

      ++i;
    }

    if( !IntervalQuantity.TryParse( value[i..], null, out var quantity, out _ ) )
    {
      return false;
    }

    if( !hasExplicitQuality )
    {
      // If the quality is not explicitly specified, we can infer it based on the interval number.
      quality = quantity.IsPerfectBased ? IntervalQuality.Perfect : IntervalQuality.Major;
    }

    if( !quality.IsValidFor( quantity ) )
    {
      return false;
    }

    interval = new Interval( quantity, quality, isDescending );
    return true;
  }

  #endregion

  #region Implementation

  /// <summary>
  ///   Packs the interval properties into a single ushort value.
  /// </summary>
  /// <param name="quantity">The quantity of the interval.</param>
  /// <param name="quality">The quality of the interval.</param>
  /// <param name="displacement">The displacement of the interval.</param>
  /// <param name="descending">Whether the interval is descending.</param>
  /// <returns>The packed ushort value representing the interval.</returns>
  /// <exception cref="ArgumentOutOfRangeException"></exception>
  private static ushort Pack(
    IntervalQuantity quantity,
    IntervalQuality quality,
    int displacement,
    bool descending )
  {
    ArgumentOutOfRangeException.ThrowIfLessThan( (int) quantity, (int) IntervalQuantity.Unison );
    ArgumentOutOfRangeException.ThrowIfGreaterThan( (int) quantity, (int) IntervalQuantity.Fourteenth );
    ArgumentOutOfRangeException.ThrowIfLessThan( (int) quality, (int) IntervalQuality.Diminished );
    ArgumentOutOfRangeException.ThrowIfGreaterThan( (int) quality, (int) IntervalQuality.Augmented );
    ArgumentOutOfRangeException.ThrowIfLessThan( displacement, -3 );
    ArgumentOutOfRangeException.ThrowIfGreaterThan( displacement, 3 );

    var q = (int) quantity;
    var semitones = s_quantitySemitones[q - 1] + displacement;

    if( descending )
    {
      semitones = -semitones;
    }

    if( semitones < -26 || semitones > 26 )
    {
      throw new ArgumentOutOfRangeException( nameof( semitones ) );
    }

    var encodedDisplacement = displacement + 3; // 0..6

    var packed =
      (ushort) (
        ( ( semitones & MASK_SEMITONES ) << SHIFT_SEMITONES )
        | ( q << SHIFT_QUANTITY )
        | ( (int) quality << SHIFT_QUALITY )
        | ( encodedDisplacement << SHIFT_DISPLACEMENT )
      );
    return packed;
  }

  private static int GetDisplacement(
    IntervalQuantity quantity,
    IntervalQuality quality )
  {
    if( quantity.IsPerfectBased )
    {
      return quality switch
      {
        IntervalQuality.Perfect => 0,
        IntervalQuality.Diminished => -1,
        IntervalQuality.Augmented => 1,
        _ => throw new ArgumentException( $"{quality} is not valid for a perfect-based interval ({quantity})" )
      };
    }

    return quality switch
    {
      IntervalQuality.Major => 0,
      IntervalQuality.Diminished => -2,
      IntervalQuality.Minor => -1,
      IntervalQuality.Augmented => 1,
      _ => throw new ArgumentException( $"{quality} is not valid for a major-based interval ({quantity})" )
    };
  }

  internal static IntervalQuality GetIntervalQuality(
    IntervalQuantity quantity,
    int semitoneCount )
  {
    var baseLine = s_quantitySemitones[(int) quantity - 1];
    var offset = semitoneCount - baseLine;

    if( quantity.IsPerfectBased )
    {
      return offset switch
      {
        0   => IntervalQuality.Perfect,
        > 0 => IntervalQuality.Augmented,
        < 0 => IntervalQuality.Diminished
      };
    }

    return offset switch
    {
      0   => IntervalQuality.Major,
      -1  => IntervalQuality.Minor,
      > 0 => IntervalQuality.Augmented,
      < 0 => IntervalQuality.Diminished
    };
  }

  #endregion

  #region Operators

  /// <summary>Explicit cast that converts the given Interval to an int.</summary>
  /// <param name="interval">The interval.</param>
  /// <returns>The result of the operation.</returns>
  public static explicit operator int(
    Interval interval )
  {
    return interval._value;
  }

  /// <summary>Explicit cast that converts the given int to an Interval.</summary>
  /// <param name="value">The value.</param>
  /// <returns>The result of the operation.</returns>
  public static explicit operator Interval(
    int value )
  {
    // Must fit in 16 bits
    if( value < ushort.MinValue || value > ushort.MaxValue )
    {
      throw new ArgumentOutOfRangeException( nameof( value ), "Packed interval must be a 16-bit value." );
    }

    var packed = (ushort) value;

    // Extract fields
    var semitones = ( packed >> SHIFT_SEMITONES ) & MASK_SEMITONES;

    if( semitones >= 32 )
    {
      semitones -= 64; // sign-extend 6-bit
    }

    var quantity = ( packed >> SHIFT_QUANTITY ) & MASK_QUANTITY;
    var quality = ( packed >> SHIFT_QUALITY ) & MASK_QUALITY;
    var displacement = ( ( packed >> SHIFT_DISPLACEMENT ) & MASK_DISPLACEMENT ) - 3;

    // Validate semitone range
    if( semitones < -26 || semitones > 26 )
    {
      throw new ArgumentException( "Invalid semitone count", nameof( value ) );
    }

    // Validate quantity
    if( (uint) quantity > 13 )
    {
      throw new ArgumentException( "Invalid interval quantity", nameof( value ) );
    }

    // Validate quality
    if( (uint) quality > 4 )
    {
      throw new ArgumentException( "Invalid interval quality", nameof( value ) );
    }

    // Validate displacement
    if( displacement < -3 || displacement > 3 )
    {
      throw new ArgumentException( "Invalid displacement", nameof( value ) );
    }

    // Validate semitone consistency with identity
    var baseline = s_quantitySemitones[quantity];
    var expected = baseline + displacement;

    var isDescending = semitones < 0;
    var actual = isDescending ? -semitones : semitones;

    if( actual != expected )
    {
      throw new ArgumentException(
        "Semitone count does not match the interval's quantity/quality/displacement",
        nameof( value )
      );
    }

    return new Interval( packed );
  }

  /// <summary>
  ///   Negates the interval, effectively reversing its direction (ascending to descending or vice versa).
  /// </summary>
  /// <param name="interval">The interval to negate.</param>
  /// <returns>The negated interval.</returns>
  public static Interval operator -(
    Interval interval )
  {
    return interval.FlipDirection();
  }

  /// <summary>Equality operator.</summary>
  /// <param name="lhs">The first instance to compare.</param>
  /// <param name="rhs">The second instance to compare.</param>
  /// <returns>The result of the operation.</returns>
  public static bool operator ==(
    Interval lhs,
    Interval rhs )
  {
    return lhs.Equals( rhs );
  }

  /// <summary>Inequality operator.</summary>
  /// <param name="lhs">The first instance to compare.</param>
  /// <param name="rhs">The second instance to compare.</param>
  /// <returns>The result of the operation.</returns>
  public static bool operator !=(
    Interval lhs,
    Interval rhs )
  {
    return !lhs.Equals( rhs );
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
