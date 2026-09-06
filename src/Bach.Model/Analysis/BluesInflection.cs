// Module Name: BluesInflection.cs
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
///   Represents a blues inflection identified in the music.
/// </summary>
public sealed record BluesInflection
{
  #region Constructors

  /// <summary>
  ///   Initializes a blues inflection analysis record.
  /// </summary>
  public BluesInflection(
    AnalysisTarget target,
    InterpretationKinds.BluesInflectionKind kind,
    DegreeAlteration alteration,
    EvidenceReason evidence )
  {
    Target = target ?? throw new ArgumentNullException( nameof( target ) );
    Kind = kind;
    Alteration = alteration;
    Evidence = evidence ?? throw new ArgumentNullException( nameof( evidence ) );
  }

  #endregion

  #region Properties

  /// <summary>The event or pitch identified as the blues-inflected tone.</summary>
  public AnalysisTarget Target { get; }

  /// <summary>The type of blues inflection.</summary>
  public InterpretationKinds.BluesInflectionKind Kind { get; }

  /// <summary>The spelled alteration representing the blues inflection.</summary>
  public DegreeAlteration Alteration { get; }

  /// <summary>The reason that supports the classification.</summary>
  public EvidenceReason Evidence { get; }

  #endregion
}
