// Module Name: PitchCollection.cs
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
  using System.Text;
  using Internal;

  /// <summary>Collection of pitches.</summary>
  public class PitchCollection
    : IReadOnlyList<Pitch>,
      IEquatable<IEnumerable<Pitch>>
  {
    #region Data Members

    private readonly Pitch[] _pitches;

    #endregion

    #region Constructors

    public PitchCollection(Pitch[] notes)
    {
      Contract.Requires<ArgumentNullException>(notes != null);
      _pitches = notes;
    }

    public PitchCollection(IEnumerable<Pitch> notes)
    {
      Contract.Requires<ArgumentNullException>(notes != null);
      _pitches = notes.ToArray();
    }

    #endregion

    #region IEquatable<IEnumerable<Pitch>> Members

    /// <inheritdoc />
    public bool Equals(IEnumerable<Pitch> other)
    {
      if( ReferenceEquals(other, this) )
      {
        return true;
      }

      return !ReferenceEquals(other, null) && _pitches.SequenceEqual(other);
    }

    #endregion

    #region IReadOnlyList<Pitch> Members

    /// <inheritdoc />
    public int Count => _pitches.Length;

    /// <inheritdoc />
    public Pitch this[int index] => _pitches[index];

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <inheritdoc />
    public IEnumerator<Pitch> GetEnumerator() => ( (IEnumerable<Pitch>)_pitches ).GetEnumerator();

    #endregion

    #region Public Methods

    /// <summary>Attempts to parse a pitch collection from the given string.</summary>
    /// <param name="value">The value to parse.</param>
    /// <param name="pitches">[out] The pitch collection.</param>
    /// <returns>True if it succeeds, false if it fails.</returns>
    public static bool TryParse(string value,
                                out PitchCollection pitches)
    {
      if( string.IsNullOrEmpty(value) )
      {
        pitches = null;
        return false;
      }

      var tmp = new List<Pitch>();
      foreach( string s in value.Split(',') )
      {
        if( !Pitch.TryParse(s, out Pitch note) )
        {
          pitches = null;
          return false;
        }

        tmp.Add(note);
      }

      pitches = new PitchCollection(tmp);
      return true;
    }

    /// <summary>Parses the provided string.</summary>
    /// <exception cref="FormatException">Thrown when the provided string doesn't represent a pitch collection.</exception>
    /// <exception cref="ArgumentNullException">Thrown when a null string is provided.</exception>
    /// <exception cref="ArgumentException">Thrown when an empty string is provided.</exception>
    /// <param name="value">The value to parse.</param>
    /// <returns>A PitchCollection.</returns>
    public static PitchCollection Parse(string value)
    {
      Contract.Requires<ArgumentNullException>(value != null);
      Contract.Requires<ArgumentException>(value.Length > 0);

      if( !TryParse(value, out PitchCollection notes) )
      {
        throw new FormatException($"{value} contains invalid pitches");
      }

      return notes;
    }

    /// <summary>Converts a sequence of pitches into a string representation.</summary>
    /// <exception cref="ArgumentNullException">Thrown when pitches argument is null.</exception>
    /// <returns>A string that represents the sequence of pitches.</returns>
    public static string ToString(IEnumerable<Pitch> pitches)
    {
      Contract.Requires<ArgumentNullException>(pitches != null);

      var buf = new StringBuilder();
      var needsComma = false;

      foreach( Pitch note in pitches )
      {
        if( needsComma )
        {
          buf.Append(',');
        }
        else
        {
          needsComma = true;
        }

        buf.Append(note);
      }

      return buf.ToString();
    }

    #endregion

    #region Overrides

    /// <inheritdoc />
    public override string ToString() => ToString(this);

    /// <inheritdoc />
    public override bool Equals(object other)
    {
      if( ReferenceEquals(other, this) )
      {
        return true;
      }

      if( ReferenceEquals(other, null) || other.GetType() != GetType() )
      {
        return false;
      }

      return Equals((PitchCollection)other);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
      const int Multiplier = 89;

      Pitch first = this.FirstOrDefault();
      Pitch last = this.LastOrDefault();

      unchecked
      {
        int result = ( ( first.GetHashCode() + Count ) * Multiplier ) + last.GetHashCode() + Count;
        return result;
      }
    }

    #endregion
  }
}
