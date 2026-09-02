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
public class StepCollection
  : IReadOnlyCollection<int>,
    ISpanParsable<StepCollection>,
    IFormattable
{
  #region Constants

  private const char STEP_SEPARATOR = '-';
  private const string STANDARD_TO_STRING_FORMAT = "S";

  #endregion

  #region Fields

  private readonly int[] _steps;

  #endregion

  #region Constructors

  /// <summary>
  /// Represents a collection of musical steps that span an octave.
  /// </summary>
  /// <param name="steps">The collection of step values. Must contain 2-12 steps with values 1-3 that sum to 12.</param>
  public StepCollection(
    IEnumerable<int> steps )
  {
    ArgumentNullException.ThrowIfNull( steps );
    _steps = [.. steps];
  }

  #endregion

  #region Properties

  /// <summary>
  /// Gets the number of steps in the collection.
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
  /// Parses a string into a <see cref="StepCollection"/>.
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
  /// Parses a string into a <see cref="StepCollection"/>.
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
  /// Parses a <see cref="ReadOnlySpan{T}"/> of characters into a <see cref="StepCollection"/>.
  /// </summary>
  /// <param name="span">The span of characters to parse.</param>
  /// <returns>The parsed <see cref="StepCollection"/>.</returns>
  /// <exception cref="ArgumentException">Thrown when the span is empty.</exception>
  /// <exception cref="FormatException">Thrown when the span is not in a valid format.</exception>
  public static StepCollection Parse(
    ReadOnlySpan<char> span )
  {
    return Parse( span, null );
  }

  /// <summary>
  /// Parses a <see cref="ReadOnlySpan{T}"/> of characters into a <see cref="StepCollection"/>.
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
  /// Converts the step collection to its string representation using the standard format.
  /// </summary>
  /// <returns>The string representation of the step collection.</returns>
  public override string ToString()
  {
    return ToString( STANDARD_TO_STRING_FORMAT, null );
  }

  /// <summary>
  /// Converts the step collection to its string representation using the specified format and format provider.
  /// </summary>
  /// <param name="format">The format string.</param>
  /// <returns>The string representation of the step collection.</returns>
  /// <remarks>
  ///
  /// <para>Format specifiers:</para>
  ///
  /// <para>"N": Numeric pattern. e.g. "1-2-3".</para>
  ///
  /// <para>"S": Standard uppercase pattern. e.g. "W-W-H-W".</para>
  ///
  /// <para>"s": Standard lowercase pattern. e.g. "w-w-h-w".</para>
  /// </remarks>
  public string ToString(
    string? format )
  {
    return ToString( format, null );
  }

  /// <summary>
  /// Converts the step collection to its string representation using the specified format and format provider.
  /// </summary>
  /// <param name="format">The format string.</param>
  /// <param name="formatProvider">The format provider.</param>
  /// <returns>The string representation of the step collection.</returns>
  /// <remarks>
  ///
  /// <para>Format specifiers:</para>
  ///
  /// <para>"N": Numeric pattern. e.g. "1-2-3".</para>
  ///
  /// <para>"S": Standard uppercase pattern. e.g. "W-W-H-W".</para>
  ///
  /// <para>"s": Standard lowercase pattern. e.g. "w-w-h-w".</para>
  /// </remarks>
  public string ToString(
    string? format,
    IFormatProvider? formatProvider )
  {
    format ??= STANDARD_TO_STRING_FORMAT;

    var buf = new StringBuilder();

    foreach( var c in format )
    {
      switch( c )
      {
        case 'N':
          buf.Append( string.Join( STEP_SEPARATOR, _steps ) );
          break;

        case 'S':
          buf.Append( string.Join( STEP_SEPARATOR, _steps.Select( ToUpperCase ) ) );
          break;

        case 's':
          buf.Append( string.Join( STEP_SEPARATOR, _steps.Select( ToLowerCase ) ) );
          break;

        default:
          buf.Append( c );
          break;
      }
    }

    return buf.ToString();

    static char ToUpperCase(
      int step )
    {
      return step switch
      {
        1 => 'H',
        2 => 'W',
        _ => (char) ( '0' + step )
      };
    }

    static char ToLowerCase(
      int step )
    {
      return step switch
      {
        1 => 'h',
        2 => 'w',
        _ => (char) ( '0' + step )
      };
    }
  }

  /// <summary>
  /// Tries to parse a string into a <see cref="StepCollection"/>.
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
  /// Tries to parse a string into a <see cref="StepCollection"/>.
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
  /// Tries to parse a <see cref="ReadOnlySpan{T}"/> of characters into a <see cref="StepCollection"/>.
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

    steps = new StepCollection( tmp );
    return true;
  }

  #endregion
}
