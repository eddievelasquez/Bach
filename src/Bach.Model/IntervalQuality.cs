// 
//   IntervalQuality.cs: 
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

   public enum IntervalQuality
   {
      Unknown = -1,
      Diminished,
      Minor,
      Perfect,
      Major,
      Augmented
   }

   public static class IntervalQualityExtensions
   {
      #region Constants

      private static readonly string[] s_symbols = { "d", "m", "P", "M", "A" };
      private static readonly string[] s_short = { "dim", "min", "Perf", "Maj", "Aug" };
      private static readonly string[] s_long = { "diminished", "minor", "perfect", "major", "augmented" };

      #endregion

      #region Public Methods

      public static IntervalQuality Parse(string value)
      {
         IntervalQuality quality;
         if( !TryParse(value, out quality) )
            throw new FormatException(string.Format("\"{0}\" is not a valid interval quality", value));

         return quality;
      }

      public static bool TryParse(string value, out IntervalQuality quality)
      {
         if( !string.IsNullOrEmpty(value) )
         {
            for( int i = 0; i < s_symbols.Length; i++ )
            {
               if( s_symbols[i].Equals(value) )
               {
                  quality = (IntervalQuality)i;
                  return true;
               }
            }
         }

         quality = IntervalQuality.Unknown;
         return false;
      }

      public static string Symbol(this IntervalQuality quality)
      {
         Contract.Requires<ArgumentOutOfRangeException>(quality >= IntervalQuality.Diminished, "quality");
         Contract.Requires<ArgumentOutOfRangeException>(quality <= IntervalQuality.Augmented, "quality");

         return s_symbols[(int)quality];
      }

      public static string ShortName(this IntervalQuality quality)
      {
         Contract.Requires<ArgumentOutOfRangeException>(quality >= IntervalQuality.Diminished, "quality");
         Contract.Requires<ArgumentOutOfRangeException>(quality <= IntervalQuality.Augmented, "quality");

         return s_short[(int)quality];
      }

      public static string LongName(this IntervalQuality quality)
      {
         Contract.Requires<ArgumentOutOfRangeException>(quality >= IntervalQuality.Diminished, "quality");
         Contract.Requires<ArgumentOutOfRangeException>(quality <= IntervalQuality.Augmented, "quality");

         return s_long[(int)quality];
      }

      #endregion
   }
}
