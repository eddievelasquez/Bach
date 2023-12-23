// Module Name: PitchTest.cs
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

public sealed class PitchTest
{
  #region Public Methods

  [Fact]
  public void CompareToContractTest()
  {
    {
      var a = Pitch.Create( NoteName.A, Accidental.Natural, 1 );
      ( a.CompareTo( a ) == 0 ).Should()
                               .BeTrue();

      var b = Pitch.Create( NoteName.A, Accidental.Natural, 1 );
      ( a.CompareTo( b ) == 0 ).Should()
                               .BeTrue();
      ( b.CompareTo( a ) == 0 ).Should()
                               .BeTrue();

      var c = Pitch.Create( NoteName.A, Accidental.Natural, 1 );
      ( b.CompareTo( c ) == 0 ).Should()
                               .BeTrue();
      ( a.CompareTo( c ) == 0 ).Should()
                               .BeTrue();
    }

    {
      var a = Pitch.Create( NoteName.C, Accidental.Natural, 1 );
      var b = Pitch.Create( NoteName.D, Accidental.Natural, 1 );

      ( -b.CompareTo( a ) ).Should()
                           .Be( a.CompareTo( b ) );

      var c = Pitch.Create( NoteName.E, Accidental.Natural, 1 );
      ( a.CompareTo( b ) < 0 ).Should()
                              .BeTrue();
      ( b.CompareTo( c ) < 0 ).Should()
                              .BeTrue();
      ( a.CompareTo( c ) < 0 ).Should()
                              .BeTrue();
    }
  }

  [Fact]
  public void CompareToTest()
  {
    var a1 = Pitch.Create( NoteName.A, Accidental.Natural, 1 );
    var aSharp1 = Pitch.Create( NoteName.A, Accidental.Sharp, 1 );
    var aFlat1 = Pitch.Create( NoteName.A, Accidental.Flat, 1 );
    var a2 = Pitch.Create( NoteName.A, Accidental.Natural, 2 );
    var aSharp2 = Pitch.Create( NoteName.A, Accidental.Sharp, 2 );
    var aFlat2 = Pitch.Create( NoteName.A, Accidental.Flat, 2 );

    ( a1.CompareTo( a1 ) == 0 ).Should()
                               .BeTrue();
    ( a1.CompareTo( aSharp1 ) < 0 ).Should()
                                   .BeTrue();
    ( a1.CompareTo( aFlat1 ) > 0 ).Should()
                                  .BeTrue();
    ( a1.CompareTo( a2 ) < 0 ).Should()
                              .BeTrue();
    ( a1.CompareTo( aFlat2 ) < 0 ).Should()
                                  .BeTrue();
    ( a1.CompareTo( aSharp2 ) < 0 ).Should()
                                   .BeTrue();

    var c1 = Pitch.Create( NoteName.C, Accidental.Natural, 1 );
    ( a1.CompareTo( c1 ) > 0 ).Should()
                              .BeTrue();
    ( c1.CompareTo( a1 ) < 0 ).Should()
                              .BeTrue();
  }

  [Fact]
  public void ComparisonOperatorsTest()
  {
    var a = Pitch.Create( NoteName.A, Accidental.Natural, 1 );
    var b = Pitch.Create( NoteName.B, Accidental.Natural, 1 );

    ( b > a ).Should()
             .BeTrue();
    ( b >= a ).Should()
              .BeTrue();
    ( b < a ).Should()
             .BeFalse();
    ( b <= a ).Should()
              .BeFalse();
  }

  [Fact]
  public void CreateWithNoteTest()
  {
    var target = Pitch.Create( PitchClass.A, 1 );
    target.PitchClass.Should()
          .Be( PitchClass.A );
    target.Octave.Should()
          .Be( 1 );

    var act1 = () => Pitch.Create(
      PitchClass.Create( NoteName.C, Accidental.Flat ),
      Pitch.MinOctave
    );
    act1.Should()
        .Throw<ArgumentOutOfRangeException>();

    var act2 = () => Pitch.Create( PitchClass.Create( NoteName.C, Accidental.DoubleFlat ), Pitch.MinOctave );

    act2.Should()
        .Throw<ArgumentOutOfRangeException>();

    var act3 = () => Pitch.Create(
      PitchClass.Create( NoteName.B, Accidental.Sharp ),
      Pitch.MaxOctave
    );
    act3.Should()
        .Throw<ArgumentOutOfRangeException>();

    var act4 = () => Pitch.Create( PitchClass.Create( NoteName.B, Accidental.DoubleSharp ), Pitch.MaxOctave );
    act4.Should()
        .Throw<ArgumentOutOfRangeException>();
  }

  [Fact]
  public void CreateWithToneAndAccidentalTest()
  {
    var target = Pitch.Create( NoteName.A, Accidental.Natural, 1 );
    target.PitchClass.NoteName.Should()
          .Be( NoteName.A );
    target.PitchClass.Accidental.Should()
          .Be( Accidental.Natural );
    target.Octave.Should()
          .Be( 1 );

    var act1 = () => Pitch.Create( NoteName.C, Accidental.Flat, Pitch.MinOctave );
    act1.Should()
        .Throw<ArgumentOutOfRangeException>();
    var act2 = () => Pitch.Create( NoteName.C, Accidental.DoubleFlat, Pitch.MinOctave );
    act2.Should()
        .Throw<ArgumentOutOfRangeException>();

    var act3 = () => Pitch.Create( NoteName.B, Accidental.Sharp, Pitch.MaxOctave );
    act3.Should()
        .Throw<ArgumentOutOfRangeException>();

    var act4 = () => Pitch.Create( NoteName.B, Accidental.DoubleSharp, Pitch.MaxOctave );
    act4.Should()
        .Throw<ArgumentOutOfRangeException>();
  }

  [Fact]
  public void EqualsContractTest()
  {
    object x = Pitch.Create( NoteName.A, Accidental.Natural, 1 );
    object y = Pitch.Create( NoteName.A, Accidental.Natural, 1 );
    object z = Pitch.Create( NoteName.A, Accidental.Natural, 1 );

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
    object actual = Pitch.Create( NoteName.A, Accidental.Natural, 1 );
    actual.Equals( int.MinValue )
          .Should()
          .BeFalse();
  }

  [Fact]
  public void EqualsFailsWithNullTest()
  {
    object actual = Pitch.Create( NoteName.A, Accidental.Natural, 1 );
    actual.Equals( null )
          .Should()
          .BeFalse();
  }

  [Fact]
  public void EqualsSucceedsWithSameObjectTest()
  {
    var actual = Pitch.Create( NoteName.A, Accidental.Natural, 1 );
    actual.Equals( actual )
          .Should()
          .BeTrue();
  }

  [Fact]
  public void FrequencyTest()
  {
    Math.Round(
          Pitch.Parse( "A4" )
               .Frequency,
          2
        )
        .Should()
        .Be( 440.0 );
    Math.Round(
          Pitch.Parse( "C5" )
               .Frequency,
          2
        )
        .Should()
        .Be( 523.25 );
    Math.Round(
          Pitch.Parse( "F4" )
               .Frequency,
          2
        )
        .Should()
        .Be( 349.23 );
    Math.Round(
          Pitch.Parse( "A5" )
               .Frequency,
          2
        )
        .Should()
        .Be( 880.0 );
  }

  [Fact]
  public void GetHashcodeTest()
  {
    var actual = Pitch.Create( NoteName.A, Accidental.Natural, 1 );
    var expected = Pitch.Create( NoteName.A, Accidental.Natural, 1 );
    expected.Equals( actual )
            .Should()
            .BeTrue();
    actual.GetHashCode()
          .Should()
          .Be( expected.GetHashCode() );
  }

  [Fact]
  public void MaxTest()
  {
    Pitch.Max( Pitch.Parse( "A4" ), Pitch.Parse( "B4" ) )
         .Should()
         .Be( Pitch.Parse( "B4" ) );
    Pitch.Max( Pitch.Parse( "B4" ), Pitch.Parse( "A4" ) )
         .Should()
         .Be( Pitch.Parse( "B4" ) );
  }

  [Fact]
  public void MidiTest()
  {
    Pitch.Parse( "C0" )
         .Midi.Should()
         .Be( 12 );
    Pitch.Parse( "C1" )
         .Midi.Should()
         .Be( 24 );
    Pitch.Parse( "C2" )
         .Midi.Should()
         .Be( 36 );
    Pitch.Parse( "C3" )
         .Midi.Should()
         .Be( 48 );
    Pitch.Parse( "C4" )
         .Midi.Should()
         .Be( 60 );
    Pitch.Parse( "C5" )
         .Midi.Should()
         .Be( 72 );
    Pitch.Parse( "C6" )
         .Midi.Should()
         .Be( 84 );
    Pitch.Parse( "C7" )
         .Midi.Should()
         .Be( 96 );
    Pitch.Parse( "C8" )
         .Midi.Should()
         .Be( 108 );
    Pitch.Parse( "C9" )
         .Midi.Should()
         .Be( 120 );
    Pitch.Parse( "G9" )
         .Midi.Should()
         .Be( 127 );
    var act = () => Pitch.CreateFromMidi( 11 );
    act.Should()
       .Throw<ArgumentOutOfRangeException>();
  }

  [Fact]
  public void MinTest()
  {
    Pitch.Min( Pitch.Parse( "A4" ), Pitch.Parse( "B4" ) )
         .Should()
         .Be( Pitch.Parse( "A4" ) );
    Pitch.Min( Pitch.Parse( "B4" ), Pitch.Parse( "A4" ) )
         .Should()
         .Be( Pitch.Parse( "A4" ) );
  }

  [Fact]
  public void op_AdditionIntTest()
  {
    var c2 = Pitch.Create( NoteName.C, Accidental.Natural, 2 );

    ( c2 + 1 ).Should()
              .Be( Pitch.Create( NoteName.C, Accidental.Sharp, 2 ) );
    ( c2 + -1 ).Should()
               .Be( Pitch.Create( NoteName.B, Accidental.Natural, 1 ) );
    ( c2 + 2 ).Should()
              .Be( Pitch.Create( NoteName.D, Accidental.Natural, 2 ) );
    ( c2 + -2 ).Should()
               .Be( Pitch.Create( NoteName.A, Accidental.Sharp, 1 ) );
  }

  [Fact]
  public void op_DecrementTest()
  {
    var c2 = Pitch.Create( NoteName.C, Accidental.Natural, 2 );

    ( --c2 ).Should()
            .Be( Pitch.Create( NoteName.B, Accidental.Natural, 1 ) );
    ( --c2 ).Should()
            .Be( Pitch.Create( NoteName.A, Accidental.Sharp, 1 ) );
  }

  [Fact]
  public void op_EqualityTest()
  {
    var a = Pitch.Create( NoteName.A, Accidental.Natural, 1 );
    var b = Pitch.Create( NoteName.A, Accidental.Natural, 1 );
    var c = Pitch.Create( NoteName.B, Accidental.Natural, 1 );

    ( a == b ).Should()
              .BeTrue();
    ( a == c ).Should()
              .BeFalse();
    ( b == c ).Should()
              .BeFalse();
  }

  [Fact]
  public void op_IncrementTest()
  {
    var c2 = Pitch.Create( NoteName.C, Accidental.Natural, 2 );

    ( ++c2 ).Should()
            .Be( Pitch.Create( NoteName.C, Accidental.Sharp, 2 ) );
    ( ++c2 ).Should()
            .Be( Pitch.Create( NoteName.D, Accidental.Natural, 2 ) );
  }

  [Fact]
  public void op_InequalityTest()
  {
    var a = Pitch.Create( NoteName.A, Accidental.Natural, 1 );
    var b = Pitch.Create( NoteName.A, Accidental.Natural, 1 );
    var c = Pitch.Create( NoteName.B, Accidental.Natural, 1 );

    ( a != c ).Should()
              .BeTrue();
    ( b != c ).Should()
              .BeTrue();
    ( a != b ).Should()
              .BeFalse();
  }

  [Fact]
  public void op_SubtractionIntTest()
  {
    var c2 = Pitch.Create( NoteName.C, Accidental.Natural, 2 );

    ( c2 - 1 ).Should()
              .Be( Pitch.Create( NoteName.B, Accidental.Natural, 1 ) );
    ( c2 - 2 ).Should()
              .Be( Pitch.Create( NoteName.A, Accidental.Sharp, 1 ) );
  }

  [Fact]
  public void op_SubtractionNoteTest()
  {
    var cDoubleFlat2 = Pitch.Create( NoteName.C, Accidental.DoubleFlat, 2 );
    var cFlat2 = Pitch.Create( NoteName.C, Accidental.Flat, 2 );
    var c2 = Pitch.Create( NoteName.C, Accidental.Natural, 2 );
    var cSharp2 = Pitch.Create( NoteName.C, Accidental.Sharp, 2 );
    var cDoubleSharp2 = Pitch.Create( NoteName.C, Accidental.DoubleSharp, 2 );

    // Test interval with same pitches in the same octave with different accidentals
    ( cDoubleFlat2 - cDoubleFlat2 ).Should()
                                   .Be( 0 );
    ( cDoubleFlat2 - cFlat2 ).Should()
                             .Be( -1 );
    ( cDoubleFlat2 - c2 ).Should()
                         .Be( -2 );
    ( cDoubleFlat2 - cSharp2 ).Should()
                              .Be( -3 );
    ( cDoubleFlat2 - cDoubleSharp2 ).Should()
                                    .Be( -4 );
    ( cFlat2 - cDoubleFlat2 ).Should()
                             .Be( 1 );
    ( c2 - cDoubleFlat2 ).Should()
                         .Be( 2 );
    ( cSharp2 - cDoubleFlat2 ).Should()
                              .Be( 3 );
    ( cDoubleSharp2 - cDoubleFlat2 ).Should()
                                    .Be( 4 );

    var c3 = Pitch.Create( NoteName.C, Accidental.Natural, 3 );
    ( c2 - c3 ).Should()
               .Be( -12 );
    ( c3 - c2 ).Should()
               .Be( 12 );
  }

  [Fact]
  public void ParseTest()
  {
    var act1 = () => Pitch.Parse( "C$4" );
    act1.Should()
        .Throw<FormatException>();
    var act2 = () => { Pitch.Parse( "A9" ); };
    act2.Should()
        .Throw<ArgumentOutOfRangeException>();
  }

  [Fact]
  public void PitchIntervalAdditionTest()
  {
    ( Pitch.Parse( "C4" ) + Interval.MajorThird ).Should()
                                                 .Be( Pitch.Parse( "E4" ) );
    ( Pitch.Parse( "C#4" ) + Interval.MinorThird ).Should()
                                                  .Be( Pitch.Parse( "E4" ) );
    ( Pitch.Parse( "D4" ) + Interval.MinorThird ).Should()
                                                 .Be( Pitch.Parse( "F4" ) );
    ( Pitch.Parse( "D4" ) + Interval.Fourth ).Should()
                                             .Be( Pitch.Parse( "G4" ) );
    ( Pitch.Parse( "E4" ) + Interval.Fourth ).Should()
                                             .Be( Pitch.Parse( "A4" ) );
    ( Pitch.Parse( "Eb4" ) + Interval.Fourth ).Should()
                                              .Be( Pitch.Parse( "Ab4" ) );
    ( Pitch.Parse( "Eb4" ) + Interval.AugmentedThird ).Should()
                                                      .Be( Pitch.Parse( "G#4" ) );
    ( Pitch.Parse( "F4" ) + Interval.MajorSixth ).Should()
                                                 .Be( Pitch.Parse( "D5" ) );
    ( Pitch.Parse( "G4" ) + Interval.Fifth ).Should()
                                            .Be( Pitch.Parse( "D5" ) );
    ( Pitch.Parse( "F4" ) + Interval.Fifth ).Should()
                                            .Be( Pitch.Parse( "C5" ) );
    ( Pitch.Parse( "A4" ) + Interval.Fifth ).Should()
                                            .Be( Pitch.Parse( "E5" ) );
    ( Pitch.Parse( "Ab4" ) + Interval.Fifth ).Should()
                                             .Be( Pitch.Parse( "Eb5" ) );
    ( Pitch.Parse( "G#4" ) + Interval.DiminishedSixth ).Should()
                                                       .Be( Pitch.Parse( "Eb5" ) );
    ( Pitch.Parse( "F#4" ) + Interval.AugmentedFourth ).Should()
                                                       .Be( Pitch.Parse( "C5" ) );
    ( Pitch.Parse( "Gb4" ) + Interval.DiminishedFifth ).Should()
                                                       .Be( Pitch.Parse( "C5" ) );
    ( Pitch.Parse( "C4" ) + Interval.AugmentedSecond ).Should()
                                                      .Be( Pitch.Parse( "D#4" ) );
    ( Pitch.Parse( "C4" ) + Interval.DiminishedFifth ).Should()
                                                      .Be( Pitch.Parse( "F#4" ) );
    ( Pitch.Parse( "C4" ) + Interval.AugmentedFourth ).Should()
                                                      .Be( Pitch.Parse( "Gb4" ) );
    ( Pitch.Parse( "D#4" ) + Interval.DiminishedSeventh ).Should()
                                                         .Be( Pitch.Parse( "C5" ) );
    ( Pitch.Parse( "D#4" ) + Interval.DiminishedThird ).Should()
                                                       .Be( Pitch.Parse( "F4" ) );
    ( Pitch.Parse( "D##4" ) + Interval.DiminishedFourth ).Should()
                                                         .Be( Pitch.Parse( "G#4" ) );
  }

  [Fact]
  public void ToStringTest()
  {
    var target = Pitch.Create( NoteName.A, Accidental.DoubleFlat, 1 );
    target.ToString()
          .Should()
          .Be( "Abb1" );

    target = Pitch.Create( NoteName.A, Accidental.Flat, 1 );
    target.ToString()
          .Should()
          .Be( "Ab1" );

    target = Pitch.Create( NoteName.A, Accidental.Natural, 1 );
    target.ToString()
          .Should()
          .Be( "A1" );

    target = Pitch.Create( NoteName.A, Accidental.Sharp, 1 );
    target.ToString()
          .Should()
          .Be( "A#1" );

    target = Pitch.Create( NoteName.A, Accidental.DoubleSharp, 1 );
    target.ToString()
          .Should()
          .Be( "A##1" );
  }

  [Fact]
  public void TryParseTest()
  {
    Pitch.TryParse( "C4", out var actual )
         .Should()
         .BeTrue();
    actual.Should()
          .Be( Pitch.Create( NoteName.C, Accidental.Natural, 4 ) );

    Pitch.TryParse( "C#4", out actual )
         .Should()
         .BeTrue();
    actual.Should()
          .Be( Pitch.Create( NoteName.C, Accidental.Sharp, 4 ) );

    Pitch.TryParse( "C##4", out actual )
         .Should()
         .BeTrue();
    actual.Should()
          .Be( Pitch.Create( NoteName.C, Accidental.DoubleSharp, 4 ) );

    Pitch.TryParse( "Cb4", out actual )
         .Should()
         .BeTrue();
    actual.Should()
          .Be( Pitch.Create( NoteName.C, Accidental.Flat, 4 ) );

    Pitch.TryParse( "Cbb4", out actual )
         .Should()
         .BeTrue();
    actual.Should()
          .Be( Pitch.Create( NoteName.C, Accidental.DoubleFlat, 4 ) );

    Pitch.TryParse( "C2", out actual )
         .Should()
         .BeTrue();
    actual.Should()
          .Be( Pitch.Create( NoteName.C, Accidental.Natural, 2 ) );

    Pitch.TryParse( "C#2", out actual )
         .Should()
         .BeTrue();
    actual.Should()
          .Be( Pitch.Create( NoteName.C, Accidental.Sharp, 2 ) );

    Pitch.TryParse( "C##2", out actual )
         .Should()
         .BeTrue();
    actual.Should()
          .Be( Pitch.Create( NoteName.C, Accidental.DoubleSharp, 2 ) );

    Pitch.TryParse( "Cb2", out actual )
         .Should()
         .BeTrue();
    actual.Should()
          .Be( Pitch.Create( NoteName.C, Accidental.Flat, 2 ) );

    Pitch.TryParse( "Cbb2", out actual )
         .Should()
         .BeTrue();
    actual.Should()
          .Be( Pitch.Create( NoteName.C, Accidental.DoubleFlat, 2 ) );

    Pitch.TryParse( "60", out actual )
         .Should()
         .BeTrue();
    actual.Should()
          .Be( Pitch.Create( NoteName.C, Accidental.Natural, 4 ) );

    Pitch.TryParse( "H", out actual )
         .Should()
         .BeFalse();
    actual.IsValid.Should()
          .BeFalse();

    Pitch.TryParse( "C!", out actual )
         .Should()
         .BeFalse();
    actual.IsValid.Should()
          .BeFalse();

    Pitch.TryParse( "C#-1", out actual )
         .Should()
         .BeFalse();
    actual.IsValid.Should()
          .BeFalse();

    Pitch.TryParse( "C#10", out actual )
         .Should()
         .BeFalse();
    actual.IsValid.Should()
          .BeFalse();

    Pitch.TryParse( "C#b2", out actual )
         .Should()
         .BeFalse();
    actual.IsValid.Should()
          .BeFalse();

    Pitch.TryParse( "Cb#2", out actual )
         .Should()
         .BeFalse();
    actual.IsValid.Should()
          .BeFalse();

    Pitch.TryParse( null!, out _ )
         .Should()
         .BeFalse();
    Pitch.TryParse( "", out _ )
         .Should()
         .BeFalse();
    Pitch.TryParse( "256", out _ )
         .Should()
         .BeFalse();
    Pitch.TryParse( "-1", out _ )
         .Should()
         .BeFalse();
    Pitch.TryParse( "1X", out _ )
         .Should()
         .BeFalse();
  }

  [Fact]
  public void TypeSafeEqualsContractTest()
  {
    var x = Pitch.Create( NoteName.A, Accidental.Natural, 1 );
    var y = Pitch.Create( NoteName.A, Accidental.Natural, 1 );
    var z = Pitch.Create( NoteName.A, Accidental.Natural, 1 );

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
    var actual = Pitch.Create( NoteName.A, Accidental.Natural, 1 );

    // ReSharper disable once SuspiciousTypeConversion.Global
    actual.Equals( int.MinValue )
          .Should()
          .BeFalse();
  }

  [Fact]
  public void TypeSafeEqualsFailsWithNullTest()
  {
    var actual = Pitch.Create( NoteName.A, Accidental.Natural, 1 );
    actual.Equals( null )
          .Should()
          .BeFalse();
  }

  #endregion
}
