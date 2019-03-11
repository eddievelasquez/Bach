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
// PARTICULAR PURPOSE AND NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE
// OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

namespace Bach.Model.Test
{
  using Xunit;

  public class NoteNameTest
  {
    [Fact]
    public void AddTest()
    {
      Assert.Equal(NoteName.C, NoteName.C.Add(0));
      Assert.Equal(NoteName.D, NoteName.C.Add(1));
      Assert.Equal(NoteName.E, NoteName.C.Add(2));
      Assert.Equal(NoteName.B, NoteName.C.Add(6));
      Assert.Equal(NoteName.C, NoteName.C.Add(7));
      Assert.Equal(NoteName.D, NoteName.C.Add(8));
    }

    [Fact]
    public void SubtractTest()
    {
      Assert.Equal(NoteName.B, NoteName.B.Subtract(0));
      Assert.Equal(NoteName.A, NoteName.B.Subtract(1));
      Assert.Equal(NoteName.G, NoteName.B.Subtract(2));
      Assert.Equal(NoteName.C, NoteName.B.Subtract(6));
      Assert.Equal(NoteName.B, NoteName.B.Subtract(7));
      Assert.Equal(NoteName.A, NoteName.B.Subtract(8));
    }

    [Fact]
    public void AdditionOperatorTest()
    {
      Assert.Equal(NoteName.C, NoteName.C + 0);
      Assert.Equal(NoteName.D, NoteName.C + 1);
      Assert.Equal(NoteName.E, NoteName.C + 2);
      Assert.Equal(NoteName.B, NoteName.C + 6);
      Assert.Equal(NoteName.C, NoteName.C + 7);
      Assert.Equal(NoteName.D, NoteName.C + 8);
      Assert.Equal(NoteName.C, NoteName.B + 1);
    }

    [Fact]
    public void SubtractionOperatorTest()
    {
      Assert.Equal(NoteName.B, NoteName.B - 0);
      Assert.Equal(NoteName.A, NoteName.B - 1);
      Assert.Equal(NoteName.G, NoteName.B - 2);
      Assert.Equal(NoteName.C, NoteName.B - 6);
      Assert.Equal(NoteName.B, NoteName.B - 7);
      Assert.Equal(NoteName.A, NoteName.B - 8);
      Assert.Equal(NoteName.C, NoteName.D - 1);
      Assert.Equal(NoteName.B, NoteName.C - 1);
    }

    [Fact]
    public void SemitonesBetweenTest()
    {
      Assert.Equal(2, NoteName.C.SemitonesBetween(NoteName.D));
      Assert.Equal(2, NoteName.D.SemitonesBetween(NoteName.E));
      Assert.Equal(1, NoteName.E.SemitonesBetween(NoteName.F));
      Assert.Equal(2, NoteName.F.SemitonesBetween(NoteName.G));
      Assert.Equal(2, NoteName.G.SemitonesBetween(NoteName.A));
      Assert.Equal(2, NoteName.A.SemitonesBetween(NoteName.B));
      Assert.Equal(1, NoteName.B.SemitonesBetween(NoteName.C));

      Assert.Equal(0, NoteName.C.SemitonesBetween(NoteName.C));
      Assert.Equal(2, NoteName.C.SemitonesBetween(NoteName.D));
      Assert.Equal(4, NoteName.C.SemitonesBetween(NoteName.E));
      Assert.Equal(5, NoteName.C.SemitonesBetween(NoteName.F));
      Assert.Equal(7, NoteName.C.SemitonesBetween(NoteName.G));
      Assert.Equal(9, NoteName.C.SemitonesBetween(NoteName.A));
      Assert.Equal(11, NoteName.C.SemitonesBetween(NoteName.B));
    }
  }
}
