// 
//   ChordTest.cs: 
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
   using System.Linq;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   /// <summary>
   ///    This is a test class for ChordTest and is intended
   ///    to contain all ChordTest Unit Tests
   /// </summary>
   [ TestClass ]
   public class ChordTest
   {
      #region Properties

      /// <summary>
      ///    Gets or sets the test context which provides
      ///    information about and functionality for the current test run.
      /// </summary>
      public TestContext TestContext { get; set; }

      #endregion

      /// <summary>
      ///    A test for Chord Constructor
      /// </summary>
      [ TestMethod ]
      public void ChordConstructorTest()
      {
         Note root = Note.Parse("C4");
         ChordFormula formula = ChordFormula.Minor;
         var target = new Chord(root, formula);

         Assert.AreEqual(root, target.Root);
         Assert.AreEqual(formula, target.Formula);
         Assert.AreEqual("Cm", target.Name);
         CollectionAssert.AreEqual(Note.ParseArray("C,Eb,G").ToArray(), target.Notes);
      }

      [ TestMethod ]
      public void EqualsContractTest()
      {
         object x = new Chord(Note.Parse("C4"), ChordFormula.Major);
         object y = new Chord(Note.Parse("C4"), ChordFormula.Major);
         object z = new Chord(Note.Parse("C4"), ChordFormula.Major);

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
         var x = new Chord(Note.Parse("C4"), ChordFormula.Major);
         var y = new Chord(Note.Parse("C4"), ChordFormula.Major);
         var z = new Chord(Note.Parse("C4"), ChordFormula.Major);

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
         object actual = new Chord(Note.Parse("C4"), ChordFormula.Major);
         Assert.IsFalse(actual.Equals(Int32.MinValue));
      }

      [ TestMethod ]
      public void TypeSafeEqualsFailsWithDifferentTypeTest()
      {
         var actual = new Chord(Note.Parse("C4"), ChordFormula.Major);
         Assert.IsFalse(actual.Equals(Int32.MinValue));
      }

      [ TestMethod ]
      public void EqualsFailsWithNullTest()
      {
         object actual = new Chord(Note.Parse("C4"), ChordFormula.Major);
         Assert.IsFalse(actual.Equals(null));
      }

      [ TestMethod ]
      public void TypeSafeEqualsFailsWithNullTest()
      {
         var actual = new Chord(Note.Parse("C4"), ChordFormula.Major);
         Assert.IsFalse(actual.Equals(null));
      }

      [ TestMethod ]
      public void EqualsSucceedsWithSameObjectTest()
      {
         var actual = new Chord(Note.Parse("C4"), ChordFormula.Major);
         Assert.IsTrue(actual.Equals(actual));
      }

      [ TestMethod ]
      public void GetHashcodeTest()
      {
         var actual = new Chord(Note.Parse("C4"), ChordFormula.Major);
         var expected = new Chord(Note.Parse("C4"), ChordFormula.Major);
         Assert.IsTrue(expected.Equals(actual));
         Assert.AreEqual(expected.GetHashCode(), actual.GetHashCode());
      }
   }
}
