// Module Name: Mode.cs
// Project:     Bach.Model
// Copyright (c) 2012, 2023  Eddie Velasquez.
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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bach.Model.Internal;

namespace Bach.Model;

/// <summary>A mode is a type of scale coupled with a set of melodic behaviors.</summary>
public sealed class Mode
  : IEquatable<Mode>,
    IEnumerable<PitchClass>
{
#region Constructors

  /// <summary>Constructor.</summary>
  /// <param name="scale">The scale.</param>
  /// <param name="formula">The mode formula.</param>
  /// <exception cref="ArgumentNullException">Thrown when the scale or the formula are null.</exception>
  public Mode(
    Scale scale,
    ModeFormula formula )
  {
    Requires.NotNull( scale );
    Requires.NotNull( formula );

    Scale = scale;
    Formula = formula;

    var buf = new StringBuilder();
    buf.Append( scale.Name );
    buf.Append( ' ' );
    buf.Append( formula.Name );

    Name = buf.ToString();
    PitchClasses
      = new PitchClassCollection( scale.GetAscending().Skip( Formula.Tonic - 1 ).Take( scale.PitchClasses.Count ) );
  }

#endregion

#region Properties

  /// <summary>Gets the mode's pitchClasses.</summary>
  /// <value>The pitchClasses.</value>
  public PitchClassCollection PitchClasses { get; }

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

#region Public Methods

  /// <inheritdoc />
  public bool Equals( Mode? other )
  {
    if( ReferenceEquals( other, this ) )
    {
      return true;
    }

    if( other is null )
    {
      return false;
    }

    return Scale.Equals( other.Scale ) && Formula.Equals( other.Formula );
  }

  /// <inheritdoc />
  public override bool Equals( object? obj )
  {
    if( ReferenceEquals( obj, this ) )
    {
      return true;
    }

    return obj is Mode other && Equals( other );
  }

  /// <inheritdoc />
  public IEnumerator<PitchClass> GetEnumerator()
  {
    // maxIterationCount provides a way to break out of an otherwise infinite
    // loop, as it doesn't make sense to generate more pitch classes than
    // the number of pitches that are supported.
    var maxIterationCount = Pitch.TotalPitchCount;
    var index = 0;

    while( maxIterationCount-- >= 0 )
    {
      yield return PitchClasses[index];
      index = PitchClasses.WrapIndex( index + 1 );
    }
  }

  /// <inheritdoc />
  IEnumerator IEnumerable.GetEnumerator()
  {
    return GetEnumerator();
  }

  /// <inheritdoc />
  public override int GetHashCode()
  {
    return HashCode.Combine( Scale, Formula );
  }

  /// <inheritdoc />
  public override string ToString()
  {
    return string.Join( ",", PitchClasses );
  }

#endregion
}
