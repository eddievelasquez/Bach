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

namespace Intercode.MusicLib.Test
{
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
   }
}
