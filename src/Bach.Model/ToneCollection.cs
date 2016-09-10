//
// Module Name: ToneCollection.cs
// Project:     Bach.Model
// Copyright (c) 2016  Eddie Velasquez.
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
//  portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A
// PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE
// OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

namespace Bach.Model
{
  using System;
  using System.Collections.Generic;
  using System.Collections.ObjectModel;
  using System.Diagnostics.Contracts;
  using System.Linq;
  using System.Text;

  public class ToneCollection: Collection<Tone>,
                               IEquatable<IEnumerable<Tone>>
  {
    #region Construction/Destruction

    public ToneCollection()
    {
    }

    public ToneCollection(IList<Tone> tones)
      : base(tones)
    {
    }

    #endregion

    #region IEquatable<IEnumerable<Tone>> Members

    public bool Equals(IEnumerable<Tone> other)
    {
      if( ReferenceEquals(other, this) )
      {
        return true;
      }

      if( ReferenceEquals(other, null) )
      {
        return false;
      }

      return this.SequenceEqual(other);
    }

    #endregion

    #region Public Methods

    public static bool TryParse(string value, out ToneCollection tones)
    {
      if( string.IsNullOrEmpty(value) )
      {
        tones = null;
        return false;
      }

      tones = new ToneCollection();

      foreach( string s in value.Split(',') )
      {
        Tone tone;
        if( !Tone.TryParse(s, out tone) )
        {
          tones = null;
          return false;
        }

        tones.Add(tone);
      }

      return true;
    }

    public static ToneCollection Parse(string value)
    {
      Contract.Requires<ArgumentNullException>(value != null);
      Contract.Requires<ArgumentException>(value.Length > 0);

      ToneCollection tones;
      if( !TryParse(value, out tones) )
      {
        throw new FormatException($"{value} contains invalid tones");
      }

      return tones;
    }

    public static string ToString(IEnumerable<Tone> tones)
    {
      Contract.Requires<ArgumentNullException>(tones != null);

      var buf = new StringBuilder();
      var needsComma = false;

      foreach( Tone note in tones )
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

    public override string ToString()
    {
      return ToString(this);
    }

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

      return Equals((ToneCollection) other);
    }

    public override int GetHashCode()
    {
      const int MULTIPLIER = 89;

      var hashCode = 0;
      foreach( Tone tone in Items )
      {
        unchecked
        {
          hashCode += tone.GetHashCode() * MULTIPLIER;
        }
      }

      return hashCode;
    }

    #endregion
  }
}
