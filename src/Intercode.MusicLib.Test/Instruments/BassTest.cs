// 
// BassTest.cs: 
// 
// Author: evelasquez
// 
// Copyright (c) 2014  Intercode Consulting, LLC.  All Rights Reserved.
// 
// Unauthorized use, duplication or distribution of this software, 
// or any portion of it, is prohibited.  
// 
// http://www.intercodeconsulting.com

namespace Bach.Model.Test.Instruments
{
   using System;
   using Bach.Model.Instruments;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   [ TestClass ]
   public class BassTest
   {
      [ TestMethod ]
      public void TestConstructor()
      {
         var bass = new Bass();
         Assert.AreEqual(bass.Name, "Bass");
         Assert.AreEqual(bass.StringCount, 4);
         Assert.IsNotNull(bass.Tunings);
         Assert.AreNotEqual(bass.Tunings.Count, 0);
      }

      [ TestMethod ]
      public void EqualsContractTest()
      {
         object x = new Bass();
         object y = new Bass();
         object z = new Bass();

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
         var x = new Bass();
         var y = new Bass();
         var z = new Bass();

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
         object a = new Bass();
         object b = new Guitar();
         Assert.IsFalse(a.Equals(b));
         Assert.IsFalse(b.Equals(a));
         Assert.IsFalse(Equals(a, b));
         Assert.IsFalse(Equals(b, a));
      }

      [ TestMethod ]
      public void TypeSafeEqualsFailsWithDifferentTypeTest()
      {
         StringedInstrument a = new Bass();
         StringedInstrument b = new Guitar();
         Assert.IsFalse(a.Equals(b));
         Assert.IsFalse(b.Equals(a));
         Assert.IsFalse(Equals(a, b));
         Assert.IsFalse(Equals(b, a));
      }

      [ TestMethod ]
      public void EqualsFailsWithNullTest()
      {
         object actual = new Bass();
         Assert.IsFalse(actual.Equals(null));
      }

      [ TestMethod ]
      public void TypeSafeEqualsFailsWithNullTest()
      {
         var actual = new Bass();
         Assert.IsFalse(actual.Equals(null));
      }

      [ TestMethod ]
      public void EqualsSucceedsWithSameObjectTest()
      {
         var actual = new Bass();
         Assert.IsTrue(actual.Equals(actual));
      }

      [ TestMethod ]
      public void GetHashcodeTest()
      {
         var actual = new Bass();
         var expected = new Bass();
         Assert.IsTrue(expected.Equals(actual));
         Assert.AreEqual(expected.GetHashCode(), actual.GetHashCode());
      }
   }
}
