﻿// Module Name: Chord.cs
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
  using System.Linq;
  using System.Text;
  using Internal;

  /// <summary>A chord is a set of notes defined by a ChordFormula .</summary>
  public class Chord
    : IEquatable<Chord>,
      IEnumerable<Note>
  {
    #region Constructors

    /// <summary>Constructor.</summary>
    /// <param name="root">The root note of the chord.</param>
    /// <param name="formula">The formula used to generate the chord.</param>
    public Chord(Note root,
                 ChordFormula formula)
      : this(root, formula, 0)
    {
    }

    /// <summary>Constructor.</summary>
    /// <param name="root">The root note of the chord.</param>
    /// <param name="formulaName">Key of the formula as defined in the Registry.</param>
    public Chord(Note root,
                 string formulaName)
      : this(root, Registry.ChordFormulas[formulaName], 0)
    {
    }

    /// <summary>Specialized constructor for use only by derived classes.</summary>
    /// <exception cref="ArgumentNullException">Thrown when formula is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    ///   Thrown when the inversion is less than zero or greater than the number of
    ///   intervals in the chord's formula.
    /// </exception>
    /// <param name="root">The root note of the chord.</param>
    /// <param name="formula">The formula used to generate the chord.</param>
    /// <param name="inversion">The inversion.</param>
    protected Chord(Note root,
                    ChordFormula formula,
                    int inversion)
    {
      Contract.Requires<ArgumentNullException>(formula != null);
      Contract.Requires<ArgumentOutOfRangeException>(inversion >= 0);
      Contract.Requires<ArgumentOutOfRangeException>(inversion < formula.Intervals.Count);

      Root = root;
      Formula = formula;
      Inversion = inversion;
      Notes = Formula.Generate(Root).Skip(inversion).Take(Formula.Intervals.Count).ToArray();
      Name = GenerateName(root, formula, Notes.First());
    }

    #endregion

    #region Properties

    /// <summary>Gets the root note for the chord.</summary>
    /// <value>The root.</value>
    public Note Root { get; }

    /// <summary>Gets the bass note for the chord. The Bass note is differs from the root for chord inversions.</summary>
    /// <value>The bass.</value>
    public Note Bass => Notes[0];

    /// <summary>Gets the inversion number of the current instance.</summary>
    /// <value>The inversion.</value>
    public int Inversion { get; }

    /// <summary>Gets the chord's name.</summary>
    /// <value>The name.</value>
    public string Name { get; }

    /// <summary>Gets the chord's formula.</summary>
    /// <value>The formula.</value>
    public ChordFormula Formula { get; }

    /// <summary>Gets the notes that compose the current chord.</summary>
    /// <value>The notes.</value>
    public Note[] Notes { get; }

    /// <summary>An extended chord uses intervals whose quantity extends beyond the octave.</summary>
    /// <value>True if this instance is an extended chord, false if not.</value>
    public bool IsExtended
    {
      get
      {
        Interval lastInterval = Formula.Intervals[Formula.Intervals.Count - 1];
        return lastInterval.Quantity > IntervalQuantity.Octave;
      }
    }

    #endregion

    #region IEnumerable<Note> Members

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <inheritdoc />
    public IEnumerator<Note> GetEnumerator() => ( (IEnumerable<Note>)Notes ).GetEnumerator();

    #endregion

    #region IEquatable<Chord> Members

    /// <inheritdoc />
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

    /// <summary>Returns a rendered version of the scale starting with the provided pitch.</summary>
    /// <param name="octave">The octave for the starting pitch.</param>
    /// <returns>An enumerator for a pitch sequence for this chord.</returns>
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

    /// <summary>Generates an inversion for the current chord.</summary>
    /// <param name="inversion">The inversion to generate.</param>
    /// <returns>A Chord.</returns>
    public Chord GetInversion(int inversion)
    {
      var result = new Chord(Root, Formula, inversion);
      return result;
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

      return Equals((Chord)other);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
      int hashCode = Root.GetHashCode() ^ Formula.GetHashCode();
      return hashCode;
    }

    /// <inheritdoc />
    public override string ToString() => Name;

    #endregion

    #region  Implementation

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
