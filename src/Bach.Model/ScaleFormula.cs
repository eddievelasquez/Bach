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

namespace Bach.Model;

/// <summary>
///   A scale formula defines how the pitchClasses of a scale relate to each other.
/// </summary>
public class ScaleFormula: Formula
{
  #region Constructors

  /// <summary>
  ///   Initializes a new instance of the <see cref="ScaleFormula"/> class with the specified parameters.
  /// </summary>
  /// <param name="id">The unique identifier of the scale formula.</param>
  /// <param name="name">The name of the scale formula.</param>
  /// <param name="ascendingDegrees">The ascending degrees of the scale formula.</param>
  /// <param name="descendingDegrees">The descending degrees of the scale formula.</param>
  /// <param name="categories">The categories of the scale formula.</param>
  /// <param name="aliases">The aliases of the scale formula.</param>
  internal ScaleFormula(
    string id,
    string name,
    IReadOnlyList<ScaleDegreeStep> ascendingDegrees,
    IReadOnlyList<ScaleDegreeStep> descendingDegrees,
    IEnumerable<string> categories,
    IEnumerable<string> aliases )
    : base( id, name, [.. ascendingDegrees.Select( d => d.Interval )] )
  {
    Debug.Assert( categories != null );
    Debug.Assert( aliases != null );

    AscendingDegrees = ascendingDegrees.ToArray();
    DescendingDegrees = descendingDegrees.ToArray();

    Categories = categories.ToFrozenSet( StringComparer.OrdinalIgnoreCase );
    Aliases = aliases.ToFrozenSet( StringComparer.OrdinalIgnoreCase );
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
  ///   Gets the optional categories for this scale formula.
  /// </summary>
  public IReadOnlySet<string> Categories { get; }

  /// <summary>
  ///   Gets the optional aliases for this scale formula.
  /// </summary>
  public IReadOnlySet<string> Aliases { get; }

  /// <summary>
  ///   Gets a value indicating whether this instance is diatonic.
  /// </summary>
  /// <value><c>true</c> if this instance is diatonic, <c>false</c> if not.</value>
  public bool IsDiatonic => Categories.Contains( ScaleCategory.Diatonic );

  /// <summary>
  ///   Query if this instance is a major scale formula.
  /// </summary>
  /// <value><c>true</c> if major; otherwise, <c>false</c>.</value>
  public bool IsMajor => Categories.Contains( ScaleCategory.Major );

  /// <summary>
  ///   Query if this instance is a minor scale formula.
  /// </summary>
  /// <value><c>true</c> if minor; otherwise, <c>false</c>.</value>
  public bool IsMinor => Categories.Contains( ScaleCategory.Minor );

  #endregion

  #region Public Methods

  /// <summary>
  ///   Gets the semitone separations between consecutive ascending degrees.
  /// </summary>
  /// <returns>An enumerable of semitone steps.</returns>
  public IEnumerable<int> GetSemitoneSteps()
  {
    return AscendingDegrees.Select( degree => degree.Interval )
                           .GetSemitoneSteps();
  }

  #endregion
}
