// 
//   Interval.cs: 
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
   using System.Text;

   public struct Interval: IEquatable<Interval>, IComparable<Interval>
   {
      #region Constants

      private const int QUALITY_COUNT = IntervalQuality.Augmented - IntervalQuality.Diminished + 1;
      private const int NAME_COUNT = 8 - 1 + 1;

      private static readonly int[,] s_steps = new int[NAME_COUNT, QUALITY_COUNT]
      {
         // Diminished, Minor, Perfect, Major, Augmented
         { -1, -1,  0, -1, 1 }, // First
         {  0,  1, -1,  2, 3 }, // Second
         {  2,  3, -1,  4, 5 }, // Third
         {  4, -1,  5, -1, 6 }, // Fourth
         {  6, -1,  7, -1, 8 }, // Fifth
         {  7,  8, -1,  9, 10 }, // Sixth
         {  9, 10, -1, 11, 12 }, // Seventh
         { 11, -1, 12, -1, 13 } // Eighth
      };

      public static readonly Interval Perfect1 = new Interval(1, IntervalQuality.Perfect);
      public static readonly Interval Augmented1 = new Interval(1, IntervalQuality.Augmented);
      public static readonly Interval Diminished2 = new Interval(2, IntervalQuality.Diminished);
      public static readonly Interval Minor2 = new Interval(2, IntervalQuality.Minor);
      public static readonly Interval Major2 = new Interval(2, IntervalQuality.Major);
      public static readonly Interval Augmented2 = new Interval(2, IntervalQuality.Augmented);
      public static readonly Interval Diminished3 = new Interval(3, IntervalQuality.Diminished);
      public static readonly Interval Minor3 = new Interval(3, IntervalQuality.Minor);
      public static readonly Interval Major3 = new Interval(3, IntervalQuality.Major);
      public static readonly Interval Augmented3 = new Interval(3, IntervalQuality.Augmented);
      public static readonly Interval Diminished4 = new Interval(4, IntervalQuality.Diminished);
      public static readonly Interval Perfect4 = new Interval(4, IntervalQuality.Perfect);
      public static readonly Interval Augmented4 = new Interval(4, IntervalQuality.Augmented);
      public static readonly Interval Diminished5 = new Interval(5, IntervalQuality.Diminished);
      public static readonly Interval Perfect5 = new Interval(5, IntervalQuality.Perfect);
      public static readonly Interval Augmented5 = new Interval(5, IntervalQuality.Augmented);
      public static readonly Interval Diminished6 = new Interval(6, IntervalQuality.Diminished);
      public static readonly Interval Minor6 = new Interval(6, IntervalQuality.Minor);
      public static readonly Interval Major6 = new Interval(6, IntervalQuality.Major);
      public static readonly Interval Augmented6 = new Interval(6, IntervalQuality.Augmented);
      public static readonly Interval Diminished7 = new Interval(7, IntervalQuality.Diminished);
      public static readonly Interval Minor7 = new Interval(7, IntervalQuality.Minor);
      public static readonly Interval Major7 = new Interval(7, IntervalQuality.Major);
      public static readonly Interval Augmented7 = new Interval(7, IntervalQuality.Augmented);
      public static readonly Interval Diminished8 = new Interval(8, IntervalQuality.Diminished);
      public static readonly Interval Perfect8 = new Interval(8, IntervalQuality.Perfect);
      public static readonly Interval Augmented8 = new Interval(8, IntervalQuality.Augmented);
      public static readonly Interval Invalid = new Interval();

      #endregion

      #region Data Members

      private readonly Byte _number;
      private readonly Byte _quality;

      #endregion

      #region Construction

      public Interval(int number, IntervalQuality quality)
      {
         // Validate that the provided number and quality 
         // refer to a valid interval
         GetSteps(number, quality);

         _number = (byte)number;
         _quality = (byte)quality;
      }

      #endregion

      #region Properties

      public Int32 Number
      {
         get { return _number; }
      }

      public IntervalQuality Quality
      {
         get { return (IntervalQuality)_quality; }
      }

      public Int32 Steps
      {
         get
         {
            if( Number == 0 )
               return 0;

            return GetSteps(Number, Quality);
         }
      }

      #endregion

      #region Methods

      public static bool IsValid(int number, IntervalQuality quality)
      {
         if( number < 1 || number > 8 )
            return false;

         if( quality < IntervalQuality.Diminished || quality > IntervalQuality.Augmented )
            return false;

         int steps = s_steps[number - 1, (int)quality];
         return steps != -1;
      }

      public static int GetSteps(int number, IntervalQuality quality)
      {
         Contract.Requires<ArgumentOutOfRangeException>(number >= 1, "number");
         Contract.Requires<ArgumentOutOfRangeException>(number <= 8, "number");
         Contract.Requires<ArgumentOutOfRangeException>(quality >= IntervalQuality.Diminished, "quality");
         Contract.Requires<ArgumentOutOfRangeException>(quality <= IntervalQuality.Augmented, "quality");

         int steps = s_steps[number - 1, (int)quality];
         if( steps == -1 )
            throw new ArgumentException(String.Format("{0} is not a valid interval", ToString(number, quality)));

         return steps;
      }

      public override string ToString()
      {
         if( Number == 0 )
            return String.Empty;

         return ToString(Number, Quality);
      }

      private static string ToString(int number, IntervalQuality quality, bool supressPerfectAndMajor = true)
      {
         var buf = new StringBuilder();
         if( quality != IntervalQuality.Perfect && quality != IntervalQuality.Major || supressPerfectAndMajor )
            buf.Append(quality.Symbol());

         buf.Append(number);
         return buf.ToString();
      }

      public static Interval Parse(string value)
      {
         Interval interval;
         if( !TryParse(value, out interval) )
            throw new FormatException(value + " is not a valid interval");

         return interval;
      }

      public static bool TryParse(string value, out Interval interval)
      {
         interval = Invalid;

         if( String.IsNullOrWhiteSpace(value) )
            return false;

         int i = 0;

         // skip whitespaces
         while( i < value.Length && Char.IsWhiteSpace(value, i) )
            ++i;

         var quality = (IntervalQuality)(-1);

         if( Char.IsLetter(value, i) )
         {
            if( !IntervalQualityExtensions.TryParse(value.Substring(i, 1), out quality) )
               return false;

            ++i;
         }

         int number;
         if( !Int32.TryParse(value.Substring(i), out number) )
            return false;

         if( number < 1 || number > 8 )
            return false;

         if( quality == IntervalQuality.Unknown )
         {
            if( number == 1 || number == 4 || number == 5 || number == 8 )
               quality = IntervalQuality.Perfect;
            else
               quality = IntervalQuality.Major;
         }

         if( !IsValid(number, quality) )
            return false;

         interval = new Interval(number, quality);
         return true;
      }

      #endregion

      #region IEquatable<Interval> Implementation

      public bool Equals(Interval obj)
      {
         return obj.Number == Number && obj.Quality == Quality;
      }

      public override bool Equals(object obj)
      {
         if( ReferenceEquals(obj, null) || obj.GetType() != GetType() )
            return false;

         return Equals((Interval)obj);
      }

      public override int GetHashCode()
      {
         int hashCode = (_quality << 8) | _number;
         return hashCode;
      }

      #endregion

      #region IComparable<Interval>

      public int CompareTo(Interval other)
      {
         int result = _number - other._number;
         if( result != 0 )
            return result;

         result = _quality - _quality;
         return result;
      }

      #endregion

      #region Operators

      public static bool operator ==(Interval lhs, Interval rhs)
      {
         return Equals(lhs, rhs);
      }

      public static bool operator !=(Interval lhs, Interval rhs)
      {
         return !Equals(lhs, rhs);
      }

      public static bool operator <(Interval lhs, Interval rhs)
      {
         return lhs.CompareTo(rhs) < 0;
      }

      public static bool operator <=(Interval lhs, Interval rhs)
      {
         return lhs.CompareTo(rhs) <= 0;
      }

      public static bool operator >(Interval lhs, Interval rhs)
      {
         return lhs.CompareTo(rhs) > 0;
      }

      public static bool operator >=(Interval lhs, Interval rhs)
      {
         return lhs.CompareTo(rhs) >= 0;
      }

      #endregion
   }
}
