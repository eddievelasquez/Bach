// 
//   ToneExtensionsTest.cs: 
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

namespace Bach.Model.Test
{
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   [ TestClass ]
   public class ToneExtensionsTest
   {
      [ TestMethod ]
      public void NextTest()
      {
         Assert.AreEqual(Tone.D, Tone.C.Next());
         Assert.AreEqual(Tone.C, Tone.B.Next());
      }

      [ TestMethod ]
      public void PreviousTest()
      {
         Assert.AreEqual(Tone.C, Tone.D.Previous());         
         Assert.AreEqual(Tone.B, Tone.C.Previous());
      }
   }
}
