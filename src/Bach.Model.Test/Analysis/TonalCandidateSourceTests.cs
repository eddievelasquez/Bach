// Module Name: TonalCandidateSourceTests.cs
// Project:     Bach.Model.Test
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

using System.Linq;

namespace Bach.Model.Analysis.Test;

public sealed class TonalCandidateSourceTests
{
  #region Public Methods

  [Fact]
  public void GetDefaultCandidates_ShouldReturnOnlyKeyCandidateFormulas()
  {
    var candidates = TonalCandidateSource.GetDefaultCandidates()
                                         .ToArray();

    candidates.Should()
              .NotBeEmpty();

    candidates.Should()
              .OnlyContain( candidate => candidate.Scale.Formula.Classification.IsKeyCandidate );
  }

  [Fact]
  public void GetDefaultCandidates_ShouldReturnAllSupportedSpellingsForMajor()
  {
    var candidates = TonalCandidateSource.GetDefaultCandidates()
                                         .Where( candidate => candidate.ScaleDefinition.FormulaId
                                                              == ScaleDefinition.Major.FormulaId
                                         )
                                         .ToArray();

    candidates.Should()
              .HaveCount( 17 );

    candidates.Select( candidate => candidate.Tonic )
              .Should()
              .Contain(
                [
                  PitchClass.C, PitchClass.CSharp, PitchClass.DFlat,
                  PitchClass.D, PitchClass.DSharp, PitchClass.EFlat,
                  PitchClass.E, PitchClass.F, PitchClass.FSharp,
                  PitchClass.GFlat, PitchClass.G, PitchClass.GSharp,
                  PitchClass.AFlat, PitchClass.A, PitchClass.ASharp,
                  PitchClass.BFlat, PitchClass.B
                ]
              );
  }

  [Fact]
  public void GetDefaultCandidates_ShouldFilterRepertoireTags()
  {
    var candidates = TonalCandidateSource.GetDefaultCandidates( RepertoireProfile.Modal )
                                         .ToArray();

    candidates.Should()
              .NotBeEmpty();

    candidates.Should()
              .OnlyContain( candidate => candidate.Scale.Formula.Classification.RepertoireTags.Count == 0
                                         || candidate.Scale.Formula.Classification.RepertoireTags.Any( tag =>
                                           RepertoireProfile.Modal.EnabledTags.Contains( tag )
                                         )
              );
  }

  [Fact]
  public void GetDefaultCandidates_ShouldPreserveEnharmonicallyDistinctTonics()
  {
    var candidates = TonalCandidateSource.GetDefaultCandidates()
                                         .Where( candidate => candidate.ScaleDefinition.FormulaId
                                                              == ScaleDefinition.Major.FormulaId
                                         )
                                         .ToArray();

    candidates.Should()
              .Contain( candidate => candidate.Tonic == PitchClass.CSharp );

    candidates.Should()
              .Contain( candidate => candidate.Tonic == PitchClass.DFlat );

    candidates.Select( candidate => candidate.Tonic )
              .Distinct()
              .Should()
              .HaveCount( 17 );
  }

  #endregion
}
