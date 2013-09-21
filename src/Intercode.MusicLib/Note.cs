// 
//   Note.cs: 
// 
//   Author: Eddie Velasquez
// 
//   Copyright (c) 2013  Intercode Consulting, LLC.  All Rights Reserved.
// 
//      Unauthorized use, duplication or distribution of this software, 
//      or any portion of it, is prohibited.  
// 
//   http://www.intercodeconsulting.com
// 

namespace Bach.Model
{
   using System;
   using System.Diagnostics.Contracts;
   using System.Text;

   public struct Note: IEquatable<Note>, IComparable<Note>
   {
      #region CoreNote struct

      private struct CoreNote
      {
         private readonly Tone _tone;
         private readonly Accidental _accidental;

         public CoreNote(Tone tone, Accidental accidental = Accidental.Natural)
         {
            _tone = tone;
            _accidental = accidental;
         }

         public Tone Tone
         {
            get { return _tone; }
         }

         public Accidental Accidental
         {
            get { return _accidental; }
         }
      }

      #endregion

      #region Constants

      public const int MIN_OCTAVE = 0;
      public const int MAX_OCTAVE = 9;
      public const double A4_FREQUENCY = 440.0;
      private const int INTERVALS_PER_OCTAVE = 12;

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
      private static readonly byte s_minAbsoluteValue = CalcAbsoluteValue(Tone.C, Accidental.Natural, MIN_OCTAVE);

      // G9 is the highest note supported by MIDI
      private static readonly byte s_maxAbsoluteValue = CalcAbsoluteValue(Tone.G, Accidental.Natural, MAX_OCTAVE);

      private static readonly CoreNote[] s_sharps =
      {
         new CoreNote(Tone.C), new CoreNote(Tone.C, Accidental.Sharp),
         new CoreNote(Tone.D), new CoreNote(Tone.D, Accidental.Sharp), new CoreNote(Tone.E), new CoreNote(Tone.F),
         new CoreNote(Tone.F, Accidental.Sharp), new CoreNote(Tone.G), new CoreNote(Tone.G, Accidental.Sharp),
         new CoreNote(Tone.A), new CoreNote(Tone.A, Accidental.Sharp), new CoreNote(Tone.B)
      };

      private static readonly CoreNote[] s_flats =
      {
         new CoreNote(Tone.C), new CoreNote(Tone.D, Accidental.Flat),
         new CoreNote(Tone.D), new CoreNote(Tone.E, Accidental.Flat), new CoreNote(Tone.E), new CoreNote(Tone.F),
         new CoreNote(Tone.G, Accidental.Flat), new CoreNote(Tone.G), new CoreNote(Tone.A, Accidental.Flat),
         new CoreNote(Tone.A), new CoreNote(Tone.B, Accidental.Flat), new CoreNote(Tone.B)
      };

      private static readonly Note s_a4 = Create(Tone.A, Accidental.Natural, 4);

      #endregion

      #region Data Members

      public static readonly Note Empty = new Note();
      private readonly byte _tone;
      private readonly byte _accidental;
      private readonly byte _octave;
      private readonly byte _absoluteValue;

      #endregion

      #region Construction

      private Note(int absoluteValue, AccidentalMode accidentalMode)
      {
         Contract.Requires<ArgumentOutOfRangeException>(absoluteValue >= 0, "absoluteValue");
         Contract.Requires<ArgumentOutOfRangeException>(absoluteValue < 128, "absoluteValue");

         _absoluteValue = (byte)absoluteValue;
         CalcNote(_absoluteValue, out _tone, out _accidental, out _octave, accidentalMode);
      }

      private Note(Tone tone, Accidental accidental, int octave, int absoluteValue)
      {
         Contract.Requires<ArgumentOutOfRangeException>(absoluteValue >= 0, "absoluteValue");
         Contract.Requires<ArgumentOutOfRangeException>(absoluteValue < 128, "absoluteValue");

         _tone = (byte)tone;
         _accidental = ToByte(accidental);
         _octave = (byte)octave;
         _absoluteValue = (byte)absoluteValue;
      }

      public static Note Create(Tone tone, Accidental accidental, int octave)
      {
         Contract.Requires<ArgumentOutOfRangeException>(tone >= Tone.C, "tone");
         Contract.Requires<ArgumentOutOfRangeException>(tone <= Tone.B, "tone");
         Contract.Requires<ArgumentOutOfRangeException>(accidental >= Accidental.DoubleFlat, "accidental");
         Contract.Requires<ArgumentOutOfRangeException>(accidental <= Accidental.DoubleSharp, "accidental");
         Contract.Requires<ArgumentOutOfRangeException>(octave >= MIN_OCTAVE, "octave");
         Contract.Requires<ArgumentOutOfRangeException>(octave <= MAX_OCTAVE, "octave");

         int abs = CalcAbsoluteValue(tone, accidental, octave);
         if( abs < s_minAbsoluteValue )
         {
            throw new ArgumentException(String.Format("Must be equal to or greater than {0}",
               new Note(s_minAbsoluteValue, AccidentalMode)));
         }

         if( abs > s_maxAbsoluteValue )
         {
            throw new ArgumentException(String.Format("Must be equal to or less than {0}",
               new Note(s_maxAbsoluteValue, AccidentalMode)));
         }

         return new Note(tone, accidental, octave, abs);
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

      public Tone Tone
      {
         get { return (Tone)_tone; }
      }

      public Accidental Accidental
      {
         get { return ToAccidental(_accidental); }
      }

      public int Octave
      {
         get { return _octave; }
      }

      public int AbsoluteValue
      {
         get { return _absoluteValue; }
      }

      public double Frequency
      {
         get
         {
            int interval = AbsoluteValue - s_a4.AbsoluteValue;
            double freq = Math.Pow(2, interval / 12.0) * A4_FREQUENCY;
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

      #region Public Methods

      public Note ApplyAccidental(Accidental accidental)
      {
         byte tone;
         byte octave;
         byte acc;
         CalcNote((byte)(AbsoluteValue + accidental), out tone, out acc, out octave,
            accidental < Accidental.Natural ? AccidentalMode.FavorFlats : AccidentalMode.FavorSharps);

         Note note = Create((Tone)tone, ToAccidental(acc), octave);
         return note;
      }

      #endregion

      #region IComparable<Note> Implementation

      public int CompareTo(Note other)
      {
         return AbsoluteValue - other.AbsoluteValue;
      }

      #endregion

      #region IEquatable<Note> Implementation

      public bool Equals(Note obj)
      {
         return obj.AbsoluteValue == AbsoluteValue;
      }

      #endregion

      #region Overrides

      public override bool Equals(object obj)
      {
         if( ReferenceEquals(obj, null) || obj.GetType() != GetType() )
            return false;

         return Equals((Note)obj);
      }

      public override int GetHashCode()
      {
         return AbsoluteValue;
      }

      public override string ToString()
      {
         return String.Format("{0}{1}{2}", Tone, Accidental.ToSymbol(), Octave);
      }

      #endregion

      #region Operators

      public static bool operator ==(Note lhs, Note rhs)
      {
         return Equals(lhs, rhs);
      }

      public static bool operator !=(Note lhs, Note rhs)
      {
         return !Equals(lhs, rhs);
      }

      public static bool operator >(Note left, Note right)
      {
         return left.CompareTo(right) > 0;
      }

      public static bool operator <(Note left, Note right)
      {
         return left.CompareTo(right) < 0;
      }

      public static bool operator >=(Note left, Note right)
      {
         return left.CompareTo(right) >= 0;
      }

      public static bool operator <=(Note left, Note right)
      {
         return left.CompareTo(right) <= 0;
      }

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
         Contract.Requires<ArgumentNullException>(note != null, "note");
         return note.Add(interval, AccidentalMode);
      }

      public static Note operator ++(Note note)
      {
         Contract.Requires<ArgumentNullException>(note != null, "note");
         return note.Add(1, AccidentalMode);
      }

      public static Note operator -(Note note, int interval)
      {
         Contract.Requires<ArgumentNullException>(note != null, "note");
         return note.Subtract(interval, AccidentalMode);
      }

      public static Note operator --(Note note)
      {
         Contract.Requires<ArgumentNullException>(note != null, "note");
         return note.Subtract(1, AccidentalMode);
      }

      public static int operator -(Note left, Note right)
      {
         Contract.Requires<ArgumentNullException>(left != null, "left");
         Contract.Requires<ArgumentNullException>(right != null, "right");

         return right.AbsoluteValue - left.AbsoluteValue;
      }

      #endregion

      #region Parsing

      public static bool TryParse(string value, out Note note, int defaultOctave = 4)
      {
         note = new Note();
         if( String.IsNullOrEmpty(value) )
            return false;

         Tone tone;
         if( !Enum.TryParse(value.Substring(0, 1), true, out tone) )
            return false;

         int index = 1;

         Accidental accidental;
         int octave = defaultOctave;

         TryGetAccidental(value, ref index, out accidental);
         TryGetOctave(value, ref index, ref octave);
         if( index < value.Length )
            return false;

         note = Create(tone, accidental, octave);
         return true;
      }

      public static Note Parse(string value, int defaultOctave = 4)
      {
         Note result;
         if( !TryParse(value, out result) )
            throw new ArgumentException(String.Format("{0} is not a valid note", value));

         return result;
      }

      #endregion

      #region Implementation

      private static Accidental ToAccidental(byte b)
      {
         return (Accidental)(b - 2);
      }

      private static byte ToByte(Accidental accidental)
      {
         return (byte)(accidental + 2);
      }

      private static byte CalcAbsoluteValue(Tone tone, Accidental accidental, int octave)
      {
         int value = (octave * INTERVALS_PER_OCTAVE) + s_intervals[(int)tone] + (int)accidental;
         return (byte)value;
      }

      private static void CalcNote(byte absoluteValue, out byte tone, out byte accidental, out byte octave,
         AccidentalMode accidentalMode)
      {
         int remainder;
         octave = (byte)Math.DivRem(absoluteValue, INTERVALS_PER_OCTAVE, out remainder);

         CoreNote[] notes = accidentalMode == AccidentalMode.FavorFlats ? s_flats : s_sharps;
         CoreNote coreNote = notes[remainder];

         tone = (byte)coreNote.Tone;
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
                  break;

               return;
            }

            buf.Append(ch);
         }

         if( buf.Length == 0 )
            return;

         if( AccidentalExtensions.TryParse(buf.ToString(), out accidental) )
            index += buf.Length;
      }

      private static void TryGetOctave(string value, ref int index, ref int octave)
      {
         if( index >= value.Length || !int.TryParse(value.Substring(index, 1), out octave) )
            return;

         if( octave >= MIN_OCTAVE && octave <= MAX_OCTAVE )
            ++index;
      }

      #endregion
   }
}
