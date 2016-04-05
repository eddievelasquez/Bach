﻿//  
// Module Name: NoteCollection.cs
// Project:     Bach.Model
// Copyright (c) 2013  Eddie Velasquez.
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

  public class AbsoluteNoteCollection: Collection<AbsoluteNote>,
                                       IEquatable<IEnumerable<AbsoluteNote>>
  {
    #region Construction/Destruction

    public AbsoluteNoteCollection()
    {
    }

    public AbsoluteNoteCollection(IList<AbsoluteNote> list)
      : base(list)
    {
    }

    #endregion

    #region IEquatable<IEnumerable<AbsoluteNote>> Members

    public bool Equals(IEnumerable<AbsoluteNote> other)
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

    public static bool TryParse(string value, out AbsoluteNoteCollection notes, int defaultOctave = 4)
    {
      Contract.Requires<ArgumentNullException>(value != null);
      Contract.Requires<ArgumentException>(value.Length > 0);
      Contract.Requires<ArgumentOutOfRangeException>(defaultOctave >= AbsoluteNote.MinOctave);
      Contract.Requires<ArgumentOutOfRangeException>(defaultOctave <= AbsoluteNote.MaxOctave);

      notes = new AbsoluteNoteCollection();

      foreach( string s in value.Split(',') )
      {
        AbsoluteNote note;
        if( !AbsoluteNote.TryParse(s, out note, defaultOctave) )
        {
          notes = null;
          return false;
        }

        notes.Add(note);
      }

      return true;
    }

    public static AbsoluteNoteCollection Parse(string value, int defaultOctave = 4)
    {
      Contract.Requires<ArgumentNullException>(value != null);
      Contract.Requires<ArgumentException>(value.Length > 0);
      Contract.Requires<ArgumentOutOfRangeException>(defaultOctave >= AbsoluteNote.MinOctave);
      Contract.Requires<ArgumentOutOfRangeException>(defaultOctave <= AbsoluteNote.MaxOctave);

      AbsoluteNoteCollection notes;
      if( !TryParse(value, out notes, defaultOctave) )
      {
        throw new FormatException($"{value} contains invalid notes");
      }

      return notes;
    }

    public static string ToString(IEnumerable<AbsoluteNote> notes)
    {
      Contract.Requires<ArgumentNullException>(notes != null);

      var buf = new StringBuilder();
      var needsComma = false;

      foreach( AbsoluteNote note in notes )
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

      return Equals((AbsoluteNoteCollection) other);
    }

    public override int GetHashCode()
    {
      const int MULTIPLIER = 89;

      AbsoluteNote first = this.FirstOrDefault();
      AbsoluteNote last = this.LastOrDefault();

      unchecked
      {
        int result = (first.GetHashCode() + Count) * MULTIPLIER + last.GetHashCode() + Count;
        return result;
      }
    }

    #endregion
  }
}