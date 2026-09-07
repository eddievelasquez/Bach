// Module Name: TonalCandidateSource.cs
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
///   Discovers candidate keys for tonal evaluation using registry metadata and optional caller-supplied additions.
/// </summary>
public static class TonalCandidateSource
{
  #region Constants

  private static readonly PitchClass[] s_supportedTonics =
  [
    PitchClass.C, PitchClass.CSharp, PitchClass.DFlat,
    PitchClass.D, PitchClass.DSharp, PitchClass.EFlat,
    PitchClass.E, PitchClass.F, PitchClass.FSharp,
    PitchClass.GFlat, PitchClass.G, PitchClass.GSharp,
    PitchClass.AFlat, PitchClass.A, PitchClass.ASharp,
    PitchClass.BFlat, PitchClass.B
  ];

  #endregion

  #region Public Methods

  /// <summary>
  ///   Enumerates the default candidate keys discovered from the registry. Only formulas marked as key
  ///   candidates are considered. When a profile is supplied, formulas without matching repertoire tags
  ///   are excluded.
  /// </summary>
  public static IEnumerable<Key> GetDefaultCandidates(
    RepertoireProfile? profile = null )
  {
    return Registry.ScaleFormulas.Where( formula => formula.Classification.IsKeyCandidate )
                   .Where( formula => profile is null
                                      || formula.Classification.RepertoireTags.Count == 0
                                      || formula.Classification.RepertoireTags.Any( t => profile.EnabledTags.Contains( t ) )
                   )
                   .SelectMany(
                     _ => s_supportedTonics,
                     (
                       formula,
                       tonic ) => new Key( tonic, ScaleDefinition.FromFormulaId( formula.Id ) )
                   );
  }

  #endregion
}
