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
  using System.Linq;
  using System.Text;

  public class Chord: IEquatable<Chord>,
                      IEnumerable<Note>
  {
    #region Construction/Destruction

    private Chord(Note root,
                  ChordFormula formula,
                  int inversion)
    {
      Contract.Requires<ArgumentNullException>(formula != null);
      Contract.Requires<ArgumentOutOfRangeException>(inversion >= 0);
      Contract.Requires<ArgumentOutOfRangeException>(inversion < formula.IntervalCount);

      Root = root;
      Formula = formula;
      Inversion = inversion;
      Notes = Formula.Generate(Root).Skip(inversion).Take(Formula.IntervalCount).ToArray();
      Name = GenerateName(root, formula, Notes.First());
    }

    public Chord(Note root,
                 ChordFormula formula)
      : this(root, formula, 0)
    {
    }

    public Chord(Note root,
                 string formulaName)
      : this(root, Registry.ChordFormulas[formulaName], 0)
    {
    }

    #endregion

    #region Properties

    public Note Root { get; }

    public Note Bass => Notes[0];

    public int Inversion { get; }
    public string Name { get; }
    public ChordFormula Formula { get; }
    public Note[] Notes { get; }

    #endregion

    #region Public Methods

    public IEnumerable<Pitch> Render(int octave)
    {
      if( Inversion != 0 )
      {
        Pitch bass = Pitch.Create(Bass, octave);
        yield return bass;
      }

      Pitch root = Pitch.Create(Root, octave);
      foreach( Pitch note in Formula.Generate(root) )
      {
        yield return note;
      }
    }

    public Chord Invert(int inversion = 1)
    {
      var result = new Chord(Root, Formula, inversion);
      return result;
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

    public override string ToString() => Name;

    #endregion

    #region IEnumerable<Pitch> Members

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<Note> GetEnumerator() => Formula.Generate(Root).Take(Formula.IntervalCount).GetEnumerator();

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

    #region Implementation

    private static string GenerateName(Note root,
                                       ChordFormula formula,
                                       Note bass)
    {
      var buf = new StringBuilder();
      buf.Append(root);
      buf.Append(formula.Symbol);

      if( root != bass )
      {
        buf.Append("/");
        buf.Append(bass);
      }

      string result = buf.ToString();
      return result;
    }

    #endregion
  }
}
