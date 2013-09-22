// 
//   AbsoluteFormulaTest.cs: 
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
   using System;
   using System.Linq;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   [ TestClass ]
   public class AbsoluteFormulaTest
   {
      [ TestMethod ]
      public void ConstructorTest()
      {
         var actual = new AbsoluteFormula(1, 2, 3);
         Assert.AreEqual(3, actual.Count);
         CollectionAssert.AreEqual(new[] { 1, 2, 3 }, actual.Intervals);
      }

      [ TestMethod ]
      public void EnumerableTest()
      {
         var actual = new AbsoluteFormula(1, 2, 3);
         Assert.AreEqual(3, actual.Count);
         Assert.IsTrue(new[] { 1, 2, 3 }.SequenceEqual(actual));
      }

      [ TestMethod ]
      public void EqualsContractTest()
      {
         object x = new AbsoluteFormula(1, 2, 3);
         object y = new AbsoluteFormula(1, 2, 3);
         object z = new AbsoluteFormula(1, 2, 3);

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
         var x = new AbsoluteFormula(1, 2, 3);
         var y = new AbsoluteFormula(1, 2, 3);
         var z = new AbsoluteFormula(1, 2, 3);

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
         object actual = new AbsoluteFormula(1, 2, 3);
         Assert.IsFalse(actual.Equals(Int32.MinValue));
      }

      [ TestMethod ]
      public void TypeSafeEqualsFailsWithDifferentTypeTest()
      {
         var actual = new AbsoluteFormula(1, 2, 3);
         Assert.IsFalse(actual.Equals(Int32.MinValue));
      }

      [ TestMethod ]
      public void EqualsFailsWithNullTest()
      {
         object actual = new AbsoluteFormula(1, 2, 3);
         Assert.IsFalse(actual.Equals(null));
      }

      [ TestMethod ]
      public void TypeSafeEqualsFailsWithNullTest()
      {
         var actual = new AbsoluteFormula(1, 2, 3);
         Assert.IsFalse(actual.Equals(null));
      }

      [ TestMethod ]
      public void EqualsSucceedsWithSameObjectTest()
      {
         var actual = new AbsoluteFormula(1, 2, 3);
         Assert.IsTrue(actual.Equals(actual));
      }

      [ TestMethod ]
      public void GetHashcodeTest()
      {
         var actual = new AbsoluteFormula(1, 2, 3);
         var expected = new AbsoluteFormula(1, 2, 3);
         Assert.IsTrue(expected.Equals(actual));
         Assert.AreEqual(expected.GetHashCode(), actual.GetHashCode());
      }
   }
}
