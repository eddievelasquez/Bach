// Module Name: ScaleTest.cs
// Project:     Bach.Model.Test
// Copyright (c) 2012, 2023  Eddie Velasquez.
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

using System.Collections.Generic;
using System.Linq;

public sealed class ScaleTest
{
  #region Public Methods

  [Fact]
  public void ContainsTest()
  {
    var scale = new Scale( PitchClass.C, "major" );
    scale.Contains( new[] { PitchClass.C } )
         .Should()
         .BeTrue();
    scale.Contains( new[] { PitchClass.C, PitchClass.E, PitchClass.G } )
         .Should()
         .BeTrue();

    scale.Contains( new[] { PitchClass.C, PitchClass.E, PitchClass.GFlat } )
         .Should()
         .BeFalse();
  }

  [Fact]
  public void EnharmonicScaleTest()
  {
    TestEnharmonic( PitchClass.C, PitchClass.C, "major" );
    TestEnharmonic( PitchClass.CSharp, PitchClass.DFlat, "major" );
    TestEnharmonic( PitchClass.DSharp, PitchClass.EFlat, "major" );
    TestEnharmonic( PitchClass.Parse( "E#" ), PitchClass.F, "major" );
    TestEnharmonic( PitchClass.Parse( "Fb" ), PitchClass.E, "major" );
    TestEnharmonic( PitchClass.GSharp, PitchClass.AFlat, "major" );
    TestEnharmonic( PitchClass.ASharp, PitchClass.BFlat, "major" );
    TestEnharmonic( PitchClass.Parse( "B#" ), PitchClass.C, "major" );
  }

  [Fact]
  public void EqualsContractTest()
  {
    object x = new Scale( PitchClass.C, "Major" );
    object y = new Scale( PitchClass.C, "Major" );
    object z = new Scale( PitchClass.C, "Major" );

    // ReSharper disable once EqualExpressionComparison
    x.Equals( x )
     .Should()
     .BeTrue(); // Reflexive
    x.Equals( y )
     .Should()
     .BeTrue(); // Symmetric
    y.Equals( x )
     .Should()
     .BeTrue();
    y.Equals( z )
     .Should()
     .BeTrue(); // Transitive
    x.Equals( z )
     .Should()
     .BeTrue();
    x.Equals( null )
     .Should()
     .BeFalse(); // Never equal to null
  }

  [Fact]
  public void EqualsFailsWithDifferentTypeTest()
  {
    object actual = new Scale( PitchClass.C, "Major" );
    actual.Equals( int.MinValue )
          .Should()
          .BeFalse();
  }

  [Fact]
  public void EqualsFailsWithNullTest()
  {
    object actual = new Scale( PitchClass.C, "Major" );
    actual.Equals( null )
          .Should()
          .BeFalse();
  }

  [Fact]
  public void EqualsSucceedsWithSameObjectTest()
  {
    var actual = new Scale( PitchClass.C, "Major" );
    actual.Equals( actual )
          .Should()
          .BeTrue();
  }

  [Fact]
  public void FormulaConstructorTest()
  {
    var formula = Registry.ScaleFormulas["Major"];
    var actual = new Scale( PitchClass.C, formula );
    actual.Name.Should()
          .BeEquivalentTo( "C" );
    actual.Root.Should()
          .BeEquivalentTo( PitchClass.C );
    actual.Formula.Should()
          .BeEquivalentTo( Registry.ScaleFormulas["Major"] );
  }

  [Fact]
  public void FormulaConstructorThrowsOnNullFormulaTest()
  {
    var act = () => new Scale( PitchClass.C, (ScaleFormula) null! );
    act.Should()
       .Throw<ArgumentNullException>();
  }

  [Fact]
  public void GetAscendingEnumeratorTest()
  {
    var scale = new Scale( PitchClass.C, "Major" );
    using var enumerator = scale.GetAscending()
                                .GetEnumerator();
    enumerator.MoveNext()
              .Should()
              .BeTrue();
    enumerator.Current.Should()
              .BeEquivalentTo( PitchClass.C );
    enumerator.MoveNext()
              .Should()
              .BeTrue();
    enumerator.Current.Should()
              .BeEquivalentTo( PitchClass.D );
    enumerator.MoveNext()
              .Should()
              .BeTrue();
    enumerator.Current.Should()
              .BeEquivalentTo( PitchClass.E );
    enumerator.MoveNext()
              .Should()
              .BeTrue();
    enumerator.Current.Should()
              .BeEquivalentTo( PitchClass.F );
    enumerator.MoveNext()
              .Should()
              .BeTrue();
    enumerator.Current.Should()
              .BeEquivalentTo( PitchClass.G );
    enumerator.MoveNext()
              .Should()
              .BeTrue();
    enumerator.Current.Should()
              .BeEquivalentTo( PitchClass.A );
    enumerator.MoveNext()
              .Should()
              .BeTrue();
    enumerator.Current.Should()
              .BeEquivalentTo( PitchClass.B );
    enumerator.MoveNext()
              .Should()
              .BeTrue(); // Scale enumerator wraps around infinitely
    enumerator.Current.Should()
              .BeEquivalentTo( PitchClass.C );
  }

  [Fact]
  public void GetDescendingEnumeratorTest()
  {
    var scale = new Scale( PitchClass.C, "Major" );
    using var enumerator = scale.GetDescending()
                                .GetEnumerator();
    enumerator.MoveNext()
              .Should()
              .BeTrue();
    enumerator.Current.Should()
              .BeEquivalentTo( PitchClass.C );
    enumerator.MoveNext()
              .Should()
              .BeTrue();
    enumerator.Current.Should()
              .BeEquivalentTo( PitchClass.B );
    enumerator.MoveNext()
              .Should()
              .BeTrue();
    enumerator.Current.Should()
              .BeEquivalentTo( PitchClass.A );
    enumerator.MoveNext()
              .Should()
              .BeTrue();
    enumerator.Current.Should()
              .BeEquivalentTo( PitchClass.G );
    enumerator.MoveNext()
              .Should()
              .BeTrue();
    enumerator.Current.Should()
              .BeEquivalentTo( PitchClass.F );
    enumerator.MoveNext()
              .Should()
              .BeTrue();
    enumerator.Current.Should()
              .BeEquivalentTo( PitchClass.E );
    enumerator.MoveNext()
              .Should()
              .BeTrue();
    enumerator.Current.Should()
              .BeEquivalentTo( PitchClass.D );
    enumerator.MoveNext()
              .Should()
              .BeTrue(); // Scale enumerator wraps around infinitely
    enumerator.Current.Should()
              .BeEquivalentTo( PitchClass.C );
  }

  [Fact]
  public void GetHashcodeTest()
  {
    var actual = new Scale( PitchClass.C, "Major" );
    var expected = new Scale( PitchClass.C, "Major" );
    expected.Equals( actual )
            .Should()
            .BeTrue();
    actual.GetHashCode()
          .Should()
          .Be( expected.GetHashCode() );
  }

  [Fact]
  public void IsDiatonicTest()
  {
    TestPredicate( PitchClass.C, "Major", scale => scale.Formula.Categories.Contains( "Diatonic" ) );
    TestPredicate( PitchClass.C, "LeadingTone", scale => scale.Formula.Categories.Contains( "Diatonic" ) );
    TestPredicate( PitchClass.C, "LydianDominant", scale => scale.Formula.Categories.Contains( "Diatonic" ) );
    TestPredicate( PitchClass.C, "Hindu", scale => scale.Formula.Categories.Contains( "Diatonic" ) );
    TestPredicate( PitchClass.C, "Arabian", scale => scale.Formula.Categories.Contains( "Diatonic" ) );
    TestPredicate( PitchClass.C, "NaturalMinor", scale => scale.Formula.Categories.Contains( "Diatonic" ) );
    TestPredicate( PitchClass.C, "Javanese", scale => scale.Formula.Categories.Contains( "Diatonic" ) );
    TestPredicate( PitchClass.C, "NeapolitanMajor", scale => scale.Formula.Categories.Contains( "Diatonic" ) );

    TestPredicate( PitchClass.C, "HungarianFolk", scale => !scale.Formula.Categories.Contains( "Diatonic" ) );
    TestPredicate( PitchClass.C, "Gypsy", scale => !scale.Formula.Categories.Contains( "Diatonic" ) );
    TestPredicate( PitchClass.C, "EnigmaticMajor", scale => !scale.Formula.Categories.Contains( "Diatonic" ) );
    TestPredicate( PitchClass.C, "Persian", scale => !scale.Formula.Categories.Contains( "Diatonic" ) );
    TestPredicate( PitchClass.C, "Mongolian", scale => !scale.Formula.Categories.Contains( "Diatonic" ) );
  }

  [Fact]
  public void IsMajorTest()
  {
    TestPredicate( PitchClass.C, "Major", scale => scale.Formula.Categories.Contains( "Major" ) );
    TestPredicate( PitchClass.C, "HungarianFolk", scale => scale.Formula.Categories.Contains( "Major" ) );
    TestPredicate( PitchClass.C, "Gypsy", scale => scale.Formula.Categories.Contains( "Major" ) );
    TestPredicate( PitchClass.C, "Mongolian", scale => scale.Formula.Categories.Contains( "Major" ) );
  }

  [Fact]
  public void IsMinorTest()
  {
    TestPredicate( PitchClass.C, "NaturalMinor", scale => scale.Formula.Categories.Contains( "Minor" ) );
    TestPredicate( PitchClass.C, "GypsyMinor", scale => scale.Formula.Categories.Contains( "Minor" ) );
    TestPredicate( PitchClass.C, "Javanese", scale => scale.Formula.Categories.Contains( "Minor" ) );
    TestPredicate( PitchClass.C, "NeapolitanMinor", scale => scale.Formula.Categories.Contains( "Minor" ) );
    TestPredicate( PitchClass.C, "NeapolitanMajor", scale => scale.Formula.Categories.Contains( "Minor" ) );
    TestPredicate( PitchClass.C, "HungarianGypsy", scale => scale.Formula.Categories.Contains( "Minor" ) );
    TestPredicate( PitchClass.C, "Yo", scale => scale.Formula.Categories.Contains( "Minor" ) );
    TestPredicate( PitchClass.C, "Hirajoshi", scale => scale.Formula.Categories.Contains( "Minor" ) );
    TestPredicate( PitchClass.C, "Balinese", scale => scale.Formula.Categories.Contains( "Minor" ) );
  }

  [Fact]
  public void IsTheoreticalTest()
  {
    new Scale( PitchClass.C, "major" ).Theoretical.Should()
                                      .BeFalse();
    new Scale( PitchClass.DSharp, "major" ).Theoretical.Should()
                                           .BeTrue();
    new Scale( PitchClass.Parse( "E#" ), "major" ).Theoretical.Should()
                                                  .BeTrue();
    new Scale( PitchClass.Parse( "Fb" ), "major" ).Theoretical.Should()
                                                  .BeTrue();
    new Scale( PitchClass.GSharp, "major" ).Theoretical.Should()
                                           .BeTrue();
    new Scale( PitchClass.ASharp, "major" ).Theoretical.Should()
                                           .BeTrue();
    new Scale( PitchClass.Parse( "B#" ), "major" ).Theoretical.Should()
                                                  .BeTrue();
  }

  [Fact]
  public void NoteCountTest()
  {
    var scale = new Scale( PitchClass.C, "MinorPentatonic" );
    scale.PitchClasses.Count.Should()
         .Be( Registry.ScaleFormulas["MinorPentatonic"].Intervals.Count );
  }

  [Fact]
  public void NotesTest()
  {
    var scale = new Scale( PitchClass.C, "MinorPentatonic" );
    scale.PitchClasses[0]
         .Should()
         .BeEquivalentTo( PitchClass.C );
    scale.PitchClasses[1]
         .Should()
         .BeEquivalentTo( PitchClass.EFlat );
    scale.PitchClasses[2]
         .Should()
         .BeEquivalentTo( PitchClass.F );
    scale.PitchClasses[3]
         .Should()
         .BeEquivalentTo( PitchClass.G );
    scale.PitchClasses[4]
         .Should()
         .BeEquivalentTo( PitchClass.BFlat );
  }

  [Fact]
  public void RenderTest()
  {
    TestRender( new Scale( PitchClass.C, "MinorPentatonic" ), 1, "C1,Eb1,F1,G1,Bb1" );
    TestRender( new Scale( PitchClass.D, "MinorPentatonic" ), 1, "D1,F1,G1,A1,C2" );
    TestRender( new Scale( PitchClass.B, "MinorPentatonic" ), 1, "B1,D2,E2,F#2,A2" );
  }

  [Fact]
  public void RenderUsesChoosesAppropriateAccidentalForMajorScaleTest()
  {
    TestScaleAscending( "C,D,E,F,G,A,B", PitchClass.C, "Major" );
    TestScaleAscending( "C#,D#,E#,F#,G#,A#,B#", PitchClass.CSharp, "Major" );
    TestScaleAscending( "Db,Eb,F,Gb,Ab,Bb,C", PitchClass.DFlat, "Major" );
    TestScaleAscending( "D,E,F#,G,A,B,C#", PitchClass.D, "Major" );
    TestScaleAscending( "D#,E#,F##,G#,A#,B#,C##", PitchClass.DSharp, "Major" );
    TestScaleAscending( "Eb,F,G,Ab,Bb,C,D", PitchClass.EFlat, "Major" );
    TestScaleAscending( "E,F#,G#,A,B,C#,D#", PitchClass.E, "Major" );
    TestScaleAscending( "E#,F##,G##,A#,B#,C##,D##", PitchClass.Parse( "E#" ), "Major" );
    TestScaleAscending( "Fb,Gb,Ab,Bbb,Cb,Db,Eb", PitchClass.Parse( "Fb" ), "Major" );
    TestScaleAscending( "F,G,A,Bb,C,D,E", PitchClass.F, "Major" );
    TestScaleAscending( "F#,G#,A#,B,C#,D#,E#", PitchClass.FSharp, "Major" );
    TestScaleAscending( "Gb,Ab,Bb,Cb,Db,Eb,F", PitchClass.GFlat, "Major" );
    TestScaleAscending( "G,A,B,C,D,E,F#", PitchClass.G, "Major" );
    TestScaleAscending( "G#,A#,B#,C#,D#,E#,F##", PitchClass.GSharp, "Major" );
    TestScaleAscending( "Ab,Bb,C,Db,Eb,F,G", PitchClass.AFlat, "Major" );
    TestScaleAscending( "A,B,C#,D,E,F#,G#", PitchClass.A, "Major" );
    TestScaleAscending( "A#,B#,C##,D#,E#,F##,G##", PitchClass.ASharp, "Major" );
    TestScaleAscending( "Bb,C,D,Eb,F,G,A", PitchClass.BFlat, "Major" );
    TestScaleAscending( "B,C#,D#,E,F#,G#,A#", PitchClass.B, "Major" );
    TestScaleAscending( "B#,C##,D##,E#,F##,G##,A##", PitchClass.Parse( "B#" ), "Major" );
    TestScaleAscending( "Cb,Db,Eb,Fb,Gb,Ab,Bb", PitchClass.Parse( "Cb" ), "Major" );
  }

  [Fact]
  public void RenderUsesChoosesAppropriateAccidentalForNaturalMinorScaleTest()
  {
    TestScaleAscending( "C,D,Eb,F,G,Ab,Bb", PitchClass.C, "NaturalMinor" );
    TestScaleAscending( "C#,D#,E,F#,G#,A,B", PitchClass.CSharp, "NaturalMinor" );
    TestScaleAscending( "Db,Eb,Fb,Gb,Ab,Bbb,Cb", PitchClass.DFlat, "NaturalMinor" );
    TestScaleAscending( "D,E,F,G,A,Bb,C", PitchClass.D, "NaturalMinor" );
    TestScaleAscending( "D#,E#,F#,G#,A#,B,C#", PitchClass.DSharp, "NaturalMinor" );
    TestScaleAscending( "EB,F,Gb,Ab,Bb,Cb,Db", PitchClass.EFlat, "NaturalMinor" );
    TestScaleAscending( "E,F#,G,A,B,C,D", PitchClass.E, "NaturalMinor" );
    TestScaleAscending( "E#,F##,G#,A#,B#,C#,D#", PitchClass.Parse( "E#" ), "NaturalMinor" );
    TestScaleAscending( "Fb,Gb,Abb,Bbb,Cb,Dbb,Ebb", PitchClass.Parse( "Fb" ), "NaturalMinor" );
    TestScaleAscending( "F,G,Ab,Bb,C,Db,Eb", PitchClass.F, "NaturalMinor" );
    TestScaleAscending( "F#,G#,A,B,C#,D,E", PitchClass.FSharp, "NaturalMinor" );
    TestScaleAscending( "Gb,Ab,Bbb,Cb,Db,Ebb,Fb", PitchClass.GFlat, "NaturalMinor" );
    TestScaleAscending( "G,A,Bb,C,D,Eb,F", PitchClass.G, "NaturalMinor" );
    TestScaleAscending( "G#,A#,B,C#,D#,E,F#", PitchClass.GSharp, "NaturalMinor" );
    TestScaleAscending( "Ab,Bb,Cb,Db,Eb,Fb,Gb", PitchClass.AFlat, "NaturalMinor" );
    TestScaleAscending( "A,B,C,D,E,F,G", PitchClass.A, "NaturalMinor" );
    TestScaleAscending( "A#,B#,C#,D#,E#,F#,G#", PitchClass.ASharp, "NaturalMinor" );
    TestScaleAscending( "Bb,C,Db,Eb,F,Gb,Ab", PitchClass.BFlat, "NaturalMinor" );
    TestScaleAscending( "B,C#,D,E,F#,G,A", PitchClass.B, "NaturalMinor" );
    TestScaleAscending( "B#,C##,D#,E#,F##,G#,A#", PitchClass.Parse( "B#" ), "NaturalMinor" );
    TestScaleAscending( "Cb,Db,Ebb,Fb,Gb,Abb,Bbb", PitchClass.Parse( "Cb" ), "NaturalMinor" );
  }

  [Fact]
  public void ScaleAscendingTest()
  {
    var root = PitchClass.C;
    TestScaleAscending( "C,D,E,F,G,A,B", root, "Major" );
    TestScaleAscending( "C,D,Eb,F,G,Ab,Bb", root, "NaturalMinor" );
    TestScaleAscending( "C,D,Eb,F,G,Ab,B", root, "HarmonicMinor" );
    TestScaleAscending( "C,D,Eb,F,G,A,B", root, "MelodicMinor" );
    TestScaleAscending( "C,D,Eb,F,Gb,G#,A,B", root, "Diminished" );
    TestScaleAscending( "C,Db,Eb,E,F#,G,A,Bb", root, "Polytonal" );
    TestScaleAscending( "C,D,E,F#,G#,A#", root, "WholeTone" );
    TestScaleAscending( "C,D,E,G,A", root, "Pentatonic" );
    TestScaleAscending( "C,Eb,F,G,Bb", root, "MinorPentatonic" );
    TestScaleAscending( "C,Eb,F,Gb,G,Bb", root, "MinorBlues" );
    TestScaleAscending( "C,D,Eb,E,G,A", root, "MajorBlues" );
  }

  [Fact]
  public void ScaleDescendingTest()
  {
    var root = PitchClass.C;
    TestScaleDescending( "C,B,A,G,F,E,D", root, "Major" );
    TestScaleDescending( "C,Bb,Ab,G,F,Eb,D", root, "NaturalMinor" );
    TestScaleDescending( "C,B,Ab,G,F,Eb,D", root, "HarmonicMinor" );
    TestScaleDescending( "C,B,A,G,F,Eb,D", root, "MelodicMinor" );
    TestScaleDescending( "C,B,A,G#,Gb,F,Eb,D", root, "Diminished" );
    TestScaleDescending( "C,Bb,A,G,F#,E,Eb,Db", root, "Polytonal" );
    TestScaleDescending( "C,A#,G#,F#,E,D", root, "WholeTone" );
    TestScaleDescending( "C,A,G,E,D", root, "Pentatonic" );
    TestScaleDescending( "C,Bb,G,F,Eb", root, "MinorPentatonic" );
    TestScaleDescending( "C,Bb,G,Gb,F,Eb", root, "MinorBlues" );
    TestScaleDescending( "C,A,G,E,Eb,D", root, "MajorBlues" );
  }

  [Fact]
  public void ScalesContainingTest()
  {
    IDictionary<string, Scale> scales = Scale.ScalesContaining( new[] { PitchClass.C, PitchClass.E, PitchClass.G } )
                                             .ToDictionary( scale => scale.Name );

    scales.Should()
          .ContainKey( "C" );
    scales.Should()
          .ContainKey( "C Pentatonic" );
    scales.Should()
          .ContainKey( "E Natural Minor" );
    scales.Should()
          .ContainKey( "E Harmonic Minor" );
    scales.Should()
          .ContainKey( "G" );
    scales.Should()
          .ContainKey( "G Melodic Minor" );
    scales.Should()
          .ContainKey( "G Diminished" );
  }

  [Fact]
  public void StringConstructorTest()
  {
    var actual = new Scale( PitchClass.C, "Major" );
    actual.Name.Should()
          .BeEquivalentTo( "C" );
    actual.Root.Should()
          .BeEquivalentTo( PitchClass.C );
    actual.Formula.Should()
          .BeEquivalentTo( Registry.ScaleFormulas["Major"] );
  }

  [Fact]
  public void StringConstructorThrowsOnEmptyFormulaNameTest()
  {
    var act = () => new Scale( PitchClass.C, "" );
    act.Should()
       .Throw<ArgumentException>();
  }

  [Fact]
  public void StringConstructorThrowsOnNullFormulaNameTest()
  {
    var act = () => new Scale( PitchClass.C, (string) null! );
    act.Should()
       .Throw<ArgumentNullException>();
  }

  [Fact]
  public void ToStringTest()
  {
    var scale = new Scale( PitchClass.C, "MinorPentatonic" );
    scale.ToString()
         .Should()
         .BeEquivalentTo( "C Minor Pentatonic {C,Eb,F,G,Bb}" );
  }

  [Fact]
  public void TypeSafeEqualsContractTest()
  {
    var x = new Scale( PitchClass.C, "Major" );
    var y = new Scale( PitchClass.C, "Major" );
    var z = new Scale( PitchClass.C, "Major" );

    x.Equals( x )
     .Should()
     .BeTrue(); // Reflexive
    x.Equals( y )
     .Should()
     .BeTrue(); // Symmetric
    y.Equals( x )
     .Should()
     .BeTrue();
    y.Equals( z )
     .Should()
     .BeTrue(); // Transitive
    x.Equals( z )
     .Should()
     .BeTrue();
    x.Equals( null )
     .Should()
     .BeFalse(); // Never equal to null
  }

  [Fact]
  public void TypeSafeEqualsFailsWithDifferentTypeTest()
  {
    var actual = new Scale( PitchClass.C, "Major" );

    // ReSharper disable once SuspiciousTypeConversion.Global
    actual.Equals( int.MinValue )
          .Should()
          .BeFalse();
  }

  [Fact]
  public void TypeSafeEqualsFailsWithNullTest()
  {
    var actual = new Scale( PitchClass.C, "Major" );
    actual.Equals( null )
          .Should()
          .BeFalse();
  }

  #endregion

  #region Implementation

  private static void TestEnharmonic(
    PitchClass root,
    PitchClass enharmonicRoot,
    string scaleKey )
  {
    var scale = new Scale( root, scaleKey );
    var actual = scale.GetEnharmonicScale();
    var expected = new Scale( enharmonicRoot, scaleKey );
    actual.Should()
          .BeEquivalentTo( expected );
  }

  private static void TestRender(
    Scale scale,
    int octave,
    string expectedNotes )
  {
    var actual = scale.Render( octave )
                      .Take( scale.Formula.Intervals.Count )
                      .ToArray();
    actual.Should()
          .BeEquivalentTo( PitchCollection.Parse( expectedNotes ) );
  }

  private static void TestScaleAscending(
    string expectedNotes,
    PitchClass root,
    string formulaName )
  {
    var expected = PitchClassCollection.Parse( expectedNotes );
    var scale = new Scale( root, formulaName );
    var actual = scale.GetAscending()
                      .Take( expected.Count );
    actual.Should()
          .BeEquivalentTo( expected );
  }

  private static void TestScaleDescending(
    string expectedNotes,
    PitchClass root,
    string formulaName )
  {
    var expected = PitchClassCollection.Parse( expectedNotes );
    var scale = new Scale( root, formulaName );
    var actual = scale.GetDescending()
                      .Take( expected.Count );
    actual.Should()
          .BeEquivalentTo( expected );
  }

  private static void TestPredicate(
    PitchClass root,
    string formulaName,
    Predicate<Scale> predicate )
  {
    var scale = new Scale( root, formulaName );
    predicate( scale )
      .Should()
      .BeTrue();
  }

  #endregion
}
