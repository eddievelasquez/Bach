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
   using System.Linq;
   using System.Text;
   using System.Text.RegularExpressions;

   public class Formula: IEnumerable<FormulaStep>
   {
      #region FormulaStepComparer class

      private class FormulaStepComparer: IComparer<FormulaStep>
      {
         public int Compare(FormulaStep x, FormulaStep y)
         {
            int result = x.Interval - y.Interval;
            if( result == 0 )
               result = x.Accidental - y.Accidental;

            return result;
         }
      }

      #endregion

      #region Data Members

      private static readonly Regex s_formulaRx = new Regex("(\\d\\d?)(bb?|##?)?(?:,)?", RegexOptions.Singleline);
      private readonly SortedSet<FormulaStep> _steps;

      #endregion

      #region Construction

      public Formula()
      {
         _steps = new SortedSet<FormulaStep>(new FormulaStepComparer());
      }

      public Formula(IEnumerable<FormulaStep> steps)
      {
         Contract.Requires<ArgumentNullException>(steps != null, "steps");
         _steps = new SortedSet<FormulaStep>(steps, new FormulaStepComparer());
      }

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
         var steps = new List<FormulaStep>();

         foreach( Match match in matches )
            steps.Add(new FormulaStep(Int32.Parse(match.Groups[1].Value), AccidentalExtensions.Parse(match.Groups[2].Value)));

         var formula = new Formula(steps);
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

      #region IEquatable<Formula> Implementation

      public bool Equals(Formula other)
      {
         if( ReferenceEquals(other, this) )
            return true;

         if( ReferenceEquals(other, null) )
            return false;

         return _steps.SequenceEqual(other._steps);
      }

      public override bool Equals(object other)
      {
         if( ReferenceEquals(other, this) )
            return true;

         if( ReferenceEquals(other, null) || other.GetType() != GetType() )
            return false;

         return Equals((Formula)other);
      }

      public override int GetHashCode()
      {
         const int MULTIPLIER = 89;

         var first = _steps.FirstOrDefault();
         var last = _steps.LastOrDefault();
         var length = _steps.Count;

         unchecked
         {
            int result = ((first.GetHashCode() + length) * MULTIPLIER) + (last.GetHashCode() + length);
            return result;
         }
      }

      #endregion

      #region Overrides

      public override string ToString()
      {
         var buf = new StringBuilder();
         bool needsComma = false;

         foreach( var step in _steps )
         {
            if( needsComma )
               buf.Append(',');
            else
               needsComma = true;

            buf.Append(step);
         }

         return buf.ToString();
      }

      #endregion

      #region Public Methods

      public IEnumerable<Note> Generate(Note root)
      {
         Contract.Requires<ArgumentNullException>(root != null, "root");

         var majorScale = new Scale(root, ScaleFormula.Major).Notes;
         var length = majorScale.Length;

         foreach( var step in _steps )
         {
            var note = majorScale[(step.Interval - 1) % length];

            if( step.Accidental != Accidental.Natural )
               note = note.ApplyAccidental(step.Accidental);

            yield return note;
         }
      }

      #endregion
   }
}
