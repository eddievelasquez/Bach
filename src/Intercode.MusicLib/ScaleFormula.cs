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

namespace Bach.Model
{
   using System;
   using System.Collections.Generic;
   using System.Diagnostics.Contracts;

   public class ScaleFormula: IEquatable<ScaleFormula>
   {
      public static readonly ScaleFormula Major = new ScaleFormula("Major", 2, 2, 1, 2, 2, 2, 1);
      public static readonly ScaleFormula NaturalMinor = new ScaleFormula("Natural Minor", "1,2,b3,4,5,b6,b7");
      public static readonly ScaleFormula HarmonicMinor = new ScaleFormula("Harmonic Minor", "1,2,b3,4,5,b6,7");
      public static readonly ScaleFormula MelodicMinor = new ScaleFormula("Melodic Minor", "1,2,b3,4,5,6,7");
      public static readonly ScaleFormula Diminished = new ScaleFormula("Diminished", "1,2,b3,4,b5,5#,6,7");
      public static readonly ScaleFormula Polytonal = new ScaleFormula("Polytonal", "1,b2,b3,b4,4#,5,6,b7");
      public static readonly ScaleFormula Pentatonic = new ScaleFormula("Pentatonic", "1,2,3,5,6");
      public static readonly ScaleFormula MinorPentatonic = new ScaleFormula("Minor Pentatonic", "1,b3,4,5,b7");
      public static readonly ScaleFormula Blues = new ScaleFormula("Blues", "1,b3,4,b5,5,b7");
      public static readonly ScaleFormula Gospel = new ScaleFormula("Gospel", "1,2,b3,3,bb6,6");
      private static readonly StringComparer s_comparer = StringComparer.CurrentCultureIgnoreCase;

      #region Construction

      public ScaleFormula(string name, params int[] intervals)
         : this(name, new AbsoluteFormula(intervals))
      {
      }

      public ScaleFormula(string name, string formula)
         : this(name, RelativeFormula.Parse(formula))
      {
      }

      public ScaleFormula(string name, IFormula formula)
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
         get { return Formula.Count; }
      }

      public IFormula Formula { get; private set; }

      #endregion

      #region IEquatable<ScaleFormula> Members

      public bool Equals(ScaleFormula other)
      {
         if( ReferenceEquals(other, this) )
            return true;

         if( ReferenceEquals(other, null) )
            return false;

         return s_comparer.Equals(Name, other.Name) && Formula.Equals(other.Formula);
      }

      #endregion

      #region Overrides

      public override string ToString()
      {
         return Name;
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

      public IEnumerable<Note> Generate(Note root)
      {
         return Formula.Generate(root);
      }
   }
}
