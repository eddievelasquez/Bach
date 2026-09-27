// Module Name: ChordCore.cs
// Project:     Bach.Model
// Copyright (c) 2012, 2026  Eddie Velasquez.
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

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bach.Model.Harmony;

/// <summary>
///   Internal representation of a chord with a specific root element type and pitch type.
/// </summary>
/// <typeparam name="TPitch">The type of the pitches in the chord.</typeparam>
/// <remarks>
///   This class encompasses common chord functionality, including pitch generation, name generation, and equality
///   comparison. It is designed to be used by <see cref="Chord"/> and <see cref="PitchChord"/> for their implementation
///   of the <see cref="IChord{TPitch}"/> interface.
/// </remarks>
internal sealed class ChordCore<TPitch>
  : IChord<TPitch>
  where TPitch: struct, IPitch
{
  #region Fields

  private readonly TPitch[] _pitches;

  #endregion

  #region Constructors

  /// <summary>Specialized constructor for use only by derived classes.</summary>
  /// <param name="root">The root pitch class of the chord.</param>
  /// <param name="formula">The formula used to generate the chord.</param>
  /// <param name="inversion">The bass-position index: zero puts the root in the bass; each higher value selects the next chord tone.</param>
  /// <exception cref="ArgumentNullException">Thrown when formula is null.</exception>
  /// <exception cref="ArgumentOutOfRangeException">
  ///   Thrown when the index is less than zero or greater than the number of chord tones minus one.
  /// </exception>
  public ChordCore(
    TPitch root,
    ChordFormula formula,
    int inversion )
  {
    _pitches = CreatePitches( root, formula, inversion );

    Root = root;
    Formula = formula;
    Inversion = inversion;
    Name = GenerateName( root, formula, _pitches[0] );
  }

  #endregion

  #region Properties

  /// <summary>
  ///   Gets the pitch at the specified index in the chord.
  /// </summary>
  /// <param name="index">The index of the pitch to retrieve.</param>
  /// <returns>The pitch at the specified index.</returns>
  public TPitch this[
    int index ] => _pitches[index];

  #endregion

  #region Public Methods

  /// <summary>
  ///   Determines whether the specified chord is equal to the current chord.
  /// </summary>
  /// <param name="other">The chord to compare with the current chord.</param>
  /// <returns>
  ///   <c>true</c> if the specified chord is equal to the current chord; otherwise, <c>false</c>.
  /// </returns>
  public bool Equals(
    IChord<TPitch>? other )
  {
    if( other is null )
    {
      return false;
    }

    if( ReferenceEquals( this, other ) )
    {
      return true;
    }

    return Formula.Equals( other.Formula )
           && Root.Equals( other.Root )
           && Inversion == other.Inversion;
  }

  /// <summary>
  ///   Returns an enumerator for the chord's pitches.
  /// </summary>
  /// <returns>An enumerator for the chord's pitches.</returns>
  public IEnumerator<TPitch> GetEnumerator()
  {
    return ( (IEnumerable<TPitch>) _pitches ).GetEnumerator();
  }

  /// <summary>
  ///   Returns the index of the specified pitch in the chord.
  /// </summary>
  /// <param name="pitch">The pitch to find.</param>
  /// <returns>The pitch index, or -1 if the chord does not contain the pitch.</returns>
  public int IndexOf(
    TPitch pitch )
  {
    return _pitches.IndexOf( pitch );
  }

  /// <summary>
  ///   Returns a hash code for the current chord.
  /// </summary>
  /// <returns>The hash code.</returns>
  public override int GetHashCode()
  {
    return HashCode.Combine( Formula, Root, Inversion );
  }

  /// <summary>
  ///   Gets the pitches of the chord at the specified octave.
  /// </summary>
  /// <param name="octave">The octave to get the pitches at.</param>
  /// <returns>The pitches of the chord.</returns>
  public IEnumerable<Pitch> GetPitches(
    int octave )
  {
    // If the chord is inverted, we need to include the bass pitch in the output.
    if( Inversion != 0 )
    {
      var bass = Bass.GetPitchClass();
      yield return new Pitch( bass, octave );
    }

    // Generate the pitches of the chord based on the root and formula.
    var root = new Pitch( Root.GetPitchClass(), octave );

    foreach( var pitch in Formula.Generate( root ) )
    {
      yield return pitch;
    }
  }

  #endregion

  #region IChord<TPitch> Implementation

  /// <summary>
  ///   Gets a value indicating whether the chord formula's highest interval extends beyond an octave,
  ///   as in a ninth or eleventh chord. A formula whose highest interval is a seventh is not extended.
  /// </summary>
  public bool IsExtended
  {
    get
    {
      var lastInterval = Formula.Intervals[^1];
      return lastInterval.Quantity > IntervalQuantity.Octave;
    }
  }

  /// <summary>Gets the chord's name.</summary>
  /// <value>The name.</value>
  public string Name { get; }

  /// <summary>Gets the chord's formula.</summary>
  /// <value>The formula.</value>
  public ChordFormula Formula { get; }

  /// <summary>Gets the bass-position index for the chord.</summary>
  /// <value>Zero puts the root in the bass; each higher value puts the next chord tone there.</value>
  public int Inversion { get; }

  /// <summary>Gets the root pitch for the chord.</summary>
  /// <value>The root.</value>
  public TPitch Root { get; }

  /// <summary>
  ///   Gets the bass element of the chord. It differs from the root when the chord is inverted.
  /// </summary>
  /// <value>The bass.</value>
  public TPitch Bass => this[0];

  #endregion

  #region IEnumerable Implementation

  IEnumerator IEnumerable.GetEnumerator()
  {
    return GetEnumerator();
  }

  #endregion

  #region IReadOnlyCollection<TPitch> Implementation

  /// <summary>
  ///   Gets the number of pitches in the chord.
  /// </summary>
  public int Count => _pitches.Length;

  #endregion

  #region Implementation

  /// <summary>
  ///   Creates the chord pitches with the selected chord tone in the bass.
  /// </summary>
  /// <param name="root">The root pitch of the chord.</param>
  /// <param name="formula">The chord formula.</param>
  /// <param name="inversion">The bass-position index. Zero puts the root in the bass.</param>
  /// <returns>The pitches of the chord.</returns>
  private static TPitch[] CreatePitches(
    TPitch root,
    ChordFormula formula,
    int inversion )
  {
    ArgumentNullException.ThrowIfNull( formula );
    ArgumentOutOfRangeException.ThrowIfLessThan( inversion, 0 );
    ArgumentOutOfRangeException.ThrowIfGreaterThan( inversion, formula.Intervals.Count - 1 );

    return
    [
      .. formula.Generate( root )
                .Skip( inversion )
                .Take( formula.Intervals.Count )
    ];
  }

  /// <summary>
  ///   Generates the name of the chord based on the root, formula, and bass pitch.
  /// </summary>
  /// <param name="root">The root pitch of the chord.</param>
  /// <param name="formula">The chord formula.</param>
  /// <param name="bass">The bass pitch of the chord.</param>
  /// <returns>The name of the chord.</returns>
  private static string GenerateName(
    TPitch root,
    ChordFormula formula,
    TPitch bass )
  {
    var buf = new StringBuilder();
    buf.Append( root.GetPitchClass() );
    buf.Append( formula.Symbol );

    if( !root.Equals( bass ) )
    {
      buf.Append( "/" );
      buf.Append( bass );
    }

    var result = buf.ToString();
    return result;
  }

  #endregion
}
