// Module Name: AlteredDegree.cs
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

namespace Bach.Model.Analysis;

/// <summary>
///   Represents a spelled scale degree used outside the governing scale or mode.
/// </summary>
public sealed record AlteredDegree
{
  #region Constructors

  /// <summary>
  ///   Initializes an altered degree.
  /// </summary>
  /// <param name="target">The event or pitch identified as the altered degree.</param>
  /// <param name="alteration">The spelled alteration relative to the governing key.</param>
  /// <param name="kind">The type of altered-degree use.</param>
  /// <param name="evidence">The reason that supports the classification.</param>
  public AlteredDegree(
    AnalysisTarget target,
    DegreeAlteration alteration,
    AlteredDegreeKind kind,
    EvidenceReason evidence )
  {
    Target = target ?? throw new ArgumentNullException( nameof( target ) );
    Evidence = evidence ?? throw new ArgumentNullException( nameof( evidence ) );

    Alteration = alteration;
    Kind = kind;
  }

  #endregion

  #region Properties

  /// <summary>
  ///   Gets the event or pitch identified as the altered degree.
  /// </summary>
  public AnalysisTarget Target { get; }

  /// <summary>
  ///   Gets the spelled alteration relative to the governing key.
  /// </summary>
  public DegreeAlteration Alteration { get; }

  /// <summary>
  ///   Gets the type of altered-degree use.
  /// </summary>
  public AlteredDegreeKind Kind { get; }

  /// <summary>
  ///   Gets the reason that supports the classification.
  /// </summary>
  public EvidenceReason Evidence { get; }

  #endregion
}
