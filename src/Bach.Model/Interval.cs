//
// Module Name: Interval.cs
// Project:     Bach.Model
// Copyright (c) 2016  Eddie Velasquez.
//
// This source is subject to the MIT License.
// See http://opensource.org/licenses/MIT.
// All other rights reserved.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software
// and associated documentation files (the "Software"), to deal in the Software without restriction,
// including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense,
// and/or sell copies of the Software, and to permit persons to whom the Software is furnished to
// do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial
//  portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A
// PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE
// OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

namespace Bach.Model
{
  using System;
  using System.Diagnostics.Contracts;
  using System.Text;

  public struct Interval: IEquatable<Interval>,
                          IComparable<Interval>
  {
    #region Constants

    public const int MinInterval = 1;
    public const int MaxInterval = 15;

    private static readonly int[,] s_steps =
    {
      // Diminished, Minor, Perfect, Major, Augmented
      { -1, -1, 0, -1, 1 }, // First
      { 0, 1, -1, 2, 3 }, // Second
      { 2, 3, -1, 4, 5 }, // Third
      { 4, -1, 5, -1, 6 }, // Fourth
      { 6, -1, 7, -1, 8 }, // Fifth
      { 7, 8, -1, 9, 10 }, // Sixth
      { 9, 10, -1, 11, 12 }, // Seventh
      { 11, -1, 12, -1, 13 }, // Eighth
      { 12, 13, -1, 14, 15 }, // Ninth (Second)
      { 14, 15, -1, 16, 17 }, // Tenth (Third)
      { 16, -1, 17, -1, 18 }, // Eleventh (Fourth)
      { 18, -1, 19, -1, 20 }, // Twelfth (Fifth)
      { 19, 20, -1, 21, 22 }, // Thirteenth (Sixth)
      { 21, 22, -1, 23, 24 }, // Forteenth (Seventh)
      { 23, -1, 24, -1, 25 } // Fifteenth (Eighth)
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

    private readonly byte _number;
    private readonly byte _quality;

    #endregion

    #region Construction

    public Interval(int number,
                    IntervalQuality quality)
    {
      // Validate that the provided number and quality
      // refer to a valid interval
      GetSteps(number, quality);

      _number = (byte) number;
      _quality = (byte) quality;
    }

    #endregion

    #region Properties

    public int Number => _number;

    public IntervalQuality Quality => (IntervalQuality) _quality;

    public int Steps => Number != 0 ? GetSteps(Number, Quality) : 0;

    #endregion

    #region Methods

    public static bool IsValid(int number,
                               IntervalQuality quality)
    {
      if( number < MinInterval || number > MaxInterval )
      {
        return false;
      }

      if( quality < IntervalQuality.Diminished || quality > IntervalQuality.Augmented )
      {
        return false;
      }

      int steps = s_steps[number - 1, (int) quality];
      return steps != -1;
    }

    public static int GetSteps(int number,
                               IntervalQuality quality)
    {
      Contract.Requires<ArgumentOutOfRangeException>(number >= MinInterval);
      Contract.Requires<ArgumentOutOfRangeException>(number <= MaxInterval);
      Contract.Requires<ArgumentOutOfRangeException>(quality >= IntervalQuality.Diminished);
      Contract.Requires<ArgumentOutOfRangeException>(quality <= IntervalQuality.Augmented);

      int steps = s_steps[number - 1, (int) quality];
      if( steps == -1 )
      {
        throw new ArgumentException($"{ToString(number, quality)} is not a valid interval");
      }

      return steps;
    }

    public override string ToString() => Number == 0 ? string.Empty : ToString(Number, Quality);

    private static string ToString(int number,
                                   IntervalQuality quality,
                                   bool supressPerfectAndMajor = true)
    {
      var buf = new StringBuilder();
      if( quality != IntervalQuality.Perfect && quality != IntervalQuality.Major || supressPerfectAndMajor )
      {
        buf.Append(quality.Symbol());
      }

      buf.Append(number);
      return buf.ToString();
    }

    public static Interval Parse(string value)
    {
      Interval interval;
      if( !TryParse(value, out interval) )
      {
        throw new FormatException(value + " is not a valid interval");
      }

      return interval;
    }

    public static bool TryParse(string value,
                                out Interval interval)
    {
      interval = Invalid;

      if( string.IsNullOrWhiteSpace(value) )
      {
        return false;
      }

      var i = 0;

      // skip whitespaces
      while( i < value.Length && char.IsWhiteSpace(value, i) )
      {
        ++i;
      }

      var quality = (IntervalQuality) (-1);

      if( char.IsLetter(value, i) )
      {
        if( value[i] == 'R' )
        {
          interval = new Interval(1, IntervalQuality.Perfect);
          return true;
        }

        if( !IntervalQualityExtensions.TryParse(value.Substring(i, 1), out quality) )
        {
          return false;
        }

        ++i;
      }

      int number;
      if( !int.TryParse(value.Substring(i), out number) )
      {
        return false;
      }

      if( quality == IntervalQuality.Unknown )
      {
        int temp = ((number - 1) % 7) + 1;
        if( temp == 1 || temp == 4 || temp == 5 )
        {
          quality = IntervalQuality.Perfect;
        }
        else
        {
          quality = IntervalQuality.Major;
        }
      }

      if( !IsValid(number, quality) )
      {
        return false;
      }

      interval = new Interval(number, quality);
      return true;
    }

    #endregion

    #region IEquatable<Interval> Implementation

    public bool Equals(Interval obj) => obj.Number == Number && obj.Quality == Quality;

    public override bool Equals(object obj)
    {
      if( ReferenceEquals(obj, null) || obj.GetType() != GetType() )
      {
        return false;
      }

      return Equals((Interval) obj);
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
      {
        return result;
      }

      result = _quality - other._quality;
      return result;
    }

    #endregion

    #region Operators

    public static bool operator==(Interval lhs,
                                  Interval rhs) => Equals(lhs, rhs);

    public static bool operator!=(Interval lhs,
                                  Interval rhs) => !Equals(lhs, rhs);

    public static bool operator<(Interval lhs,
                                 Interval rhs) => lhs.CompareTo(rhs) < 0;

    public static bool operator<=(Interval lhs,
                                  Interval rhs) => lhs.CompareTo(rhs) <= 0;

    public static bool operator>(Interval lhs,
                                 Interval rhs) => lhs.CompareTo(rhs) > 0;

    public static bool operator>=(Interval lhs,
                                  Interval rhs) => lhs.CompareTo(rhs) >= 0;

    #endregion
  }
}
