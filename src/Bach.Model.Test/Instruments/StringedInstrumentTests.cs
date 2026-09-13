// Module Name: StringedInstrumentTest.cs
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

namespace Bach.Model.Instruments.Test;

public sealed class StringedInstrumentTests
{
  #region Public Methods

  [Fact]
  public void CreateWithDefinitionDefaultTuningTest()
  {
    var definition = Registry.StringedInstrumentDefinitions["guitar"];
    var expectedTuning = definition.Tunings.Standard;

    var instrument = new StringedInstrument( definition, 22, (Tuning?)null );

    instrument.Should()
              .NotBeNull();

    instrument.Definition.Should()
              .Be( definition );

    instrument.PositionCount.Should()
              .Be( 22 );

    instrument.Tuning.Should()
              .Be( expectedTuning );
  }

  [Fact]
  public void CreateWithDefinitionTest()
  {
    var definition = Registry.StringedInstrumentDefinitions["guitar"];
    var expectedTuning = definition.Tunings.Standard;

    var instrument = new StringedInstrument( definition, 22, expectedTuning );

    instrument.Should()
              .NotBeNull();

    instrument.Definition.Should()
              .Be( definition );

    instrument.PositionCount.Should()
              .Be( 22 );

    instrument.Tuning.Should()
              .Be( expectedTuning );
  }

  [Fact]
  public void CreateWithDefinitionThrowsOnMismatchedStringCountTest()
  {
    var definition = Registry.StringedInstrumentDefinitions["guitar"];
    var act = () => new StringedInstrument( definition, 0, (Tuning?)null );

    act.Should()
       .Throw<ArgumentOutOfRangeException>();
  }

  [Fact]
  public void CreateWithDefinitionThrowsOnNullDefinitionTest()
  {
    var definition = Registry.StringedInstrumentDefinitions["guitar"];
    var expectedTuning = definition.Tunings.Standard;
    var act = () => new StringedInstrument( (StringedInstrumentDefinition)null!, 22, expectedTuning );

    act.Should()
       .Throw<ArgumentNullException>();
  }

  [Fact]
  public void CreateWithNamesDefaultTuningTest()
  {
    var definition = Registry.StringedInstrumentDefinitions["guitar"];
    var expectedTuning = definition.Tunings.Standard;

    var instrument = new StringedInstrument( "guitar", 22, null );

    instrument.Should()
              .NotBeNull();

    instrument.Definition.Should()
              .Be( definition );

    instrument.PositionCount.Should()
              .Be( 22 );

    instrument.Tuning.Should()
              .Be( expectedTuning );
  }

  [Fact]
  public void CreateWithNamesTest()
  {
    var definition = Registry.StringedInstrumentDefinitions["guitar"];
    var expectedTuning = definition.Tunings.Standard;

    var instrument = new StringedInstrument( "guitar", 22, "standard" );

    instrument.Should()
              .NotBeNull();

    instrument.Definition.Should()
              .Be( definition );

    instrument.PositionCount.Should()
              .Be( 22 );

    instrument.Tuning.Should()
              .Be( expectedTuning );
  }

  [Fact]
  public void CreateWithNamesThrowsOnInvalidPositionCountTest()
  {
    var act = () => new StringedInstrument( "guitar", 0, null );

    act.Should()
       .Throw<ArgumentOutOfRangeException>();
  }

  [Fact]
  public void CreateWithNamesThrowsOnNullDefinitionNameTest()
  {
    var act = () => new StringedInstrument( (string)null!, 22, "standard" );

    act.Should()
       .Throw<ArgumentNullException>();
  }

  [Fact]
  public void EqualsContractTest()
  {
    var definition = Registry.StringedInstrumentDefinitions["guitar"];

    object x = new StringedInstrument( definition, 22, (Tuning?)null );
    object y = new StringedInstrument( definition, 22, (Tuning?)null );
    object z = new StringedInstrument( definition, 22, (Tuning?)null );

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
    var definition = Registry.StringedInstrumentDefinitions["guitar"];
    object a = new StringedInstrument( definition, 22, (Tuning?)null );
    object b = new StringedInstrument( Registry.StringedInstrumentDefinitions["bass"], 22, (Tuning?)null );

    a.Equals( b )
     .Should()
     .BeFalse();

    b.Equals( a )
     .Should()
     .BeFalse();

    Equals( a, b )
      .Should()
      .BeFalse();

    Equals( b, a )
      .Should()
      .BeFalse();
  }

  [Fact]
  public void EqualsFailsWithNullTest()
  {
    var definition = Registry.StringedInstrumentDefinitions["guitar"];
    object actual = new StringedInstrument( definition, 22, (Tuning?)null );

    actual.Equals( null )
          .Should()
          .BeFalse();
  }

  [Fact]
  public void EqualsSucceedsWithSameObjectTest()
  {
    var definition = Registry.StringedInstrumentDefinitions["guitar"];
    var actual = new StringedInstrument( definition, 22, (Tuning?)null );

    actual.Equals( actual )
          .Should()
          .BeTrue();
  }

  [Fact]
  public void GetHashcodeTest()
  {
    var definition = Registry.StringedInstrumentDefinitions["guitar"];
    var actual = new StringedInstrument( definition, 22, (Tuning?)null );
    var expected = new StringedInstrument( definition, 22, (Tuning?)null );

    expected.Equals( actual )
            .Should()
            .BeTrue();

    actual.GetHashCode()
          .Should()
          .Be( expected.GetHashCode() );
  }

  [Fact]
  public void GetFingeringCMajorChordPosition0Test()
  {
    var instrument = new StringedInstrument( "guitar", 22, null );

    var chord = Chord.Create( PitchClass.C, Registry.ChordFormulas["Major"] );

    RenderChord( instrument, chord, 0, 4 )
      .Should()
      .Be( "6x 53 42 30 21 10" );
  }

  [Fact]
  public void GetFingeringCMajorChordPosition12Test()
  {
    var instrument = new StringedInstrument( "guitar", 22, null );

    var chord = Chord.Create( PitchClass.C, Registry.ChordFormulas["Major"] );

    RenderChord( instrument, chord, 12, 4 )
      .Should()
      .Be( "6x 515 414 312 213 112" );
  }

  [Fact]
  public void GetFingeringCMajorChordPosition3Test()
  {
    var instrument = new StringedInstrument( "guitar", 22, null );

    var chord = Chord.Create( PitchClass.C, Registry.ChordFormulas["Major"] );

    RenderChord( instrument, chord, 3, 4 )
      .Should()
      .Be( "6x 53 45 35 25 13" );
  }

  [Fact]
  public void GetFingeringCMajorChordPosition5Test()
  {
    var instrument = new StringedInstrument( "guitar", 22, null );

    var chord = Chord.Create( PitchClass.C, Registry.ChordFormulas["Major"] );

    RenderChord( instrument, chord, 5, 4 )
      .Should()
      .Be( "68 57 45 35 25 18" );
  }

  [Fact]
  public void GetFingeringCMajorChordPosition8Test()
  {
    var instrument = new StringedInstrument( "guitar", 22, null );

    var chord = Chord.Create( PitchClass.C, Registry.ChordFormulas["Major"] );

    RenderChord( instrument, chord, 8, 4 )
      .Should()
      .Be( "68 510 410 39 28 18" );
  }

  [Fact]
  public void GetFingeringCMajorEChordPosition0Test()
  {
    var instrument = new StringedInstrument( "guitar", 22, null );

    var chord = Chord.Create( PitchClass.C, Registry.ChordFormulas["Major"] )
                     .GetInversion( 1 );

    RenderChord( instrument, chord, 0, 4 )
      .Should()
      .Be( "60 53 42 30 21 10" );
  }

  [Fact]
  public void GetFingeringCMajorEChordPosition12Test()
  {
    var instrument = new StringedInstrument( "guitar", 22, null );

    var chord = Chord.Create( PitchClass.C, Registry.ChordFormulas["Major"] );

    RenderChord( instrument, chord, 12, 4 )
      .Should()
      .Be( "6x 515 414 312 213 112" );
  }

  [Fact]
  public void GetFingeringCMajorEChordPosition3Test()
  {
    var instrument = new StringedInstrument( "guitar", 22, null );

    var chord = Chord.Create( PitchClass.C, Registry.ChordFormulas["Major"] );

    RenderChord( instrument, chord, 3, 4 )
      .Should()
      .Be( "6x 53 45 35 25 13" );
  }

  [Fact]
  public void GetFingeringCMajorEChordPosition5Test()
  {
    var instrument = new StringedInstrument( "guitar", 22, null );

    var chord = Chord.Create( PitchClass.C, Registry.ChordFormulas["Major"] );

    RenderChord( instrument, chord, 5, 4 )
      .Should()
      .Be( "68 57 45 35 25 18" );
  }

  [Fact]
  public void GetFingeringCMajorEChordPosition8Test()
  {
    var instrument = new StringedInstrument( "guitar", 22, null );

    var chord = Chord.Create( PitchClass.C, Registry.ChordFormulas["Major"] );

    RenderChord( instrument, chord, 8, 4 )
      .Should()
      .Be( "68 510 410 39 28 18" );
  }

  [Fact]
  public void GetFingeringDMajorChordPosition0Test()
  {
    var instrument = new StringedInstrument( "guitar", 22, null );

    var chord = Chord.Create( PitchClass.D, Registry.ChordFormulas["Major"] );

    RenderChord( instrument, chord, 0, 4 )
      .Should()
      .Be( "6x 5x 40 32 23 12" );
  }

  [Fact]
  public void GetFingeringDMajorChordPosition12Test()
  {
    var instrument = new StringedInstrument( "guitar", 22, null );

    var chord = Chord.Create( PitchClass.D, Registry.ChordFormulas["Major"] );

    RenderChord( instrument, chord, 12, 4 )
      .Should()
      .Be( "6x 5x 412 314 215 114" );
  }

  [Fact]
  public void GetFingeringDMajorChordPosition5Test()
  {
    var instrument = new StringedInstrument( "guitar", 22, null );

    var chord = Chord.Create( PitchClass.D, Registry.ChordFormulas["Major"] );

    RenderChord( instrument, chord, 5, 4 )
      .Should()
      .Be( "6x 55 47 37 27 15" );
  }

  [Fact]
  public void GetFingeringDMajorChordPosition7Test()
  {
    var instrument = new StringedInstrument( "guitar", 22, null );

    var chord = Chord.Create( PitchClass.D, Registry.ChordFormulas["Major"] );

    RenderChord( instrument, chord, 7, 4 )
      .Should()
      .Be( "610 59 47 37 27 110" );
  }

  [Fact]
  public void GetFingeringMelodicMinorStartFifthPositionFourPositionSpanTest()
  {
    var instrument = new StringedInstrument( "guitar", 22, null );

    var scale = new Scale( PitchClass.A, "MelodicMinor" );

    RenderScale( instrument, scale, 5, 4 )
      .Should()
      .Be( "65 67 68 55 57 59 46 47 49 35 37 25 27 29 15 17 18" );

    scale = new Scale( PitchClass.G, "MelodicMinor" );

    RenderScale( instrument, scale, 5, 4 )
      .Should()
      .Be( "65 66 68 55 57 59 45 47 48 35 37 25 27 28 15 16 18" );
  }

  [Fact]
  public void GetFingeringMelodicMinorStartOpenFourPositionSpanTest()
  {
    var instrument = new StringedInstrument( "guitar", 22, null );

    var scale = new Scale( PitchClass.A, "MelodicMinor" );

    RenderScale( instrument, scale, 0, 4 )
      .Should()
      .Be( "60 62 64 50 52 53 40 42 44 31 32 20 21 23 10 12 14" );

    scale = new Scale( PitchClass.G, "MelodicMinor" );

    RenderScale( instrument, scale, 0, 4 )
      .Should()
      .Be( "60 62 63 50 51 53 40 42 44 30 32 33 21 23 10 12 13" );
  }

  [Fact]
  public void GetFingeringMinorPentatonicStartFifthPositionFourPositionSpanTest()
  {
    var instrument = new StringedInstrument( "guitar", 22, null );

    var scale = new Scale( PitchClass.A, "MinorPentatonic" );

    RenderScale( instrument, scale, 5, 4 )
      .Should()
      .Be( "65 68 55 57 45 47 35 37 25 28 15 18" );

    scale = new Scale( PitchClass.G, "MinorPentatonic" );

    RenderScale( instrument, scale, 5, 4 )
      .Should()
      .Be( "66 68 55 58 45 48 35 37 26 28 16 18" );
  }

  [Fact]
  public void GetFingeringMinorPentatonicStartOpenFourPositionSpanTest()
  {
    var instrument = new StringedInstrument( "guitar", 22, null );

    var scale = new Scale( PitchClass.A, "MinorPentatonic" );

    RenderScale( instrument, scale, 0, 4 )
      .Should()
      .Be( "60 63 50 53 40 42 30 32 21 23 10 13" );

    scale = new Scale( PitchClass.G, "MinorPentatonic" );

    RenderScale( instrument, scale, 0, 4 )
      .Should()
      .Be( "61 63 51 53 40 43 30 33 21 23 11 13" );
  }

  [Fact]
  public void GetFingeringMustSkipSeveralPositionsForStartingNoteTest()
  {
    var instrument = new StringedInstrument( "guitar", 22, null );

    var scale = new Scale( PitchClass.DSharp, "MinorPentatonic" );

    RenderScale( instrument, scale, 0, 4 )
      .Should()
      .Be( "62 64 51 54 41 44 31 33 22 24 12 14" );
  }

  [Fact]
  public void TestFactoryDefaultTuning()
  {
    var definition = Registry.StringedInstrumentDefinitions["guitar"];
    var instrument = new StringedInstrument( definition, 22, (Tuning?)null );

    instrument.Tuning.Should()
              .Be( definition.Tunings.Standard );
  }

  [Fact]
  public void TestFactoryInvalidPositionCount()
  {
    var act = () => new StringedInstrument( Registry.StringedInstrumentDefinitions["bass"], 0, (Tuning?)null );

    act.Should()
       .Throw<ArgumentOutOfRangeException>();
  }

  [Fact]
  public void TestFactoryNullDefinition()
  {
    var act = () => { new StringedInstrument( (StringedInstrumentDefinition) null!, 22, (Tuning?)null ); };

    act.Should()
       .Throw<ArgumentNullException>();
  }

  [Fact]
  public void TypeSafeEqualsContractTest()
  {
    var definition = Registry.StringedInstrumentDefinitions["guitar"];

    var x = new StringedInstrument( definition, 22, (Tuning?)null );
    var y = new StringedInstrument( definition, 22, (Tuning?)null );
    var z = new StringedInstrument( definition, 22, (Tuning?)null );

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
    var definition = Registry.StringedInstrumentDefinitions["guitar"];
    var a = new StringedInstrument( definition, 22, (Tuning?)null );
    var b = new StringedInstrument( Registry.StringedInstrumentDefinitions["bass"], 22, (Tuning?)null );

    a.Equals( b )
     .Should()
     .BeFalse();

    b.Equals( a )
     .Should()
     .BeFalse();

    Equals( a, b )
      .Should()
      .BeFalse();

    Equals( b, a )
      .Should()
      .BeFalse();
  }

  [Fact]
  public void TypeSafeEqualsFailsWithNullTest()
  {
    var definition = Registry.StringedInstrumentDefinitions["guitar"];
    var actual = new StringedInstrument( definition, 22, (Tuning?)null );

    actual.Equals( null )
          .Should()
          .BeFalse();
  }

  #endregion

  #region Implementation

  private static string RenderScale(
    StringedInstrument instrument,
    Scale scale,
    int startPosition,
    int positionSpan )
  {
    var result = string.Join(
      " ",
      Array.ConvertAll(
        [.. instrument.GetFingering( scale, startPosition, positionSpan )],
        f => f.ToString()
      )
    );

    return result;
  }

  private static string RenderChord(
    StringedInstrument instrument,
    Chord chord,
    int startPosition,
    int positionSpan )
  {
    var result = string.Join(
      " ",
      Array.ConvertAll(
        [.. instrument.GetFingering( chord, startPosition, positionSpan )],
        f => f.ToString()
      )
    );

    return result;
  }

  #endregion
}
