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
   public class ScaleFormula: Formula
   {
      #region Constants

      public static readonly ScaleFormula Major = new ScaleFormula("Major", "1,2,3,4,5,6,7");
      public static readonly ScaleFormula NaturalMinor = new ScaleFormula("Natural Minor", "1,2,m3,4,5,m6,m7");
      public static readonly ScaleFormula HarmonicMinor = new ScaleFormula("Harmonic Minor", "1,2,m3,4,5,m6,7");
      public static readonly ScaleFormula MelodicMinor = new ScaleFormula("Melodic Minor", "1,2,m3,4,5,6,7");
      public static readonly ScaleFormula Diminished = new ScaleFormula("Diminished", "1,2,m3,4,d5,A5,6,7");
      public static readonly ScaleFormula Polytonal = new ScaleFormula("Polytonal", "1,m2,m3,d4,A4,5,6,m7");
      public static readonly ScaleFormula WholeTone = new ScaleFormula("Whole Tone", "1,2,3,A4,A5,A6");
      public static readonly ScaleFormula Pentatonic = new ScaleFormula("Pentatonic", "1,2,3,5,6");
      public static readonly ScaleFormula MinorPentatonic = new ScaleFormula("Minor Pentatonic", "1,m3,4,5,m7");
      public static readonly ScaleFormula Blues = new ScaleFormula("Blues", "1,m3,4,d5,5,m7");
      public static readonly ScaleFormula Gospel = new ScaleFormula("Gospel", "1,2,m3,3,5,6");

      #endregion

      #region Construction

      public ScaleFormula(string name, params Interval[] intervals)
         : base(name, intervals)
      {
      }

      public ScaleFormula(string name, string formula)
         : base(name, formula)
      {
      }

      #endregion
   }
}
