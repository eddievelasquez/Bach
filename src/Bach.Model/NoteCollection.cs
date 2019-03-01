//
// Module Name: NoteCollection.cs
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
  using System.Linq;
  using System.Text;

  public class NoteCollection : Collection<Note>,
                               IEquatable<IEnumerable<Note>>
  {
    #region Construction/Destruction

    public NoteCollection()
    {
    }

    public NoteCollection(IList<Note> tones)
      : base(tones)
    {
    }

    #endregion

    #region IEquatable<IEnumerable<Note>> Members

    public bool Equals(IEnumerable<Note> other)
    {
      if (ReferenceEquals(other, this))
      {
        return true;
      }

      if (ReferenceEquals(other, null))
      {
        return false;
      }

      return this.SequenceEqual(other);
    }

    #endregion

    #region Public Methods

    public static bool TryParse(string value,
                                out NoteCollection notes)
    {
      if (string.IsNullOrEmpty(value))
      {
        notes = null;
        return false;
      }

      notes = new NoteCollection();

      foreach (string s in value.Split(','))
      {
        if (!Note.TryParse(s, out Note tone))
        {
          notes = null;
          return false;
        }

        notes.Add(tone);
      }

      return true;
    }

    public static NoteCollection Parse(string value)
    {
      Contract.RequiresNotNullOrEmpty(value, "Must provide a value");

      if (!TryParse(value, out NoteCollection tones))
      {
        throw new FormatException($"{value} contains invalid notes");
      }

      return tones;
    }

    public static string ToString(IEnumerable<Note> tones)
    {
      Contract.Requires<ArgumentNullException>(tones != null);

      var buf = new StringBuilder();
      var needsComma = false;

      foreach (Note note in tones)
      {
        if (needsComma)
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

    public override string ToString() => ToString(this);

    public override bool Equals(object other)
    {
      if (ReferenceEquals(other, this))
      {
        return true;
      }

      if (ReferenceEquals(other, null) || other.GetType() != GetType())
      {
        return false;
      }

      return Equals((NoteCollection)other);
    }

    public override int GetHashCode()
    {
      const int MULTIPLIER = 89;

      var hashCode = 0;
      foreach (Note tone in Items)
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
