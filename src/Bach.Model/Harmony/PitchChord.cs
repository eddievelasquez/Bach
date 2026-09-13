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

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Bach.Model.Internal;

namespace Bach.Model.Harmony;

/// <summary>
///   A chord expressed as a collection of actual pitches rather than pitch classes.
/// </summary>
public class PitchChord
  : Chord<PitchChord, Pitch>,
    IChordFactory<PitchChord, Pitch>,
    IChordParser<PitchChord>,
    IChordEvent
{
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
    : base( root, formula, inversion )
  {
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

  #region Public Methods

  /// <summary>
  ///   Creates an inversion of the current chord.
  /// </summary>
  /// <param name="inversion">The inversion number.</param>
  /// <returns>A new <see cref="PitchChord"/> representing the specified inversion.</returns>
  public new PitchChord GetInversion(
    int inversion )
  {
    return new PitchChord( Root, Formula, inversion );
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

  #endregion

  #region IChordFactory<PitchChord,Pitch> Implementation

  /// <summary>
  ///   Creates a new <see cref="PitchChord"/> instance with the specified root, formula, and inversion.
  /// </summary>
  /// <param name="root">The root pitch of the chord.</param>
  /// <param name="formula">The formula used to generate the chord.</param>
  /// <param name="inversion">The inversion.</param>
  /// <returns>A new <see cref="PitchChord"/> instance with the specified parameters.</returns>
  static PitchChord IChordFactory<PitchChord, Pitch>.Create(
    Pitch root,
    ChordFormula formula,
    int inversion )
  {
    return new PitchChord( root, formula, inversion );
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
}
