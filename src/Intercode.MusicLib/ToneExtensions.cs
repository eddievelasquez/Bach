// 
//   ToneExtensions.cs: 
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
   public static class ToneExtensions
   {
      public static Tone Next(this Tone tone)
      {
         if( tone == Tone.B )
            return Tone.C;

         return tone + 1;
      }

      public static Tone Previous(this Tone tone)
      {
         if( tone == Tone.C )
            return Tone.B;

         return tone - 1;
      }
   }
}
