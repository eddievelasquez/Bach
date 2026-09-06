// Module Name: RootlessJazzVoicing.cs
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

namespace Bach.Model.Analysis;

/// <summary>
///   Represents an observed rootless jazz voicing with an implied root hypothesis.
/// </summary>
public sealed record RootlessJazzVoicing
{
  #region Constructors

  /// <summary>
  ///   Initializes a rootless jazz voicing analysis record.
  /// </summary>
  /// <param name="observedPitchClasses">Ordered observed pitch classes in the voicing.</param>
  /// <param name="observedBass">Optional observed bass pitch class if present in the event.</param>
  /// <param name="impliedRoot">The implied root pitch class for the voicing.</param>
  /// <param name="confidence">Confidence in the implied root, range 0.0 to 1.0.</param>
  /// <param name="evidence">The reason that supports the implied-root hypothesis.</param>
  public RootlessJazzVoicing(
    IEnumerable<PitchClass> observedPitchClasses,
    PitchClass? observedBass,
    PitchClass impliedRoot,
    double confidence,
    EvidenceReason evidence )
  {
    if( observedPitchClasses is null )
    {
      throw new ArgumentNullException( nameof( observedPitchClasses ) );
    }

    var list = observedPitchClasses.ToArray();

    if( list.Length == 0 )
    {
      throw new ArgumentException(
        "The voicing must contain at least one observed pitch class.",
        nameof( observedPitchClasses )
      );
    }

    // PitchClass is a value type with a fixed set of canonical values; no extra validity check is required here.
    if( confidence < 0.0 || confidence > 1.0 )
    {
      throw new ArgumentOutOfRangeException( nameof( confidence ), "Confidence must be between 0.0 and 1.0." );
    }

    ObservedPitchClasses = Array.AsReadOnly( list );
    ObservedBass = observedBass;
    ImpliedRoot = impliedRoot;
    Confidence = confidence;
    Evidence = evidence ?? throw new ArgumentNullException( nameof( evidence ) );
  }

  #endregion

  #region Properties

  /// <summary>Ordered observed pitch classes in the voicing.</summary>
  public IReadOnlyList<PitchClass> ObservedPitchClasses { get; }

  /// <summary>Optional observed bass pitch class when available.</summary>
  public PitchClass? ObservedBass { get; }

  /// <summary>The implied root pitch class (analytical hypothesis).</summary>
  public PitchClass ImpliedRoot { get; }

  /// <summary>Confidence in the implied root in the range 0.0 to 1.0.</summary>
  public double Confidence { get; }

  /// <summary>The reason that supports the implied-root hypothesis.</summary>
  public EvidenceReason Evidence { get; }

  #endregion
}
