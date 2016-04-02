//  
// Module Name: ChordFormulaTest.cs
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
  using Microsoft.VisualStudio.TestTools.UnitTesting;

  /// <summary>
  ///    This is a test class for ChordFormulaTest and is intended
  ///    to contain all ChordFormulaTest Unit Tests
  /// </summary>
  [TestClass]
  public class ChordFormulaTest
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
    ///    A test for ChordFormula Constructor
    /// </summary>
    [TestMethod]
    public void ChordFormulaConstructorTest()
    {
      const string NAME = "Name";
      const string SYMBOL = "Symbol";
      const string FORMULA = "1,2,3";
      var target = new ChordFormula(NAME, SYMBOL, FORMULA);

      Assert.AreEqual(NAME, target.Name);
      Assert.AreEqual(SYMBOL, target.Symbol);
      Assert.AreEqual("Name: P1,M2,M3", target.ToString());
    }

    [TestMethod]
    public void EqualsContractTest()
    {
      object x = new ChordFormula("Name", "Symbol", "1,2,3");
      object y = new ChordFormula("Name", "Symbol", "1,2,3");
      object z = new ChordFormula("Name", "Symbol", "1,2,3");

      Assert.IsTrue(x.Equals(x)); // Reflexive
      Assert.IsTrue(x.Equals(y)); // Symetric
      Assert.IsTrue(y.Equals(x));
      Assert.IsTrue(y.Equals(z)); // Transitive
      Assert.IsTrue(x.Equals(z));
      Assert.IsFalse(x.Equals(null)); // Never equal to null
    }

    [TestMethod]
    public void TypeSafeEqualsContractTest()
    {
      var x = new ChordFormula("Name", "Symbol", "1,2,3");
      var y = new ChordFormula("Name", "Symbol", "1,2,3");
      var z = new ChordFormula("Name", "Symbol", "1,2,3");

      Assert.IsTrue(x.Equals(x)); // Reflexive
      Assert.IsTrue(x.Equals(y)); // Symetric
      Assert.IsTrue(y.Equals(x));
      Assert.IsTrue(y.Equals(z)); // Transitive
      Assert.IsTrue(x.Equals(z));
      Assert.IsFalse(x.Equals(null)); // Never equal to null
    }

    [TestMethod]
    public void EqualsFailsWithDifferentTypeTest()
    {
      object actual = new ChordFormula("Name", "Symbol", "1,2,3");
      Assert.IsFalse(actual.Equals(int.MinValue));
    }

    [TestMethod]
    public void TypeSafeEqualsFailsWithDifferentTypeTest()
    {
      var actual = new ChordFormula("Name", "Symbol", "1,2,3");
      Assert.IsFalse(actual.Equals(int.MinValue));
    }

    [TestMethod]
    public void EqualsFailsWithNullTest()
    {
      object actual = new ChordFormula("Name", "Symbol", "1,2,3");
      Assert.IsFalse(actual.Equals(null));
    }

    [TestMethod]
    public void TypeSafeEqualsFailsWithNullTest()
    {
      var actual = new ChordFormula("Name", "Symbol", "1,2,3");
      Assert.IsFalse(actual.Equals(null));
    }

    [TestMethod]
    public void EqualsSucceedsWithSameObjectTest()
    {
      var actual = new ChordFormula("Name", "Symbol", "1,2,3");
      Assert.IsTrue(actual.Equals(actual));
    }

    [TestMethod]
    public void GetHashcodeTest()
    {
      var actual = new ChordFormula("Name", "Symbol", "1,2,3");
      var expected = new ChordFormula("Name", "Symbol", "1,2,3");
      Assert.IsTrue(expected.Equals(actual));
      Assert.AreEqual(expected.GetHashCode(), actual.GetHashCode());
    }

    #endregion
  }
}
