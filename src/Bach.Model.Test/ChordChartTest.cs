// Module Name: ChordChartTest.cs
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

public sealed class ChordChartTest
{
  #region Public Methods

  [Fact]
  public void Constructor_ShouldInitializePropertiesAndResolveChords_ForMajorKeyProgression()
  {
    var key = new Key( PitchClass.C, ModeType.Major );

    var progression = new ChordProgression(
      ScaleDegree.Tonic,
      ScaleDegree.Subdominant,
      ScaleDegree.Dominant,
      ScaleDegree.Tonic
    );

    var chart = new ChordChart( key, progression );

    chart.Key.Should()
         .BeSameAs( key );

    chart.Progression.Should()
         .BeSameAs( progression );

    chart.Chords.Should()
         .HaveCount( 4 );

    chart.Chords.Select( chord => chord.ToString() )
         .Should()
         .Equal( "C", "F", "G", "C" );
  }

  [Fact]
  public void Constructor_ShouldInitializePropertiesAndResolveChords_ForProgressionString()
  {
    var key = new Key( PitchClass.C, ModeType.Major );

    var chart = new ChordChart( key, "I-IV-V-I" );

    chart.Key.Should()
         .BeSameAs( key );

    chart.Progression.ToString()
         .Should()
         .Be( "I-IV-V-I" );

    chart.Chords.Select( chord => chord.ToString() )
         .Should()
         .Equal( "C", "F", "G", "C" );
  }

  [Fact]
  public void Constructor_ShouldInitializePropertiesAndResolveChords_ForScaleDegrees()
  {
    var key = new Key( PitchClass.C, ModeType.Major );

    var chart = new ChordChart(
      key,
      ScaleDegree.Tonic,
      ScaleDegree.Subdominant,
      ScaleDegree.Dominant,
      ScaleDegree.Tonic
    );

    chart.Key.Should()
         .BeSameAs( key );

    chart.Progression.ScaleDegrees.Should()
         .Equal( ScaleDegree.Tonic, ScaleDegree.Subdominant, ScaleDegree.Dominant, ScaleDegree.Tonic );

    chart.Chords.Select( chord => chord.ToString() )
         .Should()
         .Equal( "C", "F", "G", "C" );
  }

  [Fact]
  public void Constructor_ShouldResolveModeSpecificTriads_ForMinorKeyProgression()
  {
    var key = new Key( PitchClass.A, ModeType.Minor );

    var progression = new ChordProgression(
      ScaleDegree.Tonic,
      ScaleDegree.Mediant,
      ScaleDegree.Dominant
    );

    var chart = new ChordChart( key, progression );

    chart.Key.Should()
         .BeSameAs( key );

    chart.Chords.Select( chord => chord.ToString() )
         .Should()
         .Equal( "Am", "C", "Em" );
  }

  [Fact]
  public void Constructor_ShouldPreserveKey_WhenScaleDegreesAreSuppliedForMinorKey()
  {
    var key = new Key( PitchClass.E, ModeType.Minor );

    var chart = new ChordChart( key, ScaleDegree.Tonic, ScaleDegree.Subdominant, ScaleDegree.Dominant );

    chart.Key.Should()
         .BeSameAs( key );
    chart.Key.Tonic.Should()
         .Be( PitchClass.E );
    chart.Key.Mode.Should()
         .Be( ModeType.Minor );
  }

  [Fact]
  public void Constructor_ShouldResolveModeSpecificTriads_ForMinorProgressionString()
  {
    var key = new Key( PitchClass.A, ModeType.Minor );

    var chart = new ChordChart( key, "i-III-V" );

    chart.Key.Should()
         .BeSameAs( key );

    chart.Chords.Select( chord => chord.ToString() )
         .Should()
         .Equal( "Am", "C", "Em" );
  }

  [Theory]
  [InlineData( null )]
  [InlineData( "" )]
  [InlineData( "   " )]
  public void Constructor_ShouldThrowArgumentException_WhenProgressionStringIsNullOrWhitespace(
    string? progression )
  {
    var key = new Key( PitchClass.C, ModeType.Major );

    var act = () => new ChordChart( key, progression! );

    act.Should()
       .Throw<ArgumentException>();
  }

  [Fact]
  public void Constructor_ShouldThrowArgumentNullException_WhenKeyIsNull()
  {
    var progression = new ChordProgression( ScaleDegree.Tonic );

    var act = () => new ChordChart( null!, progression );

    act.Should()
       .Throw<ArgumentNullException>();
  }

  [Fact]
  public void Constructor_ShouldThrowArgumentNullException_WhenKeyIsNullAndScaleDegreesAreProvided()
  {
    var act = () => new ChordChart( null!, ScaleDegree.Tonic );

    act.Should()
       .Throw<ArgumentNullException>();
  }

  [Fact]
  public void Constructor_ShouldThrowArgumentException_WhenNoScaleDegreesAreProvided()
  {
    var key = new Key( PitchClass.C, ModeType.Major );

    var act = () => new ChordChart( key );

    act.Should()
       .Throw<ArgumentException>();
  }

  [Fact]
  public void Constructor_ShouldThrowArgumentNullException_WhenKeyIsNullAndProgressionStringIsProvided()
  {
    var act = () => new ChordChart( null!, "I-IV-V-I" );

    act.Should()
       .Throw<ArgumentNullException>();
  }

  [Fact]
  public void Constructor_ShouldThrowArgumentNullException_WhenProgressionIsNull()
  {
    var key = new Key( PitchClass.C, ModeType.Major );

    var act = () => new ChordChart( key, (ChordProgression) null! );

    act.Should()
       .Throw<ArgumentNullException>();
  }

  [Fact]
  public void Constructor_ShouldThrowFormatException_WhenProgressionStringIsInvalid()
  {
    var key = new Key( PitchClass.C, ModeType.Major );

    var act = () => new ChordChart( key, "I-INVALID-V-I" );

    act.Should()
       .Throw<FormatException>();
  }

  [Fact]
  public void ToString_ShouldJoinChordNamesWithHyphens()
  {
    var key = new Key( PitchClass.C, ModeType.Major );

    var progression = new ChordProgression(
      ScaleDegree.Tonic,
      ScaleDegree.Subdominant,
      ScaleDegree.Dominant,
      ScaleDegree.Tonic
    );
    var chart = new ChordChart( key, progression );

    chart.ToString()
         .Should()
         .Be( "C-F-G-C" );
  }

  #endregion
}
