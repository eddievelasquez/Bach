//  
// Module Name: Chord.cs
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
  using Util;

  public class Chord: IEquatable<Chord>,
                      IEnumerable<Note>
  {
    #region Data Members

    #endregion

    #region Construction/Destruction

    private Chord(Note root, ChordFormula formula, string name, IReadOnlyCollection<Note> notes)
    {
      Contract.Requires<ArgumentNullException>(formula != null);
      Contract.Requires<ArgumentNullException>(name != null);
      Contract.Requires<ArgumentException>(name.Length > 0);
      Contract.Requires<ArgumentNullException>(notes != null);
      Contract.Requires<ArgumentOutOfRangeException>(notes.Count == formula.Count);

      Root = root;
      Formula = formula;
      Name = name;
      Notes = notes.ToArray();
    }

    public Chord(Note root, ChordFormula formula)
    {
      Contract.Requires<ArgumentNullException>(formula != null);

      Root = root;
      Formula = formula;

      var buf = new StringBuilder();
      buf.Append(root.Tone);
      buf.Append(root.Accidental.ToSymbol());
      buf.Append(formula.Symbol);

      Name = buf.ToString();
      Notes = Formula.Generate(Root).Take(formula.Count).ToArray();
    }

    #endregion

    #region Properties

    public Note Root { get; }
    public string Name { get; }
    public ChordFormula Formula { get; }
    public Note[] Notes { get; }

    #endregion

    #region Public Methods

    public IEnumerable<AbsoluteNote> Render(AbsoluteNote startNote)
    {
      int pos = Array.IndexOf(Notes, startNote.Note);
      if( pos == -1 )
      {
        return Enumerable.Empty<AbsoluteNote>();
      }

      int octave = startNote.Octave;
      if( startNote.Note < Root )
      {
        --octave;
      }

      AbsoluteNote root = AbsoluteNote.Create(Root, octave);
      return Formula.Generate(root).Skip(pos);
    }

    public Chord Invert(int inversion = 1)
    {
      Contract.Requires<ArgumentOutOfRangeException>(inversion > 0);
      Contract.Requires<ArgumentOutOfRangeException>(inversion < Formula.Count);

      var notes = Formula.Generate(Root).Skip(inversion).Take(Formula.Count).ToArray();
      string inversionName = $"{Name} - {inversion.ToOrdinal()} inversion";
      var inv = new Chord(Root, Formula, inversionName, notes);
      return inv;
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

      return Equals((Chord) other);
    }

    public override int GetHashCode()
    {
      int hashCode = Root.GetHashCode() ^ Formula.GetHashCode();
      return hashCode;
    }

    public override string ToString()
    {
      return Name;
    }

    #endregion

    #region IEnumerable<Note> Members

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    public IEnumerator<Note> GetEnumerator()
    {
      return Formula.Generate(Root).Take(Formula.Count).GetEnumerator();
    }

    #endregion

    #region IEquatable<Chord> Members

    public bool Equals(Chord other)
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
