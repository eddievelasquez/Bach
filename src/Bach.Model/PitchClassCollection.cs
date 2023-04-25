// Module Name: PitchClassCollection.cs
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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Bach.Model.Internal;

namespace Bach.Model;

/// <summary>Collection of pitch classes.</summary>
public sealed class PitchClassCollection
  : IReadOnlyList<PitchClass>,
    IEquatable<IEnumerable<PitchClass>>
{
#region Fields

  private readonly PitchClass[] _pitchClasses;

#endregion

#region Constructors

  public PitchClassCollection( IEnumerable<PitchClass> pitchClasses )
  {
    Requires.NotNull( pitchClasses );
    _pitchClasses = pitchClasses.ToArray();
  }

  public PitchClassCollection( PitchClass[] pitchClasses )
  {
    Requires.NotNull( pitchClasses );
    _pitchClasses = pitchClasses;
  }

#endregion

#region Properties

  /// <inheritdoc />
  public int Count => _pitchClasses.Length;

  /// <inheritdoc />
  public PitchClass this[ int index ] => _pitchClasses[index];

#endregion

#region Public Methods

  /// <inheritdoc />
  public bool Equals( IEnumerable<PitchClass>? other )
  {
    if( ReferenceEquals( this, other ) )
    {
      return true;
    }

    if( other is null )
    {
      return false;
    }

    return _pitchClasses.SequenceEqual( other );
  }

  /// <inheritdoc />
  public override bool Equals( object? obj )
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

  public int IndexOf( PitchClass pitchClass )
  {
    return Array.IndexOf( _pitchClasses, pitchClass );
  }

  /// <summary>Parses the provided string.</summary>
  /// <exception cref="FormatException">Thrown when the provided string doesn't represent a pitch class collection.</exception>
  /// <exception cref="ArgumentNullException">Thrown when a null string is provided.</exception>
  /// <exception cref="ArgumentException">Thrown when an empty string is provided.</exception>
  /// <param name="value">The value to parse.</param>
  /// <returns>A PitchClassCollection.</returns>
  public static PitchClassCollection Parse( string value )
  {
    Requires.NotNullOrEmpty( value );

    if( !TryParse( value, out var notes ) )
    {
      throw new FormatException( $"{value} contains invalid pitchClasses" );
    }

    return notes;
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
    string value,
    [NotNullWhen( true )] out PitchClassCollection? pitchClasses )
  {
    if( string.IsNullOrEmpty( value ) )
    {
      pitchClasses = null;
      return false;
    }

    var tmp = new List<PitchClass>();

    foreach( var s in value.Split( new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries ) )
    {
      if( !PitchClass.TryParse( s, out var note ) )
      {
        pitchClasses = null;
        return false;
      }

      tmp.Add( note );
    }

    pitchClasses = new PitchClassCollection( tmp.ToArray() );
    return true;
  }

#endregion
}
