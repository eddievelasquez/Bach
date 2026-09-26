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

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Bach.Model.Harmony;

/// <summary>
///   A chord expressed as a collection of actual pitches rather than pitch classes.
/// </summary>
public class PitchChord
  : IChord<PitchChord, Pitch>,
    IChordEvent,
    ISpanConsumingParsable<PitchChord>
{
  #region Fields

  private readonly ChordCore<PitchChord, Pitch> _impl;

  #endregion

  #region Constructors

  /// <summary>
  ///   Initializes a new instance of the <see cref="PitchChord"/> class.
  /// </summary>
  /// <param name="root">The root pitch of the chord.</param>
  /// <param name="formula">The formula used to generate the chord.</param>
  /// <param name="inversion">The inversion.</param>
  public PitchChord(
    Pitch root,
    ChordFormula formula,
    int inversion = 0 )
  {
    _impl = new ChordCore<PitchChord, Pitch>( root, formula, inversion );
  }

  /// <summary>
  ///   Initializes a new instance of the <see cref="PitchChord"/> class.
  /// </summary>
  /// <param name="root">The root pitch of the chord.</param>
  /// <param name="formulaIdOrName">ID or name of the formula as defined in the Registry.</param>
  /// <param name="inversion">The inversion.</param>
  public PitchChord(
    Pitch root,
    string formulaIdOrName,
    int inversion = 0 )
    : this( root, Registry.ChordFormulas[formulaIdOrName], inversion )
  {
  }

  /// <summary>
  ///   Initializes a new instance of the <see cref="PitchChord"/> class using a root pitch class, formula, octave, and
  ///   inversion.
  /// </summary>
  /// <param name="root">The root pitch class of the chord.</param>
  /// <param name="formula">The formula used to generate the chord.</param>
  /// <param name="octave">The octave of the root pitch.</param>
  /// <param name="inversion">The inversion.</param>
  public PitchChord(
    PitchClass root,
    ChordFormula formula,
    int octave = 4,
    int inversion = 0 )
    : this( new Pitch( root, octave ), formula, inversion )
  {
  }

  /// <summary>
  ///   Initializes a new instance of the <see cref="PitchChord"/> class using a root pitch class, formula ID or name,
  ///   octave, and inversion.
  /// </summary>
  /// <param name="root">The root pitch class of the chord.</param>
  /// <param name="formulaIdOrName">ID or name of the formula as defined in the Registry.</param>
  /// <param name="octave">The octave of the root pitch.</param>
  /// <param name="inversion">The inversion.</param>
  public PitchChord(
    PitchClass root,
    string formulaIdOrName,
    int octave = 4,
    int inversion = 0 )
    : this( root, Registry.ChordFormulas[formulaIdOrName], octave, inversion )
  {
  }

  #endregion

  #region Properties

  /// <summary>
  ///   Gets the pitch at the specified index in the chord's collection of pitches.
  /// </summary>
  /// <param name="index">The zero-based index of the pitch to get.</param>
  /// <returns>The pitch at the specified index.</returns>
  public Pitch this[
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
    return _impl.Equals( obj );
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
  ///   Returns a string that represents the current chord.
  /// </summary>
  /// <returns>A string that represents the current chord.</returns>
  public override string ToString()
  {
    return Name;
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

  /// <summary>
  ///   Parses a string representation of a chord and returns the corresponding Chord object.
  /// </summary>
  /// <param name="s">The string representation of the chord.</param>
  /// <returns>The corresponding Chord object.</returns>
  public static PitchChord Parse(
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
  public static PitchChord Parse(
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
  public static PitchChord Parse(
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
    [NotNullWhen( true )] out PitchChord? chord )
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
    [NotNullWhen( true )] out PitchChord? chord )
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
    [NotNullWhen( true )] out PitchChord? chord )
  {
    // We want to ensure that the entire string is consumed, so we check if the tail is empty after parsing.
    return TryParse( span, provider, out chord, out var tail ) && tail.IsEmpty;
  }

  /// <summary>
  ///   Tries to parse a <see cref="PitchChord"/> from the provided span.
  /// </summary>
  /// <param name="span">The span of characters to parse.</param>
  /// <param name="provider">The format provider.</param>
  /// <param name="chord">The parsed chord, if successful.</param>
  /// <param name="tail">The remaining characters after parsing.</param>
  /// <returns>True if the chord was parsed successfully; otherwise, false.</returns>
  public static bool TryParse(
    ReadOnlySpan<char> span,
    IFormatProvider? provider,
    [NotNullWhen( true )] out PitchChord? chord,
    out ReadOnlySpan<char> tail )
  {
    span = span.TrimStart();

    // If the span is empty, we cannot parse a chord.
    if( span.IsEmpty )
    {
      chord = null;
      tail = ReadOnlySpan<char>.Empty;
      return false;
    }

    // Try to parse the root pitch from the span.
    if( !PitchClass.TryParse( span, provider, out var rootPitchClass, out tail ) )
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

    // If we have a chord formula, we can consume the characters corresponding to the formula's symbol from the tail.
    tail = tail[chordFormula.Symbol.Length..];

    // Do we have a bass note?
    var bassSeparatorPos = tail.IndexOf( '/' );

    if( bassSeparatorPos == -1 )
    {
      chord = new PitchChord( rootPitchClass, chordFormula );
      return true;
    }

    // If we have a bass note, we need to parse it as a pitch.
    if( !TryParseBassPitch( tail[( bassSeparatorPos + 1 )..], provider, out var bassPitch, out tail ) )
    {
      chord = null;
      return false;
    }

    // Determine the inversion before creating the chord. If the bass pitch is not part of the chord, return false.
    var rootPosition = new Chord( rootPitchClass, chordFormula );
    var inversion = rootPosition.IndexOf( bassPitch.PitchClass );

    // If the bass pitch is not part of the chord, return false.
    if( inversion < 0 )
    {
      chord = null;
      return false;
    }

    chord = new PitchChord( rootPitchClass, chordFormula, bassPitch.Octave, inversion );
    return true;

    static bool TryParseBassPitch(
      ReadOnlySpan<char> span,
      IFormatProvider? provider,
      out Pitch pitch,
      out ReadOnlySpan<char> tail )
    {
      // Try to parse the bass pitch as a full pitch first.
      if( Pitch.TryParse( span, provider, out pitch, out var tmpTail ) )
      {
        tail = tmpTail;
        return true;
      }

      // If that fails, try to parse it as a pitch class and assume octave 4.
      if( PitchClass.TryParse( span, provider, out var pitchClass, out tmpTail ) )
      {
        pitch = new Pitch( pitchClass, 4 );
        tail = tmpTail;
        return true;
      }

      tail = tmpTail;
      return false;
    }
  }

  /// <summary>
  ///   Creates a new <see cref="PitchChord"/> instance with the specified root, formula, and inversion.
  /// </summary>
  /// <param name="root">The root pitch of the chord.</param>
  /// <param name="formula">The formula used to generate the chord.</param>
  /// <param name="inversion">The inversion.</param>
  /// <returns>A new <see cref="PitchChord"/> instance with the specified parameters.</returns>
  public static PitchChord Create(
    Pitch root,
    ChordFormula formula,
    int inversion )
  {
    return new PitchChord( root, formula, inversion );
  }

  /// <summary>
  ///   Returns the index of the specified pitch in the chord's collection of pitches.
  /// </summary>
  /// <param name="pitch">The pitch to locate in the chord's collection of pitches.</param>
  /// <returns>The zero-based index of the specified pitch if found; otherwise, -1.</returns>
  public int IndexOf(
    Pitch pitch )
  {
    return _impl.IndexOf( pitch );
  }

  /// <summary>
  ///   Returns an enumerator that iterates through the collection of pitches in the chord.
  /// </summary>
  /// <returns>
  ///   An enumerator that can be used to iterate through the collection of pitches in the chord.
  /// </returns>
  public IEnumerator<Pitch> GetEnumerator()
  {
    return _impl.GetEnumerator();
  }

  /// <summary>
  ///   Determines whether the specified <see cref="PitchChord"/> is equal to the current <see cref="PitchChord"/>.
  /// </summary>
  /// <param name="other">
  ///   The <see cref="PitchChord"/> to compare with the current <see cref="PitchChord"/>.
  /// </param>
  /// <returns>
  ///   <c>true</c> if the specified <see cref="PitchChord"/> is equal to the current <see cref="PitchChord"/>; otherwise,
  ///   <c>false</c>.
  /// </returns>
  public bool Equals(
    PitchChord? other )
  {
    return _impl.Equals( other );
  }

  #endregion

  #region IChord<PitchChord,Pitch> Implementation

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
  public Pitch Root => _impl.Root;

  /// <summary>
  ///   Gets the bass of the chord.
  /// </summary>
  public Pitch Bass => _impl.Bass;

  /// <summary>
  ///   Gets a value indicating whether the chord formula's highest interval extends beyond an octave,
  ///   as in a ninth or eleventh chord. A formula whose highest interval is a seventh is not extended.
  /// </summary>
  public bool IsExtended => _impl.IsExtended;

  #endregion

  #region IEnumerable Implementation

  /// <summary>
  ///   Returns an enumerator that iterates through the collection of pitches in the chord.
  /// </summary>
  /// <returns>
  ///   An enumerator that can be used to iterate through the collection of pitches in the chord.
  /// </returns>
  IEnumerator IEnumerable.GetEnumerator()
  {
    return GetEnumerator();
  }

  #endregion

  #region IPartEvent Implementation

  /// <inheritdoc/>
  IEnumerable<PitchClass> IPartEvent.PitchClasses => this.Select( p => p.PitchClass );

  /// <inheritdoc/>
  bool IPartEvent.Any(
    PitchClass pitchClass )
  {
    return this.Any( p => p.PitchClass == pitchClass );
  }

  #endregion

  #region IReadOnlyCollection<Pitch> Implementation

  /// <summary>
  ///   Gets the number of pitches in the chord.
  /// </summary>
  public int Count => _impl.Count;

  #endregion
}
