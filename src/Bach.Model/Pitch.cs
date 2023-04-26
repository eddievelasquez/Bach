// Module Name: Pitch.cs
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
using System.Text;
using Bach.Model.Internal;

namespace Bach.Model;

/// <summary>
///   A Pitch represents the pitch of a sound (<see cref="PitchClass" />)
///   on a given octave.
/// </summary>
/// <remarks>
///   The octave of a Pitch ranges from 0 to 9,
///   which corresponds to MIDI pitches from 12 (C0)
///   to 127 (B9).
/// </remarks>
public readonly struct Pitch
  : IEquatable<Pitch>,
    IComparable<Pitch>
{
#region Constants

  /// <summary>The minimum supported octave.</summary>
  public const int MinOctave = 0;

  /// <summary>The maximum supported octave.</summary>
  public const int MaxOctave = 9;

  internal const double A4Frequency = 440.0;
  internal const int IntervalsPerOctave = 12;

  private static readonly int[] s_semitonesBetween =
  {
    2, // C-D
    2, // D-E
    1, // E-F
    2, // F-G
    2, // G-A
    2, // A-B
    1 // B-C
  };

  // Midi supports C-1, but we only support C0 and above
  private static readonly int s_minAbsoluteValue = CalcAbsoluteValue( NoteName.C, Accidental.Natural, MinOctave );

  // G9 is the highest pitch supported by MIDI
  private static readonly int s_maxAbsoluteValue = CalcAbsoluteValue( NoteName.G, Accidental.Natural, MaxOctave );

  private static readonly Pitch s_a4 = Create( NoteName.A, Accidental.Natural, 4 );

  /// <summary>An empty pitch.</summary>
  public static readonly Pitch Empty = new( PitchClass.B, 9, 128 );

  /// <summary>The minimum possible pitch value.</summary>
  public static readonly Pitch MinValue = Create( PitchClass.C, MinOctave );

  /// <summary>The maximum possible pitch value.</summary>
  public static readonly Pitch MaxValue = Create( PitchClass.G, MaxOctave );

#endregion

#region Fields

  private readonly byte _absoluteValue;
  private readonly byte _octave;
  private readonly ushort _note;

#endregion

#region Constructors

  private Pitch( int absoluteValue )
  {
    Requires.Between( absoluteValue, 0, 127 );

    _absoluteValue = (byte) absoluteValue;
    CalcNote( _absoluteValue, out var note, out _octave );
    _note = (ushort) note;
  }

  private Pitch(
    PitchClass pitchClass,
    int octave,
    int absoluteValue )
  {
    _note = (ushort) pitchClass;
    _octave = (byte) octave;
    _absoluteValue = (byte) absoluteValue;
  }

#endregion

#region Properties

  /// <summary>Gets the number of total pitches.</summary>
  ///
  /// <value>The total number of pitch count.</value>
  public static int TotalPitchCount => MaxValue._absoluteValue - MinValue._absoluteValue;

  /// <summary>Gets a value indicating whether this instance is a valid pitch.</summary>
  /// <value>True if this instance is a valid false, false if it is not.</value>
  public bool IsValid
  {
    get
    {
      var abs = _absoluteValue + (int) PitchClass.Accidental;
      return abs >= s_minAbsoluteValue && abs <= s_maxAbsoluteValue;
    }
  }

  /// <summary>Gets the pitch's pitch class.</summary>
  /// <value>The pitch class.</value>
  public PitchClass PitchClass => (PitchClass) _note;

  /// <summary>Gets the pitch's frequency.</summary>
  /// <value>The frequency.</value>
  public double Frequency
  {
    get
    {
      var interval = _absoluteValue - s_a4._absoluteValue;
      var freq = Math.Pow( 2, interval / 12.0 ) * A4Frequency;
      return freq;
    }
  }

  /// <summary>Gets the pitch's MIDI value.</summary>
  /// <value>The MIDI value.</value>
  public int Midi => _absoluteValue + 12;

  /// <summary>Gets the pitch's octave.</summary>
  /// <value>The octave.</value>
  public int Octave => _octave;

#endregion

#region Public Methods

  /// <summary>Adds number of semitones to the current instance.</summary>
  /// <param name="semitoneCount">Number of semitones.</param>
  /// <returns>A Pitch.</returns>
  public Pitch Add( int semitoneCount )
  {
    var result = new Pitch( _absoluteValue + semitoneCount );
    return result;
  }

  /// <summary>Adds an Interval to a given Pitch.</summary>
  /// <param name="pitch">The pitch.</param>
  /// <param name="interval">A interval to add to it.</param>
  /// <returns>A Pitch.</returns>
  public static Pitch Add(
    Pitch pitch,
    Interval interval )
  {
    var absoluteValue = (byte) ( pitch._absoluteValue + interval.SemitoneCount );
    CalcNote( absoluteValue, out var _, out var octave );

    var newPitchClass = pitch.PitchClass + interval;
    var result = new Pitch( newPitchClass, octave, absoluteValue );
    return result;
  }

  /// <inheritdoc />
  public int CompareTo( Pitch other )
  {
    return _absoluteValue - other._absoluteValue;
  }

  /// <summary>Creates a new Pitch.</summary>
  /// <exception cref="ArgumentOutOfRangeException">Thrown when created pitch would be out of the supported range C0..G9.</exception>
  /// <param name="pitchClass">The pitch class.</param>
  /// <param name="octave">The octave.</param>
  /// <returns>A Pitch.</returns>
  public static Pitch Create(
    PitchClass pitchClass,
    int octave )
  {
    Requires.Between( octave, MinOctave, MaxOctave );

    var abs = CalcAbsoluteValue( pitchClass.NoteName, pitchClass.Accidental, octave );
    if( abs < s_minAbsoluteValue )
    {
      throw new ArgumentOutOfRangeException( $"Must be equal to or greater than {new Pitch( s_minAbsoluteValue )}" );
    }

    if( abs > s_maxAbsoluteValue )
    {
      throw new ArgumentOutOfRangeException( $"Must be equal to or less than {new Pitch( s_maxAbsoluteValue )}" );
    }

    return new Pitch( pitchClass, octave, abs );
  }

  /// <summary>Creates a new Pitch.</summary>
  /// <exception cref="ArgumentOutOfRangeException">Thrown when created pitch would be out of the supported range C0..G9.</exception>
  /// <param name="noteName">Name of the pitch class.</param>
  /// <param name="accidental">The accidental.</param>
  /// <param name="octave">The octave.</param>
  /// <returns>A Pitch.</returns>
  public static Pitch Create(
    NoteName noteName,
    Accidental accidental,
    int octave )
  {
    return Create( PitchClass.Create( noteName, accidental ), octave );
  }

  /// <summary>Creates a pitch from a MIDI pitch value.</summary>
  /// <exception cref="ArgumentOutOfRangeException">Thrown when created pitch would be out of the supported range C0..G9.</exception>
  /// <param name="midi">The MIDI pitch.</param>
  /// <returns>A pitch.</returns>
  public static Pitch CreateFromMidi( int midi )
  {
    Requires.Between( midi, 0, 127 );

    var absoluteValue = midi - 12;
    if( absoluteValue < 0 )
    {
      throw new ArgumentOutOfRangeException( nameof( midi ), "midi is out of range" );
    }

    var note = new Pitch( absoluteValue );
    return note;
  }

  /// <inheritdoc />
  public bool Equals( Pitch obj )
  {
    return obj._absoluteValue == _absoluteValue;
  }

  /// <inheritdoc />
  public override bool Equals( object? obj )
  {
    return obj is Pitch other && Equals( other );
  }

  /// <inheritdoc />
  public override int GetHashCode()
  {
    return _absoluteValue;
  }

  /// <summary>Determines the maximum of the given pitches.</summary>
  /// <param name="a">A Pitch to process.</param>
  /// <param name="b">A Pitch to process.</param>
  /// <returns>The maximum value.</returns>
  public static Pitch Max(
    Pitch a,
    Pitch b )
  {
    return a._absoluteValue >= b._absoluteValue ? a : b;
  }

  /// <summary>Determines the minimum of the given pitches.</summary>
  /// <param name="a">A Pitch to process.</param>
  /// <param name="b">A Pitch to process.</param>
  /// <returns>The minimum value.</returns>
  public static Pitch Min(
    Pitch a,
    Pitch b )
  {
    return a._absoluteValue <= b._absoluteValue ? a : b;
  }

  /// <summary>Parses the provided string.</summary>
  /// <exception cref="FormatException">Thrown when the provided string doesn't represent a Pitch.</exception>
  /// <exception cref="ArgumentNullException">Thrown when a null string is provided.</exception>
  /// <exception cref="ArgumentException">Thrown when an empty string is provided.</exception>
  /// <param name="value">The value to parse.</param>
  /// <returns>A Pitch.</returns>
  public static Pitch Parse( string value )
  {
    if( !TryParse( value, out var result ) )
    {
      throw new FormatException( $"{value} is not a valid pitch" );
    }

    return result;
  }

  /// <summary>Subtracts a number of semitones to the current instance.</summary>
  /// <param name="semitoneCount">Number of semitones.</param>
  /// <returns>A Pitch.</returns>
  public Pitch Subtract( int semitoneCount )
  {
    var result = new Pitch( _absoluteValue - semitoneCount );
    return result;
  }

  /// <inheritdoc />
  public override string ToString()
  {
    return $"{PitchClass}{Octave}";
  }

  /// <summary>Attempts to parse a Pitch from the given string.</summary>
  /// <param name="value">The value to parse.</param>
  /// <param name="pitch">[out] The pitch class.</param>
  /// <returns>True if it succeeds, false if it fails.</returns>
  public static bool TryParse(
    string value,
    out Pitch pitch )
  {
    pitch = Empty;
    if( string.IsNullOrEmpty( value ) )
    {
      return false;
    }

    return char.IsDigit( value, 0 ) ? TryParseMidi( value, ref pitch ) : TryParseNotes( value, ref pitch );
  }

#endregion

#region Implementation

  private static int CalcAbsoluteValue(
    NoteName noteName,
    Accidental accidental,
    int octave )
  {
    var absoluteValue = ( octave * IntervalsPerOctave ) + SemitonesBetween( NoteName.C, noteName ) + (int) accidental;
    return absoluteValue;
  }

  private static void CalcNote(
    byte absoluteValue,
    out PitchClass pitchClass,
    out byte octave )
  {
    octave = (byte) Math.DivRem( absoluteValue, IntervalsPerOctave, out var remainder );
    pitchClass = PitchClass.LookupNote( remainder );
  }

  private static int SemitonesBetween(
    NoteName start,
    NoteName end )
  {
    var semitones = 0;
    var noteName = start;
    while( noteName != end )
    {
      semitones += s_semitonesBetween[(int) noteName];
      noteName = (NoteName) s_semitonesBetween.WrapIndex( 0, (int) (noteName + 1) );
    }

    return semitones;
  }

  private static void TryGetAccidental(
    string value,
    ref int index,
    out Accidental accidental )
  {
    accidental = Accidental.Natural;

    var buf = new StringBuilder();
    for( var i = index; i < value.Length; ++i )
    {
      var ch = value[i];
      if( ch != '#' && ch != 'b' && ch != 'B' )
      {
        if( buf.Length > 0 )
        {
          break;
        }

        return;
      }

      buf.Append( ch );
    }

    if( Accidental.TryParse( buf.ToString(), out accidental ) )
    {
      index += buf.Length;
    }
  }

  private static void TryGetOctave(
    string value,
    ref int index,
    out int octave )
  {
    if( index >= value.Length || !int.TryParse( value.Substring( index, 1 ), out octave ) )
    {
      octave = -1;
      return;
    }

    if( octave is >= MinOctave and <= MaxOctave )
    {
      ++index;
    }
  }

  private static bool TryParseNotes(
    string value,
    ref Pitch pitch )
  {
    if( !NoteName.TryParse( value, out var toneName ) )
    {
      return false;
    }

    var index = 1;
    TryGetAccidental( value, ref index, out var accidental );
    TryGetOctave( value, ref index, out var octave );

    if( index < value.Length || octave < MinOctave || octave > MaxOctave )
    {
      return false;
    }

    pitch = Create( toneName, accidental, octave );
    return true;
  }

  private static bool TryParseMidi(
    string value,
    ref Pitch pitch )
  {
    if( !int.TryParse( value, out var midi ) )
    {
      return false;
    }

    if( midi < 0 || midi > 127 )
    {
      return false;
    }

    pitch = CreateFromMidi( midi );
    return true;
  }

#endregion

#region Operators

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

  /// <summary>Addition operator.</summary>
  /// <param name="pitch">The first value.</param>
  /// <param name="semitoneCount">A value to add to it.</param>
  /// <returns>The result of the operation.</returns>
  public static Pitch operator +(
    Pitch pitch,
    int semitoneCount )
  {
    return pitch.Add( semitoneCount );
  }

  /// <summary>Addition operator.</summary>
  /// <param name="pitch">The first value.</param>
  /// <param name="interval">A value to add to it.</param>
  /// <returns>The result of the operation.</returns>
  public static Pitch operator +(
    Pitch pitch,
    Interval interval )
  {
    return Add( pitch, interval );
  }

  /// <summary>Increment operator.</summary>
  /// <param name="pitch">The pitch.</param>
  /// <returns>The result of the operation.</returns>
  public static Pitch operator ++( Pitch pitch )
  {
    return pitch.Add( 1 );
  }

  /// <summary>Subtraction operator.</summary>
  /// <param name="pitch">The first value.</param>
  /// <param name="semitoneCount">A value to subtract from it.</param>
  /// <returns>The result of the operation.</returns>
  public static Pitch operator -(
    Pitch pitch,
    int semitoneCount )
  {
    return pitch.Subtract( semitoneCount );
  }

  /// <summary>Decrement operator.</summary>
  /// <param name="pitch">The pitch.</param>
  /// <returns>The result of the operation.</returns>
  public static Pitch operator --( Pitch pitch )
  {
    return pitch.Subtract( 1 );
  }

  /// <summary>Subtraction operator.</summary>
  /// <param name="left">The first value.</param>
  /// <param name="right">A value to subtract from it.</param>
  /// <returns>The result of the operation.</returns>
  public static int operator -(
    Pitch left,
    Pitch right )
  {
    return left._absoluteValue - right._absoluteValue;
  }

#endregion
}
