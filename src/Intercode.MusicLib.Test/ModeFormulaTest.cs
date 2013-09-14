namespace Intercode.MusicLib.Test
{
   using System;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   /// <summary>
   ///    This is a test class for ChordFormulaTest and is intended
   ///    to contain all ChordFormulaTest Unit Tests
   /// </summary>
   [ TestClass ]
   public class ModeFormulaTest
   {
      #region Properties

      /// <summary>
      ///    Gets or sets the test context which provides
      ///    information about and functionality for the current test run.
      /// </summary>
      public TestContext TestContext { get; set; }

      #endregion

      /// <summary>
      ///    A test for ModeFormula Constructor
      /// </summary>
      [ TestMethod ]
      public void ChordFormulaConstructorTest()
      {
         const string NAME = "Name";
         const int TONIC = 2;
         var target = new ModeFormula(NAME, TONIC);

         Assert.AreEqual(NAME, target.Name);
         Assert.AreEqual(TONIC, target.Tonic);
      }

      [ TestMethod ]
      public void EqualsContractTest()
      {
         object x = new ModeFormula("Name", 2);
         object y = new ModeFormula("Name", 2);
         object z = new ModeFormula("Name", 2);

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
         var x = new ModeFormula("Name", 2);
         var y = new ModeFormula("Name", 2);
         var z = new ModeFormula("Name", 2);

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
         object actual = new ModeFormula("Name", 2);
         Assert.IsFalse(actual.Equals(Int32.MinValue));
      }

      [ TestMethod ]
      public void TypeSafeEqualsFailsWithDifferentTypeTest()
      {
         var actual = new ModeFormula("Name", 2);
         Assert.IsFalse(actual.Equals(Int32.MinValue));
      }

      [ TestMethod ]
      public void EqualsFailsWithNullTest()
      {
         object actual = new ModeFormula("Name", 2);
         Assert.IsFalse(actual.Equals(null));
      }

      [ TestMethod ]
      public void TypeSafeEqualsFailsWithNullTest()
      {
         var actual = new ModeFormula("Name", 2);
         Assert.IsFalse(actual.Equals(null));
      }

      [ TestMethod ]
      public void EqualsSucceedsWithSameObjectTest()
      {
         var actual = new ModeFormula("Name", 2);
         Assert.IsTrue(actual.Equals(actual));
      }

      [ TestMethod ]
      public void GetHashcodeTest()
      {
         var actual = new ModeFormula("Name", 2);
         var expected = new ModeFormula("Name", 2);
         Assert.IsTrue(expected.Equals(actual));
         Assert.AreEqual(expected.GetHashCode(), actual.GetHashCode());
      }
   }
}