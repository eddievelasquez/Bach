// Module Name: ${File.FileName}
// Project:     ${File.ProjectName}
// Copyright (c) 2012, ${CurrentDate.Year}  Eddie Velasquez.
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

public sealed class ScaleTest
{
  #region Public Methods

  [Fact]
  public void Contains_ShouldReturnTrue_WhenScaleContainsAllPitchClasses()
  {
    var scale = new Scale( PitchClass.C, "major" );
    scale.Contains( [PitchClass.C] )
         .Should()
         .BeTrue();
    scale.Contains( [PitchClass.C, PitchClass.E, PitchClass.G] )
         .Should()
         .BeTrue();

    scale.Contains( [PitchClass.C, PitchClass.E, PitchClass.GFlat] )
         .Should()
         .BeFalse();
  }

  [Theory]
  [MemberData( nameof( EnharmonicScaleData ) )]
  public void GetEnharmonicScale_ShouldReturnEquivalentScale_WhenEnharmonicExists( PitchClass root, PitchClass enharmonicRoot, string scaleName )
  {
    var scale = new Scale( root, scaleName );
    var actual = scale.GetEnharmonicScale();
    var expected = new Scale( enharmonicRoot, scaleName );
    actual.Should()
          .BeEquivalentTo( expected );
  }

  [Fact]
  public void Equals_ShouldSatisfyEquivalenceRelation_ObjectVariant()
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
  public void Equals_ShouldReturnFalse_WhenComparingDifferentTypes()
  {
    object actual = new Scale( PitchClass.C, "Major" );
    actual.Equals( int.MinValue )
          .Should()
          .BeFalse();
  }

  [Fact]
  public void Equals_ShouldReturnFalse_WhenComparingWithNull()
  {
    object actual = new Scale( PitchClass.C, "Major" );
    actual.Equals( null )
          .Should()
          .BeFalse();
  }

  [Fact]
  public void Equals_ShouldReturnTrue_WhenComparingSameObject()
  {
    var actual = new Scale( PitchClass.C, "Major" );
    actual.Equals( actual )
          .Should()
          .BeTrue();
  }

  [Fact]
  public void FormulaConstructor_ShouldInitializeCorrectly_WhenGivenValidFormula()
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
  public void FormulaConstructor_ShouldThrowArgumentNullException_WhenGivenNullFormula()
  {
    var act = () => new Scale( PitchClass.C, (ScaleFormula) null! );
    act.Should()
       .Throw<ArgumentNullException>();
  }

  [Fact]
  public void GetAscending_ShouldReturnCorrectNotes_WhenIteratingScale()
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
  public void GetDescending_ShouldReturnCorrectNotes_WhenIteratingScale()
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
  public void GetHashCode_ShouldReturnSameValue_WhenScalesAreEqual()
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

  [Theory]
  [MemberData( nameof( ScaleCategoryData ) )]
  public void Scale_ShouldBelongToCategory_WhenMatchingCriteria( string scaleName, Predicate<Scale> condition )
  {
    var scale = new Scale( PitchClass.C, scaleName );
    condition( scale ).Should().BeTrue();
  }

  [Theory]
  [MemberData( nameof( TheoreticalScaleData ) )]
  public void Theoretical_ShouldReturnExpectedValue_WhenScaleContainsComplexAccidentals( PitchClass root, bool isTheoretical )
  {
    new Scale( root, "major" ).Theoretical.Should().Be( isTheoretical );
  }

  [Fact]
  public void PitchClasses_ShouldContainExpectedIntervalCount()
  {
    var scale = new Scale( PitchClass.C, "MinorPentatonic" );
    scale.PitchClasses.Count.Should()
         .Be( Registry.ScaleFormulas["MinorPentatonic"].Intervals.Count );
  }

  [Theory]
  [MemberData( nameof( PitchClassesInScaleData ) )]
  public void PitchClasses_ShouldContainExpectedPitchClasses_WhenGivenScale( PitchClass root, string scaleName, PitchClass[] expectedPitchClasses )
  {
    var scale = new Scale( root, scaleName );
    scale.PitchClasses.Should().BeEquivalentTo( expectedPitchClasses, options => options.WithStrictOrdering() );
  }

  [Theory]
  [MemberData( nameof( RenderTestData ) )]
  public void Render_ShouldReturnExpectedPitches_WhenGivenScaleAndOctave( PitchClass root, string scaleName, int octave, string expectedNotes )
  {
    var scale = new Scale( root, scaleName );

    var actual = scale.Render( octave )
                      .Take( scale.Formula.Intervals.Count )
                      .ToArray();

    actual.Should()
          .BeEquivalentTo( PitchCollection.Parse( expectedNotes ) );
  }

  [Theory]
  [MemberData( nameof( RenderAccidentalTestData ) )]
  public void Render_ShouldChooseAppropriateAccidental_WhenGivenScaleRootAndName( PitchClass root, string scaleName, string expectedNotes )
  {
    var expectedPitchesClasses = PitchClassCollection.Parse( expectedNotes );
    var scale = new Scale( root, scaleName );
    var actualPitchClasses = scale.GetAscending()
                      .Take( expectedPitchesClasses.Count );

    actualPitchClasses.Should()
          .BeEquivalentTo( expectedPitchesClasses );
  }

  [Theory]
  [MemberData( nameof( DescendingScaleData ) )]
  public void GetDescending_ShouldReturnCorrectNotes_WhenIteratingSpecificScale( string expectedNotes, string formulaName )
  {
    var expected = PitchClassCollection.Parse( expectedNotes );
    var scale = new Scale( PitchClass.C, formulaName );
    var actual = scale.GetDescending()
                      .Take( expected.Count );
    actual.Should()
          .BeEquivalentTo( expected );
  }

  [Theory]
  [MemberData( nameof( ScalesContainingData ) )]
  public void ScalesContaining_ShouldReturnExpectedScales_WhenGivenPitchClasses( string pitchClassNames, string[] expectedScaleNames )
  {
    var pitchClasses = PitchClassCollection.Parse( pitchClassNames );

    var actualScales = Scale.ScalesContaining( pitchClasses )
                     .Select( scale => scale.Name )
                     .ToHashSet( StringComparer.OrdinalIgnoreCase );

    var expectedScales = expectedScaleNames.ToHashSet( StringComparer.OrdinalIgnoreCase );

    expectedScales.IsSubsetOf( actualScales )
                  .Should()
                  .BeTrue();
  }

  [Fact]
  public void Constructor_ShouldInitialize_WhenPassingValidValues()
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
  public void Constructor_ShouldThrowArgumentException_WhenPassingEmptyScaleName()
  {
    var act = () => new Scale( PitchClass.C, "" );
    act.Should()
       .Throw<ArgumentException>();
  }

  [Fact]
  public void Constructor_ShouldThrowArgumentNullException_WhenPassingNullScaleName()
  {
    var act = () => new Scale( PitchClass.C, (string) null! );
    act.Should()
       .Throw<ArgumentNullException>();
  }

  public static TheoryData<string, string, string> ToStringTestData => new()
  {
    { "C", "MinorPentatonic", "C Minor Pentatonic {C,Eb,F,G,Bb}" },
    { "C", "Major", "C {C,D,E,F,G,A,B}" },
    { "A", "NaturalMinor", "A Natural Minor {A,B,C,D,E,F,G}" },
    { "Eb", "Major", "Eb {Eb,F,G,Ab,Bb,C,D}" }
  };

  [Theory]
  [MemberData( nameof( ToStringTestData ) )]
  public void ToString_ShouldReturnExpectedValue_WhenGivenScale( string root, string scaleName, string expected )
  {
    var scale = new Scale( PitchClass.Parse( root ), scaleName );
    scale.ToString()
         .Should()
         .BeEquivalentTo( expected );
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
  }

  #endregion

  #region Test Data

  public static TheoryData<PitchClass, PitchClass, string> EnharmonicScaleData => new()
  {
    { PitchClass.C, PitchClass.C, "major" },
    { PitchClass.CSharp, PitchClass.DFlat, "major" },
    { PitchClass.DSharp, PitchClass.EFlat, "major" },
    { PitchClass.Parse("E#"), PitchClass.F, "major" },
    { PitchClass.Parse("Fb"), PitchClass.E, "major" },
    { PitchClass.GSharp, PitchClass.AFlat, "major" },
    { PitchClass.ASharp, PitchClass.BFlat, "major" },
    { PitchClass.Parse("B#"), PitchClass.C, "major" }
  };

  public static TheoryData<string, Predicate<Scale>> ScaleCategoryData => new()
  {
    // Diatonic scales
    { "Major", scale => scale.Formula.Categories.Contains("Diatonic") },
    { "LeadingTone", scale => scale.Formula.Categories.Contains("Diatonic") },
    { "LydianDominant", scale => scale.Formula.Categories.Contains("Diatonic") },
    { "Hindu", scale => scale.Formula.Categories.Contains("Diatonic") },
    { "Arabian", scale => scale.Formula.Categories.Contains("Diatonic") },
    { "NaturalMinor", scale => scale.Formula.Categories.Contains("Diatonic") },
    { "Javanese", scale => scale.Formula.Categories.Contains("Diatonic") },
    { "NeapolitanMajor", scale => scale.Formula.Categories.Contains("Diatonic") },

    // Major scales
    { "Major", scale => scale.Formula.Categories.Contains("Major") },
    { "HungarianFolk", scale => scale.Formula.Categories.Contains("Major") },
    { "Gypsy", scale => scale.Formula.Categories.Contains("Major") },
    { "Mongolian", scale => scale.Formula.Categories.Contains("Major") },

    // Minor scales
    { "NaturalMinor", scale => scale.Formula.Categories.Contains("Minor") },
    { "GypsyMinor", scale => scale.Formula.Categories.Contains("Minor") },
    { "Javanese", scale => scale.Formula.Categories.Contains("Minor") },
    { "NeapolitanMinor", scale => scale.Formula.Categories.Contains("Minor") },
    { "NeapolitanMajor", scale => scale.Formula.Categories.Contains("Minor") },
    { "HungarianGypsy", scale => scale.Formula.Categories.Contains("Minor") },
    { "Yo", scale => scale.Formula.Categories.Contains("Minor") },
    { "Hirajoshi", scale => scale.Formula.Categories.Contains("Minor") },
    { "Balinese", scale => scale.Formula.Categories.Contains("Minor") },

    // Non-diatonic scales
    { "HungarianFolk", scale => !scale.Formula.Categories.Contains("Diatonic") },
    { "Gypsy", scale => !scale.Formula.Categories.Contains("Diatonic") },
    { "EnigmaticMajor", scale => !scale.Formula.Categories.Contains("Diatonic") },
    { "Persian", scale => !scale.Formula.Categories.Contains("Diatonic") },
    { "Mongolian", scale => !scale.Formula.Categories.Contains("Diatonic") }
  };

  public static TheoryData<PitchClass, bool> TheoreticalScaleData => new()
  {
    { PitchClass.C, false },
    { PitchClass.DSharp, true },
    { PitchClass.Parse("E#"), true },
    { PitchClass.Parse("Fb"), true },
    { PitchClass.GSharp, true },
    { PitchClass.ASharp, true },
    { PitchClass.Parse("B#"), true }
  };

  public static TheoryData<string, string> DescendingScaleData => new()
  {
    { "C,B,A,G,F,E,D", "Major" },
    { "C,Bb,Ab,G,F,Eb,D", "NaturalMinor" },
    { "C,B,Ab,G,F,Eb,D", "HarmonicMinor" },
    { "C,B,A,G,F,Eb,D", "MelodicMinor" },
    { "C,B,A,G#,Gb,F,Eb,D", "Diminished" },
    { "C,Bb,A,G,F#,E,Eb,Db", "Polytonal" },
    { "C,A#,G#,F#,E,D", "WholeTone" },
    { "C,A,G,E,D", "Pentatonic" },
    { "C,Bb,G,F,Eb", "MinorPentatonic" },
    { "C,Bb,G,Gb,F,Eb", "MinorBlues" },
    { "C,A,G,E,Eb,D", "MajorBlues" }
  };

  public static TheoryData<string, string[]> ScalesContainingData => new()
  {
    {
        "C,E,G",
        ["C",
         "C Pentatonic",
         "E Natural Minor",
         "E Harmonic Minor",
         "G",
         "G Melodic Minor",
         "G Diminished"]
    }
  };

  public static TheoryData<PitchClass, string, PitchClass[]> PitchClassesInScaleData => new()
  {
    { PitchClass.C, "MinorPentatonic", [PitchClass.C, PitchClass.EFlat, PitchClass.F, PitchClass.G, PitchClass.BFlat] },
    { PitchClass.D, "MinorPentatonic", [PitchClass.D, PitchClass.F, PitchClass.G, PitchClass.A, PitchClass.C] },
    { PitchClass.B, "MinorPentatonic", [PitchClass.B, PitchClass.D, PitchClass.E, PitchClass.FSharp, PitchClass.A] }
  };

  public static TheoryData<PitchClass, string, int, string> RenderTestData => new()
  {
    { PitchClass.C, "MinorPentatonic", 1, "C1,Eb1,F1,G1,Bb1" },
    { PitchClass.D, "MinorPentatonic", 1, "D1,F1,G1,A1,C2" },
    { PitchClass.B, "MinorPentatonic", 1, "B1,D2,E2,F#2,A2" }
  };

  public static TheoryData<PitchClass, string, string> RenderAccidentalTestData => new()
  {
    { PitchClass.C, "Major", "C,D,E,F,G,A,B" },
    { PitchClass.CSharp, "Major", "C#,D#,E#,F#,G#,A#,B#" },
    { PitchClass.DFlat, "Major", "Db,Eb,F,Gb,Ab,Bb,C" },
    { PitchClass.D, "Major", "D,E,F#,G,A,B,C#" },
    { PitchClass.DSharp, "Major", "D#,E#,F##,G#,A#,B#,C##" },
    { PitchClass.EFlat, "Major", "Eb,F,G,Ab,Bb,C,D" },
    { PitchClass.E, "Major", "E,F#,G#,A,B,C#,D#" },
    { PitchClass.Parse("E#"), "Major", "E#,F##,G##,A#,B#,C##,D##" },
    { PitchClass.Parse("Fb"), "Major", "Fb,Gb,Ab,Bbb,Cb,Db,Eb" },
    { PitchClass.F, "Major", "F,G,A,Bb,C,D,E" },
    { PitchClass.FSharp, "Major", "F#,G#,A#,B,C#,D#,E#" },
    { PitchClass.GFlat, "Major", "Gb,Ab,Bb,Cb,Db,Eb,F" },
    { PitchClass.G, "Major", "G,A,B,C,D,E,F#" },
    { PitchClass.GSharp, "Major", "G#,A#,B#,C#,D#,E#,F##" },
    { PitchClass.AFlat, "Major", "Ab,Bb,C,Db,Eb,F,G" },
    { PitchClass.A, "Major", "A,B,C#,D,E,F#,G#" },
    { PitchClass.ASharp, "Major", "A#,B#,C##,D#,E#,F##,G##" },
    { PitchClass.BFlat, "Major", "Bb,C,D,Eb,F,G,A" },
    { PitchClass.B, "Major", "B,C#,D#,E,F#,G#,A#" },
    { PitchClass.Parse("B#"), "Major", "B#,C##,D##,E#,F##,G##,A##" },
    { PitchClass.Parse("Cb"), "Major", "Cb,Db,Eb,Fb,Gb,Ab,Bb" },
    { PitchClass.C, "NaturalMinor", "C,D,Eb,F,G,Ab,Bb" },
    { PitchClass.CSharp, "NaturalMinor", "C#,D#,E,F#,G#,A,B" },
    { PitchClass.DFlat, "NaturalMinor", "Db,Eb,Fb,Gb,Ab,Bbb,Cb" },
    { PitchClass.D, "NaturalMinor", "D,E,F,G,A,Bb,C" },
    { PitchClass.DSharp, "NaturalMinor", "D#,E#,F#,G#,A#,B,C#" },
    { PitchClass.EFlat, "NaturalMinor", "EB,F,Gb,Ab,Bb,Cb,Db" },
    { PitchClass.E, "NaturalMinor", "E,F#,G,A,B,C,D" },
    { PitchClass.Parse("E#"), "NaturalMinor", "E#,F##,G#,A#,B#,C#,D#" },
    { PitchClass.Parse("Fb"), "NaturalMinor", "Fb,Gb,Abb,Bbb,Cb,Dbb,Ebb" },
    { PitchClass.F, "NaturalMinor", "F,G,Ab,Bb,C,Db,Eb" },
    { PitchClass.FSharp, "NaturalMinor", "F#,G#,A,B,C#,D,E" },
    { PitchClass.GFlat, "NaturalMinor", "Gb,Ab,Bbb,Cb,Db,Ebb,Fb" },
    { PitchClass.G, "NaturalMinor", "G,A,Bb,C,D,Eb,F" },
    { PitchClass.GSharp, "NaturalMinor", "G#,A#,B,C#,D#,E,F#" },
    { PitchClass.AFlat, "NaturalMinor", "Ab,Bb,Cb,Db,Eb,Fb,Gb" },
    { PitchClass.A, "NaturalMinor", "A,B,C,D,E,F,G" },
    { PitchClass.ASharp, "NaturalMinor", "A#,B#,C#,D#,E#,F#,G#" },
    { PitchClass.BFlat, "NaturalMinor", "Bb,C,Db,Eb,F,Gb,Ab" },
    { PitchClass.B, "NaturalMinor", "B,C#,D,E,F#,G,A" },
    { PitchClass.Parse("B#"), "NaturalMinor", "B#,C##,D#,E#,F##,G#,A#" },
    { PitchClass.Parse("Cb"), "NaturalMinor", "Cb,Db,Ebb,Fb,Gb,Abb,Bbb" }
  };

  #endregion
}
