//
// Module Name: ScaleFormulaTest.cs
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
  using Xunit;

  public class ScaleFormulaTest
  {
    #region Public Methods

    [Fact]
    public void ConstructorTest()
    {
      var actual = new ScaleFormula("Major", "Major", "R,2,3,4,5,6,7");
      Assert.Equal(Registry.ScaleFormulas["Major"], actual);
      actual = new ScaleFormula(
        "Major",
        "Major",
        new[]
        {
          Interval.Perfect1,
          Interval.Major2,
          Interval.Major3,
          Interval.Perfect4,
          Interval.Perfect5,
          Interval.Major6,
          Interval.Major7
        });
      Assert.Equal(Registry.ScaleFormulas["Major"], actual);
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
      TestGetSteps("Blues", 3, 2, 1, 1, 3, 2);
      TestGetSteps("MinorPentatonic", 3, 2, 2, 3, 2);
      TestGetSteps("Pentatonic", 2, 2, 3, 2, 3);
    }

    #endregion

    #region Implementation

    private static void TestGetSteps(string scaleName,
                                     params int[] expected)
    {
      ScaleFormula scale = Registry.ScaleFormulas[scaleName];
      Assert.Equal(expected, scale.GetRelativeSteps());
    }

    #endregion
  }
}
