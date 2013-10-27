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
   using System.Collections.ObjectModel;
   using System.Diagnostics.Contracts;
   using System.Linq;
   using System.Text;

   public class ScaleFormula: IEquatable<ScaleFormula>
   {
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
      private static readonly StringComparer s_comparer = StringComparer.CurrentCultureIgnoreCase;
      private readonly Interval[] _intervals;

      #region Construction

      public ScaleFormula(string name, params Interval[] intervals)
      {
         Contract.Requires<ArgumentNullException>(name != null, "name");
         Contract.Requires<ArgumentException>(name.Length > 0, "name");
         Contract.Requires<ArgumentOutOfRangeException>(intervals.Length > 0, "intervals");

         Name = name;
         _intervals = intervals;
      }

      public ScaleFormula(string name, string formula)
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

      #region IEquatable<ScaleFormula> Members

      public bool Equals(ScaleFormula other)
      {
         if( ReferenceEquals(other, this) )
            return true;

         if( ReferenceEquals(other, null) )
            return false;

         return s_comparer.Equals(Name, other.Name) && _intervals.SequenceEqual(other.Intervals);
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

         return Equals((ScaleFormula)other);
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
