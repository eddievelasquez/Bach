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
// PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE 
// OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

namespace Bach.Model.Test
{
  using System;
  using Xunit;

  public class AccidentalExtensionsTest
  {
    #region Public Methods

    [Fact]
    public void ToSymbolTest()
    {
      Assert.Equal(Accidental.DoubleFlat.ToSymbol(), "bb");
      Assert.Equal(Accidental.Flat.ToSymbol(), "b");
      Assert.Equal(Accidental.Natural.ToSymbol(), "");
      Assert.Equal(Accidental.Sharp.ToSymbol(), "#");
      Assert.Equal(Accidental.DoubleSharp.ToSymbol(), "##");
    }

    [Fact]
    public void ParseTest()
    {
      Assert.Equal(Accidental.DoubleFlat, AccidentalExtensions.Parse("bb"));
      Assert.Equal(Accidental.Flat, AccidentalExtensions.Parse("b"));
      Assert.Equal(Accidental.Natural, AccidentalExtensions.Parse(""));
      Assert.Equal(Accidental.Sharp, AccidentalExtensions.Parse("#"));
      Assert.Equal(Accidental.DoubleSharp, AccidentalExtensions.Parse("##"));
    }

    [Fact]
    public void TryParseTest()
    {
      Accidental accidental;
      Assert.True(AccidentalExtensions.TryParse("bb", out accidental));
      Assert.Equal(Accidental.DoubleFlat, accidental);
      Assert.True(AccidentalExtensions.TryParse("b", out accidental));
      Assert.Equal(Accidental.Flat, accidental);
      Assert.True(AccidentalExtensions.TryParse(null, out accidental));
      Assert.Equal(Accidental.Natural, accidental);
      Assert.True(AccidentalExtensions.TryParse("", out accidental));
      Assert.Equal(Accidental.Natural, accidental);
      Assert.True(AccidentalExtensions.TryParse("#", out accidental));
      Assert.Equal(Accidental.Sharp, accidental);
      Assert.True(AccidentalExtensions.TryParse("##", out accidental));
      Assert.Equal(Accidental.DoubleSharp, accidental);
      Assert.False(AccidentalExtensions.TryParse("bbb", out accidental));
      Assert.False(AccidentalExtensions.TryParse("###", out accidental));
      Assert.False(AccidentalExtensions.TryParse("$", out accidental));
    }

    [Fact]
    public void ParseThrowsWithInvalidAccidentalSymbolTest()
    {
      Assert.Throws<FormatException>(() => { AccidentalExtensions.Parse("&"); });
    }

    #endregion
  }
}
