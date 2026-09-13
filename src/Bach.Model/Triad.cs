// Module Name: Triad.cs
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

/// <summary>A triad is a chord with three pitch classes stacked in thirds.</summary>
public sealed class Triad: Chord
{
  #region Constants

  private static readonly ChordFormula s_majorTriad
    = new(
      "MajorTriad",
      "MajorTriad",
      "",
      Interval.Unison,
      Interval.MajorThird,
      Interval.Fifth
    );

  private static readonly ChordFormula s_minorTriad
    = new(
      "MinorTriad",
      "MinorTriad",
      "m",
      Interval.Unison,
      Interval.MinorThird,
      Interval.Fifth
    );

  private static readonly ChordFormula s_diminishedTriad = new(
    "DiminishedTriad",
    "DiminishedTriad",
    "dim",
    Interval.Unison,
    Interval.MinorThird,
    Interval.DiminishedFifth
  );

  private static readonly ChordFormula s_augmentedTriad = new(
    "AugmentedTriad",
    "AugmentedTriad",
    "aug",
    Interval.Unison,
    Interval.MajorThird,
    Interval.AugmentedFifth
  );

  #endregion

  #region Constructors

  /// <summary>
  ///   Constructor.
  /// </summary>
  /// <param name="root">The triad's root pitch class.</param>
  /// <param name="quality">The triad's quality.</param>
  /// <param name="inversion">The triad's inversion.</param>
  public Triad(
    PitchClass root,
    TriadQuality quality,
    int inversion = 0 )
    : base( root, GetFormula( quality ), inversion )
  {
    Quality = quality;
  }

  #endregion

  #region Properties

  /// <summary>Gets the triad's quality.</summary>
  /// <value>The quality.</value>
  public TriadQuality Quality { get; }

  #endregion

  #region Public Methods

  /// <summary>Generates an inversion for the current triad.</summary>
  /// <param name="inversion">The inversion to generate.</param>
  /// <returns>A Triad.</returns>
  public override Triad GetInversion(
    int inversion )
  {
    return new Triad( Root, Quality, inversion );
  }

  #endregion

  #region Implementation

  /// <summary>
  ///   Gets the chord formula for the specified triad quality.
  /// </summary>
  /// <param name="quality">The triad quality.</param>
  /// <returns>The chord formula.</returns>
  /// <exception cref="ArgumentOutOfRangeException">Thrown when the triad quality is not recognized.</exception>
  private static ChordFormula GetFormula(
    TriadQuality quality )
  {
    return quality switch
    {
      TriadQuality.Major      => s_majorTriad,
      TriadQuality.Minor      => s_minorTriad,
      TriadQuality.Diminished => s_diminishedTriad,
      TriadQuality.Augmented  => s_augmentedTriad,
      _                       => throw new ArgumentOutOfRangeException( nameof( quality ), quality, null )
    };
  }

  #endregion
}
