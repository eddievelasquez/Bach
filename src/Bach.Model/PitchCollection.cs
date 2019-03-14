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
  using System.Collections.Generic;
  using System.Collections.ObjectModel;
  using System.Linq;
  using System.Text;
  using Internal;

  public class PitchCollection
    : Collection<Pitch>,
      IEquatable<IEnumerable<Pitch>>
  {
    #region Constructors

    public PitchCollection()
    {
    }

    public PitchCollection(IList<Pitch> notes)
      : base(notes)
    {
    }

    #endregion

    #region IEquatable<IEnumerable<Pitch>> Members

    public bool Equals(IEnumerable<Pitch> other)
    {
      if( ReferenceEquals(other, this) )
      {
        return true;
      }

      return !ReferenceEquals(other, null) && this.SequenceEqual(other);
    }

    #endregion

    #region Public Methods

    public static bool TryParse(string value,
                                out PitchCollection pitches)
    {
      if( string.IsNullOrEmpty(value) )
      {
        pitches = null;
        return false;
      }

      pitches = new PitchCollection();

      foreach( string s in value.Split(',') )
      {
        if( !Pitch.TryParse(s, out Pitch note) )
        {
          pitches = null;
          return false;
        }

        pitches.Add(note);
      }

      return true;
    }

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

    public static string ToString(IEnumerable<Pitch> notes)
    {
      Contract.Requires<ArgumentNullException>(notes != null);

      var buf = new StringBuilder();
      var needsComma = false;

      foreach( Pitch note in notes )
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

    public override string ToString() => ToString(this);

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
