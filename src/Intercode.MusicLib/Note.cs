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

   public class Note: IEquatable<Note>, IComparable<Note>
   {
      #region Constants

      private const int INTERVALS_PER_OCTAVE = 12;

      private static readonly int[] s_intervals = { 0, // C 
         2, // D
         4, // E
         5, // F
         7, // G
         9, // A
         11, // B
         12 // C
      };

      private static readonly int s_minNote = CalcAbsoluteValue(Tone.C, Accidental.Natural, 1);
      private static readonly int s_maxNote = CalcAbsoluteValue(Tone.B, Accidental.Natural, 8);

      #endregion

      #region Data Members

      private readonly Tone _tone;
      private readonly Accidental _accidental;
      private readonly int _octave;
      private readonly int _absoluteValue;

      #endregion

      #region Construction

      private Note(int absoluteValue)
      {
         _absoluteValue = absoluteValue;
         CalcNote(absoluteValue, out _tone, out _accidental, out _octave);
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

         // TODO: Create a AbsoluteValueToNote function so the note in the error message 
         // isn't hardcoded.
         int abs = CalcAbsoluteValue(tone, accidental, octave);
         if( abs < s_minNote )
            throw new ArgumentException("Must be equal to or greater than C1");

         if( abs > s_maxNote )
            throw new ArgumentException("Must be equal to or less than B8");

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

      public static Note operator +(Note note, int interval)
      {
         Contract.Requires<ArgumentNullException>(note != null, "note");

         var result = new Note(note.AbsoluteValue + interval);
         return result;
      }

      public static Note operator ++(Note note)
      {
         Contract.Requires<ArgumentNullException>(note != null, "note");

         var result = new Note(note.AbsoluteValue + 1);
         return result;
      }

      public static Note operator -(Note note, int interval)
      {
         Contract.Requires<ArgumentNullException>(note != null, "note");

         var result = new Note(note.AbsoluteValue - interval);
         return result;
      }

      public static Note operator --(Note note)
      {
         Contract.Requires<ArgumentNullException>(note != null, "note");

         var result = new Note(note.AbsoluteValue - 1);
         return result;
      }

      public static int operator -(Note left, Note right)
      {
         Contract.Requires<ArgumentNullException>(left != null, "left");
         Contract.Requires<ArgumentNullException>(right != null, "right");

         return right.AbsoluteValue - left.AbsoluteValue;
      }

      #endregion

      #region Implementation

      private static int CalcAbsoluteValue(Tone tone, Accidental accidental, int octave)
      {
         int value = ((octave - 1) * INTERVALS_PER_OCTAVE) + s_intervals[(int)tone] + (int)accidental;
         return value;
      }

      private static void CalcNote(int absoluteValue, out Tone tone, out Accidental accidental, out int octave)
      {
         int remainder;
         octave = Math.DivRem(absoluteValue, INTERVALS_PER_OCTAVE, out remainder) + 1;

         tone = Tone.C;
         foreach( var interval in s_intervals )
         {
            if( remainder - interval - AccidentalMode <= 0 )
            {
               remainder -= interval;
               break;
            }

            ++tone;
         }

         accidental = (Accidental)remainder;
      }

      #endregion
   }
}
