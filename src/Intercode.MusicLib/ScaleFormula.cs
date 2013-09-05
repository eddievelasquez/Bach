// 
//   ScaleFormula.cs: 
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

namespace Intercode.MusicLib
{
   using System;
   using System.Collections.Generic;
   using System.Diagnostics.Contracts;
   using System.Linq;
   using System.Text.RegularExpressions;

   public class ScaleFormula
   {
      #region Constants

      private static readonly Regex s_formulaRx = new Regex("(\\d\\d?)(bb?|##?)?(?:,)?", RegexOptions.Singleline);

      public static readonly ScaleFormula Major = new ScaleFormula("Major", 2, 2, 1, 2, 2, 2, 1);
      public static readonly ScaleFormula NaturalMinor = new ScaleFormula("Natural Minor", "1,2,3b,4,5,6b,7b");
      public static readonly ScaleFormula HarmonicMinor = new ScaleFormula("Harmonic Minor", "1,2,3b,4,5,6b,7");
      public static readonly ScaleFormula MelodicMinor = new ScaleFormula("Melodic Minor", "1,2,3b,4,5,6,7");
      public static readonly ScaleFormula Diminished = new ScaleFormula("Diminished", "1,2,3b,4,5b,5#,6,7");
      public static readonly ScaleFormula Polytonal = new ScaleFormula("Polytonal", "1,2b,3b,4b,4#,5,6,7b");
      public static readonly ScaleFormula Pentatonic = new ScaleFormula("Pentatonic", "1,2,3,5,6");
      public static readonly ScaleFormula MinorPentatonic = new ScaleFormula("Minor Pentatonic", "1,3b,4,5,7b");
      public static readonly ScaleFormula Blues = new ScaleFormula("Blues", "1,3b,4,5b,5,7b");
      public static readonly ScaleFormula Gospel = new ScaleFormula("Gospel", "1,2,3b,3,6bb,6");

      #endregion

      #region Construction

      public ScaleFormula(string name, params int[] intervals)
      {
         Contract.Requires<ArgumentNullException>(name != null, "name");
         Contract.Requires<ArgumentException>(name.Length > 0, "name");
         Contract.Requires<ArgumentException>(intervals.Length > 0, "intervals");

         Name = name;
         Intervals = intervals;
      }

      public ScaleFormula(string name, string formula)
      {
         Contract.Requires<ArgumentNullException>(name != null, "name");
         Contract.Requires<ArgumentException>(name.Length > 0, "name");

         Name = name;
         Values = Parse(formula);
      }

      public ScaleFormula(string name, params ScaleFormulaValue[] values)
      {
         Contract.Requires<ArgumentNullException>(name != null, "name");
         Contract.Requires<ArgumentException>(name.Length > 0, "name");
         Contract.Requires<ArgumentException>(values.Length > 0, "values");

         Name = name;
         Values = values;
      }

      #endregion

      #region Properties

      public String Name { get; private set; }

      public Int32 Count
      {
         get { return Intervals.Length; }
      }

      public Int32[] Intervals { get; private set; }
      public ScaleFormulaValue[] Values { get; private set; }

      #endregion

      #region Overrides

      public override string ToString()
      {
         return Name;
      }

      #endregion

      #region Implementation

      private IEnumerable<Note> GenerateScaleWithIntervals(Note root)
      {
         int index = 0;
         Note current = root;

         while( true )
         {
            yield return current;

            index %= Intervals.Length;
            current += Intervals[index++];
         }
      }

      private IEnumerable<Note> GenerateScaleWithValues(Note root)
      {
         var majorScale = Major.GenerateScale(root);
         foreach( var value in Values )
         {
            var note = majorScale.ElementAt(value.Number - 1);

            if( value.Accidental != Accidental.Natural )
               note = note.ApplyAccidental(value.Accidental);

            yield return note;
         }
      }

      #endregion

      public IEnumerable<Note> GenerateScale(Note root)
      {
         Contract.Requires<ArgumentNullException>(root != null, "root");

         if( Intervals != null )
            return GenerateScaleWithIntervals(root);

         return GenerateScaleWithValues(root);
      }

      private static ScaleFormulaValue[] Parse(string s)
      {
         Contract.Requires<ArgumentNullException>(s != null, "s");
         Contract.Requires<ArgumentException>(s.Length > 0, "s");

         var matches = s_formulaRx.Matches(s);
         var values = new List<ScaleFormulaValue>(matches.Count);
         values.AddRange(from Match match in matches
            select
               new ScaleFormulaValue(int.Parse(match.Groups[1].Value), AccidentalExtensions.Parse(match.Groups[2].Value)));

         return values.ToArray();
      }
   }
}
