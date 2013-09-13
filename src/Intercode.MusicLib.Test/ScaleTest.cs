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
   using System.Text;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   [ TestClass ]
   public class ScaleFormulaTest
   {
      #region Implementation

      private static void TestScale(Note root, ScaleFormula formula, IEnumerable<Note> expectedNotes)
      {
         Assert.IsNotNull(formula);

         var actualScale = new StringBuilder();
         actualScale.AppendFormat("{0}: ", formula.Name);

         var expectedScale = new StringBuilder();
         expectedScale.AppendFormat("{0}: ", formula.Name);

         var actualNotes = formula.Generate(root).GetEnumerator();
         bool needComma = false;

         foreach( var expectedNote in expectedNotes )
         {
            Assert.IsTrue(actualNotes.MoveNext());
            Assert.AreEqual(expectedNote, actualNotes.Current);

            if( needComma )
            {
               actualScale.Append(',');
               expectedScale.Append(',');
            }
            else
               needComma = true;

            actualScale.Append(actualNotes.Current);
            expectedScale.Append(expectedNote);
         }

         Assert.AreEqual(expectedScale.ToString(), actualScale.ToString());
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
      public void GenerateScaleTest()
      {
         var root = Note.Create(Tone.C, Accidental.Natural, 4);
         TestScale(root, ScaleFormula.Major, Note.ParseArray("C,D,E,F,G,A,B"));
         TestScale(root, ScaleFormula.NaturalMinor, Note.ParseArray("C,D,Eb,F,G,Ab,Bb"));
         TestScale(root, ScaleFormula.HarmonicMinor, Note.ParseArray("C,D,Eb,F,G,Ab,B"));
         TestScale(root, ScaleFormula.MelodicMinor, Note.ParseArray("C,D,Eb,F,G,A,B"));
         TestScale(root, ScaleFormula.Diminished, Note.ParseArray("C,D,Eb,F,Gb,G#,A,B"));
         TestScale(root, ScaleFormula.Polytonal, Note.ParseArray("C,Db,Eb,E,F#,G,A,Bb"));
         TestScale(root, ScaleFormula.Pentatonic, Note.ParseArray("C,D,E,G,A"));
         TestScale(root, ScaleFormula.Blues, Note.ParseArray("C,Eb,F,Gb,G,Bb"));
         TestScale(root, ScaleFormula.Gospel, Note.ParseArray("C,D,Eb,E,G,A"));
      }

      #endregion
   }
}
