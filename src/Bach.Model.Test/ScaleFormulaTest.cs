// Module Name: ScaleFormulaTest.cs
// Project:     Bach.Model.Test
// Copyright (c) 2012, 2019  Eddie Velasquez.
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

namespace Bach.Model.Test
{
  using System;
  using System.Collections.Generic;
  using Xunit;

  public class ScaleFormulaTest
  {
    #region Public Methods

    [Fact]
    public void ConstructorWithFormulaTest()
    {
      const string KEY = "Key";
      const string NAME = "Name";
      const string FORMULA = "R,M2,M3";
      var actual = new ScaleFormula(KEY, NAME, FORMULA);

      Assert.Equal(KEY, actual.Key);
      Assert.Equal(NAME, actual.Name);
      Assert.Equal(new[] { Interval.Unison, Interval.MajorSecond, Interval.MajorThird }, actual.Intervals);
      Assert.Equal("Name: P1,M2,M3", actual.ToString());
    }

    [Fact]
    public void ConstructorWithIntervalsTest()
    {
      const string KEY = "Key";
      const string NAME = "Name";
      var actual = new ScaleFormula(KEY, NAME, Interval.Unison, Interval.MajorSecond, Interval.MajorThird);

      Assert.Equal(KEY, actual.Key);
      Assert.Equal(NAME, actual.Name);
      Assert.Equal(new[] { Interval.Unison, Interval.MajorSecond, Interval.MajorThird }, actual.Intervals);
      Assert.Equal("Name: P1,M2,M3", actual.ToString());
    }

    [Fact]
    public void EqualsContractTest()
    {
      object x = new ScaleFormula("Key", "Name", "R,M2,M3");
      object y = new ScaleFormula("Key", "Name", "R,M2,M3");
      object z = new ScaleFormula("Key", "Name", "R,M2,M3");

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
      var x = new ScaleFormula("Key", "Name", "R,M2,M3");
      var y = new ScaleFormula("Key", "Name", "R,M2,M3");
      var z = new ScaleFormula("Key", "Name", "R,M2,M3");

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
      object actual = new ScaleFormula("Key", "Name", "R,M2,M3");
      Assert.False(actual.Equals(int.MinValue));
    }

    [Fact]
    public void TypeSafeEqualsFailsWithDifferentTypeTest()
    {
      var actual = new ScaleFormula("Key", "Name", "R,M2,M3");
      Assert.False(actual.Equals(int.MinValue));
    }

    [Fact]
    public void EqualsFailsWithNullTest()
    {
      object actual = new ScaleFormula("Key", "Name", "R,M2,M3");
      Assert.False(actual.Equals(null));
    }

    [Fact]
    public void TypeSafeEqualsFailsWithNullTest()
    {
      var actual = new ScaleFormula("Key", "Name", "R,M2,M3");
      Assert.False(actual.Equals(null));
    }

    [Fact]
    public void EqualsSucceedsWithSameObjectTest()
    {
      var actual = new ScaleFormula("Key", "Name", "R,M2,M3");
      Assert.True(actual.Equals(actual));
    }

    [Fact]
    public void GetHashcodeTest()
    {
      var actual = new ScaleFormula("Key", "Name", "R,M2,M3");
      var expected = new ScaleFormula("Key", "Name", "R,M2,M3");
      Assert.True(expected.Equals(actual));
      Assert.Equal(expected.GetHashCode(), actual.GetHashCode());
    }

    [Fact]
    public void GetStepsTest()
    {
      TestGetSteps("Major", 2, 2, 1, 2, 2, 2, 1);
      TestGetSteps("NaturalMinor", 2, 1, 2, 2, 1, 2, 2);
      TestGetSteps("MelodicMinor", 2, 1, 2, 2, 2, 2, 1);
      TestGetSteps("HarmonicMinor", 2, 1, 2, 2, 1, 3, 1);
      TestGetSteps("Diminished", 2, 1, 2, 1, 2, 1, 2, 1);
      TestGetSteps("WholeTone", 2, 2, 2, 2, 2, 2);
      TestGetSteps("MinorBlues", 3, 2, 1, 1, 3, 2);
      TestGetSteps("MinorPentatonic", 3, 2, 2, 3, 2);
      TestGetSteps("Pentatonic", 2, 2, 3, 2, 3);
    }

    [Fact]
    public void GenerateTest()
    {
      var formula = new ScaleFormula("Key", "Test", "R,M2,M3");
      using( IEnumerator<Pitch> pitches = formula.Generate(Pitch.MinValue).GetEnumerator() )
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

    [Fact]
    public void IntervalsMustHaveNoDuplicatesTest()
    {
      Assert.Throws<ArgumentException>(() => new ScaleFormula("Key", "Test", "R,M2,M2,M3"));
    }

    [Fact]
    public void IntervalsMustBeSortedTest()
    {
      Assert.Throws<ArgumentException>(() => new ScaleFormula("Key", "Test", "R,M3,M2"));
    }

    #endregion

    #region  Implementation

    private static void TestGetSteps(string scaleName,
                                     params int[] expected)
    {
      ScaleFormula scale = Registry.ScaleFormulas[scaleName];
      Assert.Equal(expected, scale.GetRelativeSteps());
    }

    #endregion
  }
}
