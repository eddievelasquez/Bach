// 
//   AbsoluteFormula.cs: 
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
   using System.Collections;
   using System.Collections.Generic;
   using System.Diagnostics.Contracts;
   using System.Linq;

   public class AbsoluteFormula: IFormula, IEquatable<AbsoluteFormula>, IEnumerable<int>
   {
      #region Construction

      public AbsoluteFormula(params int[] intervals)
      {
         Contract.Requires<ArgumentException>(intervals.Length > 0, "intervals");
         Intervals = intervals;
      }

      #endregion

      #region Properties

      public Int32[] Intervals { get; private set; }

      #endregion

      #region IFormula Members

      public int Count
      {
         get { return Intervals.Length; }
      }

      public IEnumerable<Note> Generate(Note root)
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

      #endregion

      #region IEquatable<AbsoluteFormula> Implementation

      public bool Equals(AbsoluteFormula other)
      {
         if( ReferenceEquals(other, this) )
            return true;

         if( ReferenceEquals(other, null) )
            return false;

         return Intervals.SequenceEqual(other.Intervals);
      }

      public override bool Equals(object other)
      {
         if( ReferenceEquals(other, this) )
            return true;

         if( ReferenceEquals(other, null) || other.GetType() != GetType() )
            return false;

         return Equals((AbsoluteFormula)other);
      }

      public override int GetHashCode()
      {
         const int MULTIPLIER = 89;

         var first = Intervals.FirstOrDefault();
         var last = Intervals.LastOrDefault();
         var length = Intervals.Length;

         unchecked
         {
            int result = ((first.GetHashCode() + length) * MULTIPLIER) + (last.GetHashCode() + length);
            return result;
         }
      }

      #endregion

      #region IEnumerable<Int> Implementation

      public IEnumerator<int> GetEnumerator()
      {
         return ((IEnumerable<int>)Intervals).GetEnumerator();
      }

      IEnumerator IEnumerable.GetEnumerator()
      {
         return GetEnumerator();
      }

      #endregion
   }
}
