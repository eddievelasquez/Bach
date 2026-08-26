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

using System.Diagnostics.CodeAnalysis;
using Bach.Model.Internal;

namespace Bach.Model;

/// <summary>A chord is a set of pitch classes defined by a ChordFormula .</summary>
public class Chord
  : Chord<Chord, PitchClass>,
    IChordFactory<Chord, PitchClass>,
    IChordParser<Chord>
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
    int inversion = 0 )
    : base( root, formula, inversion )
  {
  }

  #endregion

  #region Public Methods

  /// <summary>
  ///   Creates a new chord instance with the specified root, formula, and inversion.
  /// </summary>
  /// <param name="root">The root pitch class of the chord.</param>
  /// <param name="formula">The formula used to generate the chord.</param>
  /// <param name="inversion">The inversion.</param>
  /// <returns>A new chord instance with the specified parameters.</returns>
  public static Chord Create(
    PitchClass root,
    ChordFormula formula,
    int inversion = 0 )
  {
    return new Chord( root, formula, inversion );
  }

  /// <summary>
  ///   Creates a new chord instance with the specified root, formula ID or name, and inversion.
  /// </summary>
  /// <param name="root">The root pitch class of the chord.</param>
  /// <param name="formulaIdOrName">ID or name of the formula as defined in the Registry.</param>
  /// <param name="inversion">The inversion.</param>
  /// <returns>A new chord instance with the specified parameters.</returns>
  public static Chord Create(
    PitchClass root,
    string formulaIdOrName,
    int inversion = 0 )
  {
    return new Chord( root, Registry.ChordFormulas[formulaIdOrName], inversion );
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

    if( !Registry.TryGetChordFormulaBySymbol( formulaSymbolSpan, out var chordFormula ) )
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
      chord = Create( root, chordFormula );
      return true;
    }

    // Parse the bass pitch class from the tail. If parsing fails, return false.
    if( !Pitch.TryParse( tail[( bassSeparatorPos + 1 )..], provider, out var bass, out tail ) )
    {
      chord = null;
      return false;
    }

    // Determine the inversion before creating the chord. If the bass pitch is not part of the chord, return false.
    var rootPosition = Create( root, chordFormula );
    var inversion = rootPosition.IndexOf( bass.PitchClass );

    // If the bass pitch is not part of the chord, return false.
    if( inversion < 0 )
    {
      chord = null;
      return false;
    }

    chord = Create( root, chordFormula, inversion );
    return true;
  }

  #endregion
}
