//
// Module Name: Pitch.cs
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
  using System.Text;

  /// <summary>
  ///   A Pitch represents the pitch of a sound (<see cref="Note" />)
  ///   on a given octave.
  /// </summary>
  /// <remarks>
  ///   The octave of a Pitch ranges from 0 to 9,
  ///   which corresponds to MIDI pitches from 12 (C0)
  ///   to 127 (B9).
  /// </remarks>
  public struct Pitch
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

    // Midi supports C-1, but we only support C0 and above
    private static readonly byte s_minAbsoluteValue = (byte)CalcAbsoluteValue(NoteName.C, Accidental.Natural, MinOctave);

    // G9 is the highest pitch supported by MIDI
    private static readonly byte s_maxAbsoluteValue = (byte)CalcAbsoluteValue(NoteName.G, Accidental.Natural, MaxOctave);

    private static readonly Pitch s_a4 = Create(NoteName.A, Accidental.Natural, 4);

    /// <summary>An empty pitch.</summary>
    public static readonly Pitch Empty = new Pitch();

    /// <summary>The maximum possible pitch value.</summary>
    //TODO: Why have this if G9 is the maximum supported pitch?
    public static readonly Pitch MaxValue = new Pitch(Note.B, MaxOctave, 128);

    #endregion

    #region Data Members

    private readonly byte _absoluteValue;
    private readonly Note _note;
    private readonly byte _octave;

    #endregion

    #region Constructors

    private Pitch(int absoluteValue,
                  AccidentalMode accidentalMode)
    {
      Contract.Requires<ArgumentOutOfRangeException>(absoluteValue >= 0);
      Contract.Requires<ArgumentOutOfRangeException>(absoluteValue < 128);

      _absoluteValue = (byte)absoluteValue;
      CalcNote(_absoluteValue, out _note, out _octave, accidentalMode);
    }

    private Pitch(Note note,
                  int octave,
                  int absoluteValue)
    {
      _note = note;
      _octave = (byte)octave;
      _absoluteValue = (byte)absoluteValue;
    }

    #endregion

    #region Properties

    /// <summary>Gets a value indicating whether this instance is a valid pitch.</summary>
    /// <value>True if this instance is a valid false, false if it is not.</value>
    public bool IsValid
    {
      get
      {
        int abs = _absoluteValue + (int)_note.Accidental;
        return abs >= s_minAbsoluteValue && abs <= s_maxAbsoluteValue;
      }
    }

    /// <summary>Gets the pitch's note.</summary>
    /// <value>The note.</value>
    public Note Note => _note;

    /// <summary>Gets the pitch's octave.</summary>
    /// <value>The octave.</value>
    public int Octave => _octave;

    /// <summary>Gets the pitch's frequency.</summary>
    /// <value>The frequency.</value>
    public double Frequency
    {
      get
      {
        int interval = _absoluteValue - s_a4._absoluteValue;
        double freq = Math.Pow(2, interval / 12.0) * A4Frequency;
        return freq;
      }
    }

    /// <summary>Gets the pitch's MIDI value.</summary>
    /// <value>The MIDI value.</value>
    public int Midi => _absoluteValue + 12;

    public static AccidentalMode AccidentalMode
    {
      get => Note.AccidentalMode;
      set => Note.AccidentalMode = value;
    }

    #endregion

    #region IComparable<Pitch> Members

    /// <inheritdoc />
    public int CompareTo(Pitch other) => _absoluteValue - other._absoluteValue;

    #endregion

    #region IEquatable<Pitch> Members

    /// <inheritdoc />
    public bool Equals(Pitch obj) => obj._absoluteValue == _absoluteValue;

    #endregion

    #region Public Methods

    /// <summary>Creates a new Pitch.</summary>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when created pitch would be out of the supported range C0..G9.</exception>
    /// <param name="note">The note.</param>
    /// <param name="octave">The octave.</param>
    /// <returns>A Pitch.</returns>
    public static Pitch Create(Note note,
                               int octave)
    {
      Contract.Requires<ArgumentOutOfRangeException>(octave >= MinOctave);
      Contract.Requires<ArgumentOutOfRangeException>(octave <= MaxOctave);

      int abs = CalcAbsoluteValue(note.NoteName, note.Accidental, octave);
      if( abs < s_minAbsoluteValue )
      {
        throw new ArgumentOutOfRangeException($"Must be equal to or greater than {new Pitch(s_minAbsoluteValue, AccidentalMode)}");
      }

      if( abs > s_maxAbsoluteValue )
      {
        throw new ArgumentOutOfRangeException($"Must be equal to or less than {new Pitch(s_maxAbsoluteValue, AccidentalMode)}");
      }

      return new Pitch(note, octave, abs);
    }

    /// <summary>Creates a new Pitch.</summary>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when created pitch would be out of the supported range C0..G9.</exception>
    /// <param name="noteName">Name of the note.</param>
    /// <param name="accidental">The accidental.</param>
    /// <param name="octave">The octave.</param>
    /// <returns>A Pitch.</returns>
    public static Pitch Create(NoteName noteName,
                               Accidental accidental,
                               int octave)
      => Create(Note.Create(noteName, accidental), octave);

    /// <summary>Creates a pitch from a MIDI pitch value.</summary>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when created pitch would be out of the supported range C0..G9.</exception>
    /// <param name="midi">The MIDI pitch.</param>
    /// <returns>A pitch.</returns>
    public static Pitch CreateFromMidi(int midi)
    {
      Contract.Requires<ArgumentOutOfRangeException>(midi >= 0);
      Contract.Requires<ArgumentOutOfRangeException>(midi <= 127);

      int absoluteValue = midi - 12;
      if( absoluteValue < 0 )
      {
        throw new ArgumentOutOfRangeException(nameof(midi), "midi is out of range");
      }

      var note = new Pitch(absoluteValue, AccidentalMode);
      return note;
    }

    /// <summary>Adds number of semitones to the current instance.</summary>
    /// <param name="semitoneCount">Number of semitones.</param>
    /// <param name="accidentalMode">(Optional) The accidental mode.</param>
    /// <returns>A Pitch.</returns>
    public Pitch Add(int semitoneCount,
                     AccidentalMode accidentalMode = AccidentalMode.FavorSharps)
    {
      var result = new Pitch(_absoluteValue + semitoneCount, accidentalMode);
      return result;
    }

    /// <summary>Adds an Interval to a given Pitch.</summary>
    /// <param name="pitch">The pitch.</param>
    /// <param name="interval">A interval to add to it.</param>
    /// <returns>A Pitch.</returns>
    public static Pitch Add(Pitch pitch,
                            Interval interval)
    {
      var absoluteValue = (byte)( pitch._absoluteValue + interval.SemitoneCount );
      CalcNote(absoluteValue, out Note _, out byte octave, AccidentalMode.FavorSharps);

      Note newNote = pitch.Note + interval;
      var result = new Pitch(newNote, octave, absoluteValue);
      return result;
    }

    /// <summary>Subtracts a number of semitones to the current instance.</summary>
    /// <param name="semitoneCount">Number of semitones.</param>
    /// <param name="accidentalMode">(Optional) The accidental mode.</param>
    /// <returns>A Pitch.</returns>
    public Pitch Subtract(int semitoneCount,
                          AccidentalMode accidentalMode = AccidentalMode.FavorSharps)
    {
      var result = new Pitch(_absoluteValue - semitoneCount, accidentalMode);
      return result;
    }

    /// <summary>Determines the minimum of the given pitches.</summary>
    /// <param name="a">A Pitch to process.</param>
    /// <param name="b">A Pitch to process.</param>
    /// <returns>The minimum value.</returns>
    public static Pitch Min(Pitch a,
                            Pitch b)
      => a._absoluteValue <= b._absoluteValue ? a : b;

    /// <summary>Determines the maximum of the given pitches.</summary>
    /// <param name="a">A Pitch to process.</param>
    /// <param name="b">A Pitch to process.</param>
    /// <returns>The maximum value.</returns>
    public static Pitch Max(Pitch a,
                            Pitch b)
      => a._absoluteValue >= b._absoluteValue ? a : b;

    #endregion

    #region Overrides

    /// <inheritdoc />
    public override bool Equals(object obj)
    {
      if( obj is null || obj.GetType() != GetType() )
      {
        return false;
      }

      return Equals((Pitch)obj);
    }

    /// <inheritdoc />
    public override int GetHashCode() => _absoluteValue;

    /// <inheritdoc />
    public override string ToString() => $"{_note}{Octave}";

    #endregion

    #region  Implementation

    private static int CalcAbsoluteValue(NoteName noteName,
                                         Accidental accidental,
                                         int octave)
    {
      int absoluteValue = ( octave * IntervalsPerOctave ) + NoteName.C.SemitonesBetween(noteName) + (int)accidental;
      return absoluteValue;
    }

    private static void CalcNote(byte absoluteValue,
                                 out Note note,
                                 out byte octave,
                                 AccidentalMode accidentalMode)
    {
      octave = (byte)Math.DivRem(absoluteValue, IntervalsPerOctave, out int remainder);
      note = Note.FindNote(remainder, accidentalMode == AccidentalMode.FavorSharps);
    }

    #endregion

    #region Operators

    /// <summary>Equality operator.</summary>
    /// <param name="lhs">The first instance to compare.</param>
    /// <param name="rhs">The second instance to compare.</param>
    /// <returns>The result of the operation.</returns>
    public static bool operator==(Pitch lhs,
                                  Pitch rhs)
      => Equals(lhs, rhs);

    /// <summary>Inequality operator.</summary>
    /// <param name="lhs">The first instance to compare.</param>
    /// <param name="rhs">The second instance to compare.</param>
    /// <returns>The result of the operation.</returns>
    public static bool operator!=(Pitch lhs,
                                  Pitch rhs)
      => !Equals(lhs, rhs);

    /// <summary>Greater-than comparison operator.</summary>
    /// <param name="left">The first instance to compare.</param>
    /// <param name="right">The second instance to compare.</param>
    /// <returns>The result of the operation.</returns>
    public static bool operator>(Pitch left,
                                 Pitch right)
      => left.CompareTo(right) > 0;

    /// <summary>Lesser-than comparison operator.</summary>
    /// <param name="left">The first instance to compare.</param>
    /// <param name="right">The second instance to compare.</param>
    /// <returns>The result of the operation.</returns>
    public static bool operator<(Pitch left,
                                 Pitch right)
      => left.CompareTo(right) < 0;

    /// <summary>Greater-than-or-equal comparison operator.</summary>
    /// <param name="left">The first instance to compare.</param>
    /// <param name="right">The second instance to compare.</param>
    /// <returns>The result of the operation.</returns>
    public static bool operator>=(Pitch left,
                                  Pitch right)
      => left.CompareTo(right) >= 0;

    /// <summary>Lesser-than-or-equal comparison operator.</summary>
    /// <param name="left">The first instance to compare.</param>
    /// <param name="right">The second instance to compare.</param>
    /// <returns>The result of the operation.</returns>
    public static bool operator<=(Pitch left,
                                  Pitch right)
      => left.CompareTo(right) <= 0;

    /// <summary>Addition operator.</summary>
    /// <param name="pitch">The first value.</param>
    /// <param name="semitoneCount">A value to add to it.</param>
    /// <returns>The result of the operation.</returns>
    public static Pitch operator+(Pitch pitch,
                                  int semitoneCount)
      => pitch.Add(semitoneCount, AccidentalMode);

    /// <summary>Addition operator.</summary>
    /// <param name="pitch">The first value.</param>
    /// <param name="interval">A value to add to it.</param>
    /// <returns>The result of the operation.</returns>
    public static Pitch operator+(Pitch pitch,
                                  Interval interval)
      => Add(pitch, interval);

    /// <summary>Increment operator.</summary>
    /// <param name="pitch">The pitch.</param>
    /// <returns>The result of the operation.</returns>
    public static Pitch operator++(Pitch pitch) => pitch.Add(1, AccidentalMode);

    /// <summary>Subtraction operator.</summary>
    /// <param name="pitch">The first value.</param>
    /// <param name="semitoneCount">A value to subtract from it.</param>
    /// <returns>The result of the operation.</returns>
    public static Pitch operator-(Pitch pitch,
                                  int semitoneCount)
      => pitch.Subtract(semitoneCount, AccidentalMode);

    /// <summary>Decrement operator.</summary>
    /// <param name="pitch">The pitch.</param>
    /// <returns>The result of the operation.</returns>
    public static Pitch operator--(Pitch pitch) => pitch.Subtract(1, AccidentalMode);

    /// <summary>Subtraction operator.</summary>
    /// <param name="left">The first value.</param>
    /// <param name="right">A value to subtract from it.</param>
    /// <returns>The result of the operation.</returns>
    public static int operator-(Pitch left,
                                Pitch right)
      => left._absoluteValue - right._absoluteValue;

    #endregion

    #region Parsing

    private static void TryGetAccidental(string value,
                                         ref int index,
                                         out Accidental accidental)
    {
      accidental = Accidental.Natural;

      var buf = new StringBuilder();
      for( int i = index; i < value.Length; ++i )
      {
        char ch = value[i];
        if( ch != '#' && ch != 'b' && ch != 'B' )
        {
          if( buf.Length > 0 )
          {
            break;
          }

          return;
        }

        buf.Append(ch);
      }

      if( buf.Length == 0 )
      {
        return;
      }

      if( Accidental.TryParse(buf.ToString(), out accidental) )
      {
        index += buf.Length;
      }
    }

    private static void TryGetOctave(string value,
                                     ref int index,
                                     out int octave)
    {
      if( index >= value.Length || !int.TryParse(value.Substring(index, 1), out octave) )
      {
        octave = -1;
        return;
      }

      if( octave >= MinOctave && octave <= MaxOctave )
      {
        ++index;
      }
    }

    private static bool TryParseNotes(string value,
                                      ref Pitch pitch)
    {
      if( !NoteName.TryParse(value, out NoteName toneName) )
      {
        return false;
      }

      var index = 1;
      TryGetAccidental(value, ref index, out Accidental accidental);
      TryGetOctave(value, ref index, out int octave);

      if( index < value.Length || octave < MinOctave || octave > MaxOctave )
      {
        return false;
      }

      pitch = Create(toneName, accidental, octave);
      return true;
    }

    private static bool TryParseMidi(string value,
                                     ref Pitch pitch)
    {
      if( !int.TryParse(value, out int midi) )
      {
        return false;
      }

      if( midi < 0 || midi > 127 )
      {
        return false;
      }

      pitch = CreateFromMidi(midi);
      return true;
    }

    /// <summary>Attempts to parse a Pitch from the given string.</summary>
    /// <param name="value">The value to parse.</param>
    /// <param name="pitch">[out] The note.</param>
    /// <returns>True if it succeeds, false if it fails.</returns>
    public static bool TryParse(string value,
                                out Pitch pitch)
    {
      pitch = Empty;
      if( string.IsNullOrEmpty(value) )
      {
        return false;
      }

      return char.IsDigit(value, 0) ? TryParseMidi(value, ref pitch) : TryParseNotes(value, ref pitch);
    }

    /// <summary>Parses the provided string.</summary>
    /// <exception cref="FormatException">Thrown when the provided string doesn't represent a Pitch.</exception>
    /// <exception cref="ArgumentNullException">Thrown when a null string is provided.</exception>
    /// <exception cref="ArgumentException">Thrown when an empty string is provided.</exception>
    /// <param name="value">The value to parse.</param>
    /// <returns>A Pitch.</returns>
    public static Pitch Parse(string value)
    {
      if( !TryParse(value, out Pitch result) )
      {
        throw new FormatException($"{value} is not a valid pitch");
      }

      return result;
    }

    #endregion
  }
}
