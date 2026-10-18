// Module Name: ChordFormula.cs
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

namespace Bach.Model.Harmony;

/// <summary>
///   Defines a chord by the intervals from its root to each chord tone. The formula also provides
///   a display name and symbol used when naming or parsing chords.
/// </summary>
public sealed class ChordFormula: Formula
{
  #region Constructors

  /// <summary>Creates a chord formula from its ID, name, symbol, and intervals.</summary>
  /// <param name="id">The language-neutral id of the chord.</param>
  /// <param name="name">The localizable name of the chord.</param>
  /// <param name="symbol">The symbol for the chord.</param>
  /// <param name="intervals">
  ///   The intervals from the root to each chord tone.
  /// </param>
  public ChordFormula(
    string id,
    string name,
    string? symbol,
    params Interval[] intervals )
    : base( id, name, intervals )
  {
    // A chord is composed by two or more pitch classes...
    ArgumentOutOfRangeException.ThrowIfLessThan( intervals.Length, 2 );
    Symbol = symbol ?? name;
  }

  /// <summary>Creates a chord formula from a comma-separated interval string.</summary>
  /// <param name="id">The language-neutral id of the chord.</param>
  /// <param name="name">The localizable name of the chord.</param>
  /// <param name="symbol">The symbol for the chord.</param>
  /// <param name="formula">
  ///   The comma-separated intervals from the root to each chord tone. See
  ///   <see cref="Interval.ToString()"/> for the interval format.
  /// </param>
  public ChordFormula(
    string id,
    string name,
    string? symbol,
    string formula )
    : this( id, name, symbol, ParseIntervals( formula ) )
  {
  }

  #endregion

  #region Properties

  /// <summary>
  ///   Gets the chord formula for a major chord.
  /// </summary>
  public static ChordFormula Major => Registry.Instance.ChordFormulas[nameof( Major )];

  /// <summary>
  ///   Gets the chord formula for a major seventh chord.
  /// </summary>
  public static ChordFormula Major7 => Registry.Instance.ChordFormulas[nameof( Major7 )];

  /// <summary>
  ///   Gets the chord formula for a minor chord.
  /// </summary>
  public static ChordFormula Minor => Registry.Instance.ChordFormulas[nameof( Minor )];

  /// <summary>
  ///   Gets the chord formula for a minor seventh chord.
  /// </summary>
  public static ChordFormula Minor7 => Registry.Instance.ChordFormulas[nameof( Minor7 )];

  /// <summary>
  ///   Gets the chord formula for a dominant seventh chord.
  /// </summary>
  public static ChordFormula Dominant7 => Registry.Instance.ChordFormulas[nameof( Dominant7 )];

  /// <summary>
  ///   Gets the chord formula for a diminished chord.
  /// </summary>
  public static ChordFormula Diminished => Registry.Instance.ChordFormulas[nameof( Diminished )];

  /// <summary>
  ///   Gets the chord formula for a half-diminished seventh chord.
  /// </summary>
  public static ChordFormula HalfDiminished7 => Registry.Instance.ChordFormulas["HalfDiminished"];

  /// <summary>Gets the symbol for the chord.</summary>
  /// <value>The symbol.</value>
  public string Symbol { get; }

  #endregion
}
