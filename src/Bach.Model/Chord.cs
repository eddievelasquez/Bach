//  
// Module Name: Chord.cs
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
  using System.Collections;
  using System.Collections.Generic;
  using System.Collections.ObjectModel;
  using System.Diagnostics.Contracts;
  using System.Linq;
  using System.Text;

  public class Chord: IEquatable<Chord>,
                      IEnumerable<AbsoluteNote>
  {
    #region Data Members

    private readonly AbsoluteNoteCollection _notes;

    #endregion

    #region Construction/Destruction

    private Chord(AbsoluteNote root, ChordFormula formula, string name, IList<AbsoluteNote> notes)
    {
      Contract.Requires<ArgumentException>(root.IsValid);
      Contract.Requires<ArgumentNullException>(formula != null);
      Contract.Requires<ArgumentNullException>(name != null);
      Contract.Requires<ArgumentException>(name.Length > 0);
      Contract.Requires<ArgumentNullException>(notes != null);
      Contract.Requires<ArgumentException>(notes.Count > 0);

      Root = root;
      Formula = formula;
      Name = name;
      _notes = new AbsoluteNoteCollection(notes);
    }

    public Chord(AbsoluteNote root, ChordFormula formula)
    {
      Contract.Requires<ArgumentNullException>(root != null);
      Contract.Requires<ArgumentNullException>(formula != null);

      Root = root;
      Formula = formula;

      var buf = new StringBuilder();
      buf.Append(root.Tone);
      buf.Append(root.Accidental.ToSymbol());
      buf.Append(formula.Symbol);

      Name = buf.ToString();

      _notes = new AbsoluteNoteCollection(Formula.Generate(Root).Take(formula.Count).ToArray());
    }

    #endregion

    #region Properties

    public AbsoluteNote Root { get; }
    public string Name { get; }
    public ChordFormula Formula { get; }

    public ReadOnlyCollection<AbsoluteNote> Notes => new ReadOnlyCollection<AbsoluteNote>(_notes);

    #endregion

    #region IEnumerable<Note> Members

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    public IEnumerator<AbsoluteNote> GetEnumerator()
    {
      return Formula.Generate(Root).GetEnumerator();
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

    #region Public Methods

    public Note[] GetNotes(int start = 0)
    {
      return Formula.Generate(Root.Note).Skip(start).Take(Formula.Count).ToArray();
    }

    public IEnumerable<AbsoluteNote> Render(AbsoluteNote startNote)
    {
      int pos = Array.IndexOf(GetNotes(), startNote.Note);
      if (pos == -1)
      {
        return Enumerable.Empty<AbsoluteNote>();
      }

      return Formula.Generate(Root).Skip(pos);
    }

    public Chord Invert(int inversion = 1)
    {
      Contract.Requires<ArgumentOutOfRangeException>(inversion > 0);

      var notes = Notes.ToList();
      while( inversion > 0 )
      {
        AbsoluteNote bass = notes[0] + AbsoluteNote.IntervalsPerOctave;
        notes.RemoveAt(0);
        notes.Add(bass);

        --inversion;
      }

      var inv = new Chord(Root, Formula, Name, notes);
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
      return _notes.ToString();
    }

    #endregion
  }
}
