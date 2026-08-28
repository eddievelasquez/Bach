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
using Bach.Model.Internal;

namespace Bach.Model;

/// <summary>
///   Represents a collection of musical steps that span an octave.
/// </summary>
public class StepCollection
  : IReadOnlyCollection<int>,
    ISpanParsable<StepCollection>
{
  private const char STEP_SEPARATOR = '-';

  #region Fields

  private readonly int[] _steps;

  #endregion

  #region Constructors

  /// <summary>
  ///   Represents a collection of musical steps that span an octave.
  /// </summary>
  /// <param name="steps">The collection of step values. Must contain 2-12 steps with values 1-3 that sum to 12.</param>
  public StepCollection(
    ICollection<int> steps )
  {
    var result = Validate( steps );

    if( !result.IsSuccess )
    {
      throw new ArgumentOutOfRangeException( nameof( steps ), result.Error );
    }

    _steps = result.Value;
  }

  #endregion

  #region Properties

  /// <summary>
  ///   Gets the number of steps in the collection.
  /// </summary>
  public int Count => _steps.Length;

  #endregion

  #region Public Methods

  /// <inheritdoc/>
  public IEnumerator<int> GetEnumerator()
  {
    return _steps.AsEnumerable()
                 .GetEnumerator();
  }

  /// <inheritdoc/>
  IEnumerator IEnumerable.GetEnumerator()
  {
    return GetEnumerator();
  }

  /// <summary>
  ///   Converts the step pattern into a sequence of ascending intervals, beginning with the unison.
  /// </summary>
  /// <returns>The intervals represented by the step pattern.</returns>
  public IntervalCollection ToIntervals()
  {
    var intervals = new List<Interval>( _steps.Length )
    {
      Interval.Unison
    };

    var semitones = 0;

    for( var degree = 1; degree < _steps.Length; degree++ )
    {
      semitones += _steps[degree - 1];
      intervals.Add( Interval.FromSemitones( semitones ) );
    }

    return new IntervalCollection( intervals );
  }

  /// <summary>
  ///   Parses a string into a <see cref="StepCollection"/>.
  /// </summary>
  /// <param name="s">The string to parse.</param>
  /// <returns>The parsed <see cref="StepCollection"/>.</returns>
  /// <exception cref="ArgumentException">Thrown when the span is empty.</exception>
  /// <exception cref="FormatException">Thrown when the span is not in a valid format.</exception>
  public static StepCollection Parse(
    string s )
  {
    return Parse( s.AsSpan(), null );
  }

  /// <summary>
  ///   Parses a string into a <see cref="StepCollection"/>.
  /// </summary>
  /// <param name="s">The string to parse.</param>
  /// <param name="provider">An optional format provider.</param>
  /// <returns>The parsed <see cref="StepCollection"/>.</returns>
  /// <exception cref="ArgumentException">Thrown when the span is empty.</exception>
  /// <exception cref="FormatException">Thrown when the span is not in a valid format.</exception>
  public static StepCollection Parse(
    string s,
    IFormatProvider? provider )
  {
    ArgumentNullException.ThrowIfNull( s );
    return Parse( s.AsSpan(), provider );
  }

  /// <summary>
  ///   Parses a <see cref="ReadOnlySpan{T}"/> of characters into a <see cref="StepCollection"/>.
  /// </summary>
  /// <param name="span">The span of characters to parse.</param>
  /// <param name="provider">An optional format provider.</param>
  /// <returns>The parsed <see cref="StepCollection"/>.</returns>
  /// <exception cref="ArgumentException">Thrown when the span is empty.</exception>
  /// <exception cref="FormatException">Thrown when the span is not in a valid format.</exception>
  public static StepCollection Parse(
    ReadOnlySpan<char> span,
    IFormatProvider? provider )
  {
    if( span.IsEmpty )
    {
      throw new ArgumentException( "The value cannot be empty.", nameof( span ) );
    }

    return TryParse( span, provider, out var steps )
      ? steps
      : throw new FormatException( "The value is not in a valid format." );
  }

  /// <summary>
  ///   Tries to parse a string into a <see cref="StepCollection"/>.
  /// </summary>
  /// <param name="s">The string to parse.</param>
  /// <param name="steps">The resulting <see cref="StepCollection"/> if parsing is successful.</param>
  /// <returns>True if parsing is successful; otherwise, false.</returns>
  public static bool TryParse(
    [NotNullWhen( true )] string? s,
    [NotNullWhen( true )] out StepCollection? steps )
  {
    return TryParse( s.AsSpan(), null, out steps );
  }

  /// <summary>
  ///   Tries to parse a string into a <see cref="StepCollection"/>.
  /// </summary>
  /// <param name="s">The string to parse.</param>
  /// <param name="provider">An optional format provider.</param>
  /// <param name="steps">The resulting <see cref="StepCollection"/> if parsing is successful.</param>
  /// <returns>True if parsing is successful; otherwise, false.</returns>
  public static bool TryParse(
    [NotNullWhen( true )] string? s,
    IFormatProvider? provider,
    [NotNullWhen( true )] out StepCollection? steps )
  {
    return TryParse( s.AsSpan(), provider, out steps );
  }

  /// <summary>
  ///   Tries to parse a <see cref="ReadOnlySpan{T}"/> of characters into a <see cref="StepCollection"/>.
  /// </summary>
  /// <param name="span">The span of characters to parse.</param>
  /// <param name="provider">An optional format provider.</param>
  /// <param name="steps">The resulting <see cref="StepCollection"/> if parsing is successful.</param>
  /// <returns>True if parsing is successful; otherwise, false.</returns>
  public static bool TryParse(
    ReadOnlySpan<char> span,
    IFormatProvider? provider,
    [NotNullWhen( true )] out StepCollection? steps )
  {
    steps = null;

    var sepCount = span.Count( STEP_SEPARATOR );

    if( sepCount < Constants.MinimumScaleStepCount - 1 || sepCount > Constants.MaximumScaleStepCount - 1 )
    {
      return false;
    }

    // Allocate a stack-allocated array of ranges to hold the start and end indices of each step in the span.
    Span<Range> ranges = stackalloc Range[sepCount + 1];
    var rangeCount = span.Split( ranges, STEP_SEPARATOR, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries );
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
        '3'               => 3,
        _                 => 0
      };

      // If the step value is 0, it means the character was invalid, so return false.
      if( step == 0 )
      {
        return false;
      }

      tmp.Add( step );
    }

    // Validate the collection of steps to ensure it meets the required criteria.
    var result = Validate( tmp );

    if( !result.IsSuccess )
    {
      return false;
    }

    steps = new StepCollection( result.Value );
    return true;
  }

  #endregion

  #region Implementation

  /// <summary>
  ///   Validates the provided collection of steps to ensure it meets the required criteria.
  /// </summary>
  /// <param name="steps">The collection of steps to validate.</param>
  /// <returns>The validated array of steps.</returns>
  /// <exception cref="ArgumentOutOfRangeException">
  ///   Thrown when the steps collection doesn't meet the validation requirements.
  /// </exception>
  /// <remarks>
  ///   This method ensures that the steps collection contains between 5 and 12 steps, each step is between 1 and 3, and
  ///   the sum of all steps equals 12.
  /// </remarks>
  private static Result<int[]> Validate(
    ICollection<int> steps )
  {
    if( steps.Count < Constants.MinimumScaleStepCount )
    {
      return Result<int[]>.Fail( $"A Step Collection must contain at least {Constants.MinimumScaleStepCount} steps." );
    }

    if( steps.Count > Constants.MaximumScaleStepCount )
    {
      return Result<int[]>.Fail( $"A Step Collection cannot contain more than {Constants.MaximumScaleStepCount} steps." );
    }

    if( steps.Any( step => step < Constants.MinimumScaleStepSize || step > Constants.MaximumScaleStepSize ) )
    {
      return Result<int[]>.Fail(
        $"All steps must be between {Constants.MinimumScaleStepSize} and {Constants.MaximumScaleStepSize}."
      );
    }

    if( steps.Sum() != Constants.OctaveSemitoneCount )
    {
      return Result<int[]>.Fail( $"The sum of all steps must be {Constants.OctaveSemitoneCount}." );
    }

    return Result<int[]>.Ok( [.. steps] );
  }

  #endregion
}
