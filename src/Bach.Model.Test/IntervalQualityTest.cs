// Module Name: IntervalQualityTest.cs
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

public sealed class IntervalQualityTest
{
  #region Public Methods

  [Fact]
  public void AdditionOperatorTest()
  {
    ( IntervalQuality.Diminished + 0 ).Should()
                                      .Be( IntervalQuality.Diminished );
    ( IntervalQuality.Diminished + 1 ).Should()
                                      .Be( IntervalQuality.Minor );
    ( IntervalQuality.Minor + 1 ).Should()
                                 .Be( IntervalQuality.Perfect );
    ( IntervalQuality.Perfect + 1 ).Should()
                                   .Be( IntervalQuality.Major );
    ( IntervalQuality.Major + 1 ).Should()
                                 .Be( IntervalQuality.Augmented );
  }

  [Fact]
  public void AdditionOperatorThrowsWhenOutOfRangeTest()
  {
    var act = () => IntervalQuality.Augmented + 1;
    act.Should()
       .Throw<ArgumentOutOfRangeException>();
  }

  [Fact]
  public void AddTest()
  {
    IntervalQuality.Diminished.Add( 0 )
                   .Should()
                   .Be( IntervalQuality.Diminished );
    IntervalQuality.Diminished.Add( 1 )
                   .Should()
                   .Be( IntervalQuality.Minor );
    IntervalQuality.Minor.Add( 1 )
                   .Should()
                   .Be( IntervalQuality.Perfect );
    IntervalQuality.Perfect.Add( 1 )
                   .Should()
                   .Be( IntervalQuality.Major );
    IntervalQuality.Major.Add( 1 )
                   .Should()
                   .Be( IntervalQuality.Augmented );
  }

  [Fact]
  public void AddThrowsWhenOutOfRangeTest()
  {
    var act1 = () => IntervalQuality.Diminished.Add( -1 );
    act1.Should()
        .Throw<ArgumentOutOfRangeException>();
    var act2 = () => IntervalQuality.Augmented.Add( 1 );
    act2.Should()
        .Throw<ArgumentOutOfRangeException>();
  }

  [Fact]
  public void CompareToContractTest()
  {
    object x = IntervalQuality.Diminished;
    object y = new IntervalQuality();
    object z = (IntervalQuality) 0;

    ( (IComparable) x ).CompareTo( x )
                       .Should()
                       .Be( 0 ); // Reflexive
    ( (IComparable) x ).CompareTo( y )
                       .Should()
                       .Be( 0 ); // Symmetric
    ( (IComparable) y ).CompareTo( x )
                       .Should()
                       .Be( 0 );
    ( (IComparable) y ).CompareTo( z )
                       .Should()
                       .Be( 0 ); // Transitive
    ( (IComparable) x ).CompareTo( z )
                       .Should()
                       .Be( 0 );
    ( (IComparable) x ).CompareTo( null )
                       .Should()
                       .NotBe( 0 ); // Never equal to null
  }

  [Fact]
  public void DecrementOperatorTest()
  {
    var quality = IntervalQuality.Augmented;
    ( --quality ).Should()
                 .Be( IntervalQuality.Major );
    ( --quality ).Should()
                 .Be( IntervalQuality.Perfect );
    ( --quality ).Should()
                 .Be( IntervalQuality.Minor );
    ( --quality ).Should()
                 .Be( IntervalQuality.Diminished );
  }

  [Fact]
  public void DecrementOperatorThrowsWhenOutOfRangeTest()
  {
    var quality = IntervalQuality.Diminished;
    var act = () => --quality;
    act.Should()
       .Throw<ArgumentOutOfRangeException>();
  }

  [Fact]
  public void EqualsContractTest()
  {
    object x = IntervalQuality.Diminished;
    object y = new IntervalQuality();
    object z = (IntervalQuality) 0;

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
    object actual = IntervalQuality.Diminished;
    actual.Equals( int.MinValue )
          .Should()
          .BeFalse();
  }

  [Fact]
  public void EqualsFailsWithNullTest()
  {
    object actual = IntervalQuality.Diminished;
    actual.Equals( null )
          .Should()
          .BeFalse();
  }

  [Fact]
  public void EqualsSucceedsWithSameObjectTest()
  {
    var actual = IntervalQuality.Diminished;
    actual.Equals( actual )
          .Should()
          .BeTrue();
  }

  [Fact]
  public void GetHashcodeTest()
  {
    var actual = IntervalQuality.Diminished;
    var expected = (IntervalQuality) 0;
    expected.Equals( actual )
            .Should()
            .BeTrue();
    actual.GetHashCode()
          .Should()
          .Be( expected.GetHashCode() );
  }

  [Fact]
  public void IncrementOperatorTest()
  {
    var quality = IntervalQuality.Diminished;
    ( ++quality ).Should()
                 .Be( IntervalQuality.Minor );
    ( ++quality ).Should()
                 .Be( IntervalQuality.Perfect );
    ( ++quality ).Should()
                 .Be( IntervalQuality.Major );
    ( ++quality ).Should()
                 .Be( IntervalQuality.Augmented );
  }

  [Fact]
  public void IncrementOperatorThrowsWhenOutOfRangeTest()
  {
    var quality = IntervalQuality.Augmented;
    var act = () => ++quality;
    act.Should()
       .Throw<ArgumentOutOfRangeException>();
  }

  [Fact]
  public void LogicalOperatorsTest()
  {
    ( IntervalQuality.Diminished == (IntervalQuality) 0 ).Should()
                                                         .BeTrue();
    ( IntervalQuality.Perfect != IntervalQuality.Major ).Should()
                                                        .BeTrue();

    ( IntervalQuality.Diminished < IntervalQuality.Minor ).Should()
                                                          .BeTrue();
    ( IntervalQuality.Diminished <= IntervalQuality.Minor ).Should()
                                                           .BeTrue();
    ( IntervalQuality.Minor < IntervalQuality.Perfect ).Should()
                                                       .BeTrue();
    ( IntervalQuality.Minor <= IntervalQuality.Perfect ).Should()
                                                        .BeTrue();
    ( IntervalQuality.Perfect < IntervalQuality.Major ).Should()
                                                       .BeTrue();
    ( IntervalQuality.Perfect <= IntervalQuality.Major ).Should()
                                                        .BeTrue();
    ( IntervalQuality.Major < IntervalQuality.Augmented ).Should()
                                                         .BeTrue();
    ( IntervalQuality.Major <= IntervalQuality.Augmented ).Should()
                                                          .BeTrue();

    ( IntervalQuality.Augmented > IntervalQuality.Major ).Should()
                                                         .BeTrue();
    ( IntervalQuality.Augmented >= IntervalQuality.Major ).Should()
                                                          .BeTrue();
    ( IntervalQuality.Major > IntervalQuality.Perfect ).Should()
                                                       .BeTrue();
    ( IntervalQuality.Major >= IntervalQuality.Perfect ).Should()
                                                        .BeTrue();
    ( IntervalQuality.Perfect > IntervalQuality.Minor ).Should()
                                                       .BeTrue();
    ( IntervalQuality.Perfect >= IntervalQuality.Minor ).Should()
                                                        .BeTrue();
    ( IntervalQuality.Minor > IntervalQuality.Diminished ).Should()
                                                          .BeTrue();
    ( IntervalQuality.Minor >= IntervalQuality.Diminished ).Should()
                                                           .BeTrue();
  }

  [Fact]
  public void LongNameTest()
  {
    IntervalQuality.Diminished.LongName.Should()
                   .Be( "Diminished" );
    IntervalQuality.Minor.LongName.Should()
                   .Be( "Minor" );
    IntervalQuality.Perfect.LongName.Should()
                   .Be( "Perfect" );
    IntervalQuality.Major.LongName.Should()
                   .Be( "Major" );
    IntervalQuality.Augmented.LongName.Should()
                   .Be( "Augmented" );
  }

  [Fact]
  public void ParseTest()
  {
    IntervalQuality.Parse( "d" )
                   .Should()
                   .Be( IntervalQuality.Diminished );
    IntervalQuality.Parse( "m" )
                   .Should()
                   .Be( IntervalQuality.Minor );
    IntervalQuality.Parse( "P" )
                   .Should()
                   .Be( IntervalQuality.Perfect );
    IntervalQuality.Parse( "M" )
                   .Should()
                   .Be( IntervalQuality.Major );
    IntervalQuality.Parse( "A" )
                   .Should()
                   .Be( IntervalQuality.Augmented );
  }

  [Fact]
  public void ParseThrowsWithInvalidValuesTest()
  {
    var act = () => IntervalQuality.Parse( "X" );
    act.Should()
       .Throw<FormatException>();
  }

  [Fact]
  public void ShortNameTest()
  {
    IntervalQuality.Diminished.ShortName.Should()
                   .Be( "dim" );
    IntervalQuality.Minor.ShortName.Should()
                   .Be( "min" );
    IntervalQuality.Perfect.ShortName.Should()
                   .Be( "Perf" );
    IntervalQuality.Major.ShortName.Should()
                   .Be( "Maj" );
    IntervalQuality.Augmented.ShortName.Should()
                   .Be( "Aug" );
  }

  [Fact]
  public void SubtractOperatorTest()
  {
    ( IntervalQuality.Augmented - 0 ).Should()
                                     .Be( IntervalQuality.Augmented );
    ( IntervalQuality.Augmented - 1 ).Should()
                                     .Be( IntervalQuality.Major );
    ( IntervalQuality.Major - 1 ).Should()
                                 .Be( IntervalQuality.Perfect );
    ( IntervalQuality.Perfect - 1 ).Should()
                                   .Be( IntervalQuality.Minor );
    ( IntervalQuality.Minor - 1 ).Should()
                                 .Be( IntervalQuality.Diminished );
  }

  [Fact]
  public void SubtractOperatorThrowsWhenOutOfRangeTest()
  {
    var act = () => IntervalQuality.Diminished - 1;
    act.Should()
       .Throw<ArgumentOutOfRangeException>();
  }

  [Fact]
  public void SubtractTest()
  {
    IntervalQuality.Augmented.Subtract( 0 )
                   .Should()
                   .Be( IntervalQuality.Augmented );
    IntervalQuality.Augmented.Subtract( 1 )
                   .Should()
                   .Be( IntervalQuality.Major );
    IntervalQuality.Major.Subtract( 1 )
                   .Should()
                   .Be( IntervalQuality.Perfect );
    IntervalQuality.Perfect.Subtract( 1 )
                   .Should()
                   .Be( IntervalQuality.Minor );
    IntervalQuality.Minor.Subtract( 1 )
                   .Should()
                   .Be( IntervalQuality.Diminished );
  }

  [Fact]
  public void SubtractThrowsWhenOutOfRangeTest()
  {
    var act1 = () => IntervalQuality.Diminished.Subtract( 1 );
    act1.Should()
        .Throw<ArgumentOutOfRangeException>();
    var act2 = () => IntervalQuality.Augmented.Subtract( -1 );
    act2.Should()
        .Throw<ArgumentOutOfRangeException>();
  }

  [Fact]
  public void ToStringTest()
  {
    IntervalQuality.Diminished.ToString()
                   .Should()
                   .Be( "Diminished" );
    IntervalQuality.Minor.ToString()
                   .Should()
                   .Be( "Minor" );
    IntervalQuality.Perfect.ToString()
                   .Should()
                   .Be( "Perfect" );
    IntervalQuality.Major.ToString()
                   .Should()
                   .Be( "Major" );
    IntervalQuality.Augmented.ToString()
                   .Should()
                   .Be( "Augmented" );
  }

  [Fact]
  public void TypeSafeCompareToContractTest()
  {
    var x = IntervalQuality.Diminished;
    var y = new IntervalQuality();
    var z = (IntervalQuality) 0;

    x.CompareTo( x )
     .Should()
     .Be( 0 ); // Reflexive
    x.CompareTo( y )
     .Should()
     .Be( 0 ); // Symmetric
    y.CompareTo( x )
     .Should()
     .Be( 0 );
    y.CompareTo( z )
     .Should()
     .Be( 0 ); // Transitive
    x.CompareTo( z )
     .Should()
     .Be( 0 );
    x.CompareTo( null )
     .Should()
     .NotBe( 0 ); // Never equal to null
  }

  [Fact]
  public void TypeSafeEqualsContractTest()
  {
    var x = IntervalQuality.Diminished;
    var y = new IntervalQuality();
    var z = (IntervalQuality) 0;

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
    var actual = IntervalQuality.Diminished;

    // ReSharper disable once SuspiciousTypeConversion.Global
    actual.Equals( int.MinValue )
          .Should()
          .BeFalse();
  }

  [Fact]
  public void TypeSafeEqualsFailsWithNullTest()
  {
    var actual = IntervalQuality.Diminished;
    actual.Equals( null )
          .Should()
          .BeFalse();
  }

  #endregion
}
