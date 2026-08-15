// Module Name: Chord.cs
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

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using Bach.Model.Internal;

/// <summary>A chord is a set of pitch classes defined by a ChordFormula .</summary>
public class Chord
  : PitchClassCollection,
    IChord<Chord, PitchClass>,
    IEquatable<Chord>,
    IEnumerable<PitchClass>
{
  #region Constants

  /// <summary>
  ///   Gets a C major chord.
  /// </summary>
  public static readonly Chord CMajor = new( PitchClass.C, ChordFormula.Major );

  /// <summary>
  ///   Gets a C# major chord.
  /// </summary>
  public static readonly Chord CSharpMajor = new( PitchClass.CSharp, ChordFormula.Major );

  /// <summary>
  ///   Gets a D major chord.
  /// </summary>
  public static readonly Chord DMajor = new( PitchClass.D, ChordFormula.Major );

  /// <summary>
  ///   Gets a E♭ major chord.
  /// </summary>
  public static readonly Chord EFlatMajor = new( PitchClass.EFlat, ChordFormula.Major );

  /// <summary>
  ///   Gets a E major chord.
  /// </summary>
  public static readonly Chord EMajor = new( PitchClass.E, ChordFormula.Major );

  /// <summary>
  ///   Gets a F major chord.
  /// </summary>
  public static readonly Chord FMajor = new( PitchClass.F, ChordFormula.Major );

  /// <summary>
  ///   Gets a F# major chord.
  /// </summary>
  public static readonly Chord FSharpMajor = new( PitchClass.FSharp, ChordFormula.Major );

  /// <summary>
  ///   Gets a G major chord.
  /// </summary>
  public static readonly Chord GMajor = new( PitchClass.G, ChordFormula.Major );

  /// <summary>
  ///   Gets a A♭ major chord.
  /// </summary>
  public static readonly Chord AFlatMajor = new( PitchClass.AFlat, ChordFormula.Major );

  /// <summary>
  ///   Gets a A major chord.
  /// </summary>
  public static readonly Chord AMajor = new( PitchClass.A, ChordFormula.Major );

  /// <summary>
  ///   Gets a B♭ major chord.
  /// </summary>
  public static readonly Chord BFlatMajor = new( PitchClass.BFlat, ChordFormula.Major );

  /// <summary>
  ///   Gets a B major chord.
  /// </summary>
  public static readonly Chord BMajor = new( PitchClass.B, ChordFormula.Major );

  /// <summary>
  ///   Gets a C minor chord.
  /// </summary>
  public static readonly Chord CMinor = new( PitchClass.C, ChordFormula.Minor );

  /// <summary>
  ///   Gets a C# minor chord.
  /// </summary>
  public static readonly Chord CSharpMinor = new( PitchClass.CSharp, ChordFormula.Minor );

  /// <summary>
  ///   Gets a D minor chord.
  /// </summary>
  public static readonly Chord DMinor = new( PitchClass.D, ChordFormula.Minor );

  /// <summary>
  ///   Gets a E♭ minor chord.
  /// </summary>
  public static readonly Chord EFlatMinor = new( PitchClass.EFlat, ChordFormula.Minor );

  /// <summary>
  ///   Gets a E minor chord.
  /// </summary>
  public static readonly Chord EMinor = new( PitchClass.E, ChordFormula.Minor );

  /// <summary>
  ///   Gets a F minor chord.
  /// </summary>
  public static readonly Chord FMinor = new( PitchClass.F, ChordFormula.Minor );

  /// <summary>
  ///   Gets a F# minor chord.
  /// </summary>
  public static readonly Chord FSharpMinor = new( PitchClass.FSharp, ChordFormula.Minor );

  /// <summary>
  ///   Gets a G minor chord.
  /// </summary>
  public static readonly Chord GMinor = new( PitchClass.G, ChordFormula.Minor );

  /// <summary>
  ///   Gets a A♭ minor chord.
  /// </summary>
  public static readonly Chord AFlatMinor = new( PitchClass.AFlat, ChordFormula.Minor );

  /// <summary>
  ///   Gets a A minor chord.
  /// </summary>
  public static readonly Chord AMinor = new( PitchClass.A, ChordFormula.Minor );

  /// <summary>
  ///   Gets a B♭ minor chord.
  /// </summary>
  public static readonly Chord BFlatMinor = new( PitchClass.BFlat, ChordFormula.Minor );

  /// <summary>
  ///   Gets a B minor chord.
  /// </summary>
  public static readonly Chord BMinor = new( PitchClass.B, ChordFormula.Minor );

  #endregion

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
  /// <param name="formulaIdOrName">ID or name of the formula as defined in the Registry.</param>
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
    : this( root, formula, inversion, CreatePitchClasses( root, formula, inversion ) )
  {
  }

  /// <summary>
  ///   Constructor.
  /// </summary>
  /// <param name="root">The root pitch class of the chord.</param>
  /// <param name="formula">The formula used to generate the chord.</param>
  /// <param name="inversion">The inversion.</param>
  /// <param name="pitchClasses">The collection of pitch classes.</param>
  private Chord(
    PitchClass root,
    ChordFormula formula,
    int inversion,
    PitchClass[] pitchClasses )
    : base( pitchClasses )
  {
    ArgumentNullException.ThrowIfNull( formula );
    ArgumentNullException.ThrowIfNull( pitchClasses );
    ArgumentOutOfRangeException.ThrowIfLessThan( inversion, 0 );
    ArgumentOutOfRangeException.ThrowIfGreaterThan( inversion, formula.Intervals.Count - 1 );

    Root = root;
    Formula = formula;
    Inversion = inversion;

    Name = GenerateName( root, formula, this[0] );
  }

  #endregion

  #region Properties

  /// <summary>Gets the root pitch class for the chord.</summary>
  /// <value>The root.</value>
  public PitchClass Root { get; }

  /// <summary>Gets the bass pitch class for the chord. The Bass pitch class is differs from the root for chord inversions.</summary>
  /// <value>The bass.</value>
  public PitchClass Bass => this[0];

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

  /// <inheritdoc />
  public bool Equals(
    Chord? other )
  {
    if( ReferenceEquals( other, this ) )
    {
      return true;
    }

    if( other is null )
    {
      return false;
    }

    return Root.Equals( other.Root ) && Formula.Equals( other.Formula ) && Inversion == other.Inversion;
  }

  /// <inheritdoc />
  public override bool Equals(
    object? obj )
  {
    if( ReferenceEquals( obj, this ) )
    {
      return true;
    }

    return obj is Chord other && Equals( other );
  }

  /// <inheritdoc />
  public new IEnumerator<PitchClass> GetEnumerator()
  {
    return base.GetEnumerator();
  }

  /// <inheritdoc />
  IEnumerator IEnumerable.GetEnumerator()
  {
    return GetEnumerator();
  }

  /// <inheritdoc />
  public override int GetHashCode()
  {
    return HashCode.Combine( Root, Formula, Inversion );
  }

  /// <summary>Generates an inversion for the current chord.</summary>
  /// <param name="inversion">The inversion to generate.</param>
  /// <returns>A Chord.</returns>
  public Chord GetInversion(
    int inversion )
  {
    var result = new Chord( Root, Formula, inversion );
    return result;
  }

  /// <summary>
  ///   Parses a string representation of a chord and returns the corresponding Chord object.
  /// </summary>
  /// <param name="s">The string representation of the chord.</param>
  /// <param name="provider">An object that supplies culture-specific formatting information.</param>
  /// <returns>The corresponding Chord object.</returns>
  public static Chord Parse(
    string s,
    IFormatProvider? provider )
  {
    ArgumentNullException.ThrowIfNull( s );
    return Parse( s.AsSpan(), provider );
  }

  /// <summary>
  ///   Parses a string representation of a chord and returns the corresponding Chord object.
  /// </summary>
  /// <param name="s">The string representation of the chord.</param>
  /// <returns>The corresponding Chord object.</returns>
  public static Chord Parse(
    string s )
  {
    ArgumentNullException.ThrowIfNull( s );
    return Parse( s.AsSpan(), null );
  }

  /// <summary>
  ///   Parses a string representation of a chord and returns the corresponding Chord object.
  /// </summary>
  /// <param name="value">The string representation of the chord.</param>
  /// <param name="provider">An object that supplies culture-specific formatting information.</param>
  /// <returns>The corresponding Chord object.</returns>
  /// <exception cref="ArgumentException">Thrown when the input string is invalid.</exception>
  /// <exception cref="FormatException">Thrown when the input string is not a valid chord representation.</exception>
  public static Chord Parse(
    ReadOnlySpan<char> value,
    IFormatProvider? provider )
  {
    if( value.IsEmpty )
    {
      throw new ArgumentException( "Value cannot be empty.", nameof( value ) );
    }

    return TryParse( value, provider, out var chord )
      ? chord!
      : throw new FormatException( $"{value} is not a valid chord" );
  }

  /// <summary>Returns a rendered version of the scale starting with the provided pitch.</summary>
  /// <param name="octave">The octave for the starting pitch.</param>
  /// <returns>An enumerator for a pitch sequence for this chord.</returns>
  public override IEnumerable<Pitch> Render(
    int octave )
  {
    if( Inversion != 0 )
    {
      yield return Pitch.Create( Bass, octave );
    }

    foreach( var pitch in Formula.Generate( Pitch.Create( Root, octave ) ) )
    {
      yield return pitch;
    }
  }

  /// <inheritdoc />
  public override string ToString()
  {
    return Name;
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
    [NotNullWhen( true )] out Chord? chord )
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
    [NotNullWhen( true )] out Chord? chord )
  {
    // We want to ensure that the entire string is consumed, so we check if the tail is empty after parsing.
    return TryParse( span, provider, out chord, out var tail ) && tail.IsEmpty;
  }

  /// <summary>
  ///   Attempts to parse a string representation of a chord and returns the corresponding Chord object, along with any
  ///   remaining unparsed characters.
  /// </summary>
  /// <param name="span">The string representation of the chord.</param>
  /// <param name="provider">An object that supplies culture-specific formatting information.</param>
  /// <param name="chord">The parsed Chord object, or null if parsing fails.</param>
  /// <param name="tail">The remaining unparsed characters.</param>
  /// <returns>true if the string was successfully parsed; otherwise, false.</returns>
  public static bool TryParse(
    ReadOnlySpan<char> span,
    IFormatProvider? provider,
    [NotNullWhen( true )] out Chord? chord,
    out ReadOnlySpan<char> tail )
  {
    span = span.TrimStart();

    // If the input span is empty after trimming, we cannot parse a chord.
    if( span.IsEmpty )
    {
      chord = null;
      tail = ReadOnlySpan<char>.Empty;
      return false;
    }

    // Parse the root pitch class from the input span. If parsing fails, return false.
    if( !PitchClass.TryParse( span, provider, out var root, out tail ) )
    {
      chord = null;
      return false;
    }

    // If the tail is empty after parsing the root, we cannot parse a chord formula.
    var nonSymbolPos = tail.IndexOfNonChordSymbol();
    var formulaSymbolSpan = nonSymbolPos != -1 ? tail[..nonSymbolPos] : tail;
    if( !Registry.TryGetChordFormulaBySymbol( formulaSymbolSpan.ToString(), out var chordFormula ) )
    {
      chord = null;
      return false;
    }

    // If we have a chord formula, we can consume the characters corresponding to the formula's symbol from the tail.                                    5
    tail = tail[chordFormula.Symbol.Length..];

    // Do we have a bass note?
    var bassSeparatorPos = tail.IndexOf( '/' );

    // No bass note, so we can create the chord with the root and formula.
    if( bassSeparatorPos == -1 )
    {
      chord = new Chord( root, chordFormula );
      return true;
    }

    // Parse the bass pitch class from the tail. If parsing fails, return false.
    if( !PitchClass.TryParse( tail[(bassSeparatorPos + 1) ..], provider, out var bass, out tail ) )
    {
      chord = null;
      return false;
    }

    // Determine the inversion before creating the chord. If the bass pitch is not part of the chord, return false.
    var rootPosition = new Chord( root, chordFormula );
    var inversion = FindInversion( rootPosition, bass );

    // If the bass pitch is not part of the chord, return false.
    if( inversion < 0 )
    {
      chord = null;
      return false;
    }

    chord = new Chord( root, chordFormula, inversion );
    return true;

    static int FindInversion(
      Chord chord,
      PitchClass bassPitchClass )
    {
      for( var i = 0; i < chord.Count; i++ )
      {
        if( chord[i] == bassPitchClass )
        {
          return i;
        }
      }

      return -1;
    }
  }

  #endregion

  #region Implementation

  private static PitchClass[] CreatePitchClasses(
    PitchClass root,
    ChordFormula formula,
    int inversion )
  {
    ArgumentNullException.ThrowIfNull( formula );

    return formula.Generate( root )
                  .Skip( inversion )
                  .Take( formula.Intervals.Count )
                  .ToArray();
  }

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
