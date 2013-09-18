// 
//   FormulaStepTest.cs: 
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
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   [ TestClass ]
   public class FormulaStepTest
   {
      [ TestMethod ]
      public void EqualsContractTest()
      {
         object x = new FormulaStep(2, Accidental.Sharp);
         object y = new FormulaStep(2, Accidental.Sharp);
         object z = new FormulaStep(2, Accidental.Sharp);

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
         var x = new FormulaStep(2, Accidental.Sharp);
         var y = new FormulaStep(2, Accidental.Sharp);
         var z = new FormulaStep(2, Accidental.Sharp);

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
         object actual = new FormulaStep(2, Accidental.Sharp);
         Assert.IsFalse(actual.Equals(Int32.MinValue));
      }

      [ TestMethod ]
      public void TypeSafeEqualsFailsWithDifferentTypeTest()
      {
         var actual = new FormulaStep(2, Accidental.Sharp);
         Assert.IsFalse(actual.Equals(Int32.MinValue));
      }

      [ TestMethod ]
      public void EqualsFailsWithNullTest()
      {
         object actual = new FormulaStep(2, Accidental.Sharp);
         Assert.IsFalse(actual.Equals(null));
      }

      [ TestMethod ]
      public void TypeSafeEqualsFailsWithNullTest()
      {
         var actual = new FormulaStep(2, Accidental.Sharp);
         Assert.IsFalse(actual.Equals(null));
      }

      [ TestMethod ]
      public void EqualsSucceedsWithSameObjectTest()
      {
         var actual = new FormulaStep(2, Accidental.Sharp);
         Assert.IsTrue(actual.Equals(actual));
      }

      [ TestMethod ]
      public void GetHashcodeTest()
      {
         var actual = new FormulaStep(2, Accidental.Sharp);
         var expected = new FormulaStep(2, Accidental.Sharp);
         Assert.IsTrue(expected.Equals(actual));
         Assert.AreEqual(expected.GetHashCode(), actual.GetHashCode());
      }

      [ TestMethod ]
      public void EqualitySucceedsWithTwoObjectsTest()
      {
         var lhs = new FormulaStep(2, Accidental.Sharp);
         var rhs = new FormulaStep(2, Accidental.Sharp);
         Assert.IsTrue(lhs == rhs);
      }

      [ TestMethod ]
      public void EqualitySucceedsWithSameObjectTest()
      {
#pragma warning disable 1718
         var lhs = new FormulaStep(2, Accidental.Sharp);
         Assert.IsTrue(lhs == lhs);
#pragma warning restore 1718
      }

      [ TestMethod ]
      public void EqualityFailsWithNullTest()
      {
         var lhs = new FormulaStep(2, Accidental.Sharp);
         Assert.IsFalse(lhs == null);
      }

      [ TestMethod ]
      public void InequalitySucceedsWithTwoObjectsTest()
      {
         var lhs = new FormulaStep(2, Accidental.Sharp);
         var rhs = new FormulaStep(3, Accidental.Sharp);
         Assert.IsTrue(lhs != rhs);
      }

      [ TestMethod ]
      public void InequalityFailsWithSameObjectTest()
      {
#pragma warning disable 1718
         var lhs = new FormulaStep(2, Accidental.Sharp);
         Assert.IsFalse(lhs != lhs);
#pragma warning restore 1718
      }
   }
}
