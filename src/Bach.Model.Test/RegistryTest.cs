// Module Name: RegistryTest.cs
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

namespace Bach.Model.Test;

using System.Linq;
using Bach.Model.Instruments;

public sealed class RegistryTest
{
  #region Public Methods

  [Fact]
  public void ChordFormulas_ShouldReturnExpectedValues_WhenAccessedById()
  {
    var chordFormulas = Registry.ChordFormulas.ToArray();

    chordFormulas.Should()
                 .NotBeNull();

    chordFormulas.Should()
                 .NotBeEmpty();

    foreach( var expected in chordFormulas )
    {
      var actual = Registry.ChordFormulas[expected.Id];

      actual.Should()
            .Be( expected );
    }
  }

  [Fact]
  public void ScaleFormulas_ShouldReturnExpectedValues_WhenAccessedById()
  {
    var scaleFormulas = Registry.ScaleFormulas.ToArray();

    scaleFormulas.Should()
                 .NotBeNull();

    scaleFormulas.Should()
                 .NotBeEmpty();

    foreach( var expected in scaleFormulas )
    {
      var actual = Registry.ScaleFormulas[expected.Id];

      actual.Should()
            .Be( expected );
    }
  }

  [Fact]
  public void StringedInstrumentDefinitions_ShouldReturnExpectedValues_WhenAccessedById()
  {
    var instrumentDefinitions = Registry.StringedInstrumentDefinitions.ToArray();

    instrumentDefinitions.Should()
                         .NotBeNull();

    instrumentDefinitions.Should()
                         .NotBeEmpty();

    foreach( var expected in instrumentDefinitions )
    {
      InstrumentDefinition actual = Registry.StringedInstrumentDefinitions[expected.Id];

      actual.Should()
            .Be( expected );
    }
  }

  [Fact]
  public void TryGetChordFormula_ShouldReturnFalse_WhenChordFormulaDoesNotExist()
  {
    var result = Registry.TryGetChordFormula( "NonExistentChord", out var formula );

    result.Should()
          .BeFalse();

    formula.Should()
           .BeNull();
  }

  [Fact]
  public void TryGetChordFormula_ShouldReturnTrue_WhenChordFormulaExistsById()
  {
    var result = Registry.TryGetChordFormula( "Major", out var formula );

    result.Should()
          .BeTrue();

    formula.Should()
           .NotBeNull();

    formula!.Id.Should()
            .Be( "Major" );
  }

  [Fact]
  public void TryGetChordFormula_ShouldReturnTrue_WhenChordFormulaExistsByName()
  {
    var result = Registry.TryGetChordFormula( "Major", out var formula );

    result.Should()
          .BeTrue();

    formula.Should()
           .NotBeNull();

    formula!.Name.Should()
            .Be( "Major" );
  }

  [Fact]
  public void TryGetScaleFormula_ShouldReturnFalse_WhenScaleFormulaDoesNotExist()
  {
    var result = Registry.TryGetScaleFormula( "NonExistentScale", out var formula );

    result.Should()
          .BeFalse();

    formula.Should()
           .BeNull();
  }

  [Fact]
  public void TryGetScaleFormula_ShouldReturnTrue_WhenScaleFormulaExistsById()
  {
    var result = Registry.TryGetScaleFormula( "Major", out var formula );

    result.Should()
          .BeTrue();

    formula.Should()
           .NotBeNull();

    formula!.Id.Should()
            .Be( "Major" );
  }

  [Fact]
  public void TryGetScaleFormula_ShouldReturnTrue_WhenScaleFormulaExistsByName()
  {
    var result = Registry.TryGetScaleFormula( "Natural Minor", out var formula );

    result.Should()
          .BeTrue();

    formula.Should()
           .NotBeNull();

    formula!.Name.Should()
            .Be( "Natural Minor" );
  }

  [Fact]
  public void TryGetStringedInstrumentDefinition_ShouldReturnFalse_WhenInstrumentDoesNotExist()
  {
    var result = Registry.TryGetStringedInstrumentDefinition( "NonExistentInstrument", out var definition );

    result.Should()
          .BeFalse();

    definition.Should()
              .BeNull();
  }

  [Fact]
  public void TryGetStringedInstrumentDefinition_ShouldReturnTrue_WhenInstrumentExistsById()
  {
    var result = Registry.TryGetStringedInstrumentDefinition( "Guitar", out var definition );

    result.Should()
          .BeTrue();

    definition.Should()
              .NotBeNull();

    definition!.Id.Should()
               .Be( "Guitar" );
  }

  [Fact]
  public void TryGetStringedInstrumentDefinition_ShouldReturnTrue_WhenInstrumentExistsByName()
  {
    var result = Registry.TryGetStringedInstrumentDefinition( "Guitar", out var definition );

    result.Should()
          .BeTrue();

    definition.Should()
              .NotBeNull();

    definition!.Name.Should()
               .Be( "Guitar" );
  }

  #endregion
}
