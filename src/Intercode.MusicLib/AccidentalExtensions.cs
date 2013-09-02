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
      private static readonly int s_doubleFlatOffset = Math.Abs((int)Accidental.DoubleFlat);

      public static String ToSymbol(this Accidental accidental)
      {
         Contract.Requires(accidental >= Accidental.DoubleFlat && accidental <= Accidental.DoubleSharp);
         return s_symbols[(int)accidental + s_doubleFlatOffset];
      }

      public static bool TryParse(string value, out Accidental accidental)
      {
         accidental = Accidental.Natural;
         if( String.IsNullOrEmpty(value) )
            return true;

         if( value.Length > 2 )
            return false;

         foreach( char c in value )
         {
            if( c == 'b' || c == 'B' )
               --accidental;
            else if( c == '#' )
               ++accidental;
            else
               return false;
         }

         // Cannot be natural unless the "b#" or "#b" combinations are found
         return accidental != Accidental.Natural;
      }

      public static Accidental Parse(string value)
      {
         Accidental accidental;
         if( !TryParse(value, out accidental) )
            throw new ArgumentException(String.Format("{0} is not a valid accidental", value));

         return accidental;
      }
   }
}
