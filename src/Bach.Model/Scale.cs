//
// Module Name: Scale.cs
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
  using System.Collections;
  using System.Collections.Generic;
  using System.Diagnostics.Contracts;
  using System.Linq;
  using System.Text;

  public class Scale: IEquatable<Scale>,
                      IEnumerable<Tone>
  {
    #region Construction/Destruction

    public Scale(Tone root, ScaleFormula formula)
    {
      Contract.Requires<ArgumentNullException>(root != null);
      Contract.Requires<ArgumentNullException>(formula != null);

      Root = root;
      Formula = formula;

      var buf = new StringBuilder();
      buf.Append(root.ToneName);
      buf.Append(root.Accidental.ToSymbol());

      if( !StringComparer.CurrentCultureIgnoreCase.Equals(formula.Name, "Major") )
      {
        buf.Append(' ');
        buf.Append(formula.Name);
      }

      Name = buf.ToString();
    }

    public Scale(Tone root, string formulaName)
      : this(root, Registry.ScaleFormulas[formulaName])
    {
      Contract.Requires<ArgumentNullException>(formulaName != null);
      Contract.Requires<ArgumentException>(formulaName.Length > 0);
    }

    #endregion

    #region Properties

    public Tone Root { get; }
    public string Name { get; }
    public ScaleFormula Formula { get; }
    public int ToneCount => Formula.IntervalCount;
    public Tone this[int i] => this.Skip(i).Take(1).Single();

    #endregion

    #region Public Methods

    public IEnumerable<Note> Render(Note startNote)
    {
      int pos = Array.IndexOf(GetNotes(), startNote.Tone);
      if( pos == -1 )
      {
        return Enumerable.Empty<Note>();
      }

      int octave = startNote.Octave;
      if( startNote.Tone < Root )
      {
        --octave;
      }

      Note root = Note.Create(Root, octave);
      return Formula.Generate(root).Skip(pos);
    }

    public Tone[] GetNotes(int start = 0)
    {
      return this.Skip(start).Take(Formula.IntervalCount).ToArray();
    }

    public int IndexOf(Tone tone) => Array.IndexOf(GetNotes(), tone);
    public int IndexOf(Note note) => Array.IndexOf(GetNotes(), note.Tone);

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

      return Equals((Scale) other);
    }

    public override int GetHashCode()
    {
      var hashCode = 17;

      unchecked
      {
        hashCode = hashCode * 23 + Root.GetHashCode();
        hashCode = hashCode * 23 + Formula.GetHashCode();
      }

      return hashCode;
    }

    public override string ToString()
    {
      return ToneCollection.ToString(this.Take(ToneCount));
    }

    #endregion

    #region IEnumerable<Note> Members

    public IEnumerator<Tone> GetEnumerator()
    {
      return Formula.Generate(Root).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    #endregion

    #region IEquatable<Scale> Members

    public bool Equals(Scale other)
    {
      if( ReferenceEquals(other, this) )
      {
        return true;
      }

      if( ReferenceEquals(other, null) )
      {
        return false;
      }

      return Root.Equals(other.Root) && Formula.Equals(other.Formula);
    }

    #endregion
  }
}
