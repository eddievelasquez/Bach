// 
//   ChordFormula.cs: 
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
   using System.Diagnostics.Contracts;

   public class ChordFormula: Formula
   {
      #region Constants

      public static readonly ChordFormula Major = new ChordFormula("Major", "", "1,3,5");
      public static readonly ChordFormula Major7 = new ChordFormula("Major Seventh", "M7", "1,3,5,7");
      public static readonly ChordFormula Major9 = new ChordFormula("Major Ninth", "M9", "1,3,5,7,9");
      public static readonly ChordFormula Major11 = new ChordFormula("Major Eleventh", "M11", "1,3,5,7,9,11");
      public static readonly ChordFormula Major13 = new ChordFormula("Major Thirteenth", "M13", "1,3,5,7,9,11,13");
      public static readonly ChordFormula Minor = new ChordFormula("Minor", "m", "1,m3,5");
      public static readonly ChordFormula Minor7 = new ChordFormula("Minor Seventh", "m7", "1,m3,5,m7");
      public static readonly ChordFormula Minor9 = new ChordFormula("Minor Ninth", "m9", "1,m3,5,m7,9");
      public static readonly ChordFormula Minor11 = new ChordFormula("Minor Eleventh", "m11", "1,m3,5,m7,9,11");
      public static readonly ChordFormula Minor13 = new ChordFormula("Minor Thirteenth", "m13", "1,m3,5,m7,9,11,13");
      public static readonly ChordFormula Dominant7 = new ChordFormula("Dominant Seventh", "7", "1,3,5,m7");
      public static readonly ChordFormula Dominant9 = new ChordFormula("Dominant Ninth", "9", "1,3,5,m7,9");
      public static readonly ChordFormula Dominant11 = new ChordFormula("Dominant Eleventh", "11", "1,3,5,m7,9,11");
      public static readonly ChordFormula Dominant13 = new ChordFormula("Dominant Thirteenth", "13", "1,3,5,m7,9,11,13");
      public static readonly ChordFormula SixNine = new ChordFormula("Six Nine", "6/9", "1,3,5,6,9");
      public static readonly ChordFormula AddNine = new ChordFormula("Add Nine", "add9", "1,3,5,9");
      public static readonly ChordFormula Diminished = new ChordFormula("Diminished", "dim", "1,m3,d5");
      public static readonly ChordFormula Diminished7 = new ChordFormula("Diminished Seventh", "dim7", "1,m3,d5,d7");
      public static readonly ChordFormula HalfDiminished = new ChordFormula("Half Diminished", "7dim5", "1,m3,d5,m7");
      public static readonly ChordFormula Augmented = new ChordFormula("Augmented", "aug", "1,3,A5");
      private static readonly StringComparer s_comparer = StringComparer.CurrentCultureIgnoreCase;

      #endregion

      #region Construction

      public ChordFormula(string name, string symbol, string formula)
         : base(name, formula)
      {
         Contract.Requires<ArgumentNullException>(symbol != null, "symbol");
         Symbol = symbol;
      }

      #endregion

      #region Properties

      public string Symbol { get; private set; }

      #endregion
   }
}
