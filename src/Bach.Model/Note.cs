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
  public struct Note
    : IEquatable<Note>,
      IComparable<Note>
  {
    #region Constants

    private const ushort ToneNameMask = 7;
    private const ushort ToneNameShift = 7;
    private const ushort AccidentalMask = 7;
    private const ushort AccidentalShift = 4;
    private const ushort AbsoluteValueMask = 0x0F;
    private const int AbsoluteValueCount = 12;

    // DoubleFlat, Flat, Natural, Sharp, DoubleSharp
    private static readonly Note?[,] s_enharmonics =
    {
      { Create(0, NoteName.D, Accidental.DoubleFlat), null, Create(0, NoteName.C, Accidental.Natural), Create(0, NoteName.B, Accidental.Sharp), null },
      { null, Create(1, NoteName.D, Accidental.Flat), null, Create(1, NoteName.C, Accidental.Sharp), Create(1, NoteName.B, Accidental.DoubleSharp) },
      { Create(2, NoteName.E, Accidental.DoubleFlat), null, Create(2, NoteName.D, Accidental.Natural), null, Create(2, NoteName.C, Accidental.DoubleSharp) },
      { Create(3, NoteName.F, Accidental.DoubleFlat), Create(3, NoteName.E, Accidental.Flat), null, Create(3, NoteName.D, Accidental.Sharp), null },
      { null, Create(4, NoteName.F, Accidental.Flat), Create(4, NoteName.E, Accidental.Natural), null, Create(4, NoteName.D, Accidental.DoubleSharp) },
      { Create(5, NoteName.G, Accidental.DoubleFlat), null, Create(5, NoteName.F, Accidental.Natural), Create(5, NoteName.E, Accidental.Sharp), null },
      { null, Create(6, NoteName.G, Accidental.Flat), null, Create(6, NoteName.F, Accidental.Sharp), Create(6, NoteName.E, Accidental.DoubleSharp) },
      { Create(7, NoteName.A, Accidental.DoubleFlat), null, Create(7, NoteName.G, Accidental.Natural), null, Create(7, NoteName.F, Accidental.DoubleSharp) },
      { null, Create(8, NoteName.A, Accidental.Flat), null, Create(8, NoteName.G, Accidental.Sharp), null },
      { Create(9, NoteName.B, Accidental.DoubleFlat), null, Create(9, NoteName.A, Accidental.Natural), null, Create(9, NoteName.G, Accidental.DoubleSharp) },
      { Create(10, NoteName.C, Accidental.DoubleFlat), Create(10, NoteName.B, Accidental.Flat), null, Create(10, NoteName.A, Accidental.Sharp), null },
      { null, Create(11, NoteName.C, Accidental.Flat), Create(11, NoteName.B, Accidental.Natural), null, Create(11, NoteName.A, Accidental.DoubleSharp) }
    };

    private static readonly int[] s_noteNameIndices =
    {
      0, // NoteName.C
      2, // NoteName.D
      4, // NoteName.E
      5, // NoteName.F
      7, // NoteName.G
      9, // NoteName.A
      11 // NoteName.B
    };

    /// <summary>C note.</summary>
    public static readonly Note C;

    /// <summary>C♯ note.</summary>
    public static readonly Note CSharp;

    /// <summary>D♭ note.</summary>
    public static readonly Note DFlat;

    /// <summary>D note.</summary>
    public static readonly Note D;

    /// <summary>D♯ note.</summary>
    public static readonly Note DSharp;

    /// <summary>E♭ note.</summary>
    public static readonly Note EFlat;

    /// <summary>E note.</summary>
    public static readonly Note E;

    /// <summary>F note.</summary>
    public static readonly Note F;

    /// <summary>F♯ note.</summary>
    public static readonly Note FSharp;

    /// <summary>G♭ note.</summary>
    public static readonly Note GFlat;

    /// <summary>G note.</summary>
    public static readonly Note G;

    /// <summary>G♯ note.</summary>
    public static readonly Note GSharp;

    /// <summary>A♭ note.</summary>
    public static readonly Note AFlat;

    /// <summary>A note.</summary>
    public static readonly Note A;

    /// <summary>A♯ note.</summary>
    public static readonly Note ASharp;

    /// <summary>B♭ note.</summary>
    public static readonly Note BFlat;

    /// <summary>B note.</summary>
    public static readonly Note B;

    #endregion

    #region Data Members

    private readonly ushort _encoded;

    #endregion

    #region Constructors

    static Note()
    {
      C = GetNote(0, Accidental.Natural);
      CSharp = GetNote(1, Accidental.Sharp);
      DFlat = GetNote(1, Accidental.Flat);
      D = GetNote(2, Accidental.Natural);
      DSharp = GetNote(3, Accidental.Sharp);
      EFlat = GetNote(3, Accidental.Flat);
      E = GetNote(4, Accidental.Natural);
      F = GetNote(5, Accidental.Natural);
      FSharp = GetNote(6, Accidental.Sharp);
      GFlat = GetNote(6, Accidental.Flat);
      G = GetNote(7, Accidental.Natural);
      GSharp = GetNote(8, Accidental.Sharp);
      AFlat = GetNote(8, Accidental.Flat);
      A = GetNote(9, Accidental.Natural);
      ASharp = GetNote(10, Accidental.Sharp);
      BFlat = GetNote(10, Accidental.Flat);
      B = GetNote(11, Accidental.Natural);
      AccidentalMode = AccidentalMode.FavorSharps;
    }

    private Note(int absoluteValue,
                 NoteName noteName,
                 Accidental accidental)
    {
      _encoded = Encode(absoluteValue, noteName, accidental);
    }

    #endregion

    #region Properties

    /// <summary>Gets or sets the accidental mode.</summary>
    /// <value>The accidental mode.</value>
    public static AccidentalMode AccidentalMode { get; set; }

    private int AbsoluteValue => DecodeAbsoluteValue(_encoded);

    /// <summary>Gets the name of the note.</summary>
    /// <value>The name of the note.</value>
    public NoteName NoteName => DecodeToneName(_encoded);

    /// <summary>Gets the accidental.</summary>
    /// <value>The accidental.</value>
    public Accidental Accidental => DecodeAccidental(_encoded);

    #endregion

    #region IComparable<Note> Members

    /// <inheritdoc />
    public int CompareTo(Note other) => AbsoluteValue - other.AbsoluteValue;

    #endregion

    #region IEquatable<Note> Members

    /// <inheritdoc />
    public bool Equals(Note other) => AbsoluteValue == other.AbsoluteValue;

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
      Contract.Requires<ArgumentOutOfRangeException>(noteName >= NoteName.C && noteName <= NoteName.B);
      Contract.Requires<ArgumentOutOfRangeException>(accidental >= Accidental.DoubleFlat && accidental <= Accidental.DoubleSharp);

      // This really doesn't create a Note but returns one of the pre-created ones
      // from the enharmonics table.
      // TODO: Maybe we should change the name of Create to Lookup?
      int accidentalIndex = (int)accidental + Math.Abs((int)Accidental.DoubleFlat);
      int noteNameIndex = s_noteNameIndices[(int)noteName];
      int noteIndex = s_enharmonics.WrapIndex(0, noteNameIndex + (int)accidental);
      Note? note = s_enharmonics[noteIndex, accidentalIndex];
      Debug.Assert(note.HasValue);

      return note.Value;
    }

    /// <summary>Gets the enharmonic note for this instance or null if non exists.</summary>
    /// <param name="noteName">The name of the enharmonic note.</param>
    /// <returns>The enharmonic.</returns>
    [Pure]
    public Note? GetEnharmonic(NoteName noteName)
    {
      int absoluteValue = AbsoluteValue;
      for( var accidental = (int)Accidental.DoubleFlat; accidental <= (int)Accidental.DoubleSharp; ++accidental )
      {
        int index = accidental + Math.Abs((int)Accidental.DoubleFlat);
        Note? note = s_enharmonics[absoluteValue, index];
        if( !note.HasValue )
        {
          continue;
        }

        if( note.Value.NoteName == noteName )
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

      var accidental = Accidental.Natural;
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
      var current = a;
      int count = 0;

      while( current != b )
      {
        ++count;
        current = current.Add(1);
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
      int absoluteValue = ArrayExtensions.WrapIndex(AbsoluteValueCount, AbsoluteValue + semitoneCount);
      return FindNote(absoluteValue, mode == AccidentalMode.FavorSharps);
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
      int absoluteValue = ArrayExtensions.WrapIndex(AbsoluteValueCount, AbsoluteValue - semitoneCount);
      return FindNote(absoluteValue, mode == AccidentalMode.FavorSharps);
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

      var semitoneCount = SemitonesBetween(a, b);
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
    public override int GetHashCode() => AbsoluteValue;

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

    private static Note GetNote(int absoluteValue,
                                Accidental accidental)
    {
      int index = (int)accidental + Math.Abs((int)Accidental.DoubleFlat);
      Note? note = s_enharmonics[absoluteValue, index];
      Debug.Assert(note.HasValue);

      return note.Value;
    }

    // This is a helper function for creating notes for the enharmonic table
    private static Note Create(int absoluteValue,
                               NoteName noteName,
                               Accidental accidental)
      => new Note(absoluteValue, noteName, accidental);

    // Finds a note that corresponds to the provided absolute value,
    // attempting to match the desired accidental mode
    internal static Note FindNote(int absoluteValue,
                                  bool favorSharps)
    {
      int index = (int)Accidental.Natural + Math.Abs((int)Accidental.DoubleFlat);
      if( favorSharps )
      {
        int max = (int)Accidental.DoubleSharp + Math.Abs((int)Accidental.DoubleFlat);
        while( index <= max )
        {
          Note? current = s_enharmonics[absoluteValue, index];
          if( current.HasValue )
          {
            return current.Value;
          }

          ++index;
        }
      }
      else
      {
        while( index >= 0 )
        {
          Note? current = s_enharmonics[absoluteValue, index];
          if( current.HasValue )
          {
            return current.Value;
          }

          --index;
        }
      }

      throw new Exception("Must be able to find enharmonic");
    }

    private static ushort Encode(int value,
                                 NoteName noteName,
                                 Accidental accidental)
    {
      Contract.Requires<ArgumentOutOfRangeException>(value >= 0 && value <= 11);
      Contract.Requires<ArgumentOutOfRangeException>(noteName >= NoteName.C && noteName <= NoteName.B);
      Contract.Requires<ArgumentOutOfRangeException>(accidental >= Accidental.DoubleFlat && accidental <= Accidental.DoubleSharp);

      var encoded = (ushort)( ( ( (ushort)noteName & ToneNameMask ) << ToneNameShift )
                              | ( ( (ushort)( (int)accidental + 2 ) & AccidentalMask ) << AccidentalShift )
                              | ( (ushort)value & AbsoluteValueMask ) );
      return encoded;
    }

    private static int DecodeAbsoluteValue(ushort encoded)
    {
      int result = encoded & AbsoluteValueMask;
      return result;
    }

    private static NoteName DecodeToneName(ushort encoded)
    {
      int result = ( encoded >> ToneNameShift ) & ToneNameMask;
      return (NoteName)result;
    }

    private static Accidental DecodeAccidental(ushort encoded)
    {
      int result = ( ( encoded >> AccidentalShift ) & AccidentalMask ) - 2;
      return (Accidental)result;
    }

    #endregion

    #region Operators

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
