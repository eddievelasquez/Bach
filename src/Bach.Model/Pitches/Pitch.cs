// Module Name: Pitch.cs
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
using System.Text;
using Bach.Model.Internal;

namespace Bach.Model.Pitches;

/// <summary>
///   A Pitch represents the pitch of a sound (<see cref="PitchClass"/>)
///   on a given octave.
/// </summary>
/// <remarks>
///   The octave of a Pitch ranges from 0 to 9, which corresponds to
///   MIDI pitches from 12 (C0) to 127 (B9).
/// </remarks>
public readonly struct Pitch
  : IPitch<Pitch>,
    IPartEvent
{
  #region Constants

  /// <summary>The minimum supported octave.</summary>
  public const int MinOctave = 0;

  /// <summary>The maximum supported octave.</summary>
  public const int MaxOctave = 9;

  private const int MIN_MIDI = 12; // C0
  private const int MAX_MIDI = 127; // B9

  internal const double A4Frequency = 440.0;

  /// <summary>The minimum possible <see cref="Pitch"/> value.</summary>
  public static readonly Pitch MinValue = new( ToMidi( PitchClass.C, MinOctave ) );

  /// <summary>The maximum possible <see cref="Pitch"/> value.</summary>
  public static readonly Pitch MaxValue = new( ToMidi( PitchClass.G, MaxOctave ) );

  private static readonly Pitch s_a4 = new( ToMidi( PitchClass.A, 4 ) );

  #endregion

  #region Fields

  private readonly byte _midi;
  private readonly PitchClass _pitchClass;

  #endregion

  #region Constructors

  /// <summary>
  ///   Creates a pitch from a MIDI value.
  /// </summary>
  /// <param name="midi">The MIDI value of the pitch.</param>
  public Pitch(
    int midi )
  {
    ArgumentOutOfRangeException.ThrowIfLessThan( midi, MIN_MIDI );
    ArgumentOutOfRangeException.ThrowIfGreaterThan( midi, MAX_MIDI );

    _midi = (byte) midi;
    _pitchClass = ToPitchClass( _midi );
  }

  /// <summary>
  ///   Creates a pitch from a pitch class and octave.
  /// </summary>
  /// <param name="pitchClass">The pitch class.</param>
  /// <param name="octave">The octave.</param>
  /// <exception cref="ArgumentOutOfRangeException">
  ///   Thrown when the octave or resulting pitch is outside the supported range C0 to G9.
  /// </exception>
  public Pitch(
    PitchClass pitchClass,
    int octave )
  {
    var midi = ToMidi( pitchClass, octave );

    if( midi < MIN_MIDI || midi > MAX_MIDI )
    {
      throw new ArgumentOutOfRangeException(
        $"The {pitchClass} in octave {octave} is outside the supported range {MinValue} to {MaxValue}."
      );
    }

    _pitchClass = pitchClass;
    _midi = (byte) midi;
  }

  /// <summary>
  ///   Creates a pitch from a note name, accidental, and octave.
  /// </summary>
  /// <param name="noteName">The name of the note.</param>
  /// <param name="accidental">The accidental of the note.</param>
  /// <param name="octave">The octave of the note.</param>
  public Pitch(
    NoteName noteName,
    Accidental accidental,
    int octave )
    : this( new PitchClass( noteName, accidental ), octave )
  {
  }

  #endregion

  #region Properties

  /// <summary>
  ///   Gets an equality comparer that compares pitches by enharmonic equivalence rather than spelling.
  /// </summary>
  public static IEqualityComparer<Pitch> EnharmonicComparer { get; } = new EnharmonicEqualityComparer();

  /// <summary>Gets the total number of supported pitches.</summary>
  /// <value>The total number of supported pitches.</value>
  public static int TotalPitchCount => MAX_MIDI - MIN_MIDI;

  /// <summary>Gets a value indicating whether this instance is a valid pitch.</summary>
  /// <value>True if this instance is a valid pitch, false if it is not.</value>
  public bool IsValid
  {
    get { return Midi is >= MIN_MIDI and <= MAX_MIDI; }
  }

  /// <summary>Gets the pitch's frequency.</summary>
  /// <value>The frequency.</value>
  public double Frequency
  {
    get
    {
      var interval = Midi - s_a4.Midi;
      var freq = Math.Pow( 2, interval / (double) Constants.OctaveSemitoneCount ) * A4Frequency;
      return freq;
    }
  }

  /// <summary>Gets the pitch's octave.</summary>
  /// <value>The octave.</value>
  public int Octave => ( Midi - Constants.OctaveSemitoneCount - PitchClass.SemitoneOffset ) / Constants.OctaveSemitoneCount;

  /// <summary>Gets the pitch's MIDI value.</summary>
  /// <value>The MIDI value.</value>
  public int Midi => _midi;

  #endregion

  #region Public Methods

  /// <inheritdoc/>
  public int CompareTo(
    Pitch other )
  {
    var result = EnharmonicCompareTo( other );
    return result != 0 ? result : PitchClass.CompareTo( other.PitchClass );
  }

  /// <summary>
  ///   Compares the chromatic pitch height of this instance to another pitch, ignoring spelling.
  /// </summary>
  /// <param name="other">The pitch to compare against.</param>
  /// <returns>
  ///   A negative value if this instance sounds lower than <paramref name="other"/>, zero if both
  ///   sound at the same pitch height, or a positive value if this instance sounds higher.
  /// </returns>
  public int EnharmonicCompareTo(
    Pitch other )
  {
    return Midi.CompareTo( other.Midi );
  }

  /// <inheritdoc/>
  public bool Equals(
    Pitch obj )
  {
    return CompareTo( obj ) == 0;
  }

  /// <summary>
  ///   Determines whether this instance and another pitch are enharmonically equivalent.
  /// </summary>
  /// <param name="other">The pitch to compare against.</param>
  /// <returns>True if both pitches have the same sounding pitch; otherwise, false.</returns>
  public bool EnharmonicEquals(
    Pitch other )
  {
    return Midi == other.Midi;
  }

  /// <inheritdoc/>
  public override bool Equals(
    object? obj )
  {
    return obj is Pitch other && Equals( other );
  }

  /// <summary>Gets an enharmonic pitch with the given note name.</summary>
  /// <param name="noteName">The target note name.</param>
  /// <returns>
  ///   The enharmonic pitch, or null if none exists or the result would fall outside the supported range.
  /// </returns>
  public Pitch? GetEnharmonic(
    NoteName noteName )
  {
    var enharmonicPitchClass = PitchClass.GetEnharmonic( noteName );

    if( enharmonicPitchClass is null )
    {
      return null;
    }

    try
    {
      return new Pitch( enharmonicPitchClass.Value, Octave );
    }
    catch( ArgumentOutOfRangeException )
    {
      return null;
    }
  }

  /// <inheritdoc/>
  public override int GetHashCode()
  {
    return Midi;
  }

  /// <summary>Parses the provided string.</summary>
  /// <exception cref="FormatException">Thrown when the provided string doesn't represent a Pitch.</exception>
  /// <exception cref="ArgumentNullException">Thrown when a null string is provided.</exception>
  /// <exception cref="ArgumentException">Thrown when an empty string is provided.</exception>
  /// <param name="value">The value to parse.</param>
  /// <returns>A Pitch.</returns>
  public static Pitch Parse(
    string value )
  {
    ArgumentNullException.ThrowIfNull( value );
    return Parse( value.AsSpan(), null );
  }

  /// <summary>Parses the provided string using the specified format provider.</summary>
  /// <param name="value">The value to parse.</param>
  /// <param name="provider">The format provider.</param>
  /// <returns>A Pitch.</returns>
  public static Pitch Parse(
    string value,
    IFormatProvider? provider )
  {
    ArgumentNullException.ThrowIfNull( value );
    return Parse( value.AsSpan(), provider );
  }

  /// <summary>Parses the provided span using the specified format provider.</summary>
  /// <param name="value">The value to parse.</param>
  /// <param name="provider">The format provider.</param>
  /// <returns>A Pitch.</returns>
  public static Pitch Parse(
    ReadOnlySpan<char> value,
    IFormatProvider? provider )
  {
    if( !TryParse( value, provider, out var result ) )
    {
      throw new FormatException( $"{value.ToString()} is not a valid pitch" );
    }

    return result;
  }

  /// <inheritdoc/>
  public override string ToString()
  {
    return $"{PitchClass}{Octave}";
  }

  /// <summary>
  ///   Returns a string representation of the value of this <see cref="Pitch"/> instance according to the provided
  ///   format.
  /// </summary>
  /// <param name="format">A format string.</param>
  /// <returns>A formatted string.</returns>
  public string ToString(
    string format )
  {
    return ToString( format, null );
  }

  /// <summary>
  ///   Returns a string representation of the value of this <see cref="Pitch"/> instance according to the provided
  ///   format and format provider.
  /// </summary>
  /// <param name="format">A format string.</param>
  /// <param name="provider">The format provider.</param>
  /// <returns>A formatted string.</returns>
  public string ToString(
    string? format,
    IFormatProvider? provider )
  {
    if( string.IsNullOrEmpty( format ) )
    {
      return ToString();
    }

    var buf = new StringBuilder();

    foreach( var f in format )
    {
      switch( f )
      {
        case 'N':
          buf.Append( PitchClass );
          break;

        case 'O':
          buf.Append( Octave );
          break;

        case 'M':
          buf.Append( Midi );
          break;

        case 'F':
          buf.Append( Frequency.ToString( provider ) );
          break;

        default:
          buf.Append( f );
          break;
      }
    }

    return buf.ToString();
  }

  /// <summary>Adds number of semitones to the current instance.</summary>
  /// <param name="semitoneCount">Number of semitones.</param>
  /// <returns>A Pitch.</returns>
  public Pitch Transpose(
    int semitoneCount )
  {
    var result = new Pitch( Midi + semitoneCount );
    return result;
  }

  /// <summary>Adds an interval to the current instance.</summary>
  /// <param name="interval">An interval to add.</param>
  /// <returns>A Pitch.</returns>
  public Pitch Transpose(
    Interval interval )
  {
    var newPitchClass = PitchClass + interval;
    var newMidi = Midi + interval.SemitoneCount;

    var octave = ( newMidi - Constants.OctaveSemitoneCount - newPitchClass.SemitoneOffset )
                 / Constants.OctaveSemitoneCount;

    return new Pitch( newPitchClass, octave );
  }

  /// <summary>
  ///   Attempts to transpose a pitch by the given interval.
  /// </summary>
  /// <param name="pitch">The pitch to transpose.</param>
  /// <param name="interval">The interval to transpose by.</param>
  /// <param name="result">The resulting pitch if the transpose is successful.</param>
  /// <returns>True if the transpose is successful, false otherwise.</returns>
  public static bool TryTranspose(
    Pitch pitch,
    Interval interval,
    out Pitch result )
  {
    var newMidi = pitch.Midi + interval.SemitoneCount;

    if( newMidi < MIN_MIDI || newMidi > MAX_MIDI )
    {
      result = default;
      return false;
    }

    var newPitchClass = pitch.PitchClass + interval;

    var octave = ( newMidi - Constants.OctaveSemitoneCount - newPitchClass.SemitoneOffset )
                 / Constants.OctaveSemitoneCount;

    result = new Pitch( newPitchClass, octave );
    return true;
  }

  /// <summary>Attempts to parse a Pitch from the given string.</summary>
  /// <param name="value">The value to parse.</param>
  /// <param name="pitch">[out] The pitch class.</param>
  /// <returns>True if it succeeds, false if it fails.</returns>
  public static bool TryParse(
    string? value,
    out Pitch pitch )
  {
    return TryParse( value, null, out pitch );
  }

  /// <summary>
  ///   Attempts to parse a Pitch from the given string using the specified format provider.
  /// </summary>
  /// <param name="value">The value to parse.</param>
  /// <param name="provider">The format provider.</param>
  /// <param name="pitch">[out] The pitch class.</param>
  /// <returns>True if it succeeds, false if it fails.</returns>
  public static bool TryParse(
    string? value,
    IFormatProvider? provider,
    out Pitch pitch )
  {
    return TryParse( value.AsSpan(), provider, out pitch );
  }

  /// <summary>Attempts to parse a Pitch from the given string.</summary>
  /// <param name="value">The value to parse.</param>
  /// <param name="pitch">[out] The pitch class.</param>
  /// <returns>True if it succeeds, false if it fails.</returns>
  public static bool TryParse(
    ReadOnlySpan<char> value,
    out Pitch pitch )
  {
    return TryParse( value, null, out pitch );
  }

  /// <summary>
  ///   Attempts to parse a Pitch from the given span using the specified format provider.
  /// </summary>
  /// <param name="value">The value to parse.</param>
  /// <param name="provider">The format provider.</param>
  /// <param name="pitch">[out] The pitch class.</param>
  /// <returns>True if it succeeds, false if it fails.</returns>
  public static bool TryParse(
    ReadOnlySpan<char> value,
    IFormatProvider? provider,
    out Pitch pitch )
  {
    // We only want to return true if the entire string was consumed,
    // so we check that the tail is empty.
    if( TryParse( value, provider, out pitch, out var tail ) && tail.IsEmpty )
    {
      return true;
    }

    pitch = default;
    return false;
  }

  /// <summary>
  ///   Attempts to parse a Pitch from the given span using the specified format provider.
  /// </summary>
  /// <param name="value">The value to parse.</param>
  /// <param name="provider">The format provider.</param>
  /// <param name="pitch">[out] The pitch class.</param>
  /// <param name="tail">[out] The remaining portion of the string.</param>
  /// <returns>True if it succeeds, false if it fails.</returns>
  public static bool TryParse(
    ReadOnlySpan<char> value,
    IFormatProvider? provider,
    out Pitch pitch,
    out ReadOnlySpan<char> tail )
  {
    value = value.TrimStart();

    if( value.IsEmpty )
    {
      pitch = default;
      tail = ReadOnlySpan<char>.Empty;
      return false;
    }

    if( char.IsDigit( value[0] ) )
    {
      return TryParseMidi( value, provider, out pitch, out tail );
    }

    return TryParseNotes( value, provider, out pitch, out tail );
  }

  #endregion

  #region IPartEvent Implementation

  /// <inheritdoc/>
  bool IPartEvent.Any(
    PitchClass pitchClass )
  {
    return PitchClass == pitchClass;
  }

  /// <summary>
  ///   Gets the pitch classes contained in the event.
  /// </summary>
  IEnumerable<PitchClass> IPartEvent.PitchClasses
  {
    get { yield return PitchClass; }
  }

  #endregion

  #region IPitch<Pitch> Implementation

  /// <summary>Gets the note name of the pitch.</summary>
  /// <value>The note name.</value>
  public NoteName NoteName => PitchClass.NoteName;

  /// <summary>Gets the accidental of the pitch.</summary>
  /// <value>The accidental.</value>
  public Accidental Accidental => PitchClass.Accidental;

  /// <summary>Gets the pitch's pitch class.</summary>
  /// <value>The pitch class.</value>
  public PitchClass PitchClass => _pitchClass;

  #endregion

  #region Implementation

  /// <summary>
  ///   Calculates the MIDI value of a pitch class at a given octave.
  /// </summary>
  /// <param name="pitchClass">The pitch class.</param>
  /// <param name="octave">The octave of the pitch.</param>
  /// <returns>The MIDI value of the pitch.</returns>
  private static int ToMidi(
    PitchClass pitchClass,
    int octave )
  {
    var midi = ( octave + 1 ) * Constants.OctaveSemitoneCount + pitchClass.SemitoneOffset;
    return midi;
  }

  /// <summary>
  ///   Calculates the pitch class from a MIDI value.
  /// </summary>
  /// <param name="midi">The MIDI value of the pitch.</param>
  /// <returns>The calculated pitch class.</returns>
  private static PitchClass ToPitchClass(
    byte midi )
  {
    // Calculate the semitone offset within the octave.
    Math.DivRem( midi - Constants.OctaveSemitoneCount, Constants.OctaveSemitoneCount, out var semitonesInOctave );

    return PitchClass.LookupPitchClass( semitonesInOctave );
  }

  /// <summary>
  ///   Attempts to parse a Pitch from the given span using the specified format provider.
  /// </summary>
  /// <param name="value">The span of characters to parse.</param>
  /// <param name="provider">The format provider.</param>
  /// <param name="pitch">[out] The parsed pitch.</param>
  /// <param name="tail">[out] The remaining unparsed characters.</param>
  /// <returns>True if parsing was successful; otherwise, false.</returns>
  private static bool TryParseNotes(
    ReadOnlySpan<char> value,
    IFormatProvider? provider,
    out Pitch pitch,
    out ReadOnlySpan<char> tail )
  {
    value = value.TrimStart();

    if( !PitchClass.TryParse( value, provider, out var pitchClass, out tail ) )
    {
      pitch = default;
      return false;
    }

    var octave = 4;

    // If the tail is not empty, we expect it to be a single digit representing the octave.
    if( !tail.IsEmpty )
    {
      if( !char.IsDigit( tail[0] ) )
      {
        pitch = new Pitch( pitchClass, octave );
        return true;
      }

      // If the tail is a digit, we parse it as the octave.
      if( !int.TryParse( tail, provider, out octave ) || octave < MinOctave || octave > MaxOctave )
      {
        pitch = default;
        return false;
      }

      tail = tail[1..];
    }

    pitch = new Pitch( pitchClass, octave );
    return true;
  }

  /// <summary>
  ///   Attempts to parse a Pitch from the given span using the specified format provider, interpreting the input as a
  ///   MIDI pitch value.
  /// </summary>
  /// <param name="value">The span of characters to parse.</param>
  /// <param name="provider">The format provider.</param>
  /// <param name="pitch">[out] The parsed pitch.</param>
  /// <param name="tail">[out] The remaining unparsed characters.</param>
  /// <returns>True if parsing was successful; otherwise, false.</returns>
  private static bool TryParseMidi(
    ReadOnlySpan<char> value,
    IFormatProvider? provider,
    out Pitch pitch,
    out ReadOnlySpan<char> tail )
  {
    // We expect the MIDI value to be a number between 12 and 127, inclusive.
    if( !int.TryParse( value, provider, out var midi ) || midi is < MIN_MIDI or > MAX_MIDI )
    {
      pitch = default;
      tail = value;
      return false;
    }

    pitch = new Pitch( midi );

    // The tail is the remaining characters after the MIDI number. If the MIDI number is less than 100, it will be 2 digits; otherwise, it will be 3 digits.
    // The case for less than 10 is not reachable because the minimum MIDI value is 12.
    tail = midi switch
    {
      < 100 => value[2..],
      _     => value[3..]
    };

    return true;
  }

  #endregion

  #region Operators

  /// <summary>Greater-than comparison operator.</summary>
  /// <param name="left">The first instance to compare.</param>
  /// <param name="right">The second instance to compare.</param>
  /// <returns>The result of the operation.</returns>
  public static bool operator >(
    Pitch left,
    Pitch right )
  {
    return left.CompareTo( right ) > 0;
  }

  /// <summary>Lesser-than comparison operator.</summary>
  /// <param name="left">The first instance to compare.</param>
  /// <param name="right">The second instance to compare.</param>
  /// <returns>The result of the operation.</returns>
  public static bool operator <(
    Pitch left,
    Pitch right )
  {
    return left.CompareTo( right ) < 0;
  }

  /// <summary>Greater-than-or-equal comparison operator.</summary>
  /// <param name="left">The first instance to compare.</param>
  /// <param name="right">The second instance to compare.</param>
  /// <returns>The result of the operation.</returns>
  public static bool operator >=(
    Pitch left,
    Pitch right )
  {
    return left.CompareTo( right ) >= 0;
  }

  /// <summary>Lesser-than-or-equal comparison operator.</summary>
  /// <param name="left">The first instance to compare.</param>
  /// <param name="right">The second instance to compare.</param>
  /// <returns>The result of the operation.</returns>
  public static bool operator <=(
    Pitch left,
    Pitch right )
  {
    return left.CompareTo( right ) <= 0;
  }

  /// <summary>Equality operator.</summary>
  /// <param name="lhs">The first instance to compare.</param>
  /// <param name="rhs">The second instance to compare.</param>
  /// <returns>The result of the operation.</returns>
  public static bool operator ==(
    Pitch lhs,
    Pitch rhs )
  {
    return Equals( lhs, rhs );
  }

  /// <summary>Inequality operator.</summary>
  /// <param name="lhs">The first instance to compare.</param>
  /// <param name="rhs">The second instance to compare.</param>
  /// <returns>The result of the operation.</returns>
  public static bool operator !=(
    Pitch lhs,
    Pitch rhs )
  {
    return !Equals( lhs, rhs );
  }

  /// <summary>Addition operator.</summary>
  /// <param name="pitch">The first value.</param>
  /// <param name="semitoneCount">A value to add to it.</param>
  /// <returns>The result of the operation.</returns>
  public static Pitch operator +(
    Pitch pitch,
    int semitoneCount )
  {
    return pitch.Transpose( semitoneCount );
  }

  /// <summary>Addition operator.</summary>
  /// <param name="pitch">The first value.</param>
  /// <param name="interval">A value to add to it.</param>
  /// <returns>The result of the operation.</returns>
  public static Pitch operator +(
    Pitch pitch,
    Interval interval )
  {
    return pitch.Transpose( interval );
  }

  /// <summary>Increment operator.</summary>
  /// <param name="pitch">The pitch.</param>
  /// <returns>The result of the operation.</returns>
  public static Pitch operator ++(
    Pitch pitch )
  {
    return pitch.Transpose( 1 );
  }

  /// <summary>Subtraction operator.</summary>
  /// <param name="pitch">The first value.</param>
  /// <param name="semitoneCount">A value to subtract from it.</param>
  /// <returns>The result of the operation.</returns>
  public static Pitch operator -(
    Pitch pitch,
    int semitoneCount )
  {
    return pitch.Transpose( -semitoneCount );
  }

  /// <summary>Decrement operator.</summary>
  /// <param name="pitch">The pitch.</param>
  /// <returns>The result of the operation.</returns>
  public static Pitch operator --(
    Pitch pitch )
  {
    return pitch.Transpose( -1 );
  }

  /// <summary>Subtraction operator.</summary>
  /// <param name="left">The first value.</param>
  /// <param name="right">A value to subtract from it.</param>
  /// <returns>The result of the operation.</returns>
  public static int operator -(
    Pitch left,
    Pitch right )
  {
    return left.Midi - right.Midi;
  }

  #endregion

  #region Nested Types

  private sealed class EnharmonicEqualityComparer: IEqualityComparer<Pitch>
  {
    #region Public Methods

    public bool Equals(
      Pitch x,
      Pitch y )
    {
      return x.EnharmonicEquals( y );
    }

    public int GetHashCode(
      Pitch obj )
    {
      return obj._midi;
    }

    #endregion
  }

  #endregion
}
