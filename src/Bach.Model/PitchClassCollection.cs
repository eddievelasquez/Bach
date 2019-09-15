// Module Name: PitchClassCollection.cs
// Project:     Bach.Model
// Copyright (c) 2012, 2019  Eddie Velasquez.
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

namespace Bach.Model
{
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;
  using Internal;

  /// <summary>Collection of pitch classes.</summary>
  public class PitchClassCollection
    : IReadOnlyList<PitchClass>,
      IEquatable<IEnumerable<PitchClass>>
  {
    #region Data Members

    private readonly PitchClass[] _pitchClasses;

    #endregion

    #region Constructors

    /// <inheritdoc />
    public PitchClassCollection(IEnumerable<PitchClass> pitchClasses)
    {
      Contract.Requires<ArgumentNullException>(pitchClasses != null);
      _pitchClasses = pitchClasses.ToArray();
    }

    /// <inheritdoc />
    public PitchClassCollection(PitchClass[] pitchClasses)
    {
      Contract.Requires<ArgumentNullException>(pitchClasses != null);
      _pitchClasses = pitchClasses;
    }

    #endregion

    #region IEquatable<IEnumerable<PitchClass>> Members

    /// <inheritdoc />
    public bool Equals(IEnumerable<PitchClass> other)
    {
      if( ReferenceEquals(this, other) )
      {
        return true;
      }

      if( other is null )
      {
        return false;
      }

      return _pitchClasses.SequenceEqual(other);
    }

    #endregion

    #region IReadOnlyList<PitchClass> Members

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <inheritdoc />
    public IEnumerator<PitchClass> GetEnumerator() => ( (IEnumerable<PitchClass>)_pitchClasses ).GetEnumerator();

    /// <inheritdoc />
    public int Count => _pitchClasses.Length;

    /// <inheritdoc />
    public PitchClass this[int index] => _pitchClasses[index];

    #endregion

    #region Public Methods

    /// <summary>Attempts to parse a PitchClass collection from the given string.</summary>
    /// <param name="value">The value to parse.</param>
    /// <param name="pitchClasses">[out] The pitch class collection.</param>
    /// <returns>True if it succeeds, false if it fails.</returns>
    public static bool TryParse(string value,
                                out PitchClassCollection pitchClasses)
    {
      if( string.IsNullOrEmpty(value) )
      {
        pitchClasses = null;
        return false;
      }

      var tmp = new List<PitchClass>();

      foreach( string s in value.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries) )
      {
        if( !PitchClass.TryParse(s, out PitchClass note) )
        {
          pitchClasses = null;
          return false;
        }

        tmp.Add(note);
      }

      pitchClasses = new PitchClassCollection(tmp.ToArray());
      return true;
    }

    /// <summary>Parses the provided string.</summary>
    /// <exception cref="FormatException">Thrown when the provided string doesn't represent a pitch class collection.</exception>
    /// <exception cref="ArgumentNullException">Thrown when a null string is provided.</exception>
    /// <exception cref="ArgumentException">Thrown when an empty string is provided.</exception>
    /// <param name="value">The value to parse.</param>
    /// <returns>A PitchClassCollection.</returns>
    public static PitchClassCollection Parse(string value)
    {
      Contract.RequiresNotNullOrEmpty(value, "Must provide a value");

      if( !TryParse(value, out PitchClassCollection notes) )
      {
        throw new FormatException($"{value} contains invalid pitchClasses");
      }

      return notes;
    }

    public int IndexOf(PitchClass pitchClass) => Array.IndexOf(_pitchClasses, pitchClass);

    #endregion

    #region Overrides

    /// <inheritdoc />
    public override string ToString() => string.Join(",", _pitchClasses);

    /// <inheritdoc />
    public override bool Equals(object obj)
    {
      if( ReferenceEquals(this, obj) )
      {
        return true;
      }

      return obj is PitchClassCollection other && Equals(other);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
      unchecked
      {
        var hash = 17;
        foreach( PitchClass note in _pitchClasses )
        {
          hash = ( hash * 23 ) + note.GetHashCode();
        }

        return hash;
      }
    }

    #endregion
  }
}
