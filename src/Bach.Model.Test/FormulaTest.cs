﻿//
// Module Name: FormulaTest.cs
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
  using System.Collections.Generic;
  using Xunit;

  public class FormulaTest
  {
    [Fact]
    public void ConstructorTest()
    {
      var actual = new Formula("Major",
                               "Major",
                               new[]
                               {
                                 Interval.Unison,
                                 Interval.MajorSecond,
                                 Interval.MajorThird,
                                 Interval.Fourth,
                                 Interval.Fifth,
                                 Interval.MajorSixth,
                                 Interval.MajorSeventh
                               });
      Assert.Equal("Major", actual.Key);
      Assert.Equal("Major", actual.Name);
      Assert.Equal(7, actual.IntervalCount);
    }

    [Fact]
    public void EqualsContractTest()
    {
      object x = new Formula("Key", "Test", "R,2,3");
      object y = new Formula("Key", "Test", "R,2,3");
      object z = new Formula("Key", "Test", "R,2,3");

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
      var x = new Formula("Key", "Test", "R,2,3");
      var y = new Formula("Key", "Test", "R,2,3");
      var z = new Formula("Key", "Test", "R,2,3");

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
      object actual = new Formula("Key", "Test", "R,2,3");
      Assert.False(actual.Equals(int.MinValue));
    }

    [Fact]
    public void TypeSafeEqualsFailsWithDifferentTypeTest()
    {
      var actual = new Formula("Key", "Test", "R,2,3");
      Assert.False(actual.Equals(int.MinValue));
    }

    [Fact]
    public void EqualsFailsWithNullTest()
    {
      object actual = new Formula("Key", "Test", "R,2,3");
      Assert.False(actual.Equals(null));
    }

    [Fact]
    public void TypeSafeEqualsFailsWithNullTest()
    {
      var actual = new Formula("Key", "Test", "R,2,3");
      Assert.False(actual.Equals(null));
    }

    [Fact]
    public void EqualsSucceedsWithSameObjectTest()
    {
      var actual = new Formula("Key", "Test", "R,2,3");
      Assert.True(actual.Equals(actual));
    }

    [Fact]
    public void GetHashcodeTest()
    {
      var actual = new Formula("Key", "Test", "R,2,3");
      var expected = new Formula("Key", "Test", "R,2,3");
      Assert.True(expected.Equals(actual));
      Assert.Equal(expected.GetHashCode(), actual.GetHashCode());
    }

    [Fact]
    public void GenerateTest()
    {
      var formula = new Formula("Key", "Test", "R,2,3");
      using( var pitches = formula.Generate(Pitch.MinValue).GetEnumerator() )
      {
        var count = 0;
        while( pitches.MoveNext() )
        {
          Assert.True(pitches.Current <= Pitch.MaxValue);
          ++count;
        }

        // 3 notes per octave, 10 octaves total.
        Assert.Equal(30, count);
      }
    }
  }
}
