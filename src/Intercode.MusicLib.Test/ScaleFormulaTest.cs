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
   using System.Linq;
   using System.Text;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   [ TestClass ]
   public class ScaleFormulaTest
   {
      #region Implementation

      private static void TestScale(Note root, ScaleFormula formula, Note[] expectedNotes)
      {
         Assert.IsNotNull(formula);

         var actualScale = new StringBuilder();
         actualScale.AppendFormat("{0}: ", formula.Name);

         var expectedScale = new StringBuilder();
         expectedScale.AppendFormat("{0}: ", formula.Name);

         var actualNotes = new Scale(root, formula).Take(expectedNotes.Length).ToArray();

         Assert.AreEqual(expectedNotes.Length, actualNotes.Length);

         bool needComma = false;

         for( int i = 0; i < actualNotes.Length; i++ )
         {
            Note actualNote = actualNotes[i];
            Note expectedNote = expectedNotes[i];
            Assert.AreEqual(expectedNote, actualNote);

            if( needComma )
            {
               actualScale.Append(',');
               expectedScale.Append(',');
            }
            else
               needComma = true;

            actualScale.Append(actualNote);
            expectedScale.Append(expectedNote);
         }

         Assert.AreEqual(expectedScale.ToString(), actualScale.ToString());
      }

      #endregion

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
         TestScale(root, ScaleFormula.Major, Note.ParseArray("C,D,E,F,G,A,B").ToArray());
         TestScale(root, ScaleFormula.NaturalMinor, Note.ParseArray("C,D,Eb,F,G,Ab,Bb").ToArray());
         TestScale(root, ScaleFormula.HarmonicMinor, Note.ParseArray("C,D,Eb,F,G,Ab,B").ToArray());
         TestScale(root, ScaleFormula.MelodicMinor, Note.ParseArray("C,D,Eb,F,G,A,B").ToArray());
         TestScale(root, ScaleFormula.Diminished, Note.ParseArray("C,D,Eb,F,Gb,G#,A,B").ToArray());
         TestScale(root, ScaleFormula.Polytonal, Note.ParseArray("C,Db,Eb,E,F#,G,A,Bb").ToArray());
         TestScale(root, ScaleFormula.Pentatonic, Note.ParseArray("C,D,E,G,A").ToArray());
         TestScale(root, ScaleFormula.Blues, Note.ParseArray("C,Eb,F,Gb,G,Bb").ToArray());
         TestScale(root, ScaleFormula.Gospel, Note.ParseArray("C,D,Eb,E,G,A").ToArray());
      }
   }
}
