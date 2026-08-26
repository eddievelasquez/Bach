// Module Name: ChordOfT.cs
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

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace Bach.Model;

/// <summary>
///   Represents a chord, which is a collection of pitches of type <typeparamref name="TPitch"/>.
/// </summary>
/// <typeparam name="TPitch">The type of the pitches in the chord.</typeparam>
/// <typeparam name="TSelf">The type of the chord itself.</typeparam>
public abstract class Chord<TSelf, TPitch>
  : PitchCollection<TPitch>,
    IChord<TSelf, TPitch>,
    IEquatable<TSelf>
  where TSelf: PitchCollection<TPitch>, IChord<TSelf, TPitch>, IChordFactory<TSelf, TPitch>, IChordParser<TSelf>
  where TPitch: struct, IPitch<TPitch>
{
  #region Constructors

  /// <summary>Specialized constructor for use only by derived classes.</summary>
  /// <exception cref="ArgumentNullException">Thrown when formula is null.</exception>
  /// <exception cref="ArgumentOutOfRangeException">
  ///   Thrown when the inversion is less than zero or greater than the number of intervals in the chord's formula.
  /// </exception>
  /// <param name="root">The root pitch class of the chord.</param>
  /// <param name="formula">The formula used to generate the chord.</param>
  /// <param name="inversion">The inversion.</param>
  protected Chord(
    TPitch root,
    ChordFormula formula,
    int inversion )
    : base( CreatePitchClasses( root, formula, inversion ) )
  {
    Root = root;
    Formula = formula;
    Inversion = inversion;
    Name = GenerateName( root, formula, this[0] );
  }

  #endregion

  #region Properties

  /// <summary>Gets the root pitch for the chord.</summary>
  /// <value>The root.</value>
  public TPitch Root { get; }

  /// <summary>
  ///   Gets the bass pitch class for the chord. The Bass pitch differs from the root for chord inversions.
  /// </summary>
  /// <value>The bass.</value>
  public TPitch Bass => this[0];

  /// <summary>Gets the inversion number of the current instance.</summary>
  /// <value>The inversion.</value>
  public int Inversion { get; }

  /// <summary>Gets the chord's name.</summary>
  /// <value>The name.</value>
  public string Name { get; }

  /// <summary>Gets the chord's formula.</summary>
  /// <value>The formula.</value>
  public ChordFormula Formula { get; }

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

  /// <summary>
  ///   Determines whether the specified chord is equal to the current chord.
  /// </summary>
  /// <param name="other">The chord to compare with the current chord.</param>
  /// <returns><c>true</c> if the specified chord is equal to the current chord; otherwise, <c>false</c>.</returns>
  public bool Equals(
    TSelf? other )
  {
    if( other is null )
    {
      return false;
    }

    if( ReferenceEquals( this, other ) )
    {
      return true;
    }

    return Root.Equals( other.Root )
           && Inversion == other.Inversion
           && Formula.Equals( other.Formula );
  }

  /// <summary>
  ///   Determines whether the specified object is equal to the current chord.
  /// </summary>
  /// <param name="obj">The object to compare with the current chord.</param>
  /// <returns><c>true</c> if the specified object is equal to the current chord; otherwise, <c>false</c>.</returns>
  public override bool Equals(
    object? obj )
  {
    if( obj is null )
    {
      return false;
    }

    if( ReferenceEquals( this, obj ) )
    {
      return true;
    }

    if( obj.GetType() != GetType() )
    {
      return false;
    }

    return Equals( (TSelf) obj );
  }

  /// <summary>
  ///   Returns a hash code for the current chord.
  /// </summary>
  /// <returns>The hash code.</returns>
  public override int GetHashCode()
  {
    return HashCode.Combine( Root, Inversion, Formula );
  }

  /// <summary>
  ///   Gets a new chord instance with the specified inversion.
  /// </summary>
  /// <param name="inversion">The inversion to generate.</param>
  /// <returns>A new chord instance with the specified inversion.</returns>
  public TSelf GetInversion(
    int inversion )
  {
    return TSelf.Create( Root, Formula, inversion );
  }

  /// <summary>
  ///   Parses a string representation of a chord and returns the corresponding Chord object.
  /// </summary>
  /// <param name="s">The string representation of the chord.</param>
  /// <returns>The corresponding Chord object.</returns>
  public static TSelf Parse(
    string s )
  {
    ArgumentNullException.ThrowIfNull( s );
    return Parse( s.AsSpan(), null );
  }

  /// <summary>
  ///   Parses a string representation of a chord and returns the corresponding Chord object.
  /// </summary>
  /// <param name="s">The string representation of the chord.</param>
  /// <param name="provider">An object that supplies culture-specific formatting information.</param>
  /// <returns>The corresponding Chord object.</returns>
  public static TSelf Parse(
    string s,
    IFormatProvider? provider )
  {
    ArgumentNullException.ThrowIfNull( s );
    return Parse( s.AsSpan(), provider );
  }

  /// <summary>
  ///   Parses a string representation of a chord and returns the corresponding Chord object.
  /// </summary>
  /// <param name="span">The string representation of the chord.</param>
  /// <param name="provider">An object that supplies culture-specific formatting information.</param>
  /// <returns>The corresponding Chord object.</returns>
  /// <exception cref="ArgumentException">Thrown when the input string is invalid.</exception>
  /// <exception cref="FormatException">Thrown when the input string is not a valid chord representation.</exception>
  public static TSelf Parse(
    ReadOnlySpan<char> span,
    IFormatProvider? provider )
  {
    if( span.IsEmpty )
    {
      throw new ArgumentException( "Value cannot be empty.", nameof( span ) );
    }

    return TryParse( span, provider, out var chord )
      ? chord
      : throw new FormatException( $"{span} is not a valid chord" );
  }

  /// <summary>
  ///   Renders the chord at the specified octave.
  /// </summary>
  /// <param name="octave">The octave to render at.</param>
  /// <returns>The rendered pitches.</returns>
  public IEnumerable<Pitch> Render(
    int octave )
  {
    if( Inversion != 0 )
    {
      yield return Pitch.Create( Bass.PitchClass, octave );
    }

    foreach( var pitch in Formula.Generate( Pitch.Create( Root.PitchClass, octave ) ) )
    {
      yield return pitch;
    }
  }

  /// <summary>
  ///   Returns a string representation of the current chord.
  /// </summary>
  /// <returns>The string representation of the chord.</returns>
  public override string ToString()
  {
    return Name;
  }

  /// <summary>
  ///   Attempts to parse a string representation of a chord and returns the corresponding Chord object.
  /// </summary>
  /// <param name="s">The string representation of the chord.</param>
  /// <param name="chord">The parsed Chord object, or null if parsing fails.</param>
  /// <returns>true if the string was successfully parsed; otherwise, false.</returns>
  public static bool TryParse(
    [NotNullWhen( true )] string? s,
    [NotNullWhen( true )] out TSelf? chord )
  {
    return TryParse( s.AsSpan(), null, out chord );
  }

  /// <summary>
  ///   Attempts to parse a string representation of a chord and returns the corresponding Chord object.
  /// </summary>
  /// <param name="s">The string representation of the chord.</param>
  /// <param name="provider">An object that supplies culture-specific formatting information.</param>
  /// <param name="chord">The parsed Chord object, or null if parsing fails.</param>
  /// <returns>true if the string was successfully parsed; otherwise, false.</returns>
  public static bool TryParse(
    [NotNullWhen( true )] string? s,
    IFormatProvider? provider,
    [NotNullWhen( true )] out TSelf? chord )
  {
    return TryParse( s.AsSpan(), provider, out chord );
  }

  /// <summary>
  ///   Attempts to parse a string representation of a chord and returns the corresponding Chord object.
  /// </summary>
  /// <param name="span">The string representation of the chord.</param>
  /// <param name="provider">An object that supplies culture-specific formatting information.</param>
  /// <param name="chord">The parsed Chord object, or null if parsing fails.</param>
  /// <returns>true if the string was successfully parsed; otherwise, false.</returns>
  public static bool TryParse(
    ReadOnlySpan<char> span,
    IFormatProvider? provider,
    [NotNullWhen( true )] out TSelf? chord )
  {
    // We want to ensure that the entire string is consumed, so we check if the tail is empty after parsing.
    return TSelf.TryParse( span, provider, out chord, out var tail ) && tail.IsEmpty;
  }

  #endregion

  #region Implementation

  private static IEnumerable<TPitch> CreatePitchClasses(
    TPitch root,
    ChordFormula formula,
    int inversion )
  {
    ArgumentNullException.ThrowIfNull( formula );
    ArgumentOutOfRangeException.ThrowIfLessThan( inversion, 0 );
    ArgumentOutOfRangeException.ThrowIfGreaterThan( inversion, formula.Intervals.Count - 1 );

    return formula.Generate( root )
                  .Skip( inversion )
                  .Take( formula.Intervals.Count );
  }

  private static string GenerateName(
    TPitch root,
    ChordFormula formula,
    TPitch bass )
  {
    var buf = new StringBuilder();
    buf.Append( root.PitchClass );
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
