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
  /// A Pitch represents the pitch of a sound (<see cref="Note"/>)
  /// on a given octave.
  /// </summary>
  /// <remarks>
  /// The octave of a Pitch ranges from 0 to 9,
  /// which corresponds to MIDI pitches from 12 (C0)
  /// to 127 (B9).
  /// </remarks>
  public struct Pitch
    : IEquatable<Pitch>,
      IComparable<Pitch>
  {
    public const int MinOctave = 0;
    public const int MaxOctave = 9;
    internal const double A4Frequency = 440.0;
    internal const int IntervalsPerOctave = 12;

    private static readonly int[] s_intervals =
    {
      0, // C
      2, // D
      4, // E
      5, // F
      7, // G
      9, // A
      11, // B
      12 // C
    };

    // Midi supports C-1, but we only support C0 and above
    private static readonly byte s_minAbsoluteValue = (byte) CalcAbsoluteValue(NoteName.C, Accidental.Natural, MinOctave);

    // G9 is the highest pitch supported by MIDI
    private static readonly byte s_maxAbsoluteValue = (byte) CalcAbsoluteValue(NoteName.G, Accidental.Natural, MaxOctave);

    private static readonly Pitch s_a4 = Create(NoteName.A, Accidental.Natural, 4);

    public static readonly Pitch Empty = new Pitch();
    public static readonly Pitch MaxValue = new Pitch(Note.B, MaxOctave, 128);

    private readonly byte _absoluteValue;
    private readonly byte _octave;
    private readonly Note _note;

    private Pitch(int absoluteValue,
                  AccidentalMode accidentalMode)
    {
      Contract.Requires<ArgumentOutOfRangeException>(absoluteValue >= 0);
      Contract.Requires<ArgumentOutOfRangeException>(absoluteValue < 128);

      _absoluteValue = (byte) absoluteValue;
      CalcNote(_absoluteValue, out _note, out _octave, accidentalMode);
    }

    private Pitch(Note note,
                  int octave,
                  int absoluteValue)
    {
      _note = note;
      _octave = (byte) octave;
      _absoluteValue = (byte) absoluteValue;
    }

    private Pitch(NoteName noteName,
                  Accidental accidental,
                  int octave,
                  int absoluteValue)
    {
      Contract.Requires<ArgumentOutOfRangeException>(absoluteValue >= 0);
      Contract.Requires<ArgumentOutOfRangeException>(absoluteValue < 128);

      _note = new Note(noteName, accidental);
      _octave = (byte) octave;
      _absoluteValue = (byte) absoluteValue;
    }

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

    public static Pitch Create(NoteName noteName,
                               Accidental accidental,
                               int octave)
    {
      Contract.Requires<ArgumentOutOfRangeException>(noteName >= NoteName.C);
      Contract.Requires<ArgumentOutOfRangeException>(noteName <= NoteName.B);
      Contract.Requires<ArgumentOutOfRangeException>(accidental >= Accidental.DoubleFlat);
      Contract.Requires<ArgumentOutOfRangeException>(accidental <= Accidental.DoubleSharp);
      Contract.Requires<ArgumentOutOfRangeException>(octave >= MinOctave);
      Contract.Requires<ArgumentOutOfRangeException>(octave <= MaxOctave);

      int abs = CalcAbsoluteValue(noteName, accidental, octave);
      if( abs < s_minAbsoluteValue )
      {
        throw new ArgumentOutOfRangeException($"Must be equal to or greater than {new Pitch(s_minAbsoluteValue, AccidentalMode)}");
      }

      if( abs > s_maxAbsoluteValue )
      {
        throw new ArgumentOutOfRangeException($"Must be equal to or less than {new Pitch(s_maxAbsoluteValue, AccidentalMode)}");
      }

      return new Pitch(noteName, accidental, octave, abs);
    }

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

    public bool IsValid
    {
      get
      {
        int abs = _absoluteValue + (int) _note.Accidental;
        return abs >= s_minAbsoluteValue && abs <= s_maxAbsoluteValue;
      }
    }

    public Note Note => _note;

    public NoteName NoteName => _note.NoteName;

    public Accidental Accidental => _note.Accidental;

    public int Octave => _octave;

    public int AbsoluteValue => _absoluteValue;

    public double Frequency
    {
      get
      {
        int interval = AbsoluteValue - s_a4.AbsoluteValue;
        double freq = Math.Pow(2, interval / 12.0) * A4Frequency;
        return freq;
      }
    }

    // The formula to convert a pitch to MIDI (according to http://en.wikipedia.org/wiki/Pitch)
    // is p = 69 + 12 x log2(freq / 440). As it happens, our AbsoluteValue almost
    // matches, but we don't support the -1 octave we must add 12 to obtain the
    // MIDI number.
    public int Midi => AbsoluteValue + 12;

    public static AccidentalMode AccidentalMode
    {
      get => Note.AccidentalMode;
      set => Note.AccidentalMode = value;
    }

    public int CompareTo(Pitch other) => AbsoluteValue - other.AbsoluteValue;

    public bool Equals(Pitch obj) => obj.AbsoluteValue == AbsoluteValue;

    public override bool Equals(object obj)
    {
      if( ReferenceEquals(obj, null) || obj.GetType() != GetType() )
      {
        return false;
      }

      return Equals((Pitch) obj);
    }

    public override int GetHashCode() => AbsoluteValue;

    public override string ToString() => $"{_note}{Octave}";

    private static bool TryParseNotes(string value,
                                      ref Pitch pitch,
                                      int defaultOctave)
    {
      if( !Enum.TryParse(value.Substring(0, 1), true, out NoteName toneName) )
      {
        return false;
      }

      var index = 1;
      int octave = defaultOctave;

      TryGetAccidental(value, ref index, out Accidental accidental);
      TryGetOctave(value, ref index, ref octave);
      if( index < value.Length )
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

    private static int CalcAbsoluteValue(NoteName noteName,
                                         Accidental accidental,
                                         int octave)
    {
      int value = ( octave * IntervalsPerOctave ) + s_intervals[(int) noteName] + (int) accidental;
      return value;
    }

    private static void CalcNote(byte absoluteValue,
                                 out Note note,
                                 out byte octave,
                                 AccidentalMode accidentalMode)
    {
      octave = (byte) Math.DivRem(absoluteValue, IntervalsPerOctave, out int remainder);
      note = Note.GetNote(remainder, accidentalMode == AccidentalMode.FavorSharps);
    }

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

      if( AccidentalExtensions.TryParse(buf.ToString(), out accidental) )
      {
        index += buf.Length;
      }
    }

    private static void TryGetOctave(string value,
                                     ref int index,
                                     ref int octave)
    {
      if( index >= value.Length || !int.TryParse(value.Substring(index, 1), out octave) )
      {
        return;
      }

      if( octave >= MinOctave && octave <= MaxOctave )
      {
        ++index;
      }
    }

    public Pitch ApplyAccidental(Accidental accidental)
    {
      CalcNote(
        (byte) ( AbsoluteValue + accidental ),
        out Note tone,
        out byte octave,
        accidental < Accidental.Natural ? AccidentalMode.FavorFlats : AccidentalMode.FavorSharps);

      return Create(tone, octave);
    }

    public static bool operator==(Pitch lhs,
                                  Pitch rhs) =>
      Equals(lhs, rhs);

    public static bool operator!=(Pitch lhs,
                                  Pitch rhs) =>
      !Equals(lhs, rhs);

    public static bool operator>(Pitch left,
                                 Pitch right) =>
      left.CompareTo(right) > 0;

    public static bool operator<(Pitch left,
                                 Pitch right) =>
      left.CompareTo(right) < 0;

    public static bool operator>=(Pitch left,
                                  Pitch right) =>
      left.CompareTo(right) >= 0;

    public static bool operator<=(Pitch left,
                                  Pitch right) =>
      left.CompareTo(right) <= 0;

    public Pitch Add(int interval,
                     AccidentalMode accidentalMode = AccidentalMode.FavorSharps)
    {
      var result = new Pitch(AbsoluteValue + interval, accidentalMode);
      return result;
    }

    public Pitch Subtract(int interval,
                          AccidentalMode accidentalMode = AccidentalMode.FavorSharps)
    {
      var result = new Pitch(AbsoluteValue - interval, accidentalMode);
      return result;
    }

    public static Pitch operator+(Pitch pitch,
                                  int interval) =>
      pitch.Add(interval, AccidentalMode);

    public static Pitch operator++(Pitch pitch) => pitch.Add(1, AccidentalMode);

    public static Pitch operator-(Pitch pitch,
                                  int interval) =>
      pitch.Subtract(interval, AccidentalMode);

    public static Pitch operator--(Pitch pitch) => pitch.Subtract(1, AccidentalMode);

    public static int operator-(Pitch left,
                                Pitch right) =>
      left.AbsoluteValue - right.AbsoluteValue;

    public static bool TryParse(string value,
                                out Pitch pitch,
                                int defaultOctave = 4)
    {
      pitch = new Pitch();
      if( string.IsNullOrEmpty(value) )
      {
        return false;
      }

      if( char.IsDigit(value, 0) )
      {
        return TryParseMidi(value, ref pitch);
      }

      return TryParseNotes(value, ref pitch, defaultOctave);
    }

    public static Pitch Parse(string value,
                              int defaultOctave = 4)
    {
      if( !TryParse(value, out Pitch result) )
      {
        throw new FormatException($"{value} is not a valid pitch");
      }

      return result;
    }

    public static Pitch Min(Pitch a,
                            Pitch b) =>
      a.AbsoluteValue < b.AbsoluteValue ? a : b;
  }
}
