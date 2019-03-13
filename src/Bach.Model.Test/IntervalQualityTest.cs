﻿//
// Module Name: IntervalQualityExtensionsTest.cs
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
// PARTICULAR PURPOSE AND NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE
// OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

namespace Bach.Model.Test
{
  using System;
  using Xunit;

  public class IntervalQualityTest
  {
    #region Public Methods

    [Fact]
    public void AddTest()
    {
      Assert.Equal(IntervalQuality.Unknown, IntervalQuality.Unknown.Add(0));
      Assert.Equal(IntervalQuality.Diminished, IntervalQuality.Unknown.Add(1));
      Assert.Equal(IntervalQuality.Minor, IntervalQuality.Diminished.Add(1));
      Assert.Equal(IntervalQuality.Perfect, IntervalQuality.Minor.Add(1));
      Assert.Equal(IntervalQuality.Major, IntervalQuality.Perfect.Add(1));
      Assert.Equal(IntervalQuality.Augmented, IntervalQuality.Major.Add(1));
    }

    [Fact]
    public void AdditionOperatorTest()
    {
      Assert.Equal(IntervalQuality.Unknown, IntervalQuality.Unknown + 0);
      Assert.Equal(IntervalQuality.Diminished, IntervalQuality.Unknown + 1);
      Assert.Equal(IntervalQuality.Minor, IntervalQuality.Diminished + 1);
      Assert.Equal(IntervalQuality.Perfect, IntervalQuality.Minor + 1);
      Assert.Equal(IntervalQuality.Major, IntervalQuality.Perfect + 1);
      Assert.Equal(IntervalQuality.Augmented, IntervalQuality.Major + 1);
    }

    [Fact]
    public void IncrementOperatorTest()
    {
      IntervalQuality quality = IntervalQuality.Unknown;
      Assert.Equal(IntervalQuality.Diminished, ++quality);
      Assert.Equal(IntervalQuality.Minor, ++quality);
      Assert.Equal(IntervalQuality.Perfect, ++quality);
      Assert.Equal(IntervalQuality.Major, ++quality);
      Assert.Equal(IntervalQuality.Augmented, ++quality);
    }

    [Fact]
    public void SubtractTest()
    {
      Assert.Equal(IntervalQuality.Augmented, IntervalQuality.Augmented.Subtract(0));
      Assert.Equal(IntervalQuality.Major, IntervalQuality.Augmented.Subtract(1));
      Assert.Equal(IntervalQuality.Perfect, IntervalQuality.Major.Subtract(1));
      Assert.Equal(IntervalQuality.Minor, IntervalQuality.Perfect.Subtract(1));
      Assert.Equal(IntervalQuality.Diminished, IntervalQuality.Minor.Subtract(1));
      Assert.Equal(IntervalQuality.Unknown, IntervalQuality.Diminished.Subtract(1));
    }

    [Fact]
    public void DecrementOperatorTest()
    {
      IntervalQuality quality = IntervalQuality.Augmented;
      Assert.Equal(IntervalQuality.Major, --quality);
      Assert.Equal(IntervalQuality.Perfect, --quality);
      Assert.Equal(IntervalQuality.Minor, --quality);
      Assert.Equal(IntervalQuality.Diminished, --quality);
      Assert.Equal(IntervalQuality.Unknown, --quality);
    }

    [Fact]
    public void SubtractOperatorTest()
    {
      Assert.Equal(IntervalQuality.Augmented, IntervalQuality.Augmented - 0);
      Assert.Equal(IntervalQuality.Major, IntervalQuality.Augmented - 1);
      Assert.Equal(IntervalQuality.Perfect, IntervalQuality.Major - 1);
      Assert.Equal(IntervalQuality.Minor, IntervalQuality.Perfect - 1);
      Assert.Equal(IntervalQuality.Diminished, IntervalQuality.Minor - 1);
      Assert.Equal(IntervalQuality.Unknown, IntervalQuality.Diminished - 1);
    }

    [Fact]
    public void ParseTest()
    {
      Assert.Equal(IntervalQuality.Perfect, IntervalQuality.Parse("P"));
      Assert.Throws<FormatException>(() => IntervalQuality.Parse("X"));
    }

    [Fact]
    public void ShortNameTest()
    {
      Assert.Equal("dim", IntervalQuality.Diminished.ShortName);
      Assert.Equal("min", IntervalQuality.Minor.ShortName);
      Assert.Equal("Perf", IntervalQuality.Perfect.ShortName);
      Assert.Equal("Maj", IntervalQuality.Major.ShortName);
      Assert.Equal("Aug", IntervalQuality.Augmented.ShortName);
    }

    [Fact]
    public void LongNameTest()
    {
      Assert.Equal("diminished", IntervalQuality.Diminished.LongName);
      Assert.Equal("minor", IntervalQuality.Minor.LongName);
      Assert.Equal("perfect", IntervalQuality.Perfect.LongName);
      Assert.Equal("major", IntervalQuality.Major.LongName);
      Assert.Equal("augmented", IntervalQuality.Augmented.LongName);
    }

    [Fact]
    public void EqualsContractTest()
    {
      object x = IntervalQuality.Diminished;
      object y = new IntervalQuality();
      object z = (IntervalQuality)0;

      Assert.True(x.Equals(x)); // Reflexive
      Assert.True(x.Equals(y)); // Symmetric
      Assert.True(y.Equals(x));
      Assert.True(y.Equals(z)); // Transitive
      Assert.True(x.Equals(z));
      Assert.False(x.Equals(null)); // Never equal to null
    }

    [Fact]
    public void TypeSafeEqualsContractTest()
    {
      var x = IntervalQuality.Diminished;
      var y = new IntervalQuality();
      var z = (IntervalQuality)0;

      Assert.True(x.Equals(x)); // Reflexive
      Assert.True(x.Equals(y)); // Symmetric
      Assert.True(y.Equals(x));
      Assert.True(y.Equals(z)); // Transitive
      Assert.True(x.Equals(z));
      Assert.False(x.Equals(null)); // Never equal to null
    }

    [Fact]
    public void EqualsFailsWithDifferentTypeTest()
    {
      object actual = IntervalQuality.Diminished;
      Assert.False(actual.Equals(int.MinValue));
    }

    [Fact]
    public void TypeSafeEqualsFailsWithDifferentTypeTest()
    {
      var actual = IntervalQuality.Diminished;
      Assert.False(actual.Equals(int.MinValue));
    }

    [Fact]
    public void EqualsFailsWithNullTest()
    {
      object actual = IntervalQuality.Diminished;
      Assert.False(actual.Equals(null));
    }

    [Fact]
    public void TypeSafeEqualsFailsWithNullTest()
    {
      var actual = IntervalQuality.Diminished;
      Assert.False(actual.Equals(null));
    }

    [Fact]
    public void EqualsSucceedsWithSameObjectTest()
    {
      var actual = IntervalQuality.Diminished;
      Assert.True(actual.Equals(actual));
    }

    [Fact]
    public void GetHashcodeTest()
    {
      var actual = IntervalQuality.Diminished;
      var expected = (IntervalQuality)0;
      Assert.True(expected.Equals(actual));
      Assert.Equal(expected.GetHashCode(), actual.GetHashCode());
    }

    [Fact]
    public void CompareToContractTest()
    {
      object x = IntervalQuality.Diminished;
      object y = new IntervalQuality();
      object z = (IntervalQuality)0;

      Assert.Equal(0, ( (IComparable)x ).CompareTo(x)); // Reflexive
      Assert.Equal(0, ( (IComparable)x ).CompareTo(y)); // Symmetric
      Assert.Equal(0, ( (IComparable)y ).CompareTo(x));
      Assert.Equal(0, ( (IComparable)y ).CompareTo(z)); // Transitive
      Assert.Equal(0, ( (IComparable)x ).CompareTo(z));
      Assert.NotEqual(0, ( (IComparable)x ).CompareTo(null)); // Never equal to null
    }

    [Fact]
    public void TypeSafeCompareToContractTest()
    {
      var x = IntervalQuality.Diminished;
      var y = new IntervalQuality();
      var z = (IntervalQuality)0;

      Assert.Equal(0, x.CompareTo(x)); // Reflexive
      Assert.Equal(0, x.CompareTo(y)); // Symmetric
      Assert.Equal(0, y.CompareTo(x));
      Assert.Equal(0, y.CompareTo(z)); // Transitive
      Assert.Equal(0, x.CompareTo(z));
      Assert.NotEqual(0, x.CompareTo(null)); // Never equal to null
    }

    #endregion
  }
}