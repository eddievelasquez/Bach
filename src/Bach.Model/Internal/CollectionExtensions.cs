// Module Name: CollectionExtensions.cs
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
using System.Linq;
using System.Text;

namespace Bach.Model.Internal;

/// <summary>
///   Provides extension methods for collections of intervals and steps.
/// </summary>
public static class CollectionExtensions
{
  private const char STEP_SEPARATOR = '-';
  private const string STANDARD_TO_STRING_FORMAT = "S";

  #region Implementation

  extension<T>(
    IReadOnlyCollection<T> collection )
  {
    #region Properties

    /// <summary>
    ///   Gets a value indicating whether the collection is empty.
    /// </summary>
    public bool Empty => collection.Count == 0;

    #endregion
  }

  extension(
    IEnumerable<Interval> intervals )
  {
    #region Public Methods

    /// <summary>
    ///   Converts a collection of intervals to a collection of steps.
    /// </summary>
    /// <returns></returns>
    public IEnumerable<int> GetSemitoneSteps()
    {
      ArgumentNullException.ThrowIfNull( intervals );

      // Must call a core method to enable checking for null before the first yield
      return intervals.GetSemitoneStepsCore();
    }

    #endregion

    #region Implementation

    private IEnumerable<int> GetSemitoneStepsCore()
    {
      var lastCount = 0;

      // Skip the first interval since it is always 0, and we don't want to include it in the steps
      foreach( var interval in intervals.Skip( 1 ) )
      {
        var semitoneCount = interval.SemitoneCount;
        var step = semitoneCount - lastCount;
        yield return step;

        lastCount = semitoneCount;
      }

      // Add the final step to complete the octave
      yield return Constants.OctaveSemitoneCount - lastCount;
    }

    #endregion
  }

  #endregion

  extension(
    IEnumerable<int> semitoneSteps )
  {
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
      format ??= STANDARD_TO_STRING_FORMAT;

      var buf = new StringBuilder();

      foreach( var c in format )
      {
        switch( c )
        {
          case 'N':
            buf.Append( string.Join( STEP_SEPARATOR, semitoneSteps ) );
            break;

          case 'S':
            buf.Append( string.Join( STEP_SEPARATOR, semitoneSteps.Select( ToUpperCase ) ) );
            break;

          case 's':
            buf.Append( string.Join( STEP_SEPARATOR, semitoneSteps.Select( ToLowerCase ) ) );
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
  }
}
