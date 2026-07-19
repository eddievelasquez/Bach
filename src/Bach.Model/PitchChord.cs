// Module Name: PitchChord.cs
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

namespace Bach.Model;

using System.Linq;
using System.Text;

/// <summary>
///   A chord expressed as a collection of actual pitches rather than pitch classes.
/// </summary>
public class PitchChord
  : PitchCollection,
    IEquatable<PitchChord>,
    IChord<PitchChord, Pitch>,
    IPartEvent
{
  #region Constructors

  /// <summary>
  ///   Initializes a new instance of the <see cref="PitchChord" /> class.
  /// </summary>
  /// <param name="root">The root pitch of the chord.</param>
  /// <param name="formula">The formula used to generate the chord.</param>
  public PitchChord(
    Pitch root,
    ChordFormula formula )
    : this( root, formula, 0 )
  {
  }

  /// <summary>
  ///   Initializes a new instance of the <see cref="PitchChord" /> class.
  /// </summary>
  /// <param name="root">The root pitch of the chord.</param>
  /// <param name="formulaIdOrName">ID or name of the formula as defined in the Registry.</param>
  public PitchChord(
    Pitch root,
    string formulaIdOrName )
    : this( root, Registry.ChordFormulas[formulaIdOrName], 0 )
  {
  }

  /// <summary>
  ///   Initializes a new instance of the <see cref="PitchChord" /> class.
  /// </summary>
  /// <param name="root">The root pitch of the chord.</param>
  /// <param name="formula">The formula used to generate the chord.</param>
  /// <param name="inversion">The inversion.</param>
  public PitchChord(
    Pitch root,
    ChordFormula formula,
    int inversion )
    : base( CreatePitches( root, formula, inversion ) )
  {
    ArgumentNullException.ThrowIfNull( formula );
    ArgumentOutOfRangeException.ThrowIfLessThan( inversion, 0 );
    ArgumentOutOfRangeException.ThrowIfGreaterThan( inversion, formula.Intervals.Count - 1 );

    Root = root;
    Formula = formula;
    Inversion = inversion;
    Name = GenerateName( root, formula, this[0] );
  }

  #endregion

  #region Properties

  /// <summary>
  ///   Gets the root pitch of the chord.
  /// </summary>
  public Pitch Root { get; }

  /// <summary>
  ///   Gets the bass pitch of the chord.
  /// </summary>
  public Pitch Bass => this[0];

  /// <summary>
  ///   Gets the inversion number for the chord.
  /// </summary>
  public int Inversion { get; }

  /// <summary>
  ///   Gets the chord formula.
  /// </summary>
  public ChordFormula Formula { get; }

  /// <summary>
  ///   Gets the chord's display name.
  /// </summary>
  public string Name { get; }

  #endregion

  #region Public Methods

  /// <inheritdoc />
  public bool Equals(
    PitchChord? other )
  {
    if( ReferenceEquals( this, other ) )
    {
      return true;
    }

    return other is not null
           && Root.Equals( other.Root )
           && Formula.Equals( other.Formula )
           && Inversion == other.Inversion;
  }

  /// <inheritdoc />
  public override bool Equals(
    object? obj )
  {
    return ReferenceEquals( this, obj ) || ( obj is PitchChord other && Equals( other ) );
  }

  /// <inheritdoc />
  public override int GetHashCode()
  {
    return HashCode.Combine( Root, Formula, Inversion );
  }

  /// <summary>
  ///   Creates an inversion of the current chord.
  /// </summary>
  /// <param name="inversion">The inversion number.</param>
  /// <returns>A new <see cref="PitchChord"/> representing the specified inversion.</returns>
  public PitchChord GetInversion(
    int inversion )
  {
    return new PitchChord( Root, Formula, inversion );
  }

  /// <inheritdoc />
  public override string ToString()
  {
    return Name;
  }

  #endregion

  #region Implementation

  private static Pitch[] CreatePitches(
    Pitch root,
    ChordFormula formula,
    int inversion )
  {
    return formula.Generate( root )
                  .Skip( inversion )
                  .Take( formula.Intervals.Count )
                  .ToArray();
  }

  private static string GenerateName(
    Pitch root,
    ChordFormula formula,
    Pitch bass )
  {
    var buf = new StringBuilder();
    buf.Append( root.PitchClass );

    if( !string.IsNullOrEmpty( formula.Symbol ) )
    {
      buf.Append( formula.Symbol );
    }

    if( root.PitchClass != bass.PitchClass )
    {
      buf.Append( '/' );
      buf.Append( bass.PitchClass );
    }

    return buf.ToString();
  }

  #endregion
}
