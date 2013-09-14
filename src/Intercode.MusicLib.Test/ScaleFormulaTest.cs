// 
//   ScaleFormulaTest.cs: 
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

namespace Intercode.MusicLib.Test
{
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   [ TestClass ]
   public class ScaleFormulaTest
   {
      [ TestMethod ]
      public void ConstructorTest()
      {
         var major = new ScaleFormula("Major", 2, 2, 1, 2, 2, 2, 2);
         Assert.AreEqual("Major", major.Name);
         Assert.AreEqual(7, major.Count);
      }
   }
}
