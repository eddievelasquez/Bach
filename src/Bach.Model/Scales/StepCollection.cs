// Module Name: StepCollection.cs
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

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Bach.Model.Scales;

/// <summary>
/// Represents a sequence of musical steps that spans an octave.
/// </summary>
/// <remarks>
/// This type checks that each step has a known size and that parsed input has a supported number
/// of steps. It does not check that the steps span an octave or form a valid scale. Pass the steps
/// to <see cref="ScaleFormulaBuilder.Build"/> to validate a scale formula.
/// </remarks>
public static class StepCollection
{
  #region Constants

  private const char STEP_SEPARATOR = '-';

  #endregion

  /// <summary>
  /// Parses a string into a list of step sizes.
  /// </summary>
  /// <param name="s">The string that contains the steps.</param>
  /// <returns>The parsed step sizes.</returns>
  /// <exception cref="ArgumentException">Thrown when the span is empty.</exception>
  /// <exception cref="FormatException">Thrown when the span is not in a valid format.</exception>
  public static List<int> Parse(
    string s )
  {
    return Parse( s.AsSpan() );
  }

  /// <summary>
  /// Parses a character span into a list of step sizes.
  /// </summary>
  /// <param name="span">The span that contains the steps.</param>
  /// <returns>The parsed step sizes.</returns>
  /// <exception cref="ArgumentException">Thrown when the span is empty.</exception>
  /// <exception cref="FormatException">Thrown when the span is not in a valid format.</exception>
  /// <remarks>
  /// This method checks that the number of steps is in the supported range and that each step
  /// character is valid. It does not check that the steps span an octave. Use
  /// <see cref="ScaleFormulaBuilder.Build"/> to validate a scale formula.
  /// </remarks>
  public static List<int> Parse(
    ReadOnlySpan<char> span )
  {
    if( span.IsEmpty )
    {
      throw new ArgumentException( "The value cannot be empty.", nameof( span ) );
    }

    return TryParse( span, out var steps )
      ? steps
      : throw new FormatException( "The value is not in a valid format." );
  }

  /// <summary>
  /// Tries to parse a string into step sizes.
  /// </summary>
  /// <param name="s">The string that contains the steps.</param>
  /// <param name="steps">The parsed step sizes, or <see langword="null"/> if parsing fails.</param>
  /// <returns><see langword="true"/> if parsing succeeds; otherwise, <see langword="false"/>.</returns>
  public static bool TryParse(
    [NotNullWhen( true )] string? s,
    [NotNullWhen( true )] out List<int>? steps )
  {
    return TryParse( s.AsSpan(), out steps );
  }

  /// <summary>
  /// Tries to parse a character span into step sizes.
  /// </summary>
  /// <param name="span">The span that contains the steps.</param>
  /// <param name="steps">The parsed step sizes, or <see langword="null"/> if parsing fails.</param>
  /// <returns><see langword="true"/> if parsing succeeds; otherwise, <see langword="false"/>.</returns>
  /// <remarks>
  /// This method checks that the number of steps is in the supported range and that each step
  /// character is valid. It does not check that the steps span an octave. Use
  /// <see cref="ScaleFormulaBuilder.Build"/> to validate a scale formula.
  /// </remarks>
  public static bool TryParse(
    ReadOnlySpan<char> span,
    [NotNullWhen( true )] out List<int>? steps )
  {
    steps = null;

    var sepCount = span.Count( STEP_SEPARATOR );

    if( sepCount < Constants.MinimumScaleIntervalCount - 1 || sepCount > Constants.MaximumScaleIntervalCount - 1 )
    {
      return false;
    }

    // Allocate a stack-allocated array of ranges to hold the start and end indices of each step in the span.
    Span<Range> ranges = stackalloc Range[sepCount + 1];

    var rangeCount = span.Split(
      ranges,
      STEP_SEPARATOR,
      StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries
    );

    var tmp = new List<int>( rangeCount );

    for( var i = 0; i < rangeCount; i++ )
    {
      var range = span[ranges[i]];

      // Validate that the range is a single character representing a step.
      if( range.Length != 1 )
      {
        return false;
      }

      // Map the character to a step value (1, 2, or 3) based on the character.
      var step = range[0] switch
      {
        'H' or 'h' or '1' => 1,
        'W' or 'w' or '2' => 2,
        '3' => 3,
        '4' => 4,
        _ => 0
      };

      // If the step value is 0, it means the character was invalid, so return false.
      if( step == 0 )
      {
        return false;
      }

      tmp.Add( step );
    }

    steps = tmp;
    return true;
  }
}
