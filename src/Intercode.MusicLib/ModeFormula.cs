// 
//   ModeFormula.cs: 
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

   public class ModeFormula: IEquatable<ModeFormula>
   {
      #region Constants

      public const int MIN_TONIC = 1;
      public const int MAX_TONIC = 7;

      public static readonly ModeFormula Ionian = new ModeFormula("Ionian", 1);
      public static readonly ModeFormula Dorian = new ModeFormula("Dorian", 2);
      public static readonly ModeFormula Phrygian = new ModeFormula("Phrygian", 3);
      public static readonly ModeFormula Lydian = new ModeFormula("Lydian", 4);
      public static readonly ModeFormula Mixolydian = new ModeFormula("Mixolydian", 5);
      public static readonly ModeFormula Aeolian = new ModeFormula("Aeolian", 6);
      public static readonly ModeFormula Locrian = new ModeFormula("Locrian", 7);

      #endregion

      #region Construction

      public ModeFormula(string name, int tonic)
      {
         Contract.Requires<ArgumentNullException>(name != null, "name");
         Contract.Requires<ArgumentException>(name.Length > 0, "name");
         Contract.Requires<ArgumentOutOfRangeException>(tonic >= MIN_TONIC, "tonic");
         Contract.Requires<ArgumentOutOfRangeException>(tonic <= MAX_TONIC, "tonic");

         Name = name;
         Tonic = tonic;
      }

      #endregion

      #region Properties

      public string Name { get; private set; }
      public int Tonic { get; private set; }

      #endregion

      #region IEquatable<ModeFormula> Implementation

      public bool Equals(ModeFormula other)
      {
         if( ReferenceEquals(other, this) )
            return true;

         if( ReferenceEquals(other, null) )
            return false;

         return Tonic == other.Tonic;
      }

      public override bool Equals(object other)
      {
         if( ReferenceEquals(other, this) )
            return true;

         if( ReferenceEquals(other, null) || other.GetType() != GetType() )
            return false;

         return Equals((ModeFormula)other);
      }

      public override int GetHashCode()
      {
         return Tonic;
      }

      #endregion

      #region Overrides

      public override string ToString()
      {
         return Name;
      }

      #endregion
   }
}
