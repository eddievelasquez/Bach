// Module Name: Key.cs
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

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

/// <summary>Represents a tonal key defined by a tonic, mode, and key signature.</summary>
public sealed class Key:
  IParsable<Key>,
  ISpanParsable<Key>
{
  #region Constants

  private static readonly Dictionary<(PitchClass Tonic, ModeType Mode), KeySignature> s_keySignatureTable =
    new()
    {
      [( PitchClass.C, ModeType.Major )] = KeySignature.CMajor,
      [( PitchClass.G, ModeType.Major )] = KeySignature.GMajor,
      [( PitchClass.D, ModeType.Major )] = KeySignature.DMajor,
      [( PitchClass.A, ModeType.Major )] = KeySignature.AMajor,
      [( PitchClass.E, ModeType.Major )] = KeySignature.EMajor,
      [( PitchClass.B, ModeType.Major )] = KeySignature.BMajor,
      [( PitchClass.FSharp, ModeType.Major )] = KeySignature.FSharpMajor,
      [( PitchClass.F, ModeType.Major )] = KeySignature.FMajor,
      [( PitchClass.BFlat, ModeType.Major )] = KeySignature.BFlatMajor,
      [( PitchClass.EFlat, ModeType.Major )] = KeySignature.EFlatMajor,
      [( PitchClass.AFlat, ModeType.Major )] = KeySignature.AFlatMajor,
      [( PitchClass.DFlat, ModeType.Major )] = KeySignature.DFlatMajor,
      [( PitchClass.GFlat, ModeType.Major )] = KeySignature.GFlatMajor,
      [( PitchClass.B, ModeType.Major )] = KeySignature.CFlatMajor,
      [( PitchClass.A, ModeType.Minor )] = KeySignature.AMinor,
      [( PitchClass.E, ModeType.Minor )] = KeySignature.EMinor,
      [( PitchClass.B, ModeType.Minor )] = KeySignature.BMinor,
      [( PitchClass.FSharp, ModeType.Minor )] = KeySignature.FSharpMinor,
      [( PitchClass.CSharp, ModeType.Minor )] = KeySignature.CSharpMinor,
      [( PitchClass.GSharp, ModeType.Minor )] = KeySignature.GSharpMinor,
      [( PitchClass.DSharp, ModeType.Minor )] = KeySignature.DSharpMinor,
      [( PitchClass.D, ModeType.Minor )] = KeySignature.DMinor,
      [( PitchClass.G, ModeType.Minor )] = KeySignature.GMinor,
      [( PitchClass.C, ModeType.Minor )] = KeySignature.CMinor,
      [( PitchClass.F, ModeType.Minor )] = KeySignature.FMinor,
      [( PitchClass.BFlat, ModeType.Minor )] = KeySignature.BFlatMinor,
      [( PitchClass.EFlat, ModeType.Minor )] = KeySignature.EFlatMinor,
      [( PitchClass.AFlat, ModeType.Minor )] = KeySignature.AFlatMinor
    };

  #endregion

  #region Constructors

  /// <summary>
  ///   Initializes a new instance of the <see cref="Key" /> class.
  /// </summary>
  /// <param name="pitchClass">The name of the tonic note.</param>
  /// <param name="mode">The mode of the key.</param>
  public Key(
    PitchClass pitchClass,
    ModeType mode )
  {
    Tonic = pitchClass;
    Mode = mode;
    Scale = new Scale( Tonic, ResolveScaleFormula( mode ) );
    KeySignature = s_keySignatureTable.TryGetValue( ( Tonic, Mode ), out var signature )
      ? signature
      : KeySignature.Empty;
  }

  #endregion

  #region Properties

  /// <summary>Gets the tonic pitch class.</summary>
  public PitchClass Tonic { get; }

  /// <summary>Gets the mode for the key.</summary>
  public ModeType Mode { get; }

  /// <summary>Gets the key signature for the key.</summary>
  public KeySignature KeySignature { get; }

  /// <summary>Gets the scale implied by the key.</summary>
  public Scale Scale { get; }

  #endregion

  #region Public Methods

  /// <summary>
  ///   Parses a string representation of a key into a <see cref="Key" /> object.
  /// </summary>
  /// <param name="value">The string representation of the key.</param>
  /// <returns>The parsed <see cref="Key" /> object.</returns>
  public static Key Parse(
    string value )
  {
    ArgumentNullException.ThrowIfNull(value);
    return Parse( value.AsSpan(), null );
  }

  /// <summary>
  ///   Parses a string representation of a key into a <see cref="Key" /> object.
  /// </summary>
  /// <param name="value">The string representation of the key.</param>
  /// <param name="provider">The format provider.</param>
  /// <returns>The parsed <see cref="Key" /> object.</returns>
  /// <exception cref="FormatException"></exception>
  public static Key Parse(
    string value,
    IFormatProvider? provider )
  {
    ArgumentNullException.ThrowIfNull(value);
    return Parse( value.AsSpan(), provider);
  }

  /// <summary>
  /// Parses a string representation of a key into a <see cref="Key" /> object.
  /// </summary>
  /// <param name="s">The string representation of the key.</param>
  /// <param name="provider">The format provider.</param>
  /// <returns>The parsed <see cref="Key" /> object.</returns>
  /// <exception cref="FormatException"></exception>
  public static Key Parse(
    ReadOnlySpan<char> s,
    IFormatProvider? provider )
  {
    return TryParse( s, provider, out var result ) ? result : throw new FormatException();
  }


  /// <inheritdoc />
  public override string ToString()
  {
    return $"{Tonic} {Mode}";
  }

  /// <summary>
  ///   Attempts to parse a string representation of a key into a <see cref="Key" /> object.
  /// </summary>
  /// <param name="s">The string representation of the key.</param>
  /// <param name="result">The parsed <see cref="Key" /> object if successful; otherwise, null.</param>
  /// <returns>True if the parsing was successful; otherwise, false.</returns>
  public static bool TryParse(
    string? s,
    [MaybeNullWhen( false )] out Key result )
  {
    return TryParse( s.AsSpan(), null, out result );
  }

  /// <summary>
  ///   Attempts to parse a string representation of a key into a <see cref="Key" /> object.
  /// </summary>
  /// <param name="s">The string representation of the key.</param>
  /// <param name="provider">The format provider.</param>
  /// <param name="result">The parsed <see cref="Key" /> object if successful; otherwise, null.</param>
  /// <returns>True if the parsing was successful; otherwise, false.</returns>
  public static bool TryParse(
    string? s,
    IFormatProvider? provider,
    [MaybeNullWhen( false )] out Key result )
  {
    return TryParse( s.AsSpan(), provider, out result );
  }

  #endregion

  /// <summary>
  /// Attempts to parse a string representation of a key into a <see cref="Key" /> object.
  /// </summary>
  /// <param name="value">The value representing the string representation of the key.</param>
  /// <param name="provider">The format provider.</param>
  /// <param name="result">The parsed <see cref="Key" /> object if successful; otherwise, null.</param>
  /// <returns>True if the parsing was successful; otherwise, false.</returns>
  public static bool TryParse(
    ReadOnlySpan<char> value,
    IFormatProvider? provider,
    [MaybeNullWhen(false)] out Key result )
  {
    value = value.Trim();

    if( value.IsEmpty )
    {
      result = null;
      return false;
    }

    var modeType = ModeType.Major;
    var pitchClassSpan = value;

    // Check if the last character indicates the mode (M for Major (optional), m for Minor)
    if( value.Length > 1 )
    {
      switch (value[^1])
      {
        case 'm':
          modeType = ModeType.Minor;
          pitchClassSpan = value[..^1]; // Exclude the mode from the pitch class span
          break;

        case 'M':
          modeType = ModeType.Major;
          pitchClassSpan = value[..^1]; // Exclude the mode from the pitch class span
          break;
      }
    }

    if( !PitchClass.TryParse( pitchClassSpan, provider, out var pitchClass ) )
    {
      result = null;
      return false;
    }

    result = new Key( pitchClass, modeType );
    return true;
  }

  #region Implementation

  private static string ResolveScaleFormula(
    ModeType mode )
  {
    return mode switch
    {
      ModeType.Minor => "NaturalMinor",
      _              => "Major"
    };
  }

  #endregion
}
