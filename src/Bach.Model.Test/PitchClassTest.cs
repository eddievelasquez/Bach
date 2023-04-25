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

using System;
using Xunit;

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
    Assert.Equal( PitchClass.C, PitchClass.B + 1 );
    Assert.Equal( PitchClass.C, PitchClass.C + 12 );
    Assert.Equal( PitchClass.B, PitchClass.C - 1 );
    Assert.Equal( PitchClass.C, PitchClass.C - 12 );

    var pitchClass = PitchClass.B;
    Assert.Equal( PitchClass.B, pitchClass++ );
    Assert.Equal( PitchClass.C, pitchClass );
    Assert.Equal( PitchClass.CSharp, ++pitchClass );

    pitchClass = PitchClass.C;
    Assert.Equal( PitchClass.C, pitchClass-- );
    Assert.Equal( PitchClass.B, pitchClass );
    Assert.Equal( PitchClass.BFlat, --pitchClass );
  }

  [Fact]
  public void CompareToTest()
  {
    Assert.True( PitchClass.C.CompareTo( PitchClass.C ) == 0 );
    Assert.True( PitchClass.C.CompareTo( PitchClass.D ) < 0 );
    Assert.True( PitchClass.D.CompareTo( PitchClass.C ) > 0 );
    Assert.True( PitchClass.C.CompareTo( PitchClass.B ) < 0 );
    Assert.True( PitchClass.B.CompareTo( PitchClass.C ) > 0 );
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
    Assert.True( PitchClass.C.Equals( actual ) );
    Assert.False( PitchClass.C.Equals( null ) );
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
    Assert.True( PitchClass.C == PitchClass.Create( NoteName.B, Accidental.Sharp ) );
    Assert.True( PitchClass.C != PitchClass.B );
    Assert.True( PitchClass.C < PitchClass.B );
    Assert.True( PitchClass.C <= PitchClass.B );
    Assert.True( PitchClass.D > PitchClass.C );
    Assert.True( PitchClass.D >= PitchClass.C );
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
    Assert.Equal( PitchClass.E, PitchClass.C + Interval.MajorThird );
    Assert.Equal( PitchClass.E, PitchClass.CSharp + Interval.MinorThird );
    Assert.Equal( PitchClass.F, PitchClass.D + Interval.MinorThird );
    Assert.Equal( PitchClass.G, PitchClass.D + Interval.Fourth );
    Assert.Equal( PitchClass.A, PitchClass.E + Interval.Fourth );
    Assert.Equal( PitchClass.AFlat, PitchClass.EFlat + Interval.Fourth );
    Assert.Equal( PitchClass.GSharp, PitchClass.EFlat + Interval.AugmentedThird );
    Assert.Equal( PitchClass.D, PitchClass.F + Interval.MajorSixth );
    Assert.Equal( PitchClass.D, PitchClass.G + Interval.Fifth );
    Assert.Equal( PitchClass.C, PitchClass.F + Interval.Fifth );
    Assert.Equal( PitchClass.E, PitchClass.A + Interval.Fifth );
    Assert.Equal( PitchClass.EFlat, PitchClass.AFlat + Interval.Fifth );
    Assert.Equal( PitchClass.EFlat, PitchClass.GSharp + Interval.DiminishedSixth );
    Assert.Equal( PitchClass.C, PitchClass.FSharp + Interval.AugmentedFourth );
    Assert.Equal( PitchClass.C, PitchClass.GFlat + Interval.DiminishedFifth );
    Assert.Equal( PitchClass.DSharp, PitchClass.C + Interval.AugmentedSecond );
    Assert.Equal( PitchClass.FSharp, PitchClass.C + Interval.DiminishedFifth );
    Assert.Equal( PitchClass.GFlat, PitchClass.C + Interval.AugmentedFourth );
    Assert.Equal( PitchClass.C, PitchClass.DSharp + Interval.DiminishedSeventh );
    Assert.Equal( PitchClass.F, PitchClass.DSharp + Interval.DiminishedThird );
    Assert.Equal( PitchClass.GSharp, PitchClass.Parse( "D##" ) + Interval.DiminishedFourth );
  }

  [Fact]
  public void NoteSubtractIntervalTest()
  {
    Assert.Equal( PitchClass.Parse( "Cb" ), PitchClass.F - Interval.AugmentedFourth );
    Assert.Equal( PitchClass.C, PitchClass.E - Interval.MajorThird );
    Assert.Equal( PitchClass.CSharp, PitchClass.E - Interval.MinorThird );
    Assert.Equal( PitchClass.D, PitchClass.F - Interval.MinorThird );
    Assert.Equal( PitchClass.D, PitchClass.G - Interval.Fourth );
    Assert.Equal( PitchClass.E, PitchClass.A - Interval.Fourth );
    Assert.Equal( PitchClass.EFlat, PitchClass.AFlat - Interval.Fourth );
    Assert.Equal( PitchClass.EFlat, PitchClass.GSharp - Interval.AugmentedThird );
    Assert.Equal( PitchClass.F, PitchClass.D - Interval.MajorSixth );
    Assert.Equal( PitchClass.G, PitchClass.D - Interval.Fifth );
    Assert.Equal( PitchClass.F, PitchClass.C - Interval.Fifth );
    Assert.Equal( PitchClass.A, PitchClass.E - Interval.Fifth );
    Assert.Equal( PitchClass.AFlat, PitchClass.EFlat - Interval.Fifth );
    Assert.Equal( PitchClass.GSharp, PitchClass.EFlat - Interval.DiminishedSixth );
    Assert.Equal( PitchClass.FSharp, PitchClass.C - Interval.AugmentedFourth );
    Assert.Equal( PitchClass.GFlat, PitchClass.C - Interval.DiminishedFifth );
    Assert.Equal( PitchClass.C, PitchClass.DSharp - Interval.AugmentedSecond );
    Assert.Equal( PitchClass.C, PitchClass.FSharp - Interval.DiminishedFifth );
    Assert.Equal( PitchClass.C, PitchClass.GFlat - Interval.AugmentedFourth );
    Assert.Equal( PitchClass.DSharp, PitchClass.C - Interval.DiminishedSeventh );
    Assert.Equal( PitchClass.DSharp, PitchClass.F - Interval.DiminishedThird );
    Assert.Equal( PitchClass.Parse( "D##" ), PitchClass.GSharp - Interval.DiminishedFourth );
  }

  [Fact]
  public void NoteSubtractionTest()
  {
    Assert.Equal( Interval.MajorThird, PitchClass.C - PitchClass.E );
    Assert.Equal( Interval.MinorThird, PitchClass.CSharp - PitchClass.E );
    Assert.Equal( Interval.MinorThird, PitchClass.D - PitchClass.F );
    Assert.Equal( Interval.Fourth, PitchClass.D - PitchClass.G );
    Assert.Equal( Interval.Fourth, PitchClass.E - PitchClass.A );
    Assert.Equal( Interval.Fourth, PitchClass.EFlat - PitchClass.AFlat );
    Assert.Equal( Interval.AugmentedThird, PitchClass.EFlat - PitchClass.GSharp );
    Assert.Equal( Interval.MajorSixth, PitchClass.F - PitchClass.D );
    Assert.Equal( Interval.Fifth, PitchClass.G - PitchClass.D );
    Assert.Equal( Interval.Fifth, PitchClass.F - PitchClass.C );
    Assert.Equal( Interval.Fifth, PitchClass.A - PitchClass.E );
    Assert.Equal( Interval.Fifth, PitchClass.AFlat - PitchClass.EFlat );
    Assert.Equal( Interval.DiminishedSixth, PitchClass.GSharp - PitchClass.EFlat );
    Assert.Equal( Interval.AugmentedFourth, PitchClass.C - PitchClass.FSharp );
    Assert.Equal( Interval.DiminishedFifth, PitchClass.C - PitchClass.GFlat );
    Assert.Equal( Interval.AugmentedSecond, PitchClass.C - PitchClass.DSharp );
    Assert.Equal( Interval.DiminishedFifth, PitchClass.FSharp - PitchClass.C );
    Assert.Equal( Interval.AugmentedFourth, PitchClass.GFlat - PitchClass.C );
    Assert.Equal( Interval.DiminishedSeventh, PitchClass.DSharp - PitchClass.C );
    Assert.Equal( Interval.DiminishedThird, PitchClass.C - PitchClass.Create( NoteName.E, Accidental.DoubleFlat ) );
    Assert.Equal( Interval.DiminishedFourth,
                  PitchClass.Create( NoteName.D, Accidental.DoubleSharp ) - PitchClass.GSharp );
  }

  [Fact]
  public void ParseRejectsInvalidStringsTest()
  {
    Assert.Throws<ArgumentNullException>( () => PitchClass.Parse( null ) );
    Assert.Throws<ArgumentException>( () => PitchClass.Parse( "" ) );
    Assert.Throws<FormatException>( () => PitchClass.Parse( "J" ) );
    Assert.Throws<FormatException>( () => PitchClass.Parse( "C$" ) );
  }

  [Fact]
  public void ParseTest()
  {
    Assert.Equal( PitchClass.Create( NoteName.C, Accidental.DoubleFlat ), PitchClass.Parse( "Cbb" ) );
    Assert.Equal( PitchClass.Create( NoteName.C, Accidental.Flat ), PitchClass.Parse( "CB" ) );
    Assert.Equal( PitchClass.C, PitchClass.Parse( "C" ) );
    Assert.Equal( PitchClass.CSharp, PitchClass.Parse( "c#" ) );
    Assert.Equal( PitchClass.Create( NoteName.C, Accidental.DoubleSharp ), PitchClass.Parse( "c##" ) );
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
    Assert.Equal( "Cbb", PitchClass.Create( NoteName.C, Accidental.DoubleFlat ).ToString() );
    Assert.Equal( "Cb", PitchClass.Create( NoteName.C, Accidental.Flat ).ToString() );
    Assert.Equal( "C", PitchClass.Create( NoteName.C ).ToString() );
    Assert.Equal( "C#", PitchClass.Create( NoteName.C, Accidental.Sharp ).ToString() );
    Assert.Equal( "C##", PitchClass.Create( NoteName.C, Accidental.DoubleSharp ).ToString() );
  }

  [Fact]
  public void TryParseRejectsInvalidStringsTest()
  {
    Assert.False( PitchClass.TryParse( null, out _ ) );
    Assert.False( PitchClass.TryParse( "", out _ ) );
    Assert.False( PitchClass.TryParse( "J", out _ ) );
    Assert.False( PitchClass.TryParse( "C$", out _ ) );
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
      Assert.Null( pitchClass.GetEnharmonic( startInclusive ) );
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
      Assert.Equal( enharmonic, pitchClass.GetEnharmonic( enharmonic.NoteName ) );
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
    Assert.Equal( noteName, pitchClass.NoteName );
    Assert.Equal( accidental, pitchClass.Accidental );
  }

  private static void TryParseTestImpl(
    string value,
    PitchClass expected )
  {
    Assert.True( PitchClass.TryParse( value, out var actual ) );
    Assert.Equal( expected, actual );
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
    Assert.Equal( expectedSharp, pitchClass.Add( interval ) );
    Assert.Equal( expectedFlat ?? expectedSharp, pitchClass.Add( interval ) );
  }

  private static void SubtractTestImpl(
    PitchClass pitchClass,
    int interval,
    PitchClass expectedSharp,
    PitchClass? expectedFlat = null )
  {
    Assert.Equal( expectedSharp, pitchClass.Subtract( interval ) );
    Assert.Equal( expectedFlat ?? expectedSharp, pitchClass.Subtract( interval ) );
  }

#endregion
}
