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

   public class ScaleFormula: IEquatable<ScaleFormula>
   {
      #region Constants

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
      private static readonly StringComparer s_comparer = StringComparer.CurrentCulture;

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
         Formula = Formula.Parse(formula);
      }

      public ScaleFormula(string name, Formula formula)
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

      #region Public Methods

      public IEnumerable<Note> Generate(Note root)
      {
         if( Formula != null )
            return Formula.Generate(root);

         return GenerateWithIntervals(root, Intervals);
      }

      #endregion

      #region Overrides

      public override string ToString()
      {
         return Name;
      }

      #endregion

      #region IEquatable<ScaleFormula> Implementation

      public bool Equals(ScaleFormula other)
      {
         if( ReferenceEquals(other, this) )
            return true;

         if( ReferenceEquals(other, null) )
            return false;

         return s_comparer.Equals(Name, other.Name) && Formula.Equals(other.Formula);
      }

      public override bool Equals(object other)
      {
         if( ReferenceEquals(other, this) )
            return true;

         if( ReferenceEquals(other, null) || other.GetType() != GetType() )
            return false;

         return Equals((ScaleFormula)other);
      }

      public override int GetHashCode()
      {
         return Name.GetHashCode();
      }

      #endregion

      #region Implementation

      private static IEnumerable<Note> GenerateWithIntervals(Note root, IList<int> intervals)
      {
         Contract.Requires<ArgumentNullException>(root != null, "root");
         Contract.Requires<ArgumentNullException>(intervals != null, "intervals");

         int index = 0;
         Note current = root;

         while( true )
         {
            yield return current;

            index %= intervals.Count;
            current += intervals[index++];
         }
      }

      #endregion
   }
}
