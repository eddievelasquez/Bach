// Module Name: PitchClassParseExtensions.cs
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

using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

/// <summary>
///   Provides extension methods for parsing collections of <see cref="PitchClass" /> from strings or spans.
/// </summary>
internal static class PitchClassParseExtensions
{
  #region Implementation

  /// <param name="value">The value to parse.</param>
  extension(
    string value )
  {
    #region Public Methods

    /// <summary>Parses the provided string using the specified format provider.</summary>
    /// <param name="provider">The format provider.</param>
    /// <returns>A PitchClassCollection.</returns>
    public List<PitchClass> ParsePitchClasses(
      IFormatProvider? provider = null )
    {
      ArgumentException.ThrowIfNullOrEmpty( value );

      return value.AsSpan()
                  .ParsePitchClasses( provider );
    }

    #endregion
  }

  /// <param name="span">The span to parse.</param>
  extension(
    ReadOnlySpan<char> span )
  {
    #region Public Methods

    /// <summary>Parses the provided span using the specified format provider.</summary>
    /// <param name="provider">The format provider.</param>
    /// <returns>The list of parsed pitch classes.</returns>
    public List<PitchClass> ParsePitchClasses(
      IFormatProvider? provider = null )
    {
      return span.TryParse( provider, out var notes )
        ? notes
        : throw new FormatException( $"{span} contains invalid pitchClasses" );
    }

    #endregion
  }

  /// <param name="span">The span to parse.</param>
  extension(
    ReadOnlySpan<char> span )
  {
    #region Public Methods

    /// <summary>Attempts to parse a pitch class collection from the given span using the specified format provider.</summary>
    /// <param name="provider">The format provider.</param>
    /// <param name="result">[out] The list of parsed pitch classes.</param>
    /// <returns><c>true</c> if it succeeds, <c>false</c> if it fails.</returns>
    public bool TryParse(
      IFormatProvider? provider,
      [NotNullWhen( true )] out List<PitchClass>? result )
    {
      // We want to ensure that the entire string is consumed, so we check if the tail is empty after parsing.
      return span.TryParse( provider, out result, out var tail ) && tail.IsEmpty;
    }

    /// <summary>
    ///   Attempts to parse a pitch class collection from the given span using the specified format provider, returning any
    ///   unparsed tail.
    /// </summary>
    /// <param name="provider">The format provider.</param>
    /// <param name="pitchClasses">[out] The list of parsed pitch classes.</param>
    /// <param name="tail">[out] The unparsed tail.</param>
    /// <returns><c>true</c> if it succeeds, <c>false</c> if it fails.</returns>
    public bool TryParse(
      IFormatProvider? provider,
      [NotNullWhen( true )] out List<PitchClass>? pitchClasses,
      out ReadOnlySpan<char> tail )
    {
      // An empty span is not valid
      if( span.IsEmpty )
      {
        pitchClasses = null;
        tail = ReadOnlySpan<char>.Empty;
        return false;
      }

      // Start
      tail = span.TrimStart();
      var tmp = new List<PitchClass>();

      // While there are more pitch classes
      while( !tail.IsEmpty )
      {
        // Try to parse the next pitch class
        if( !PitchClass.TryParse( tail, provider, out var note, out tail ) )
        {
          pitchClasses = null;
          return false;
        }

        tmp.Add( note );

        var separatorIndex = tail.IndexOf( ',' );

        if( separatorIndex < 0 )
        {
          // No more separators, so we should be at the end of the string. If not, it's an error.
          if( !tail.IsEmpty )
          {
            pitchClasses = null;
            tail = ReadOnlySpan<char>.Empty;
            return false;
          }

          break;
        }

        // Move past the separator and trim any leading whitespace
        tail = tail[( separatorIndex + 1 )..]
          .TrimStart();
      }

      // Ensure that we have at least one pitch class
      Debug.Assert( tmp.Count > 0 );

      pitchClasses = tmp;
      return true;
    }

    #endregion
  }

  #endregion
}
