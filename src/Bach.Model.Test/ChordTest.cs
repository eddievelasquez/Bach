//  
// Module Name: ChordTest.cs
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
  public class ChordTest
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
    ///    A test for Chord Constructor
    /// </summary>
    [TestMethod]
    public void ChordConstructorTest()
    {
      Note root = Note.Parse("C4");
      ChordFormula formula = ChordFormula.Minor;
      var target = new Chord(root, formula);

      Assert.AreEqual(root, target.Root);
      Assert.AreEqual(formula, target.Formula);
      Assert.AreEqual("Cm", target.Name);
      CollectionAssert.AreEqual(target.Notes, NoteCollection.Parse("C,Eb,G"));
    }

    [TestMethod]
    public void ChordsTest()
    {
      Note root = Note.Parse("C4");
      TestChord("C4,E4,G4", root, ChordFormula.Major);
      TestChord("C4,E4,G4,B4", root, ChordFormula.Major7);
      TestChord("C4,E4,G4,B4,D5", root, ChordFormula.Major9);
      TestChord("C4,E4,G4,B4,D5,F5", root, ChordFormula.Major11);
      TestChord("C4,E4,G4,B4,D5,F5,A5", root, ChordFormula.Major13);
      TestChord("C4,Eb4,G4", root, ChordFormula.Minor);
      TestChord("C4,Eb4,G4,Bb4", root, ChordFormula.Minor7);
      TestChord("C4,Eb4,G4,Bb4,D5", root, ChordFormula.Minor9);
      TestChord("C4,Eb4,G4,Bb4,D5,F5", root, ChordFormula.Minor11);
      TestChord("C4,Eb4,G4,Bb4,D5,F5,A5", root, ChordFormula.Minor13);
      TestChord("C4,E4,G4,Bb4", root, ChordFormula.Dominant7);
      TestChord("C4,E4,G4,Bb4,D5", root, ChordFormula.Dominant9);
      TestChord("C4,E4,G4,Bb4,D5,F5", root, ChordFormula.Dominant11);
      TestChord("C4,E4,G4,Bb4,D5,F5,A5", root, ChordFormula.Dominant13);
      TestChord("C4,E4,G4,A4,D5", root, ChordFormula.SixNine);
      TestChord("C4,E4,G4,D5", root, ChordFormula.AddNine);
      TestChord("C4,Eb4,Gb4", root, ChordFormula.Diminished);
      TestChord("C4,Eb4,Gb4,Bbb4", root, ChordFormula.Diminished7);
      TestChord("C4,Eb4,Gb4,Bb4", root, ChordFormula.HalfDiminished);
      TestChord("C4,E4,G#4", root, ChordFormula.Augmented);
    }

    [TestMethod]
    public void EqualsContractTest()
    {
      object x = new Chord(Note.Parse("C4"), ChordFormula.Major);
      object y = new Chord(Note.Parse("C4"), ChordFormula.Major);
      object z = new Chord(Note.Parse("C4"), ChordFormula.Major);

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
      var x = new Chord(Note.Parse("C4"), ChordFormula.Major);
      var y = new Chord(Note.Parse("C4"), ChordFormula.Major);
      var z = new Chord(Note.Parse("C4"), ChordFormula.Major);

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
      object actual = new Chord(Note.Parse("C4"), ChordFormula.Major);
      Assert.IsFalse(actual.Equals(int.MinValue));
    }

    [TestMethod]
    public void TypeSafeEqualsFailsWithDifferentTypeTest()
    {
      var actual = new Chord(Note.Parse("C4"), ChordFormula.Major);
      Assert.IsFalse(actual.Equals(int.MinValue));
    }

    [TestMethod]
    public void EqualsFailsWithNullTest()
    {
      object actual = new Chord(Note.Parse("C4"), ChordFormula.Major);
      Assert.IsFalse(actual.Equals(null));
    }

    [TestMethod]
    public void TypeSafeEqualsFailsWithNullTest()
    {
      var actual = new Chord(Note.Parse("C4"), ChordFormula.Major);
      Assert.IsFalse(actual.Equals(null));
    }

    [TestMethod]
    public void EqualsSucceedsWithSameObjectTest()
    {
      var actual = new Chord(Note.Parse("C4"), ChordFormula.Major);
      Assert.IsTrue(actual.Equals(actual));
    }

    [TestMethod]
    public void GetHashcodeTest()
    {
      var actual = new Chord(Note.Parse("C4"), ChordFormula.Major);
      var expected = new Chord(Note.Parse("C4"), ChordFormula.Major);
      Assert.IsTrue(expected.Equals(actual));
      Assert.AreEqual(expected.GetHashCode(), actual.GetHashCode());
    }

    [TestMethod]
    public void InvertTest()
    {
      var c4 = new Chord(Note.Parse("C4"), ChordFormula.Major);
      NoteCollection firstInversion = NoteCollection.Parse("E4,G4,C5");
      Chord actual = c4.Invert(1);
      Assert.IsNotNull(actual);
      CollectionAssert.AreEqual(firstInversion, actual.Notes);

      NoteCollection secondInversion = NoteCollection.Parse("G4,C5,E5");
      actual = c4.Invert(2);
      Assert.IsNotNull(actual);
      CollectionAssert.AreEqual(secondInversion, actual.Notes);

      NoteCollection thirdInversion = NoteCollection.Parse("C5,E5,G5");
      actual = c4.Invert(3);
      Assert.IsNotNull(actual);
      CollectionAssert.AreEqual(thirdInversion, actual.Notes);
    }

    #endregion

    #region Implementation

    private static void TestChord(string expectedNotes, Note root, ChordFormula formula)
    {
      NoteCollection expected = NoteCollection.Parse(expectedNotes);
      var actual = new Chord(root, formula).Take(expected.Count).ToArray();
      CollectionAssert.AreEqual(expected, actual);
    }

    #endregion
  }
}
