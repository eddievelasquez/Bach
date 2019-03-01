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
  using System.Linq;
  using System.Text;

  public class Scale: IEquatable<Scale>,
                      IEnumerable<Note>
  {
    #region Construction/Destruction

    public Scale(Note root,
                 ScaleFormula formula)
    {
      Contract.Requires<ArgumentNullException>(formula != null);

      Root = root;
      Formula = formula;

      var buf = new StringBuilder();
      buf.Append(root.NoteName);
      buf.Append(root.Accidental.ToSymbol());

      if( !StringComparer.CurrentCultureIgnoreCase.Equals(formula.Name, "Major") )
      {
        buf.Append(' ');
        buf.Append(formula.Name);
      }

      Name = buf.ToString();
    }

    public Scale(Note root,
                 string formulaName)
      : this(root, Registry.ScaleFormulas[formulaName])
    {
      Contract.RequiresNotNullOrEmpty(formulaName, "Must provide a formula name");
    }

    #endregion

    #region Properties

    public Note Root { get; }
    public string Name { get; }
    public ScaleFormula Formula { get; }

    public int ToneCount => Formula.IntervalCount;

    public Note this[int i] => this.Skip(i).Take(1).Single();

    #endregion

    #region Public Methods

    public IEnumerable<Pitch> Render(Pitch startPitch)
    {
      int pos = Array.IndexOf(GetNotes(), startPitch.Note);
      if( pos == -1 )
      {
        return Enumerable.Empty<Pitch>();
      }

      int octave = startPitch.Octave;
      if( startPitch.Note < Root )
      {
        --octave;
      }

      Pitch root = Pitch.Create(Root, octave);
      return Formula.Generate(root).Skip(pos);
    }

    public Note[] GetNotes(int start = 0) => this.Skip(start).Take(Formula.IntervalCount).ToArray();

    public int IndexOf(Note note) => Array.IndexOf(GetNotes(), note);

    public int IndexOf(Pitch pitch) => Array.IndexOf(GetNotes(), pitch.Note);

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
        hashCode = (hashCode * 23) + Root.GetHashCode();
        hashCode = (hashCode * 23) + Formula.GetHashCode();
      }

      return hashCode;
    }

    public override string ToString() => NoteCollection.ToString(this.Take(ToneCount));

    #endregion

    #region IEnumerable<Pitch> Members

    public IEnumerator<Note> GetEnumerator() => Formula.Generate(Root).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

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
