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

using System;
using Xunit;

namespace Bach.Model.Test;

public sealed class IntervalQualityTest
{
#region Public Methods

  [Fact]
  public void AdditionOperatorTest()
  {
    Assert.Equal( IntervalQuality.Diminished, IntervalQuality.Diminished + 0 );
    Assert.Equal( IntervalQuality.Minor, IntervalQuality.Diminished + 1 );
    Assert.Equal( IntervalQuality.Perfect, IntervalQuality.Minor + 1 );
    Assert.Equal( IntervalQuality.Major, IntervalQuality.Perfect + 1 );
    Assert.Equal( IntervalQuality.Augmented, IntervalQuality.Major + 1 );
  }

  [Fact]
  public void AdditionOperatorThrowsWhenOutOfRangeTest()
  {
    Assert.Throws<ArgumentOutOfRangeException>( () => IntervalQuality.Augmented + 1 );
  }

  [Fact]
  public void AddTest()
  {
    Assert.Equal( IntervalQuality.Diminished, IntervalQuality.Diminished.Add( 0 ) );
    Assert.Equal( IntervalQuality.Minor, IntervalQuality.Diminished.Add( 1 ) );
    Assert.Equal( IntervalQuality.Perfect, IntervalQuality.Minor.Add( 1 ) );
    Assert.Equal( IntervalQuality.Major, IntervalQuality.Perfect.Add( 1 ) );
    Assert.Equal( IntervalQuality.Augmented, IntervalQuality.Major.Add( 1 ) );
  }

  [Fact]
  public void AddThrowsWhenOutOfRangeTest()
  {
    Assert.Throws<ArgumentOutOfRangeException>( () => IntervalQuality.Diminished.Add( -1 ) );
    Assert.Throws<ArgumentOutOfRangeException>( () => IntervalQuality.Augmented.Add( 1 ) );
  }

  [Fact]
  public void CompareToContractTest()
  {
    object x = IntervalQuality.Diminished;
    object y = new IntervalQuality();
    object z = (IntervalQuality) 0;

    Assert.Equal( 0, ( (IComparable) x ).CompareTo( x ) ); // Reflexive
    Assert.Equal( 0, ( (IComparable) x ).CompareTo( y ) ); // Symmetric
    Assert.Equal( 0, ( (IComparable) y ).CompareTo( x ) );
    Assert.Equal( 0, ( (IComparable) y ).CompareTo( z ) ); // Transitive
    Assert.Equal( 0, ( (IComparable) x ).CompareTo( z ) );
    Assert.NotEqual( 0, ( (IComparable) x ).CompareTo( null ) ); // Never equal to null
  }

  [Fact]
  public void DecrementOperatorTest()
  {
    var quality = IntervalQuality.Augmented;
    Assert.Equal( IntervalQuality.Major, --quality );
    Assert.Equal( IntervalQuality.Perfect, --quality );
    Assert.Equal( IntervalQuality.Minor, --quality );
    Assert.Equal( IntervalQuality.Diminished, --quality );
  }

  [Fact]
  public void DecrementOperatorThrowsWhenOutOfRangeTest()
  {
    var quality = IntervalQuality.Diminished;
    Assert.Throws<ArgumentOutOfRangeException>( () => --quality );
  }

  [Fact]
  public void EqualsContractTest()
  {
    object x = IntervalQuality.Diminished;
    object y = new IntervalQuality();
    object z = (IntervalQuality) 0;

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
    object actual = IntervalQuality.Diminished;
    Assert.False( actual.Equals( int.MinValue ) );
  }

  [Fact]
  public void EqualsFailsWithNullTest()
  {
    object actual = IntervalQuality.Diminished;
    Assert.False( actual.Equals( null ) );
  }

  [Fact]
  public void EqualsSucceedsWithSameObjectTest()
  {
    var actual = IntervalQuality.Diminished;
    Assert.True( actual.Equals( actual ) );
  }

  [Fact]
  public void GetHashcodeTest()
  {
    var actual = IntervalQuality.Diminished;
    var expected = (IntervalQuality) 0;
    Assert.True( expected.Equals( actual ) );
    Assert.Equal( expected.GetHashCode(), actual.GetHashCode() );
  }

  [Fact]
  public void IncrementOperatorTest()
  {
    var quality = IntervalQuality.Diminished;
    Assert.Equal( IntervalQuality.Minor, ++quality );
    Assert.Equal( IntervalQuality.Perfect, ++quality );
    Assert.Equal( IntervalQuality.Major, ++quality );
    Assert.Equal( IntervalQuality.Augmented, ++quality );
  }

  [Fact]
  public void IncrementOperatorThrowsWhenOutOfRangeTest()
  {
    var quality = IntervalQuality.Augmented;
    Assert.Throws<ArgumentOutOfRangeException>( () => ++quality );
  }

  [Fact]
  public void LogicalOperatorsTest()
  {
    Assert.True( IntervalQuality.Diminished == (IntervalQuality) 0 );
    Assert.True( IntervalQuality.Perfect != IntervalQuality.Major );

    Assert.True( IntervalQuality.Diminished < IntervalQuality.Minor );
    Assert.True( IntervalQuality.Diminished <= IntervalQuality.Minor );
    Assert.True( IntervalQuality.Minor < IntervalQuality.Perfect );
    Assert.True( IntervalQuality.Minor <= IntervalQuality.Perfect );
    Assert.True( IntervalQuality.Perfect < IntervalQuality.Major );
    Assert.True( IntervalQuality.Perfect <= IntervalQuality.Major );
    Assert.True( IntervalQuality.Major < IntervalQuality.Augmented );
    Assert.True( IntervalQuality.Major <= IntervalQuality.Augmented );

    Assert.True( IntervalQuality.Augmented > IntervalQuality.Major );
    Assert.True( IntervalQuality.Augmented >= IntervalQuality.Major );
    Assert.True( IntervalQuality.Major > IntervalQuality.Perfect );
    Assert.True( IntervalQuality.Major >= IntervalQuality.Perfect );
    Assert.True( IntervalQuality.Perfect > IntervalQuality.Minor );
    Assert.True( IntervalQuality.Perfect >= IntervalQuality.Minor );
    Assert.True( IntervalQuality.Minor > IntervalQuality.Diminished );
    Assert.True( IntervalQuality.Minor >= IntervalQuality.Diminished );
  }

  [Fact]
  public void LongNameTest()
  {
    Assert.Equal( "Diminished", IntervalQuality.Diminished.LongName );
    Assert.Equal( "Minor", IntervalQuality.Minor.LongName );
    Assert.Equal( "Perfect", IntervalQuality.Perfect.LongName );
    Assert.Equal( "Major", IntervalQuality.Major.LongName );
    Assert.Equal( "Augmented", IntervalQuality.Augmented.LongName );
  }

  [Fact]
  public void ParseTest()
  {
    Assert.Equal( IntervalQuality.Diminished, IntervalQuality.Parse( "d" ) );
    Assert.Equal( IntervalQuality.Minor, IntervalQuality.Parse( "m" ) );
    Assert.Equal( IntervalQuality.Perfect, IntervalQuality.Parse( "P" ) );
    Assert.Equal( IntervalQuality.Major, IntervalQuality.Parse( "M" ) );
    Assert.Equal( IntervalQuality.Augmented, IntervalQuality.Parse( "A" ) );
  }

  [Fact]
  public void ParseThrowsWithInvalidValuesTest()
  {
    Assert.Throws<FormatException>( () => IntervalQuality.Parse( "X" ) );
  }

  [Fact]
  public void ShortNameTest()
  {
    Assert.Equal( "dim", IntervalQuality.Diminished.ShortName );
    Assert.Equal( "min", IntervalQuality.Minor.ShortName );
    Assert.Equal( "Perf", IntervalQuality.Perfect.ShortName );
    Assert.Equal( "Maj", IntervalQuality.Major.ShortName );
    Assert.Equal( "Aug", IntervalQuality.Augmented.ShortName );
  }

  [Fact]
  public void SubtractOperatorTest()
  {
    Assert.Equal( IntervalQuality.Augmented, IntervalQuality.Augmented - 0 );
    Assert.Equal( IntervalQuality.Major, IntervalQuality.Augmented - 1 );
    Assert.Equal( IntervalQuality.Perfect, IntervalQuality.Major - 1 );
    Assert.Equal( IntervalQuality.Minor, IntervalQuality.Perfect - 1 );
    Assert.Equal( IntervalQuality.Diminished, IntervalQuality.Minor - 1 );
  }

  [Fact]
  public void SubtractOperatorThrowsWhenOutOfRangeTest()
  {
    Assert.Throws<ArgumentOutOfRangeException>( () => IntervalQuality.Diminished - 1 );
  }

  [Fact]
  public void SubtractTest()
  {
    Assert.Equal( IntervalQuality.Augmented, IntervalQuality.Augmented.Subtract( 0 ) );
    Assert.Equal( IntervalQuality.Major, IntervalQuality.Augmented.Subtract( 1 ) );
    Assert.Equal( IntervalQuality.Perfect, IntervalQuality.Major.Subtract( 1 ) );
    Assert.Equal( IntervalQuality.Minor, IntervalQuality.Perfect.Subtract( 1 ) );
    Assert.Equal( IntervalQuality.Diminished, IntervalQuality.Minor.Subtract( 1 ) );
  }

  [Fact]
  public void SubtractThrowsWhenOutOfRangeTest()
  {
    Assert.Throws<ArgumentOutOfRangeException>( () => IntervalQuality.Diminished.Subtract( 1 ) );
    Assert.Throws<ArgumentOutOfRangeException>( () => IntervalQuality.Augmented.Subtract( -1 ) );
  }

  [Fact]
  public void ToStringTest()
  {
    Assert.Equal( "Diminished", IntervalQuality.Diminished.ToString() );
    Assert.Equal( "Minor", IntervalQuality.Minor.ToString() );
    Assert.Equal( "Perfect", IntervalQuality.Perfect.ToString() );
    Assert.Equal( "Major", IntervalQuality.Major.ToString() );
    Assert.Equal( "Augmented", IntervalQuality.Augmented.ToString() );
  }

  [Fact]
  public void TypeSafeCompareToContractTest()
  {
    var x = IntervalQuality.Diminished;
    var y = new IntervalQuality();
    var z = (IntervalQuality) 0;

    Assert.Equal( 0, x.CompareTo( x ) ); // Reflexive
    Assert.Equal( 0, x.CompareTo( y ) ); // Symmetric
    Assert.Equal( 0, y.CompareTo( x ) );
    Assert.Equal( 0, y.CompareTo( z ) ); // Transitive
    Assert.Equal( 0, x.CompareTo( z ) );
    Assert.NotEqual( 0, x.CompareTo( null ) ); // Never equal to null
  }

  [Fact]
  public void TypeSafeEqualsContractTest()
  {
    var x = IntervalQuality.Diminished;
    var y = new IntervalQuality();
    var z = (IntervalQuality) 0;

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
    var actual = IntervalQuality.Diminished;

    // ReSharper disable once SuspiciousTypeConversion.Global
    Assert.False( actual.Equals( int.MinValue ) );
  }

  [Fact]
  public void TypeSafeEqualsFailsWithNullTest()
  {
    var actual = IntervalQuality.Diminished;
    Assert.False( actual.Equals( null ) );
  }

#endregion
}
