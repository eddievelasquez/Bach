// Module Name: ScaleClassificationBuilder.cs
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
using System.Linq;

namespace Bach.Model;

/// <summary>
///   Creates scale classification metadata.
/// </summary>
public sealed class ScaleClassificationBuilder
{
  #region Fields

  private readonly HashSet<ScaleCategory> _categories = [];
  private readonly HashSet<ScaleTag> _repertoireTags = [];
  private IReadOnlyList<ScaleDegreeStep>? _ascendingDegrees;
  private string? _parentScaleId;
  private int? _modalRotationIndex;
  private bool _isKeyCandidate;

  #endregion

  #region Public Methods

  /// <summary>
  ///   Adds a structural category.
  /// </summary>
  /// <param name="category">
  ///   The category to add.
  /// </param>
  /// <returns>
  ///   This instance.
  /// </returns>
  public ScaleClassificationBuilder AddCategory(
    ScaleCategory category )
  {
    _categories.Add( category );
    return this;
  }

  /// <summary>
  ///   Adds structural categories.
  /// </summary>
  /// <param name="categories">
  ///   The categories to add.
  /// </param>
  /// <returns>
  ///   This instance.
  /// </returns>
  public ScaleClassificationBuilder AddCategories(
    IEnumerable<ScaleCategory> categories )
  {
    ArgumentNullException.ThrowIfNull( categories );
    _categories.UnionWith( categories );
    return this;
  }

  /// <summary>
  ///   Adds a repertoire tag.
  /// </summary>
  /// <param name="tag">
  ///   The tag to add.
  /// </param>
  /// <returns>
  ///   This instance.
  /// </returns>
  public ScaleClassificationBuilder AddRepertoireTag(
    ScaleTag tag )
  {
    _repertoireTags.Add( tag );
    return this;
  }

  /// <summary>
  ///   Adds repertoire tags.
  /// </summary>
  /// <param name="tags">
  ///   The tags to add.
  /// </param>
  /// <returns>
  ///   This instance.
  /// </returns>
  public ScaleClassificationBuilder AddRepertoireTags(
    IEnumerable<ScaleTag> tags )
  {
    ArgumentNullException.ThrowIfNull( tags );
    _repertoireTags.UnionWith( tags );
    return this;
  }

  /// <summary>
  ///   Adds repertoire tags from a semicolon-delimited string.
  /// </summary>
  /// <param name="tag">
  ///   The semicolon-delimited string of tags to add.
  /// </param>
  /// <returns>
  ///   This instance.
  /// </returns>
  public ScaleClassificationBuilder AddRepertoireTag(
    string? tag )
  {
    return AddRepertoireTags( ParseRepertoireTags( tag ) );

    static IEnumerable<ScaleTag> ParseRepertoireTags(
      string? tags )
    {
      return string.IsNullOrWhiteSpace( tags )
        ? []
        : tags.Split( ';', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries )
              .Select( tag => Enum.Parse<ScaleTag>( tag, true ) );
    }
  }

  /// <summary>
  ///   Sets the ascending degrees used to create canonical degree spellings.
  /// </summary>
  /// <param name="ascendingDegrees">
  ///   The ascending degrees.
  /// </param>
  /// <returns>
  ///   This instance.
  /// </returns>
  public ScaleClassificationBuilder SetAscendingDegrees(
    IReadOnlyList<ScaleDegreeStep>? ascendingDegrees )
  {
    _ascendingDegrees = ascendingDegrees;
    return this;
  }

  /// <summary>
  ///   Sets the parent scale formula identifier.
  /// </summary>
  /// <param name="parentScaleId">
  ///   The parent scale formula identifier.
  /// </param>
  /// <returns>
  ///   This instance.
  /// </returns>
  public ScaleClassificationBuilder SetParentScaleId(
    string? parentScaleId )
  {
    _parentScaleId = parentScaleId;
    return this;
  }

  /// <summary>
  ///   Sets the zero-based modal rotation index.
  /// </summary>
  /// <param name="modalRotationIndex">
  ///   The modal rotation index.
  /// </param>
  /// <returns>
  ///   This instance.
  /// </returns>
  public ScaleClassificationBuilder SetModalRotationIndex(
    int? modalRotationIndex )
  {
    _modalRotationIndex = modalRotationIndex;
    return this;
  }

  /// <summary>
  ///   Sets whether the formula is a key candidate.
  /// </summary>
  /// <param name="isKeyCandidate">
  ///   The key-candidate value.
  /// </param>
  /// <returns>
  ///   This instance.
  /// </returns>
  public ScaleClassificationBuilder SetKeyCandidate(
    bool isKeyCandidate )
  {
    _isKeyCandidate = isKeyCandidate;
    return this;
  }

  /// <summary>
  ///   Builds a scale classification.
  /// </summary>
  /// <returns>
  ///   A scale classification.
  /// </returns>
  public ScaleClassification Build()
  {
    if( _ascendingDegrees is null || _ascendingDegrees.Count == 0 )
    {
      throw new InvalidOperationException( "Ascending degrees must be set before building a scale classification." );
    }

    return new ScaleClassification(
      _ascendingDegrees,
      _categories,
      _repertoireTags,
      _parentScaleId,
      _modalRotationIndex,
      _isKeyCandidate
    );
  }

  #endregion
}
