// Module Name: ChordProgression.cs
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
using System.Linq;
using Bach.Model.Internal;

/// <summary>Represents a sequence of chords forming a progression.</summary>
public sealed class ChordProgression: IFormattable, IParsable<ChordProgression>
{
  #region Constructors

  /// <summary>
  ///   Initializes a new instance of the <see cref="ChordProgression" /> class with the specified scale degrees.
  /// </summary>
  /// <param name="scaleDegrees">The scale degrees for the chord progression.</param>
  public ChordProgression( params ScaleDegree[] scaleDegrees )
  {
    ArgumentException.ThrowIfNullOrEmpty(scaleDegrees);

    ScaleDegrees = scaleDegrees.ToList();
  }

  #endregion

  #region Properties

  /// <summary>Gets the scale degrees in the progression.</summary>
  public IReadOnlyList<ScaleDegree> ScaleDegrees { get; }

  #endregion

  #region Public Methods

  /// <inheritdoc />
  public override string ToString()
  {
    return ToString( null, null );
  }

  /// <summary>
  ///   Formats the chord progression as a string based on the roman numeral format and format provider.
  /// </summary>
  /// <param name="formatProvider">The format provider.</param>
  /// <returns>A string representation of the chord progression.</returns>
  public string ToString(
    IFormatProvider? formatProvider )
  {
    return ToString( null, formatProvider );
  }

  /// <summary>
  ///   Formats the chord progression as a string based on the specified format and the default format provider.
  /// </summary>
  /// <param name="format">The format string.</param>
  /// <returns>A string representation of the chord progression.</returns>
  /// <remarks>
  ///   The supported format strings are:
  ///   <list type="bullet">
  ///     <item>
  ///       <description>
  ///         "G" - General format, displays the
  ///         symbols of the scale degrees.
  ///       </description>
  ///     </item>
  ///     <item>
  ///       <description>
  ///         "R" - Roman numeral format, displays the
  ///         symbols of the scale degrees.
  ///       </description>
  ///     </item>
  ///     <item>
  ///       <description>
  ///         "N" - Nashville Numeric format, displays the
  ///         numeric values of the scale degrees.
  ///       </description>
  ///     </item>
  ///   </list>
  /// </remarks>
  public string ToString(
    string? format )
  {
    return ToString( format, null );
  }

  /// <summary>
  ///   Formats the chord progression as a string based on the specified format and format provider.
  /// </summary>
  /// <param name="format">The format string.</param>
  /// <param name="formatProvider">The format provider.</param>
  /// <returns>A string representation of the chord progression.</returns>
  /// <exception cref="FormatException">Thrown when the format string is not supported.</exception>
  /// <remarks>
  ///   The supported format strings are:
  ///   <list type="bullet">
  ///     <item>
  ///       <description>
  ///         "G" - General format, displays the
  ///         symbols of the scale degrees.
  ///       </description>
  ///     </item>
  ///     <item>
  ///       <description>
  ///         "R" - Roman numeral format, displays the
  ///         symbols of the scale degrees.
  ///       </description>
  ///     </item>
  ///     <item>
  ///       <description>
  ///         "N" - Nashville Numeric format, displays the
  ///         numeric values of the scale degrees.
  ///       </description>
  ///     </item>
  ///   </list>
  /// </remarks>
  public string ToString(
    string? format,
    IFormatProvider? formatProvider )
  {
    format ??= "G";

    return format switch
    {
      "G" or "R" => string.Join( "-", ScaleDegrees.Select( degree => degree.Symbol ) ),
      "N"        => string.Join( "-", ScaleDegrees.Select( degree => degree.Degree ) ),
      _          => throw new FormatException( $"The format string '{format}' is not supported." )
    };
  }

  /// <summary>Parses a chord progression from the specified string.</summary>
  /// <param name="value">The chord progression string.</param>
  /// <returns>A parsed chord progression.</returns>
  /// <exception cref="ArgumentException">Thrown when the string is null, empty, or whitespace.</exception>
  /// <exception cref="FormatException">Thrown when the string is not a valid chord progression.</exception>
  public static ChordProgression Parse(
    string value )
  {
    return Parse( value, null );
  }

  /// <summary>Parses a chord progression from the specified string.</summary>
  /// <param name="s">The chord progression string.</param>
  /// <param name="provider">The format provider.</param>
  /// <returns>A parsed chord progression.</returns>
  /// <exception cref="ArgumentException">Thrown when the string is null, empty, or whitespace.</exception>
  /// <exception cref="FormatException">Thrown when the string is not a valid chord progression.</exception>
  public static ChordProgression Parse(
    string s,
    IFormatProvider? provider )
  {
    ArgumentException.ThrowIfNullOrWhiteSpace( s );

    if( !TryParse( s, provider, out var progression ) )
    {
      throw new FormatException( $"The value '{s}' is not a valid chord progression." );
    }

    return progression;
  }

  /// <summary>Attempts to parse a chord progression from the specified string.</summary>
  /// <param name="value">The chord progression string.</param>
  /// <param name="progression">The parsed chord progression, if successful.</param>
  /// <returns>True when parsing succeeds; otherwise, false.</returns>
  public static bool TryParse(
    [NotNullWhen( true )] string? value,
    [NotNullWhen( true )] out ChordProgression? progression )
  {
    return TryParse( value, null, out progression );
  }

  /// <summary>Attempts to parse a chord progression from the specified string.</summary>
  /// <param name="s">The chord progression string.</param>
  /// <param name="provider">The format provider.</param>
  /// <param name="progression">The parsed chord progression, if successful.</param>
  /// <returns>True when parsing succeeds; otherwise, false.</returns>
  public static bool TryParse(
    [NotNullWhen( true )] string? s,
    IFormatProvider? provider,
    [NotNullWhen( true )] out ChordProgression? progression )
  {
    progression = null;

    if( string.IsNullOrWhiteSpace( s ) )
    {
      return false;
    }

    var parts = s.Split( ['-', ' ', '\t', '\r', '\n'], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries );
    if( parts.Length == 0 )
    {
      return false;
    }

    // Detect if the progression is in Roman numeral or Nashville format, but not both.
    var hasRoman = false;
    var hasNashville = false;

    foreach( var part in parts )
    {
      if( ScaleDegree.IsRomanNumeral( part ) )
      {
        hasRoman = true;
      }
      else if( ScaleDegree.IsNashville( part ) )
      {
        hasNashville = true;
      }
      else
      {
        return false;
      }
    }

    // Cannot combine Roman and Nashville numerals in the same progression.
    if( hasRoman == hasNashville )
    {
      return false;
    }

    var scaleDegrees = new List<ScaleDegree>( parts.Length );

    foreach( var part in parts )
    {
      if( !ScaleDegree.TryParse( part, out var scaleDegree ) )
      {
        return false;
      }

      scaleDegrees.Add( scaleDegree );
    }

    progression = new ChordProgression( scaleDegrees.ToArray() );
    return true;
  }

  #endregion
}
