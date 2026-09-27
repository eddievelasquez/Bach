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

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Bach.Model.Harmony;

/// <summary>
///   Represents an ordered set of pitch classes generated from a root and a chord formula.
///   Use the inversion to choose which chord tone is the bass; use <see cref="GetPitches"/> to
///   place the chord in an octave.
/// </summary>
public class Chord
  : IChord<PitchClass>,
    ISpanConsumingParsable<Chord>
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

  #region Fields

  private readonly ChordCore<PitchClass> _impl;

  #endregion

  #region Constructors

  /// <summary>
  ///   Initializes a new instance of the <see cref="Chord"/> class.
  /// </summary>
  /// <param name="root">The root pitch class of the chord.</param>
  /// <param name="formula">The formula used to generate the chord.</param>
  /// <param name="inversion">The inversion.</param>
  public Chord(
    PitchClass root,
    ChordFormula formula,
    int inversion = 0 )
  {
    _impl = new ChordCore<PitchClass>( root, formula, inversion );
  }

  /// <summary>
  ///   Initializes a new instance of the <see cref="Chord"/> class.
  /// </summary>
  /// <param name="root">The root pitch class of the chord.</param>
  /// <param name="formulaIdOrName">The identifier or name of the formula used to generate the chord.</param>
  /// <param name="inversion">The inversion.</param>
  public Chord(
    PitchClass root,
    string formulaIdOrName,
    int inversion = 0 )
    : this( root, Registry.Instance.ChordFormulas[formulaIdOrName], inversion )
  {
  }

  #endregion

  #region Properties

  /// <summary>
  ///   Gets the pitch class at the specified index in the chord's collection of pitch classes.
  /// </summary>
  /// <param name="index">The index of the pitch class to retrieve.</param>
  /// <returns>The pitch class at the specified index.</returns>
  public PitchClass this[
    int index ] =>
    _impl[index];

  #endregion

  #region Public Methods

  /// <summary>
  ///   Determines whether the specified object is equal to the current chord.
  /// </summary>
  /// <param name="obj">The object to compare with the current chord.</param>
  /// <returns>
  ///   <c>true</c> if the specified object is equal to the current chord; otherwise, <c>false</c>.
  /// </returns>
  public override bool Equals(
    object? obj )
  {
    return obj is Chord chord && _impl.Equals( chord._impl );
  }

  /// <summary>
  ///   Returns a hash code for the current chord.
  /// </summary>
  /// <returns>A hash code for the current chord.</returns>
  public override int GetHashCode()
  {
    return _impl.GetHashCode();
  }

  /// <summary>
  ///   Returns a string representation of the chord, which is its name.
  /// </summary>
  /// <returns>A string representation of the chord, which is its name.</returns>
  public override string ToString()
  {
    return Name;
  }

  /// <summary>
  ///   Returns the inversion of the chord.
  /// </summary>
  /// <param name="inversion">The inversion to retrieve.</param>
  /// <returns>The specified inversion of the chord.</returns>
  public virtual Chord GetInversion(
    int inversion )
  {
    return new Chord( Root, Formula, inversion );
  }

  /// <summary>
  ///   Returns the pitches of the chord in the specified octave.
  /// </summary>
  /// <param name="octave">The octave in which to retrieve the pitches.</param>
  /// <returns>The pitches of the chord in the specified octave.</returns>
  public IEnumerable<Pitch> GetPitches(
    int octave )
  {
    return _impl.GetPitches( octave );
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
  /// <param name="span">The string representation of the chord.</param>
  /// <param name="provider">An object that supplies culture-specific formatting information.</param>
  /// <returns>The corresponding Chord object.</returns>
  /// <exception cref="ArgumentException">Thrown when the input string is invalid.</exception>
  /// <exception cref="FormatException">Thrown when the input string is not a valid chord representation.</exception>
  public static Chord Parse(
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
  ///   Attempts to parse a string representation of a chord and returns the corresponding Chord object.
  /// </summary>
  /// <param name="s">The string representation of the chord.</param>
  /// <param name="chord">The parsed Chord object, or null if parsing fails.</param>
  /// <returns>true if the string was successfully parsed; otherwise, false.</returns>
  public static bool TryParse(
    [NotNullWhen( true )] string? s,
    [NotNullWhen( true )] out Chord? chord )
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

    if( !Registry.Instance.TryGetChordFormulaBySymbol( formulaSymbolSpan, out var chordFormula ) )
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
    if( !Pitch.TryParse( tail[( bassSeparatorPos + 1 )..], provider, out var bass, out tail ) )
    {
      chord = null;
      return false;
    }

    // Determine the inversion before creating the chord. If the bass pitch is not part of the chord, return false.
    var rootPosition = new Chord( root, chordFormula );
    var inversion = rootPosition.IndexOf( bass.PitchClass );

    // If the bass pitch is not part of the chord, return false.
    if( inversion < 0 )
    {
      chord = null;
      return false;
    }

    chord = new Chord( root, chordFormula, inversion );
    return true;
  }

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
    int inversion )
  {
    return new Chord( root, formula, inversion );
  }

  /// <summary>
  ///   Returns an enumerator that iterates through the collection of pitch classes in the chord.
  /// </summary>
  /// <returns>
  ///   An enumerator that can be used to iterate through the collection of pitch classes in the chord.
  /// </returns>
  public IEnumerator<PitchClass> GetEnumerator()
  {
    return _impl.GetEnumerator();
  }

  /// <summary>
  ///   Determines whether the specified chord is equal to the current chord.
  /// </summary>
  /// <param name="other">The chord to compare with the current chord.</param>
  /// <returns>
  ///   <c>true</c> if the specified chord is equal to the current chord; otherwise, <c>false</c>.
  /// </returns>
  public bool Equals(
    Chord? other )
  {
    return other is not null && _impl.Equals( other._impl );
  }

  /// <summary>
  ///   Returns the index of the specified pitch class in the chord's collection of pitch classes.
  /// </summary>
  /// <param name="pitch">The pitch class to locate in the chord's collection of pitch classes.</param>
  /// <returns>
  ///   The index of the specified pitch class in the chord's collection of pitch classes.
  /// </returns>
  public int IndexOf(
    PitchClass pitch )
  {
    return _impl.IndexOf( pitch );
  }

  #endregion

  #region IChord<Chord,PitchClass> Implementation

  /// <summary>
  ///   Gets the display name of the chord.
  /// </summary>
  public string Name => _impl.Name;

  /// <summary>
  ///   Gets the chord formula.
  /// </summary>
  public ChordFormula Formula => _impl.Formula;

  /// <summary>
  ///   Gets the inversion number of the chord.
  /// </summary>
  public int Inversion => _impl.Inversion;

  /// <summary>
  ///   Gets the root of the chord.
  /// </summary>
  public PitchClass Root => _impl.Root;

  /// <summary>
  ///   Gets the bass of the chord.
  /// </summary>
  public PitchClass Bass => _impl.Bass;

  /// <summary>
  ///   Gets a value indicating whether the chord formula's highest interval extends beyond an octave,
  ///   as in a ninth or eleventh chord. A formula whose highest interval is a seventh is not extended.
  /// </summary>
  public bool IsExtended => _impl.IsExtended;

  #endregion

  #region IEnumerable Implementation

  /// <summary>
  ///   Returns an enumerator that iterates through the collection of pitch classes in the chord.
  /// </summary>
  /// <returns>
  ///   An enumerator that can be used to iterate through the collection of pitch classes in the chord.
  /// </returns>
  IEnumerator IEnumerable.GetEnumerator()
  {
    return GetEnumerator();
  }

  #endregion

  #region IReadOnlyCollection<PitchClass> Implementation

  /// <summary>
  ///   Gets the number of pitch classes in the chord's collection of pitch classes.
  /// </summary>
  public int Count => _impl.Count;

  #endregion
}
