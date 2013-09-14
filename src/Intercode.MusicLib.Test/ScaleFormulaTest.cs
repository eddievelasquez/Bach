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
   using System;
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

      [ TestMethod ]
      public void EqualsContractTest()
      {
         object x = new ScaleFormula("Test", "1,2,3");
         object y = new ScaleFormula("Test", "1,2,3");
         object z = new ScaleFormula("Test", "1,2,3");

         Assert.IsTrue(x.Equals(x)); // Reflexive
         Assert.IsTrue(x.Equals(y)); // Symetric
         Assert.IsTrue(y.Equals(x));
         Assert.IsTrue(y.Equals(z)); // Transitive
         Assert.IsTrue(x.Equals(z));
         Assert.IsFalse(x.Equals(null)); // Never equal to null
      }

      [ TestMethod ]
      public void TypeSafeEqualsContractTest()
      {
         var x = new ScaleFormula("Test", "1,2,3");
         var y = new ScaleFormula("Test", "1,2,3");
         var z = new ScaleFormula("Test", "1,2,3");

         Assert.IsTrue(x.Equals(x)); // Reflexive
         Assert.IsTrue(x.Equals(y)); // Symetric
         Assert.IsTrue(y.Equals(x));
         Assert.IsTrue(y.Equals(z)); // Transitive
         Assert.IsTrue(x.Equals(z));
         Assert.IsFalse(x.Equals(null)); // Never equal to null
      }

      [ TestMethod ]
      public void EqualsFailsWithDifferentTypeTest()
      {
         object actual = new ScaleFormula("Test", "1,2,3");
         Assert.IsFalse(actual.Equals(Int32.MinValue));
      }

      [ TestMethod ]
      public void TypeSafeEqualsFailsWithDifferentTypeTest()
      {
         var actual = new ScaleFormula("Test", "1,2,3");
         Assert.IsFalse(actual.Equals(Int32.MinValue));
      }

      [ TestMethod ]
      public void EqualsFailsWithNullTest()
      {
         object actual = new ScaleFormula("Test", "1,2,3");
         Assert.IsFalse(actual.Equals(null));
      }

      [ TestMethod ]
      public void TypeSafeEqualsFailsWithNullTest()
      {
         var actual = new ScaleFormula("Test", "1,2,3");
         Assert.IsFalse(actual.Equals(null));
      }

      [ TestMethod ]
      public void EqualsSucceedsWithSameObjectTest()
      {
         var actual = new ScaleFormula("Test", "1,2,3");
         Assert.IsTrue(actual.Equals(actual));
      }

      [ TestMethod ]
      public void GetHashcodeTest()
      {
         var actual = new ScaleFormula("Test", "1,2,3");
         var expected = new ScaleFormula("Test", "1,2,3");
         Assert.IsTrue(expected.Equals(actual));
         Assert.AreEqual(expected.GetHashCode(), actual.GetHashCode());
      }
   }
}
