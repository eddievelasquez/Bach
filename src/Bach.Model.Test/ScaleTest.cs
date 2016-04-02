//  
// Module Name: ScaleTest.cs
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

  [TestClass]
  public class ScaleTest
  {
    #region Public Methods

    [TestMethod]
    public void ConstructorTest()
    {
      var major = new ScaleFormula("Major", Interval.Perfect1, Interval.Major2, Interval.Major3, Interval.Perfect4,
                                   Interval.Perfect5, Interval.Major6, Interval.Major7);
      Assert.AreEqual("Major", major.Name);
      Assert.AreEqual(7, major.Count);
    }

    [TestMethod]
    public void GenerateScaleTest()
    {
      Note root = Note.Parse("C4");
      TestScale("C4,D4,E4,F4,G4,A4,B4", root, ScaleFormula.Major);
      TestScale("C4,D4,Eb4,F4,G4,Ab4,Bb4", root, ScaleFormula.NaturalMinor);
      TestScale("C4,D4,Eb4,F4,G4,Ab4,B4", root, ScaleFormula.HarmonicMinor);
      TestScale("C4,D4,Eb4,F4,G4,A4,B4", root, ScaleFormula.MelodicMinor);
      TestScale("C4,D4,Eb4,F4,Gb4,G#4,A4,B4", root, ScaleFormula.Diminished);
      TestScale("C4,Db4,Eb4,Fb4,F#4,G4,A4,Bb4", root, ScaleFormula.Polytonal);
      TestScale("C4,D4,E4,G4,A4", root, ScaleFormula.Pentatonic);
      TestScale("C4,Eb4,F4,Gb4,G4,Bb4", root, ScaleFormula.Blues);
      TestScale("C4,D4,Eb4,E4,G4,A4", root, ScaleFormula.Gospel);
    }

    [TestMethod]
    public void EqualsContractTest()
    {
      object x = new Scale(Note.Parse("C4"), ScaleFormula.Major);
      object y = new Scale(Note.Parse("C4"), ScaleFormula.Major);
      object z = new Scale(Note.Parse("C4"), ScaleFormula.Major);

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
      var x = new Scale(Note.Parse("C4"), ScaleFormula.Major);
      var y = new Scale(Note.Parse("C4"), ScaleFormula.Major);
      var z = new Scale(Note.Parse("C4"), ScaleFormula.Major);

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
      object actual = new Scale(Note.Parse("C4"), ScaleFormula.Major);
      Assert.IsFalse(actual.Equals(int.MinValue));
    }

    [TestMethod]
    public void TypeSafeEqualsFailsWithDifferentTypeTest()
    {
      var actual = new Scale(Note.Parse("C4"), ScaleFormula.Major);
      Assert.IsFalse(actual.Equals(int.MinValue));
    }

    [TestMethod]
    public void EqualsFailsWithNullTest()
    {
      object actual = new Scale(Note.Parse("C4"), ScaleFormula.Major);
      Assert.IsFalse(actual.Equals(null));
    }

    [TestMethod]
    public void TypeSafeEqualsFailsWithNullTest()
    {
      var actual = new Scale(Note.Parse("C4"), ScaleFormula.Major);
      Assert.IsFalse(actual.Equals(null));
    }

    [TestMethod]
    public void EqualsSucceedsWithSameObjectTest()
    {
      var actual = new Scale(Note.Parse("C4"), ScaleFormula.Major);
      Assert.IsTrue(actual.Equals(actual));
    }

    [TestMethod]
    public void GetHashcodeTest()
    {
      var actual = new Scale(Note.Parse("C4"), ScaleFormula.Major);
      var expected = new Scale(Note.Parse("C4"), ScaleFormula.Major);
      Assert.IsTrue(expected.Equals(actual));
      Assert.AreEqual(expected.GetHashCode(), actual.GetHashCode());
    }

    #endregion

    #region Implementation

    private static void TestScale(string expectedNotes, Note root, ScaleFormula formula)
    {
      NoteCollection expected = NoteCollection.Parse(expectedNotes);
      var actual = new NoteCollection(new Scale(root, formula).Take(expected.Count).ToArray());
      CollectionAssert.AreEqual(expected, actual);
    }

    #endregion
  }
}
