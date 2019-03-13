//
// Module Name: AccidentalExtensionsTest.cs
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

  public class AccidentalTest
  {
    [Fact]
    public void ToSymbolTest()
    {
      Assert.Equal("bb", Accidental.DoubleFlat.ToSymbol());
      Assert.Equal("b", Accidental.Flat.ToSymbol());
      Assert.Equal("", Accidental.Natural.ToSymbol());
      Assert.Equal("#", Accidental.Sharp.ToSymbol());
      Assert.Equal("##", Accidental.DoubleSharp.ToSymbol());
    }

    [Fact]
    public void ParseTest()
    {
      Assert.Equal(Accidental.DoubleFlat, Accidental.Parse("bb"));
      Assert.Equal(Accidental.Flat, Accidental.Parse("b"));
      Assert.Equal(Accidental.Natural, Accidental.Parse(""));
      Assert.Equal(Accidental.Sharp, Accidental.Parse("#"));
      Assert.Equal(Accidental.DoubleSharp, Accidental.Parse("##"));
    }

    [Fact]
    public void TryParseTest()
    {
      Assert.True(Accidental.TryParse("bb", out Accidental accidental));
      Assert.Equal(Accidental.DoubleFlat, accidental);
      Assert.True(Accidental.TryParse("b", out accidental));
      Assert.Equal(Accidental.Flat, accidental);
      Assert.True(Accidental.TryParse(null, out accidental));
      Assert.Equal(Accidental.Natural, accidental);
      Assert.True(Accidental.TryParse("", out accidental));
      Assert.Equal(Accidental.Natural, accidental);
      Assert.True(Accidental.TryParse("#", out accidental));
      Assert.Equal(Accidental.Sharp, accidental);
      Assert.True(Accidental.TryParse("##", out accidental));
      Assert.Equal(Accidental.DoubleSharp, accidental);
      Assert.False(Accidental.TryParse("b#", out accidental));
      Assert.False(Accidental.TryParse("#b", out accidental));
      Assert.False(Accidental.TryParse("bbb", out accidental));
      Assert.False(Accidental.TryParse("###", out accidental));
      Assert.False(Accidental.TryParse("$", out accidental));
    }

    [Fact]
    public void ParseThrowsWithInvalidAccidentalSymbolTest()
    {
      Assert.Throws<FormatException>(() => { Accidental.Parse("&"); });
    }
  }
}
