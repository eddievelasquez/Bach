// Module Name: PitchClassCollection.cs
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

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

/// <summary>Collection of <see cref="PitchClass" />.</summary>
public class PitchClassCollection
  : IReadOnlyList<PitchClass>,
    IEquatable<PitchClassCollection>,
    ISpanParsable<PitchClassCollection>
{
  #region Fields

  private readonly PitchClass[] _pitchClasses;

  #endregion

  #region Constructors

  /// <summary>
  ///   Initializes a new instance of the <see cref="PitchClassCollection" /> class.
  /// </summary>
  /// <param name="pitchClasses">The collection of pitch classes.</param>
  public PitchClassCollection(
    IEnumerable<PitchClass> pitchClasses )
  {
    ArgumentNullException.ThrowIfNull( pitchClasses );
    _pitchClasses = pitchClasses.ToArray();
  }

  #endregion

  #region Properties

  /// <inheritdoc />
  public int Count => _pitchClasses.Length;

  /// <inheritdoc />
  public PitchClass this[
    int index ] => _pitchClasses[index];

  #endregion

  #region Public Methods

  /// <inheritdoc />
  public bool Equals(
    PitchClassCollection? other )
  {
    if( ReferenceEquals( this, other ) )
    {
      return true;
    }

    return other is not null && _pitchClasses.SequenceEqual( other._pitchClasses );
  }

  /// <inheritdoc />
  public override bool Equals(
    object? obj )
  {
    if( ReferenceEquals( this, obj ) )
    {
      return true;
    }

    return obj is PitchClassCollection other && Equals( other );
  }

  /// <inheritdoc />
  public IEnumerator<PitchClass> GetEnumerator()
  {
    return ( (IEnumerable<PitchClass>) _pitchClasses ).GetEnumerator();
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

    foreach( var pitchClass in _pitchClasses )
    {
      hash.Add( pitchClass );
    }

    return hash.ToHashCode();
  }

  /// <summary>
  ///   Returns the index of the specified pitch class in the collection.
  /// </summary>
  /// z
  /// <param name="pitchClass">The pitch class to search for.</param>
  /// <returns>The index of the pitch class, or -1 if not found.</returns>
  public int IndexOf(
    PitchClass pitchClass )
  {
    return Array.IndexOf( _pitchClasses, pitchClass );
  }

  /// <summary>Parses the provided string.</summary>
  /// <exception cref="FormatException">Thrown when the provided string doesn't represent a pitch class collection.</exception>
  /// <exception cref="ArgumentNullException">Thrown when a null string is provided.</exception>
  /// <exception cref="ArgumentException">Thrown when an empty string is provided.</exception>
  /// <param name="value">The value to parse.</param>
  /// <returns>A PitchClassCollection.</returns>
  public static PitchClassCollection Parse(
    string value )
  {
    ArgumentException.ThrowIfNullOrEmpty( value );
    return Parse( value.AsSpan(), null );
  }

  /// <summary>Parses the provided string using the specified format provider.</summary>
  /// <param name="value">The value to parse.</param>
  /// <param name="provider">The format provider.</param>
  /// <returns>A PitchClassCollection.</returns>
  public static PitchClassCollection Parse(
    string value,
    IFormatProvider? provider )
  {
    ArgumentException.ThrowIfNullOrEmpty( value );
    return Parse( value.AsSpan(), provider );
  }

  /// <summary>Parses the provided span using the specified format provider.</summary>
  /// <param name="value">The value to parse.</param>
  /// <param name="provider">The format provider.</param>
  /// <returns>A PitchClassCollection.</returns>
  public static PitchClassCollection Parse(
    ReadOnlySpan<char> value,
    IFormatProvider? provider )
  {
    return TryParse( value, provider, out var notes )
      ? notes
      : throw new FormatException( $"{value} contains invalid pitchClasses" );
  }

  /// <summary>Renders the collection as <see cref="Pitch"/> instances for the provided octave.</summary>
  /// <param name="octave">The octave to render from.</param>
  /// <returns>The rendered pitches.</returns>
  public virtual IEnumerable<Pitch> Render(
    int octave )
  {
    throw new NotImplementedException();
  }

  /// <inheritdoc />
  public override string ToString()
  {
    return string.Join( ",", _pitchClasses );
  }

  /// <summary>Attempts to parse a PitchClass collection from the given string.</summary>
  /// <param name="value">The value to parse.</param>
  /// <param name="pitchClasses">[out] The pitch class collection.</param>
  /// <returns>True if it succeeds, false if it fails.</returns>
  public static bool TryParse(
    string? value,
    [NotNullWhen( true )] out PitchClassCollection? pitchClasses )
  {
    return TryParse( value, null, out pitchClasses );
  }

  /// <summary>Attempts to parse a pitch class collection from the given string using the specified format provider.</summary>
  /// <param name="value">The value to parse.</param>
  /// <param name="provider">The format provider.</param>
  /// <param name="pitchClasses">[out] The pitch class collection.</param>
  /// <returns>True if it succeeds, false if it fails.</returns>
  public static bool TryParse(
    string? value,
    IFormatProvider? provider,
    [NotNullWhen( true )] out PitchClassCollection? pitchClasses )
  {
    return TryParse( value.AsSpan(), provider, out pitchClasses );
  }

  /// <summary>Attempts to parse a pitch class collection from the given span using the specified format provider.</summary>
  /// <param name="value">The value to parse.</param>
  /// <param name="provider">The format provider.</param>
  /// <param name="pitchClasses">[out] The pitch class collection.</param>
  /// <returns>True if it succeeds, false if it fails.</returns>
  public static bool TryParse(
    ReadOnlySpan<char> value,
    IFormatProvider? provider,
    [NotNullWhen( true )] out PitchClassCollection? pitchClasses )
  {
    if( value.IsEmpty )
    {
      pitchClasses = null;
      return false;
    }

    var tmp = new List<PitchClass>();
    var start = 0;

    for( var i = 0; i < value.Length; i++ )
    {
      var c = value[i];

      if( c is not (',' or ' ') )
      {
        continue;
      }

      if( i > start )
      {
        var segment = value.Slice( start, i - start );

        if( !PitchClass.TryParse( segment, provider, out var note ) )
        {
          pitchClasses = null;
          return false;
        }

        tmp.Add( note );
      }

      start = i + 1;
    }

    // Parse the last segment
    if( start < value.Length )
    {
      var segment = value[start..];

      if( !PitchClass.TryParse( segment, provider, out var note ) )
      {
        pitchClasses = null;
        return false;
      }

      tmp.Add( note );
    }

    pitchClasses = new PitchClassCollection( tmp );
    return true;
  }

  #endregion
}
