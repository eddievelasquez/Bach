//
// Module Name: ToneNameExtensionsTest.cs
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

  public class ToneNameExtensionsTest
  {
    #region Public Methods

    [Fact]
    public void NextTest()
    {
      Assert.Equal(ToneName.D, ToneName.C.Next());
      Assert.Equal(ToneName.C, ToneName.B.Next());
    }

    [Fact]
    public void PreviousTest()
    {
      Assert.Equal(ToneName.C, ToneName.D.Previous());
      Assert.Equal(ToneName.B, ToneName.C.Previous());
    }

    [Fact]
    public void IntervalBetweenTest()
    {
      Assert.Equal(2, ToneName.C.IntervalBetween(ToneName.D));
      Assert.Equal(2, ToneName.D.IntervalBetween(ToneName.E));
      Assert.Equal(1, ToneName.E.IntervalBetween(ToneName.F));
      Assert.Equal(2, ToneName.F.IntervalBetween(ToneName.G));
      Assert.Equal(2, ToneName.G.IntervalBetween(ToneName.A));
      Assert.Equal(2, ToneName.A.IntervalBetween(ToneName.B));
      Assert.Equal(1, ToneName.B.IntervalBetween(ToneName.C));

      Assert.Equal(0, ToneName.C.IntervalBetween(ToneName.C));
      Assert.Equal(2, ToneName.C.IntervalBetween(ToneName.D));
      Assert.Equal(4, ToneName.C.IntervalBetween(ToneName.E));
      Assert.Equal(5, ToneName.C.IntervalBetween(ToneName.F));
      Assert.Equal(7, ToneName.C.IntervalBetween(ToneName.G));
      Assert.Equal(9, ToneName.C.IntervalBetween(ToneName.A));
      Assert.Equal(11, ToneName.C.IntervalBetween(ToneName.B));
    }

    #endregion
  }
}
