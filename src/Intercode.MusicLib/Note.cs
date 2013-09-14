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

namespace Intercode.MusicLib
{
   using System;
   using System.Collections.Generic;
   using System.Diagnostics.Contracts;
   using System.Linq;
   using System.Text;

   public class Note: IEquatable<Note>, IComparable<Note>
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

      private const int INTERVALS_PER_OCTAVE = 12;
      public const int MIN_OCTAVE = 1;
      public const int MAX_OCTAVE = 8;

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

      private static readonly int s_minAbsoluteValue = CalcAbsoluteValue(Tone.C, Accidental.Natural, MIN_OCTAVE);
      private static readonly int s_maxAbsoluteValue = CalcAbsoluteValue(Tone.B, Accidental.Natural, MAX_OCTAVE);

      private static readonly CoreNote[] s_sharps =
      {
         new CoreNote(Tone.C), new CoreNote(Tone.C, Accidental.Sharp),
         new CoreNote(Tone.D), new CoreNote(Tone.D, Accidental.Sharp), new CoreNote(Tone.E), new CoreNote(Tone.F),
         new CoreNote(Tone.F, Accidental.Sharp), new CoreNote(Tone.G), new CoreNote(Tone.G, Accidental.Sharp),
         new CoreNote(Tone.A), new CoreNote(Tone.A, Accidental.Sharp), new CoreNote(Tone.B),
      };

      private static readonly CoreNote[] s_flats =
      {
         new CoreNote(Tone.C), new CoreNote(Tone.D, Accidental.Flat),
         new CoreNote(Tone.D), new CoreNote(Tone.E, Accidental.Flat), new CoreNote(Tone.E), new CoreNote(Tone.F),
         new CoreNote(Tone.G, Accidental.Flat), new CoreNote(Tone.G), new CoreNote(Tone.A, Accidental.Flat),
         new CoreNote(Tone.A), new CoreNote(Tone.B, Accidental.Flat), new CoreNote(Tone.B),
      };

      private static readonly Note A4 = Create(Tone.A, Accidental.Natural, 4);

      #endregion

      #region Data Members

      private readonly Tone _tone;
      private readonly Accidental _accidental;
      private readonly int _octave;
      private readonly int _absoluteValue;

      #endregion

      #region Construction

      private Note(int absoluteValue, AccidentalMode accidentalMode)
      {
         _absoluteValue = absoluteValue;
         CalcNote(absoluteValue, out _tone, out _accidental, out _octave, accidentalMode);
      }

      private Note(Tone tone, Accidental accidental, int octave, int absoluteValue)
      {
         _tone = tone;
         _accidental = accidental;
         _octave = octave;
         _absoluteValue = absoluteValue;
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

      public Tone Tone
      {
         get { return _tone; }
      }

      public Accidental Accidental
      {
         get { return _accidental; }
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
            int interval = AbsoluteValue - A4.AbsoluteValue;
            double freq = Math.Pow(2, interval / 12.0) * 440.0;
            return freq;
         }
      }

      public static AccidentalMode AccidentalMode { get; set; }

      #endregion

      #region Public Methods

      public Note ApplyAccidental(Accidental accidental)
      {
         Tone tone;
         int octave;
         CalcNote(AbsoluteValue + (int)accidental, out tone, out accidental, out octave,
            accidental < Accidental.Natural ? AccidentalMode.FavorFlats : AccidentalMode.FavorSharps);

         Note note = Create(tone, accidental, octave);
         return note;
      }

      #endregion

      #region IComparable<Note> Members

      public int CompareTo(Note other)
      {
         if( other == null )
            return 1;

         return AbsoluteValue - other.AbsoluteValue;
      }

      #endregion

      #region IEquatable<Note> Members

      public bool Equals(Note other)
      {
         if( ReferenceEquals(other, this) )
            return true;

         if( ReferenceEquals(other, null) )
            return false;

         return AbsoluteValue == other.AbsoluteValue;
      }

      #endregion

      #region Overrides

      public override bool Equals(object other)
      {
         if( ReferenceEquals(other, this) )
            return true;

         if( ReferenceEquals(other, null) || other.GetType() != GetType() )
            return false;

         return Equals((Note)other);
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
         if( ReferenceEquals(lhs, rhs) )
            return true;

         if( ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null) )
            return false;

         return lhs.Equals(rhs);
      }

      public static bool operator !=(Note lhs, Note rhs)
      {
         if( ReferenceEquals(lhs, rhs) )
            return false;

         if( ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null) )
            return true;

         return !lhs.Equals(rhs);
      }

      public static bool operator >(Note left, Note right)
      {
         if( left == null )
            return false;

         return left.CompareTo(right) > 0;
      }

      public static bool operator <(Note left, Note right)
      {
         if( left == null )
            return true;

         return left.CompareTo(right) < 0;
      }

      public static bool operator >=(Note left, Note right)
      {
         if( left == null )
            return right == null;

         return left.CompareTo(right) >= 0;
      }

      public static bool operator <=(Note left, Note right)
      {
         if( left == null )
            return true;

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
         note = null;

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

      public static IEnumerable<Note> ParseArray(string value, int defaultOctave = 4)
      {
         Contract.Requires<ArgumentNullException>(value != null, "value");
         Contract.Requires<ArgumentException>(value.Length > 0, "value");
         Contract.Requires<ArgumentOutOfRangeException>(defaultOctave >= MIN_OCTAVE, "defaultOctave");
         Contract.Requires<ArgumentOutOfRangeException>(defaultOctave <= MAX_OCTAVE, "defaultOctave");

         string[] values = value.Split(',');
         return values.Select(s => Parse(s, defaultOctave));
      }

      #endregion

      #region Implementation

      private static int CalcAbsoluteValue(Tone tone, Accidental accidental, int octave)
      {
         int value = ((octave - 1) * INTERVALS_PER_OCTAVE) + s_intervals[(int)tone] + (int)accidental;
         return value;
      }

      private static void CalcNote(int absoluteValue, out Tone tone, out Accidental accidental, out int octave,
         AccidentalMode accidentalMode)
      {
         int remainder;
         octave = Math.DivRem(absoluteValue, INTERVALS_PER_OCTAVE, out remainder) + 1;

         CoreNote[] notes = accidentalMode == AccidentalMode.FavorFlats ? s_flats : s_sharps;
         CoreNote coreNote = notes[remainder];

         tone = coreNote.Tone;
         accidental = coreNote.Accidental;
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
