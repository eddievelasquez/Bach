//  
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
// PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE 
// OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

namespace Bach.Model.Test
{
  using System;
  using Xunit;

  public class IntervalQualityExtensionsTest
  {
    [Fact]
    public void ParseTest()
    {
      Assert.Equal(IntervalQuality.Perfect, IntervalQualityExtensions.Parse("P"));
      Assert.Throws<FormatException>(() => IntervalQualityExtensions.Parse("X"));
    }

    [Fact]
    public void ShortNameTest()
    {
      Assert.Equal("dim", IntervalQuality.Diminished.ShortName());
      Assert.Equal("min", IntervalQuality.Minor.ShortName());
      Assert.Equal("Perf", IntervalQuality.Perfect.ShortName());
      Assert.Equal("Maj", IntervalQuality.Major.ShortName());
      Assert.Equal("Aug", IntervalQuality.Augmented.ShortName());
      Assert.Throws<ArgumentOutOfRangeException>(() => (IntervalQuality.Diminished - 1).ShortName());
      Assert.Throws<ArgumentOutOfRangeException>(() => (IntervalQuality.Augmented + 1).ShortName());
    }

    [Fact]
    public void LongNameTest()
    {
      Assert.Equal("diminished", IntervalQuality.Diminished.LongName());
      Assert.Equal("minor", IntervalQuality.Minor.LongName());
      Assert.Equal("perfect", IntervalQuality.Perfect.LongName());
      Assert.Equal("major", IntervalQuality.Major.LongName());
      Assert.Equal("augmented", IntervalQuality.Augmented.LongName());
      Assert.Throws<ArgumentOutOfRangeException>(() => (IntervalQuality.Diminished - 1).LongName());
      Assert.Throws<ArgumentOutOfRangeException>(() => (IntervalQuality.Augmented + 1).LongName());
    }


  }
}
