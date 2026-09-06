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

using System.Diagnostics;
using System.Linq;

namespace Bach.Model;

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
  ///   Represents an array of all scale degrees in order from tonic to leading tone.
  /// </summary>
  public static ScaleDegree[] ScaleDegrees = [Tonic, Supertonic, Mediant, Subdominant, Dominant, Submediant, LeadingTone];

  #endregion

  #region Constructors

  private ScaleDegree(
    string name,
    int degree,
    string symbol )
  {
    Debug.Assert( !string.IsNullOrWhiteSpace(name) );
    Debug.Assert( !string.IsNullOrWhiteSpace(symbol) );
    Debug.Assert( degree is >= 1 and <= 7 );

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
    return TryParseNashville( value.AsSpan(), out _ );
  }

  /// <summary>Determines whether the supplied value can be parsed as a roman numeral scale degree.</summary>
  /// <param name="value">The candidate value.</param>
  /// <returns>True when the value is a roman numeral scale degree; otherwise, false.</returns>
  public static bool IsRomanNumeral(
    string? value )
  {
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

    // Preserve existing behavior: use ascending collection as the default direction.
    return ResolveDiatonicTriad( key, ascending: true );
  }

  /// <summary>Resolves the degree to a diatonic triad in the supplied key using the requested direction.</summary>
  /// <param name="key">The key to resolve against.</param>
  /// <param name="ascending">True to use the formula's ascending degrees; false to use descending degrees.</param>
  /// <returns>The diatonic triad for the degree.</returns>
  public Triad ResolveDiatonicTriad(
    Key key,
    bool ascending )
  {
    ArgumentNullException.ThrowIfNull( key );

    PitchClass[] degreePitchClasses;

    var degreeCount = key.Scale.Formula.AscendingDegrees.Count;

    if( ascending )
    {
      degreePitchClasses = key.Scale.GetAscending()
                                .Take( degreeCount )
                                .ToArray();
    }
    else
    {
      var desc = key.Scale.GetDescending()
                        .Take( degreeCount )
                        .ToArray();

      degreePitchClasses = new PitchClass[degreeCount];
      // tonic
      degreePitchClasses[0] = desc[0];
      // remaining degrees are in reverse order in the descending sequence
      for( var d = 2; d <= degreeCount; d++ )
      {
        degreePitchClasses[d - 1] = desc[degreeCount - d + 1];
      }
    }

    var index = Degree - 1;
    var root = degreePitchClasses[index % degreePitchClasses.Length];
    var third = degreePitchClasses[( index + 2 ) % degreePitchClasses.Length];
    var fifth = degreePitchClasses[( index + 4 ) % degreePitchClasses.Length];

    var thirdInterval = root.GetIntervalTo( third );
    var fifthInterval = root.GetIntervalTo( fifth );

    var quality = ClassifyTriadQuality( thirdInterval, fifthInterval );
    return new Triad( root, quality );
  }

  /// <summary>Resolves an applied dominant triad for a target degree in the supplied key.</summary>
  /// <param name="key">The key containing the target degree.</param>
  /// <param name="targetDegree">The degree that receives the applied dominant.</param>
  /// <returns>The applied dominant triad.</returns>
  public AppliedTriad ResolveAppliedDominant(
    Key key,
    ScaleDegree targetDegree )
  {
    ArgumentNullException.ThrowIfNull( key );
    ValidateTargetDegree( targetDegree );

    var target = targetDegree.Resolve( key );
    var triad = new Triad( target + Interval.Fifth, TriadQuality.Major );
    return new AppliedTriad( triad, targetDegree, AppliedTriadFunction.Dominant );
  }

  /// <summary>Resolves an applied leading-tone triad for a target degree in the supplied key.</summary>
  /// <param name="key">The key containing the target degree.</param>
  /// <param name="targetDegree">The degree that receives the applied leading-tone triad.</param>
  /// <returns>The applied leading-tone triad.</returns>
  public AppliedTriad ResolveAppliedLeadingTone(
    Key key,
    ScaleDegree targetDegree )
  {
    ArgumentNullException.ThrowIfNull( key );
    ValidateTargetDegree( targetDegree );

    var target = targetDegree.Resolve( key );
    var triad = new Triad( target - Interval.MinorSecond, TriadQuality.Diminished );
    return new AppliedTriad( triad, targetDegree, AppliedTriadFunction.LeadingTone );
  }

  /// <inheritdoc/>
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
    var trimmed = value.Trim();

    if( trimmed.IsEmpty )
    {
      scaleDegree = default;
      return false;
    }

    var parsed = ParseRomanNumeral( trimmed );

    if( parsed == -1 )
    {
      scaleDegree = default;
      return false;
    }

    scaleDegree = ScaleDegrees[parsed - 1];
    return true;
  }

  /// <summary>
  ///   Parses a roman numeral string and returns the corresponding scale degree number (1-7).
  /// </summary>
  /// <param name="s">The roman numeral string.</param>
  /// <returns>The corresponding scale degree number (1-7), or -1 if invalid.</returns>
  private static int ParseRomanNumeral(
    ReadOnlySpan<char> s )
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

    static char ToUpperAscii(
      char c )
    {
      return (char) ( c & ~0x20 );
    }
  }

  /// <summary>Attempts to parse the supplied value as a Nashville number scale degree.</summary>
  /// <param name="value">The candidate value.</param>
  /// <param name="scaleDegree">The parsed scale degree, if successful.</param>
  /// <returns>True when the parse succeeds; otherwise, false.</returns>
  private static bool TryParseNashville(
    ReadOnlySpan<char> value,
    out ScaleDegree scaleDegree )
  {
    var trimmed = value.Trim();

    if( trimmed.IsEmpty || !int.TryParse( trimmed, out var parsed ) || parsed is < 1 or > 7 )
    {
      scaleDegree = default;
      return false;
    }

    scaleDegree = ScaleDegrees[parsed - 1];
    return true;
  }

  /// <summary>
  ///   Gets the diatonic triad quality for the scale degree in the specified scaleDefinition.
  /// </summary>
  /// <param name="scaleDefinition">The scaleDefinition to use for determining the triad quality.</param>
  /// <returns>The diatonic triad quality for the scale degree in the specified scaleDefinition.</returns>
  /// <exception cref="ArgumentOutOfRangeException">Thrown when the scaleDefinition or degree is invalid.</exception>
  private static TriadQuality ClassifyTriadQuality(
    Interval thirdInterval,
    Interval fifthInterval )
  {
    // Determine quality primarily from the fifth, then third. This covers diminished and augmented fifths.
    if( fifthInterval == Interval.DiminishedFifth )
    {
      return TriadQuality.Diminished;
    }

    if( fifthInterval == Interval.AugmentedFifth )
    {
      return TriadQuality.Augmented;
    }

    // Perfect/normal fifth -> use third to decide major/minor
    if( thirdInterval == Interval.MajorThird )
    {
      return TriadQuality.Major;
    }

    if( thirdInterval == Interval.MinorThird )
    {
      return TriadQuality.Minor;
    }

    // Fallback: if we reach here, the triad structure is unsupported by the enum.
    throw new InvalidOperationException( $"Unsupported triad intervals: third={thirdInterval}, fifth={fifthInterval}." );
  }

  private static void ValidateTargetDegree(
    ScaleDegree targetDegree )
  {
    if( targetDegree.Degree is < 1 or > 7 )
    {
      throw new ArgumentOutOfRangeException( nameof( targetDegree ), "The target degree must be between 1 and 7." );
    }
  }

  #endregion
}
