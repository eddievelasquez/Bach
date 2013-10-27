// 
//   ScaleTest.cs: 
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
   public class ScaleTest
   {
      [ TestMethod ]
      public void ConstructorTest()
      {
         var major = new Formula("Major", Interval.Perfect1, Interval.Major2, Interval.Major3, Interval.Perfect4,
            Interval.Perfect5, Interval.Major6, Interval.Major7);
         Assert.AreEqual("Major", major.Name);
         Assert.AreEqual(7, major.Count);
      }

      private static void TestScale(string expectedNotes, Note root, Formula formula)
      {
         var expected = NoteCollection.Parse(expectedNotes);
         var actual = new NoteCollection(new Scale(root, formula).Take(expected.Count).ToArray());
         CollectionAssert.AreEqual(expected, actual);
      }

      [ TestMethod ]
      public void GenerateScaleTest()
      {
         Note root = Note.Parse("C4");
         TestScale("C4,D4,E4,F4,G4,A4,B4", root, Formula.Major);
         TestScale("C4,D4,Eb4,F4,G4,Ab4,Bb4", root, Formula.NaturalMinor);
         TestScale("C4,D4,Eb4,F4,G4,Ab4,B4", root, Formula.HarmonicMinor);
         TestScale("C4,D4,Eb4,F4,G4,A4,B4", root, Formula.MelodicMinor);
         TestScale("C4,D4,Eb4,F4,Gb4,G#4,A4,B4", root, Formula.Diminished);
         TestScale("C4,Db4,Eb4,Fb4,F#4,G4,A4,Bb4", root, Formula.Polytonal);
         TestScale("C4,D4,E4,G4,A4", root, Formula.Pentatonic);
         TestScale("C4,Eb4,F4,Gb4,G4,Bb4", root, Formula.Blues);
         TestScale("C4,D4,Eb4,E4,G4,A4", root, Formula.Gospel);
      }

      [ TestMethod ]
      public void EqualsContractTest()
      {
         object x = new Scale(Note.Parse("C4"), Formula.Major);
         object y = new Scale(Note.Parse("C4"), Formula.Major);
         object z = new Scale(Note.Parse("C4"), Formula.Major);

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
         var x = new Scale(Note.Parse("C4"), Formula.Major);
         var y = new Scale(Note.Parse("C4"), Formula.Major);
         var z = new Scale(Note.Parse("C4"), Formula.Major);

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
         object actual = new Scale(Note.Parse("C4"), Formula.Major);
         Assert.IsFalse(actual.Equals(Int32.MinValue));
      }

      [ TestMethod ]
      public void TypeSafeEqualsFailsWithDifferentTypeTest()
      {
         var actual = new Scale(Note.Parse("C4"), Formula.Major);
         Assert.IsFalse(actual.Equals(Int32.MinValue));
      }

      [ TestMethod ]
      public void EqualsFailsWithNullTest()
      {
         object actual = new Scale(Note.Parse("C4"), Formula.Major);
         Assert.IsFalse(actual.Equals(null));
      }

      [ TestMethod ]
      public void TypeSafeEqualsFailsWithNullTest()
      {
         var actual = new Scale(Note.Parse("C4"), Formula.Major);
         Assert.IsFalse(actual.Equals(null));
      }

      [ TestMethod ]
      public void EqualsSucceedsWithSameObjectTest()
      {
         var actual = new Scale(Note.Parse("C4"), Formula.Major);
         Assert.IsTrue(actual.Equals(actual));
      }

      [ TestMethod ]
      public void GetHashcodeTest()
      {
         var actual = new Scale(Note.Parse("C4"), Formula.Major);
         var expected = new Scale(Note.Parse("C4"), Formula.Major);
         Assert.IsTrue(expected.Equals(actual));
         Assert.AreEqual(expected.GetHashCode(), actual.GetHashCode());
      }
   }
}
