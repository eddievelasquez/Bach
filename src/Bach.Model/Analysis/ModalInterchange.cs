// Module Name: ModalInterchange.cs
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
///   Represents a modal-interchange (borrowed harmony) instance.
/// </summary>
public sealed record ModalInterchange
{
  #region Constructors

  /// <summary>
  ///   Initializes a modal interchange analysis record.
  /// </summary>
  /// <param name="target">The event or pitch affected by modal interchange.</param>
  /// <param name="sourceScaleId">The registry ID of the borrowed scale or mode.</param>
  /// <param name="targetDegree">The spelled degree in the local governing scale that is affected.</param>
  /// <param name="evidence">The reason that supports the classification.</param>
  public ModalInterchange(
    AnalysisTarget target,
    string sourceScaleId,
    ScaleDegree targetDegree,
    EvidenceReason evidence )
  {
    Target = target ?? throw new ArgumentNullException( nameof( target ) );
    ArgumentException.ThrowIfNullOrWhiteSpace( sourceScaleId );
    SourceScaleId = sourceScaleId;
    TargetDegree = targetDegree;
    Evidence = evidence ?? throw new ArgumentNullException( nameof( evidence ) );
  }

  #endregion

  #region Properties

  /// <summary>The event or pitch affected by modal interchange.</summary>
  public AnalysisTarget Target { get; }

  /// <summary>The registry ID of the borrowed scale or mode.</summary>
  public string SourceScaleId { get; }

  /// <summary>The spelled degree in the local governing scale that is affected.</summary>
  public ScaleDegree TargetDegree { get; }

  /// <summary>The reason that supports the classification.</summary>
  public EvidenceReason Evidence { get; }

  #endregion
}
