// Module Name: NoteName.cs
// Project:     Bach.Model
// Copyright (c) 2012, 2019  Eddie Velasquez.
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
// portions of the Software.
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
  using Internal;
  using Contract = Internal.Contract;

  /// <summary>
  ///   A NoteName represents a basic diatonic pitch
  ///   according to the English naming convention for
  ///   the 12 note chromatic scale.
  /// </summary>
  public struct NoteName
    : IEquatable<NoteName>,
      IComparable<NoteName>
  {
    #region Constants

    private const int NoteNameCount = 7;
    public static readonly NoteName C = new NoteName(0);
    public static readonly NoteName D = new NoteName(1);
    public static readonly NoteName E = new NoteName(2);
    public static readonly NoteName F = new NoteName(3);
    public static readonly NoteName G = new NoteName(4);
    public static readonly NoteName A = new NoteName(5);
    public static readonly NoteName B = new NoteName(6);

    // ReSharper disable once StringLiteralTypo
    private static readonly string s_names = "CDEFGAB";

    #endregion

    #region Data Members

    private readonly int _value;

    #endregion

    #region Constructors

    private NoteName(int value)
    {
      Contract.Requires<ArgumentOutOfRangeException>(value >= 0 && value < NoteNameCount);
      _value = value;
    }

    #endregion

    #region IComparable<NoteName> Members

    /// <inheritdoc />
    public int CompareTo(NoteName other) => _value.CompareTo(other._value);

    #endregion

    #region IEquatable<NoteName> Members

    /// <inheritdoc />
    public bool Equals(NoteName other) => _value == other._value;

    #endregion

    #region Public Methods

    /// <summary>Adds a number of steps to a note name.</summary>
    /// <param name="steps">The number of steps to add.</param>
    /// <returns>A NoteName.</returns>
    [Pure]
    public NoteName Add(int steps)
    {
      var result = (NoteName)ArrayExtensions.WrapIndex(NoteNameCount, _value + steps);
      return result;
    }

    /// <summary>Subtracts a number of steps from a note name.</summary>
    /// <param name="steps">The number of steps to subtract.</param>
    /// <returns>A NoteName.</returns>
    [Pure]
    public NoteName Subtract(int steps) => Add(-steps);

    /// <summary>Returns to number of semitones between two note names.</summary>
    /// <param name="name">The last noted name.</param>
    /// <returns>A NoteName.</returns>
    [Pure]
    public int Subtract(NoteName name) => (int)Add(-(int)name);

    /// <summary>Attempts to parse a NoteName from the given string.</summary>
    /// <param name="s">The value to parse.</param>
    /// <param name="noteName">[out] The note name.</param>
    /// <returns>True if it succeeds, false if it fails.</returns>
    public static bool TryParse(string s,
                                out NoteName noteName)
    {
      if( string.IsNullOrWhiteSpace(s) )
      {
        noteName = C;
        return false;
      }

      int value = s_names.IndexOf(s.Substring(0, 1), StringComparison.InvariantCultureIgnoreCase);
      if( value == -1 )
      {
        noteName = C;
        return false;
      }

      noteName = new NoteName(value);
      return true;
    }

    /// <summary>Parses the provided string.</summary>
    /// <exception cref="FormatException">Thrown when the provided string doesn't represent a a NoteName.</exception>
    /// <exception cref="ArgumentNullException">Thrown when a null string is provided.</exception>
    /// <exception cref="ArgumentException">Thrown when an empty string is provided.</exception>
    /// <param name="value">The value to parse.</param>
    /// <returns>A Note.</returns>
    public static NoteName Parse(string value)
    {
      Contract.RequiresNotNullOrEmpty(value, "Must provide a value");

      if( !TryParse(value, out NoteName result) )
      {
        throw new FormatException($"{value} is not a valid note name");
      }

      return result;
    }

    #endregion

    #region Overrides

    /// <inheritdoc />
    public override bool Equals(object obj)
    {
      if( obj is null )
      {
        return false;
      }

      return obj is NoteName other && Equals(other);
    }

    /// <inheritdoc />
    public override int GetHashCode() => _value;

    /// <inheritdoc />
    public override string ToString() => s_names[_value].ToString();

    #endregion

    #region Operators

    /// <summary>Explicit cast that converts the given NoteName to an int.</summary>
    /// <param name="noteName">The note name.</param>
    /// <returns>The result of the operation.</returns>
    public static explicit operator int(NoteName noteName) => noteName._value;

    /// <summary>Explicit cast that converts the given int to a NoteName.</summary>
    /// <param name="value">The value.</param>
    /// <returns>The result of the operation.</returns>
    public static explicit operator NoteName(int value) => new NoteName(value);

    /// <summary>Equality operator.</summary>
    /// <param name="left">The first instance to compare.</param>
    /// <param name="right">The second instance to compare.</param>
    /// <returns>The result of the operation.</returns>
    public static bool operator==(NoteName left,
                                  NoteName right)
      => left.Equals(right);

    /// <summary>Inequality operator.</summary>
    /// <param name="left">The first instance to compare.</param>
    /// <param name="right">The second instance to compare.</param>
    /// <returns>The result of the operation.</returns>
    public static bool operator!=(NoteName left,
                                  NoteName right)
      => !left.Equals(right);

    /// <summary>Lesser-than comparison operator.</summary>
    /// <param name="left">The first instance to compare.</param>
    /// <param name="right">The second instance to compare.</param>
    /// <returns>The result of the operation.</returns>
    public static bool operator<(NoteName left,
                                 NoteName right)
      => left.CompareTo(right) < 0;

    /// <summary>Greater-than comparison operator.</summary>
    /// <param name="left">The first instance to compare.</param>
    /// <param name="right">The second instance to compare.</param>
    /// <returns>The result of the operation.</returns>
    public static bool operator>(NoteName left,
                                 NoteName right)
      => left.CompareTo(right) > 0;

    /// <summary>Lesser-than-or-equal comparison operator.</summary>
    /// <param name="left">The first instance to compare.</param>
    /// <param name="right">The second instance to compare.</param>
    /// <returns>The result of the operation.</returns>
    public static bool operator<=(NoteName left,
                                  NoteName right)
      => left.CompareTo(right) <= 0;

    /// <summary>Greater-than-or-equal comparison operator.</summary>
    /// <param name="left">The first instance to compare.</param>
    /// <param name="right">The second instance to compare.</param>
    /// <returns>The result of the operation.</returns>
    public static bool operator>=(NoteName left,
                                  NoteName right)
      => left.CompareTo(right) >= 0;

    /// <summary>Subtraction operator.</summary>
    /// <param name="a">The first value.</param>
    /// <param name="b">The second value.</param>
    /// <returns>The result of the operation.</returns>
    public static int operator-(NoteName a,
                                NoteName b)
      => a.Subtract(b);

    /// <summary>Addition operator.</summary>
    /// <param name="noteName">The first value.</param>
    /// <param name="semitoneCount">A number of semitones to add to it.</param>
    /// <returns>The result of the operation.</returns>
    public static NoteName operator+(NoteName noteName,
                                     int semitoneCount)
      => noteName.Add(semitoneCount);

    /// <summary>Increment operator.</summary>
    /// <param name="noteName">The note.</param>
    /// <returns>The result of the operation.</returns>
    public static NoteName operator++(NoteName noteName) => noteName.Add(1);

    /// <summary>Subtraction operator.</summary>
    /// <param name="noteName">The first value.</param>
    /// <param name="semitoneCount">A number of semitones to subtract from it.</param>
    /// <returns>The result of the operation.</returns>
    public static NoteName operator-(NoteName noteName,
                                     int semitoneCount)
      => noteName.Subtract(semitoneCount);

    /// <summary>Decrement operator.</summary>
    /// <param name="noteName">The note.</param>
    /// <returns>The result of the operation.</returns>
    public static NoteName operator--(NoteName noteName) => noteName.Subtract(1);

    #endregion
  }
}
