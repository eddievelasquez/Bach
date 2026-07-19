// Module Name: PitchCollection.cs
// Project:     Bach.Model
// Copyright (c) 2012, 2023  Eddie Velasquez.
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

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

/// <summary>Collection of pitches.</summary>
public class PitchCollection
  : IReadOnlyList<Pitch>,
    IEquatable<PitchCollection>,
    ISpanParsable<PitchCollection>
{
  #region Fields

  private readonly Pitch[] _pitches;

  #endregion

  #region Constructors

  /// <summary>
  ///   Initializes a new instance of the <see cref="PitchCollection" /> class.
  /// </summary>
  /// <param name="notes">The array of pitches.</param>
  public PitchCollection(
    IEnumerable<Pitch> notes )
  {
    ArgumentNullException.ThrowIfNull( notes );
    _pitches = notes.ToArray();
  }

  #endregion

  #region Properties

  /// <inheritdoc />
  public int Count => _pitches.Length;

  /// <inheritdoc />
  public Pitch this[
    int index] => _pitches[index];

  #endregion

  #region Public Methods

  /// <inheritdoc />
  public bool Equals(
    PitchCollection? other )
  {
    if( ReferenceEquals( other, this ) )
    {
      return true;
    }

    return other is not null && _pitches.SequenceEqual( other._pitches );
  }

  /// <inheritdoc />
  public override bool Equals(
    object? obj )
  {
    if( ReferenceEquals( obj, this ) )
    {
      return true;
    }

    return obj is PitchCollection other && Equals( other );
  }

  /// <inheritdoc />
  public IEnumerator<Pitch> GetEnumerator()
  {
    return ( (IEnumerable<Pitch>) _pitches ).GetEnumerator();
  }

  /// <inheritdoc />
  IEnumerator IEnumerable.GetEnumerator()
  {
    return GetEnumerator();
  }

  /// <inheritdoc />
  public override int GetHashCode()
  {
    var hash = new HashCode();
    foreach( var pitch in _pitches )
    {
      hash.Add( pitch );
    }

    return hash.ToHashCode();
  }

  /// <summary>Parses the provided string.</summary>
  /// <exception cref="FormatException">Thrown when the provided string doesn't represent a pitch collection.</exception>
  /// <exception cref="ArgumentNullException">Thrown when a null string is provided.</exception>
  /// <exception cref="ArgumentException">Thrown when an empty string is provided.</exception>
  /// <param name="value">The value to parse.</param>
  /// <returns>A PitchCollection.</returns>
  public static PitchCollection Parse(
    string value )
  {
    ArgumentException.ThrowIfNullOrEmpty( value );
    return Parse( value.AsSpan(), null );
  }

  /// <summary>Parses the provided string using the specified format provider.</summary>
  /// <param name="value">The value to parse.</param>
  /// <param name="provider">The format provider.</param>
  /// <returns>A PitchCollection.</returns>
  public static PitchCollection Parse(
    string value,
    IFormatProvider? provider )
  {
    ArgumentException.ThrowIfNullOrEmpty( value );
    return Parse( value.AsSpan(), provider );
  }

  /// <summary>Parses the provided span using the specified format provider.</summary>
  /// <param name="value">The value to parse.</param>
  /// <param name="provider">The format provider.</param>
  /// <returns>A PitchCollection.</returns>
  public static PitchCollection Parse(
    ReadOnlySpan<char> value,
    IFormatProvider? provider )
  {
    return TryParse( value, provider, out var notes ) ? notes : throw new FormatException( $"{value.ToString()} contains invalid pitches" );
  }

  /// <inheritdoc />
  public override string ToString()
  {
    return ToString( this );
  }

  /// <summary>Converts a sequence of pitches into a string representation.</summary>
  /// <exception cref="ArgumentNullException">Thrown when pitches argument is null.</exception>
  /// <returns>A string that represents the sequence of pitches.</returns>
  public static string ToString(
    IEnumerable<Pitch> pitches )
  {
    ArgumentNullException.ThrowIfNull( pitches );

    var buf = new StringBuilder();
    var needsComma = false;

    foreach( var note in pitches )
    {
      if( needsComma )
      {
        buf.Append( ',' );
      }
      else
      {
        needsComma = true;
      }

      buf.Append( note );
    }

    return buf.ToString();
  }

  /// <summary>Attempts to parse a pitch collection from the given string.</summary>
  /// <param name="value">The value to parse.</param>
  /// <param name="pitches">[out] The pitch collection.</param>
  /// <returns>True if it succeeds, false if it fails.</returns>
  public static bool TryParse(
    string? value,
    [NotNullWhen( true )] out PitchCollection? pitches )
  {
    return TryParse( value.AsSpan(), null, out pitches );
  }

  /// <summary>Attempts to parse a pitch collection from the given string using the specified format provider.</summary>
  /// <param name="value">The value to parse.</param>
  /// <param name="provider">The format provider.</param>
  /// <param name="pitches">[out] The pitch collection.</param>
  /// <returns>True if it succeeds, false if it fails.</returns>
  public static bool TryParse(
    string? value,
    IFormatProvider? provider,
    [NotNullWhen( true )] out PitchCollection? pitches )
  {
    return TryParse( value.AsSpan(), provider, out pitches );
  }

  /// <summary>Attempts to parse a pitch collection from the given span.</summary>
  /// <param name="span">The value to parse.</param>
  /// <param name="pitches">[out] The pitch collection.</param>
  /// <returns>True if it succeeds, false if it fails.</returns>
  public static bool TryParse(
    ReadOnlySpan<char> span,
    [NotNullWhen( true )] out PitchCollection? pitches )
  {
    return TryParse( span, null, out pitches );
  }

  /// <summary>Attempts to parse a pitch collection from the given span using the specified format provider.</summary>
  /// <param name="span">The value to parse.</param>
  /// <param name="provider">The format provider.</param>
  /// <param name="pitches">[out] The pitch collection.</param>
  /// <returns>True if it succeeds, false if it fails.</returns>
  public static bool TryParse(
    ReadOnlySpan<char> span,
    IFormatProvider? provider,
    [NotNullWhen( true )] out PitchCollection? pitches )
  {
    if( span.IsEmpty )
    {
      pitches = null;
      return false;
    }

    // Count the number of commas in the span to determine how many pitches we have.
    var sepCount = span.Count( ',' );

    // Allocate a stack-allocated array of ranges to hold the start and end indices of each pitch in the span.
    Span<Range> ranges = stackalloc Range[sepCount + 1];
    var rangeCount = span.Split( ranges, ',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries );
    var tmp = new List<Pitch>( rangeCount );

    for( var i = 0; i < rangeCount; i++ )
    {
      if( !Pitch.TryParse( span[ranges[i]], out var pitch ) )
      {
        pitches = null;
        return false;
      }

      tmp.Add( pitch );
    }

    pitches = new PitchCollection( tmp );
    return true;
  }

  #endregion
}
