// Module Name: Note.cs
// Project:     Bach.Model
// Copyright (c) 2012, 2019  Eddie Velasquez.
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

namespace Bach.Model
{
  using System;
  using System.Diagnostics;
  using System.Diagnostics.Contracts;
  using Internal;
  using Contract = Internal.Contract;

  /// <summary>
  ///   A Note represents a combination of a <see cref="P:Bach.Model.Note.NoteName" />
  ///   and an optional <see cref="P:Bach.Model.Note.Accidental" /> following the English naming
  ///   convention for the 12 note chromatic scale.
  /// </summary>
  public readonly struct Note
    : IEquatable<Note>,
      IComparable<Note>
  {
    #region Constants

    private static readonly Note[] s_notes =
    {
      new Note(0, 0, NoteName.D, Accidental.DoubleFlat),
      new Note(1, 0, NoteName.C, Accidental.Natural),
      new Note(2, 0, NoteName.B, Accidental.Sharp),
      new Note(3, 1, NoteName.D, Accidental.Flat),
      new Note(4, 1, NoteName.C, Accidental.Sharp),
      new Note(5, 1, NoteName.B, Accidental.DoubleSharp),
      new Note(6, 2, NoteName.E, Accidental.DoubleFlat),
      new Note(7, 2, NoteName.D, Accidental.Natural),
      new Note(8, 2, NoteName.C, Accidental.DoubleSharp),
      new Note(9, 3, NoteName.F, Accidental.DoubleFlat),
      new Note(10, 3, NoteName.E, Accidental.Flat),
      new Note(11, 3, NoteName.D, Accidental.Sharp),
      new Note(12, 4, NoteName.F, Accidental.Flat),
      new Note(13, 4, NoteName.E, Accidental.Natural),
      new Note(14, 4, NoteName.D, Accidental.DoubleSharp),
      new Note(15, 5, NoteName.G, Accidental.DoubleFlat),
      new Note(16, 5, NoteName.F, Accidental.Natural),
      new Note(17, 5, NoteName.E, Accidental.Sharp),
      new Note(18, 6, NoteName.G, Accidental.Flat),
      new Note(19, 6, NoteName.F, Accidental.Sharp),
      new Note(20, 6, NoteName.E, Accidental.DoubleSharp),
      new Note(21, 7, NoteName.A, Accidental.DoubleFlat),
      new Note(22, 7, NoteName.G, Accidental.Natural),
      new Note(23, 7, NoteName.F, Accidental.DoubleSharp),
      new Note(24, 8, NoteName.A, Accidental.Flat),
      new Note(25, 8, NoteName.G, Accidental.Sharp),
      new Note(26, 9, NoteName.B, Accidental.DoubleFlat),
      new Note(27, 9, NoteName.A, Accidental.Natural),
      new Note(28, 9, NoteName.G, Accidental.DoubleSharp),
      new Note(29, 10, NoteName.C, Accidental.DoubleFlat),
      new Note(30, 10, NoteName.B, Accidental.Flat),
      new Note(31, 10, NoteName.A, Accidental.Sharp),
      new Note(32, 11, NoteName.C, Accidental.Flat),
      new Note(33, 11, NoteName.B, Accidental.Natural),
      new Note(34, 11, NoteName.A, Accidental.DoubleSharp)
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

    #region Data Members

    private readonly sbyte _accidental;
    private readonly byte _enharmonicIndex;
    private readonly byte _noteIndex;
    private readonly byte _noteName;

    #endregion

    #region Constructors

    private Note(int noteIndex,
                 int enharmonicIndex,
                 NoteName noteName,
                 Accidental accidental)
    {
      Debug.Assert(noteIndex >= 0 && noteIndex <= 34);
      Debug.Assert(enharmonicIndex >= 0 && enharmonicIndex <= 11);

      _noteIndex = (byte)noteIndex;
      _enharmonicIndex = (byte)enharmonicIndex;
      _noteName = (byte)noteName;
      _accidental = (sbyte)accidental;
    }

    #endregion

    #region Properties

    /// <summary>C note.</summary>
    public static Note C => s_notes[1];

    /// <summary>C♯ note.</summary>
    public static Note CSharp => s_notes[4];

    /// <summary>D♭ note.</summary>
    public static Note DFlat => s_notes[3];

    /// <summary>D note.</summary>
    public static Note D => s_notes[7];

    /// <summary>D♯ note.</summary>
    public static Note DSharp => s_notes[11];

    /// <summary>E♭ note.</summary>
    public static Note EFlat => s_notes[10];

    /// <summary>E note.</summary>
    public static Note E => s_notes[13];

    /// <summary>F note.</summary>
    public static Note F => s_notes[16];

    /// <summary>F♯ note.</summary>
    public static Note FSharp => s_notes[19];

    /// <summary>G♭ note.</summary>
    public static Note GFlat => s_notes[18];

    /// <summary>G note.</summary>
    public static Note G => s_notes[22];

    /// <summary>G♯ note.</summary>
    public static Note GSharp => s_notes[25];

    /// <summary>A♭ note.</summary>
    public static Note AFlat => s_notes[24];

    /// <summary>A note.</summary>
    public static Note A => s_notes[27];

    /// <summary>A♯ note.</summary>
    public static Note ASharp => s_notes[31];

    /// <summary>B♭ note.</summary>
    public static Note BFlat => s_notes[30];

    /// <summary>B note.</summary>
    public static Note B => s_notes[33];

    /// <summary>Gets or sets the accidental mode.</summary>
    /// <value>The accidental mode.</value>
    public static AccidentalMode AccidentalMode { get; set; } = AccidentalMode.FavorSharps;

    /// <summary>Gets the name of the note.</summary>
    /// <value>The name of the note.</value>
    public NoteName NoteName => (NoteName)_noteName;

    /// <summary>Gets the accidental.</summary>
    /// <value>The accidental.</value>
    public Accidental Accidental => (Accidental)_accidental;

    #endregion

    #region IComparable<Note> Members

    /// <inheritdoc />
    public int CompareTo(Note other) => _enharmonicIndex - other._enharmonicIndex;

    #endregion

    #region IEquatable<Note> Members

    /// <inheritdoc />
    public bool Equals(Note other) => _enharmonicIndex == other._enharmonicIndex;

    #endregion

    #region Public Methods

    /// <summary>Creates a new Note.</summary>
    /// <exception cref="ArgumentOutOfRangeException">
    ///   Thrown when one or more arguments are outside the
    ///   required range.
    /// </exception>
    /// <param name="noteName">The name of the note.</param>
    /// <returns>A Note.</returns>
    public static Note Create(NoteName noteName) => Create(noteName, Accidental.Natural);

    /// <summary>Creates a new Note.</summary>
    /// <exception cref="ArgumentOutOfRangeException">
    ///   Thrown when one or more arguments are outside the
    ///   required range.
    /// </exception>
    /// <param name="noteName">The name of the note.</param>
    /// <param name="accidental">(Optional) The accidental.</param>
    /// <returns>A Note.</returns>
    public static Note Create(NoteName noteName,
                              Accidental accidental)
    {
      // This really doesn't create a Note but returns one of the pre-created ones
      // from the enharmonics table.

      // First we determine the row in the enharmonics table that corresponds to the
      // note name.
      int noteIndex = s_noteNameIndices[(int)noteName];
      Note note = s_notes[noteIndex];
      if( accidental == Accidental.Natural )
      {
        return note;
      }

      int enharmonicIndex = note._enharmonicIndex;

      // Next we ensure that the enharmonic index wraps around when added to the accidental (-2 .. 2)
      enharmonicIndex = s_enharmonics.WrapIndex(0, enharmonicIndex + (int)accidental);

      // Next we determine the index of the note in the note table (Offset by DoubleFlat, so 0..3)
      int accidentalIndex = (int)accidental + Math.Abs((int)Accidental.DoubleFlat);
      noteIndex = s_enharmonics[enharmonicIndex, accidentalIndex];
      Debug.Assert(noteIndex != -1);

      note = s_notes[noteIndex];
      return note;
    }

    /// <summary>Gets the enharmonic note for this instance or null if none exists.</summary>
    /// <param name="noteName">The name of the enharmonic note.</param>
    /// <returns>The enharmonic.</returns>
    [Pure]
    public Note? GetEnharmonic(NoteName noteName)
    {
      int accidentalOffset = Math.Abs((int)Accidental.DoubleFlat);
      int enharmonicIndex = _enharmonicIndex;

      for( var accidental = (int)Accidental.DoubleFlat; accidental <= (int)Accidental.DoubleSharp; ++accidental )
      {
        int accidentalIndex = accidental + accidentalOffset;
        int noteIndex = s_enharmonics[enharmonicIndex, accidentalIndex];
        if( noteIndex == -1 )
        {
          continue;
        }

        Note note = s_notes[noteIndex];
        if( note.NoteName == noteName )
        {
          return note;
        }
      }

      return null;
    }

    /// <summary>Attempts to parse a Note from the given string.</summary>
    /// <param name="value">The value to parse.</param>
    /// <param name="note">[out] The note.</param>
    /// <returns>True if it succeeds, false if it fails.</returns>
    public static bool TryParse(string value,
                                out Note note)
    {
      if( string.IsNullOrEmpty(value) )
      {
        note = C;
        return false;
      }

      if( !NoteName.TryParse(value, out NoteName toneName) )
      {
        note = C;
        return false;
      }

      Accidental accidental = Accidental.Natural;
      if( value.Length > 1 && !Accidental.TryParse(value.Substring(1), out accidental) )
      {
        note = C;
        return false;
      }

      note = Create(toneName, accidental);
      return true;
    }

    /// <summary>Parses te provided string.</summary>
    /// <exception cref="FormatException">Thrown when the provided string doesn't represent a a Note.</exception>
    /// <exception cref="ArgumentNullException">Thrown when a null string is provided.</exception>
    /// <exception cref="ArgumentException">Thrown when an empty string is provided.</exception>
    /// <param name="value">The value to parse.</param>
    /// <returns>A Note.</returns>
    public static Note Parse(string value)
    {
      Contract.RequiresNotNullOrEmpty(value, "Must provide a value");

      if( !TryParse(value, out Note result) )
      {
        throw new FormatException($"{value} is not a valid note");
      }

      return result;
    }

    /// <summary>
    ///   Calculate the number of semitones between two notes
    /// </summary>
    /// <param name="a">The first note.</param>
    /// <param name="b">The second note.</param>
    /// <returns>The number of semitones.</returns>
    public static int SemitonesBetween(Note a,
                                       Note b)
    {
      Note current = a;
      var count = 0;

      while( current != b )
      {
        ++count;
        ++current;
      }

      return count;
    }

    /// <summary>Adds a number of semitones to the current instance.</summary>
    /// <param name="semitoneCount">Number of semitones.</param>
    /// <param name="mode">(Optional) The accidental mode.</param>
    /// <returns>A Note.</returns>
    public Note Add(int semitoneCount,
                    AccidentalMode mode = AccidentalMode.FavorSharps)
    {
      int enharmonicIndex = s_enharmonics.WrapIndex(0, _enharmonicIndex + semitoneCount);
      return LookupNote(enharmonicIndex, mode == AccidentalMode.FavorSharps);
    }

    /// <summary>Adds an interval to the current instance.</summary>
    /// <param name="interval">An interval to add.</param>
    /// <returns>A Note.</returns>
    public Note Add(Interval interval) => AddInterval((int)interval.Quantity, interval.SemitoneCount);

    /// <summary>Subtracts an interval from the current instance.</summary>
    /// <param name="interval">An interval to subtract.</param>
    /// <returns>A Note.</returns>
    public Note Subtract(Interval interval) => AddInterval(-(int)interval.Quantity, -interval.SemitoneCount);

    /// <summary>Subtracts a number of semitones from the current instance.</summary>
    /// <param name="semitoneCount">Number of semitones.</param>
    /// <param name="mode">(Optional) The accidental mode.</param>
    /// <returns>A Note.</returns>
    public Note Subtract(int semitoneCount,
                         AccidentalMode mode = AccidentalMode.FavorSharps)
    {
      int enharmonicIndex = s_enharmonics.WrapIndex(0, _enharmonicIndex - semitoneCount);
      return LookupNote(enharmonicIndex, mode == AccidentalMode.FavorSharps);
    }

    /// <summary>Determines the interval between two notes.</summary>
    /// <param name="a">The first note.</param>
    /// <param name="b">The second note.</param>
    /// <returns>An interval.</returns>
    public static Interval Subtract(Note a,
                                    Note b)
    {
      // First we determine the interval quantity
      var quantity = IntervalQuantity.Unison;
      NoteName current = a.NoteName;

      while( current != b.NoteName )
      {
        ++quantity;
        ++current;
      }

      // Then we determine the semitone count
      int semitoneCount = SemitonesBetween(a, b);
      var interval = new Interval(quantity, semitoneCount);
      return interval;
    }

    #endregion

    #region Overrides

    /// <inheritdoc />
    public override bool Equals(object obj)
    {
      if( obj is null )
      {
        return false;
      }

      return obj.GetType() == GetType() && Equals((Note)obj);
    }

    /// <inheritdoc />
    public override int GetHashCode() => _enharmonicIndex;

    /// <inheritdoc />
    public override string ToString() => $"{NoteName}{Accidental.ToSymbol()}";

    #endregion

    #region  Implementation

    private Note AddInterval(int intervalQuantity,
                             int semitoneCount)
    {
      NoteName calculatedNoteName = NoteName + intervalQuantity;
      Note calculatedNote = this + semitoneCount;
      if( calculatedNote.NoteName == calculatedNoteName )
      {
        return calculatedNote;
      }

      // Deal with enharmonics
      Note? enharmonic = calculatedNote.GetEnharmonic(calculatedNoteName);
      Debug.Assert(enharmonic.HasValue, "Enharmonic not found!");

      return enharmonic.Value;
    }

    // Finds a note that corresponds to the provided enharmonic index,
    // attempting to match the desired accidental mode
    internal static Note LookupNote(int enharmonicIndex,
                                    bool favorSharps)
    {
      return favorSharps ? LookupSharp(enharmonicIndex) : LookupFlat(enharmonicIndex);
    }

    private static Note LookupSharp(int enharmonicIndex)
    {
      // Starting from Natural all the way up to DoubleSharp, find the corresponding enharmonic
      int accidentalIndex = (int)Accidental.Natural + Math.Abs((int)Accidental.DoubleFlat);
      int maxAccidentalIndex = (int)Accidental.DoubleSharp + Math.Abs((int)Accidental.DoubleFlat);

      while( accidentalIndex <= maxAccidentalIndex )
      {
        int noteIndex = s_enharmonics[enharmonicIndex, accidentalIndex];
        if( noteIndex != -1 )
        {
          return s_notes[noteIndex];
        }

        ++accidentalIndex;
      }

      Trace.Assert(false, "Internal error! Must always find a note");
      return C;
    }

    private static Note LookupFlat(int enharmonicIndex)
    {
      // Starting from Natural all the way down to DoubleFlat, find the corresponding enharmonic
      int accidentalIndex = (int)Accidental.Natural + Math.Abs((int)Accidental.DoubleFlat);

      while( accidentalIndex >= 0 )
      {
        int noteIndex = s_enharmonics[enharmonicIndex, accidentalIndex];
        if( noteIndex != -1 )
        {
          return s_notes[noteIndex];
        }

        --accidentalIndex;
      }

      Trace.Assert(false, "Internal error! Must always find a note");
      return C;
    }

    #endregion

    #region Operators

    /// <summary>Explicit cast that converts the given note to an int.</summary>
    /// <param name="note">The note.</param>
    /// <returns>The result of the operation.</returns>
    public static explicit operator int(Note note) => note._noteIndex;

    /// <summary>Explicit cast that converts the given int to a Note.</summary>
    /// <param name="value">The value.</param>
    /// <returns>The result of the operation.</returns>
    public static explicit operator Note(int value) => s_notes[value];

    /// <summary>Equality operator.</summary>
    /// <param name="left">The first instance to compare.</param>
    /// <param name="right">The second instance to compare.</param>
    /// <returns>The result of the operation.</returns>
    public static bool operator==(Note left,
                                  Note right)
      => Equals(left, right);

    /// <summary>Inequality operator.</summary>
    /// <param name="left">The first instance to compare.</param>
    /// <param name="right">The second instance to compare.</param>
    /// <returns>The result of the operation.</returns>
    public static bool operator!=(Note left,
                                  Note right)
      => !Equals(left, right);

    /// <summary>Greater-than comparison operator.</summary>
    /// <param name="left">The first instance to compare.</param>
    /// <param name="right">The second instance to compare.</param>
    /// <returns>The result of the operation.</returns>
    public static bool operator>(Note left,
                                 Note right)
      => left.CompareTo(right) > 0;

    /// <summary>Less-than comparison operator.</summary>
    /// <param name="left">The first instance to compare.</param>
    /// <param name="right">The second instance to compare.</param>
    /// <returns>The result of the operation.</returns>
    public static bool operator<(Note left,
                                 Note right)
      => left.CompareTo(right) < 0;

    /// <summary>Greater-than-or-equal comparison operator.</summary>
    /// <param name="left">The first instance to compare.</param>
    /// <param name="right">The second instance to compare.</param>
    /// <returns>The result of the operation.</returns>
    public static bool operator>=(Note left,
                                  Note right)
      => left.CompareTo(right) >= 0;

    /// <summary>Less-than-or-equal comparison operator.</summary>
    /// <param name="left">The first instance to compare.</param>
    /// <param name="right">The second instance to compare.</param>
    /// <returns>The result of the operation.</returns>
    public static bool operator<=(Note left,
                                  Note right)
      => left.CompareTo(right) <= 0;

    /// <summary>Addition operator.</summary>
    /// <param name="note">The first value.</param>
    /// <param name="semitoneCount">A number of semitones to add to it.</param>
    /// <returns>The result of the operation.</returns>
    public static Note operator+(Note note,
                                 int semitoneCount)
      => note.Add(semitoneCount, AccidentalMode);

    /// <summary>Increment operator.</summary>
    /// <param name="note">The note.</param>
    /// <returns>The result of the operation.</returns>
    public static Note operator++(Note note) => note.Add(1, AccidentalMode);

    /// <summary>Subtraction operator.</summary>
    /// <param name="note">The first value.</param>
    /// <param name="semitoneCount">A number of semitones to subtract from it.</param>
    /// <returns>The result of the operation.</returns>
    public static Note operator-(Note note,
                                 int semitoneCount)
      => note.Subtract(semitoneCount, AccidentalMode);

    /// <summary>Decrement operator.</summary>
    /// <param name="note">The note.</param>
    /// <returns>The result of the operation.</returns>
    public static Note operator--(Note note) => note.Subtract(1, AccidentalMode);

    /// <summary>Addition operator.</summary>
    /// <param name="note">The note.</param>
    /// <param name="interval">An interval to add to the note.</param>
    /// <returns>A note.</returns>
    public static Note operator+(Note note,
                                 Interval interval)
      => note.Add(interval);

    /// <summary>Addition operator.</summary>
    /// <param name="note">The note.</param>
    /// <param name="interval">An interval to add to the note.</param>
    /// <returns>A note.</returns>
    public static Note operator-(Note note,
                                 Interval interval)
      => note.Subtract(interval);

    /// <summary>Subtraction operator.</summary>
    /// <param name="a">The first value.</param>
    /// <param name="b">A value to subtract from it.</param>
    /// <returns>The result of the operation.</returns>
    public static Interval operator-(Note a,
                                     Note b)
      => Subtract(a, b);

    #endregion
  }
}
