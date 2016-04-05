//  
// Module Name: AbsoluteNote.cs
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

  public struct AbsoluteNote: IEquatable<AbsoluteNote>,
                              IComparable<AbsoluteNote>
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
    private static readonly byte s_minAbsoluteValue = CalcAbsoluteValue(Tone.C, Accidental.Natural, MinOctave);

    // G9 is the highest note supported by MIDI
    private static readonly byte s_maxAbsoluteValue = CalcAbsoluteValue(Tone.G, Accidental.Natural, MaxOctave);

    private static readonly CoreNote[] s_sharps =
    {
      new CoreNote(Tone.C),
      new CoreNote(Tone.C, Accidental.Sharp),
      new CoreNote(Tone.D),
      new CoreNote(Tone.D, Accidental.Sharp),
      new CoreNote(Tone.E),
      new CoreNote(Tone.F),
      new CoreNote(Tone.F, Accidental.Sharp),
      new CoreNote(Tone.G),
      new CoreNote(Tone.G, Accidental.Sharp),
      new CoreNote(Tone.A),
      new CoreNote(Tone.A, Accidental.Sharp),
      new CoreNote(Tone.B)
    };

    private static readonly CoreNote[] s_flats =
    {
      new CoreNote(Tone.C),
      new CoreNote(Tone.D, Accidental.Flat),
      new CoreNote(Tone.D),
      new CoreNote(Tone.E, Accidental.Flat),
      new CoreNote(Tone.E),
      new CoreNote(Tone.F),
      new CoreNote(Tone.G, Accidental.Flat),
      new CoreNote(Tone.G),
      new CoreNote(Tone.A, Accidental.Flat),
      new CoreNote(Tone.A),
      new CoreNote(Tone.B, Accidental.Flat),
      new CoreNote(Tone.B)
    };

    private static readonly AbsoluteNote s_a4 = Create(Tone.A, Accidental.Natural, 4);

    public static readonly AbsoluteNote Empty = new AbsoluteNote();

    #region Data Members

    private readonly byte _absoluteValue;
    private readonly byte _accidental;
    private readonly byte _octave;
    private readonly byte _tone;

    #endregion

    #region Construction

    private AbsoluteNote(int absoluteValue, AccidentalMode accidentalMode)
    {
      Contract.Requires<ArgumentOutOfRangeException>(absoluteValue >= 0);
      Contract.Requires<ArgumentOutOfRangeException>(absoluteValue < 128);

      _absoluteValue = (byte) absoluteValue;
      CalcNote(_absoluteValue, out _tone, out _accidental, out _octave, accidentalMode);
    }

    private AbsoluteNote(Tone tone, Accidental accidental, int octave, int absoluteValue)
    {
      Contract.Requires<ArgumentOutOfRangeException>(absoluteValue >= 0);
      Contract.Requires<ArgumentOutOfRangeException>(absoluteValue < 128);

      _tone = (byte) tone;
      _accidental = ToByte(accidental);
      _octave = (byte) octave;
      _absoluteValue = (byte) absoluteValue;
    }

    #endregion

    #region Properties

    public bool IsValid
    {
      get
      {
        // An absoluteValue of zero corresponds to C0 and an accidental
        // of 0 is FlatFlat, which cannot be a valid Note.
        return _absoluteValue != 0 || _accidental != 0;
      }
    }

    public Tone Tone => (Tone) _tone;

    public Accidental Accidental => ToAccidental(_accidental);

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

    public static AccidentalMode AccidentalMode { get; set; }

    #endregion

    #region IComparable<Note> Members

    public int CompareTo(AbsoluteNote other)
    {
      return AbsoluteValue - other.AbsoluteValue;
    }

    #endregion

    #region IEquatable<Note> Members

    public bool Equals(AbsoluteNote obj)
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

      return Equals((AbsoluteNote) obj);
    }

    public override int GetHashCode()
    {
      return AbsoluteValue;
    }

    public override string ToString()
    {
      return $"{Tone}{Accidental.ToSymbol()}{Octave}";
    }

    #endregion

    private static bool TryParseNotes(string value, ref AbsoluteNote note, int defaultOctave)
    {
      Tone tone;
      if( !Enum.TryParse(value.Substring(0, 1), true, out tone) )
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

      note = Create(tone, accidental, octave);
      return true;
    }

    private static bool TryParseMidi(string value, ref AbsoluteNote note, int defaultOctave)
    {
      int midi;
      if( !int.TryParse(value, out midi) )
      {
        return false;
      }

      note = FromMidi(midi);
      return true;
    }

    private static Accidental ToAccidental(byte b)
    {
      return (Accidental) (b - 2);
    }

    private static byte ToByte(Accidental accidental)
    {
      return (byte) (accidental + 2);
    }

    private static byte CalcAbsoluteValue(Tone tone, Accidental accidental, int octave)
    {
      int value = octave * IntervalsPerOctave + s_intervals[(int) tone] + (int) accidental;
      return (byte) value;
    }

    private static void CalcNote(
      byte absoluteValue,
      out byte tone,
      out byte accidental,
      out byte octave,
      AccidentalMode accidentalMode)
    {
      int remainder;
      octave = (byte) Math.DivRem(absoluteValue, IntervalsPerOctave, out remainder);

      var notes = accidentalMode == AccidentalMode.FavorFlats ? s_flats : s_sharps;
      CoreNote coreNote = notes[remainder];

      tone = (byte) coreNote.Tone;
      accidental = ToByte(coreNote.Accidental);
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

    public static AbsoluteNote Create(Tone tone, Accidental accidental, int octave)
    {
      Contract.Requires<ArgumentOutOfRangeException>(tone >= Tone.C);
      Contract.Requires<ArgumentOutOfRangeException>(tone <= Tone.B);
      Contract.Requires<ArgumentOutOfRangeException>(accidental >= Accidental.DoubleFlat);
      Contract.Requires<ArgumentOutOfRangeException>(accidental <= Accidental.DoubleSharp);
      Contract.Requires<ArgumentOutOfRangeException>(octave >= MinOctave);
      Contract.Requires<ArgumentOutOfRangeException>(octave <= MaxOctave);

      int abs = CalcAbsoluteValue(tone, accidental, octave);
      if( abs < s_minAbsoluteValue )
      {
        throw new ArgumentException(
          $"Must be equal to or greater than {new AbsoluteNote(s_minAbsoluteValue, AccidentalMode)}");
      }

      if( abs > s_maxAbsoluteValue )
      {
        throw new ArgumentException(
          $"Must be equal to or less than {new AbsoluteNote(s_maxAbsoluteValue, AccidentalMode)}");
      }

      return new AbsoluteNote(tone, accidental, octave, abs);
    }

    public static AbsoluteNote FromMidi(int midi)
    {
      Contract.Requires<ArgumentOutOfRangeException>(midi >= 0);
      Contract.Requires<ArgumentOutOfRangeException>(midi <= 127);

      int absoluteValue = midi - 12;
      if( absoluteValue < 0 )
      {
        throw new ArgumentOutOfRangeException(nameof(midi), "midi is out of range");
      }

      var note = new AbsoluteNote(absoluteValue, AccidentalMode);
      return note;
    }

    public AbsoluteNote ApplyAccidental(Accidental accidental)
    {
      byte tone;
      byte octave;
      byte acc;
      CalcNote((byte) (AbsoluteValue + accidental), out tone, out acc, out octave,
               accidental < Accidental.Natural ? AccidentalMode.FavorFlats : AccidentalMode.FavorSharps);

      AbsoluteNote note = Create((Tone) tone, ToAccidental(acc), octave);
      return note;
    }

    public static bool operator ==(AbsoluteNote lhs, AbsoluteNote rhs) => Equals(lhs, rhs);

    public static bool operator !=(AbsoluteNote lhs, AbsoluteNote rhs) => !Equals(lhs, rhs);

    public static bool operator >(AbsoluteNote left, AbsoluteNote right) => left.CompareTo(right) > 0;

    public static bool operator <(AbsoluteNote left, AbsoluteNote right) => left.CompareTo(right) < 0;

    public static bool operator >=(AbsoluteNote left, AbsoluteNote right) => left.CompareTo(right) >= 0;

    public static bool operator <=(AbsoluteNote left, AbsoluteNote right) => left.CompareTo(right) <= 0;

    public AbsoluteNote Add(int interval, AccidentalMode accidentalMode = AccidentalMode.FavorSharps)
    {
      var result = new AbsoluteNote(AbsoluteValue + interval, accidentalMode);
      return result;
    }

    public AbsoluteNote Subtract(int interval, AccidentalMode accidentalMode = AccidentalMode.FavorSharps)
    {
      var result = new AbsoluteNote(AbsoluteValue - interval, accidentalMode);
      return result;
    }

    public static AbsoluteNote operator +(AbsoluteNote note, int interval)
    {
      Contract.Requires<ArgumentNullException>(note != null);
      return note.Add(interval, AccidentalMode);
    }

    public static AbsoluteNote operator ++(AbsoluteNote note)
    {
      Contract.Requires<ArgumentNullException>(note != null);
      return note.Add(1, AccidentalMode);
    }

    public static AbsoluteNote operator -(AbsoluteNote note, int interval)
    {
      Contract.Requires<ArgumentNullException>(note != null);
      return note.Subtract(interval, AccidentalMode);
    }

    public static AbsoluteNote operator --(AbsoluteNote note)
    {
      Contract.Requires<ArgumentNullException>(note != null);
      return note.Subtract(1, AccidentalMode);
    }

    public static int operator -(AbsoluteNote left, AbsoluteNote right)
    {
      Contract.Requires<ArgumentNullException>(left != null);
      Contract.Requires<ArgumentNullException>(right != null);

      return right.AbsoluteValue - left.AbsoluteValue;
    }

    public static bool TryParse(string value, out AbsoluteNote note, int defaultOctave = 4)
    {
      note = new AbsoluteNote();
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

    public static AbsoluteNote Parse(string value, int defaultOctave = 4)
    {
      AbsoluteNote result;
      if( !TryParse(value, out result) )
      {
        throw new ArgumentException($"{value} is not a valid note");
      }

      return result;
    }

    #region Nested type: CoreNote

    private struct CoreNote
    {
      #region Construction

      public CoreNote(Tone tone, Accidental accidental = Accidental.Natural)
      {
        Tone = tone;
        Accidental = accidental;
      }

      #endregion

      #region Properties

      public Tone Tone { get; }

      public Accidental Accidental { get; }

      #endregion
    }

    #endregion
  }
}
