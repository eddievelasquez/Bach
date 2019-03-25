// Module Name: Mode.cs
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

  /// <summary>A mode is a type of scale coupled with a set of melodic behaviors.</summary>
  public class Mode
    : IEquatable<Mode>,
      IEnumerable<Note>
  {
    #region Constructors

    /// <summary>Constructor.</summary>
    /// <param name="scale">The scale.</param>
    /// <param name="formula">The mode formula.</param>
    /// <exception cref="ArgumentNullException">Thrown when the scale or the formula are null.</exception>
    public Mode(Scale scale,
                ModeFormula formula)
    {
      Contract.Requires<ArgumentNullException>(scale != null);
      Contract.Requires<ArgumentNullException>(formula != null);

      Scale = scale;
      Formula = formula;

      var buf = new StringBuilder();
      buf.Append(scale.Name);
      buf.Append(' ');
      buf.Append(formula.Name);

      Name = buf.ToString();
      Notes = scale.Skip(Formula.Tonic - 1).Take(scale.Notes.Length).ToArray();
    }

    #endregion

    #region Properties

    public Note[] Notes { get; }

    /// <summary>Gets the mode's scale.</summary>
    /// <value>The scale.</value>
    public Scale Scale { get; }

    /// <summary>Gets the mode's name.</summary>
    /// <value>The name.</value>
    public string Name { get; }

    /// <summary>Gets the mode's formula.</summary>
    /// <value>The formula.</value>
    public ModeFormula Formula { get; }

    #endregion

    #region IEnumerable<Note> Members

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

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

    #endregion

    #region IEquatable<Mode> Members

    /// <inheritdoc />
    public bool Equals(Mode other)
    {
      if( ReferenceEquals(other, this) )
      {
        return true;
      }

      if( ReferenceEquals(other, null) )
      {
        return false;
      }

      return Scale.Equals(other.Scale) && Formula.Equals(other.Formula);
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

      return Equals((Mode)other);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
      int hashCode = Scale.GetHashCode() ^ Formula.GetHashCode();
      return hashCode;
    }

    /// <inheritdoc />
    public override string ToString() => NoteCollection.ToString(Notes);

    #endregion
  }
}
