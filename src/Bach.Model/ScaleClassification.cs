// Module Name: ScaleClassification.cs
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
using System.Linq;
using Bach.Model.Internal;

namespace Bach.Model;

/// <summary>
///   Classification metadata for a scale formula.
/// </summary>
public sealed record ScaleClassification
{
  #region Constructors

  /// <summary>
  ///   Initializes a new instance of the <see cref="ScaleClassification"/> class.
  /// </summary>
  internal ScaleClassification(
    IReadOnlyList<ScaleDegreeStep> ascendingDegrees,
    IEnumerable<ScaleCategory> categories,
    IEnumerable<ScaleTag> repertoireTags,
    string? parentScaleId = null,
    int? modalRotationIndex = null,
    bool isKeyCandidate = false )
  {
    ArgumentNullException.ThrowIfNull( ascendingDegrees );
    ArgumentOutOfRangeException.ThrowIfNegativeOrZero( ascendingDegrees.Count );
    ArgumentNullException.ThrowIfNull( categories );
    ArgumentNullException.ThrowIfNull( repertoireTags );

    Cardinality = ascendingDegrees.Count;
    Categories = categories.ToFrozenSet();
    RepertoireTags = repertoireTags.ToFrozenSet();
    ParentScaleId = parentScaleId;
    ModalRotationIndex = modalRotationIndex;

    CanonicalDegrees = ascendingDegrees.Select( step => FormatCanonicalDegree( step.Interval ) )
                                       .ToArray();
    IsKeyCandidate = isKeyCandidate;
    return;

    static string FormatCanonicalDegree(
      Interval interval )
    {
      var accidental = interval.ChromaticAlteration switch
      {
        > 0 => new string( Constants.AsciiSharpAccidentalSymbol, interval.ChromaticAlteration ),
        < 0 => new string( Constants.AsciiFlatAccidentalSymbol, -interval.ChromaticAlteration ),
        _   => string.Empty
      };

      return $"{accidental}{(int) interval.Quantity}";
    }
  }

  #endregion

  #region Properties

  /// <summary>
  ///   Gets the canonical degree spelling.
  /// </summary>
  public IReadOnlyList<string> CanonicalDegrees { get; }

  /// <summary>
  ///   Gets a value indicating whether the formula is a key candidate.
  /// </summary>
  public bool IsKeyCandidate { get; }

  /// <summary>
  ///   Gets the number of degrees in the scale formula.
  /// </summary>
  public int Cardinality { get; }

  /// <summary>
  ///   Gets the identifier of the parent scale formula.
  /// </summary>
  public string? ParentScaleId { get; }

  /// <summary>
  ///   Gets the zero-based modal rotation index.
  /// </summary>
  public int? ModalRotationIndex { get; }

  /// <summary>
  ///   Gets the calculated structural categories.
  /// </summary>
  public IReadOnlySet<ScaleCategory> Categories { get; }

  /// <summary>
  ///   Gets the registry-supplied repertoire tags.
  /// </summary>
  public IReadOnlySet<ScaleTag> RepertoireTags { get; }

  #endregion
}
