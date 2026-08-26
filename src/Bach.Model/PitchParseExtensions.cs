// Module Name: PitchParseExtensions.cs
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

namespace Bach.Model;

/// <summary>
///   Provides extension methods for parsing collections of <see cref="PitchClass"/> from strings or spans.
/// </summary>
internal static class PitchParseExtensions
{
  #region Implementation

  /// <param name="value">The value to parse.</param>
  extension(
    string value )
  {
    #region Public Methods

    /// <summary>Parses the provided string using the specified format provider.</summary>
    /// <param name="provider">The format provider.</param>
    /// <returns>The list of parsed pitch classes.</returns>
    /// <exception cref="FormatException">Thrown when the span contains invalid pitch class data.</exception>
    public List<PitchClass> ParsePitchClasses(
      IFormatProvider? provider = null )
    {
      ArgumentException.ThrowIfNullOrEmpty( value );

      return value.AsSpan()
                  .ParsePitchClasses( provider );
    }

    /// <summary>
    ///   Parses the provided string using the specified format provider.
    /// </summary>
    /// <param name="provider">The format provider.</param>
    /// <returns>The list of parsed pitches.</returns>
    /// <exception cref="FormatException">Thrown when the span contains invalid pitch data.</exception>
    public List<Pitch> ParsePitches(
      IFormatProvider? provider = null )
    {
      ArgumentException.ThrowIfNullOrEmpty( value );

      return value.AsSpan()
                  .ParsePitches( provider );
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
    /// <exception cref="FormatException">Thrown when the span contains invalid pitch class data.</exception>
    public List<PitchClass> ParsePitchClasses(
      IFormatProvider? provider = null )
    {
      return span.TryParsePitchClasses( provider, out var notes )
        ? notes
        : throw new FormatException( $"{span} contains invalid pitchClasses" );
    }

    /// <summary>
    ///   Parses the provided span using the specified format provider.
    /// </summary>
    /// <param name="provider">The format provider.</param>
    /// <returns>The list of parsed pitches.</returns>
    /// <exception cref="FormatException">Thrown when the span contains invalid pitch data.</exception>
    public List<Pitch> ParsePitches(
      IFormatProvider? provider = null )
    {
      return span.TryParsePitches( provider, out var notes )
        ? notes
        : throw new FormatException( $"{span} contains invalid pitches" );
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
    public bool TryParsePitchClasses(
      IFormatProvider? provider,
      [NotNullWhen( true )] out List<PitchClass>? result )
    {
      // We want to ensure that the entire string is consumed, so we check if the tail is empty after parsing.
      return span.TryParsePitchClasses( provider, out result, out var tail ) && tail.IsEmpty;
    }

    /// <summary>
    ///   Attempts to parse a pitch class collection from the given span using the specified format provider, returning any
    ///   unparsed tail.
    /// </summary>
    /// <param name="provider">The format provider.</param>
    /// <param name="pitchClasses">[out] The list of parsed pitch classes.</param>
    /// <param name="tail">[out] The unparsed tail.</param>
    /// <returns><c>true</c> if it succeeds, <c>false</c> if it fails.</returns>
    public bool TryParsePitchClasses(
      IFormatProvider? provider,
      [NotNullWhen( true )] out List<PitchClass>? pitchClasses,
      out ReadOnlySpan<char> tail )
    {
      return span.TryParsePitchesImpl( provider, out pitchClasses, out tail );
    }

    /// <summary>
    ///   Attempts to parse a pitch collection from the given span using the specified format provider.
    /// </summary>
    /// <param name="provider">The format provider.</param>
    /// <param name="result">[out] The list of parsed pitches.</param>
    /// <returns><c>true</c> if it succeeds, <c>false</c> if it fails.</returns>
    public bool TryParsePitches(
      IFormatProvider? provider,
      [NotNullWhen( true )] out List<Pitch>? result )
    {
      // We want to ensure that the entire string is consumed, so we check if the tail is empty after parsing.
      return span.TryParsePitches( provider, out result, out var tail ) && tail.IsEmpty;
    }

    /// <summary>
    ///   Attempts to parse a pitch collection from the given span using the specified format provider, returning any
    /// </summary>
    /// <param name="provider">The format provider.</param>
    /// <param name="pitchClasses">[out] The list of parsed pitches.</param>
    /// <param name="tail">[out] The unparsed tail.</param>
    /// <returns><c>true</c> if it succeeds, <c>false</c> if it fails.</returns>
    public bool TryParsePitches(
      IFormatProvider? provider,
      [NotNullWhen( true )] out List<Pitch>? pitchClasses,
      out ReadOnlySpan<char> tail )
    {
      return span.TryParsePitchesImpl( provider, out pitchClasses, out tail );
    }

    #endregion

    #region Implementation

    private bool TryParsePitchesImpl<TPitch>(
      IFormatProvider? provider,
      [NotNullWhen( true )] out List<TPitch>? pitches,
      out ReadOnlySpan<char> tail )
      where TPitch: IPitch<TPitch>
    {
      // An empty span is not valid
      if( span.IsEmpty )
      {
        pitches = null;
        tail = span;
        return false;
      }

      // Start
      tail = span.TrimStart();

      // Count the number of commas in the span to determine how many pitches we have.
      var sepCount = span.Count( ',' );

      // Allocate a stack-allocated array of ranges to hold the start and end indices of each pitch in the span.
      Span<Range> ranges = stackalloc Range[sepCount + 1];
      var rangeCount = tail.Split( ranges, ',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries );
      var tmp = new List<TPitch>( rangeCount );

      // Parse each pitch in the range and add it to the list.
      for( var i = 0; i < rangeCount; i++ )
      {
        if( !TPitch.TryParse( span[ranges[i]], provider, out var pitch ) )
        {
          pitches = null;
          return false;
        }

        tmp.Add( pitch );
      }

      // The tail is now past the last parsed pitch
      tail = tail[ranges[^1].End..];
      pitches = tmp;
      return true;
    }

    #endregion
  }

  #endregion
}
