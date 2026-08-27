// Module Name: IntervalQualityExtensions.cs
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
///   Extension methods for the <see cref="IntervalQuality"/> enum.
/// </summary>
public static class IntervalQualityExtensions
{
  #region Constants

  private static readonly string[] s_symbols = ["d", "m", "P", "M", "A"];
  private static readonly string[] s_shortName = ["dim", "min", "Perf", "Maj", "Aug"];
  private static readonly string[] s_longName = ["Diminished", "Minor", "Perfect", "Major", "Augmented"];

  #endregion

  #region Implementation

  extension(
    IntervalQuality intervalQuality )
  {
    #region Properties

    /// <summary>
    ///   Returns the symbol for the given interval quality.
    /// </summary>
    public string Symbol => s_symbols[(int) intervalQuality];

    /// <summary>
    ///   Returns the short name for the given interval quality.
    /// </summary>
    public string ShortName => s_shortName[(int) intervalQuality];

    /// <summary>
    ///   Returns the long name for the given interval quality.
    /// </summary>
    public string LongName => s_longName[(int) intervalQuality];

    /// <summary>
    ///   Returns the inversion of the given interval quality.
    /// </summary>
    public IntervalQuality Inversion => intervalQuality switch
    {
      IntervalQuality.Diminished => IntervalQuality.Augmented,
      IntervalQuality.Minor      => IntervalQuality.Major,
      IntervalQuality.Perfect    => IntervalQuality.Perfect,
      IntervalQuality.Major      => IntervalQuality.Minor,
      IntervalQuality.Augmented  => IntervalQuality.Diminished,
      _                          => throw new ArgumentOutOfRangeException( nameof( intervalQuality ), intervalQuality, null )
    };

    #endregion

    #region Public Methods

    /// <summary>
    ///   Adds the specified number of semitones to the given interval quality and returns the resulting interval quality.
    /// </summary>
    /// <param name="semitones">The number of semitones to add.</param>
    /// <returns>The resulting interval quality.</returns>
    public IntervalQuality Add(
      int semitones )
    {
      var quality = (IntervalQuality) ( (int) intervalQuality + semitones );

      ArgumentOutOfRangeException.ThrowIfLessThan( (int) quality, (int) IntervalQuality.Diminished );
      ArgumentOutOfRangeException.ThrowIfGreaterThan( (int) quality, (int) IntervalQuality.Augmented );

      return quality;
    }

    /// <summary>
    ///   Determines whether the specified interval quality is valid for the given interval quantity.
    /// </summary>
    /// <param name="quantity">The interval quantity.</param>
    /// <returns>True if the interval quality is valid for the given quantity; otherwise, false.</returns>
    public bool IsValidFor(
      IntervalQuantity quantity )
    {
      if( quantity.IsPerfectBased )
      {
        return intervalQuality == IntervalQuality.Diminished
               || intervalQuality == IntervalQuality.Perfect
               || intervalQuality == IntervalQuality.Augmented;
      }

      return intervalQuality == IntervalQuality.Diminished
             || intervalQuality == IntervalQuality.Minor
             || intervalQuality == IntervalQuality.Major
             || intervalQuality == IntervalQuality.Augmented;
    }

    /// <summary>
    ///   Parses the string representation of an interval quality.
    /// </summary>
    /// <param name="value">The string representation of the interval quality.</param>
    /// <param name="provider">The format provider.</param>
    /// <returns>The parsed <see cref="IntervalQuality"/>.</returns>
    /// <exception cref="FormatException">Thrown if the string is not a valid interval quality.</exception>
    public static IntervalQuality Parse(
      string value,
      IFormatProvider? provider = null )
    {
      if( !IntervalQuality.TryParse( value.AsSpan(), provider, out var quality, out _, out var tail ) || !tail.IsEmpty )
      {
        throw new FormatException( $"\"{value}\" is not a valid interval quality" );
      }

      return quality;
    }

    /// <summary>
    ///   Subtracts the specified number of semitones from the given interval quality and returns the resulting interval
    ///   quality.
    /// </summary>
    /// <param name="semitones">The number of semitones to subtract.</param>
    /// <returns>The resulting interval quality.</returns>
    public IntervalQuality Subtract(
      int semitones )
    {
      return intervalQuality.Add( -semitones );
    }

    /// <summary>
    ///   Converts the specified string representation of an interval quality to its <see cref="IntervalQuality"/>
    /// </summary>
    /// <param name="span">A read-only character span containing the interval quality to convert.</param>
    /// <param name="provider">The format provider.</param>
    /// <param name="quality">
    ///   When this method returns, contains the interval quality equivalent to the value contained in span, if the conversion
    ///   succeeded, or IntervalQuality.Perfect if the conversion failed.
    /// </param>
    /// <param name="alterationDegree">When this method returns, contains the alteration degree of the interval quality.</param>
    /// <param name="tail">When this method returns, contains the remaining unparsed portion of the span.</param>
    /// <returns>True if the conversion succeeded; otherwise, false.</returns>
    public static bool TryParse(
      ReadOnlySpan<char> span,
      IFormatProvider? provider,
      out IntervalQuality quality,
      out int alterationDegree,
      out ReadOnlySpan<char> tail )
    {
      tail = span.TrimStart();

      // TODO: Fix
      alterationDegree = 1;

      if( tail.IsEmpty )
      {
        quality = IntervalQuality.Perfect;
        alterationDegree = 1;
        return true;
      }

      var alteration = tail[0];

      switch( alteration )
      {
        case '°':
        case 'd':
          quality = IntervalQuality.Diminished;
          alterationDegree = CalcAlterationDegree( alteration, ref tail );
          return true;

        case 'm':
          quality = IntervalQuality.Minor;
          tail = tail[1..];
          return true;

        case 'P':
          quality = IntervalQuality.Perfect;
          tail = tail[1..];
          return true;

        case 'M':
          quality = IntervalQuality.Major;
          tail = tail[1..];
          return true;

        case '+':
        case 'A':
          quality = IntervalQuality.Augmented;
          alterationDegree = CalcAlterationDegree( alteration, ref tail );
          return true;

        default:
          quality = IntervalQuality.Perfect;
          return false;
      }

      static int CalcAlterationDegree(
        char alteration,
        ref ReadOnlySpan<char> span )
      {
        // Count the number of consecutive alterations (up to 3) to determine the alteration degree.
        var count = span.Count( alteration );
        var alterationDegree = Math.Clamp( count, 1, 3 );
        span = span[alterationDegree..];

        return alterationDegree;
      }
    }

    #endregion
  }

  #endregion
}
