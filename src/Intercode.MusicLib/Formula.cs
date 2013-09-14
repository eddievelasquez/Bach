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

      private enum ParseState
      {
         Start,
         Flat,
         Interval,
         Sharp
      }

      public static Formula Parse(string s)
      {
         Contract.Requires<ArgumentNullException>(s != null, "s");
         Contract.Requires<ArgumentException>(s.Length > 0, "s");

         var steps = new List<FormulaStep>();

         var state = ParseState.Start;
         var accidental = Accidental.Natural;
         Int32 interval = 0;

         foreach( char c in s )
         {
            if( Char.IsWhiteSpace(c) )
               continue;

            if( state == ParseState.Start )
            {
               accidental = Accidental.Natural;
               interval = 0;
            }

            if( c == 'b' || c == 'B' )
            {
               if( state != ParseState.Start && state != ParseState.Flat )
                  goto InvalidFormula;

               state = ParseState.Flat;
               --accidental;
               continue;
            }

            if( c >= '0' && c <= '9' )
            {
               if( state == ParseState.Sharp )
                  goto InvalidFormula;

               state = ParseState.Interval;
               interval = (interval * 10) + c - '0';
               continue;
            }

            if( c == '#' )
            {
               if( (state != ParseState.Interval && state != ParseState.Sharp) || accidental != Accidental.Natural )
                  goto InvalidFormula;

               state = ParseState.Sharp;
               ++accidental;
               continue;
            }

            if( c == ',' || c == '-' )
            {
               if( state != ParseState.Interval && state != ParseState.Sharp )
                  goto InvalidFormula;

               steps.Add(new FormulaStep(interval, accidental));
               state = ParseState.Start;
            }
            else
               goto InvalidFormula;
         }

         // Add last step 
         steps.Add(new FormulaStep(interval, accidental));

         var formula = new Formula(steps);
         return formula;

         InvalidFormula:
         throw new FormatException(String.Format("{0} is not a valid formula", s));
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

         var majorFormula = ScaleFormula.Major;
         var count = majorFormula.Count;
         var majorScale = new Scale(root, majorFormula).Take(count).ToArray();

         foreach( var step in _steps )
         {
            var note = majorScale[(step.Interval - 1) % count];

            if( step.Accidental != Accidental.Natural )
               note = note.ApplyAccidental(step.Accidental);

            yield return note;
         }
      }

      #endregion
   }
}
