//
// Module Name: IntervalQuality.cs
// Project:     Bach.Model
// Copyright (c) 2013, 2016  Eddie Velasquez.
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
  using System.Diagnostics.Contracts;

  /// <summary>Values that represent interval qualities.</summary>
  public struct IntervalQuality
    : IEquatable<IntervalQuality>,
      IComparable<IntervalQuality>,
      IComparable
  {
    #region Constants

    /// <summary>A  constant representing an unknown interval.</summary>
    public static readonly IntervalQuality Unknown = new IntervalQuality(-1);

    /// <summary>A  constant representing a diminished interval.</summary>
    public static readonly IntervalQuality Diminished = new IntervalQuality(0);

    /// <summary>A  constant representing a minor interval.</summary>
    public static readonly IntervalQuality Minor = new IntervalQuality(1);

    /// <summary>A  constant representing a perfect interval.</summary>
    public static readonly IntervalQuality Perfect = new IntervalQuality(2);

    /// <summary>A  constant representing a major interval.</summary>
    public static readonly IntervalQuality Major = new IntervalQuality(3);

    /// <summary>A  constant representing an augmented interval.</summary>
    public static readonly IntervalQuality Augmented = new IntervalQuality(4);

    private static readonly string[] s_symbols = { "d", "m", "P", "M", "A" };
    private static readonly string[] s_short = { "dim", "min", "Perf", "Maj", "Aug" };
    private static readonly string[] s_long = { "diminished", "minor", "perfect", "major", "augmented" };

    #endregion

    #region Data Members

    private readonly int _value;

    #endregion

    #region Constructors

    private IntervalQuality(int value)
    {
      Contract.Requires<ArgumentOutOfRangeException>(value >= -1 && value <= 4);
      _value = value;
    }

    #endregion

    #region Properties

    /// <summary>Returns the symbol for the given interval quality.</summary>
    /// <value>A string.</value>
    public string Symbol => s_symbols[_value];

    /// <summary>Returns the short name for the given interval quality.</summary>
    /// <value>A string.</value>
    public string ShortName => s_short[_value];

    /// <summary>Returns the long name for the given interval quality.</summary>
    /// <value>A string.</value>
    public string LongName => s_long[_value];

    #endregion

    #region IComparable Members

    /// <inheritdoc />
    public int CompareTo(object obj)
    {
      if( ReferenceEquals(null, obj) )
      {
        return 1;
      }

      return obj is IntervalQuality other ? CompareTo(other) : throw new ArgumentException($"Object must be of type {nameof(IntervalQuality)}");
    }

    #endregion

    #region IComparable<IntervalQuality> Members

    /// <inheritdoc />
    public int CompareTo(IntervalQuality other) => _value.CompareTo(other._value);

    #endregion

    #region IEquatable<IntervalQuality> Members

    /// <inheritdoc />
    public bool Equals(IntervalQuality other) => _value == other._value;

    #endregion

    #region Public Methods

    /// <summary>Adds a number of steps to a note name.</summary>
    /// <param name="steps">The number of steps to add.</param>
    /// <returns>A IntervalQuality.</returns>
    [Pure]
    public IntervalQuality Add(int steps)
    {
      var result = new IntervalQuality(_value + steps);
      return result;
    }

    /// <summary>Subtracts a number of steps from a note name.</summary>
    /// <param name="steps">The number of steps to subtract.</param>
    /// <returns>A IntervalQuality.</returns>
    [Pure]
    public IntervalQuality Subtract(int steps) => Add(-steps);

    /// <summary>
    ///   Converts the specified string representation of an interval quality to its <see cref="IntervalQuality" /> equivalent.
    /// </summary>
    /// <param name="value">A string containing the interval quality to convert.</param>
    /// <returns>An object that is equivalent to the interval quality contained in value.</returns>
    /// <exception cref="FormatException">value does not contain a valid string representation of an interval quality.</exception>
    public static IntervalQuality Parse(string value)
    {
      if( !TryParse(value, out IntervalQuality quality) )
      {
        throw new FormatException($"\"{value}\" is not a valid interval quality");
      }

      return quality;
    }

    /// <summary>
    ///   Converts the specified string representation of an interval quality to its <see cref="IntervalQuality" /> equivalent
    ///   and returns a value that indicates whether the conversion succeeded.
    /// </summary>
    /// <param name="value">A string containing the interval quality to convert.</param>
    /// <param name="quality">
    ///   When this method returns, contains the IntervalQuality value equivalent to the interval quality
    ///   contained in value, if the conversion succeeded, or <see cref="IntervalQuality.Unknown" /> if the conversion failed.
    ///   The conversion fails if the value parameter is null or empty  or does not contain a valid string
    ///   representation of an interval quality. This parameter is passed uninitialized.
    /// </param>
    /// <returns>
    ///   <see langword="true" /> if the value parameter was converted successfully; otherwise, <see langword="false" />
    ///   .
    /// </returns>
    public static bool TryParse(string value,
                                out IntervalQuality quality)
    {
      if( !string.IsNullOrEmpty(value) )
      {
        for( var i = 0; i < s_symbols.Length; i++ )
        {
          if( !s_symbols[i].Equals(value) )
          {
            continue;
          }

          quality = new IntervalQuality(i);
          return true;
        }
      }

      quality = Unknown;
      return false;
    }

    #endregion

    #region Overrides

    /// <inheritdoc />
    public override bool Equals(object obj)
    {
      if( ReferenceEquals(null, obj) )
      {
        return false;
      }

      return obj is IntervalQuality other && Equals(other);
    }

    /// <inheritdoc />
    public override int GetHashCode() => _value;

    #endregion

    #region Operators

    /// <summary>Explicit cast that converts the given IntervalQuality to an int.</summary>
    /// <param name="quality">The note name.</param>
    /// <returns>The result of the operation.</returns>
    public static explicit operator int(IntervalQuality quality) => quality._value;

    /// <summary>Explicit cast that converts the given int to a IntervalQuality.</summary>
    /// <param name="value">The value.</param>
    /// <returns>The result of the operation.</returns>
    public static explicit operator IntervalQuality(int value) => new IntervalQuality(value);

    /// <summary>Equality operator.</summary>
    /// <param name="left">The first instance to compare.</param>
    /// <param name="right">The second instance to compare.</param>
    /// <returns>The result of the operation.</returns>
    public static bool operator==(IntervalQuality left,
                                  IntervalQuality right)
      => left.Equals(right);

    /// <summary>Inequality operator.</summary>
    /// <param name="left">The first instance to compare.</param>
    /// <param name="right">The second instance to compare.</param>
    /// <returns>The result of the operation.</returns>
    public static bool operator!=(IntervalQuality left,
                                  IntervalQuality right)
      => !left.Equals(right);

    /// <summary>Lesser-than comparison operator.</summary>
    /// <param name="left">The first instance to compare.</param>
    /// <param name="right">The second instance to compare.</param>
    /// <returns>The result of the operation.</returns>
    public static bool operator<(IntervalQuality left,
                                 IntervalQuality right)
      => left.CompareTo(right) < 0;

    /// <summary>Greater-than comparison operator.</summary>
    /// <param name="left">The first instance to compare.</param>
    /// <param name="right">The second instance to compare.</param>
    /// <returns>The result of the operation.</returns>
    public static bool operator>(IntervalQuality left,
                                 IntervalQuality right)
      => left.CompareTo(right) > 0;

    /// <summary>Lesser-than-or-equal comparison operator.</summary>
    /// <param name="left">The first instance to compare.</param>
    /// <param name="right">The second instance to compare.</param>
    /// <returns>The result of the operation.</returns>
    public static bool operator<=(IntervalQuality left,
                                  IntervalQuality right)
      => left.CompareTo(right) <= 0;

    /// <summary>Greater-than-or-equal comparison operator.</summary>
    /// <param name="left">The first instance to compare.</param>
    /// <param name="right">The second instance to compare.</param>
    /// <returns>The result of the operation.</returns>
    public static bool operator>=(IntervalQuality left,
                                  IntervalQuality right)
      => left.CompareTo(right) >= 0;

    /// <summary>Addition operator.</summary>
    /// <param name="quality">The first value.</param>
    /// <param name="steps">A number of semitones to add to it.</param>
    /// <returns>The result of the operation.</returns>
    public static IntervalQuality operator+(IntervalQuality quality,
                                            int steps)
      => quality.Add(steps);

    /// <summary>Increment operator.</summary>
    /// <param name="quality">The note.</param>
    /// <returns>The result of the operation.</returns>
    public static IntervalQuality operator++(IntervalQuality quality) => quality.Add(1);

    /// <summary>Subtraction operator.</summary>
    /// <param name="quality">The first value.</param>
    /// <param name="steps">A number of semitones to subtract from it.</param>
    /// <returns>The result of the operation.</returns>
    public static IntervalQuality operator-(IntervalQuality quality,
                                            int steps)
      => quality.Subtract(steps);

    /// <summary>Decrement operator.</summary>
    /// <param name="quality">The note.</param>
    /// <returns>The result of the operation.</returns>
    public static IntervalQuality operator--(IntervalQuality quality) => quality.Subtract(1);

    #endregion
  }
}
