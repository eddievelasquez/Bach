namespace Bach.Model.Test
{
   using System;
   using System.Linq;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   /// <summary>
   ///    This is a test class for ChordTest and is intended
   ///    to contain all ChordTest Unit Tests
   /// </summary>
   [ TestClass ]
   public class ModeTest
   {
      #region Properties

      /// <summary>
      ///    Gets or sets the test context which provides
      ///    information about and functionality for the current test run.
      /// </summary>
      public TestContext TestContext { get; set; }

      #endregion

      /// <summary>
      ///    A test for Mode Constructor
      /// </summary>
      [ TestMethod ]
      public void ModeConstructorTest()
      {
         Note root = Note.Parse("C4");
         ModeFormula formula = ModeFormula.Phrygian;
         var target = new Mode(root, formula);

         Assert.AreEqual(root, target.Root);
         Assert.AreEqual(formula, target.Formula);
         Assert.AreEqual("C Phrygian", target.Name);
         CollectionAssert.AreEqual(target.ToArray(), NoteCollection.Parse("E4,F4,G4,A4,B4,C5,D5"));
      }

      private static void TestMode(string expectedNotes, Note root, ModeFormula formula)
      {
         var expected = NoteCollection.Parse(expectedNotes);
         CollectionAssert.AreEqual(expected, new Mode(root, formula).Take(expected.Count).ToArray());
      }

      [ TestMethod ]
      public void ModesTest()
      {
         Note root = Note.Parse("C4");
         TestMode("C4,D4,E4,F4,G4,A4,B4", root, ModeFormula.Ionian);
         TestMode("D4,E4,F4,G4,A4,B4,C5", root, ModeFormula.Dorian);
         TestMode("E4,F4,G4,A4,B4,C5,D5", root, ModeFormula.Phrygian);
         TestMode("F4,G4,A4,B4,C5,D5,E5", root, ModeFormula.Lydian);
         TestMode("G4,A4,B4,C5,D5,E5,F5", root, ModeFormula.Mixolydian);
         TestMode("A4,B4,C5,D5,E5,F5,G5", root, ModeFormula.Aeolian);
         TestMode("B4,C5,D5,E5,F5,G5,A5", root, ModeFormula.Locrian);
      }

      [ TestMethod ]
      public void EqualsContractTest()
      {
         object x = new Mode(Note.Parse("C4"), ModeFormula.Dorian);
         object y = new Mode(Note.Parse("C4"), ModeFormula.Dorian);
         object z = new Mode(Note.Parse("C4"), ModeFormula.Dorian);

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
         var x = new Mode(Note.Parse("C4"), ModeFormula.Dorian);
         var y = new Mode(Note.Parse("C4"), ModeFormula.Dorian);
         var z = new Mode(Note.Parse("C4"), ModeFormula.Dorian);

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
         object actual = new Mode(Note.Parse("C4"), ModeFormula.Dorian);
         Assert.IsFalse(actual.Equals(Int32.MinValue));
      }

      [ TestMethod ]
      public void TypeSafeEqualsFailsWithDifferentTypeTest()
      {
         var actual = new Mode(Note.Parse("C4"), ModeFormula.Dorian);
         Assert.IsFalse(actual.Equals(Int32.MinValue));
      }

      [ TestMethod ]
      public void EqualsFailsWithNullTest()
      {
         object actual = new Mode(Note.Parse("C4"), ModeFormula.Dorian);
         Assert.IsFalse(actual.Equals(null));
      }

      [ TestMethod ]
      public void TypeSafeEqualsFailsWithNullTest()
      {
         var actual = new Mode(Note.Parse("C4"), ModeFormula.Dorian);
         Assert.IsFalse(actual.Equals(null));
      }

      [ TestMethod ]
      public void EqualsSucceedsWithSameObjectTest()
      {
         var actual = new Mode(Note.Parse("C4"), ModeFormula.Dorian);
         Assert.IsTrue(actual.Equals(actual));
      }

      [ TestMethod ]
      public void GetHashcodeTest()
      {
         var actual = new Mode(Note.Parse("C4"), ModeFormula.Dorian);
         var expected = new Mode(Note.Parse("C4"), ModeFormula.Dorian);
         Assert.IsTrue(expected.Equals(actual));
         Assert.AreEqual(expected.GetHashCode(), actual.GetHashCode());
      }
   }
}