// 
//   AccidentalExtensions.cs: 
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

   public static class AccidentalExtensions
   {
      private static readonly String[] s_symbols = { "bb", "b", "", "#", "##" };

      public static String ToSymbol(this Accidental accidental)
      {
         Contract.Requires(accidental >= Accidental.DoubleFlat && accidental <= Accidental.DoubleSharp);
         return s_symbols[(int)accidental + Math.Abs((int)Accidental.DoubleFlat)];
      }
   }
}
