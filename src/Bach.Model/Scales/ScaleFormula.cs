// Module Name: ScaleFormula.cs
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

using System.Collections.Frozen;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Bach.Model.Internal;

namespace Bach.Model.Scales;

/// <summary>
///   A scale formula defines how the pitchClasses of a scale relate to each other.
/// </summary>
public class ScaleFormula: Formula
{
  #region Fields

  private readonly Lazy<IReadOnlySet<string>> _categories;

  #endregion

  #region Constructors

  /// <summary>
  ///   Initializes a new instance of the <see cref="ScaleFormula"/> class with the specified parameters.
  /// </summary>
  /// <param name="id">
  ///   The unique identifier of the scale formula.
  /// </param>
  /// <param name="name">
  ///   The name of the scale formula.
  /// </param>
  /// <param name="ascendingDegrees">
  ///   The ascending degrees of the scale formula.
  /// </param>
  /// <param name="descendingDegrees">
  ///   The descending degrees of the scale formula.
  /// </param>
  /// <param name="classification">
  ///   The classification of the scale formula.
  /// </param>
  /// <param name="aliases">
  ///   The aliases of the scale formula.
  /// </param>
  internal ScaleFormula(
    string id,
    string name,
    IReadOnlyList<ScaleDegreeStep> ascendingDegrees,
    IReadOnlyList<ScaleDegreeStep> descendingDegrees,
    ScaleClassification classification,
    IEnumerable<string> aliases )
    : base( id, name, [.. ascendingDegrees.Select( d => d.Interval )] )
  {
    Debug.Assert( classification != null );
    Debug.Assert( aliases != null );

    ValidateIntervalCounts( ascendingDegrees, name, nameof( ascendingDegrees ) );
    ValidateIntervalCounts( descendingDegrees, name, nameof( descendingDegrees ) );

    AscendingDegrees = ascendingDegrees.ToArray();
    DescendingDegrees = descendingDegrees.ToArray();
    Classification = classification;
    Aliases = aliases.ToFrozenSet( StringComparer.OrdinalIgnoreCase );

    _categories = new Lazy<IReadOnlySet<string>>( () => classification.Categories.Select( category => category.ToString() )
                                                                      .Concat(
                                                                        classification.RepertoireTags
                                                                          .Select( tag => tag.ToString() )
                                                                      )
                                                                      .ToFrozenSet( StringComparer.OrdinalIgnoreCase )
    );

    return;

    static void ValidateIntervalCounts(
      IReadOnlyList<ScaleDegreeStep> intervals,
      string name,
      string paramName )
    {
      if( intervals.Count < Constants.MinimumScaleIntervalCount || intervals.Count > Constants.MaximumScaleIntervalCount )
      {
        throw new ArgumentOutOfRangeException(
          paramName,
          $"{name}: A scale must contain between {Constants.MinimumScaleIntervalCount} and {Constants.MaximumScaleIntervalCount} intervals"
        );
      }
    }
  }

  #endregion

  #region Properties

  /// <summary>
  ///   Gets the authoritative ascending tonic-relative degrees.
  /// </summary>
  public IReadOnlyList<ScaleDegreeStep> AscendingDegrees { get; }

  /// <summary>
  ///   Gets the authoritative descending tonic-relative degrees.
  /// </summary>
  public IReadOnlyList<ScaleDegreeStep> DescendingDegrees { get; }

  /// <summary>
  ///   Gets the classification of this scale formula.
  /// </summary>
  public ScaleClassification Classification { get; }

  /// <summary>
  ///   Gets the calculated and repertoire categories for this scale formula.
  /// </summary>
  public IReadOnlySet<string> Categories => _categories.Value;

  /// <summary>
  ///   Gets the optional aliases for this scale formula.
  /// </summary>
  public IReadOnlySet<string> Aliases { get; }

  /// <summary>
  ///   Gets a value indicating whether this instance is diatonic.
  /// </summary>
  /// <value>
  ///   <c>true</c> if this instance is diatonic,   <c>false</c> if not.
  /// </value>
  public bool IsDiatonic => Classification.Categories.Contains( ScaleCategory.Diatonic );

  /// <summary>
  ///   Query if this instance is a major scale formula.
  /// </summary>
  /// <value>
  ///   <c>true</c> if this instance is major; otherwise, <c>false</c>.
  /// </value>
  public bool IsMajor => Classification.Categories.Contains( ScaleCategory.Major );

  /// <summary>
  ///   Query if this instance is a minor scale formula.
  /// </summary>
  /// <value>
  ///   <c>true</c> if this instance is minor; otherwise, <c>false</c>.
  /// </value>
  public bool IsMinor => Classification.Categories.Contains( ScaleCategory.Minor );

  #endregion

  #region Public Methods

  /// <summary>
  ///   Gets the semitone separations between consecutive ascending degrees.
  /// </summary>
  /// <returns>
  ///   An int enumerable of semitone steps.
  /// </returns>
  public IEnumerable<int> GetSemitoneSteps()
  {
    return AscendingDegrees.Select( degree => degree.Interval )
                           .GetSemitoneSteps();
  }

  #endregion
}
