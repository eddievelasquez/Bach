// Module Name: EvidenceReasonCategory.cs
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
///   Identifies the source of evidence used by a tonal analysis item.
/// </summary>
public enum EvidenceReasonCategory
{
  /// <summary>Evidence from the surrounding chord or chord progression.</summary>
  HarmonicContext,

  /// <summary>Evidence from the melodic motion around the analyzed item.</summary>
  MelodicMotion,

  /// <summary>Evidence from the item resolving to another pitch or degree.</summary>
  Resolution,

  /// <summary>Evidence from the governing scale, mode, or key context.</summary>
  ScaleContext,

  /// <summary>
  ///   Evidence that supports a candidate as the tonal center (recurrence, emphasis, or motion).
  /// </summary>
  TonalCenter,

  /// <summary>Evidence that the first event emphasizes the tonic.</summary>
  TonalCenterOpeningEmphasis,

  /// <summary>Evidence that the last event emphasizes the tonic.</summary>
  TonalCenterClosingEmphasis,

  /// <summary>Evidence for dominant-to-tonic motion in the ordered event sequence.</summary>
  DominantToTonicMotion,

  /// <summary>Evidence for leading-tone resolution to tonic in the ordered event sequence.</summary>
  LeadingToneResolution,

  /// <summary>Evidence entered as a direct analyst observation.</summary>
  AnalystObservation
}
