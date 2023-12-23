// Module Name: PitchClassTest.cs
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

public sealed class PitchClassTest
{
  #region Public Methods

  [Fact]
  public void AddTest()
  {
    AddTestImpl( PitchClass.C, 1, PitchClass.CSharp, PitchClass.DFlat );
    AddTestImpl( PitchClass.C, 2, PitchClass.D );
    AddTestImpl( PitchClass.C, 3, PitchClass.DSharp, PitchClass.EFlat );
    AddTestImpl( PitchClass.C, 4, PitchClass.E );
    AddTestImpl( PitchClass.C, 5, PitchClass.F );
    AddTestImpl( PitchClass.C, 6, PitchClass.FSharp, PitchClass.GFlat );
    AddTestImpl( PitchClass.C, 7, PitchClass.G );
    AddTestImpl( PitchClass.C, 8, PitchClass.GSharp, PitchClass.AFlat );
    AddTestImpl( PitchClass.C, 9, PitchClass.A );
    AddTestImpl( PitchClass.C, 10, PitchClass.ASharp, PitchClass.BFlat );
    AddTestImpl( PitchClass.C, 11, PitchClass.B );
    AddTestImpl( PitchClass.C, 12, PitchClass.C );
  }

  [Fact]
  public void ArithmeticOperatorsTest()
  {
    ( PitchClass.B + 1 ).Should()
                        .Be( PitchClass.C );
    ( PitchClass.C + 12 ).Should()
                         .Be( PitchClass.C );
    ( PitchClass.C - 1 ).Should()
                        .Be( PitchClass.B );
    ( PitchClass.C - 12 ).Should()
                         .Be( PitchClass.C );

    var pitchClass = PitchClass.B;
    ( pitchClass++ ).Should()
                    .Be( PitchClass.B );
    pitchClass.Should()
              .Be( PitchClass.C );
    ( ++pitchClass ).Should()
                    .Be( PitchClass.CSharp );

    pitchClass = PitchClass.C;
    ( pitchClass-- ).Should()
                    .Be( PitchClass.C );
    pitchClass.Should()
              .Be( PitchClass.B );
    ( --pitchClass ).Should()
                    .Be( PitchClass.BFlat );
  }

  [Fact]
  public void CompareToTest()
  {
    ( PitchClass.C.CompareTo( PitchClass.C ) == 0 ).Should()
                                                   .BeTrue();
    ( PitchClass.C.CompareTo( PitchClass.D ) < 0 ).Should()
                                                  .BeTrue();
    ( PitchClass.D.CompareTo( PitchClass.C ) > 0 ).Should()
                                                  .BeTrue();
    ( PitchClass.C.CompareTo( PitchClass.B ) < 0 ).Should()
                                                  .BeTrue();
    ( PitchClass.B.CompareTo( PitchClass.C ) > 0 ).Should()
                                                  .BeTrue();
  }

  [Fact]
  public void ConstructorTest()
  {
    ConstructorTestImpl( NoteName.C, Accidental.DoubleFlat );
    ConstructorTestImpl( NoteName.C, Accidental.Flat );
    ConstructorTestImpl( NoteName.C, Accidental.Natural );
    ConstructorTestImpl( NoteName.C, Accidental.Sharp );
    ConstructorTestImpl( NoteName.C, Accidental.DoubleSharp );

    ConstructorTestImpl( NoteName.D, Accidental.DoubleFlat );
    ConstructorTestImpl( NoteName.D, Accidental.Flat );
    ConstructorTestImpl( NoteName.D, Accidental.Natural );
    ConstructorTestImpl( NoteName.D, Accidental.Sharp );
    ConstructorTestImpl( NoteName.D, Accidental.DoubleSharp );

    ConstructorTestImpl( NoteName.E, Accidental.DoubleFlat );
    ConstructorTestImpl( NoteName.E, Accidental.Flat );
    ConstructorTestImpl( NoteName.E, Accidental.Natural );
    ConstructorTestImpl( NoteName.E, Accidental.Sharp );
    ConstructorTestImpl( NoteName.E, Accidental.DoubleSharp );

    ConstructorTestImpl( NoteName.F, Accidental.DoubleFlat );
    ConstructorTestImpl( NoteName.F, Accidental.Flat );
    ConstructorTestImpl( NoteName.F, Accidental.Natural );
    ConstructorTestImpl( NoteName.F, Accidental.Sharp );
    ConstructorTestImpl( NoteName.F, Accidental.DoubleSharp );

    ConstructorTestImpl( NoteName.G, Accidental.DoubleFlat );
    ConstructorTestImpl( NoteName.G, Accidental.Flat );
    ConstructorTestImpl( NoteName.G, Accidental.Natural );
    ConstructorTestImpl( NoteName.G, Accidental.Sharp );
    ConstructorTestImpl( NoteName.G, Accidental.DoubleSharp );

    ConstructorTestImpl( NoteName.A, Accidental.DoubleFlat );
    ConstructorTestImpl( NoteName.A, Accidental.Flat );
    ConstructorTestImpl( NoteName.A, Accidental.Natural );
    ConstructorTestImpl( NoteName.A, Accidental.Sharp );
    ConstructorTestImpl( NoteName.A, Accidental.DoubleSharp );

    ConstructorTestImpl( NoteName.B, Accidental.DoubleFlat );
    ConstructorTestImpl( NoteName.B, Accidental.Flat );
    ConstructorTestImpl( NoteName.B, Accidental.Natural );
    ConstructorTestImpl( NoteName.B, Accidental.Sharp );
    ConstructorTestImpl( NoteName.B, Accidental.DoubleSharp );
  }

  [Fact]
  public void EqualsTest()
  {
    object actual = PitchClass.Create( NoteName.C );
    PitchClass.C.Equals( actual )
              .Should()
              .BeTrue();
    PitchClass.C.Equals( null )
              .Should()
              .BeFalse();
  }

  [Fact]
  public void GetEnharmonicTest()
  {
    EnharmonicTestImpl( PitchClass.C, "Dbb", "B#" );
    EnharmonicTestImpl( PitchClass.CSharp, "Db", "B##" );
    EnharmonicTestImpl( PitchClass.D, "Ebb", "C##" );
    EnharmonicTestImpl( PitchClass.DSharp, "Fbb", "Eb" );
    EnharmonicTestImpl( PitchClass.E, "Fb", "D##" );
    EnharmonicTestImpl( PitchClass.F, "Gbb", "E#" );
    EnharmonicTestImpl( PitchClass.FSharp, "Gb", "E##" );
    EnharmonicTestImpl( PitchClass.G, "Abb", "F##" );
    EnharmonicTestImpl( PitchClass.GSharp, "Ab" );
    EnharmonicTestImpl( PitchClass.A, "Bbb", "G##" );
    EnharmonicTestImpl( PitchClass.ASharp, "Cbb", "Bb" );
    EnharmonicTestImpl( PitchClass.B, "Cb", "A##" );

    // Not enharmonic
    NotEnharmonicTestImpl( PitchClass.C, NoteName.E, NoteName.G );
    NotEnharmonicTestImpl( PitchClass.CSharp, NoteName.E, NoteName.G );
    NotEnharmonicTestImpl( PitchClass.D, NoteName.F, NoteName.C );
    NotEnharmonicTestImpl( PitchClass.DSharp, NoteName.G, NoteName.D );
    NotEnharmonicTestImpl( PitchClass.E, NoteName.G, NoteName.D );
    NotEnharmonicTestImpl( PitchClass.F, NoteName.A, NoteName.E );
    NotEnharmonicTestImpl( PitchClass.FSharp, NoteName.A, NoteName.E );
    NotEnharmonicTestImpl( PitchClass.G, NoteName.B, NoteName.F );
    NotEnharmonicTestImpl( PitchClass.GSharp, NoteName.B, NoteName.G );
    NotEnharmonicTestImpl( PitchClass.A, NoteName.C, NoteName.G );
    NotEnharmonicTestImpl( PitchClass.ASharp, NoteName.D, NoteName.A );
    NotEnharmonicTestImpl( PitchClass.B, NoteName.D, NoteName.A );
  }

  [Fact]
  public void LogicalOperatorsTest()
  {
    ( PitchClass.C == PitchClass.Create( NoteName.B, Accidental.Sharp ) ).Should()
                                                                         .BeTrue();
    ( PitchClass.C != PitchClass.B ).Should()
                                    .BeTrue();
    ( PitchClass.C < PitchClass.B ).Should()
                                   .BeTrue();
    ( PitchClass.C <= PitchClass.B ).Should()
                                    .BeTrue();
    ( PitchClass.D > PitchClass.C ).Should()
                                   .BeTrue();
    ( PitchClass.D >= PitchClass.C ).Should()
                                    .BeTrue();
  }

  [Fact]
  public void NextTest()
  {
    NextTestImpl( PitchClass.C, PitchClass.CSharp, PitchClass.DFlat );
    NextTestImpl( PitchClass.CSharp, PitchClass.D );
    NextTestImpl( PitchClass.DFlat, PitchClass.D );
    NextTestImpl( PitchClass.D, PitchClass.DSharp, PitchClass.EFlat );
    NextTestImpl( PitchClass.DSharp, PitchClass.E );
    NextTestImpl( PitchClass.EFlat, PitchClass.E );
    NextTestImpl( PitchClass.E, PitchClass.F );
    NextTestImpl( PitchClass.F, PitchClass.FSharp, PitchClass.GFlat );
    NextTestImpl( PitchClass.FSharp, PitchClass.G );
    NextTestImpl( PitchClass.GFlat, PitchClass.G );
    NextTestImpl( PitchClass.G, PitchClass.GSharp, PitchClass.AFlat );
    NextTestImpl( PitchClass.GSharp, PitchClass.A );
    NextTestImpl( PitchClass.AFlat, PitchClass.A );
    NextTestImpl( PitchClass.A, PitchClass.ASharp, PitchClass.BFlat );
    NextTestImpl( PitchClass.ASharp, PitchClass.B );
    NextTestImpl( PitchClass.BFlat, PitchClass.B );
    NextTestImpl( PitchClass.B, PitchClass.C );

    NextTestImpl( PitchClass.Create( NoteName.C, Accidental.DoubleSharp ), PitchClass.DSharp, PitchClass.EFlat );
    NextTestImpl( PitchClass.Create( NoteName.E, Accidental.DoubleSharp ), PitchClass.G );
    NextTestImpl( PitchClass.Create( NoteName.B, Accidental.DoubleSharp ), PitchClass.D );
  }

  [Fact]
  public void NoteIntervalAdditionTest()
  {
    ( PitchClass.C + Interval.MajorThird ).Should()
                                          .Be( PitchClass.E );
    ( PitchClass.CSharp + Interval.MinorThird ).Should()
                                               .Be( PitchClass.E );
    ( PitchClass.D + Interval.MinorThird ).Should()
                                          .Be( PitchClass.F );
    ( PitchClass.D + Interval.Fourth ).Should()
                                      .Be( PitchClass.G );
    ( PitchClass.E + Interval.Fourth ).Should()
                                      .Be( PitchClass.A );
    ( PitchClass.EFlat + Interval.Fourth ).Should()
                                          .Be( PitchClass.AFlat );
    ( PitchClass.EFlat + Interval.AugmentedThird ).Should()
                                                  .Be( PitchClass.GSharp );
    ( PitchClass.F + Interval.MajorSixth ).Should()
                                          .Be( PitchClass.D );
    ( PitchClass.G + Interval.Fifth ).Should()
                                     .Be( PitchClass.D );
    ( PitchClass.F + Interval.Fifth ).Should()
                                     .Be( PitchClass.C );
    ( PitchClass.A + Interval.Fifth ).Should()
                                     .Be( PitchClass.E );
    ( PitchClass.AFlat + Interval.Fifth ).Should()
                                         .Be( PitchClass.EFlat );
    ( PitchClass.GSharp + Interval.DiminishedSixth ).Should()
                                                    .Be( PitchClass.EFlat );
    ( PitchClass.FSharp + Interval.AugmentedFourth ).Should()
                                                    .Be( PitchClass.C );
    ( PitchClass.GFlat + Interval.DiminishedFifth ).Should()
                                                   .Be( PitchClass.C );
    ( PitchClass.C + Interval.AugmentedSecond ).Should()
                                               .Be( PitchClass.DSharp );
    ( PitchClass.C + Interval.DiminishedFifth ).Should()
                                               .Be( PitchClass.FSharp );
    ( PitchClass.C + Interval.AugmentedFourth ).Should()
                                               .Be( PitchClass.GFlat );
    ( PitchClass.DSharp + Interval.DiminishedSeventh ).Should()
                                                      .Be( PitchClass.C );
    ( PitchClass.DSharp + Interval.DiminishedThird ).Should()
                                                    .Be( PitchClass.F );
    ( PitchClass.Parse( "D##" ) + Interval.DiminishedFourth ).Should()
                                                             .Be( PitchClass.GSharp );
  }

  [Fact]
  public void NoteSubtractIntervalTest()
  {
    ( PitchClass.F - Interval.AugmentedFourth ).Should()
                                               .Be( PitchClass.Parse( "Cb" ) );
    ( PitchClass.E - Interval.MajorThird ).Should()
                                          .Be( PitchClass.C );
    ( PitchClass.E - Interval.MinorThird ).Should()
                                          .Be( PitchClass.CSharp );
    ( PitchClass.F - Interval.MinorThird ).Should()
                                          .Be( PitchClass.D );
    ( PitchClass.G - Interval.Fourth ).Should()
                                      .Be( PitchClass.D );
    ( PitchClass.A - Interval.Fourth ).Should()
                                      .Be( PitchClass.E );
    ( PitchClass.AFlat - Interval.Fourth ).Should()
                                          .Be( PitchClass.EFlat );
    ( PitchClass.GSharp - Interval.AugmentedThird ).Should()
                                                   .Be( PitchClass.EFlat );
    ( PitchClass.D - Interval.MajorSixth ).Should()
                                          .Be( PitchClass.F );
    ( PitchClass.D - Interval.Fifth ).Should()
                                     .Be( PitchClass.G );
    ( PitchClass.C - Interval.Fifth ).Should()
                                     .Be( PitchClass.F );
    ( PitchClass.E - Interval.Fifth ).Should()
                                     .Be( PitchClass.A );
    ( PitchClass.EFlat - Interval.Fifth ).Should()
                                         .Be( PitchClass.AFlat );
    ( PitchClass.EFlat - Interval.DiminishedSixth ).Should()
                                                   .Be( PitchClass.GSharp );
    ( PitchClass.C - Interval.AugmentedFourth ).Should()
                                               .Be( PitchClass.FSharp );
    ( PitchClass.C - Interval.DiminishedFifth ).Should()
                                               .Be( PitchClass.GFlat );
    ( PitchClass.DSharp - Interval.AugmentedSecond ).Should()
                                                    .Be( PitchClass.C );
    ( PitchClass.FSharp - Interval.DiminishedFifth ).Should()
                                                    .Be( PitchClass.C );
    ( PitchClass.GFlat - Interval.AugmentedFourth ).Should()
                                                   .Be( PitchClass.C );
    ( PitchClass.C - Interval.DiminishedSeventh ).Should()
                                                 .Be( PitchClass.DSharp );
    ( PitchClass.F - Interval.DiminishedThird ).Should()
                                               .Be( PitchClass.DSharp );
    ( PitchClass.GSharp - Interval.DiminishedFourth ).Should()
                                                     .Be( PitchClass.Parse( "D##" ) );
  }

  [Fact]
  public void NoteSubtractionTest()
  {
    ( PitchClass.C - PitchClass.E ).Should()
                                   .Be( Interval.MajorThird );
    ( PitchClass.CSharp - PitchClass.E ).Should()
                                        .Be( Interval.MinorThird );
    ( PitchClass.D - PitchClass.F ).Should()
                                   .Be( Interval.MinorThird );
    ( PitchClass.D - PitchClass.G ).Should()
                                   .Be( Interval.Fourth );
    ( PitchClass.E - PitchClass.A ).Should()
                                   .Be( Interval.Fourth );
    ( PitchClass.EFlat - PitchClass.AFlat ).Should()
                                           .Be( Interval.Fourth );
    ( PitchClass.EFlat - PitchClass.GSharp ).Should()
                                            .Be( Interval.AugmentedThird );
    ( PitchClass.F - PitchClass.D ).Should()
                                   .Be( Interval.MajorSixth );
    ( PitchClass.G - PitchClass.D ).Should()
                                   .Be( Interval.Fifth );
    ( PitchClass.F - PitchClass.C ).Should()
                                   .Be( Interval.Fifth );
    ( PitchClass.A - PitchClass.E ).Should()
                                   .Be( Interval.Fifth );
    ( PitchClass.AFlat - PitchClass.EFlat ).Should()
                                           .Be( Interval.Fifth );
    ( PitchClass.GSharp - PitchClass.EFlat ).Should()
                                            .Be( Interval.DiminishedSixth );
    ( PitchClass.C - PitchClass.FSharp ).Should()
                                        .Be( Interval.AugmentedFourth );
    ( PitchClass.C - PitchClass.GFlat ).Should()
                                       .Be( Interval.DiminishedFifth );
    ( PitchClass.C - PitchClass.DSharp ).Should()
                                        .Be( Interval.AugmentedSecond );
    ( PitchClass.FSharp - PitchClass.C ).Should()
                                        .Be( Interval.DiminishedFifth );
    ( PitchClass.GFlat - PitchClass.C ).Should()
                                       .Be( Interval.AugmentedFourth );
    ( PitchClass.DSharp - PitchClass.C ).Should()
                                        .Be( Interval.DiminishedSeventh );
    ( PitchClass.C - PitchClass.Create( NoteName.E, Accidental.DoubleFlat ) ).Should()
                                                                             .Be( Interval.DiminishedThird );

    ( PitchClass.Create( NoteName.D, Accidental.DoubleSharp ) - PitchClass.GSharp ).Should()
      .Be( Interval.DiminishedFourth );
  }

  [Fact]
  public void ParseRejectsInvalidStringsTest()
  {
    var act1 = () => PitchClass.Parse( null! );
    act1.Should()
        .Throw<ArgumentNullException>();
    var act2 = () => PitchClass.Parse( "" );
    act2.Should()
        .Throw<ArgumentException>();
    var act3 = () => PitchClass.Parse( "J" );
    act3.Should()
        .Throw<FormatException>();
    var act4 = () => PitchClass.Parse( "C$" );
    act4.Should()
        .Throw<FormatException>();
  }

  [Fact]
  public void ParseTest()
  {
    PitchClass.Parse( "Cbb" )
              .Should()
              .Be( PitchClass.Create( NoteName.C, Accidental.DoubleFlat ) );
    PitchClass.Parse( "CB" )
              .Should()
              .Be( PitchClass.Create( NoteName.C, Accidental.Flat ) );
    PitchClass.Parse( "C" )
              .Should()
              .Be( PitchClass.C );
    PitchClass.Parse( "c#" )
              .Should()
              .Be( PitchClass.CSharp );
    PitchClass.Parse( "c##" )
              .Should()
              .Be( PitchClass.Create( NoteName.C, Accidental.DoubleSharp ) );
  }

  [Fact]
  public void PredefinedNoteTest()
  {
    NoteMemberTestImpl( PitchClass.C, NoteName.C, Accidental.Natural );
    NoteMemberTestImpl( PitchClass.CSharp, NoteName.C, Accidental.Sharp );
    NoteMemberTestImpl( PitchClass.DFlat, NoteName.D, Accidental.Flat );
    NoteMemberTestImpl( PitchClass.D, NoteName.D, Accidental.Natural );
    NoteMemberTestImpl( PitchClass.DSharp, NoteName.D, Accidental.Sharp );
    NoteMemberTestImpl( PitchClass.EFlat, NoteName.E, Accidental.Flat );
    NoteMemberTestImpl( PitchClass.E, NoteName.E, Accidental.Natural );
    NoteMemberTestImpl( PitchClass.F, NoteName.F, Accidental.Natural );
    NoteMemberTestImpl( PitchClass.FSharp, NoteName.F, Accidental.Sharp );
    NoteMemberTestImpl( PitchClass.GFlat, NoteName.G, Accidental.Flat );
    NoteMemberTestImpl( PitchClass.G, NoteName.G, Accidental.Natural );
    NoteMemberTestImpl( PitchClass.GSharp, NoteName.G, Accidental.Sharp );
    NoteMemberTestImpl( PitchClass.AFlat, NoteName.A, Accidental.Flat );
    NoteMemberTestImpl( PitchClass.A, NoteName.A, Accidental.Natural );
    NoteMemberTestImpl( PitchClass.ASharp, NoteName.A, Accidental.Sharp );
    NoteMemberTestImpl( PitchClass.BFlat, NoteName.B, Accidental.Flat );
    NoteMemberTestImpl( PitchClass.B, NoteName.B, Accidental.Natural );
  }

  [Fact]
  public void PreviousTest()
  {
    PreviousTestImpl( PitchClass.C, PitchClass.B );
    PreviousTestImpl( PitchClass.CSharp, PitchClass.C );
    PreviousTestImpl( PitchClass.DFlat, PitchClass.C );
    PreviousTestImpl( PitchClass.D, PitchClass.CSharp, PitchClass.DFlat );
    PreviousTestImpl( PitchClass.DSharp, PitchClass.D );
    PreviousTestImpl( PitchClass.EFlat, PitchClass.D );
    PreviousTestImpl( PitchClass.E, PitchClass.DSharp, PitchClass.EFlat );
    PreviousTestImpl( PitchClass.F, PitchClass.E );
    PreviousTestImpl( PitchClass.FSharp, PitchClass.F );
    PreviousTestImpl( PitchClass.GFlat, PitchClass.F );
    PreviousTestImpl( PitchClass.G, PitchClass.FSharp, PitchClass.GFlat );
    PreviousTestImpl( PitchClass.GSharp, PitchClass.G );
    PreviousTestImpl( PitchClass.AFlat, PitchClass.G );
    PreviousTestImpl( PitchClass.A, PitchClass.GSharp, PitchClass.AFlat );
    PreviousTestImpl( PitchClass.ASharp, PitchClass.A );
    PreviousTestImpl( PitchClass.BFlat, PitchClass.A );
    PreviousTestImpl( PitchClass.B, PitchClass.ASharp, PitchClass.BFlat );

    PreviousTestImpl( PitchClass.Create( NoteName.B, Accidental.DoubleFlat ), PitchClass.GSharp, PitchClass.AFlat );
    PreviousTestImpl( PitchClass.Create( NoteName.C, Accidental.DoubleFlat ), PitchClass.A );
  }

  [Fact]
  public void SubtractTest()
  {
    SubtractTestImpl( PitchClass.B, 1, PitchClass.BFlat, PitchClass.ASharp );
    SubtractTestImpl( PitchClass.B, 2, PitchClass.A );
    SubtractTestImpl( PitchClass.B, 3, PitchClass.GSharp, PitchClass.AFlat );
    SubtractTestImpl( PitchClass.B, 4, PitchClass.G );
    SubtractTestImpl( PitchClass.B, 5, PitchClass.FSharp, PitchClass.GFlat );
    SubtractTestImpl( PitchClass.B, 6, PitchClass.F );
    SubtractTestImpl( PitchClass.B, 7, PitchClass.E );
    SubtractTestImpl( PitchClass.B, 8, PitchClass.DSharp, PitchClass.EFlat );
    SubtractTestImpl( PitchClass.B, 9, PitchClass.D );
    SubtractTestImpl( PitchClass.B, 10, PitchClass.CSharp, PitchClass.DFlat );
    SubtractTestImpl( PitchClass.B, 11, PitchClass.C );
    SubtractTestImpl( PitchClass.B, 12, PitchClass.B );
  }

  [Fact]
  public void ToStringTest()
  {
    PitchClass.Create( NoteName.C, Accidental.DoubleFlat )
              .ToString()
              .Should()
              .Be( "Cbb" );
    PitchClass.Create( NoteName.C, Accidental.Flat )
              .ToString()
              .Should()
              .Be( "Cb" );
    PitchClass.Create( NoteName.C )
              .ToString()
              .Should()
              .Be( "C" );
    PitchClass.Create( NoteName.C, Accidental.Sharp )
              .ToString()
              .Should()
              .Be( "C#" );
    PitchClass.Create( NoteName.C, Accidental.DoubleSharp )
              .ToString()
              .Should()
              .Be( "C##" );
  }

  [Fact]
  public void TryParseRejectsInvalidStringsTest()
  {
    PitchClass.TryParse( null!, out _ )
              .Should()
              .BeFalse();
    PitchClass.TryParse( "", out _ )
              .Should()
              .BeFalse();
    PitchClass.TryParse( "J", out _ )
              .Should()
              .BeFalse();
    PitchClass.TryParse( "C$", out _ )
              .Should()
              .BeFalse();
  }

  [Fact]
  public void TryParseTest()
  {
    TryParseTestImpl( "C", PitchClass.C );
    TryParseTestImpl( "C#", PitchClass.CSharp );
    TryParseTestImpl( "C##", PitchClass.D );
    TryParseTestImpl( "Cb", PitchClass.B );
    TryParseTestImpl( "Cbb", PitchClass.BFlat );
    TryParseTestImpl( "B#", PitchClass.C );
    TryParseTestImpl( "B##", PitchClass.CSharp );
    TryParseTestImpl( "Bb", PitchClass.BFlat );
    TryParseTestImpl( "Bbb", PitchClass.A );
  }

  #endregion

  #region Implementation

  private static void NotEnharmonicTestImpl(
    PitchClass pitchClass,
    NoteName startInclusive,
    NoteName lastExclusive )
  {
    while( startInclusive != lastExclusive )
    {
      pitchClass.GetEnharmonic( startInclusive )
                .Should()
                .BeNull();
      ++startInclusive;
    }
  }

  private static void EnharmonicTestImpl(
    PitchClass pitchClass,
    params string[] enharmonics )
  {
    foreach( var s in enharmonics )
    {
      var enharmonic = PitchClass.Parse( s );
      pitchClass.GetEnharmonic( enharmonic.NoteName )
                .Should()
                .Be( enharmonic );
    }
  }

  private static void ConstructorTestImpl(
    NoteName noteName,
    Accidental accidental )
  {
    var note = PitchClass.Create( noteName, accidental );
    NoteMemberTestImpl( note, noteName, accidental );
  }

  private static void NoteMemberTestImpl(
    PitchClass pitchClass,
    NoteName noteName,
    Accidental accidental )
  {
    pitchClass.NoteName.Should()
              .Be( noteName );
    pitchClass.Accidental.Should()
              .Be( accidental );
  }

  private static void TryParseTestImpl(
    string value,
    PitchClass expected )
  {
    PitchClass.TryParse( value, out var actual )
              .Should()
              .BeTrue();
    actual.Should()
          .Be( expected );
  }

  private static void NextTestImpl(
    PitchClass pitchClass,
    PitchClass expectedSharp,
    PitchClass? expectedFlat = null )
  {
    AddTestImpl( pitchClass, 1, expectedSharp, expectedFlat );
  }

  private static void PreviousTestImpl(
    PitchClass pitchClass,
    PitchClass expectedSharp,
    PitchClass? expectedFlat = null )
  {
    SubtractTestImpl( pitchClass, 1, expectedSharp, expectedFlat );
  }

  private static void AddTestImpl(
    PitchClass pitchClass,
    int interval,
    PitchClass expectedSharp,
    PitchClass? expectedFlat = null )
  {
    pitchClass.Add( interval )
              .Should()
              .Be( expectedSharp );
    pitchClass.Add( interval )
              .Should()
              .Be( expectedFlat ?? expectedSharp );
  }

  private static void SubtractTestImpl(
    PitchClass pitchClass,
    int interval,
    PitchClass expectedSharp,
    PitchClass? expectedFlat = null )
  {
    pitchClass.Subtract( interval )
              .Should()
              .Be( expectedSharp );
    pitchClass.Subtract( interval )
              .Should()
              .Be( expectedFlat ?? expectedSharp );
  }

  #endregion
}
