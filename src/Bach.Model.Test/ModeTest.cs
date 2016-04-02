//  
// Module Name: ModeTest.cs
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
  using System.Linq;
  using Microsoft.VisualStudio.TestTools.UnitTesting;

  /// <summary>
  ///    This is a test class for ChordTest and is intended
  ///    to contain all ChordTest Unit Tests
  /// </summary>
  [TestClass]
  public class ModeTest
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
    ///    A test for Mode Constructor
    /// </summary>
    [TestMethod]
    public void ModeConstructorTest()
    {
      Note root = Note.Parse("C4");
      ModeFormula formula = ModeFormula.Phrygian;
      var target = new Mode(root, formula);

      Assert.AreEqual(root, target.Root);
      Assert.AreEqual(formula, target.Formula);
      Assert.AreEqual("C Phrygian", target.Name);
      CollectionAssert.AreEqual(target.ToArray(), NoteCollection.Parse("E4,F4,G4,A4,B4,C5,D5"));
    }

    [TestMethod]
    public void ModesTest()
    {
      Note root = Note.Parse("C4");
      TestMode("C4,D4,E4,F4,G4,A4,B4", root, ModeFormula.Ionian);
      TestMode("D4,E4,F4,G4,A4,B4,C5", root, ModeFormula.Dorian);
      TestMode("E4,F4,G4,A4,B4,C5,D5", root, ModeFormula.Phrygian);
      TestMode("F4,G4,A4,B4,C5,D5,E5", root, ModeFormula.Lydian);
      TestMode("G4,A4,B4,C5,D5,E5,F5", root, ModeFormula.Mixolydian);
      TestMode("A4,B4,C5,D5,E5,F5,G5", root, ModeFormula.Aeolian);
      TestMode("B4,C5,D5,E5,F5,G5,A5", root, ModeFormula.Locrian);
    }

    [TestMethod]
    public void EqualsContractTest()
    {
      object x = new Mode(Note.Parse("C4"), ModeFormula.Dorian);
      object y = new Mode(Note.Parse("C4"), ModeFormula.Dorian);
      object z = new Mode(Note.Parse("C4"), ModeFormula.Dorian);

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
      var x = new Mode(Note.Parse("C4"), ModeFormula.Dorian);
      var y = new Mode(Note.Parse("C4"), ModeFormula.Dorian);
      var z = new Mode(Note.Parse("C4"), ModeFormula.Dorian);

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
      object actual = new Mode(Note.Parse("C4"), ModeFormula.Dorian);
      Assert.IsFalse(actual.Equals(int.MinValue));
    }

    [TestMethod]
    public void TypeSafeEqualsFailsWithDifferentTypeTest()
    {
      var actual = new Mode(Note.Parse("C4"), ModeFormula.Dorian);
      Assert.IsFalse(actual.Equals(int.MinValue));
    }

    [TestMethod]
    public void EqualsFailsWithNullTest()
    {
      object actual = new Mode(Note.Parse("C4"), ModeFormula.Dorian);
      Assert.IsFalse(actual.Equals(null));
    }

    [TestMethod]
    public void TypeSafeEqualsFailsWithNullTest()
    {
      var actual = new Mode(Note.Parse("C4"), ModeFormula.Dorian);
      Assert.IsFalse(actual.Equals(null));
    }

    [TestMethod]
    public void EqualsSucceedsWithSameObjectTest()
    {
      var actual = new Mode(Note.Parse("C4"), ModeFormula.Dorian);
      Assert.IsTrue(actual.Equals(actual));
    }

    [TestMethod]
    public void GetHashcodeTest()
    {
      var actual = new Mode(Note.Parse("C4"), ModeFormula.Dorian);
      var expected = new Mode(Note.Parse("C4"), ModeFormula.Dorian);
      Assert.IsTrue(expected.Equals(actual));
      Assert.AreEqual(expected.GetHashCode(), actual.GetHashCode());
    }

    #endregion

    #region Implementation

    private static void TestMode(string expectedNotes, Note root, ModeFormula formula)
    {
      NoteCollection expected = NoteCollection.Parse(expectedNotes);
      CollectionAssert.AreEqual(expected, new Mode(root, formula).Take(expected.Count).ToArray());
    }

    #endregion
  }
}
