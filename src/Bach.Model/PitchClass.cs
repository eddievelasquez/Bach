// Module Name: PitchClass.cs
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
using System.Diagnostics;
using System.Diagnostics.Contracts;
using Bach.Model.Internal;

namespace Bach.Model;

/// <summary>
///   A PitchClass represents a combination of a <see cref="P:Bach.Model.NoteName" />
///   and an optional <see cref="P:Bach.Model.Accidental" /> following the Scientific Pitch Notation.
/// </summary>
public readonly struct PitchClass
  : IEquatable<PitchClass>,
    IComparable<PitchClass>
{
#region Constants

  private const int SemitoneCount = 12;

  private static readonly PitchClass[] s_pitchClasses =
  {
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
  };

  private static readonly int[] s_noteNameIndices =
  {
    1, // NoteName.C
    7, // NoteName.D
    13, // NoteName.E
    16, // NoteName.F
    22, // NoteName.G
    27, // NoteName.A
    33 // NoteName.B
  };

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

  /// <summary>Gets the name of the pitch class.</summary>
  /// <value>The name of the pitch class.</value>
  public NoteName NoteName => (NoteName) _noteName;

  /// <summary>Gets the accidental.</summary>
  /// <value>The accidental.</value>
  public Accidental Accidental => (Accidental) _accidental;

#endregion

#region Public Methods

  /// <summary>Adds a number of semitones to the current instance.</summary>
  /// <param name="semitoneCount">Number of semitones.</param>
  /// <returns>A PitchClass.</returns>
  public PitchClass Add( int semitoneCount )
  {
    var enharmonicIndex = s_enharmonics.WrapIndex( 0, _enharmonicIndex + semitoneCount );
    return LookupNote( enharmonicIndex );
  }

  /// <summary>Adds an interval to the current instance.</summary>
  /// <param name="interval">An interval to add.</param>
  /// <returns>A PitchClass.</returns>
  public PitchClass Add( Interval interval )
  {
    return AddInterval( (int) interval.Quantity, interval.SemitoneCount );
  }

  /// <inheritdoc />
  public int CompareTo( PitchClass other )
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
  public static PitchClass Create( NoteName noteName )
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
  public bool Equals( PitchClass other )
  {
    return _enharmonicIndex == other._enharmonicIndex;
  }

  /// <inheritdoc />
  public override bool Equals( object? obj )
  {
    return obj is PitchClass other && Equals( other );
  }

  /// <summary>Gets the enharmonic pitch class for this instance or null if none exists.</summary>
  /// <param name="noteName">The name of the enharmonic pitch class.</param>
  /// <returns>The enharmonic.</returns>
  [Pure]
  public PitchClass? GetEnharmonic( NoteName noteName )
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
  public static PitchClass Parse( string value )
  {
    Requires.NotNullOrEmpty( value );

    if( !TryParse( value, out var result ) )
    {
      throw new FormatException( $"{value} is not a valid pitch class" );
    }

    return result;
  }

  /// <summary>Subtracts an interval from the current instance.</summary>
  /// <param name="interval">An interval to subtract.</param>
  /// <returns>A PitchClass.</returns>
  public PitchClass Subtract( Interval interval )
  {
    return AddInterval( -(int) interval.Quantity, -interval.SemitoneCount );
  }

  /// <summary>Subtracts a number of semitones from the current instance.</summary>
  /// <param name="semitoneCount">Number of semitones.</param>
  /// <returns>A PitchClass.</returns>
  public PitchClass Subtract( int semitoneCount )
  {
    var enharmonicIndex = s_enharmonics.WrapIndex( 0, _enharmonicIndex - semitoneCount );
    return LookupNote( enharmonicIndex );
  }

  /// <summary>Determines the interval between this instance and the provided pitch class.</summary>
  /// <param name="pitchClass">The pitch class.</param>
  /// <returns>An interval.</returns>
  public Interval Subtract( PitchClass pitchClass )
  {
    // First we determine the interval quantity
    var quantity = (IntervalQuantity) ( pitchClass.NoteName - NoteName );

    // Then we determine the semitone count
    var semitoneCount = ArrayExtensions.WrapIndex( SemitoneCount, pitchClass._enharmonicIndex - _enharmonicIndex );
    var interval = new Interval( quantity, semitoneCount );
    return interval;
  }

  /// <inheritdoc />
  public override string ToString()
  {
    return $"{NoteName}{Accidental.ToSymbol()}";
  }

  /// <summary>Attempts to parse a PitchClass from the given string.</summary>
  /// <param name="value">The value to parse.</param>
  /// <param name="pitchClass">[out] The pitch class.</param>
  /// <returns>True if it succeeds, false if it fails.</returns>
  public static bool TryParse(
    string value,
    out PitchClass pitchClass )
  {
    if( string.IsNullOrEmpty( value ) )
    {
      pitchClass = C;
      return false;
    }

    value = value.Trim();
    if( !NoteName.TryParse( value, out var toneName ) )
    {
      pitchClass = C;
      return false;
    }

    var accidental = Accidental.Natural;
    if( value.Length > 1 && !Accidental.TryParse( value.Substring( 1 ), out accidental ) )
    {
      pitchClass = C;
      return false;
    }

    pitchClass = Create( toneName, accidental );
    return true;
  }

#endregion

#region Implementation

  private PitchClass AddInterval(
    int intervalQuantity,
    int semitoneCount )
  {
    var calculatedNoteName = NoteName + intervalQuantity;
    var calculatedPitchClass = this + semitoneCount;
    if( calculatedPitchClass.NoteName == calculatedNoteName )
    {
      return calculatedPitchClass;
    }

    // Deal with enharmonics
    var enharmonic = calculatedPitchClass.GetEnharmonic( calculatedNoteName );
    return enharmonic ?? calculatedPitchClass;
  }

  // Finds a pitch class that corresponds to the provided enharmonic index,
  // attempting to match the desired accidental mode
  internal static PitchClass LookupNote( int enharmonicIndex )
  {
    // Starting from Natural all the way up to DoubleSharp, find the corresponding enharmonic
    var accidentalIndex = (int) Accidental.Natural + Math.Abs( (int) Accidental.DoubleFlat );
    var maxAccidentalIndex = (int) Accidental.DoubleSharp + Math.Abs( (int) Accidental.DoubleFlat );

    while( accidentalIndex <= maxAccidentalIndex )
    {
      var noteIndex = s_enharmonics[enharmonicIndex, accidentalIndex];
      if( noteIndex != -1 )
      {
        return s_pitchClasses[noteIndex];
      }

      ++accidentalIndex;
    }

    Trace.Assert( false, "Internal error! Must always find a pitch class" );
    return C;
  }

#endregion

#region Operators

  /// <summary>Explicit cast that converts the given pitch class to an int.</summary>
  /// <param name="pitchClass">The pitch class.</param>
  /// <returns>The result of the operation.</returns>
  public static explicit operator int( PitchClass pitchClass )
  {
    return pitchClass._noteIndex;
  }

  /// <summary>Explicit cast that converts the given int to a PitchClass.</summary>
  /// <param name="value">The value.</param>
  /// <returns>The result of the operation.</returns>
  public static explicit operator PitchClass( int value )
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
    return pitchClass.Add( semitoneCount );
  }

  /// <summary>Increment operator.</summary>
  /// <param name="pitchClass">The pitch class.</param>
  /// <returns>The result of the operation.</returns>
  public static PitchClass operator ++( PitchClass pitchClass )
  {
    return pitchClass.Add( 1 );
  }

  /// <summary>Subtraction operator.</summary>
  /// <param name="pitchClass">The first value.</param>
  /// <param name="semitoneCount">A number of semitones to subtract from it.</param>
  /// <returns>The result of the operation.</returns>
  public static PitchClass operator -(
    PitchClass pitchClass,
    int semitoneCount )
  {
    return pitchClass.Subtract( semitoneCount );
  }

  /// <summary>Decrement operator.</summary>
  /// <param name="pitchClass">The pitch class.</param>
  /// <returns>The result of the operation.</returns>
  public static PitchClass operator --( PitchClass pitchClass )
  {
    return pitchClass.Subtract( 1 );
  }

  /// <summary>Addition operator.</summary>
  /// <param name="pitchClass">The pitch class.</param>
  /// <param name="interval">An interval to add to the pitch class.</param>
  /// <returns>A pitchClass.</returns>
  public static PitchClass operator +(
    PitchClass pitchClass,
    Interval interval )
  {
    return pitchClass.Add( interval );
  }

  /// <summary>Addition operator.</summary>
  /// <param name="pitchClass">The pitch class.</param>
  /// <param name="interval">An interval to add to the pitch class.</param>
  /// <returns>A pitchClass.</returns>
  public static PitchClass operator -(
    PitchClass pitchClass,
    Interval interval )
  {
    return pitchClass.Subtract( interval );
  }

  /// <summary>Subtraction operator.</summary>
  /// <param name="a">The first value.</param>
  /// <param name="b">A value to subtract from it.</param>
  /// <returns>The result of the operation.</returns>
  public static Interval operator -(
    PitchClass a,
    PitchClass b )
  {
    return a.Subtract( b );
  }

#endregion
}
