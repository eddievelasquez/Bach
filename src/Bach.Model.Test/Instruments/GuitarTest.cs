//  
// Module Name: GuitarTest.cs
// Project:     Bach.Model.Test
// Copyright (c) 2014  Eddie Velasquez.
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

namespace Bach.Model.Test.Instruments
{
  using System;
  using Bach.Model.Instruments;
  using Microsoft.VisualStudio.TestTools.UnitTesting;

  [TestClass]
  public class GuitarTest
  {
    #region Public Methods

    [TestMethod]
    public void TestConstructor()
    {
      var guitar = new Guitar();
      Assert.AreEqual(guitar.Name, "Guitar");
      Assert.AreEqual(guitar.StringCount, 6);
      Assert.IsNotNull(guitar.Tunings);
      Assert.AreNotEqual(guitar.Tunings.Count, 0);
    }

    [TestMethod]
    public void AddTuningTest()
    {
      var guitar = new Guitar();
      int tuningCount = guitar.Tunings.Count;
      guitar.Tunings.Add(new Tuning(guitar, "Drop D Test", NoteCollection.Parse("E4,B3,G3,D3,A2,D2")));
      Assert.AreEqual(tuningCount + 1, guitar.Tunings.Count);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void AddTuningFailsWithDifferentInstrumentTuningTest()
    {
      var guitar = new Guitar();
      guitar.Tunings.Add(new Tuning(new Bass(), "Drop D", NoteCollection.Parse("G2,D2,A1,D1")));
    }

    [TestMethod]
    public void EqualsContractTest()
    {
      object x = new Guitar();
      object y = new Guitar();
      object z = new Guitar();

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
      var x = new Guitar();
      var y = new Guitar();
      var z = new Guitar();

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
      object a = new Guitar();
      object b = new Bass();
      Assert.IsFalse(a.Equals(b));
      Assert.IsFalse(b.Equals(a));
      Assert.IsFalse(Equals(a, b));
      Assert.IsFalse(Equals(b, a));
    }

    [TestMethod]
    public void TypeSafeEqualsFailsWithDifferentTypeTest()
    {
      var a = new Guitar();
      var b = new Bass();
      Assert.IsFalse(a.Equals(b));
      Assert.IsFalse(b.Equals(a));
      Assert.IsFalse(Equals(a, b));
      Assert.IsFalse(Equals(b, a));
    }

    [TestMethod]
    public void EqualsFailsWithNullTest()
    {
      object actual = new Guitar();
      Assert.IsFalse(actual.Equals(null));
    }

    [TestMethod]
    public void TypeSafeEqualsFailsWithNullTest()
    {
      var actual = new Guitar();
      Assert.IsFalse(actual.Equals(null));
    }

    [TestMethod]
    public void EqualsSucceedsWithSameObjectTest()
    {
      var actual = new Guitar();
      Assert.IsTrue(actual.Equals(actual));
    }

    [TestMethod]
    public void GetHashcodeTest()
    {
      var actual = new Guitar();
      var expected = new Guitar();
      Assert.IsTrue(expected.Equals(actual));
      Assert.AreEqual(expected.GetHashCode(), actual.GetHashCode());
    }

    #endregion
  }
}
