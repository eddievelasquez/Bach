//
// Module Name: ToneExtensionsTest.cs
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

  public class ToneExtensionsTest
  {
    #region Public Methods

    [Fact]
    public void NextTest()
    {
      Assert.Equal(Tone.D, Tone.C.Next());
      Assert.Equal(Tone.C, Tone.B.Next());
    }

    [Fact]
    public void PreviousTest()
    {
      Assert.Equal(Tone.C, Tone.D.Previous());
      Assert.Equal(Tone.B, Tone.C.Previous());
    }

    [Fact]
    public void IntervalBetweenTest()
    {
      Assert.Equal(2, Tone.C.IntervalBetween(Tone.D));
      Assert.Equal(2, Tone.D.IntervalBetween(Tone.E));
      Assert.Equal(1, Tone.E.IntervalBetween(Tone.F));
      Assert.Equal(2, Tone.F.IntervalBetween(Tone.G));
      Assert.Equal(2, Tone.G.IntervalBetween(Tone.A));
      Assert.Equal(2, Tone.A.IntervalBetween(Tone.B));
      Assert.Equal(1, Tone.B.IntervalBetween(Tone.C));

      Assert.Equal(0, Tone.C.IntervalBetween(Tone.C));
      Assert.Equal(2, Tone.C.IntervalBetween(Tone.D));
      Assert.Equal(4, Tone.C.IntervalBetween(Tone.E));
      Assert.Equal(5, Tone.C.IntervalBetween(Tone.F));
      Assert.Equal(7, Tone.C.IntervalBetween(Tone.G));
      Assert.Equal(9, Tone.C.IntervalBetween(Tone.A));
      Assert.Equal(11, Tone.C.IntervalBetween(Tone.B));
    }

    #endregion
  }
}
