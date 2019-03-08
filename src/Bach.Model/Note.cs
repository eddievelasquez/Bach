//
// Module Name: Note.cs
// Project:     Bach.Model
// Copyright (c) 2016  Eddie Velasquez.
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
//  portions of the Software.
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

  /// <summary>
  /// A Note represents a combination of a <see cref="P:Bach.Model.Note.NoteName" />
  /// and an optional <see cref="P:Bach.Model.Note.Accidental" /> following the English naming
  /// convention for the 12 note chromatic scale.
  /// </summary>
  public struct Note
    : IEquatable<Note>,
      IComparable<Note>
  {
    private const ushort ToneNameMask = 7;
    private const ushort ToneNameShift = 7;
    private const ushort AccidentalMask = 7;
    private const ushort AccidentalShift = 4;
    private const ushort AbsoluteValueMask = 0x0F;
    private const int AbsoluteValueCount = 12;

    private static readonly Link[] s_links;

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

    private readonly ushort _encoded;

    static Note()
    {
      s_links = new Link[AbsoluteValueCount];

      C = Create(NoteName.C);
      CSharp = Create(NoteName.C, Accidental.Sharp);
      DFlat = Create(NoteName.D, Accidental.Flat);
      D = Create(NoteName.D);
      DSharp = Create(NoteName.D, Accidental.Sharp);
      EFlat = Create(NoteName.E, Accidental.Flat);
      E = Create(NoteName.E);
      F = Create(NoteName.F);
      FSharp = Create(NoteName.F, Accidental.Sharp);
      GFlat = Create(NoteName.G, Accidental.Flat);
      G = Create(NoteName.G);
      GSharp = Create(NoteName.G, Accidental.Sharp);
      AFlat = Create(NoteName.A, Accidental.Flat);
      A = Create(NoteName.A);
      ASharp = Create(NoteName.A, Accidental.Sharp);
      BFlat = Create(NoteName.B, Accidental.Flat);
      B = Create(NoteName.B);

      AccidentalMode = AccidentalMode.FavorSharps;
    }

    /// <summary>Constructor.</summary>
    /// <param name="noteName">Name of the note.</param>
    /// <param name="accidental">(Optional) The accidental.</param>
    public Note(NoteName noteName,
                Accidental accidental = Accidental.Natural)
    {
      int absoluteValue = CalcAbsoluteValue(noteName, accidental);
      _encoded = Encode(absoluteValue, noteName, accidental);
    }

    private static Note Create(NoteName noteName,
                               Accidental accidental = Accidental.Natural)
    {
      var tone = new Note(noteName, accidental);
      Link link = s_links[tone.AbsoluteValue];
      if( link == null )
      {
        link = new Link();
        s_links[tone.AbsoluteValue] = link;
      }

      switch( accidental )
      {
        case Accidental.Flat:
          link.Flat = tone;
          break;

        case Accidental.Natural:
          link.Natural = tone;
          break;

        case Accidental.Sharp:
          link.Sharp = tone;
          break;
      }

      return tone;
    }

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

    /// <inheritdoc/>
    public int CompareTo(Note other) => AbsoluteValue - other.AbsoluteValue;

    /// <inheritdoc/>
    public bool Equals(Note other) => AbsoluteValue == other.AbsoluteValue;

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

      if( !Enum.TryParse(value.Substring(0, 1), true, out NoteName toneName) )
      {
        note = C;
        return false;
      }

      var accidental = Accidental.Natural;
      if( value.Length > 1 && !AccidentalExtensions.TryParse(value.Substring(1), out accidental) )
      {
        note = C;
        return false;
      }

      note = new Note(toneName, accidental);
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
    /// Calculate the number of semitones between two notes
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
      int absoluteValue = ( AbsoluteValue + semitoneCount ) % AbsoluteValueCount;
      return GetNote(absoluteValue, mode == AccidentalMode.FavorSharps);
    }

    public static Note Add(Note note,
                           Interval interval)
    {
      NoteName noteName = note.NoteName.Add((int)interval.Quantity);
      int semitoneCount = interval.SemitoneCount;
      Note calculatedNote = note.Add(semitoneCount);
      if( calculatedNote.NoteName == noteName )
      {
        return calculatedNote;
      }

      //if (calculatedNote.NoteName > noteName)
      //{
      //  // Must be sharpen
      //  return new Note(noteName, Accidental.Sharp);
      //}

      //// Next note flattened
      //return new Note(noteName, Accidental.Flat);

      // Deal with enharmonics
      Link link = s_links[calculatedNote.AbsoluteValue];
      Note? newNote = link.GetNote(noteName);
      return newNote ?? calculatedNote;
    }

    /// <summary>Subtracts a number of semitones from the current instance.</summary>
    /// <param name="semitoneCount">Number of semitones.</param>
    /// <param name="mode">(Optional) The accidental mode.</param>
    /// <returns>A Note.</returns>
    public Note Subtract(int semitoneCount,
                         AccidentalMode mode = AccidentalMode.FavorSharps)
    {
      semitoneCount %= AbsoluteValueCount;
      int absoluteValue = AbsoluteValue - semitoneCount;
      if( absoluteValue < 0 )
      {
        absoluteValue = AbsoluteValueCount - semitoneCount;
      }

      return GetNote(absoluteValue, mode == AccidentalMode.FavorSharps);
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
        current = current.Next();
      }

      var semitoneCount = SemitonesBetween(a, b);
      var interval = new Interval(quantity, semitoneCount);
      return interval;
    }

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
      => Add(note, interval);

    /// <summary>Subtraction operator.</summary>
    /// <param name="a">The first value.</param>
    /// <param name="b">A value to subtract from it.</param>
    /// <returns>The result of the operation.</returns>
    public static Interval operator-(Note a,
                                     Note b)
      => Subtract(a, b);

    private static int CalcAbsoluteValue(NoteName noteName,
                                         Accidental accidental)
    {
      int absoluteValue = ( NoteName.C.SemitonesBetween(noteName) + (int)accidental ) % AbsoluteValueCount;
      if( absoluteValue < 0 )
      {
        absoluteValue = AbsoluteValueCount + absoluteValue;
      }

      return absoluteValue;
    }

    internal static Note GetNote(int index,
                                 bool favorSharps)
    {
      Link link = s_links[index];

      if( link.Natural != null )
      {
        return link.Natural.Value;
      }

      Debug.Assert(link.Sharp.HasValue);
      Debug.Assert(link.Flat.HasValue);

      return favorSharps ? link.Sharp.Value : link.Flat.Value;
    }

    private static ushort Encode(int value,
                                 NoteName noteName,
                                 Accidental accidental)
    {
      Contract.Requires<ArgumentOutOfRangeException>(value >= 0 && value <= 11);
      Contract.Requires<ArgumentOutOfRangeException>(noteName >= NoteName.C && noteName <= NoteName.B);
      Contract.Requires<ArgumentOutOfRangeException>(accidental >= Accidental.DoubleFlat && accidental <= Accidental.DoubleSharp);
      var encoded = (ushort)( ( ( (ushort)noteName & ToneNameMask ) << ToneNameShift )
                              | ( ( (ushort)( accidental + 2 ) & AccidentalMask ) << AccidentalShift )
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

    private class Link
    {
      public Note? Natural { get; set; }
      public Note? Sharp { get; set; }
      public Note? Flat { get; set; }

      public Note? GetNote(NoteName noteName)
      {
        var note = GetNote(Flat, noteName);
        if (note.HasValue)
        {
          return note;
        }

        note = GetNote(Natural, noteName);
        return note ?? GetNote(Sharp, noteName);
      }

      private Note? GetNote(Note? note,
                            NoteName noteName)
      {
        if( note.HasValue && note.Value.NoteName == noteName )
        {
          return note;
        }

        return null;
      }
    }
  }
}
