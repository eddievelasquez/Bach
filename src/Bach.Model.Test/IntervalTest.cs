// Module Name: IntervalTest.cs
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

public sealed class IntervalTest
{
  #region Public Methods

  [Fact]
  public void EqualityFailsWithNullTest()
  {
    var lhs = new Interval( IntervalQuantity.Fifth, IntervalQuality.Perfect );
#pragma warning disable CS8073
    ( lhs == null ).Should()
                   .BeFalse();
#pragma warning restore CS8073
  }

  [Fact]
  public void EqualitySucceedsWithSameObjectTest()
  {
    var lhs = new Interval( IntervalQuantity.Fifth, IntervalQuality.Perfect );
#pragma warning disable 1718

    // ReSharper disable once EqualExpressionComparison
    ( lhs == lhs ).Should()
                  .BeTrue();
#pragma warning restore 1718
  }

  [Fact]
  public void EqualitySucceedsWithTwoObjectsTest()
  {
    var lhs = new Interval( IntervalQuantity.Fifth, IntervalQuality.Perfect );
    var rhs = new Interval( IntervalQuantity.Fifth, IntervalQuality.Perfect );
    ( lhs == rhs ).Should()
                  .BeTrue();
  }

  [Fact]
  public void EqualsContractTest()
  {
    object x = new Interval( IntervalQuantity.Fifth, IntervalQuality.Perfect );
    object y = new Interval( IntervalQuantity.Fifth, IntervalQuality.Perfect );
    object z = new Interval( IntervalQuantity.Fifth, IntervalQuality.Perfect );

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
    object actual = new Interval( IntervalQuantity.Fifth, IntervalQuality.Perfect );
    actual.Equals( int.MinValue )
          .Should()
          .BeFalse();
  }

  [Fact]
  public void EqualsFailsWithNullTest()
  {
    object actual = new Interval( IntervalQuantity.Fifth, IntervalQuality.Perfect );
    actual.Equals( null )
          .Should()
          .BeFalse();
  }

  [Fact]
  public void EqualsSucceedsWithSameObjectTest()
  {
    var actual = new Interval( IntervalQuantity.Fifth, IntervalQuality.Perfect );
    actual.Equals( actual )
          .Should()
          .BeTrue();
  }

  [Fact]
  public void GetHashcodeTest()
  {
    var actual = new Interval( IntervalQuantity.Fifth, IntervalQuality.Perfect );
    var expected = new Interval( IntervalQuantity.Fifth, IntervalQuality.Perfect );
    expected.Equals( actual )
            .Should()
            .BeTrue();
    actual.GetHashCode()
          .Should()
          .Be( expected.GetHashCode() );
  }

  [Fact]
  public void GetSemitoneCountTest()
  {
    var act = () => Interval.GetSemitoneCount( IntervalQuantity.Unison, IntervalQuality.Diminished );
    act.Should()
       .Throw<ArgumentException>();

    var act1 = () => Interval.GetSemitoneCount( IntervalQuantity.Unison, IntervalQuality.Minor );
    act1.Should()
        .Throw<ArgumentException>();

    Interval.GetSemitoneCount( IntervalQuantity.Unison, IntervalQuality.Perfect )
            .Should()
            .Be( 0 );
    var act2 = () => Interval.GetSemitoneCount( IntervalQuantity.Unison, IntervalQuality.Major );
    act2.Should()
        .Throw<ArgumentException>();

    Interval.GetSemitoneCount( IntervalQuantity.Unison, IntervalQuality.Augmented )
            .Should()
            .Be( 1 );
  }

  [Fact]
  public void InequalityFailsWithSameObjectTest()
  {
    var lhs = new Interval( IntervalQuantity.Fifth, IntervalQuality.Perfect );
#pragma warning disable 1718

    // ReSharper disable once EqualExpressionComparison
    ( lhs != lhs ).Should()
                  .BeFalse();
#pragma warning restore 1718
  }

  [Fact]
  public void InequalitySucceedsWithTwoObjectsTest()
  {
    var lhs = new Interval( IntervalQuantity.Fifth, IntervalQuality.Perfect );
    var rhs = new Interval( IntervalQuantity.Fifth, IntervalQuality.Augmented );
    ( lhs != rhs ).Should()
                  .BeTrue();
  }

  [Fact]
  public void InversionTest()
  {
    Interval.Unison.Inversion.Should()
            .Be( Interval.Octave );
    Interval.MinorSecond.Inversion.Should()
            .Be( Interval.MajorSeventh );
    Interval.MajorSecond.Inversion.Should()
            .Be( Interval.MinorSeventh );
    Interval.MinorThird.Inversion.Should()
            .Be( Interval.MajorSixth );
    Interval.MajorThird.Inversion.Should()
            .Be( Interval.MinorSixth );
    Interval.Fourth.Inversion.Should()
            .Be( Interval.Fifth );
    Interval.AugmentedFourth.Inversion.Should()
            .Be( Interval.DiminishedFifth );
    Interval.Fifth.Inversion.Should()
            .Be( Interval.Fourth );
    Interval.AugmentedFifth.Inversion.Should()
            .Be( Interval.DiminishedFourth );
    Interval.MinorSixth.Inversion.Should()
            .Be( Interval.MajorThird );
    Interval.MajorSixth.Inversion.Should()
            .Be( Interval.MinorThird );
    Interval.MinorSeventh.Inversion.Should()
            .Be( Interval.MajorSecond );
    Interval.MajorSeventh.Inversion.Should()
            .Be( Interval.MinorSecond );
    Interval.Octave.Inversion.Should()
            .Be( Interval.Unison );
  }

  [Fact]
  public void IsValidTest()
  {
    Interval.IsValid( IntervalQuantity.Unison, IntervalQuality.Diminished )
            .Should()
            .BeFalse();
    Interval.IsValid( IntervalQuantity.Fourth, IntervalQuality.Minor )
            .Should()
            .BeFalse();
    Interval.IsValid( IntervalQuantity.Fifth, IntervalQuality.Major )
            .Should()
            .BeFalse();
    Interval.IsValid( (IntervalQuantity) ( -1 ), IntervalQuality.Major )
            .Should()
            .BeFalse();
    Interval.IsValid( (IntervalQuantity) 14, IntervalQuality.Major )
            .Should()
            .BeFalse();
  }

  [Fact]
  public void LogicalOperatorsTest()
  {
    ( Interval.Unison == Interval.Parse( "P1" ) ).Should()
                                                 .BeTrue();
    ( Interval.Unison != Interval.Fourth ).Should()
                                          .BeTrue();
    ( Interval.Unison < Interval.Fourth ).Should()
                                         .BeTrue();
    ( Interval.Unison <= Interval.Fourth ).Should()
                                          .BeTrue();
    ( Interval.Fourth > Interval.Unison ).Should()
                                         .BeTrue();
    ( Interval.Fourth >= Interval.Unison ).Should()
                                          .BeTrue();
    ( Interval.MinorThird < Interval.MajorThird ).Should()
                                                 .BeTrue();
  }

  [Fact]
  public void ParseTest()
  {
    Interval.Parse( "P1" )
            .Should()
            .Be( Interval.Unison );
    var act = () => Interval.Parse( "X2" );
    act.Should()
       .Throw<FormatException>();
  }

  [Fact]
  public void SemitoneCountTest()
  {
    Interval.Unison.SemitoneCount.Should()
            .Be( 0 );
    Interval.AugmentedFirst.SemitoneCount.Should()
            .Be( 1 );
    Interval.DiminishedSecond.SemitoneCount.Should()
            .Be( 0 );
    Interval.MinorSecond.SemitoneCount.Should()
            .Be( 1 );
    Interval.MajorSecond.SemitoneCount.Should()
            .Be( 2 );
    Interval.AugmentedSecond.SemitoneCount.Should()
            .Be( 3 );
    Interval.DiminishedThird.SemitoneCount.Should()
            .Be( 2 );
    Interval.MinorThird.SemitoneCount.Should()
            .Be( 3 );
    Interval.MajorThird.SemitoneCount.Should()
            .Be( 4 );
    Interval.AugmentedThird.SemitoneCount.Should()
            .Be( 5 );
    Interval.DiminishedFourth.SemitoneCount.Should()
            .Be( 4 );
    Interval.Fourth.SemitoneCount.Should()
            .Be( 5 );
    Interval.AugmentedFourth.SemitoneCount.Should()
            .Be( 6 );
    Interval.DiminishedFifth.SemitoneCount.Should()
            .Be( 6 );
    Interval.Fifth.SemitoneCount.Should()
            .Be( 7 );
    Interval.AugmentedFifth.SemitoneCount.Should()
            .Be( 8 );
    Interval.DiminishedSixth.SemitoneCount.Should()
            .Be( 7 );
    Interval.MinorSixth.SemitoneCount.Should()
            .Be( 8 );
    Interval.MajorSixth.SemitoneCount.Should()
            .Be( 9 );
    Interval.AugmentedSixth.SemitoneCount.Should()
            .Be( 10 );
    Interval.DiminishedSeventh.SemitoneCount.Should()
            .Be( 9 );
    Interval.MinorSeventh.SemitoneCount.Should()
            .Be( 10 );
    Interval.MajorSeventh.SemitoneCount.Should()
            .Be( 11 );
    Interval.AugmentedSeventh.SemitoneCount.Should()
            .Be( 12 );
    Interval.DiminishedOctave.SemitoneCount.Should()
            .Be( 11 );
    Interval.Octave.SemitoneCount.Should()
            .Be( 12 );
  }

  [Fact]
  public void TryParseReturnsFalseBlankStringTest()
  {
    Interval.TryParse( "", out _ )
            .Should()
            .BeFalse();
    Interval.TryParse( "   ", out _ )
            .Should()
            .BeFalse();
  }

  [Fact]
  public void TryParseReturnsFalseWithNonsensicalIntervalsTest()
  {
    Interval.TryParse( "M1", out _ )
            .Should()
            .BeFalse();
    Interval.TryParse( "P2", out _ )
            .Should()
            .BeFalse();
    Interval.TryParse( "L2", out _ )
            .Should()
            .BeFalse();
    Interval.TryParse( "Px", out _ )
            .Should()
            .BeFalse();
  }

  [Fact]
  public void TryParseReturnsFalseWithNullTest()
  {
    Interval.TryParse( null!, out _ )
            .Should()
            .BeFalse();
  }

  [Fact]
  public void TryParseSkipsLeadingWhitespaceTest()
  {
    Interval.TryParse( "  P5", out var actual )
            .Should()
            .BeTrue();
    actual.Should()
          .Be( Interval.Fifth );
  }

  [Fact]
  public void TryParseTest()
  {
    Interval.TryParse( "P1", out var actual )
            .Should()
            .BeTrue();
    actual.Should()
          .Be( Interval.Unison );
    Interval.TryParse( "R", out actual )
            .Should()
            .BeTrue();
    actual.Should()
          .Be( Interval.Unison );
    Interval.TryParse( "1", out actual )
            .Should()
            .BeTrue();
    actual.Should()
          .Be( Interval.Unison );

    Interval.TryParse( "A1", out actual )
            .Should()
            .BeTrue();
    actual.Should()
          .Be( Interval.AugmentedFirst );

    Interval.TryParse( "d3", out actual )
            .Should()
            .BeTrue();
    actual.Should()
          .Be( Interval.DiminishedThird );

    Interval.TryParse( "m3", out actual )
            .Should()
            .BeTrue();
    actual.Should()
          .Be( Interval.MinorThird );

    Interval.TryParse( "M3", out actual )
            .Should()
            .BeTrue();
    actual.Should()
          .Be( Interval.MajorThird );

    Interval.TryParse( "A3", out actual )
            .Should()
            .BeTrue();
    actual.Should()
          .Be( Interval.AugmentedThird );
  }

  [Fact]
  public void TypeSafeEqualsContractTest()
  {
    var x = new Interval( IntervalQuantity.Fifth, IntervalQuality.Perfect );
    var y = new Interval( IntervalQuantity.Fifth, IntervalQuality.Perfect );
    var z = new Interval( IntervalQuantity.Fifth, IntervalQuality.Perfect );

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
    var actual = new Interval( IntervalQuantity.Fifth, IntervalQuality.Perfect );

    // ReSharper disable once SuspiciousTypeConversion.Global
    actual.Equals( int.MinValue )
          .Should()
          .BeFalse();
  }

  [Fact]
  public void TypeSafeEqualsFailsWithNullTest()
  {
    var actual = new Interval( IntervalQuantity.Fifth, IntervalQuality.Perfect );
    actual.Equals( null )
          .Should()
          .BeFalse();
  }

  #endregion
}
