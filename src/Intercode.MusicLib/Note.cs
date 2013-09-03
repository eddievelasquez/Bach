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
   using System.Diagnostics.Contracts;
   using System.Text;

   public class Note: IEquatable<Note>, IComparable<Note>
   {
      #region Constants

      private const int INTERVALS_PER_OCTAVE = 12;
      private const int MIN_OCTAVE = 1;
      private const int MAX_OCTAVE = 8;

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
         Contract.Requires<ArgumentOutOfRangeException>(octave >= 1, "octave");
         Contract.Requires<ArgumentOutOfRangeException>(octave <= 8, "octave");

         int abs = CalcAbsoluteValue(tone, accidental, octave);
         if( abs < s_minAbsoluteValue )
         {
            throw new ArgumentException(String.Format("Must be equal to or greater than {0}",
               new Note(s_minAbsoluteValue, AccidentalMode)));
         }

         if( abs > s_maxAbsoluteValue )
            throw new ArgumentException(String.Format("Must be equal to or less than {0}", new Note(s_maxAbsoluteValue, AccidentalMode)));

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

      public static AccidentalMode AccidentalMode { get; set; }

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

         tone = Tone.C;
         foreach( var interval in s_intervals )
         {
            if( remainder - interval - accidentalMode <= 0 )
            {
               remainder -= interval;
               break;
            }

            ++tone;
         }

         accidental = (Accidental)remainder;
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
