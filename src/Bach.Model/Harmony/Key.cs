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

namespace Bach.Model.Harmony;

/// <summary>Represents a tonal key defined by a tonic, mode, and key signature.</summary>
public sealed class Key
  : ISpanParsable<Key>,
    IFormattable
{
  #region Constants

  private const string DEFAULT_TO_STRING_FORMAT = "T M";

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

    DegreeAlterations = alterations.AsReadOnlyArray();
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
  ///   Parses a key from its string representation.
  /// </summary>
  /// <param name="value">The key string to parse.</param>
  /// <returns>The parsed key.</returns>
  public static Key Parse(
    string value )
  {
    ArgumentNullException.ThrowIfNull( value );
    return Parse( value.AsSpan(), null );
  }

  /// <summary>
  ///   Parses a key from its string representation.
  /// </summary>
  /// <param name="value">The key string to parse.</param>
  /// <param name="provider">The format provider.</param>
  /// <returns>The parsed key.</returns>
  /// <exception cref="FormatException">Thrown when the string is not a valid key.</exception>
  public static Key Parse(
    string value,
    IFormatProvider? provider )
  {
    ArgumentNullException.ThrowIfNull( value );
    return Parse( value.AsSpan(), provider );
  }

  /// <summary>
  ///   Parses a key from its string representation.
  /// </summary>
  /// <param name="s">The key string to parse.</param>
  /// <param name="provider">The format provider.</param>
  /// <returns>The parsed key.</returns>
  /// <exception cref="FormatException">Thrown when the span is not a valid key.</exception>
  public static Key Parse(
    ReadOnlySpan<char> s,
    IFormatProvider? provider )
  {
    return TryParse( s, provider, out var result ) ? result : throw new FormatException( $"Invalid key format: '{s}'" );
  }

  /// <inheritdoc/>
  public override string ToString()
  {
    return ToString( DEFAULT_TO_STRING_FORMAT, null );
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
  /// <remarks>
  ///   <para>
  ///     The default format is <c>"T M"</c>, which produces a human-readable tonic and governing scale or mode label.
  ///   </para>
  ///   <para>Format specifiers:</para>
  ///   <para><c>"T"</c>: Tonic.</para>
  ///   <para><c>"M"</c>: Governing scale or mode name.</para>
  ///   <para><c>"S"</c>: Key signature.</para>
  ///   <para>The format provider is not used.</para>
  /// </remarks>
  public string ToString(
    string? format,
    IFormatProvider? provider )
  {
    if( string.IsNullOrEmpty( format ) )
    {
      format = DEFAULT_TO_STRING_FORMAT;
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
  ///   Attempts to parse a key string. The format is a tonic and scale name, an optional relative-major interval after
  ///   <c>@</c>, and optional comma-separated degree alterations after <c>;</c>.
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

    var alterationSeparator = value.IndexOf( ';' );

    if( alterationSeparator >= 0 && value[( alterationSeparator + 1 )..].IndexOf( ';' ) >= 0 )
    {
      result = null;
      return false;
    }

    var keyText = alterationSeparator >= 0 ? value[..alterationSeparator].Trim() : value;
    var alterationText = alterationSeparator >= 0 ? value[( alterationSeparator + 1 )..].Trim() : ReadOnlySpan<char>.Empty;

    if( ( alterationSeparator >= 0 && alterationText.IsEmpty )
        || !TryParseKeyDefinition( keyText, provider, out var pitchClass, out var scaleDefinition )
        || !TryParseAlterations( alterationText, provider, out var alterations ) )
    {
      result = null;
      return false;
    }

    result = new Key( pitchClass, scaleDefinition, alterations );
    return true;

    static bool TryParseKeyDefinition(
      ReadOnlySpan<char> keyText,
      IFormatProvider? provider,
      out PitchClass pitchClass,
      [NotNullWhen( true )] out ScaleDefinition? scaleDefinition )
    {
      var definitionSeparator = keyText.IndexOfAny( ' ', '\t' );
      var tonicText = definitionSeparator >= 0 ? keyText[..definitionSeparator] : keyText;
      var definitionText = definitionSeparator >= 0 ? keyText[( definitionSeparator + 1 )..].Trim() : ReadOnlySpan<char>.Empty;

      if( definitionText.IsEmpty )
      {
        scaleDefinition = ScaleDefinition.Major;

        if( tonicText.Length > 1 )
        {
          switch( tonicText[^1] )
          {
            case Constants.MinorIntervalQualitySymbol:
              scaleDefinition = ScaleDefinition.NaturalMinor;
              tonicText = tonicText[..^1];
              break;

            case Constants.MajorIntervalQualitySymbol:
              tonicText = tonicText[..^1];
              break;
          }
        }

        return PitchClass.TryParse( tonicText, provider, out pitchClass );
      }

      if( !PitchClass.TryParse( tonicText, provider, out pitchClass )
          || !TryParseScaleDefinition( definitionText, provider, out scaleDefinition ) )
      {
        scaleDefinition = null;
        return false;
      }

      return true;
    }

    static bool TryParseAlterations(
      ReadOnlySpan<char> alterationText,
      IFormatProvider? provider,
      out List<DegreeAlteration> alterations )
    {
      alterations = [];

      if( alterationText.IsEmpty )
      {
        return true;
      }

      while( true )
      {
        var separator = alterationText.IndexOf( ',' );
        var item = ( separator >= 0 ? alterationText[..separator] : alterationText ).Trim();
        var equalsSeparator = item.IndexOf( '=' );

        if( item.IsEmpty
            || equalsSeparator < 0
            || item[( equalsSeparator + 1 )..].IndexOf( '=' ) >= 0
            || !ScaleDegree.TryParse( item[..equalsSeparator], provider, out var degree )
            || !Interval.TryParse( item[( equalsSeparator + 1 )..], provider, out var interval ) )
        {
          return false;
        }

        alterations.Add( new DegreeAlteration( degree, interval ) );

        if( separator < 0 )
        {
          return true;
        }

        alterationText = alterationText[( separator + 1 )..];
      }
    }

    static bool TryParseScaleDefinition(
      ReadOnlySpan<char> definitionText,
      IFormatProvider? provider,
      [NotNullWhen( true )] out ScaleDefinition? definition )
    {
      var relativeIntervalSeparator = definitionText.LastIndexOf( '@' );
      var formulaName = relativeIntervalSeparator >= 0
        ? definitionText[..relativeIntervalSeparator].Trim()
        : definitionText.Trim();
      Interval? relativeMajorInterval = null;
      var hasRelativeIntervalMarker = relativeIntervalSeparator >= 0;

      if( formulaName.IsEmpty )
      {
        definition = null;
        return false;
      }

      if( relativeIntervalSeparator >= 0 )
      {
        var relativeIntervalText = definitionText[( relativeIntervalSeparator + 1 )..].Trim();

        if( relativeIntervalText.IsEmpty )
        {
          definition = null;
          return false;
        }

        if( !relativeIntervalText.Equals( "-", StringComparison.Ordinal ) )
        {
          if( !Interval.TryParse( relativeIntervalText, provider, out var relativeInterval ) )
          {
            definition = null;
            return false;
          }

          relativeMajorInterval = relativeInterval;
        }
      }

      if( !Registry.Instance.TryGetScaleFormula( formulaName, out var formula ) )
      {
        definition = null;
        return false;
      }

      definition = hasRelativeIntervalMarker
        ? CreateScaleDefinition( formula.Id, relativeMajorInterval )
        : CreateDefaultScaleDefinition( formula.Id );
      return true;
    }
  }

  private static ScaleDefinition CreateScaleDefinition(
    string formulaId,
    Interval? relativeMajorInterval )
  {
    return ( formulaId, relativeMajorInterval ) switch
    {
      ( "Major", null )                                                  => ScaleDefinition.Major,
      ( "NaturalMinor", null )                                           => ScaleDefinition.NaturalMinor,
      ( "HarmonicMinor", null )                                          => ScaleDefinition.HarmonicMinor,
      ( "MelodicMinor", null )                                           => ScaleDefinition.MelodicMinor,
      ( "Ionian", { } interval ) when interval == Interval.Unison        => ScaleDefinition.Ionian,
      ( "Dorian", { } interval ) when interval == Interval.MajorSecond   => ScaleDefinition.Dorian,
      ( "Phrygian", { } interval ) when interval == Interval.MajorThird  => ScaleDefinition.Phrygian,
      ( "Lydian", { } interval ) when interval == Interval.Fourth        => ScaleDefinition.Lydian,
      ( "Mixolydian", { } interval ) when interval == Interval.Fifth     => ScaleDefinition.Mixolydian,
      ( "Aeolian", { } interval ) when interval == Interval.MajorSixth   => ScaleDefinition.Aeolian,
      ( "Locrian", { } interval ) when interval == Interval.MajorSeventh => ScaleDefinition.Locrian,
      _                                                                  => ScaleDefinition.FromFormulaId( formulaId, relativeMajorInterval )
    };
  }

  private static ScaleDefinition CreateDefaultScaleDefinition(
    string formulaId )
  {
    return formulaId switch
    {
      "Major" => ScaleDefinition.Major,
      "NaturalMinor" => ScaleDefinition.NaturalMinor,
      "HarmonicMinor" => ScaleDefinition.HarmonicMinor,
      "MelodicMinor" => ScaleDefinition.MelodicMinor,
      "Ionian" => ScaleDefinition.Ionian,
      "Dorian" => ScaleDefinition.Dorian,
      "Phrygian" => ScaleDefinition.Phrygian,
      "Lydian" => ScaleDefinition.Lydian,
      "Mixolydian" => ScaleDefinition.Mixolydian,
      "Aeolian" => ScaleDefinition.Aeolian,
      "Locrian" => ScaleDefinition.Locrian,
      _ => ScaleDefinition.FromFormulaId( formulaId )
    };
  }

  #endregion
}
