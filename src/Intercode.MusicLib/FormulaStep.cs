// 
//   FormulaStep.cs: 
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

   public struct FormulaStep: IEquatable<FormulaStep>
   {
      #region Data Members

      private readonly int _interval;
      private readonly Accidental _accidental;

      #endregion

      #region Construction

      public FormulaStep(int interval, Accidental accidental = Accidental.Natural)
      {
         Contract.Requires<ArgumentOutOfRangeException>(interval > 0, "interval");
         Contract.Requires<ArgumentOutOfRangeException>(interval < 16, "interval");
         Contract.Requires<ArgumentOutOfRangeException>(accidental >= Accidental.DoubleFlat, "accidental");
         Contract.Requires<ArgumentOutOfRangeException>(accidental <= Accidental.DoubleSharp, "accidental");

         _interval = interval;
         _accidental = accidental;
      }

      #endregion

      #region Properties

      public int Interval
      {
         get { return _interval; }
      }

      public Accidental Accidental
      {
         get { return _accidental; }
      }

      #endregion

      #region IEquatable<FormulaStep> Implementation

      public bool Equals(FormulaStep obj)
      {
         return Interval == obj.Interval && Accidental == obj.Accidental;
      }

      public override bool Equals(object obj)
      {
         if( ReferenceEquals(obj, null) || obj.GetType() != GetType() )
            return false;

         return Equals((FormulaStep)obj);
      }

      public override int GetHashCode()
      {
         int hashCode = ((short)(Accidental + 2) << 16) | Interval;
         return hashCode;
      }

      #endregion

      #region Overrides

      public override string ToString()
      {
         return String.Format("{0}{1}", Interval, Accidental.ToSymbol());
      }

      #endregion
   }
}
