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
      private const int TONE_COUNT = 7;

      #endregion

      #region Data Members

      // Intervals from C
      private static readonly int[] s_intervals =
      {
         0,  // C 
         2,  // D
         4,  // E
         5,  // F
         7,  // G
         9,  // A
         11, // B
         12  // C
      };

      private static readonly int MIN_NOTE = CalcAbsoluteValue(Tone.C, Accidental.Natural, 1);
      private static readonly int MAX_NOTE = CalcAbsoluteValue(Tone.B, Accidental.Natural, 8);
      private static readonly int DOUBLE_FLAT_OFFSET = Math.Abs((int)Accidental.DoubleFlat);

      #endregion

      #region Construction

      private Note(Tone tone, Accidental accidental, int octave, int absoluteValue)
      {
         Tone = tone;
         Accidental = accidental;
         Octave = octave;
         AbsoluteValue = absoluteValue;
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
         if( abs < MIN_NOTE )
            throw new ArgumentException("Must be equal to or greater than C1");

         if( abs > MAX_NOTE )
            throw new ArgumentException("Must be equal to or less than B8");

         return new Note(tone, accidental, octave, abs);
      }

      #endregion

      #region Properties

      public Tone Tone { get; private set; }
      public Accidental Accidental { get; private set; }
      public int Octave { get; private set; }
      public int AbsoluteValue { get; private set; }

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

      #endregion

      #region Intervals

      public int GetIntervalBetween(Note other)
      {
         return other.AbsoluteValue - AbsoluteValue;
      }

      #endregion

      #region Implementation

      private static int CalcAbsoluteValue(Tone tone, Accidental accidental, int octave)
      {
         int value = ((octave - 1) * INTERVALS_PER_OCTAVE) + s_intervals[(int)tone] + (int)accidental;
         return value;
      }

      private static Tone Next(Tone tone)
      {
         if( tone == Tone.B )
            return Tone.C;

         return tone + 1;
      }

      #endregion
   }
}
