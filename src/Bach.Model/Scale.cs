// Module Name: Scale.cs
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
  using System.Diagnostics;
  using System.Linq;
  using System.Text;
  using Internal;

  /// <summary>A scale is a set of notes defined by a ScaleFormula .</summary>
  public class Scale
    : IEquatable<Scale>,
      IEnumerable<Note>
  {
    #region Constructors

    /// <summary>Constructor.</summary>
    /// <param name="root">The root note of the scale.</param>
    /// <param name="formula">The formula used to generate the scale.</param>
    /// <exception cref="ArgumentNullException">Thrown when the formula is null.</exception>
    public Scale(Note root,
                 ScaleFormula formula)
    {
      Contract.Requires<ArgumentNullException>(formula != null);

      Root = root;
      Formula = formula;
      Notes = Formula.Generate(Root).Take(Formula.Intervals.Count).ToArray();

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

    /// <summary>Constructor.</summary>
    /// <param name="root">The root note of the scale.</param>
    /// <param name="formulaName">Key of the formula as defined in the Registry.</param>
    /// <exception cref="ArgumentNullException">Thrown when the formula name is null.</exception>
    public Scale(Note root,
                 string formulaName)
      : this(root, Registry.ScaleFormulas[formulaName])
    {
    }

    #endregion

    #region Properties

    /// <summary>Gets the root note of the scale.</summary>
    /// <value>The root.</value>
    public Note Root { get; }

    /// <summary>Gets the localized name of the scale.</summary>
    /// <value>The name.</value>
    public string Name { get; }

    /// <summary>Gets the formula for the scale.</summary>
    /// <value>The formula.</value>
    public ScaleFormula Formula { get; }

    /// <summary>Gets the notes that form this scale.</summary>
    /// <value>An array of notes.</value>
    public Note[] Notes { get; }

    #endregion

    #region IEnumerable<Note> Members

    /// <inheritdoc />
    public IEnumerator<Note> GetEnumerator()
    {
      var index = 0;
      while( true )
      {
        yield return Notes[index];

        index = Notes.WrapIndex(++index);
      }

      // ReSharper disable once IteratorNeverReturns
    }

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    #endregion

    #region IEquatable<Scale> Members

    /// <inheritdoc />
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

    #region Public Methods

    /// <summary>Returns a rendered version of the scale starting with the provided pitch.</summary>
    /// <param name="octave">The octave for the first pitch.</param>
    /// <returns>An enumerator for a pitch sequence for this scale.</returns>
    public IEnumerable<Pitch> Render(int octave) => Formula.Generate(Pitch.Create(Root, octave));

    /// <summary>Determines if this scale is theoretical.</summary>
    /// <notes>
    ///   A theoretical scale is one that contains at least one double flat or double sharp accidental. These
    ///   scales exist in the musical theory realm but are not used in practice due to their complexity. There's always another
    ///   practical scale that contains exactly the same enharmonic pitches in the same order. See
    ///   <see cref="GetEnharmonicScale" /> for a way to obtain said scale.
    /// </notes>
    /// <returns>True if the scale is theoretical; otherwise, it returns false.</returns>
    public bool IsTheoretical()
    {
      return Notes.Any(note => note.Accidental == Accidental.DoubleFlat || note.Accidental == Accidental.DoubleSharp);
    }

    /// <summary>Gets a enharmonic scale for this instance.</summary>
    /// <returns>The enharmonic scale.</returns>
    public Scale GetEnharmonicScale()
    {
      Note expectedNote = Root.Accidental >= Accidental.Natural ? Root + 1 : Root - 1;
      Note? enharmonicRoot = Root.GetEnharmonic(expectedNote.NoteName);
      Debug.Assert(enharmonicRoot.HasValue);

      if( enharmonicRoot.Value == Root )
      {
        return this;
      }

      var scale = new Scale(enharmonicRoot.Value, Formula);
      return scale;
    }

    #endregion

    #region Overrides

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

      return Equals((Scale)other);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
      var hashCode = 17;

      unchecked
      {
        hashCode = ( hashCode * 23 ) + Root.GetHashCode();
        hashCode = ( hashCode * 23 ) + Formula.GetHashCode();
      }

      return hashCode;
    }

    /// <inheritdoc />
    public override string ToString() => NoteCollection.ToString(Notes);

    #endregion
  }
}
