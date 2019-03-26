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
  using System.Collections.ObjectModel;
  using System.Diagnostics;
  using System.Linq;
  using System.Text;
  using Internal;

  /// <summary>A scale is a set of notes defined by a ScaleFormula .</summary>
  public class Scale
    : IEquatable<Scale>,
      IEnumerable<Note>
  {
    #region Nested type: ScaleCategory

    [Flags]
    private enum ScaleCategory
    {
      None = 0,
      Diatonic = 1,
      Major = 2,
      Minor = 4,
      Theoretical = 8
    }

    #endregion

    #region Data Members

    private readonly ScaleCategory _categories;

    #endregion

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

      _categories = Categorize(this);
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

    /// <summary>Determines if this scale is diatonic.</summary>
    /// <notes>A diatonic scale is one that includes 5 whole steps and 2 semitones.</notes>
    /// <value>True if diatonic, false if not.</value>
    public bool Diatonic => ( _categories & ScaleCategory.Diatonic ) != 0;

    /// <summary>Determines if this scale is major.</summary>
    /// <notes>A major scale is one in which the root, third and fifth form a major triad (R,M3,5).</notes>
    /// <value>True if major, false if not.</value>
    public bool Major => ( _categories & ScaleCategory.Major ) != 0;

    /// <summary>Determines if this scale is minor.</summary>
    /// <notes>A minor scale is one in which the root, third and fifth form a minor triad (R,m3,5).</notes>
    /// <value>True if minor, false if not.</value>
    public bool Minor => ( _categories & ScaleCategory.Minor ) != 0;

    /// <summary>Determines if this scale is theoretical.</summary>
    /// <notes>
    ///   A theoretical scale is one that contains at least one double flat or double sharp accidental. These
    ///   scales exist in the musical theory realm but are not used in practice due to their complexity. There's always another
    ///   practical scale that contains exactly the same enharmonic pitches in the same order. See
    ///   <see cref="GetEnharmonicScale" /> for a way to obtain said scale.
    /// </notes>
    /// <returns>True if the scale is theoretical; otherwise, it returns false.</returns>
    public bool Theoretical => ( _categories & ScaleCategory.Theoretical ) != 0;

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

    #region  Implementation

    private static ScaleCategory Categorize(Scale scale)
    {
      var category = ScaleCategory.None;
      if( IsDiatonic(scale) )
      {
        category |= ScaleCategory.Diatonic;
      }

      if( IsMajor(scale) )
      {
        category |= ScaleCategory.Major;
      }
      else if( IsMinor(scale) )
      {
        category |= ScaleCategory.Minor;
      }

      if( IsTheoretical(scale) )
      {
        category |= ScaleCategory.Theoretical;
      }

      return category;
    }

    private static bool IsDiatonic(Scale scale)
    {
      if( scale.Formula.Intervals.Count != 7 )
      {
        return false;
      }

      var wholeSteps = 0;
      var halfSteps = 0;

      foreach( int step in scale.Formula.GetRelativeSteps() )
      {
        if( step == 2 )
        {
          ++wholeSteps;
        }
        else if( step == 1 )
        {
          ++halfSteps;
        }
      }

      return wholeSteps == 5 && halfSteps == 2;
    }

    private static bool IsMajor(Scale scale)
    {
      // Scale is minor when the root, third and fifth form a major triad (R,M3,5).
      ReadOnlyCollection<Interval> intervals = scale.Formula.Intervals;
      return intervals[0] == Interval.Unison && intervals.Contains(Interval.MajorThird) && intervals.Contains(Interval.Fifth);
    }

    private static bool IsMinor(Scale scale)
    {
      // Scale is minor when the root, third and fifth form a minor triad (R,m3,5).
      ReadOnlyCollection<Interval> intervals = scale.Formula.Intervals;
      return intervals[0] == Interval.Unison && intervals.Contains(Interval.MinorThird) && intervals.Contains(Interval.Fifth);
    }

    private static bool IsTheoretical(Scale scale)
    {
      // Scale is theoretical when it contains at least one double flat or sharp.
      return scale.Notes.Any(note => note.Accidental == Accidental.DoubleFlat || note.Accidental == Accidental.DoubleSharp);
    }

    #endregion
  }
}
