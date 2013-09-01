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

   public struct Note: IEquatable<Note>, IComparable<Note>
   {
      #region Data Members

      private readonly Accidental _accidental;
      private readonly int _octave;
      private readonly Tone _tone;

      #endregion

      #region Construction

      public Note(Tone tone, Accidental accidental, int octave)
      {
         Contract.Requires<ArgumentOutOfRangeException>(tone >= Tone.C, "tone");
         Contract.Requires<ArgumentOutOfRangeException>(tone <= Tone.B, "tone");
         Contract.Requires<ArgumentOutOfRangeException>(accidental >= Accidental.DoubleFlat, "accidental");
         Contract.Requires<ArgumentOutOfRangeException>(accidental <= Accidental.DoubleSharp, "accidental");
         Contract.Requires<ArgumentOutOfRangeException>(octave >= 1, "octave");
         Contract.Requires<ArgumentOutOfRangeException>(octave <= 8, "octave");

         _tone = tone;
         _accidental = accidental;
         _octave = octave;
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

      #endregion

      #region IComparable<Note> Members

      public int CompareTo(Note other)
      {
         int result = Octave.CompareTo(other.Octave);
         if( result != 0 )
            return result;

         result = Tone.CompareTo(other.Tone);
         if( result == 0 )
            result = Accidental.CompareTo(other.Accidental);

         return result;
      }

      #endregion

      #region IEquatable<Note> Members

      public bool Equals(Note other)
      {
         return _accidental == other._accidental && _octave == other._octave && _tone == other._tone;
      }

      #endregion

      #region Overrides

      public override bool Equals(object obj)
      {
         if( ReferenceEquals(null, obj) )
            return false;

         return obj is Note && Equals((Note)obj);
      }

      public override int GetHashCode()
      {
         unchecked
         {
            var hashCode = (int)_accidental;
            hashCode = (hashCode * 397) ^ _octave;
            hashCode = (hashCode * 397) ^ (int)_tone;
            return hashCode;
         }
      }

      public override string ToString()
      {
         return String.Format("{0}{1}{2}", Tone, Accidental.ToSymbol(), Octave);
      }

      #endregion

      #region Operators

      public static bool operator ==(Note left, Note right)
      {
         return left.Equals(right);
      }

      public static bool operator !=(Note left, Note right)
      {
         return !left.Equals(right);
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

      #endregion
   }
}
