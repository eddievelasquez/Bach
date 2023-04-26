// Module Name: Chord.cs
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

/// <summary>A chord is a set of pitch classes defined by a ChordFormula .</summary>
public class Chord
  : IEquatable<Chord>,
    IEnumerable<PitchClass>
{
#region Constructors

  /// <summary>Constructor.</summary>
  /// <param name="root">The root pitch class of the chord.</param>
  /// <param name="formula">The formula used to generate the chord.</param>
  public Chord(
    PitchClass root,
    ChordFormula formula )
    : this( root, formula, 0 )
  {
  }

  /// <summary>Constructor.</summary>
  /// <param name="root">The root pitch class of the chord.</param>
  /// <param name="formulaIdOrName">Id or name of the formula as defined in the Registry.</param>
  public Chord(
    PitchClass root,
    string formulaIdOrName )
    : this( root, Registry.ChordFormulas[formulaIdOrName], 0 )
  {
  }

  /// <summary>Specialized constructor for use only by derived classes.</summary>
  /// <exception cref="ArgumentNullException">Thrown when formula is null.</exception>
  /// <exception cref="ArgumentOutOfRangeException">
  ///   Thrown when the inversion is less than zero or greater than the number of
  ///   intervals in the chord's formula.
  /// </exception>
  /// <param name="root">The root pitch class of the chord.</param>
  /// <param name="formula">The formula used to generate the chord.</param>
  /// <param name="inversion">The inversion.</param>
  protected Chord(
    PitchClass root,
    ChordFormula formula,
    int inversion )
  {
    Requires.NotNull( formula );
    Requires.Between( inversion, 0, formula.Intervals.Count - 1 );

    Root = root;
    Formula = formula;
    Inversion = inversion;
    PitchClasses
      = new PitchClassCollection( Formula.Generate( Root ).Skip( inversion ).Take( Formula.Intervals.Count ) );

    Name = GenerateName( root, formula, PitchClasses[0] );
  }

#endregion

#region Properties

  /// <summary>Gets the root pitch class for the chord.</summary>
  /// <value>The root.</value>
  public PitchClass Root { get; }

  /// <summary>Gets the bass pitch class for the chord. The Bass pitch class is differs from the root for chord inversions.</summary>
  /// <value>The bass.</value>
  public PitchClass Bass => PitchClasses[0];

  /// <summary>Gets the inversion number of the current instance.</summary>
  /// <value>The inversion.</value>
  public int Inversion { get; }

  /// <summary>Gets the chord's name.</summary>
  /// <value>The name.</value>
  public string Name { get; }

  /// <summary>Gets the chord's formula.</summary>
  /// <value>The formula.</value>
  public ChordFormula Formula { get; }

  /// <summary>Gets the pitch classes that compose the current chord.</summary>
  /// <value>The pitchClasses.</value>
  public PitchClassCollection PitchClasses { get; }

  /// <summary>An extended chord uses intervals whose quantity extends beyond the octave.</summary>
  /// <value>True if this instance is an extended chord, false if not.</value>
  public bool IsExtended
  {
    get
    {
      var lastInterval = Formula.Intervals[^1];
      return lastInterval.Quantity > IntervalQuantity.Octave;
    }
  }

#endregion

#region Public Methods

  /// <inheritdoc />
  public bool Equals( Chord? other )
  {
    if( ReferenceEquals( other, this ) )
    {
      return true;
    }

    if( other is null )
    {
      return false;
    }

    return Root.Equals( other.Root ) && Formula.Equals( other.Formula );
  }

  /// <inheritdoc />
  public override bool Equals( object? obj )
  {
    if( ReferenceEquals( obj, this ) )
    {
      return true;
    }

    return obj is Chord other && Equals( other );
  }

  /// <inheritdoc />
  public IEnumerator<PitchClass> GetEnumerator()
  {
    return PitchClasses.GetEnumerator();
  }

  /// <inheritdoc />
  IEnumerator IEnumerable.GetEnumerator()
  {
    return GetEnumerator();
  }

  /// <inheritdoc />
  public override int GetHashCode()
  {
    return HashCode.Combine( Root, Formula );
  }

  /// <summary>Generates an inversion for the current chord.</summary>
  /// <param name="inversion">The inversion to generate.</param>
  /// <returns>A Chord.</returns>
  public Chord GetInversion( int inversion )
  {
    var result = new Chord( Root, Formula, inversion );
    return result;
  }

  /// <summary>Returns a rendered version of the scale starting with the provided pitch.</summary>
  /// <param name="octave">The octave for the starting pitch.</param>
  /// <returns>An enumerator for a pitch sequence for this chord.</returns>
  public IEnumerable<Pitch> Render( int octave )
  {
    if( Inversion != 0 )
    {
      var bass = Pitch.Create( Bass, octave );
      yield return bass;
    }

    var root = Pitch.Create( Root, octave );
    foreach( var note in Formula.Generate( root ) )
    {
      yield return note;
    }
  }

  /// <inheritdoc />
  public override string ToString()
  {
    return Name;
  }

#endregion

#region Implementation

  private static string GenerateName(
    PitchClass root,
    ChordFormula formula,
    PitchClass bass )
  {
    var buf = new StringBuilder();
    buf.Append( root );
    buf.Append( formula.Symbol );

    if( root != bass )
    {
      buf.Append( "/" );
      buf.Append( bass );
    }

    var result = buf.ToString();
    return result;
  }

#endregion
}
