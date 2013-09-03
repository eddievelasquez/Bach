// 
//   ScaleFormulaTest.cs: 
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
   using System.Collections.Generic;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   [ TestClass ]
   public class ScaleFormulaTest
   {
      #region Implementation

      private static void TestScale(Note root, ScaleFormula formula, IEnumerable<Note> expectedNotes)
      {
         Assert.IsNotNull(formula);

         var actualNotes = formula.GenerateScale(root).GetEnumerator();
         foreach( var expectedNote in expectedNotes )
         {
            Assert.IsTrue(actualNotes.MoveNext());
            Assert.AreEqual(expectedNote, actualNotes.Current);
         }
      }

      #endregion

      #region Tests

      [ TestMethod ]
      public void ConstructorTest()
      {
         var major = new ScaleFormula("Major", 2, 2, 1, 2, 2, 2, 2);
         Assert.AreEqual("Major", major.Name);
         Assert.AreEqual(7, major.Count);
      }

      [ TestMethod ]
      public void GetNotesTest()
      {
         var root = Note.Create(Tone.C, Accidental.Natural, 4);
         TestScale(root, ScaleFormula.Major, Note.ParseArray("C,D,E,F,G,A,B"));
         TestScale(root, ScaleFormula.NaturalMinor, Note.ParseArray("C,D,Eb,F,G,Ab,Bb"));
         TestScale(root, ScaleFormula.HarmonicMinor, Note.ParseArray("C,D,Eb,F,G,Ab,B"));
         TestScale(root, ScaleFormula.MelodicMinor, Note.ParseArray("C,D,Eb,F,G,A,B"));
         TestScale(root, ScaleFormula.Diminished, Note.ParseArray("C,D,Eb,F,Gb,G#,A,B"));
         TestScale(root, ScaleFormula.Polytonal, Note.ParseArray("C,Db,Eb,Fb,F#,G,A,Bb"));
         TestScale(root, ScaleFormula.Pentatonic, Note.ParseArray("C,D,E,G,A"));
         TestScale(root, ScaleFormula.Blues, Note.ParseArray("C,Eb,F,Gb,G,Bb"));
         TestScale(root, ScaleFormula.Gospel, Note.ParseArray("C,D,Eb,E,G,A"));
      }

      #endregion
   }
}
