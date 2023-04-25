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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using Bach.Model.Internal;

namespace Bach.Model;

/// <summary>Collection of pitches.</summary>
public sealed class PitchCollection
  : IReadOnlyList<Pitch>,
    IEquatable<IEnumerable<Pitch>>
{
#region Fields

  private readonly Pitch[] _pitches;

#endregion

#region Constructors

  public PitchCollection( Pitch[] notes )
  {
    Requires.NotNull( notes );
    _pitches = notes;
  }

  public PitchCollection( IEnumerable<Pitch> notes )
  {
    Requires.NotNull( notes );
    _pitches = notes.ToArray();
  }

#endregion

#region Properties

  /// <inheritdoc />
  public int Count => _pitches.Length;

  /// <inheritdoc />
  public Pitch this[ int index ] => _pitches[index];

#endregion

#region Public Methods

  /// <inheritdoc />
  public bool Equals( IEnumerable<Pitch>? other )
  {
    if( ReferenceEquals( other, this ) )
    {
      return true;
    }

    return other is not null && _pitches.SequenceEqual( other );
  }

  /// <inheritdoc />
  public override bool Equals( object? obj )
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
  public static PitchCollection Parse( string value )
  {
    Requires.NotNullOrEmpty( value );

    if( !TryParse( value, out var notes ) )
    {
      throw new FormatException( $"{value} contains invalid pitches" );
    }

    return notes;
  }

  /// <inheritdoc />
  public override string ToString()
  {
    return ToString( this );
  }

  /// <summary>Converts a sequence of pitches into a string representation.</summary>
  /// <exception cref="ArgumentNullException">Thrown when pitches argument is null.</exception>
  /// <returns>A string that represents the sequence of pitches.</returns>
  public static string ToString( IEnumerable<Pitch> pitches )
  {
    Requires.NotNull( pitches );

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
    string value,
    [NotNullWhen( true )] out PitchCollection? pitches )
  {
    if( string.IsNullOrEmpty( value ) )
    {
      pitches = null;
      return false;
    }

    var tmp = new List<Pitch>();
    foreach( var s in value.Split( ',' ) )
    {
      if( !Pitch.TryParse( s, out var note ) )
      {
        pitches = null;
        return false;
      }

      tmp.Add( note );
    }

    pitches = new PitchCollection( tmp );
    return true;
  }

#endregion
}
