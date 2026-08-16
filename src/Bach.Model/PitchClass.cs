// Module Name: PitchClass.cs
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

namespace Bach.Model;

using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Text;
using Bach.Model.Internal;

/// <summary>
///   A PitchClass represents a combination of a <see cref="P:Bach.Model.NoteName" />
///   and an optional <see cref="P:Bach.Model.Accidental" /> following
///   the <see href="https://en.wikipedia.org/wiki/Scientific_pitch_notation">Scientific Pitch Notation</see>.
/// </summary>
public readonly struct PitchClass
  : IPitch<PitchClass>
{
  #region Constants

  private const int ENHARMONIC_COUNT = 5; // DoubleFlat, Flat, Natural, Sharp, DoubleSharp
  private const int SEMITONE_COUNT = 12;
  private const string NOTE_NAME_SYMBOL_TO_STRING_FORMAT = "NS";

  private static readonly PitchClass[] s_pitchClasses =
  [
    new( 0, 0, NoteName.D, Accidental.DoubleFlat ),
    new( 1, 0, NoteName.C, Accidental.Natural ),
    new( 2, 0, NoteName.B, Accidental.Sharp ),
    new( 3, 1, NoteName.D, Accidental.Flat ),
    new( 4, 1, NoteName.C, Accidental.Sharp ),
    new( 5, 1, NoteName.B, Accidental.DoubleSharp ),
    new( 6, 2, NoteName.E, Accidental.DoubleFlat ),
    new( 7, 2, NoteName.D, Accidental.Natural ),
    new( 8, 2, NoteName.C, Accidental.DoubleSharp ),
    new( 9, 3, NoteName.F, Accidental.DoubleFlat ),
    new( 10, 3, NoteName.E, Accidental.Flat ),
    new( 11, 3, NoteName.D, Accidental.Sharp ),
    new( 12, 4, NoteName.F, Accidental.Flat ),
    new( 13, 4, NoteName.E, Accidental.Natural ),
    new( 14, 4, NoteName.D, Accidental.DoubleSharp ),
    new( 15, 5, NoteName.G, Accidental.DoubleFlat ),
    new( 16, 5, NoteName.F, Accidental.Natural ),
    new( 17, 5, NoteName.E, Accidental.Sharp ),
    new( 18, 6, NoteName.G, Accidental.Flat ),
    new( 19, 6, NoteName.F, Accidental.Sharp ),
    new( 20, 6, NoteName.E, Accidental.DoubleSharp ),
    new( 21, 7, NoteName.A, Accidental.DoubleFlat ),
    new( 22, 7, NoteName.G, Accidental.Natural ),
    new( 23, 7, NoteName.F, Accidental.DoubleSharp ),
    new( 24, 8, NoteName.A, Accidental.Flat ),
    new( 25, 8, NoteName.G, Accidental.Sharp ),
    new( 26, 9, NoteName.B, Accidental.DoubleFlat ),
    new( 27, 9, NoteName.A, Accidental.Natural ),
    new( 28, 9, NoteName.G, Accidental.DoubleSharp ),
    new( 29, 10, NoteName.C, Accidental.DoubleFlat ),
    new( 30, 10, NoteName.B, Accidental.Flat ),
    new( 31, 10, NoteName.A, Accidental.Sharp ),
    new( 32, 11, NoteName.C, Accidental.Flat ),
    new( 33, 11, NoteName.B, Accidental.Natural ),
    new( 34, 11, NoteName.A, Accidental.DoubleSharp )
  ];

  private static readonly int[] s_noteNameIndices =
  [
    1, // NoteName.C
    7, // NoteName.D
    13, // NoteName.E
    16, // NoteName.F
    22, // NoteName.G
    27, // NoteName.A
    33 // NoteName.B
  ];

  // DoubleFlat, Flat, Natural, Sharp, DoubleSharp
  private static readonly int[,] s_enharmonics =
  {
    { 0, -1, 1, 2, -1 }, // Dbb, C, B#
    { -1, 3, -1, 4, 5 }, // Db, C#, B##
    { 6, -1, 7, -1, 8 }, // Ebb, D, C##
    { 9, 10, -1, 11, -1 }, // Fbb, Eb, D#
    { -1, 12, 13, -1, 14 }, // Fb, E, D##
    { 15, -1, 16, 17, -1 }, // Gbb, F, E#
    { -1, 18, -1, 19, 20 }, // Gb, F#, E##
    { 21, -1, 22, -1, 23 }, // Abb, G, F##
    { -1, 24, -1, 25, -1 }, // Ab, G#
    { 26, -1, 27, -1, 28 }, // Bbb, A, G##
    { 29, 30, -1, 31, -1 }, // Cbb, Bb, A#
    { -1, 32, 33, -1, 34 } // Cb, B, A##
  };

  #endregion

  #region Fields

  private readonly sbyte _accidental;
  private readonly byte _enharmonicIndex;
  private readonly byte _noteIndex;
  private readonly byte _noteName;

  #endregion

  #region Constructors

  private PitchClass(
    int pitchClassIndex,
    int enharmonicIndex,
    NoteName noteName,
    Accidental accidental )
  {
    Debug.Assert( pitchClassIndex is >= 0 and <= 34 );
    Debug.Assert( enharmonicIndex is >= 0 and <= 11 );

    _noteIndex = (byte) pitchClassIndex;
    _enharmonicIndex = (byte) enharmonicIndex;
    _noteName = (byte) noteName;
    _accidental = (sbyte) accidental;
  }

  #endregion

  #region Properties

  /// <summary>C pitch class.</summary>
  public static PitchClass C => s_pitchClasses[1];

  /// <summary>C♯ pitch class.</summary>
  public static PitchClass CSharp => s_pitchClasses[4];

  /// <summary>D♭ pitch class.</summary>
  public static PitchClass DFlat => s_pitchClasses[3];

  /// <summary>D pitch class.</summary>
  public static PitchClass D => s_pitchClasses[7];

  /// <summary>D♯ pitch class.</summary>
  public static PitchClass DSharp => s_pitchClasses[11];

  /// <summary>E♭ pitch class.</summary>
  public static PitchClass EFlat => s_pitchClasses[10];

  /// <summary>E pitch class.</summary>
  public static PitchClass E => s_pitchClasses[13];

  /// <summary>F pitch class.</summary>
  public static PitchClass F => s_pitchClasses[16];

  /// <summary>F♯ pitch class.</summary>
  public static PitchClass FSharp => s_pitchClasses[19];

  /// <summary>G♭ pitch class.</summary>
  public static PitchClass GFlat => s_pitchClasses[18];

  /// <summary>G pitch class.</summary>
  public static PitchClass G => s_pitchClasses[22];

  /// <summary>G♯ pitch class.</summary>
  public static PitchClass GSharp => s_pitchClasses[25];

  /// <summary>A♭ pitch class.</summary>
  public static PitchClass AFlat => s_pitchClasses[24];

  /// <summary>A pitch class.</summary>
  public static PitchClass A => s_pitchClasses[27];

  /// <summary>A♯ pitch class.</summary>
  public static PitchClass ASharp => s_pitchClasses[31];

  /// <summary>B♭ pitch class.</summary>
  public static PitchClass BFlat => s_pitchClasses[30];

  /// <summary>B pitch class.</summary>
  public static PitchClass B => s_pitchClasses[33];

  /// <summary>
  /// Gets the pitch class of the pitch-like value.
  /// </summary>
  PitchClass IPitch<PitchClass>.PitchClass => this;

  /// <summary>Gets the name of the pitch class.</summary>
  /// <value>The name of the pitch class.</value>
  public NoteName NoteName => (NoteName) _noteName;

  /// <summary>Gets the accidental.</summary>
  /// <value>The accidental.</value>
  public Accidental Accidental => (Accidental) _accidental;

  #endregion

  #region Public Methods

  /// <summary>Transposes the current instance by a number of semitones.</summary>
  /// <param name="semitoneCount">Number of semitones. Negative values transpose downward.</param>
  /// <returns>A PitchClass.</returns>
  public PitchClass Transpose(
    int semitoneCount )
  {
    var enharmonicIndex = s_enharmonics.WrapIndex( 0, _enharmonicIndex + semitoneCount );
    return LookupPitchClass( enharmonicIndex );
  }

  /// <summary>Adds an interval to the current instance.</summary>
  /// <param name="interval">An interval to add.</param>
  /// <returns>A PitchClass.</returns>
  public PitchClass Transpose(
    Interval interval )
  {
    // First we calculate the new note name from the interval quantity, wrapping around the 7 note names
    var noteIndex = (int) NoteName + ( (int) interval.Quantity * ( interval.IsAscending ? 1 : -1 ) );
    var expectedNoteName = (NoteName) noteIndex.Wrap( NoteName.TotalCount );

    // Next we calculate the new enharmonic index, wrapping around the 12 semitones
    var semitoneCount = ( _enharmonicIndex + interval.SemitoneCount ).Wrap( SEMITONE_COUNT );

    // Now we look for a pitch class that matches the calculated note name and the enharmonic index
    for( var i = 0; i < ENHARMONIC_COUNT; i++ )
    {
      var enharmonicIndex = s_enharmonics[semitoneCount, i];

      // If the enharmonic index is -1, it means that there is no pitch class for this combination of enharmonic index and accidental
      if( enharmonicIndex == -1 )
      {
        continue;
      }

      var pitchClass = s_pitchClasses[enharmonicIndex];

      // If the pitch class has the same note name as the calculated note name, we return it
      if( pitchClass.NoteName == expectedNoteName )
      {
        return pitchClass;
      }
    }

    throw new InvalidOperationException( "No pitch class found for the calculated note name and enharmonic index." );
  }

  /// <summary>Determines the interval between this instance and the provided pitch class.</summary>
  /// <param name="pitchClass">The pitch class.</param>
  /// <returns>An interval.</returns>
  public Interval GetIntervalTo(
    PitchClass pitchClass )
  {
    // First we determine the interval quantity
    var quantity = (IntervalQuantity) (pitchClass.NoteName - NoteName).Wrap( NoteName.TotalCount );

    // Then we determine the semitone count
    var semitoneCount = (pitchClass._enharmonicIndex - _enharmonicIndex).Wrap( SEMITONE_COUNT );
    var interval = new Interval( quantity, semitoneCount );
    return interval;
  }

  /// <inheritdoc />
  public int CompareTo(
    PitchClass other )
  {
    return _enharmonicIndex - other._enharmonicIndex;
  }

  /// <summary>Creates a new PitchClass.</summary>
  /// <exception cref="ArgumentOutOfRangeException">
  ///   Thrown when one or more arguments are outside the
  ///   required range.
  /// </exception>
  /// <param name="noteName">The name of the pitch class.</param>
  /// <returns>A PitchClass.</returns>
  public static PitchClass Create(
    NoteName noteName )
  {
    return Create( noteName, Accidental.Natural );
  }

  /// <summary>Creates a new PitchClass.</summary>
  /// <exception cref="ArgumentOutOfRangeException">
  ///   Thrown when one or more arguments are outside the
  ///   required range.
  /// </exception>
  /// <param name="noteName">The name of the pitch class.</param>
  /// <param name="accidental">(Optional) The accidental.</param>
  /// <returns>A PitchClass.</returns>
  public static PitchClass Create(
    NoteName noteName,
    Accidental accidental )
  {
    // This really doesn't create a PitchClass but returns one of the pre-created ones
    // from the enharmonics table.

    // First we determine the row in the enharmonics table that corresponds to the
    // pitch class name.
    var noteIndex = s_noteNameIndices[(int) noteName];
    var pitchClass = s_pitchClasses[noteIndex];

    if( accidental == Accidental.Natural )
    {
      return pitchClass;
    }

    int enharmonicIndex = pitchClass._enharmonicIndex;

    // Next we ensure that the enharmonic index wraps around when added to the accidental (-2 .. 2)
    enharmonicIndex = s_enharmonics.WrapIndex( 0, enharmonicIndex + (int) accidental );

    // Next we determine the index of the pitch class in the pitch class table (Offset by DoubleFlat, so 0..3)
    var accidentalIndex = (int) accidental + Math.Abs( (int) Accidental.DoubleFlat );
    noteIndex = s_enharmonics[enharmonicIndex, accidentalIndex];
    Debug.Assert( noteIndex != -1 );

    pitchClass = s_pitchClasses[noteIndex];
    return pitchClass;
  }

  /// <inheritdoc />
  public bool Equals(
    PitchClass other )
  {
    return _enharmonicIndex == other._enharmonicIndex;
  }

  /// <inheritdoc />
  public override bool Equals(
    object? obj )
  {
    return obj is PitchClass other && Equals( other );
  }

  /// <summary>Gets the enharmonic pitch class for this instance or null if none exists.</summary>
  /// <param name="noteName">The name of the enharmonic pitch class.</param>
  /// <returns>The enharmonic.</returns>
  [Pure]
  public PitchClass? GetEnharmonic(
    NoteName noteName )
  {
    var accidentalOffset = Math.Abs( (int) Accidental.DoubleFlat );
    int enharmonicIndex = _enharmonicIndex;

    for( var accidental = (int) Accidental.DoubleFlat; accidental <= (int) Accidental.DoubleSharp; ++accidental )
    {
      var accidentalIndex = accidental + accidentalOffset;
      var noteIndex = s_enharmonics[enharmonicIndex, accidentalIndex];

      if( noteIndex == -1 )
      {
        continue;
      }

      var pitchClass = s_pitchClasses[noteIndex];

      if( pitchClass.NoteName == noteName )
      {
        return pitchClass;
      }
    }

    return null;
  }

  /// <inheritdoc />
  public override int GetHashCode()
  {
    return _enharmonicIndex;
  }

  /// <summary>Parses the provided string.</summary>
  /// <exception cref="FormatException">Thrown when the provided string doesn't represent a a PitchClass.</exception>
  /// <exception cref="ArgumentNullException">Thrown when a null string is provided.</exception>
  /// <exception cref="ArgumentException">Thrown when an empty string is provided.</exception>
  /// <param name="value">The value to parse.</param>
  /// <returns>A PitchClass.</returns>
  public static PitchClass Parse(
    string value )
  {
    ArgumentNullException.ThrowIfNull( value );
    return Parse( value.AsSpan(), null );
  }

  /// <summary>
  ///   Parses the provided string using the given format provider.
  /// </summary>
  /// <param name="value">The value to parse.</param>
  /// <param name="provider">The format provider.</param>
  /// <returns>A PitchClass.</returns>
  /// <exception cref="FormatException">Thrown when the provided string doesn't represent a a PitchClass.</exception>
  /// <exception cref="ArgumentNullException">Thrown when a null string is provided.</exception>
  /// <exception cref="ArgumentException">Thrown when an empty string is provided.</exception>
  public static PitchClass Parse(
    string value,
    IFormatProvider? provider )
  {
    ArgumentNullException.ThrowIfNull( value );
    return Parse( value.AsSpan(), provider );
  }

  /// <summary>
  ///   Parses the provided string using the given format provider.
  /// </summary>
  /// <param name="value">The value to parse.</param>
  /// <param name="provider">The format provider.</param>
  /// <returns>A PitchClass.</returns>
  /// <exception cref="ArgumentException">Thrown when an empty string is provided.</exception>
  /// <exception cref="FormatException">Thrown when the provided string doesn't represent a PitchClass.</exception>
  public static PitchClass Parse(
    ReadOnlySpan<char> value,
    IFormatProvider? provider )
  {
    if( value.IsEmpty )
    {
      throw new ArgumentException( "Value cannot be empty.", nameof( value ) );
    }

    return TryParse( value, provider, out var result )
      ? result
      : throw new FormatException( $"{value} is not a valid pitch class" );
  }

  /// <inheritdoc />
  public override string ToString()
  {
    return $"{NoteName}{Accidental.ToSymbol()}";
  }

  /// <summary>
  ///   Returns a string representation of the value of this <see cref="Formula" /> instance, according to the
  ///   provided format specifier.
  /// </summary>
  /// <param name="format">A custom format string.</param>
  /// <returns>
  ///   A string representation of the value of the current <see cref="Formula" /> object as specified by
  ///   <paramref name="format" />.
  /// </returns>
  /// <remarks>
  ///   <para>Format specifiers:</para>
  ///   <para>"N": Name pattern. e.g. "Major".</para>
  ///   <para>"I": Intervals pattern. e.g. "P1,M3,P5".</para>
  /// </remarks>
  public string ToString(
    string format )
  {
    return ToString( format, null );
  }

  /// <summary>
  ///   Returns a string representation of the value of this <see cref="Formula" /> instance, according to the
  ///   provided format specifier and format provider.
  /// </summary>
  /// <param name="format">A custom format string.</param>
  /// <param name="provider">The format provider. (Currently unused)</param>
  /// <returns>
  ///   A string representation of the value of the current <see cref="Formula" /> object as specified by
  ///   <paramref name="format" />.
  /// </returns>
  /// <remarks>
  ///   <para>Format specifiers:</para>
  ///   <para>"N": NoteName pattern. e.g. "C".</para>
  ///   <para>"S": Symbol pattern. e.g. "#".</para>
  ///   <para>"X": Extended symbol pattern. e.g. "♯".</para>
  /// </remarks>
  public string ToString(
    string? format,
    IFormatProvider? provider )
  {
    if( string.IsNullOrEmpty( format ) )
    {
      format = NOTE_NAME_SYMBOL_TO_STRING_FORMAT;
    }

    var buf = new StringBuilder();

    foreach( var f in format )
    {
      switch( f )
      {
        case 'N':
          buf.Append( NoteName );
          break;

        case 'S':
          buf.Append( Accidental.ToSymbol() );
          break;

        case 'X':
          buf.Append( Accidental.ToExtendedSymbol() );
          break;

        default:
          buf.Append( f );
          break;
      }
    }

    return buf.ToString();
  }

  /// <summary>Attempts to parse a PitchClass from the given string.</summary>
  /// <param name="value">The value to parse.</param>
  /// <param name="pitchClass">[out] The pitch class.</param>
  /// <returns>True if it succeeds, false if it fails.</returns>
  public static bool TryParse(
    string? value,
    out PitchClass pitchClass )
  {
    return TryParse( value.AsSpan(), null, out pitchClass );
  }

  /// <summary>
  ///   Attempts to parse a PitchClass from the given string.
  /// </summary>
  /// <param name="value">The value to parse.</param>
  /// <param name="provider">The format provider.</param>
  /// <param name="pitchClass">[out] The pitch class.</param>
  /// <returns>True if it succeeds, false if it fails.</returns>
  public static bool TryParse(
    string? value,
    IFormatProvider? provider,
    out PitchClass pitchClass )
  {
    return TryParse( value.AsSpan(), provider, out pitchClass );
  }

  /// <summary>
  ///   Attempts to parse a PitchClass from the given string.
  /// </summary>
  /// <param name="value">The string representation of the pitch class.</param>
  /// <param name="pitchClass">[out] The parsed pitch class.</param>
  /// <returns>True if parsing was successful; otherwise, false.</returns>
  public static bool TryParse(
    ReadOnlySpan<char> value,
    out PitchClass pitchClass )
  {
    return TryParse( value, null, out pitchClass );
  }

  /// <summary>
  ///   Attempts to parse a PitchClass from the given string.
  /// </summary>
  /// <param name="value">The string representation of the pitch class.</param>
  /// <param name="provider">The format provider.</param>
  /// <param name="pitchClass">[out] The parsed pitch class.</param>
  /// <returns>True if parsing was successful; otherwise, false.</returns>
  public static bool TryParse(
    ReadOnlySpan<char> value,
    IFormatProvider? provider,
    out PitchClass pitchClass )
  {
    // We want to ensure that the entire string is consumed during parsing,
    // so we call the overload that provides the tail of the string after parsing.
    return TryParse( value, provider, out pitchClass, out var tail ) && tail.IsEmpty;
  }

  /// <inheritdoc />
  public static bool TryParse(
    ReadOnlySpan<char> value,
    IFormatProvider? provider,
    out PitchClass pitchClass,
    out ReadOnlySpan<char> tail )
  {
    value = value.TrimStart();

    if( value.IsEmpty )
    {
      pitchClass = C;
      tail = ReadOnlySpan<char>.Empty;
      return false;
    }

    // Must have at least one character for the note name
    if( !NoteName.TryParse( value, provider, out var noteName, out tail ) )
    {
      pitchClass = C;
      return false;
    }

    var accidental = Accidental.Natural;
    if( value.Length > 1 )
    {
      // Could be an accidental or some other character; use any partial match and
      // leave the tail to be processed by the caller
      Accidental.TryParse( tail, provider, out accidental, out tail );
    }

    pitchClass = Create( noteName, accidental );
    return true;
  }

  #endregion

  #region Implementation

  // Finds a pitch class that corresponds to the provided enharmonic index,
  // attempting to match the desired accidental mode
  internal static PitchClass LookupPitchClass(
    int enharmonicIndex )
  {
    // Preferred order: Natural, then Sharp, then Flat, then DoubleSharp, then DoubleFlat
    int[] preferred = [2, 3, 1, 4, 0];

    foreach( var accidentalIndex in preferred )
    {
      var noteIndex = s_enharmonics[enharmonicIndex, accidentalIndex];

      if( noteIndex != -1 )
      {
        return s_pitchClasses[noteIndex];
      }
    }

    Trace.Assert( false, "Internal error! Must always find a pitch class" );
    return C;
  }

  #endregion

  #region Operators

  /// <summary>Explicit cast that converts the given pitch class to an int.</summary>
  /// <param name="pitchClass">The pitch class.</param>
  /// <returns>The result of the operation.</returns>
  public static explicit operator int(
    PitchClass pitchClass )
  {
    return pitchClass._noteIndex;
  }

  /// <summary>Explicit cast that converts the given int to a PitchClass.</summary>
  /// <param name="value">The value.</param>
  /// <returns>The result of the operation.</returns>
  public static explicit operator PitchClass(
    int value )
  {
    return s_pitchClasses[value];
  }

  /// <summary>Equality operator.</summary>
  /// <param name="left">The first instance to compare.</param>
  /// <param name="right">The second instance to compare.</param>
  /// <returns>The result of the operation.</returns>
  public static bool operator ==(
    PitchClass left,
    PitchClass right )
  {
    return Equals( left, right );
  }

  /// <summary>Inequality operator.</summary>
  /// <param name="left">The first instance to compare.</param>
  /// <param name="right">The second instance to compare.</param>
  /// <returns>The result of the operation.</returns>
  public static bool operator !=(
    PitchClass left,
    PitchClass right )
  {
    return !Equals( left, right );
  }

  /// <summary>Greater-than comparison operator.</summary>
  /// <param name="left">The first instance to compare.</param>
  /// <param name="right">The second instance to compare.</param>
  /// <returns>The result of the operation.</returns>
  public static bool operator >(
    PitchClass left,
    PitchClass right )
  {
    return left.CompareTo( right ) > 0;
  }

  /// <summary>Less-than comparison operator.</summary>
  /// <param name="left">The first instance to compare.</param>
  /// <param name="right">The second instance to compare.</param>
  /// <returns>The result of the operation.</returns>
  public static bool operator <(
    PitchClass left,
    PitchClass right )
  {
    return left.CompareTo( right ) < 0;
  }

  /// <summary>Greater-than-or-equal comparison operator.</summary>
  /// <param name="left">The first instance to compare.</param>
  /// <param name="right">The second instance to compare.</param>
  /// <returns>The result of the operation.</returns>
  public static bool operator >=(
    PitchClass left,
    PitchClass right )
  {
    return left.CompareTo( right ) >= 0;
  }

  /// <summary>Less-than-or-equal comparison operator.</summary>
  /// <param name="left">The first instance to compare.</param>
  /// <param name="right">The second instance to compare.</param>
  /// <returns>The result of the operation.</returns>
  public static bool operator <=(
    PitchClass left,
    PitchClass right )
  {
    return left.CompareTo( right ) <= 0;
  }

  /// <summary>Addition operator.</summary>
  /// <param name="pitchClass">The first value.</param>
  /// <param name="semitoneCount">A number of semitones to add to it.</param>
  /// <returns>The result of the operation.</returns>
  public static PitchClass operator +(
    PitchClass pitchClass,
    int semitoneCount )
  {
    return pitchClass.Transpose( semitoneCount );
  }

  /// <summary>Increment operator.</summary>
  /// <param name="pitchClass">The pitch class.</param>
  /// <returns>The result of the operation.</returns>
  public static PitchClass operator ++(
    PitchClass pitchClass )
  {
    return pitchClass.Transpose( 1 );
  }

  /// <summary>Subtraction operator.</summary>
  /// <param name="pitchClass">The first value.</param>
  /// <param name="semitoneCount">A number of semitones to subtract from it.</param>
  /// <returns>The result of the operation.</returns>
  public static PitchClass operator -(
    PitchClass pitchClass,
    int semitoneCount )
  {
    return pitchClass.Transpose( -semitoneCount );
  }

  /// <summary>Decrement operator.</summary>
  /// <param name="pitchClass">The pitch class.</param>
  /// <returns>The result of the operation.</returns>
  public static PitchClass operator --(
    PitchClass pitchClass )
  {
    return pitchClass.Transpose( -1 );
  }

  /// <summary>Addition operator.</summary>
  /// <param name="pitchClass">The pitch class.</param>
  /// <param name="interval">An interval to add to the pitch class.</param>
  /// <returns>A pitchClass.</returns>
  public static PitchClass operator +(
    PitchClass pitchClass,
    Interval interval )
  {
    return pitchClass.Transpose( interval );
  }

  /// <summary>Addition operator.</summary>
  /// <param name="pitchClass">The pitch class.</param>
  /// <param name="interval">An interval to add to the pitch class.</param>
  /// <returns>A pitchClass.</returns>
  public static PitchClass operator -(
    PitchClass pitchClass,
    Interval interval )
  {
    return pitchClass.Transpose( -interval );
  }

  #endregion
}
