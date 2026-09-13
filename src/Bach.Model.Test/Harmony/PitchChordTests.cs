// Module Name: PitchChordTests.cs
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

namespace Bach.Model.Harmony.Test;

public sealed class PitchChordTests
{
  #region Public Methods

  [Fact]
  public void Bass_ShouldReturnCorrectPitchForSecondInversion()
  {
    var root = new Pitch( PitchClass.G, 3 );
    var chord = new PitchChord( root, ChordFormula.Major, 2 );

    var actual = chord.Bass;

    actual.Should()
          .Be( new Pitch( PitchClass.D, 4 ) );
  }

  [Fact]
  public void Bass_ShouldReturnFirstPitch_WhenInversionIsZero()
  {
    var root = new Pitch( PitchClass.C, 4 );
    var chord = new PitchChord( root, ChordFormula.Major );

    var actual = chord.Bass;

    actual.Should()
          .Be( root );
  }

  [Fact]
  public void Bass_ShouldReturnInvertedPitch_WhenInversionIsNonZero()
  {
    var root = new Pitch( PitchClass.C, 4 );
    var chord = new PitchChord( root, ChordFormula.Major, 1 );

    var actual = chord.Bass;

    actual.Should()
          .Be( new Pitch( PitchClass.E, 4 ) );
  }

  [Fact]
  public void Constructor_ShouldCreateMajorChord_WhenRootPitchProvided()
  {
    var root = new Pitch( PitchClass.C, 4 );

    var actual = new PitchChord( root, ChordFormula.Major );

    actual.Root.Should()
          .Be( root );

    actual.Formula.Should()
          .Be( ChordFormula.Major );

    actual.Inversion.Should()
          .Be( 0 );

    actual.Bass.Should()
          .Be( root );

    actual.Name.Should()
          .Be( "C" );

    actual.Should()
          .Equal( new Pitch( PitchClass.C, 4 ), new Pitch( PitchClass.E, 4 ), new Pitch( PitchClass.G, 4 ) );
  }

  [Fact]
  public void Constructor_WithRootAndFormulaIdOrName_ShouldInitializeWithRegistryFormula()
  {
    var root = new Pitch( PitchClass.E, 4 );

    var actual = new PitchChord( root, "Major" );

    actual.Root.Should()
          .Be( root );

    actual.Formula.Should()
          .Be( Registry.ChordFormulas["Major"] );

    actual.Inversion.Should()
          .Be( 0 );
  }

  [Fact]
  public void Constructor_WithRootAndFormulaIdOrName_ShouldResolveFormulaFromRegistry()
  {
    var root = new Pitch( PitchClass.B, 2 );

    var actual = new PitchChord( root, "Minor" );

    actual.Root.Should()
          .Be( root );

    actual.Formula.Should()
          .Be( Registry.ChordFormulas["Minor"] );

    actual.Inversion.Should()
          .Be( 0 );

    actual.Bass.Should()
          .Be( root );
  }

  [Fact]
  public void Constructor_WithRootAndFormula_ShouldCallConstructorWithZeroInversion()
  {
    var root = new Pitch( PitchClass.F, 5 );
    var formula = Registry.ChordFormulas["Diminished"];

    var actual = new PitchChord( root, formula );

    actual.Root.Should()
          .Be( root );

    actual.Formula.Should()
          .Be( formula );

    actual.Inversion.Should()
          .Be( 0 );

    actual.Bass.Should()
          .Be( root );
  }

  [Fact]
  public void Constructor_WithRootAndFormula_ShouldInitializeWithProvidedValues()
  {
    var root = new Pitch( PitchClass.D, 3 );
    var formula = ChordFormula.Minor;

    var actual = new PitchChord( root, formula );

    actual.Root.Should()
          .Be( root );

    actual.Formula.Should()
          .Be( formula );

    actual.Inversion.Should()
          .Be( 0 );
  }

  [Fact]
  public void Constructor_WithRootFormulaAndInversion_ShouldAcceptMaximumInversion()
  {
    var root = new Pitch( PitchClass.D, 4 );
    var formula = ChordFormula.Major;
    var maxInversion = formula.Intervals.Count - 1;

    var actual = new PitchChord( root, formula, maxInversion );

    actual.Inversion.Should()
          .Be( maxInversion );

    actual.Root.Should()
          .Be( root );
  }

  [Fact]
  public void Constructor_WithRootFormulaAndInversion_ShouldGenerateCorrectPitches()
  {
    var root = new Pitch( PitchClass.C, 4 );
    var formula = ChordFormula.Minor;

    var actual = new PitchChord( root, formula );

    actual.Should()
          .HaveCount( 3 );

    actual[0]
      .Should()
      .Be( new Pitch( PitchClass.C, 4 ) );

    actual[1]
      .Should()
      .Be( new Pitch( PitchClass.EFlat, 4 ) );

    actual[2]
      .Should()
      .Be( new Pitch( PitchClass.G, 4 ) );
  }

  [Fact]
  public void Constructor_WithRootFormulaAndInversion_ShouldInitializeWithProvidedValues()
  {
    var root = new Pitch( PitchClass.G, 3 );
    var formula = ChordFormula.Major;
    var inversion = 1;

    var actual = new PitchChord( root, formula, inversion );

    actual.Root.Should()
          .Be( root );

    actual.Formula.Should()
          .Be( formula );

    actual.Inversion.Should()
          .Be( inversion );
  }

  [Fact]
  public void Constructor_WithRootFormulaAndInversion_ShouldThrowArgumentNullException_WhenFormulaIsNull()
  {
    var root = new Pitch( PitchClass.C, 4 );
    ChordFormula? formula = null;

    var act = () => new PitchChord( root, formula! );

    act.Should()
       .Throw<ArgumentNullException>();
  }

  [Fact]
  public void Constructor_WithRootFormulaAndInversion_ShouldThrowArgumentOutOfRangeException_WhenInversionIsNegative()
  {
    var root = new Pitch( PitchClass.C, 4 );
    var formula = ChordFormula.Major;

    var act = () => new PitchChord( root, formula, -1 );

    act.Should()
       .Throw<ArgumentOutOfRangeException>();
  }

  [Fact]
  public void Constructor_WithRootFormulaAndInversion_ShouldThrowArgumentOutOfRangeException_WhenInversionIsTooLarge()
  {
    var root = new Pitch( PitchClass.C, 4 );
    var formula = ChordFormula.Major;

    var act = () => new PitchChord( root, formula, 3 );

    act.Should()
       .Throw<ArgumentOutOfRangeException>();
  }

  [Fact]
  public void EqualsObject_ShouldReturnFalse_WhenObjectIsNotPitchChord()
  {
    var root = new Pitch( PitchClass.E, 4 );
    var chord = new PitchChord( root, ChordFormula.Major );
    object other = "not a chord";

    var actual = chord.Equals( other );

    actual.Should()
          .BeFalse();
  }

  [Fact]
  public void EqualsObject_ShouldReturnFalse_WhenObjectIsNull()
  {
    var root = new Pitch( PitchClass.C, 4 );
    var chord = new PitchChord( root, ChordFormula.Major );

    var actual = chord.Equals( (object?) null );

    actual.Should()
          .BeFalse();
  }

  [Fact]
  public void EqualsObject_ShouldReturnFalse_WhenObjectIsPitchChordWithDifferentValues()
  {
    var chord1 = new PitchChord( new Pitch( PitchClass.C, 4 ), ChordFormula.Major );
    object chord2 = new PitchChord( new Pitch( PitchClass.D, 4 ), ChordFormula.Major );

    var actual = chord1.Equals( chord2 );

    actual.Should()
          .BeFalse();
  }

  [Fact]
  public void EqualsObject_ShouldReturnTrue_WhenComparingSameReference()
  {
    var root = new Pitch( PitchClass.F, 3 );
    var chord = new PitchChord( root, ChordFormula.Major );

    var actual = chord.Equals( (object) chord );

    actual.Should()
          .BeTrue();
  }

  [Fact]
  public void EqualsObject_ShouldReturnTrue_WhenObjectIsPitchChordWithSameValues()
  {
    var root = new Pitch( PitchClass.B, 2 );
    var chord1 = new PitchChord( root, ChordFormula.Minor );
    object chord2 = new PitchChord( root, ChordFormula.Minor );

    var actual = chord1.Equals( chord2 );

    actual.Should()
          .BeTrue();
  }

  [Fact]
  public void Equals_ShouldReturnFalse_WhenFormulasAreDifferent()
  {
    var root = new Pitch( PitchClass.G, 3 );
    var chord1 = new PitchChord( root, ChordFormula.Major );
    var chord2 = new PitchChord( root, ChordFormula.Minor );

    var actual = chord1.Equals( chord2 );

    actual.Should()
          .BeFalse();
  }

  [Fact]
  public void Equals_ShouldReturnFalse_WhenInversionsAreDifferent()
  {
    var root = new Pitch( PitchClass.A, 4 );
    var chord1 = new PitchChord( root, ChordFormula.Major );
    var chord2 = new PitchChord( root, ChordFormula.Major, 1 );

    var actual = chord1.Equals( chord2 );

    actual.Should()
          .BeFalse();
  }

  [Fact]
  public void Equals_ShouldReturnFalse_WhenOtherIsNull()
  {
    var root = new Pitch( PitchClass.E, 4 );
    var chord = new PitchChord( root, ChordFormula.Major );

    var actual = chord.Equals( null );

    actual.Should()
          .BeFalse();
  }

  [Fact]
  public void Equals_ShouldReturnFalse_WhenRootsAreDifferent()
  {
    var chord1 = new PitchChord( new Pitch( PitchClass.C, 4 ), ChordFormula.Major );
    var chord2 = new PitchChord( new Pitch( PitchClass.D, 4 ), ChordFormula.Major );

    var actual = chord1.Equals( chord2 );

    actual.Should()
          .BeFalse();
  }

  [Fact]
  public void Equals_ShouldReturnTrue_WhenChordsHaveSameRootFormulaAndInversion()
  {
    var root = new Pitch( PitchClass.D, 3 );
    var chord1 = new PitchChord( root, ChordFormula.Minor, 1 );
    var chord2 = new PitchChord( root, ChordFormula.Minor, 1 );

    var actual = chord1.Equals( chord2 );

    actual.Should()
          .BeTrue();
  }

  [Fact]
  public void Equals_ShouldReturnTrue_WhenComparingSameReference()
  {
    var root = new Pitch( PitchClass.C, 4 );
    var chord = new PitchChord( root, ChordFormula.Major );

    var actual = chord.Equals( chord );

    actual.Should()
          .BeTrue();
  }

  [Fact]
  public void GetHashCode_ShouldReturnDifferentValue_WhenFormulasAreDifferent()
  {
    var root = new Pitch( PitchClass.G, 3 );
    var chord1 = new PitchChord( root, ChordFormula.Major );
    var chord2 = new PitchChord( root, ChordFormula.Minor );

    var hash1 = chord1.GetHashCode();
    var hash2 = chord2.GetHashCode();

    hash1.Should()
         .NotBe( hash2 );
  }

  [Fact]
  public void GetHashCode_ShouldReturnDifferentValue_WhenInversionsAreDifferent()
  {
    var root = new Pitch( PitchClass.A, 4 );
    var chord1 = new PitchChord( root, ChordFormula.Major );
    var chord2 = new PitchChord( root, ChordFormula.Major, 1 );

    var hash1 = chord1.GetHashCode();
    var hash2 = chord2.GetHashCode();

    hash1.Should()
         .NotBe( hash2 );
  }

  [Fact]
  public void GetHashCode_ShouldReturnDifferentValue_WhenRootsAreDifferent()
  {
    var chord1 = new PitchChord( new Pitch( PitchClass.C, 4 ), ChordFormula.Major );
    var chord2 = new PitchChord( new Pitch( PitchClass.D, 4 ), ChordFormula.Major );

    var hash1 = chord1.GetHashCode();
    var hash2 = chord2.GetHashCode();

    hash1.Should()
         .NotBe( hash2 );
  }

  [Fact]
  public void GetHashCode_ShouldReturnSameValue_WhenChordsHaveSameRootFormulaAndInversion()
  {
    var root = new Pitch( PitchClass.C, 4 );
    var chord1 = new PitchChord( root, ChordFormula.Major, 1 );
    var chord2 = new PitchChord( root, ChordFormula.Major, 1 );

    var hash1 = chord1.GetHashCode();
    var hash2 = chord2.GetHashCode();

    hash1.Should()
         .Be( hash2 );
  }

  [Fact]
  public void GetInversion_ShouldCreateExpectedInversion_WhenRequested()
  {
    var root = new Pitch( PitchClass.C, 4 );

    var actual = new PitchChord( root, ChordFormula.Major )
                           .GetInversion( 1 );

    actual.Inversion.Should()
          .Be( 1 );

    actual.Bass.Should()
          .Be( new Pitch( PitchClass.E, 4 ) );

    actual.Should()
          .Equal( new Pitch( PitchClass.E, 4 ), new Pitch( PitchClass.G, 4 ), new Pitch( PitchClass.C, 5 ) );
  }

  [Fact]
  public void GetInversion_ShouldCreateNewInstance()
  {
    var root = new Pitch( PitchClass.E, 4 );
    var chord = new PitchChord( root, ChordFormula.Major );

    var actual = chord.GetInversion( 1 );

    actual.Should()
          .NotBeSameAs( chord );

    actual.Root.Should()
          .Be( chord.Root );

    actual.Formula.Should()
          .Be( chord.Formula );
  }

  [Fact]
  public void GetInversion_ShouldPreserveRootAndFormula()
  {
    var root = new Pitch( PitchClass.FSharp, 3 );
    var formula = Registry.ChordFormulas["Augmented"];
    var chord = new PitchChord( root, formula );

    var actual = chord.GetInversion( 1 );

    actual.Root.Should()
          .Be( root );

    actual.Formula.Should()
          .Be( formula );
  }

  [Fact]
  public void GetInversion_ShouldReturnRootPosition_WhenInversionIsZero()
  {
    var root = new Pitch( PitchClass.F, 4 );
    var chord = new PitchChord( root, ChordFormula.Major, 1 );

    var actual = chord.GetInversion( 0 );

    actual.Inversion.Should()
          .Be( 0 );

    actual.Root.Should()
          .Be( root );

    actual.Formula.Should()
          .Be( ChordFormula.Major );
  }

  [Fact]
  public void GetInversion_ShouldReturnSecondInversion_WhenInversionIsTwo()
  {
    var root = new Pitch( PitchClass.A, 3 );
    var chord = new PitchChord( root, ChordFormula.Major );

    var actual = chord.GetInversion( 2 );

    actual.Inversion.Should()
          .Be( 2 );

    actual.Root.Should()
          .Be( root );

    actual.Bass.Should()
          .Be( new Pitch( PitchClass.E, 4 ) );
  }

  [Fact]
  public void ImplementsGenericInterface_ShouldExposeSharedContract()
  {
    var chord = new PitchChord(PitchClass.C, "Major", 4, 0 );

    chord.Root.Should()
         .Be( new Pitch( PitchClass.C, 4 ) );

    chord.Bass.Should()
         .Be( new Pitch( PitchClass.C, 4 ) );

    chord.Inversion.Should()
         .Be( 0 );

    chord.Formula.Should()
         .Be( Registry.ChordFormulas["Major"] );

    chord.Name.Should()
         .Be( "C" );

    var inversion = chord.GetInversion( 1 );

    inversion.Should()
             .NotBeNull();

    inversion.Inversion.Should()
             .Be( 1 );

    inversion.Root.Should()
             .Be( new Pitch( PitchClass.C, 4 ) );

    inversion.Bass.Should()
             .Be( new Pitch( PitchClass.E, 4 ) );
  }

  [Fact]
  public void Name_ShouldUseFormulaName_WhenFormulaHasNoSymbol()
  {
    var formula = new ChordFormula( "custom", "Custom", null, "P1,M3,P5" );

    var chord = new PitchChord( PitchClass.C, formula, 4, 0 );

    chord.Name.Should()
         .Be( "CCustom" );
  }

  [Fact]
  public void Parse_ShouldParseInversion_WhenValueContainsBass()
  {
    var actual = PitchChord.Parse( "C/E" );

    actual.Root.Should()
          .Be( new Pitch( PitchClass.C, 4 ) );

    actual.Formula.Should()
          .Be( ChordFormula.Major );

    actual.Inversion.Should()
          .Be( 1 );

    actual.Bass.Should()
          .Be( new Pitch( PitchClass.E, 4 ) );
  }

  [Fact]
  public void Parse_ShouldParseMajorChord_WhenValueContainsOnlyRoot()
  {
    var actual = PitchChord.Parse( "C" );

    actual.Root.Should()
          .Be( new Pitch( PitchClass.C, 4 ) );

    actual.Formula.Should()
          .Be( ChordFormula.Major );

    actual.Inversion.Should()
          .Be( 0 );
  }

  [Fact]
  public void ToString_ShouldReturnDifferentValues_WhenChordsAreDifferent()
  {
    var chord1 = new PitchChord( new Pitch( PitchClass.C, 4 ), ChordFormula.Major );
    var chord2 = new PitchChord( new Pitch( PitchClass.D, 4 ), ChordFormula.Minor );

    var string1 = chord1.ToString();
    var string2 = chord2.ToString();

    string1.Should()
           .NotBe( string2 );
  }

  [Fact]
  public void ToString_ShouldReturnName()
  {
    var root = new Pitch( PitchClass.C, 4 );
    var chord = new PitchChord( root, ChordFormula.Major );

    var actual = chord.ToString();

    actual.Should()
          .Be( chord.Name );
  }

  [Fact]
  public void TryParse_ShouldReturnFalse_WhenValueHasBassThatIsNotPartOfChord()
  {
    var actual = PitchChord.TryParse( "C/B", out var chord );

    actual.Should()
          .BeFalse();

    chord.Should()
         .BeNull();
  }

  [Fact]
  public void TryParse_ShouldReturnFalse_WhenValueHasMissingBass()
  {
    var actual = PitchChord.TryParse( "C/", out var chord );

    actual.Should()
          .BeFalse();

    chord.Should()
         .BeNull();
  }

  [Fact]
  public void TryParse_ShouldReturnFalse_WhenValueHasMissingRoot()
  {
    var actual = PitchChord.TryParse( "/E", out var chord );

    actual.Should()
          .BeFalse();

    chord.Should()
         .BeNull();
  }

  [Fact]
  public void TryParse_ShouldReturnFalse_WhenValueHasUnknownFormula()
  {
    var actual = PitchChord.TryParse( "CUnknown", out var chord );

    actual.Should()
          .BeFalse();

    chord.Should()
         .BeNull();
  }

  [Fact]
  public void TryParse_ShouldReturnFalse_WhenValueIsInvalid()
  {
    var actual = PitchChord.TryParse( "not-a-chord", out var chord );

    actual.Should()
          .BeFalse();

    chord.Should()
         .BeNull();
  }

  [Fact]
  public void TryParse_ShouldReturnFalse_WhenValueIsNull()
  {
    var actual = PitchChord.TryParse( null, out var chord );

    actual.Should()
          .BeFalse();

    chord.Should()
         .BeNull();
  }

  [Fact]
  public void TryParse_ShouldReturnFalse_WhenValueIsWhitespace()
  {
    var actual = PitchChord.TryParse( "   \t  ", out var chord );

    actual.Should()
          .BeFalse();

    chord.Should()
         .BeNull();
  }

  [Fact]
  public void TryParse_ShouldReturnTrue_WhenValueContainsFormulaSymbol()
  {
    var actual = PitchChord.TryParse( "Cm", out var chord );

    actual.Should()
          .BeTrue();

    chord.Should()
         .NotBeNull();

    chord.Root.Should()
         .Be( new Pitch( PitchClass.C, 4 ) );

    chord.Formula.Should()
         .Be( ChordFormula.Minor );

    chord.Inversion.Should()
         .Be( 0 );
  }

  [Fact]
  public void TryParse_ShouldReturnTrue_WhenValueContainsPitchClassInversion()
  {
    var actual = PitchChord.TryParse( "Cmaj11/E", out var chord );

    actual.Should()
          .BeTrue();

    chord.Should()
         .NotBeNull();

    chord.Root.Should()
         .Be( new Pitch( PitchClass.C, 4 ) );

    chord.Formula.Should()
         .Be( Registry.ChordFormulas["Major11"] );

    chord.Inversion.Should()
         .Be( 1 );

    chord.Bass.Should()
         .Be( new Pitch( PitchClass.E, 4 ) );
  }

  [Fact]
  public void TryParse_ShouldReturnTrue_WhenValueContainsPitchClassRoot()
  {
    var actual = PitchChord.TryParse( "C", out var chord );

    actual.Should()
          .BeTrue();

    chord.Should()
         .NotBeNull();

    chord.Root.Should()
         .Be( new Pitch( PitchClass.C, 4 ) );

    chord.Formula.Should()
         .Be( ChordFormula.Major );

    chord.Inversion.Should()
         .Be( 0 );
  }

  [Fact]
  public void TryParse_ShouldReturnTrue_WhenValueContainsPitchInversion()
  {
    var actual = PitchChord.TryParse( "Cmaj11/E3", out var chord );

    actual.Should()
          .BeTrue();

    chord.Should()
         .NotBeNull();

    chord.Root.Should()
         .Be( new Pitch( PitchClass.C, 3 ) );

    chord.Formula.Should()
         .Be( Registry.ChordFormulas["Major11"] );

    chord.Inversion.Should()
         .Be( 1 );

    chord.Bass.Should()
         .Be( new Pitch( PitchClass.E, 3 ) );
  }

  #endregion
}
