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

namespace Bach.Model
{
   using System;
   using System.Collections.Generic;
   using System.Collections.ObjectModel;
   using System.Diagnostics.Contracts;
   using System.Linq;
   using System.Text;

   public class Formula: IEquatable<Formula>
   {
      #region Data Members

      private readonly Interval[] _intervals;

      #endregion

      #region Construction

      public Formula(string name, params Interval[] intervals)
      {
         Contract.Requires<ArgumentNullException>(name != null, "name");
         Contract.Requires<ArgumentException>(name.Length > 0, "name");
         Contract.Requires<ArgumentOutOfRangeException>(intervals.Length > 0, "intervals");

         Name = name;
         _intervals = intervals;
      }

      public Formula(string name, string formula)
         : this(name, ParseIntervals(formula))
      {
      }

      #endregion

      #region Properties

      public String Name { get; private set; }

      public ReadOnlyCollection<Interval> Intervals
      {
         get { return new ReadOnlyCollection<Interval>(_intervals); }
      }

      public Int32 Count
      {
         get { return _intervals.Length; }
      }

      #endregion

      #region IEquatable<Formula> Members

      public bool Equals(Formula other)
      {
         if( ReferenceEquals(other, this) )
            return true;

         if( ReferenceEquals(other, null) )
            return false;

         return StringComparer.CurrentCultureIgnoreCase.Equals(Name, other.Name) && _intervals.SequenceEqual(other.Intervals);
      }

      #endregion

      #region Overrides

      public override string ToString()
      {
         var buf = new StringBuilder();
         buf.Append(Name);
         buf.Append(": ");

         bool needComma = false;

         foreach( var interval in _intervals )
         {
            if( needComma )
               buf.Append(',');
            else
               needComma = true;

            buf.Append(interval);
         }

         return buf.ToString();
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
         return Name.GetHashCode();
      }

      #endregion

      public IEnumerable<Note> Generate(Note root)
      {
         int intervalCount = _intervals.Length;
         int index = 0;

         while( true )
         {
            Interval interval = _intervals[index % intervalCount];

            var accidentalMode = AccidentalMode.FavorSharps;
            if(interval.Quality == IntervalQuality.Diminished || interval.Quality == IntervalQuality.Minor)
               accidentalMode = AccidentalMode.FavorFlats;

            int octaveAdd = index / intervalCount;

            // TODO: Must ensure that enharmonic intervals are choosing the appropriate note
            Note note = root.Add(interval.Steps + octaveAdd * Note.INTERVALS_PER_OCTAVE, accidentalMode);
            yield return note;

            ++index;
         }
      }

      private static Interval[] ParseIntervals(string formula)
      {
         Contract.Requires<ArgumentNullException>(formula != null, "formula");
         Contract.Requires<ArgumentException>(formula.Length > 0, "formula");

         var values = formula.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
         return values.Select(Interval.Parse).ToArray();
      }
   }
}
