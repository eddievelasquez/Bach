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

using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Bach.Model.Internal;

namespace Bach.Model;

/// <summary>
///   A PitchClass represents a combination of a <see cref="P:Bach.Model.NoteName"/>
///   and an optional <see cref="P:Bach.Model.Accidental"/> following
///   the <see href="https://en.wikipedia.org/wiki/Scientific_pitch_notation">Scientific Pitch Notation</see>.
/// </summary>
/// <remarks>
///   <see cref="PitchClass"/>'s equality and ordering members (<see cref="Equals(PitchClass)"/>,
///   <see cref="GetHashCode"/>, <see cref="CompareTo"/>, and the relational operators) are all
///   spelling-sensitive: two pitch classes are only equal (and only compare as equal) when they
///   share the same note name and accidental, so C♯ and D♭ are <b>not</b> equal. Use
///   <see cref="EnharmonicEquals"/> to test whether two pitch classes represent the same
///   sounding pitch class regardless of spelling, <see cref="EnharmonicCompareTo"/> to order by
///   chromatic pitch height instead of spelling, and <see cref="EnharmonicComparer"/> when an
///   <see cref="IEqualityComparer{T}"/> with enharmonic behavior is required (e.g., for a
///   <see cref="HashSet{T}"/> used in pitch-class-set operations).
/// </remarks>
public readonly struct PitchClass
  : IPitch<PitchClass>,
    IComparisonOperators<PitchClass, PitchClass, bool>
{
  #region Nested Types

  // Compares PitchClass values by enharmonic equivalence (sounding pitch class) rather than by
  // spelling. Backs the public EnharmonicComparer property.
  private sealed class EnharmonicEqualityComparer: IEqualityComparer<PitchClass>
  {
    #region Public Methods

    public bool Equals(
      PitchClass x,
      PitchClass y )
    {
      return x.EnharmonicEquals( y );
    }

    public int GetHashCode(
      PitchClass obj )
    {
      return obj.EnharmonicIndex;
    }

    #endregion
  }

  #endregion

  #region Constants

  private const string NOTE_NAME_SYMBOL_TO_STRING_FORMAT = "NS";
  private const int ACCIDENTAL_COUNT = 5;
  private const int ACCIDENTAL_OFFSET = 2;
  private const int MIN_INTEGER_VALUE = -ACCIDENTAL_OFFSET;
  private const int MAX_INTEGER_VALUE = ( Constants.NoteNameCount - 1 ) * ACCIDENTAL_COUNT + ACCIDENTAL_OFFSET;

  private static readonly int[] s_naturalSemitones =
  [
    0, 2, 4,
    5, 7, 9,
    11
  ];

  private static readonly byte[] s_preferredSpellings =
  [
    2, 3, 7,
    8, 12, 17,
    18, 22, 23,
    27, 28, 32
  ];

  #endregion

  #region Fields

  private readonly byte _noteName;
  private readonly sbyte _accidental;

  #endregion

  #region Constructors

  /// <summary>
  ///   Creates a pitch class from a note name and optional accidental.
  /// </summary>
  /// <param name="noteName">The name of the pitch class.</param>
  /// <param name="accidental">The accidental. The default value is Natural.</param>
  /// <exception cref="ArgumentOutOfRangeException">
  ///   Thrown when one or more arguments are outside the required range.
  /// </exception>
  public PitchClass(
    NoteName noteName,
    Accidental accidental = default )
  {
    ArgumentOutOfRangeException.ThrowIfLessThan( (int) noteName, 0, nameof( noteName ) );
    ArgumentOutOfRangeException.ThrowIfGreaterThan( (int) noteName, Constants.NoteNameCount - 1, nameof( noteName ) );
    ArgumentOutOfRangeException.ThrowIfLessThan( (int) accidental, -ACCIDENTAL_OFFSET, nameof( accidental ) );
    ArgumentOutOfRangeException.ThrowIfGreaterThan( (int) accidental, ACCIDENTAL_OFFSET, nameof( accidental ) );

    _noteName = (byte) noteName;
    _accidental = (sbyte) accidental;
  }

  #endregion

  #region Properties

  /// <summary>
  ///   Gets the enharmonic index of the pitch class, which is the number of semitones above C in the
  ///   octave.
  /// </summary>
  private int EnharmonicIndex =>
    SemitoneOffset.Wrap( Constants.OctaveSemitoneCount );

  /// <summary>
  ///   Gets the semitone offset of the pitch class, which is the number of semitones above C in the
  ///   octave.
  /// </summary>
  internal int SemitoneOffset => s_naturalSemitones[_noteName] + _accidental;

  /// <summary>C pitch class.</summary>
  public static PitchClass C => new( NoteName.C );

  /// <summary>C♯ pitch class.</summary>
  public static PitchClass CSharp => new( NoteName.C, Accidental.Sharp );

  /// <summary>D♭ pitch class.</summary>
  public static PitchClass DFlat => new( NoteName.D, Accidental.Flat );

  /// <summary>D pitch class.</summary>
  public static PitchClass D => new( NoteName.D );

  /// <summary>D♯ pitch class.</summary>
  public static PitchClass DSharp => new( NoteName.D, Accidental.Sharp );

  /// <summary>E♭ pitch class.</summary>
  public static PitchClass EFlat => new( NoteName.E, Accidental.Flat );

  /// <summary>E pitch class.</summary>
  public static PitchClass E => new( NoteName.E );

  /// <summary>F pitch class.</summary>
  public static PitchClass F => new( NoteName.F );

  /// <summary>F♯ pitch class.</summary>
  public static PitchClass FSharp => new( NoteName.F, Accidental.Sharp );

  /// <summary>G♭ pitch class.</summary>
  public static PitchClass GFlat => new( NoteName.G, Accidental.Flat );

  /// <summary>G pitch class.</summary>
  public static PitchClass G => new( NoteName.G );

  /// <summary>G♯ pitch class.</summary>
  public static PitchClass GSharp => new( NoteName.G, Accidental.Sharp );

  /// <summary>A♭ pitch class.</summary>
  public static PitchClass AFlat => new( NoteName.A, Accidental.Flat );

  /// <summary>A pitch class.</summary>
  public static PitchClass A => new( NoteName.A );

  /// <summary>A♯ pitch class.</summary>
  public static PitchClass ASharp => new( NoteName.A, Accidental.Sharp );

  /// <summary>B♭ pitch class.</summary>
  public static PitchClass BFlat => new( NoteName.B, Accidental.Flat );

  /// <summary>B pitch class.</summary>
  public static PitchClass B => new( NoteName.B );

  /// <summary>
  ///   Gets an <see cref="IEqualityComparer{T}"/> that compares <see cref="PitchClass"/> values by
  ///   enharmonic equivalence (same sounding pitch class) rather than by spelling.
  /// </summary>
  /// <remarks>
  ///   Use this comparer when enharmonic-insensitive uniqueness or lookup is required, e.g., a
  ///   <see cref="HashSet{T}"/> or <see cref="Dictionary{TKey,TValue}"/> used for pitch-class-set
  ///   operations where C♯ and D♭ should be treated as the same entry. The default
  ///   <see cref="Equals(PitchClass)"/> and <see cref="GetHashCode"/> members remain spelling-sensitive.
  /// </remarks>
  public static IEqualityComparer<PitchClass> EnharmonicComparer { get; } = new EnharmonicEqualityComparer();

  /// <summary>
  ///   Gets the pitch class of the pitch-like value.
  /// </summary>
  PitchClass IPitch<PitchClass>.PitchClass => this;

  /// <summary>Gets the name of the pitch class.</summary>
  /// <value>The name of the pitch class.</value>
  public NoteName NoteName => (NoteName) _noteName;

  /// <summary>Gets the accidental.</summary>
  /// <value>The accidental.</value>
  public Accidental Accidental => (Accidental) _accidental;

  /// <summary>
  ///   Gets the pitch of the pitch class at the specified octave.
  /// </summary>
  /// <param name="octave">The octave.</param>
  /// <returns>The pitch.</returns>
  public Pitch this[
    int octave ] => new( this, octave );

  #endregion

  #region Public Methods

  /// <inheritdoc/>
  /// <remarks>
  ///   This ordering is spelling-sensitive, consistent with <see cref="Equals(PitchClass)"/>: it
  ///   orders first by note name (C, D, E, F, G, A, B) and then by accidental (double flat to double
  ///   sharp), so <c>CompareTo(other) == 0</c> if and only if <c>Equals(other) == true</c>. C♯ and D♭
  ///   so they do <b>not</b> compare as equal. Use <see cref="EnharmonicCompareTo"/> for an ordering
  ///   based on chromatic pitch height (enharmonic position) instead.
  /// </remarks>
  public int CompareTo(
    PitchClass other )
  {
    var result = NoteName.CompareTo( other.NoteName );
    return result != 0 ? result : Accidental.CompareTo( other.Accidental );
  }

  /// <summary>
  ///   Compares the chromatic pitch height (enharmonic position) of this instance to another pitch
  ///   class, ignoring spelling.
  /// </summary>
  /// <param name="other">The pitch class to compare against.</param>
  /// <returns>
  ///   A negative value if this instance sounds lower than <paramref name="other"/>, zero if both
  ///   sound at the same pitch height, or a positive value if this instance sounds higher than
  ///   <paramref name="other"/>.
  /// </returns>
  /// <remarks>
  ///   This differs from <see cref="CompareTo"/>, which orders by spelling and is consistent with
  ///   <see cref="Equals(PitchClass)"/>. Unlike <see cref="CompareTo"/>, C♯ and D♭ compare as equal
  ///   (0) under this method even though <c>Equals(other) == false</c> for them. Use this method (or
  ///   <see cref="EnharmonicEquals"/> for equality only) when chromatic pitch height matters,
  ///   e.g., determining octave rollover when rendering a pitch sequence.
  /// </remarks>
  public int EnharmonicCompareTo(
    PitchClass other )
  {
    return EnharmonicIndex - other.EnharmonicIndex;
  }

  /// <inheritdoc/>
  /// <remarks>
  ///   This comparison is spelling-sensitive: two pitch classes are only equal when they share the
  ///   same note name and accidental, so C♯ and D♭ are <b>not</b> equal. Use
  ///   <see cref="EnharmonicEquals"/> when enharmonic equivalence (same sounding pitch class,
  ///   regardless of spelling) is what's needed instead.
  /// </remarks>
  public bool Equals(
    PitchClass other )
  {
    return _noteName == other._noteName && _accidental == other._accidental;
  }

  /// <summary>
  ///   Determines whether this instance and another pitch class are enharmonically equivalent, i.e.,
  ///   they represent the same sounding pitch class regardless of spelling.
  /// </summary>
  /// <param name="other">The pitch class to compare against.</param>
  /// <returns>True if both pitch classes are enharmonically equivalent; otherwise, false.</returns>
  /// <remarks>
  ///   Unlike <see cref="Equals(PitchClass)"/>, which is spelling-sensitive, this comparison treats
  ///   enharmonically-equivalent spellings as equal (e.g., C♯ and D♭ are
  ///   <see cref="EnharmonicEquals"/>-equal but not <see cref="Equals(PitchClass)"/>-equal). Use
  ///   this when the sounding pitch class is what matters rather than its spelling, e.g., pitch-class-set
  ///   membership or key-signature-agnostic comparisons. See also <see cref="EnharmonicComparer"/> for an
  ///   <see cref="IEqualityComparer{T}"/> with this behavior.
  /// </remarks>
  public bool EnharmonicEquals(
    PitchClass other )
  {
    return EnharmonicIndex == other.EnharmonicIndex;
  }

  /// <summary>
  ///   Determines whether this instance and a specified object are equal.
  /// </summary>
  /// <param name="obj">The object to compare with the current instance.</param>
  /// <returns>
  ///   True if the specified object is a <see cref="PitchClass"/> and is equal to the current instance; otherwise, false.
  /// </returns>
  public override bool Equals(
    object? obj )
  {
    return obj is PitchClass other && Equals( other );
  }

  /// <summary>Gets the enharmonic pitch class for this instance or null if none exists.</summary>
  /// <param name="noteName">The name of the enharmonic pitch class.</param>
  /// <returns>The enharmonic.</returns>
  public PitchClass? GetEnharmonic(
    NoteName noteName )
  {
    return GetEnharmonicCore( noteName, EnharmonicIndex );
  }

  /// <inheritdoc/>
  /// <remarks>
  ///   Consistent with the spelling-sensitive <see cref="Equals(PitchClass)"/>: pitch classes with
  ///   different spellings (e.g., C♯ and D♭) hash differently even when enharmonically equivalent.
  /// </remarks>
  public override int GetHashCode()
  {
    return HashCode.Combine( _noteName, _accidental );
  }

  /// <summary>Determines the interval between this instance and the provided pitch class.</summary>
  /// <param name="pitchClass">The pitch class.</param>
  /// <returns>An interval.</returns>
  public Interval GetIntervalTo(
    PitchClass pitchClass )
  {
    // First we determine the interval quantity. We must add one because the interval quantity is 1-based (unison = 1, second = 2, etc.)
    var quantity = (IntervalQuantity) ( pitchClass.NoteName.Subtract( NoteName )
                                                  .Wrap( Constants.NoteNameCount )
                                        + 1 );

    // Then we determine the semitone displacement from the enharmonic index, wrapping around the 12 semitones in an octave
    var semitoneDisplacement = ( pitchClass.EnharmonicIndex - EnharmonicIndex ).Wrap( Constants.OctaveSemitoneCount );
    var quality = Interval.CalcIntervalQuality( quantity, semitoneDisplacement, out var alterationDegree );
    var interval = new Interval( quantity, quality, alterationDegree );
    return interval;
  }

  /// <summary>Parses the provided string.</summary>
  /// <exception cref="FormatException">Thrown when the provided string doesn't represent a PitchClass.</exception>
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

  /// <inheritdoc/>
  public override string ToString()
  {
    return $"{NoteName}{Accidental.ToSymbol()}";
  }

  /// <summary>
  ///   Returns a string representation of the value of this <see cref="PitchClass"/> instance, according to the
  ///   provided format specifier.
  /// </summary>
  /// <param name="format">A custom format string.</param>
  /// <returns>
  ///   A string representation of the value of the current <see cref="PitchClass"/> object as specified by
  ///   <paramref name="format"/>.
  /// </returns>
  /// <remarks>
  ///   <para>Format specifiers:</para>
  ///   <para>"N": Note name pattern. e.g. "C".</para>
  ///   <para>"S": Accidental symbol pattern. e.g. "#".</para>
  /// </remarks>
  public string ToString(
    string format )
  {
    return ToString( format, null );
  }

  /// <summary>
  ///   Returns a string representation of the value of this <see cref="PitchClass"/> instance, according to the
  ///   provided format specifier and format provider.
  /// </summary>
  /// <param name="format">A custom format string.</param>
  /// <param name="provider">The format provider. Not used.</param>
  /// <returns>
  ///   A string representation of the value of the current <see cref="PitchClass"/> object as specified by
  ///   <paramref name="format"/>.
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

  /// <summary>Transposes the current instance by a number of semitones.</summary>
  /// <param name="semitoneCount">Number of semitones. Negative values transpose downward.</param>
  /// <returns>A PitchClass.</returns>
  public PitchClass Transpose(
    int semitoneCount )
  {
    var enharmonicIndex = ( EnharmonicIndex + semitoneCount ).Wrap( Constants.OctaveSemitoneCount );
    return LookupPitchClass( enharmonicIndex );
  }

  /// <summary>Adds an interval to the current instance.</summary>
  /// <param name="interval">An interval to add.</param>
  /// <returns>A PitchClass.</returns>
  public PitchClass Transpose(
    Interval interval )
  {
    // First we calculate the new note name from the interval quantity, wrapping around the 7 note names.
    // We must subtract 1 from the interval quantity because the interval quantity is 1-based (unison = 1, second = 2, etc.)
    var noteIndex = (int) NoteName + ( ( (int) interval.Quantity - 1 ) * ( interval.IsAscending ? 1 : -1 ) );
    var expectedNoteName = (NoteName) noteIndex.Wrap( Constants.NoteNameCount );

    // Next we calculate the new enharmonic index, wrapping around the 12 semitones in an octave
    var semitoneCount = ( EnharmonicIndex + interval.SemitoneCount ).Wrap( Constants.OctaveSemitoneCount );

    // Now we look for a pitch class that matches the calculated note name and the enharmonic index
    return GetEnharmonicCore( expectedNoteName, semitoneCount )
           ?? throw new InvalidOperationException(
             "No pitch class found for the calculated note name and enharmonic index."
           );
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

  /// <summary>
  /// Attempts to parse a PitchClass from the given string, returning any unparsed characters in the tail.
  /// </summary>
  /// <param name="value">The string representation of the pitch class.</param>
  /// <param name="provider">The format provider.</param>
  /// <param name="pitchClass">[out] The parsed pitch class.</param>
  /// <param name="tail">[out] The unparsed characters.</param>
  /// <returns>True if parsing was successful; otherwise, false.</returns>
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

    if( !tail.IsEmpty )
    {
      // Could be an accidental or some other character; use any partial match and
      // leave the tail to be processed by the caller
      Accidental.TryParse( tail, provider, out accidental, out tail );
    }

    pitchClass = new PitchClass( noteName, accidental );
    return true;
  }

  #endregion

  #region Implementation

  /// <summary>
  ///   Gets the enharmonic equivalent of a pitch class for a given note name and enharmonic index.
  /// </summary>
  /// <param name="noteName">The note name.</param>
  /// <param name="enharmonicIndex">The enharmonic index.</param>
  /// <returns>The enharmonic pitch class if it exists; otherwise, null.</returns>
  private static PitchClass? GetEnharmonicCore(
    NoteName noteName,
    int enharmonicIndex )
  {
    // We calculate the accidental that would be required to spell the enharmonic index with the given note name.
    var accidental = ( enharmonicIndex - s_naturalSemitones[(int) noteName] ).Wrap( Constants.OctaveSemitoneCount );

    // If the accidental is greater than half an octave, we can subtract an octave to get a negative accidental.
    if( accidental > Constants.OctaveSemitoneCount / 2 )
    {
      accidental -= Constants.OctaveSemitoneCount;
    }

    // If the accidental is within the range of -2 to 2, we can create a new pitch class with the given note name and accidental.
    return accidental is >= -ACCIDENTAL_OFFSET and <= ACCIDENTAL_OFFSET
      ? new PitchClass( noteName, (Accidental) accidental )
      : null;
  }

  // Finds a pitch class that corresponds to the provided enharmonic index,
  // attempting to match the desired accidental mode
  internal static PitchClass LookupPitchClass(
    int enharmonicIndex )
  {
    var spelling = s_preferredSpellings[enharmonicIndex.Wrap( Constants.OctaveSemitoneCount )];
    var noteName = (NoteName) ( spelling / ACCIDENTAL_COUNT );
    var accidental = (Accidental) ( spelling % ACCIDENTAL_COUNT - ACCIDENTAL_OFFSET );
    return new PitchClass( noteName, accidental );
  }

  #endregion

  #region Operators

  /// <summary>Explicitly converts a pitch class to its spelling-order integer value.</summary>
  /// <param name="pitchClass">The pitch class.</param>
  /// <returns>An integer from -2 through 32, ordered by note name and then accidental.</returns>
  public static explicit operator int(
    PitchClass pitchClass )
  {
    return ( (int) pitchClass.NoteName * ACCIDENTAL_COUNT )
           + (int) pitchClass.Accidental;
  }

  /// <summary>Explicitly converts a spelling-order integer value to a pitch class.</summary>
  /// <param name="value">An integer from -2 through 32.</param>
  /// <returns>The pitch class represented by <paramref name="value"/>.</returns>
  /// <exception cref="ArgumentOutOfRangeException">
  ///   Thrown when <paramref name="value"/> is outside the range -2 through 32.
  /// </exception>
  public static explicit operator PitchClass(
    int value )
  {
    ArgumentOutOfRangeException.ThrowIfLessThan( value, MIN_INTEGER_VALUE );
    ArgumentOutOfRangeException.ThrowIfGreaterThan( value, MAX_INTEGER_VALUE );

    var noteName = (NoteName) ( ( value + ACCIDENTAL_OFFSET ) / ACCIDENTAL_COUNT );
    var accidental = (Accidental) ( value - ( (int) noteName * ACCIDENTAL_COUNT ) );
    return new PitchClass( noteName, accidental );
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
