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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Bach.Model.Test;

public sealed class ScaleTest
{
#region Public Methods

  [Fact]
  public void ContainsTest()
  {
    var scale = new Scale( PitchClass.C, "major" );
    Assert.True( scale.Contains( new[] { PitchClass.C } ) );
    Assert.True( scale.Contains( new[] { PitchClass.C, PitchClass.E, PitchClass.G } ) );

    Assert.False( scale.Contains( new[] { PitchClass.C, PitchClass.E, PitchClass.GFlat } ) );
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
    Assert.True( x.Equals( x ) ); // Reflexive
    Assert.True( x.Equals( y ) ); // Symmetric
    Assert.True( y.Equals( x ) );
    Assert.True( y.Equals( z ) ); // Transitive
    Assert.True( x.Equals( z ) );
    Assert.False( x.Equals( null ) ); // Never equal to null
  }

  [Fact]
  public void EqualsFailsWithDifferentTypeTest()
  {
    object actual = new Scale( PitchClass.C, "Major" );
    Assert.False( actual.Equals( int.MinValue ) );
  }

  [Fact]
  public void EqualsFailsWithNullTest()
  {
    object actual = new Scale( PitchClass.C, "Major" );
    Assert.False( actual.Equals( null ) );
  }

  [Fact]
  public void EqualsSucceedsWithSameObjectTest()
  {
    var actual = new Scale( PitchClass.C, "Major" );
    Assert.True( actual.Equals( actual ) );
  }

  [Fact]
  public void FormulaConstructorTest()
  {
    var formula = Registry.ScaleFormulas["Major"];
    var actual = new Scale( PitchClass.C, formula );
    Assert.Equal( "C", actual.Name );
    Assert.Equal( PitchClass.C, actual.Root );
    Assert.Equal( Registry.ScaleFormulas["Major"], actual.Formula );
  }

  [Fact]
  public void FormulaConstructorThrowsOnNullFormulaTest()
  {
    Assert.Throws<ArgumentNullException>( () => new Scale( PitchClass.C, (ScaleFormula) null ) );
  }

  [Fact]
  public void GetAscendingEnumeratorTest()
  {
    var scale = new Scale( PitchClass.C, "Major" );
    IEnumerator enumerator = scale.GetAscending().GetEnumerator();
    Assert.True( enumerator.MoveNext() );
    Assert.Equal( PitchClass.C, enumerator.Current );
    Assert.True( enumerator.MoveNext() );
    Assert.Equal( PitchClass.D, enumerator.Current );
    Assert.True( enumerator.MoveNext() );
    Assert.Equal( PitchClass.E, enumerator.Current );
    Assert.True( enumerator.MoveNext() );
    Assert.Equal( PitchClass.F, enumerator.Current );
    Assert.True( enumerator.MoveNext() );
    Assert.Equal( PitchClass.G, enumerator.Current );
    Assert.True( enumerator.MoveNext() );
    Assert.Equal( PitchClass.A, enumerator.Current );
    Assert.True( enumerator.MoveNext() );
    Assert.Equal( PitchClass.B, enumerator.Current );
    Assert.True( enumerator.MoveNext() ); // Scale enumerator wraps around infinitely
    Assert.Equal( PitchClass.C, enumerator.Current );
  }

  [Fact]
  public void GetDescendingEnumeratorTest()
  {
    var scale = new Scale( PitchClass.C, "Major" );
    IEnumerator enumerator = scale.GetDescending().GetEnumerator();
    Assert.True( enumerator.MoveNext() );
    Assert.Equal( PitchClass.C, enumerator.Current );
    Assert.True( enumerator.MoveNext() );
    Assert.Equal( PitchClass.B, enumerator.Current );
    Assert.True( enumerator.MoveNext() );
    Assert.Equal( PitchClass.A, enumerator.Current );
    Assert.True( enumerator.MoveNext() );
    Assert.Equal( PitchClass.G, enumerator.Current );
    Assert.True( enumerator.MoveNext() );
    Assert.Equal( PitchClass.F, enumerator.Current );
    Assert.True( enumerator.MoveNext() );
    Assert.Equal( PitchClass.E, enumerator.Current );
    Assert.True( enumerator.MoveNext() );
    Assert.Equal( PitchClass.D, enumerator.Current );
    Assert.True( enumerator.MoveNext() ); // Scale enumerator wraps around infinitely
    Assert.Equal( PitchClass.C, enumerator.Current );
  }

  [Fact]
  public void GetHashcodeTest()
  {
    var actual = new Scale( PitchClass.C, "Major" );
    var expected = new Scale( PitchClass.C, "Major" );
    Assert.True( expected.Equals( actual ) );
    Assert.Equal( expected.GetHashCode(), actual.GetHashCode() );
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
    Assert.False( new Scale( PitchClass.C, "major" ).Theoretical );
    Assert.True( new Scale( PitchClass.DSharp, "major" ).Theoretical );
    Assert.True( new Scale( PitchClass.Parse( "E#" ), "major" ).Theoretical );
    Assert.True( new Scale( PitchClass.Parse( "Fb" ), "major" ).Theoretical );
    Assert.True( new Scale( PitchClass.GSharp, "major" ).Theoretical );
    Assert.True( new Scale( PitchClass.ASharp, "major" ).Theoretical );
    Assert.True( new Scale( PitchClass.Parse( "B#" ), "major" ).Theoretical );
  }

  [Fact]
  public void NoteCountTest()
  {
    var scale = new Scale( PitchClass.C, "MinorPentatonic" );
    Assert.Equal( Registry.ScaleFormulas["MinorPentatonic"].Intervals.Count, scale.PitchClasses.Count );
  }

  [Fact]
  public void NotesTest()
  {
    var scale = new Scale( PitchClass.C, "MinorPentatonic" );
    Assert.Equal( PitchClass.C, scale.PitchClasses[0] );
    Assert.Equal( PitchClass.EFlat, scale.PitchClasses[1] );
    Assert.Equal( PitchClass.F, scale.PitchClasses[2] );
    Assert.Equal( PitchClass.G, scale.PitchClasses[3] );
    Assert.Equal( PitchClass.BFlat, scale.PitchClasses[4] );
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

    Assert.Contains( "C", scales );
    Assert.Contains( "C Pentatonic", scales );
    Assert.Contains( "E Natural Minor", scales );
    Assert.Contains( "E Harmonic Minor", scales );
    Assert.Contains( "G", scales );
    Assert.Contains( "G Melodic Minor", scales );
    Assert.Contains( "G Diminished", scales );
  }

  [Fact]
  public void StringConstructorTest()
  {
    var actual = new Scale( PitchClass.C, "Major" );
    Assert.Equal( "C", actual.Name );
    Assert.Equal( PitchClass.C, actual.Root );
    Assert.Equal( Registry.ScaleFormulas["Major"], actual.Formula );
  }

  [Fact]
  public void StringConstructorThrowsOnEmptyFormulaNameTest()
  {
    Assert.Throws<ArgumentException>( () => new Scale( PitchClass.C, "" ) );
  }

  [Fact]
  public void StringConstructorThrowsOnNullFormulaNameTest()
  {
    Assert.Throws<ArgumentNullException>( () => new Scale( PitchClass.C, (string) null ) );
  }

  [Fact]
  public void ToStringTest()
  {
    var scale = new Scale( PitchClass.C, "MinorPentatonic" );
    Assert.Equal( "C Minor Pentatonic {C,Eb,F,G,Bb}", scale.ToString() );
  }

  [Fact]
  public void TypeSafeEqualsContractTest()
  {
    var x = new Scale( PitchClass.C, "Major" );
    var y = new Scale( PitchClass.C, "Major" );
    var z = new Scale( PitchClass.C, "Major" );

    Assert.True( x.Equals( x ) ); // Reflexive
    Assert.True( x.Equals( y ) ); // Symmetric
    Assert.True( y.Equals( x ) );
    Assert.True( y.Equals( z ) ); // Transitive
    Assert.True( x.Equals( z ) );
    Assert.False( x.Equals( null ) ); // Never equal to null
  }

  [Fact]
  public void TypeSafeEqualsFailsWithDifferentTypeTest()
  {
    var actual = new Scale( PitchClass.C, "Major" );

    // ReSharper disable once SuspiciousTypeConversion.Global
    Assert.False( actual.Equals( int.MinValue ) );
  }

  [Fact]
  public void TypeSafeEqualsFailsWithNullTest()
  {
    var actual = new Scale( PitchClass.C, "Major" );
    Assert.False( actual.Equals( null ) );
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
    Assert.Equal( expected, actual );
  }

  private static void TestRender(
    Scale scale,
    int octave,
    string expectedNotes )
  {
    var actual = scale.Render( octave ).Take( scale.Formula.Intervals.Count ).ToArray();
    Assert.Equal( PitchCollection.Parse( expectedNotes ), actual );
  }

  private static void TestScaleAscending(
    string expectedNotes,
    PitchClass root,
    string formulaName )
  {
    var expected = PitchClassCollection.Parse( expectedNotes );
    var scale = new Scale( root, formulaName );
    var actual = scale.GetAscending().Take( expected.Count );
    Assert.Equal( expected, actual );
  }

  private static void TestScaleDescending(
    string expectedNotes,
    PitchClass root,
    string formulaName )
  {
    var expected = PitchClassCollection.Parse( expectedNotes );
    var scale = new Scale( root, formulaName );
    var actual = scale.GetDescending().Take( expected.Count );
    Assert.Equal( expected, actual );
  }

  private static void TestPredicate(
    PitchClass root,
    string formulaName,
    Predicate<Scale> predicate )
  {
    var scale = new Scale( root, formulaName );
    Assert.True( predicate( scale ) );
  }

#endregion
}
