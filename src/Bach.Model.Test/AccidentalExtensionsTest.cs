//  
// Module Name: AccidentalExtensionsTest.cs
// Project:     Bach.Model.Test
// Copyright (c) 2013  Eddie Velasquez.
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
  using Microsoft.VisualStudio.TestTools.UnitTesting;

  /// <summary>
  ///    This is a test class for AccidentalExtensionsTest and is intended
  ///    to contain all AccidentalExtensionsTest Unit Tests
  /// </summary>
  [TestClass]
  public class AccidentalExtensionsTest
  {
    #region Properties

    /// <summary>
    ///    Gets or sets the test context which provides
    ///    information about and functionality for the current test run.
    /// </summary>
    public TestContext TestContext { get; set; }

    #endregion

    #region Public Methods

    /// <summary>
    ///    A test for ToSymbol
    /// </summary>
    [TestMethod]
    public void ToSymbolTest()
    {
      Assert.AreEqual(Accidental.DoubleFlat.ToSymbol(), "bb");
      Assert.AreEqual(Accidental.Flat.ToSymbol(), "b");
      Assert.AreEqual(Accidental.Natural.ToSymbol(), "");
      Assert.AreEqual(Accidental.Sharp.ToSymbol(), "#");
      Assert.AreEqual(Accidental.DoubleSharp.ToSymbol(), "##");
    }

    [TestMethod]
    public void ParseTest()
    {
      Assert.AreEqual(Accidental.DoubleFlat, AccidentalExtensions.Parse("bb"));
      Assert.AreEqual(Accidental.Flat, AccidentalExtensions.Parse("b"));
      Assert.AreEqual(Accidental.Natural, AccidentalExtensions.Parse(""));
      Assert.AreEqual(Accidental.Sharp, AccidentalExtensions.Parse("#"));
      Assert.AreEqual(Accidental.DoubleSharp, AccidentalExtensions.Parse("##"));
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ParseThrowsWithInvalidAccidentalSymbolTest()
    {
      AccidentalExtensions.Parse("&");
    }

    #endregion
  }
}
