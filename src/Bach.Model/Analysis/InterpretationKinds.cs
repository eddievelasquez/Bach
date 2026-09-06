// Module Name: InterpretationKinds.cs
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
///   Common interpretation enums used by the analysis records.
/// </summary>
public static class InterpretationKinds
{
  #region Nested Types

  /// <summary>Blues inflection kinds.</summary>
  public enum BluesInflectionKind
  {
    /// <summary>
    ///   The blue third is a lowered third scale degree, often used in blues and jazz music to create a "bluesy" sound.
    /// </summary>
    BlueThird,

    /// <summary>
    ///   The blue fifth is a lowered fifth scale degree, also known as the diminished fifth or tritone, which is commonly
    ///   used in blues and jazz music to create tension and dissonance.
    /// </summary>
    BlueFifth,

    /// <summary>
    ///   The blue seventh is a lowered seventh scale degree, often used in blues and jazz music to create a "bluesy" sound.
    /// </summary>
    BlueSeventh
  }

  /// <summary>Cadence kinds.</summary>
  public enum CadenceKind
  {
    /// <summary>
    ///   An authentic cadence is a musical cadence that consists of a dominant chord (V) followed by a tonic chord (I),
    ///   creating a sense of resolution and finality.
    /// </summary>
    Authentic,

    /// <summary>
    ///   A plagal cadence is a musical cadence that consists of a subdominant chord (IV) followed by a tonic chord (I),
    /// </summary>
    Plagal,

    /// <summary>
    ///   A half cadence is a musical cadence that ends on a dominant chord (V), creating a sense of pause or suspension
    ///   rather than resolution.
    /// </summary>
    Half,

    /// <summary>
    ///   A deceptive cadence is a musical cadence that consists of a dominant chord (V) followed by a chord other than
    ///   the tonic (I),
    /// </summary>
    Deceptive,

    /// <summary>
    ///   A Phrygian half cadence is a musical cadence that occurs in the Phrygian mode and consists of a dominant chord
    ///   (V)
    /// </summary>
    PhrygianHalf
  }

  #endregion
}
