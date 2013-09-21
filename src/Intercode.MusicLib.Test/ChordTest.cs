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
         CollectionAssert.AreEqual(target.Notes, NoteCollection.Parse("C,Eb,G"));
      }

      private static void TestChord(string expectedNotes, Note root, ChordFormula formula)
      {
         var expected = NoteCollection.Parse(expectedNotes);
         CollectionAssert.AreEqual(expected, new Chord(root, formula).Take(expected.Count).ToArray());
      }

      [ TestMethod ]
      public void ChordsTest()
      {
         Note root = Note.Parse("C4");
         TestChord("C,E,G", root, ChordFormula.Major);
         TestChord("C,E,G,B", root, ChordFormula.Major7);
         TestChord("C,E,G,B,D", root, ChordFormula.Major9);
         TestChord("C,E,G,B,D,F", root, ChordFormula.Major11);
         TestChord("C,E,G,B,D,F,A", root, ChordFormula.Major13);
         TestChord("C,Eb,G", root, ChordFormula.Minor);
         TestChord("C,Eb,G,Bb", root, ChordFormula.Minor7);
         TestChord("C,Eb,G,Bb,D", root, ChordFormula.Minor9);
         TestChord("C,Eb,G,Bb,D,F", root, ChordFormula.Minor11);
         TestChord("C,Eb,G,Bb,D,F,A", root, ChordFormula.Minor13);
         TestChord("C,E,G,Bb", root, ChordFormula.Dominant7);
         TestChord("C,E,G,Bb,D", root, ChordFormula.Dominant9);
         TestChord("C,E,G,Bb,D,F", root, ChordFormula.Dominant11);
         TestChord("C,E,G,Bb,D,F,A", root, ChordFormula.Dominant13);
         TestChord("C,E,G,A,D", root, ChordFormula.SixNine);
         TestChord("C,E,G,D", root, ChordFormula.AddNine);
         TestChord("C,Eb,Gb", root, ChordFormula.Diminished);
         TestChord("C,Eb,Gb,Bbb", root, ChordFormula.Diminished7);
         TestChord("C,Eb,Gb,Bb", root, ChordFormula.HalfDiminished);
         TestChord("C,E,G#", root, ChordFormula.Augmented);
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

      [ TestMethod ]
      public void InvertTest()
      {
         var c4 = new Chord(Note.Parse("C4"), ChordFormula.Major);
         var firstInversion = NoteCollection.Parse("E4,G4,C5");
         var actual = c4.Invert(1);
         Assert.IsNotNull(actual);
         CollectionAssert.AreEqual(firstInversion, actual.Notes);

         var secondInversion = NoteCollection.Parse("G4,C5,E5");
         actual = c4.Invert(2);
         Assert.IsNotNull(actual);
         CollectionAssert.AreEqual(secondInversion, actual.Notes);

         var thirdInversion = NoteCollection.Parse("C5,E5,G5");
         actual = c4.Invert(3);
         Assert.IsNotNull(actual);
         CollectionAssert.AreEqual(thirdInversion, actual.Notes);
      }
   }
}
