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

   public class Scale
   {
      #region Constants

      public static readonly Scale Major = new Scale("Major", 2, 2, 1, 2, 2, 2, 1);
      public static readonly Scale NaturalMinor = new Scale("Natural Minor", "1,2,3b,4,5,6b,7b");
      public static readonly Scale HarmonicMinor = new Scale("Harmonic Minor", "1,2,3b,4,5,6b,7");
      public static readonly Scale MelodicMinor = new Scale("Melodic Minor", "1,2,3b,4,5,6,7");
      public static readonly Scale Diminished = new Scale("Diminished", "1,2,3b,4,5b,5#,6,7");
      public static readonly Scale Polytonal = new Scale("Polytonal", "1,2b,3b,4b,4#,5,6,7b");
      public static readonly Scale Pentatonic = new Scale("Pentatonic", "1,2,3,5,6");
      public static readonly Scale MinorPentatonic = new Scale("Minor Pentatonic", "1,3b,4,5,7b");
      public static readonly Scale Blues = new Scale("Blues", "1,3b,4,5b,5,7b");
      public static readonly Scale Gospel = new Scale("Gospel", "1,2,3b,3,6bb,6");

      #endregion

      #region Construction

      public Scale(string name, params int[] intervals)
      {
         Contract.Requires<ArgumentNullException>(name != null, "name");
         Contract.Requires<ArgumentException>(name.Length > 0, "name");
         Contract.Requires<ArgumentException>(intervals.Length > 0, "intervals");

         Name = name;
         Intervals = intervals;
      }

      public Scale(string name, string formula)
      {
         Contract.Requires<ArgumentNullException>(name != null, "name");
         Contract.Requires<ArgumentException>(name.Length > 0, "name");

         Name = name;
         Formula = Formula.Parse(formula);
      }

      public Scale(string name, Formula formula)
      {
         Contract.Requires<ArgumentNullException>(name != null, "name");
         Contract.Requires<ArgumentException>(name.Length > 0, "name");
         Contract.Requires<ArgumentNullException>(formula != null, "formula");

         Name = name;
         Formula = formula;
      }

      #endregion

      #region Properties

      public String Name { get; private set; }

      public Int32 Count
      {
         get { return Formula != null ? Formula.Count : Intervals.Length; }
      }

      public Int32[] Intervals { get; private set; }
      public Formula Formula { get; private set; }

      #endregion

      #region Overrides

      public override string ToString()
      {
         return Name;
      }

      #endregion

      #region Public Methods

      public IEnumerable<Note> GenerateScale(Note root)
      {
         Contract.Requires<ArgumentNullException>(root != null, "root");

         if( Intervals != null )
            return GenerateScaleWithIntervals(root);

         return GenerateScaleWithValues(root);
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
         foreach( var step in Formula )
         {
            var note = majorScale.ElementAt(step.Interval - 1);

            if( step.Accidental != Accidental.Natural )
               note = note.ApplyAccidental(step.Accidental);

            yield return note;
         }
      }

      #endregion
   }
}
