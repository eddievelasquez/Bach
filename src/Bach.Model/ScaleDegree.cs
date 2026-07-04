// Module Name: ScaleDegree.cs
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

using System.Diagnostics;
using System.Linq;

/// <summary>Represents a scale degree and resolves it within a key.</summary>
public readonly struct ScaleDegree: IParsable<ScaleDegree>
{
  #region Constants

  /// <summary>Represents the tonic scale degree.</summary>
  public static readonly ScaleDegree Tonic = new( nameof( Tonic ), 1, "I" );

  /// <summary>Represents the supertonic scale degree.</summary>
  public static readonly ScaleDegree Supertonic = new( nameof( Supertonic ), 2, "ii" );

  /// <summary>Represents the mediant scale degree.</summary>
  public static readonly ScaleDegree Mediant = new( nameof( Mediant ), 3, "iii" );

  /// <summary>Represents the subdominant scale degree.</summary>
  public static readonly ScaleDegree Subdominant = new( nameof( Subdominant ), 4, "IV" );

  /// <summary>Represents the dominant scale degree.</summary>
  public static readonly ScaleDegree Dominant = new( nameof( Dominant ), 5, "V" );

  /// <summary>Represents the submediant scale degree.</summary>
  public static readonly ScaleDegree Submediant = new( nameof( Submediant ), 6, "vi" );

  /// <summary>Represents the leading-tone scale degree.</summary>
  public static readonly ScaleDegree LeadingTone = new( nameof( LeadingTone ), 7, "vii" );

  /// <summary>
  /// Represents an array of all scale degrees in order from tonic to leading tone.
  /// </summary>
  public static ScaleDegree[] ScaleDegrees = [ Tonic, Supertonic, Mediant, Subdominant, Dominant, Submediant, LeadingTone ];

  #endregion

  #region Constructors

  private ScaleDegree(
    string name,
    int degree,
    string symbol )
  {
    Name = name;
    Degree = degree;
    Symbol = symbol;
  }

  #endregion

  #region Properties

  /// <summary>Gets the name of the scale degree.</summary>
  public string Name { get; }

  /// <summary>Gets the numeric degree.</summary>
  public int Degree { get; }

  /// <summary>Gets the roman numeral symbol.</summary>
  public string Symbol { get; }

  #endregion

  #region Public Methods

  /// <summary>Determines whether the supplied value can be parsed as a Nashville number scale degree.</summary>
  /// <param name="value">The candidate value.</param>
  /// <returns>True when the value is a Nashville number scale degree; otherwise, false.</returns>
  public static bool IsNashville(
    string? value )
  {
    if( string.IsNullOrWhiteSpace( value ) )
    {
      return false;
    }

    return TryParseNashville( value.AsSpan(), out _ );
  }

  /// <summary>Determines whether the supplied value can be parsed as a roman numeral scale degree.</summary>
  /// <param name="value">The candidate value.</param>
  /// <returns>True when the value is a roman numeral scale degree; otherwise, false.</returns>
  public static bool IsRomanNumeral(
    string? value )
  {
    if( string.IsNullOrWhiteSpace( value ) )
    {
      return false;
    }

    return TryParseRomanNumeral( value.AsSpan(), out _ );
  }

  /// <summary>Parses a scale degree from the supplied string.</summary>
  /// <param name="value">The scale degree value.</param>
  /// <returns>The parsed scale degree.</returns>
  /// <exception cref="ArgumentException">Thrown when the string is null, empty, or whitespace.</exception>
  /// <exception cref="FormatException">Thrown when the string is not a valid scale degree.</exception>
  public static ScaleDegree Parse(
    string value )
  {
    return Parse( value, null );
  }

  /// <summary>Parses a scale degree from the supplied string.</summary>
  /// <param name="s">The scale degree value.</param>
  /// <param name="provider">The format provider.</param>
  /// <returns>The parsed scale degree.</returns>
  /// <exception cref="ArgumentException">Thrown when the string is null, empty, or whitespace.</exception>
  /// <exception cref="FormatException">Thrown when the string is not a valid scale degree.</exception>
  public static ScaleDegree Parse(
    string s,
    IFormatProvider? provider )
  {
    ArgumentException.ThrowIfNullOrWhiteSpace( s );

    return TryParse( s, provider, out var scaleDegree )
      ? scaleDegree
      : throw new FormatException( $"The value '{s}' is not a valid scale degree." );
  }

  /// <summary>Resolves the degree to a pitch class in the supplied key.</summary>
  /// <param name="key">The key to resolve against.</param>
  /// <returns>The pitch class for the degree.</returns>
  public PitchClass Resolve(
    Key key )
  {
    ArgumentNullException.ThrowIfNull( key );

    var scale = key.Scale.GetAscending()
                   .ToArray();

    var index = Degree - 1;
    return scale[index % scale.Length];
  }

  /// <summary>Resolves the degree to a diatonic triad in the supplied key.</summary>
  /// <param name="key">The key to resolve against.</param>
  /// <returns>The diatonic triad for the degree.</returns>
  public Triad ResolveDiatonicTriad(
    Key key )
  {
    ArgumentNullException.ThrowIfNull( key );

    var root = Resolve( key );
    var quality = GetDiatonicTriadQuality( key.Mode );
    return new Triad( root, quality );
  }

  /// <inheritdoc />
  public override string ToString()
  {
    return Symbol;
  }

  /// <summary>Attempts to parse a scale degree from the supplied string.</summary>
  /// <param name="value">The scale degree value.</param>
  /// <param name="scaleDegree">The parsed scale degree, if successful.</param>
  /// <returns>True when the parse succeeds; otherwise, false.</returns>
  public static bool TryParse(
    string? value,
    out ScaleDegree scaleDegree )
  {
    return TryParse( value, null, out scaleDegree );
  }

  /// <summary>Attempts to parse a scale degree from the supplied string.</summary>
  /// <param name="s">The scale degree value.</param>
  /// <param name="provider">The format provider.</param>
  /// <param name="scaleDegree">The parsed scale degree, if successful.</param>
  /// <returns>True when the parse succeeds; otherwise, false.</returns>
  public static bool TryParse(
    string? s,
    IFormatProvider? provider,
    out ScaleDegree scaleDegree )
  {
    if( string.IsNullOrWhiteSpace( s ) )
    {
      scaleDegree = default;
      return false;
    }

    var trimmed = s.AsSpan()
                   .Trim();

    if( trimmed.IsEmpty )
    {
      scaleDegree = default;
      return false;
    }

    return char.IsAsciiDigit( trimmed[0] )
      ? TryParseNashville( trimmed, out scaleDegree )
      : TryParseRomanNumeral( trimmed, out scaleDegree );
  }

  #endregion

  #region Implementation

  /// <summary>Attempts to parse the supplied value as a roman numeral scale degree.</summary>
  /// <param name="value">The candidate value.</param>
  /// <param name="scaleDegree">The parsed scale degree, if successful.</param>
  /// <returns>True when the parse succeeds; otherwise, false.</returns>
  private static bool TryParseRomanNumeral(
    ReadOnlySpan<char> value,
    out ScaleDegree scaleDegree )
  {
    Debug.Assert( !value.IsEmpty);

    scaleDegree = default;

    var parsed = ParseRoman( value );
    if( parsed == -1 )
    {
      return false;
    }

    scaleDegree = ScaleDegrees[parsed - 1];
    return true;
  }

  /// <summary>
  /// Parses a roman numeral string and returns the corresponding scale degree number (1-7).
  /// </summary>
  /// <param name="s">The roman numeral string.</param>
  /// <returns>The corresponding scale degree number (1-7), or -1 if invalid.</returns>
  private static int ParseRoman( ReadOnlySpan<char> s )
  {
    return s.Length switch
    {
      1 => ToUpperAscii( s[0] ) switch
      {
        'I' => 1,
        'V' => 5,
        _   => -1
      },

      2 => ToUpperAscii( s[0] ) switch
      {
        'I' => ToUpperAscii( s[1] ) switch
        {
          'I' => 2,
          'V' => 4,
          _   => -1
        },
        'V' => ToUpperAscii( s[1] ) switch
        {
          'I' => 6,
          _   => -1
        },
        _ => -1
      },

      3 => ToUpperAscii( s[0] ) switch
      {
        'I' => ToUpperAscii( s[1] ) == 'I' && ToUpperAscii( s[2] ) == 'I' ? 3 : -1,
        'V' => ToUpperAscii( s[1] ) == 'I' && ToUpperAscii( s[2] ) == 'I' ? 7 : -1,
        _   => -1
      },

      _ => -1
    };

    static char ToUpperAscii( char c ) => (char) ( c & ~0x20 ); // ASCII uppercase
  }

  /// <summary>Attempts to parse the supplied value as a Nashville number scale degree.</summary>
  /// <param name="value">The candidate value.</param>
  /// <param name="scaleDegree">The parsed scale degree, if successful.</param>
  /// <returns>True when the parse succeeds; otherwise, false.</returns>
  private static bool TryParseNashville(
    ReadOnlySpan<char> value,
    out ScaleDegree scaleDegree )
  {
    scaleDegree = default;

    if( !int.TryParse( value, out var parsed ) || parsed is < 1 or > 7 )
    {
      return false;
    }

    scaleDegree = ScaleDegrees[parsed - 1];
    return true;
  }

  /// <summary>
  /// Gets the diatonic triad quality for the scale degree in the specified mode.
  /// </summary>
  /// <param name="mode">The mode to use for determining the triad quality.</param>
  /// <returns>The diatonic triad quality for the scale degree in the specified mode.</returns>
  /// <exception cref="ArgumentOutOfRangeException">Thrown when the mode or degree is invalid.</exception>
  private TriadQuality GetDiatonicTriadQuality(
    ModeType mode )
  {
    return mode switch
    {
      ModeType.Major => Degree switch
      {
        1 => TriadQuality.Major,
        2 => TriadQuality.Minor,
        3 => TriadQuality.Minor,
        4 => TriadQuality.Major,
        5 => TriadQuality.Major,
        6 => TriadQuality.Minor,
        7 => TriadQuality.Diminished,
        _ => throw new ArgumentOutOfRangeException( nameof( Degree ), Degree, "Invalid scale degree." )
      },
      ModeType.Minor => Degree switch
      {
        1 => TriadQuality.Minor,
        2 => TriadQuality.Diminished,
        3 => TriadQuality.Major,
        4 => TriadQuality.Minor,
        5 => TriadQuality.Minor,
        6 => TriadQuality.Major,
        7 => TriadQuality.Major,
        _ => throw new ArgumentOutOfRangeException( nameof( Degree ), Degree, "Invalid scale degree." )
      },
      _ => throw new ArgumentOutOfRangeException( nameof( mode ), mode, "Unsupported mode." )
    };
  }

  #endregion
}
