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

namespace Intercode.MusicLib
{
   using System;
   using System.Diagnostics.Contracts;

   public struct FormulaStep
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
   }
}
