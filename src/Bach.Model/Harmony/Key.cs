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

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using Bach.Model.Internal;

namespace Bach.Model.Harmony;

/// <summary>Represents a tonal key defined by a tonic, mode, and key signature.</summary>
public sealed class Key
  : ISpanParsable<Key>,
    IFormattable
{
  #region Constants

  private static readonly Dictionary<(PitchClass Tonic, bool IsMinor), KeySignature> s_keySignatureTable =
    new()
    {
      [( PitchClass.C, false )] = KeySignature.CMajor,
      [( PitchClass.G, false )] = KeySignature.GMajor,
      [( PitchClass.D, false )] = KeySignature.DMajor,
      [( PitchClass.A, false )] = KeySignature.AMajor,
      [( PitchClass.E, false )] = KeySignature.EMajor,
      [( PitchClass.B, false )] = KeySignature.BMajor,
      [( PitchClass.FSharp, false )] = KeySignature.FSharpMajor,
      [( PitchClass.F, false )] = KeySignature.FMajor,
      [( PitchClass.BFlat, false )] = KeySignature.BFlatMajor,
      [( PitchClass.EFlat, false )] = KeySignature.EFlatMajor,
      [( PitchClass.AFlat, false )] = KeySignature.AFlatMajor,
      [( PitchClass.DFlat, false )] = KeySignature.DFlatMajor,
      [( PitchClass.GFlat, false )] = KeySignature.GFlatMajor,
      [( PitchClass.B, false )] = KeySignature.CFlatMajor,
      [( PitchClass.A, true )] = KeySignature.AMinor,
      [( PitchClass.E, true )] = KeySignature.EMinor,
      [( PitchClass.B, true )] = KeySignature.BMinor,
      [( PitchClass.FSharp, true )] = KeySignature.FSharpMinor,
      [( PitchClass.CSharp, true )] = KeySignature.CSharpMinor,
      [( PitchClass.GSharp, true )] = KeySignature.GSharpMinor,
      [( PitchClass.DSharp, true )] = KeySignature.DSharpMinor,
      [( PitchClass.D, true )] = KeySignature.DMinor,
      [( PitchClass.G, true )] = KeySignature.GMinor,
      [( PitchClass.C, true )] = KeySignature.CMinor,
      [( PitchClass.F, true )] = KeySignature.FMinor,
      [( PitchClass.BFlat, true )] = KeySignature.BFlatMinor,
      [( PitchClass.EFlat, true )] = KeySignature.EFlatMinor,
      [( PitchClass.AFlat, true )] = KeySignature.AFlatMinor
    };

  #endregion

  #region Constructors

  /// <summary>
  ///   Initializes a new instance of the <see cref="Key"/> class using a governing scale or mode.
  /// </summary>
  /// <param name="pitchClass">The tonic pitch class.</param>
  /// <param name="scaleDefinition">The governing scale or mode.</param>
  /// <param name="alterations">Optional local degree alterations.</param>
  public Key(
    PitchClass pitchClass,
    ScaleDefinition scaleDefinition,
    IEnumerable<DegreeAlteration>? alterations = null )
  {
    Tonic = pitchClass;
    ScaleDefinition = scaleDefinition ?? throw new ArgumentNullException( nameof( scaleDefinition ) );

    Scale = new Scale( Tonic, ScaleDefinition.Formula );

    var signatureTonic = ScaleDefinition.RelativeMajorInterval is { } relativeMajorInterval
      ? Tonic - relativeMajorInterval
      : Tonic;

    var signatureIsMinor = ScaleDefinition.RelativeMajorInterval is null && ScaleDefinition.IsMinor;

    if( s_keySignatureTable.TryGetValue( ( signatureTonic, signatureIsMinor ), out var signature ) )
    {
      KeySignature = signature;
    }
    else
    {
      KeySignature = KeySignature.Empty;
    }

    DegreeAlterations = ( alterations ?? [] ).ToArray();
  }

  #endregion

  #region Properties

  /// <summary>Gets the tonic pitch class.</summary>
  public PitchClass Tonic { get; }

  /// <summary>Gets the governing scale or mode for the key.</summary>
  public ScaleDefinition ScaleDefinition { get; }

  /// <summary>Gets the key signature for the key.</summary>
  public KeySignature KeySignature { get; }

  /// <summary>Gets the scale implied by the key.</summary>
  public Scale Scale { get; }

  /// <summary>Gets any local degree alterations for the key.</summary>
  public IReadOnlyList<DegreeAlteration> DegreeAlterations { get; }

  #endregion

  #region Public Methods

  /// <summary>
  ///   Parses a string representation of a key into a <see cref="Key"/> object.
  /// </summary>
  /// <param name="value">The string representation of the key.</param>
  /// <returns>The parsed <see cref="Key"/> object.</returns>
  public static Key Parse(
    string value )
  {
    ArgumentNullException.ThrowIfNull( value );
    return Parse( value.AsSpan(), null );
  }

  /// <summary>
  ///   Parses a string representation of a key into a <see cref="Key"/> object.
  /// </summary>
  /// <param name="value">The string representation of the key.</param>
  /// <param name="provider">The format provider.</param>
  /// <returns>The parsed <see cref="Key"/> object.</returns>
  /// <exception cref="FormatException"></exception>
  public static Key Parse(
    string value,
    IFormatProvider? provider )
  {
    ArgumentNullException.ThrowIfNull( value );
    return Parse( value.AsSpan(), provider );
  }

  /// <summary>
  ///   Parses a string representation of a key into a <see cref="Key"/> object.
  /// </summary>
  /// <param name="s">The string representation of the key.</param>
  /// <param name="provider">The format provider.</param>
  /// <returns>The parsed <see cref="Key"/> object.</returns>
  /// <exception cref="FormatException"></exception>
  public static Key Parse(
    ReadOnlySpan<char> s,
    IFormatProvider? provider )
  {
    return TryParse( s, provider, out var result ) ? result : throw new FormatException( $"Invalid key format: '{s}'" );
  }

  /// <inheritdoc/>
  public override string ToString()
  {
    return $"{Tonic} {ScaleDefinition.Name}";
  }

  /// <summary>
  ///   Returns a string representation of the value of this <see cref="Key"/> instance according to the provided
  ///   format.
  /// </summary>
  /// <param name="format">A format string.</param>
  /// <returns>A formatted string.</returns>
  public string ToString(
    string? format )
  {
    return ToString( format, null );
  }

  /// <summary>
  ///   Returns a string representation of the value of this <see cref="Key"/> instance according to the provided
  ///   format and format provider.
  /// </summary>
  /// <param name="format">A format string.</param>
  /// <param name="provider">The format provider.</param>
  /// <returns>A formatted string.</returns>
  public string ToString(
    string? format,
    IFormatProvider? provider )
  {
    if( string.IsNullOrEmpty( format ) )
    {
      return ToString();
    }

    var buf = new StringBuilder();

    foreach( var f in format )
    {
      switch( f )
      {
        case 'T':
          buf.Append( Tonic );
          break;

        case 'M':
          buf.Append( ScaleDefinition.Name );
          break;

        case 'S':
          buf.Append( KeySignature );
          break;

        default:
          buf.Append( f );
          break;
      }
    }

    return buf.ToString();
  }

  /// <summary>
  ///   Attempts to parse a string representation of a key into a <see cref="Key"/> object.
  /// </summary>
  /// <param name="s">The string representation of the key.</param>
  /// <param name="result">The parsed <see cref="Key"/> object if successful; otherwise, null.</param>
  /// <returns>True if the parsing was successful; otherwise, false.</returns>
  public static bool TryParse(
    string? s,
    [MaybeNullWhen( false )] out Key result )
  {
    return TryParse( s, null, out result );
  }

  /// <summary>
  ///   Attempts to parse a string representation of a key into a <see cref="Key"/> object.
  /// </summary>
  /// <param name="s">The string representation of the key.</param>
  /// <param name="provider">The format provider.</param>
  /// <param name="result">The parsed <see cref="Key"/> object if successful; otherwise, null.</param>
  /// <returns>True if the parsing was successful; otherwise, false.</returns>
  public static bool TryParse(
    string? s,
    IFormatProvider? provider,
    [MaybeNullWhen( false )] out Key result )
  {
    if( s is null )
    {
      result = null;
      return false;
    }

    return TryParse( s.AsSpan(), provider, out result );
  }

  /// <summary>
  ///   Attempts to parse a string representation of a key into a <see cref="Key"/> object.
  /// </summary>
  /// <param name="value">The value representing the string representation of the key.</param>
  /// <param name="provider">The format provider.</param>
  /// <param name="result">The parsed <see cref="Key"/> object if successful; otherwise, null.</param>
  /// <returns>True if the parsing was successful; otherwise, false.</returns>
  public static bool TryParse(
    ReadOnlySpan<char> value,
    IFormatProvider? provider,
    [NotNullWhen( true )] out Key? result )
  {
    value = value.Trim();

    if( value.IsEmpty )
    {
      result = null;
      return false;
    }

    var scaleDefinition = ScaleDefinition.Major;
    var pitchClassSpan = value;

    // Check if the last character indicates the mode (M for Major (optional), m for Minor)
    if( value.Length > 1 )
    {
      switch( value[^1] )
      {
        case Constants.MinorIntervalQualitySymbol:
          scaleDefinition = ScaleDefinition.NaturalMinor;
          pitchClassSpan = value[..^1]; // Exclude the mode from the pitch class span
          break;

        case Constants.MajorIntervalQualitySymbol:
          scaleDefinition = ScaleDefinition.Major;
          pitchClassSpan = value[..^1]; // Exclude the mode from the pitch class span
          break;
      }
    }

    if( !PitchClass.TryParse( pitchClassSpan, provider, out var pitchClass ) )
    {
      result = null;
      return false;
    }

    result = new Key( pitchClass, scaleDefinition );
    return true;
  }

  #endregion
}
