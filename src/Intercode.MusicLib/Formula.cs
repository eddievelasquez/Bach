// 
//   Formula.cs: 
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
   using System.Collections;
   using System.Collections.Generic;
   using System.Diagnostics.Contracts;
   using System.Text.RegularExpressions;

   public class Formula: IEnumerable<FormulaStep>
   {
      #region Data Members

      private static readonly Regex s_formulaRx = new Regex("(\\d\\d?)(bb?|##?)?(?:,)?", RegexOptions.Singleline);
      private readonly List<FormulaStep> _steps = new List<FormulaStep>();

      #endregion

      #region Public Methods

      public void AddStep(int interval, Accidental accidental = Accidental.Natural)
      {
         AddStep(new FormulaStep(interval, accidental));
      }

      public void AddStep(FormulaStep step)
      {
         _steps.Add(step);
      }

      public int Count
      {
         get { return _steps.Count; }
      }

      public static Formula Parse(string s)
      {
         Contract.Requires<ArgumentNullException>(s != null, "s");
         Contract.Requires<ArgumentException>(s.Length > 0, "s");

         var matches = s_formulaRx.Matches(s);
         var formula = new Formula();
         foreach( Match match in matches )
            formula.AddStep(Int32.Parse(match.Groups[1].Value), AccidentalExtensions.Parse(match.Groups[2].Value));

         return formula;
      }

      #endregion

      #region IEnumerable implementation

      public IEnumerator<FormulaStep> GetEnumerator()
      {
         return _steps.GetEnumerator();
      }

      IEnumerator IEnumerable.GetEnumerator()
      {
         return GetEnumerator();
      }

      #endregion
   }
}
