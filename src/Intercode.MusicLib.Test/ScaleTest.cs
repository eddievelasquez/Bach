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
         var major = new ScaleFormula("Major", 2, 2, 1, 2, 2, 2, 2);
         Assert.AreEqual("Major", major.Name);
         Assert.AreEqual(7, major.Count);
      }

      private static void TestScale(string expectedNotes, Note root, ScaleFormula formula)
      {
         var expected = Note.ParseArray(expectedNotes).ToArray();
         CollectionAssert.AreEqual(expected, new Scale(root, formula).Take(expected.Length).ToArray());
      }

      [ TestMethod ]
      public void GenerateScaleTest()
      {
         Note root = Note.Parse("C4");
         TestScale("C,D,E,F,G,A,B", root, ScaleFormula.Major);
         TestScale("C,D,Eb,F,G,Ab,Bb", root, ScaleFormula.NaturalMinor);
         TestScale("C,D,Eb,F,G,Ab,B", root, ScaleFormula.HarmonicMinor);
         TestScale("C,D,Eb,F,G,A,B", root, ScaleFormula.MelodicMinor);
         TestScale("C,D,Eb,F,Gb,G#,A,B", root, ScaleFormula.Diminished);
         TestScale("C,Db,Eb,E,F#,G,A,Bb", root, ScaleFormula.Polytonal);
         TestScale("C,D,E,G,A", root, ScaleFormula.Pentatonic);
         TestScale("C,Eb,F,Gb,G,Bb", root, ScaleFormula.Blues);
         TestScale("C,D,Eb,E,G,A", root, ScaleFormula.Gospel);
      }

      [ TestMethod ]
      public void EqualsContractTest()
      {
         object x = new Scale(Note.Parse("C4"), ScaleFormula.Major);
         object y = new Scale(Note.Parse("C4"), ScaleFormula.Major);
         object z = new Scale(Note.Parse("C4"), ScaleFormula.Major);

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
         var x = new Scale(Note.Parse("C4"), ScaleFormula.Major);
         var y = new Scale(Note.Parse("C4"), ScaleFormula.Major);
         var z = new Scale(Note.Parse("C4"), ScaleFormula.Major);

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
         object actual = new Scale(Note.Parse("C4"), ScaleFormula.Major);
         Assert.IsFalse(actual.Equals(Int32.MinValue));
      }

      [ TestMethod ]
      public void TypeSafeEqualsFailsWithDifferentTypeTest()
      {
         var actual = new Scale(Note.Parse("C4"), ScaleFormula.Major);
         Assert.IsFalse(actual.Equals(Int32.MinValue));
      }

      [ TestMethod ]
      public void EqualsFailsWithNullTest()
      {
         object actual = new Scale(Note.Parse("C4"), ScaleFormula.Major);
         Assert.IsFalse(actual.Equals(null));
      }

      [ TestMethod ]
      public void TypeSafeEqualsFailsWithNullTest()
      {
         var actual = new Scale(Note.Parse("C4"), ScaleFormula.Major);
         Assert.IsFalse(actual.Equals(null));
      }

      [ TestMethod ]
      public void EqualsSucceedsWithSameObjectTest()
      {
         var actual = new Scale(Note.Parse("C4"), ScaleFormula.Major);
         Assert.IsTrue(actual.Equals(actual));
      }

      [ TestMethod ]
      public void GetHashcodeTest()
      {
         var actual = new Scale(Note.Parse("C4"), ScaleFormula.Major);
         var expected = new Scale(Note.Parse("C4"), ScaleFormula.Major);
         Assert.IsTrue(expected.Equals(actual));
         Assert.AreEqual(expected.GetHashCode(), actual.GetHashCode());
      }
   }
}
