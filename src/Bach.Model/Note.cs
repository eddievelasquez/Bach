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
// PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE
// OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

namespace Bach.Model
{
  using System;
  using System.Diagnostics.Contracts;
  using System.Text;

  public struct Note: IEquatable<Note>,
                      IComparable<Note>
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
    private static readonly byte s_minAbsoluteValue = (byte) CalcAbsoluteValue(ToneName.C, Accidental.Natural, MinOctave);

    // G9 is the highest note supported by MIDI
    private static readonly byte s_maxAbsoluteValue = (byte) CalcAbsoluteValue(ToneName.G, Accidental.Natural, MaxOctave);

    private static readonly Note s_a4 = Create(ToneName.A, Accidental.Natural, 4);

    public static readonly Note Empty = new Note();
    public static readonly Note MaxValue = new Note(Tone.B, MaxOctave, 128);

    #region Data Members

    private readonly byte _absoluteValue;
    private readonly byte _octave;
    private readonly Tone _tone;

    #endregion

    #region Construction

    private Note(int absoluteValue, AccidentalMode accidentalMode)
    {
      Contract.Requires<ArgumentOutOfRangeException>(absoluteValue >= 0);
      Contract.Requires<ArgumentOutOfRangeException>(absoluteValue < 128);

      _absoluteValue = (byte) absoluteValue;
      CalcNote(_absoluteValue, out _tone, out _octave, accidentalMode);
    }

    private Note(Tone tone, int octave, int absoluteValue)
    {
      _tone = tone;
      _octave = (byte) octave;
      _absoluteValue = (byte) absoluteValue;
    }

    private Note(ToneName toneName, Accidental accidental, int octave, int absoluteValue)
    {
      Contract.Requires<ArgumentOutOfRangeException>(absoluteValue >= 0);
      Contract.Requires<ArgumentOutOfRangeException>(absoluteValue < 128);

      _tone = new Tone(toneName, accidental);
      _octave = (byte) octave;
      _absoluteValue = (byte) absoluteValue;
    }

    #endregion

    #region Factories

    public static Note Create(Tone tone, int octave)
    {
      Contract.Requires<ArgumentOutOfRangeException>(octave >= MinOctave);
      Contract.Requires<ArgumentOutOfRangeException>(octave <= MaxOctave);

      int abs = CalcAbsoluteValue(tone.ToneName, tone.Accidental, octave);
      if( abs < s_minAbsoluteValue )
      {
        throw new ArgumentOutOfRangeException(
          $"Must be equal to or greater than {new Note(s_minAbsoluteValue, AccidentalMode)}");
      }

      if( abs > s_maxAbsoluteValue )
      {
        throw new ArgumentOutOfRangeException(
          $"Must be equal to or less than {new Note(s_maxAbsoluteValue, AccidentalMode)}");
      }

      return new Note(tone, octave, abs);
    }

    public static Note Create(ToneName toneName, Accidental accidental, int octave)
    {
      Contract.Requires<ArgumentOutOfRangeException>(toneName >= ToneName.C);
      Contract.Requires<ArgumentOutOfRangeException>(toneName <= ToneName.B);
      Contract.Requires<ArgumentOutOfRangeException>(accidental >= Accidental.DoubleFlat);
      Contract.Requires<ArgumentOutOfRangeException>(accidental <= Accidental.DoubleSharp);
      Contract.Requires<ArgumentOutOfRangeException>(octave >= MinOctave);
      Contract.Requires<ArgumentOutOfRangeException>(octave <= MaxOctave);

      int abs = CalcAbsoluteValue(toneName, accidental, octave);
      if( abs < s_minAbsoluteValue )
      {
        throw new ArgumentOutOfRangeException(
          $"Must be equal to or greater than {new Note(s_minAbsoluteValue, AccidentalMode)}");
      }

      if( abs > s_maxAbsoluteValue )
      {
        throw new ArgumentOutOfRangeException(
          $"Must be equal to or less than {new Note(s_maxAbsoluteValue, AccidentalMode)}");
      }

      return new Note(toneName, accidental, octave, abs);
    }

    public static Note CreateFromMidi(int midi)
    {
      Contract.Requires<ArgumentOutOfRangeException>(midi >= 0);
      Contract.Requires<ArgumentOutOfRangeException>(midi <= 127);

      int absoluteValue = midi - 12;
      if( absoluteValue < 0 )
      {
        throw new ArgumentOutOfRangeException(nameof(midi), "midi is out of range");
      }

      var note = new Note(absoluteValue, AccidentalMode);
      return note;
    }

    #endregion

    #region Properties

    public bool IsValid
    {
      get
      {
        int abs = _absoluteValue + (int) _tone.Accidental;
        return abs >= s_minAbsoluteValue && abs <= s_maxAbsoluteValue;
      }
    }

    public Tone Tone => _tone;

    public ToneName ToneName => _tone.ToneName;

    public Accidental Accidental => _tone.Accidental;

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

    public int Midi
    {
      get
      {
        // The formula to convert a note to MIDI (according to http://en.wikipedia.org/wiki/Note)
        // is p = 69 + 12 x log2(freq / 440). As it happens, our AbsoluteValue almost
        // matches, but we don't support the -1 octave we must add 12 to obtain the
        // MIDI number.
        return AbsoluteValue + 12;
      }
    }

    public static AccidentalMode AccidentalMode
    {
      get { return Tone.AccidentalMode; }
      set { Tone.AccidentalMode = value; }
    }

    #endregion

    #region IComparable<Note> Members

    public int CompareTo(Note other)
    {
      return AbsoluteValue - other.AbsoluteValue;
    }

    #endregion

    #region IEquatable<Note> Members

    public bool Equals(Note obj)
    {
      return obj.AbsoluteValue == AbsoluteValue;
    }

    #endregion

    #region Overrides

    public override bool Equals(object obj)
    {
      if( ReferenceEquals(obj, null) || obj.GetType() != GetType() )
      {
        return false;
      }

      return Equals((Note) obj);
    }

    public override int GetHashCode()
    {
      return AbsoluteValue;
    }

    public override string ToString()
    {
      return $"{_tone}{Octave}";
    }

    #endregion

    private static bool TryParseNotes(string value, ref Note note, int defaultOctave)
    {
      ToneName toneName;
      if( !Enum.TryParse(value.Substring(0, 1), true, out toneName) )
      {
        return false;
      }

      var index = 1;

      Accidental accidental;
      int octave = defaultOctave;

      TryGetAccidental(value, ref index, out accidental);
      TryGetOctave(value, ref index, ref octave);
      if( index < value.Length )
      {
        return false;
      }

      note = Create(toneName, accidental, octave);
      return true;
    }

    private static bool TryParseMidi(string value, ref Note note, int defaultOctave)
    {
      int midi;
      if( !int.TryParse(value, out midi) )
      {
        return false;
      }

      if( midi < 0 || midi > 127 )
      {
        return false;
      }

      note = CreateFromMidi(midi);
      return true;
    }

    private static int CalcAbsoluteValue(ToneName toneName, Accidental accidental, int octave)
    {
      int value = octave * IntervalsPerOctave + s_intervals[(int) toneName] + (int) accidental;
      return value;
    }

    private static void CalcNote(byte absoluteValue, out Tone tone, out byte octave, AccidentalMode accidentalMode)
    {
      int remainder;
      octave = (byte) Math.DivRem(absoluteValue, IntervalsPerOctave, out remainder);
      tone = Tone.GetNote(remainder, accidentalMode == AccidentalMode.FavorSharps);
    }

    private static void TryGetAccidental(string value, ref int index, out Accidental accidental)
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

    private static void TryGetOctave(string value, ref int index, ref int octave)
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

    public Note ApplyAccidental(Accidental accidental)
    {
      byte octave;
      Tone tone;
      CalcNote((byte) (AbsoluteValue + accidental), out tone, out octave,
               accidental < Accidental.Natural ? AccidentalMode.FavorFlats : AccidentalMode.FavorSharps);

      return Create(tone, octave);
    }

    public static bool operator ==(Note lhs, Note rhs) => Equals(lhs, rhs);

    public static bool operator !=(Note lhs, Note rhs) => !Equals(lhs, rhs);

    public static bool operator >(Note left, Note right) => left.CompareTo(right) > 0;

    public static bool operator <(Note left, Note right) => left.CompareTo(right) < 0;

    public static bool operator >=(Note left, Note right) => left.CompareTo(right) >= 0;

    public static bool operator <=(Note left, Note right) => left.CompareTo(right) <= 0;

    public Note Add(int interval, AccidentalMode accidentalMode = AccidentalMode.FavorSharps)
    {
      var result = new Note(AbsoluteValue + interval, accidentalMode);
      return result;
    }

    public Note Subtract(int interval, AccidentalMode accidentalMode = AccidentalMode.FavorSharps)
    {
      var result = new Note(AbsoluteValue - interval, accidentalMode);
      return result;
    }

    public static Note operator +(Note note, int interval)
    {
      return note.Add(interval, AccidentalMode);
    }

    public static Note operator ++(Note note)
    {
      return note.Add(1, AccidentalMode);
    }

    public static Note operator -(Note note, int interval)
    {
      return note.Subtract(interval, AccidentalMode);
    }

    public static Note operator --(Note note)
    {
      return note.Subtract(1, AccidentalMode);
    }

    public static int operator -(Note left, Note right)
    {
      return left.AbsoluteValue - right.AbsoluteValue;
    }

    public static bool TryParse(string value, out Note note, int defaultOctave = 4)
    {
      note = new Note();
      if( string.IsNullOrEmpty(value) )
      {
        return false;
      }

      if( char.IsDigit(value, 0) )
      {
        return TryParseMidi(value, ref note, defaultOctave);
      }

      return TryParseNotes(value, ref note, defaultOctave);
    }

    public static Note Parse(string value, int defaultOctave = 4)
    {
      Note result;
      if( !TryParse(value, out result) )
      {
        throw new FormatException($"{value} is not a valid note");
      }

      return result;
    }

    public static Note Min(Note a, Note b)
    {
      return a.AbsoluteValue < b.AbsoluteValue ? a : b;
    }
  }
}
