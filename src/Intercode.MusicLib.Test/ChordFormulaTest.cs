// 
//   ChordFormulaTest.cs: 
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

   /// <summary>
   ///    This is a test class for ChordFormulaTest and is intended
   ///    to contain all ChordFormulaTest Unit Tests
   /// </summary>
   [ TestClass ]
   public class ChordFormulaTest
   {
      #region Properties

      /// <summary>
      ///    Gets or sets the test context which provides
      ///    information about and functionality for the current test run.
      /// </summary>
      public TestContext TestContext { get; set; }

      #endregion

      /// <summary>
      ///    A test for ChordFormula Constructor
      /// </summary>
      [ TestMethod ]
      public void ChordFormulaConstructorTest()
      {
         const string NAME = "Name";
         const string SYMBOL = "Symbol";
         const string FORMULA = "1,2,3";
         var target = new ChordFormula(NAME, SYMBOL, FORMULA);

         Assert.AreEqual(NAME, target.Name);
         Assert.AreEqual(SYMBOL, target.Symbol);
         Assert.IsNotNull(target.Formula);
         Assert.AreEqual("1,2,3", target.Formula.ToString());
      }

      [ TestMethod ]
      public void EqualsContractTest()
      {
         object x = new ChordFormula("Name", "Symbol", "1,2,3");
         object y = new ChordFormula("Name", "Symbol", "1,2,3");
         object z = new ChordFormula("Name", "Symbol", "1,2,3");

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
         var x = new ChordFormula("Name", "Symbol", "1,2,3");
         var y = new ChordFormula("Name", "Symbol", "1,2,3");
         var z = new ChordFormula("Name", "Symbol", "1,2,3");

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
         object actual = new ChordFormula("Name", "Symbol", "1,2,3");
         Assert.IsFalse(actual.Equals(Int32.MinValue));
      }

      [ TestMethod ]
      public void TypeSafeEqualsFailsWithDifferentTypeTest()
      {
         var actual = new ChordFormula("Name", "Symbol", "1,2,3");
         Assert.IsFalse(actual.Equals(Int32.MinValue));
      }

      [ TestMethod ]
      public void EqualsFailsWithNullTest()
      {
         object actual = new ChordFormula("Name", "Symbol", "1,2,3");
         Assert.IsFalse(actual.Equals(null));
      }

      [ TestMethod ]
      public void TypeSafeEqualsFailsWithNullTest()
      {
         var actual = new ChordFormula("Name", "Symbol", "1,2,3");
         Assert.IsFalse(actual.Equals(null));
      }

      [ TestMethod ]
      public void EqualsSucceedsWithSameObjectTest()
      {
         var actual = new ChordFormula("Name", "Symbol", "1,2,3");
         Assert.IsTrue(actual.Equals(actual));
      }

      [ TestMethod ]
      public void GetHashcodeTest()
      {
         var actual = new ChordFormula("Name", "Symbol", "1,2,3");
         var expected = new ChordFormula("Name", "Symbol", "1,2,3");
         Assert.IsTrue(expected.Equals(actual));
         Assert.AreEqual(expected.GetHashCode(), actual.GetHashCode());
      }
   }
}
