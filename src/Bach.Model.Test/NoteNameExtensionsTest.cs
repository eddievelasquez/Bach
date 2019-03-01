//
// Module Name: NoteNameExtensionsTest.cs
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

  public class NoteNameExtensionsTest
  {
    #region Public Methods

    [Fact]
    public void NextTest()
    {
      Assert.Equal(NoteName.D, NoteName.C.Next());
      Assert.Equal(NoteName.C, NoteName.B.Next());
    }

    [Fact]
    public void PreviousTest()
    {
      Assert.Equal(NoteName.C, NoteName.D.Previous());
      Assert.Equal(NoteName.B, NoteName.C.Previous());
    }

    [Fact]
    public void IntervalBetweenTest()
    {
      Assert.Equal(2, NoteName.C.IntervalBetween(NoteName.D));
      Assert.Equal(2, NoteName.D.IntervalBetween(NoteName.E));
      Assert.Equal(1, NoteName.E.IntervalBetween(NoteName.F));
      Assert.Equal(2, NoteName.F.IntervalBetween(NoteName.G));
      Assert.Equal(2, NoteName.G.IntervalBetween(NoteName.A));
      Assert.Equal(2, NoteName.A.IntervalBetween(NoteName.B));
      Assert.Equal(1, NoteName.B.IntervalBetween(NoteName.C));

      Assert.Equal(0, NoteName.C.IntervalBetween(NoteName.C));
      Assert.Equal(2, NoteName.C.IntervalBetween(NoteName.D));
      Assert.Equal(4, NoteName.C.IntervalBetween(NoteName.E));
      Assert.Equal(5, NoteName.C.IntervalBetween(NoteName.F));
      Assert.Equal(7, NoteName.C.IntervalBetween(NoteName.G));
      Assert.Equal(9, NoteName.C.IntervalBetween(NoteName.A));
      Assert.Equal(11, NoteName.C.IntervalBetween(NoteName.B));
    }

    #endregion
  }
}
