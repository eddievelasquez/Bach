//  
// Module Name: TuningTest.cs
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
  using System.Linq;
  using Bach.Model.Instruments;
  using Microsoft.VisualStudio.TestTools.UnitTesting;

  [TestClass]
  public class TuningTest
  {
    #region Public Methods

    [TestMethod]
    public void TestConstructor()
    {
      var guitar = new Guitar();
      var actual = new Tuning(guitar, "Drop D", NoteCollection.Parse("E4,B3,G3,D3,A2,D2"));
      Assert.AreEqual(guitar, actual.Instrument);
      Assert.AreEqual("Drop D", actual.Name);
      Assert.IsNotNull(actual.Notes);
      Assert.AreEqual(6, actual.Notes.Length);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ConstructorFailsWithNullInstrumentTest()
    {
      new Tuning(null, "Drop D", NoteCollection.Parse("E4,B3,G3,D3,A2,D2"));
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ConstructorFailsWithNullNameTest()
    {
      var guitar = new Guitar();
      new Tuning(guitar, null, NoteCollection.Parse("E4,B3,G3,D3,A2,D2"));
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ConstructorFailsWithEmptyNameTest()
    {
      var guitar = new Guitar();
      new Tuning(guitar, "", NoteCollection.Parse("E4,B3,G3,D3,A2,D2"));
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ConstructorFailsWithNullNoteCollectionTest()
    {
      var guitar = new Guitar();
      new Tuning(guitar, "", (NoteCollection) null);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ConstructorFailsWithNullNoteArrayTest()
    {
      var guitar = new Guitar();
      new Tuning(guitar, "", (Note[]) null);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void ConstructorFailsWithInvalidNoteCollectionCountTest()
    {
      var guitar = new Guitar();
      new Tuning(guitar, "Drop D", NoteCollection.Parse("E4,B3,G3,D3,A2"));
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void ConstructorFailsWithInvalidNoteArrayLengthTest()
    {
      var guitar = new Guitar();
      new Tuning(guitar, "Drop D", NoteCollection.Parse("E4,B3,G3,D3,A2").ToArray());
    }

    [TestMethod]
    public void EqualsContractTest()
    {
      object x = new Tuning(new Guitar(), "Drop D", NoteCollection.Parse("E4,B3,G3,D3,A2,D2"));
      object y = new Tuning(new Guitar(), "Drop D", NoteCollection.Parse("E4,B3,G3,D3,A2,D2"));
      object z = new Tuning(new Guitar(), "Drop D", NoteCollection.Parse("E4,B3,G3,D3,A2,D2"));

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
      var x = new Tuning(new Guitar(), "Drop D", NoteCollection.Parse("E4,B3,G3,D3,A2,D2"));
      var y = new Tuning(new Guitar(), "Drop D", NoteCollection.Parse("E4,B3,G3,D3,A2,D2"));
      var z = new Tuning(new Guitar(), "Drop D", NoteCollection.Parse("E4,B3,G3,D3,A2,D2"));

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
      object a = new Tuning(new Guitar(), "Drop D", NoteCollection.Parse("E4,B3,G3,D3,A2,D2"));
      object b = new Guitar();
      Assert.IsFalse(a.Equals(b));
      Assert.IsFalse(b.Equals(a));
      Assert.IsFalse(Equals(a, b));
      Assert.IsFalse(Equals(b, a));
    }

    [TestMethod]
    public void TypeSafeEqualsFailsWithDifferentTypeTest()
    {
      var a = new Tuning(new Guitar(), "Drop D", NoteCollection.Parse("E4,B3,G3,D3,A2,D2"));
      var b = new Guitar();
      Assert.IsFalse(a.Equals(b));
      Assert.IsFalse(b.Equals(a));
      Assert.IsFalse(Equals(a, b));
      Assert.IsFalse(Equals(b, a));
    }

    [TestMethod]
    public void EqualsFailsWithNullTest()
    {
      object actual = new Tuning(new Guitar(), "Drop D", NoteCollection.Parse("E4,B3,G3,D3,A2,D2"));
      Assert.IsFalse(actual.Equals(null));
    }

    [TestMethod]
    public void TypeSafeEqualsFailsWithNullTest()
    {
      var actual = new Tuning(new Guitar(), "Drop D", NoteCollection.Parse("E4,B3,G3,D3,A2,D2"));
      Assert.IsFalse(actual.Equals(null));
    }

    [TestMethod]
    public void EqualsSucceedsWithSameObjectTest()
    {
      var actual = new Tuning(new Guitar(), "Drop D", NoteCollection.Parse("E4,B3,G3,D3,A2,D2"));
      Assert.IsTrue(actual.Equals(actual));
    }

    [TestMethod]
    public void GetHashcodeTest()
    {
      var actual = new Tuning(new Guitar(), "Drop D", NoteCollection.Parse("E4,B3,G3,D3,A2,D2"));
      var expected = new Tuning(new Guitar(), "Drop D", NoteCollection.Parse("E4,B3,G3,D3,A2,D2"));
      Assert.IsTrue(expected.Equals(actual));
      Assert.AreEqual(expected.GetHashCode(), actual.GetHashCode());
    }

    #endregion
  }
}
