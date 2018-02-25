//
// Module Name: IntervalTest.cs
// Project:     Bach.Model.Test
// Copyright (c) 2016  Eddie Velasquez.
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
//  portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A
// PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE
// OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

namespace Bach.Model.Test
{
  using System;
  using Xunit;

  public class IntervalTest
  {
    #region Public Methods

    [Fact]
    public void StepTest()
    {
      Assert.Equal(0, Interval.Perfect1.Steps);
      Assert.Equal(1, Interval.Augmented1.Steps);
      Assert.Equal(0, Interval.Diminished2.Steps);
      Assert.Equal(1, Interval.Minor2.Steps);
      Assert.Equal(2, Interval.Major2.Steps);
      Assert.Equal(3, Interval.Augmented2.Steps);
      Assert.Equal(2, Interval.Diminished3.Steps);
      Assert.Equal(3, Interval.Minor3.Steps);
      Assert.Equal(4, Interval.Major3.Steps);
      Assert.Equal(5, Interval.Augmented3.Steps);
      Assert.Equal(4, Interval.Diminished4.Steps);
      Assert.Equal(5, Interval.Perfect4.Steps);
      Assert.Equal(6, Interval.Augmented4.Steps);
      Assert.Equal(6, Interval.Diminished5.Steps);
      Assert.Equal(7, Interval.Perfect5.Steps);
      Assert.Equal(8, Interval.Augmented5.Steps);
      Assert.Equal(7, Interval.Diminished6.Steps);
      Assert.Equal(8, Interval.Minor6.Steps);
      Assert.Equal(9, Interval.Major6.Steps);
      Assert.Equal(10, Interval.Augmented6.Steps);
      Assert.Equal(9, Interval.Diminished7.Steps);
      Assert.Equal(10, Interval.Minor7.Steps);
      Assert.Equal(11, Interval.Major7.Steps);
      Assert.Equal(12, Interval.Augmented7.Steps);
      Assert.Equal(11, Interval.Diminished8.Steps);
      Assert.Equal(12, Interval.Perfect8.Steps);
      Assert.Equal(13, Interval.Augmented8.Steps);
    }

    [Fact]
    public void EqualsContractTest()
    {
      object x = new Interval(5, IntervalQuality.Perfect);
      object y = new Interval(5, IntervalQuality.Perfect);
      object z = new Interval(5, IntervalQuality.Perfect);

      Assert.True(x.Equals(x)); // Reflexive
      Assert.True(x.Equals(y)); // Symetric
      Assert.True(y.Equals(x));
      Assert.True(y.Equals(z)); // Transitive
      Assert.True(x.Equals(z));
      Assert.False(x.Equals(null)); // Never equal to null
    }

    [Fact]
    public void TypeSafeEqualsContractTest()
    {
      var x = new Interval(5, IntervalQuality.Perfect);
      var y = new Interval(5, IntervalQuality.Perfect);
      var z = new Interval(5, IntervalQuality.Perfect);

      Assert.True(x.Equals(x)); // Reflexive
      Assert.True(x.Equals(y)); // Symetric
      Assert.True(y.Equals(x));
      Assert.True(y.Equals(z)); // Transitive
      Assert.True(x.Equals(z));
      Assert.False(x.Equals(null)); // Never equal to null
    }

    [Fact]
    public void EqualsFailsWithDifferentTypeTest()
    {
      object actual = new Interval(5, IntervalQuality.Perfect);
      Assert.False(actual.Equals(int.MinValue));
    }

    [Fact]
    public void TypeSafeEqualsFailsWithDifferentTypeTest()
    {
      var actual = new Interval(5, IntervalQuality.Perfect);
      Assert.False(actual.Equals(int.MinValue));
    }

    [Fact]
    public void EqualsFailsWithNullTest()
    {
      object actual = new Interval(5, IntervalQuality.Perfect);
      Assert.False(actual.Equals(null));
    }

    [Fact]
    public void TypeSafeEqualsFailsWithNullTest()
    {
      var actual = new Interval(5, IntervalQuality.Perfect);
      Assert.False(actual.Equals(null));
    }

    [Fact]
    public void EqualsSucceedsWithSameObjectTest()
    {
      var actual = new Interval(5, IntervalQuality.Perfect);
      Assert.True(actual.Equals(actual));
    }

    [Fact]
    public void GetHashcodeTest()
    {
      var actual = new Interval(5, IntervalQuality.Perfect);
      var expected = new Interval(5, IntervalQuality.Perfect);
      Assert.True(expected.Equals(actual));
      Assert.Equal(expected.GetHashCode(), actual.GetHashCode());
    }

    [Fact]
    public void EqualitySucceedsWithTwoObjectsTest()
    {
      var lhs = new Interval(5, IntervalQuality.Perfect);
      var rhs = new Interval(5, IntervalQuality.Perfect);
      Assert.True(lhs == rhs);
    }

    [Fact]
    public void EqualitySucceedsWithSameObjectTest()
    {
#pragma warning disable 1718
      var lhs = new Interval(5, IntervalQuality.Perfect);
      Assert.True(lhs == lhs);
#pragma warning restore 1718
    }

    [Fact]
    public void EqualityFailsWithNullTest()
    {
      var lhs = new Interval(5, IntervalQuality.Perfect);
      Assert.False(lhs == null);
    }

    [Fact]
    public void InequalitySucceedsWithTwoObjectsTest()
    {
      var lhs = new Interval(5, IntervalQuality.Perfect);
      var rhs = new Interval(5, IntervalQuality.Augmented);
      Assert.True(lhs != rhs);
    }

    [Fact]
    public void InequalityFailsWithSameObjectTest()
    {
#pragma warning disable 1718
      var lhs = new Interval(5, IntervalQuality.Perfect);
      Assert.False(lhs != lhs);
#pragma warning restore 1718
    }

    [Fact]
    public void TryParseTest()
    {
      Interval actual;
      Assert.True(Interval.TryParse("P1", out actual));
      Assert.Equal(Interval.Perfect1, actual);
      Assert.True(Interval.TryParse("1", out actual));
      Assert.Equal(Interval.Perfect1, actual);
      Assert.True(Interval.TryParse("M2", out actual));
      Assert.Equal(Interval.Major2, actual);
      Assert.True(Interval.TryParse("2", out actual));
      Assert.Equal(Interval.Major2, actual);
      Assert.False(Interval.TryParse("M1", out actual));
      Assert.Equal(Interval.Invalid, actual);
      Assert.False(Interval.TryParse("P2", out actual));
      Assert.Equal(Interval.Invalid, actual);
      Assert.False(Interval.TryParse(null, out actual));
      Assert.False(Interval.TryParse("", out actual));
      Assert.True(Interval.TryParse("  P1", out actual));
      Assert.False(Interval.TryParse("L2", out actual));
      Assert.False(Interval.TryParse("Px", out actual));
    }

    [Fact]
    public void ParseTest()
    {
      Assert.Equal(Interval.Perfect1, Interval.Parse("P1"));
      Assert.Throws<FormatException>(() => Interval.Parse("X2"));
    }

    [Fact]
    public void GetStepsTest()
    {
      Assert.Throws<ArgumentException>(() => Interval.GetSteps(1, IntervalQuality.Diminished));
      Assert.Throws<ArgumentException>(() => Interval.GetSteps(1, IntervalQuality.Minor));
      Assert.Equal(0, Interval.GetSteps(1, IntervalQuality.Perfect));
      Assert.Throws<ArgumentException>(() => Interval.GetSteps(1, IntervalQuality.Major));
      Assert.Equal(1, Interval.GetSteps(1, IntervalQuality.Augmented));
    }

    [Fact]
    public void IsValidTest()
    {
      Assert.False(Interval.IsValid(1, IntervalQuality.Diminished));
      Assert.False(Interval.IsValid(Interval.MinInterval - 1, IntervalQuality.Perfect));
      Assert.False(Interval.IsValid(Interval.MaxInterval + 1, IntervalQuality.Perfect));
      Assert.False(Interval.IsValid(1, IntervalQuality.Diminished - 1));
      Assert.False(Interval.IsValid(1, IntervalQuality.Augmented + 1));
    }

    [Fact]
    public void LogicalOperatorsTest()
    {
      Assert.True(Interval.Perfect1 == Interval.Parse("P1"));
      Assert.True(Interval.Perfect1 != Interval.Perfect4);
      Assert.True(Interval.Perfect1 < Interval.Perfect4);
      Assert.True(Interval.Perfect1 <= Interval.Perfect4);
      Assert.True(Interval.Perfect4 > Interval.Perfect1);
      Assert.True(Interval.Perfect4 >= Interval.Perfect1);
      Assert.True(Interval.Minor3 < Interval.Major3);
    }

    #endregion
  }
}
