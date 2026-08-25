// Module Name: IntervalQuantityExtensions.cs
// Project:     Bach.Model
// Copyright (c) 2012, 2026  Eddie Velasquez.
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

namespace Bach.Model;

/// <summary>
///   Extension methods for the <see cref="IntervalQuantity"/> enumeration.
/// </summary>
public static class IntervalQuantityExtensions
{
  #region Implementation

  extension(
    IntervalQuantity quantity )
  {
    #region Properties

    /// <summary>
    ///   Determines whether the interval quantity is a perfect interval (unison, fourth, fifth, or octave).
    /// </summary>
    public bool IsPerfectBased =>
      (IntervalQuantity) ( (int) quantity % (int) IntervalQuantity.Seventh ) is IntervalQuantity.Unison
      or IntervalQuantity.Fourth
      or IntervalQuantity.Fifth;

    /// <summary>
    ///   Determines whether the interval quantity is a major or minor interval (second, third, sixth, or seventh).
    /// </summary>
    public bool IsMajorBased => !quantity.IsPerfectBased;

    /// <summary>
    ///   Determines whether the interval quantity is a simple interval (unison, second, third, fourth, fifth, sixth,
    ///   seventh, or octave).
    /// </summary>
    public bool IsSimpleInterval => (int) quantity <= (int) IntervalQuantity.Octave;

    /// <summary>
    ///   Determines whether the interval quantity is a compound interval (ninth, tenth, eleventh, twelfth,
    ///   thirteenth, or fourteenth).
    /// </summary>
    public bool IsCompoundInterval => (int) quantity > (int) IntervalQuantity.Octave;

    /// <summary>
    ///   Gets the inverse of the interval quantity.
    /// </summary>
    /// <remarks>
    ///   The sum of the interval quantities of an interval and its inversion is always 9 (e.g., a 3rd inverts to a 6th,
    ///   a 2nd inverts to a 7th, etc.). Compound intervals are treated as their simple equivalents (e.g., a 10th inverts
    ///   to a 3rd, an 11th inverts to a 4th, etc.).
    /// </remarks>
    public IntervalQuantity Inverse => (IntervalQuantity) ( 9 - ( ( ( (int) quantity - 1 ) % NoteName.TotalCount ) + 1 ) );

    #endregion

    #region Public Methods

    /// <summary>
    ///   Tries to parse a string representation of an interval quantity into an <see cref="IntervalQuantity"/> value.
    /// </summary>
    /// <param name="span">The span of characters to parse.</param>
    /// <param name="provider">The format provider.</param>
    /// <param name="intervalQuantity">
    ///   When this method returns, contains the parsed <see cref="IntervalQuantity"/> value if the parsing succeeded;
    ///   otherwise, <see cref="IntervalQuantity.Undefined"/>.
    /// </param>
    /// <param name="tail">When this method returns, contains the remaining unparsed characters in the span.</param>
    /// <returns><c>true</c> if the parsing succeeded; otherwise, <c>false</c>.</returns>
    public static bool TryParse(
      ReadOnlySpan<char> span,
      IFormatProvider? provider,
      out IntervalQuantity intervalQuantity,
      out ReadOnlySpan<char> tail )
    {
      tail = span.TrimStart();

      // Must provide at least 1 digit
      if( tail.IsEmpty )
      {
        intervalQuantity = IntervalQuantity.Undefined;
        return false;
      }

      // Interval quantities are only 1 or 2 digits long; anything beyond that is ignored.
      var numberLen = 1;

      if( tail.Length > 1 && char.IsDigit( tail[1] ) )
      {
        numberLen = 2;
      }

      if( !char.IsDigit( tail[0] ) || !int.TryParse( tail[..numberLen], out var number ) || number < 1 || number > 14 )
      {
        intervalQuantity = IntervalQuantity.Undefined;
        return false;
      }

      intervalQuantity = (IntervalQuantity) number;
      tail = tail[numberLen..];
      return true;
    }

    #endregion
  }

  #endregion
}
