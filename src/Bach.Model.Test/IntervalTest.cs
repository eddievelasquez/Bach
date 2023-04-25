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

using System;
using Xunit;

namespace Bach.Model.Test;

public sealed class IntervalTest
{
#region Public Methods

  [Fact]
  public void EqualityFailsWithNullTest()
  {
    var lhs = new Interval( IntervalQuantity.Fifth, IntervalQuality.Perfect );
#pragma warning disable CS8073
    Assert.False( lhs == null );
#pragma warning restore CS8073
  }

  [Fact]
  public void EqualitySucceedsWithSameObjectTest()
  {
    var lhs = new Interval( IntervalQuantity.Fifth, IntervalQuality.Perfect );
#pragma warning disable 1718

    // ReSharper disable once EqualExpressionComparison
    Assert.True( lhs == lhs );
#pragma warning restore 1718
  }

  [Fact]
  public void EqualitySucceedsWithTwoObjectsTest()
  {
    var lhs = new Interval( IntervalQuantity.Fifth, IntervalQuality.Perfect );
    var rhs = new Interval( IntervalQuantity.Fifth, IntervalQuality.Perfect );
    Assert.True( lhs == rhs );
  }

  [Fact]
  public void EqualsContractTest()
  {
    object x = new Interval( IntervalQuantity.Fifth, IntervalQuality.Perfect );
    object y = new Interval( IntervalQuantity.Fifth, IntervalQuality.Perfect );
    object z = new Interval( IntervalQuantity.Fifth, IntervalQuality.Perfect );

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
    object actual = new Interval( IntervalQuantity.Fifth, IntervalQuality.Perfect );
    Assert.False( actual.Equals( int.MinValue ) );
  }

  [Fact]
  public void EqualsFailsWithNullTest()
  {
    object actual = new Interval( IntervalQuantity.Fifth, IntervalQuality.Perfect );
    Assert.False( actual.Equals( null ) );
  }

  [Fact]
  public void EqualsSucceedsWithSameObjectTest()
  {
    var actual = new Interval( IntervalQuantity.Fifth, IntervalQuality.Perfect );
    Assert.True( actual.Equals( actual ) );
  }

  [Fact]
  public void GetHashcodeTest()
  {
    var actual = new Interval( IntervalQuantity.Fifth, IntervalQuality.Perfect );
    var expected = new Interval( IntervalQuantity.Fifth, IntervalQuality.Perfect );
    Assert.True( expected.Equals( actual ) );
    Assert.Equal( expected.GetHashCode(), actual.GetHashCode() );
  }

  [Fact]
  public void GetSemitoneCountTest()
  {
    Assert.Throws<ArgumentException>( () => Interval.GetSemitoneCount( IntervalQuantity.Unison,
                                                                       IntervalQuality.Diminished ) );

    Assert.Throws<ArgumentException>(
      () => Interval.GetSemitoneCount( IntervalQuantity.Unison, IntervalQuality.Minor ) );

    Assert.Equal( 0, Interval.GetSemitoneCount( IntervalQuantity.Unison, IntervalQuality.Perfect ) );
    Assert.Throws<ArgumentException>(
      () => Interval.GetSemitoneCount( IntervalQuantity.Unison, IntervalQuality.Major ) );

    Assert.Equal( 1, Interval.GetSemitoneCount( IntervalQuantity.Unison, IntervalQuality.Augmented ) );
  }

  [Fact]
  public void InequalityFailsWithSameObjectTest()
  {
    var lhs = new Interval( IntervalQuantity.Fifth, IntervalQuality.Perfect );
#pragma warning disable 1718

    // ReSharper disable once EqualExpressionComparison
    Assert.False( lhs != lhs );
#pragma warning restore 1718
  }

  [Fact]
  public void InequalitySucceedsWithTwoObjectsTest()
  {
    var lhs = new Interval( IntervalQuantity.Fifth, IntervalQuality.Perfect );
    var rhs = new Interval( IntervalQuantity.Fifth, IntervalQuality.Augmented );
    Assert.True( lhs != rhs );
  }

  [Fact]
  public void InversionTest()
  {
    Assert.Equal( Interval.Octave, Interval.Unison.Inversion );
    Assert.Equal( Interval.MajorSeventh, Interval.MinorSecond.Inversion );
    Assert.Equal( Interval.MinorSeventh, Interval.MajorSecond.Inversion );
    Assert.Equal( Interval.MajorSixth, Interval.MinorThird.Inversion );
    Assert.Equal( Interval.MinorSixth, Interval.MajorThird.Inversion );
    Assert.Equal( Interval.Fifth, Interval.Fourth.Inversion );
    Assert.Equal( Interval.DiminishedFifth, Interval.AugmentedFourth.Inversion );
    Assert.Equal( Interval.Fourth, Interval.Fifth.Inversion );
    Assert.Equal( Interval.DiminishedFourth, Interval.AugmentedFifth.Inversion );
    Assert.Equal( Interval.MajorThird, Interval.MinorSixth.Inversion );
    Assert.Equal( Interval.MinorThird, Interval.MajorSixth.Inversion );
    Assert.Equal( Interval.MajorSecond, Interval.MinorSeventh.Inversion );
    Assert.Equal( Interval.MinorSecond, Interval.MajorSeventh.Inversion );
    Assert.Equal( Interval.Unison, Interval.Octave.Inversion );
  }

  [Fact]
  public void IsValidTest()
  {
    Assert.False( Interval.IsValid( IntervalQuantity.Unison, IntervalQuality.Diminished ) );
    Assert.False( Interval.IsValid( IntervalQuantity.Fourth, IntervalQuality.Minor ) );
    Assert.False( Interval.IsValid( IntervalQuantity.Fifth, IntervalQuality.Major ) );
    Assert.False( Interval.IsValid( (IntervalQuantity) ( -1 ), IntervalQuality.Major ) );
    Assert.False( Interval.IsValid( (IntervalQuantity) ( 14 ), IntervalQuality.Major ) );
  }

  [Fact]
  public void LogicalOperatorsTest()
  {
    Assert.True( Interval.Unison == Interval.Parse( "P1" ) );
    Assert.True( Interval.Unison != Interval.Fourth );
    Assert.True( Interval.Unison < Interval.Fourth );
    Assert.True( Interval.Unison <= Interval.Fourth );
    Assert.True( Interval.Fourth > Interval.Unison );
    Assert.True( Interval.Fourth >= Interval.Unison );
    Assert.True( Interval.MinorThird < Interval.MajorThird );
  }

  [Fact]
  public void ParseTest()
  {
    Assert.Equal( Interval.Unison, Interval.Parse( "P1" ) );
    Assert.Throws<FormatException>( () => Interval.Parse( "X2" ) );
  }

  [Fact]
  public void SemitoneCountTest()
  {
    Assert.Equal( 0, Interval.Unison.SemitoneCount );
    Assert.Equal( 1, Interval.AugmentedFirst.SemitoneCount );
    Assert.Equal( 0, Interval.DiminishedSecond.SemitoneCount );
    Assert.Equal( 1, Interval.MinorSecond.SemitoneCount );
    Assert.Equal( 2, Interval.MajorSecond.SemitoneCount );
    Assert.Equal( 3, Interval.AugmentedSecond.SemitoneCount );
    Assert.Equal( 2, Interval.DiminishedThird.SemitoneCount );
    Assert.Equal( 3, Interval.MinorThird.SemitoneCount );
    Assert.Equal( 4, Interval.MajorThird.SemitoneCount );
    Assert.Equal( 5, Interval.AugmentedThird.SemitoneCount );
    Assert.Equal( 4, Interval.DiminishedFourth.SemitoneCount );
    Assert.Equal( 5, Interval.Fourth.SemitoneCount );
    Assert.Equal( 6, Interval.AugmentedFourth.SemitoneCount );
    Assert.Equal( 6, Interval.DiminishedFifth.SemitoneCount );
    Assert.Equal( 7, Interval.Fifth.SemitoneCount );
    Assert.Equal( 8, Interval.AugmentedFifth.SemitoneCount );
    Assert.Equal( 7, Interval.DiminishedSixth.SemitoneCount );
    Assert.Equal( 8, Interval.MinorSixth.SemitoneCount );
    Assert.Equal( 9, Interval.MajorSixth.SemitoneCount );
    Assert.Equal( 10, Interval.AugmentedSixth.SemitoneCount );
    Assert.Equal( 9, Interval.DiminishedSeventh.SemitoneCount );
    Assert.Equal( 10, Interval.MinorSeventh.SemitoneCount );
    Assert.Equal( 11, Interval.MajorSeventh.SemitoneCount );
    Assert.Equal( 12, Interval.AugmentedSeventh.SemitoneCount );
    Assert.Equal( 11, Interval.DiminishedOctave.SemitoneCount );
    Assert.Equal( 12, Interval.Octave.SemitoneCount );
  }

  [Fact]
  public void TryParseReturnsFalseBlankStringTest()
  {
    Assert.False( Interval.TryParse( "", out var _ ) );
    Assert.False( Interval.TryParse( "   ", out var _ ) );
  }

  [Fact]
  public void TryParseReturnsFalseWithNonsensicalIntervalsTest()
  {
    Assert.False( Interval.TryParse( "M1", out var _ ) );
    Assert.False( Interval.TryParse( "P2", out var _ ) );
    Assert.False( Interval.TryParse( "L2", out var _ ) );
    Assert.False( Interval.TryParse( "Px", out var _ ) );
  }

  [Fact]
  public void TryParseReturnsFalseWithNullTest()
  {
    Assert.False( Interval.TryParse( null, out var _ ) );
  }

  [Fact]
  public void TryParseSkipsLeadingWhitespaceTest()
  {
    Assert.True( Interval.TryParse( "  P5", out var actual ) );
    Assert.Equal( Interval.Fifth, actual );
  }

  [Fact]
  public void TryParseTest()
  {
    Assert.True( Interval.TryParse( "P1", out var actual ) );
    Assert.Equal( Interval.Unison, actual );
    Assert.True( Interval.TryParse( "R", out actual ) );
    Assert.Equal( Interval.Unison, actual );
    Assert.True( Interval.TryParse( "1", out actual ) );
    Assert.Equal( Interval.Unison, actual );

    Assert.True( Interval.TryParse( "A1", out actual ) );
    Assert.Equal( Interval.AugmentedFirst, actual );

    Assert.True( Interval.TryParse( "d3", out actual ) );
    Assert.Equal( Interval.DiminishedThird, actual );

    Assert.True( Interval.TryParse( "m3", out actual ) );
    Assert.Equal( Interval.MinorThird, actual );

    Assert.True( Interval.TryParse( "M3", out actual ) );
    Assert.Equal( Interval.MajorThird, actual );

    Assert.True( Interval.TryParse( "A3", out actual ) );
    Assert.Equal( Interval.AugmentedThird, actual );
  }

  [Fact]
  public void TypeSafeEqualsContractTest()
  {
    var x = new Interval( IntervalQuantity.Fifth, IntervalQuality.Perfect );
    var y = new Interval( IntervalQuantity.Fifth, IntervalQuality.Perfect );
    var z = new Interval( IntervalQuantity.Fifth, IntervalQuality.Perfect );

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
    var actual = new Interval( IntervalQuantity.Fifth, IntervalQuality.Perfect );

    // ReSharper disable once SuspiciousTypeConversion.Global
    Assert.False( actual.Equals( int.MinValue ) );
  }

  [Fact]
  public void TypeSafeEqualsFailsWithNullTest()
  {
    var actual = new Interval( IntervalQuantity.Fifth, IntervalQuality.Perfect );
    Assert.False( actual.Equals( null ) );
  }

#endregion
}
