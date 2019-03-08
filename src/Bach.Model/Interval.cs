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
// PARTICULAR PURPOSE AND NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE
// OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

namespace Bach.Model
{
  using System;
  using System.Text;

  /// <summary>An interval.</summary>
  public struct Interval
    : IEquatable<Interval>,
      IComparable<Interval>
  {
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
      { 11, -1, 12, -1, 13 }, // Octave
      { 12, 13, -1, 14, 15 }, // Ninth (Second)
      { 14, 15, -1, 16, 17 }, // Tenth (Third)
      { 16, -1, 17, -1, 18 }, // Eleventh (Fourth)
      { 18, -1, 19, -1, 20 }, // Twelfth (Fifth)
      { 19, 20, -1, 21, 22 }, // Thirteenth (Sixth)
      { 21, 22, -1, 23, 24 } // Fourteenth (Seventh)
    };

    public static readonly Interval Unison = new Interval(IntervalQuantity.Unison, IntervalQuality.Perfect);
    public static readonly Interval AugmentedFirst = new Interval(IntervalQuantity.Unison, IntervalQuality.Augmented);
    public static readonly Interval DiminishedSecond = new Interval(IntervalQuantity.Second, IntervalQuality.Diminished);
    public static readonly Interval MinorSecond = new Interval(IntervalQuantity.Second, IntervalQuality.Minor);
    public static readonly Interval MajorSecond = new Interval(IntervalQuantity.Second, IntervalQuality.Major);
    public static readonly Interval AugmentedSecond = new Interval(IntervalQuantity.Second, IntervalQuality.Augmented);
    public static readonly Interval DiminishedThird = new Interval(IntervalQuantity.Third, IntervalQuality.Diminished);
    public static readonly Interval MinorThird = new Interval(IntervalQuantity.Third, IntervalQuality.Minor);
    public static readonly Interval MajorThird = new Interval(IntervalQuantity.Third, IntervalQuality.Major);
    public static readonly Interval AugmentedThird = new Interval(IntervalQuantity.Third, IntervalQuality.Augmented);
    public static readonly Interval DiminishedFourth = new Interval(IntervalQuantity.Fourth, IntervalQuality.Diminished);
    public static readonly Interval Fourth = new Interval(IntervalQuantity.Fourth, IntervalQuality.Perfect);
    public static readonly Interval AugmentedFourth = new Interval(IntervalQuantity.Fourth, IntervalQuality.Augmented);
    public static readonly Interval DiminishedFifth = new Interval(IntervalQuantity.Fifth, IntervalQuality.Diminished);
    public static readonly Interval Fifth = new Interval(IntervalQuantity.Fifth, IntervalQuality.Perfect);
    public static readonly Interval AugmentedFifth = new Interval(IntervalQuantity.Fifth, IntervalQuality.Augmented);
    public static readonly Interval DiminishedSixth = new Interval(IntervalQuantity.Sixth, IntervalQuality.Diminished);
    public static readonly Interval MinorSixth = new Interval(IntervalQuantity.Sixth, IntervalQuality.Minor);
    public static readonly Interval MajorSixth = new Interval(IntervalQuantity.Sixth, IntervalQuality.Major);
    public static readonly Interval AugmentedSixth = new Interval(IntervalQuantity.Sixth, IntervalQuality.Augmented);
    public static readonly Interval DiminishedSeventh = new Interval(IntervalQuantity.Seventh, IntervalQuality.Diminished);
    public static readonly Interval MinorSeventh = new Interval(IntervalQuantity.Seventh, IntervalQuality.Minor);
    public static readonly Interval MajorSeventh = new Interval(IntervalQuantity.Seventh, IntervalQuality.Major);
    public static readonly Interval AugmentedSeventh = new Interval(IntervalQuantity.Seventh, IntervalQuality.Augmented);
    public static readonly Interval DiminishedOctave = new Interval(IntervalQuantity.Octave, IntervalQuality.Diminished);
    public static readonly Interval Octave = new Interval(IntervalQuantity.Octave, IntervalQuality.Perfect);
    public static readonly Interval Invalid = new Interval();

    private readonly byte _quantity;
    private readonly byte _quality;

    /// <summary>Constructor.</summary>
    /// <param name="quantity">The interval quantity.</param>
    /// <param name="quality">The interval quality.</param>
    public Interval(IntervalQuantity quantity,
                    IntervalQuality quality)
    {
      // Validate that the provided number and quality
      // refer to a valid interval
      GetSemitoneCount(quantity, quality);

      _quantity = (byte)quantity;
      _quality = (byte)quality;
    }

    /// <summary>Constructor.</summary>
    /// <param name="quantity">The interval quantity.</param>
    /// <param name="semitoneCount">The number of semitones in the interval.</param>
    public Interval(IntervalQuantity quantity,
                    int semitoneCount)
    {
      var quality = IntervalQuality.Diminished;
      var found = false;

      for (; quality <= IntervalQuality.Augmented; ++quality)
      {
        if (s_steps[(int)quantity, (int)quality] == semitoneCount)
        {
          found = true;
          break;
        }
      }

      if (!found)
      {
        throw new ArgumentException($"A {quantity} with {semitoneCount} is not a valid interval");
      }

      _quantity = (byte)quantity;
      _quality = (byte)quality;
    }

    /// <summary>Gets the interval's quantity.</summary>
    /// <value>The quantity.</value>
    public IntervalQuantity Quantity => (IntervalQuantity)_quantity;

    /// <summary>Gets the interval's quality.</summary>
    /// <value>The quality.</value>
    public IntervalQuality Quality => (IntervalQuality)_quality;

    /// <summary>Gets the number of semitones in the interval.</summary>
    /// <value>The number of semitones.</value>
    public int SemitoneCount => GetSemitoneCount(Quantity, Quality);

    /// <summary>Determine if the interval quantity and quality refer to a valid interval.</summary>
    /// <param name="quantity">The interval quantity.</param>
    /// <param name="quality">The interval quality.</param>
    /// <returns>True if valid, false if not.</returns>
    public static bool IsValid(IntervalQuantity quantity,
                               IntervalQuality quality)
    {
      if (quantity < IntervalQuantity.Unison || quantity > IntervalQuantity.Fourteenth)
      {
        return false;
      }

      if (quality < IntervalQuality.Diminished || quality > IntervalQuality.Augmented)
      {
        return false;
      }

      int steps = s_steps[(int)quantity, (int)quality];
      return steps != -1;
    }

    /// <summary>Gets semitone count of the interval.</summary>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when one or more arguments are outside the
    ///                                               required range.</exception>
    /// <exception cref="ArgumentException">Thrown when the interval quantity and quality combination doesn't refer to a valid interval.</exception>
    /// <param name="quantity">The interval quantity.</param>
    /// <param name="quality">The interval quality.</param>
    /// <returns>The semitone count.</returns>
    public static int GetSemitoneCount(IntervalQuantity quantity,
                                       IntervalQuality quality)
    {
      Contract.Requires<ArgumentOutOfRangeException>(quantity >= IntervalQuantity.Unison);
      Contract.Requires<ArgumentOutOfRangeException>(quantity <= IntervalQuantity.Fourteenth);
      Contract.Requires<ArgumentOutOfRangeException>(quality >= IntervalQuality.Diminished);
      Contract.Requires<ArgumentOutOfRangeException>(quality <= IntervalQuality.Augmented);

      int steps = s_steps[(int)quantity, (int)quality];
      if (steps == -1)
      {
        throw new ArgumentException($"{ToString(quantity, quality)} is not a valid interval");
      }

      return steps;
    }

    /// <summary>Returns the fully qualified type name of this instance.</summary>
    /// <returns>The fully qualified type name.</returns>
    public override string ToString() => ToString(Quantity, Quality);

    private static string ToString(IntervalQuantity quantity,
                                   IntervalQuality quality,
                                   bool suppressPerfectAndMajor = true)
    {
      var buf = new StringBuilder();
      if (quality != IntervalQuality.Perfect && quality != IntervalQuality.Major || suppressPerfectAndMajor)
      {
        buf.Append(quality.Symbol());
      }

      buf.Append((int)(quantity + 1));
      return buf.ToString();
    }

    /// <summary>
    /// Converts the specified string representation of an interval to its <see cref="Interval"/> equivalent.
    /// </summary>
    /// <param name="value">A string containing the interval to convert.</param>
    /// <returns>An object that is equivalent to the interval contained in value.</returns>
    /// <exception cref="FormatException">value does not contain a valid string representation of an interval.</exception>
    public static Interval Parse(string value)
    {
      if (!TryParse(value, out Interval interval))
      {
        throw new FormatException(value + " is not a valid interval");
      }

      return interval;
    }

    /// <summary>
    /// Converts the specified string representation of an interval to its <see cref="Interval"/> equivalent
    /// and returns a value that indicates whether the conversion succeeded.
    /// </summary>
    /// <param name="value">A string containing the interval quality to convert.</param>
    /// <param name="interval">When this method returns, contains the Interval value equivalent to the interval
    /// contained in value, if the conversion succeeded, or <see cref="Interval.Invalid"/> if the conversion failed.
    /// The conversion fails if the value parameter is null or empty or does not contain a valid string
    /// representation of an interval. This parameter is passed uninitialized.</param>
    /// <returns><see langword="true"/> if the value parameter was converted successfully; otherwise, <see langword="false"/>.</returns>
    public static bool TryParse(string value,
                                out Interval interval)
    {
      interval = Invalid;

      if (string.IsNullOrWhiteSpace(value))
      {
        return false;
      }

      var i = 0;

      // skip whitespaces
      while (i < value.Length && char.IsWhiteSpace(value, i))
      {
        ++i;
      }

      var quality = (IntervalQuality)(-1);

      if (char.IsLetter(value, i))
      {
        if (value[i] == 'R')
        {
          interval = new Interval(IntervalQuantity.Unison, IntervalQuality.Perfect);
          return true;
        }

        if (!IntervalQualityExtensions.TryParse(value.Substring(i, 1), out quality))
        {
          return false;
        }

        ++i;
      }

      if (!int.TryParse(value.Substring(i), out int number))
      {
        return false;
      }

      if (quality == IntervalQuality.Unknown)
      {
        int temp = ((number - 1) % 7) + 1;
        if (temp == 1 || temp == 4 || temp == 5)
        {
          quality = IntervalQuality.Perfect;
        }
        else
        {
          quality = IntervalQuality.Major;
        }
      }

      var quantity = (IntervalQuantity)(number - 1);
      if (!IsValid(quantity, quality))
      {
        return false;
      }

      interval = new Interval(quantity, quality);
      return true;
    }

    /// <inheritdoc />
    public bool Equals(Interval other) => other.Quantity == Quantity && other.Quality == Quality;

    /// <inheritdoc />
    public override bool Equals(object obj)
    {
      if (ReferenceEquals(obj, null) || obj.GetType() != GetType())
      {
        return false;
      }

      return Equals((Interval)obj);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
      int hashCode = (_quality << 8) | _quantity;
      return hashCode;
    }

    /// <inheritdoc />
    public int CompareTo(Interval other)
    {
      int result = _quantity - other._quantity;
      if (result != 0)
      {
        return result;
      }

      result = _quality - other._quality;
      return result;
    }

    /// <summary>Equality operator.</summary>
    /// <param name="lhs">The first instance to compare.</param>
    /// <param name="rhs">The second instance to compare.</param>
    /// <returns>The result of the operation.</returns>
    public static bool operator ==(Interval lhs,
                                  Interval rhs)
      => Equals(lhs, rhs);

    /// <summary>Inequality operator.</summary>
    /// <param name="lhs">The first instance to compare.</param>
    /// <param name="rhs">The second instance to compare.</param>
    /// <returns>The result of the operation.</returns>
    public static bool operator !=(Interval lhs,
                                  Interval rhs)
      => !Equals(lhs, rhs);

    /// <summary>Lesser-than comparison operator.</summary>
    /// <param name="lhs">The first instance to compare.</param>
    /// <param name="rhs">The second instance to compare.</param>
    /// <returns>The result of the operation.</returns>
    public static bool operator <(Interval lhs,
                                 Interval rhs)
      => lhs.CompareTo(rhs) < 0;

    /// <summary>Lesser-than-or-equal comparison operator.</summary>
    /// <param name="lhs">The first instance to compare.</param>
    /// <param name="rhs">The second instance to compare.</param>
    /// <returns>The result of the operation.</returns>
    public static bool operator <=(Interval lhs,
                                  Interval rhs)
      => lhs.CompareTo(rhs) <= 0;

    /// <summary>Greater-than comparison operator.</summary>
    /// <param name="lhs">The first instance to compare.</param>
    /// <param name="rhs">The second instance to compare.</param>
    /// <returns>The result of the operation.</returns>
    public static bool operator >(Interval lhs,
                                 Interval rhs)
      => lhs.CompareTo(rhs) > 0;

    /// <summary>Greater-than-or-equal comparison operator.</summary>
    /// <param name="lhs">The first instance to compare.</param>
    /// <param name="rhs">The second instance to compare.</param>
    /// <returns>The result of the operation.</returns>
    public static bool operator >=(Interval lhs,
                                  Interval rhs)
      => lhs.CompareTo(rhs) >= 0;
  }
}
