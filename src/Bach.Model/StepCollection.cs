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

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using Bach.Model.Internal;

namespace Bach.Model;

/// <summary>
/// Represents a collection of musical steps that span an octave.
/// </summary>
/// <remarks>
/// <see cref="StepCollection"/> only enforces shape-level rules at construction: each step
/// character must map to a known step size, and (when parsing) the number of steps must fall
/// within the supported range. It does <b>not</b> guarantee music-theory invariants such as
/// octave closure (steps summing to 12 semitones) or a valid scale cardinality. The only
/// supported way to get a <see cref="StepCollection"/> that is guaranteed to represent a
/// valid scale is through <see cref="ScaleFormulaBuilder.Build"/>, which performs that
/// validation before constructing the owning <see cref="ScaleFormula"/>. A <see cref="StepCollection"/>
/// returned directly from a constructor or a <c>Parse</c>/<c>TryParse</c> call should be treated
/// as unverified until it has passed through <see cref="ScaleFormulaBuilder"/>.
/// </remarks>
public static class StepCollection
{
  #region Constants

  private const char STEP_SEPARATOR = '-';

  #endregion

  /// <summary>
  /// Parses a string into a <see cref="List{Int32}"/>.
  /// </summary>
  /// <param name="s">The string to parse.</param>
  /// <returns>The parsed <see cref="List{Int32}"/>.</returns>
  /// <exception cref="ArgumentException">Thrown when the span is empty.</exception>
  /// <exception cref="FormatException">Thrown when the span is not in a valid format.</exception>
  public static List<int> Parse(
    string s )
  {
    return Parse( s.AsSpan() );
  }

  /// <summary>
  /// Parses a <see cref="ReadOnlySpan{T}"/> of characters into a <see cref="List{Int32}"/>.
  /// </summary>
  /// <param name="span">The span of characters to parse.</param>
  /// <returns>The parsed <see cref="List{Int32}"/>.</returns>
  /// <exception cref="ArgumentException">Thrown when the span is empty.</exception>
  /// <exception cref="FormatException">Thrown when the span is not in a valid format.</exception>
  /// <remarks>
  /// Parsing only validates that the number of steps is within the supported range and that each
  /// step character is recognized; it does not verify octave closure (steps summing to 12
  /// semitones). Use <see cref="ScaleFormulaBuilder"/> to get a validated scale formula.
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
  /// Tries to parse a string into a <see cref="List{Int32}"/>.
  /// </summary>
  /// <param name="s">The string to parse.</param>
  /// <param name="steps">The resulting <see cref="List{Int32}"/> if parsing is successful.</param>
  /// <returns>True if parsing is successful; otherwise, false.</returns>
  public static bool TryParse(
    [NotNullWhen( true )] string? s,
    [NotNullWhen( true )] out List<int>? steps )
  {
    return TryParse( s.AsSpan(), out steps );
  }

  /// <summary>
  /// Tries to parse a <see cref="ReadOnlySpan{T}"/> of characters into a <see cref="List{Int32}"/>.
  /// </summary>
  /// <param name="span">The span of characters to parse.</param>
  /// <param name="steps">The resulting <see cref="List{Int32}"/> if parsing is successful.</param>
  /// <returns>True if parsing is successful; otherwise, false.</returns>
  /// <remarks>
  /// Parsing only validates that the number of steps is within the supported range and that each
  /// step character is recognized; it does not verify octave closure (steps summing to 12
  /// semitones). Use <see cref="ScaleFormulaBuilder"/> to get a validated scale formula.
  /// </remarks>
  public static bool TryParse(
    ReadOnlySpan<char> span,
    [NotNullWhen( true )] out List<int>? steps )
  {
    steps = null;

    var sepCount = span.Count( STEP_SEPARATOR );

    if( sepCount < Constants.MinimumScaleStepCount - 1 || sepCount > Constants.MaximumScaleStepCount - 1 )
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
