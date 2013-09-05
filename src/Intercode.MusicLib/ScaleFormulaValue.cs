// 
//   ScaleFormulaValue.cs: 
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
   public struct ScaleFormulaValue
   {
      #region Data Members

      private readonly Accidental _accidental;
      private readonly int _number;

      #endregion

      #region Construction

      public ScaleFormulaValue(int number, Accidental accidental = Accidental.Natural)
      {
         _number = number;
         _accidental = accidental;
      }

      #endregion

      #region Properties

      public int Number
      {
         get { return _number; }
      }

      public Accidental Accidental
      {
         get { return _accidental; }
      }

      #endregion
   }
}
