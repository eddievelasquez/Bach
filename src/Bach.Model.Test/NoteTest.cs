//  
// Module Name: NoteTest.cs
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
  ///    This is a test class for NoteTest and is intended
  ///    to contain all NoteTest Unit Tests
  /// </summary>
  [TestClass]
  public class NoteTest
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
    ///    A test for Note Constructor
    /// </summary>
    [TestMethod]
    public void NoteConstructorTest()
    {
      Note target = Note.Create(Tone.A, Accidental.Natural, 1);
      Assert.AreEqual(target.Tone, Tone.A);
      Assert.AreEqual(target.Accidental, Accidental.Natural);
      Assert.AreEqual(target.Octave, 1);
    }

    [TestMethod]
    public void EqualsContractTest()
    {
      object x = Note.Create(Tone.A, Accidental.Natural, 1);
      object y = Note.Create(Tone.A, Accidental.Natural, 1);
      object z = Note.Create(Tone.A, Accidental.Natural, 1);

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
      Note x = Note.Create(Tone.A, Accidental.Natural, 1);
      Note y = Note.Create(Tone.A, Accidental.Natural, 1);
      Note z = Note.Create(Tone.A, Accidental.Natural, 1);

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
      object actual = Note.Create(Tone.A, Accidental.Natural, 1);
      Assert.IsFalse(actual.Equals(int.MinValue));
    }

    [TestMethod]
    public void TypeSafeEqualsFailsWithDifferentTypeTest()
    {
      Note actual = Note.Create(Tone.A, Accidental.Natural, 1);
      Assert.IsFalse(actual.Equals(int.MinValue));
    }

    [TestMethod]
    public void EqualsFailsWithNullTest()
    {
      object actual = Note.Create(Tone.A, Accidental.Natural, 1);
      Assert.IsFalse(actual.Equals(null));
    }

    [TestMethod]
    public void TypeSafeEqualsFailsWithNullTest()
    {
      Note actual = Note.Create(Tone.A, Accidental.Natural, 1);
      Assert.IsFalse(actual.Equals(null));
    }

    [TestMethod]
    public void EqualsSucceedsWithSameObjectTest()
    {
      Note actual = Note.Create(Tone.A, Accidental.Natural, 1);
      Assert.IsTrue(actual.Equals(actual));
    }

    [TestMethod]
    public void GetHashcodeTest()
    {
      Note actual = Note.Create(Tone.A, Accidental.Natural, 1);
      Note expected = Note.Create(Tone.A, Accidental.Natural, 1);
      Assert.IsTrue(expected.Equals(actual));
      Assert.AreEqual(expected.GetHashCode(), actual.GetHashCode());
    }

    [TestMethod]
    public void CompareToContractTest()
    {
      {
        Note a = Note.Create(Tone.A, Accidental.Natural, 1);
        Assert.IsTrue(a.CompareTo(a) == 0);

        Note b = Note.Create(Tone.A, Accidental.Natural, 1);
        Assert.IsTrue(a.CompareTo(b) == 0);
        Assert.IsTrue(b.CompareTo(a) == 0);

        Note c = Note.Create(Tone.A, Accidental.Natural, 1);
        Assert.IsTrue(b.CompareTo(c) == 0);
        Assert.IsTrue(a.CompareTo(c) == 0);
      }
      {
        Note a = Note.Create(Tone.C, Accidental.Natural, 1);
        Note b = Note.Create(Tone.D, Accidental.Natural, 1);

        Assert.AreEqual(a.CompareTo(b), -b.CompareTo(a));

        Note c = Note.Create(Tone.E, Accidental.Natural, 1);
        Assert.IsTrue(a.CompareTo(b) < 0);
        Assert.IsTrue(b.CompareTo(c) < 0);
        Assert.IsTrue(a.CompareTo(c) < 0);
      }
    }

    [TestMethod]
    public void CompareToTest()
    {
      Note a1 = Note.Create(Tone.A, Accidental.Natural, 1);
      Note aSharp1 = Note.Create(Tone.A, Accidental.Sharp, 1);
      Note aFlat1 = Note.Create(Tone.A, Accidental.Flat, 1);
      Note a2 = Note.Create(Tone.A, Accidental.Natural, 2);
      Note aSharp2 = Note.Create(Tone.A, Accidental.Sharp, 2);
      Note aFlat2 = Note.Create(Tone.A, Accidental.Flat, 2);

      Assert.IsTrue(a1.CompareTo(a1) == 0);
      Assert.IsTrue(a1.CompareTo(aSharp1) < 0);
      Assert.IsTrue(a1.CompareTo(aFlat1) > 0);
      Assert.IsTrue(a1.CompareTo(a2) < 0);
      Assert.IsTrue(a1.CompareTo(aFlat2) < 0);
      Assert.IsTrue(a1.CompareTo(aSharp2) < 0);

      Note c1 = Note.Create(Tone.C, Accidental.Natural, 1);
      Assert.IsTrue(a1.CompareTo(c1) > 0);
      Assert.IsTrue(c1.CompareTo(a1) < 0);
    }

    [TestMethod]
    public void ToStringTest()
    {
      Note target = Note.Create(Tone.A, Accidental.DoubleFlat, 1);
      Assert.AreEqual("Abb1", target.ToString());

      target = Note.Create(Tone.A, Accidental.Flat, 1);
      Assert.AreEqual("Ab1", target.ToString());

      target = Note.Create(Tone.A, Accidental.Natural, 1);
      Assert.AreEqual("A1", target.ToString());

      target = Note.Create(Tone.A, Accidental.Sharp, 1);
      Assert.AreEqual("A#1", target.ToString());

      target = Note.Create(Tone.A, Accidental.DoubleSharp, 1);
      Assert.AreEqual("A##1", target.ToString());
    }

    [TestMethod]
    public void op_EqualityTest()
    {
      Note a = Note.Create(Tone.A, Accidental.Natural, 1);
      Note b = Note.Create(Tone.A, Accidental.Natural, 1);
      Note c = Note.Create(Tone.B, Accidental.Natural, 1);

      Assert.IsTrue(a == b);
      Assert.IsFalse(a == c);
      Assert.IsFalse(b == c);
    }

    [TestMethod]
    public void op_InequalityTest()
    {
      Note a = Note.Create(Tone.A, Accidental.Natural, 1);
      Note b = Note.Create(Tone.A, Accidental.Natural, 1);
      Note c = Note.Create(Tone.B, Accidental.Natural, 1);

      Assert.IsTrue(a != c);
      Assert.IsTrue(b != c);
      Assert.IsFalse(a != b);
    }

    [TestMethod]
    public void ComparisonOperatorsTest()
    {
      Note a = Note.Create(Tone.A, Accidental.Natural, 1);
      Note b = Note.Create(Tone.B, Accidental.Natural, 1);

      Assert.IsTrue(b > a);
      Assert.IsTrue(b >= a);
      Assert.IsFalse(b < a);
      Assert.IsFalse(b <= a);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void C1DoubleFlatThrowsTest()
    {
      Note.Create(Tone.C, Accidental.DoubleFlat, 0);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void C1FlatThrowsTest()
    {
      Note.Create(Tone.C, Accidental.Flat, 0);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void B8SharpThrowsTest()
    {
      Note.Create(Tone.B, Accidental.Sharp, 9);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void B8DoubleSharpThrowsTest()
    {
      Note.Create(Tone.B, Accidental.DoubleSharp, 9);
    }

    [TestMethod]
    public void AbsoluteValueTest()
    {
      Assert.AreEqual(0, Note.Parse("C0").AbsoluteValue);
      Assert.AreEqual(1, Note.Parse("C#0").AbsoluteValue);
      Assert.AreEqual(2, Note.Parse("C##0").AbsoluteValue);
      Assert.AreEqual(11, Note.Parse("B0").AbsoluteValue);
      Assert.AreEqual(12, Note.Parse("C1").AbsoluteValue);
      Assert.AreEqual(Note.Parse("Db1").AbsoluteValue, Note.Parse("C#1").AbsoluteValue);
      Assert.AreEqual(Note.Parse("C2").AbsoluteValue, Note.Parse("B#1").AbsoluteValue);
    }

    [TestMethod]
    public void op_SubtractionNoteTest()
    {
      Note cDoubleFlat2 = Note.Create(Tone.C, Accidental.DoubleFlat, 2);
      Note cFlat2 = Note.Create(Tone.C, Accidental.Flat, 2);
      Note c2 = Note.Create(Tone.C, Accidental.Natural, 2);
      Note cSharp2 = Note.Create(Tone.C, Accidental.Sharp, 2);
      Note cDoubleSharp2 = Note.Create(Tone.C, Accidental.DoubleSharp, 2);

      // Test interval with same notes in the same octave with different accidentals
      Assert.AreEqual(cDoubleFlat2 - cDoubleFlat2, 0);
      Assert.AreEqual(cDoubleFlat2 - cFlat2, 1);
      Assert.AreEqual(cDoubleFlat2 - c2, 2);
      Assert.AreEqual(cDoubleFlat2 - cSharp2, 3);
      Assert.AreEqual(cDoubleFlat2 - cDoubleSharp2, 4);
      Assert.AreEqual(cFlat2 - cDoubleFlat2, -1);
      Assert.AreEqual(c2 - cDoubleFlat2, -2);
      Assert.AreEqual(cSharp2 - cDoubleFlat2, -3);
      Assert.AreEqual(cDoubleSharp2 - cDoubleFlat2, -4);

      Note c3 = Note.Create(Tone.C, Accidental.Natural, 3);
      Assert.AreEqual(c2 - c3, 12);
      Assert.AreEqual(c3 - c2, -12);
    }

    [TestMethod]
    public void op_AdditionIntTest()
    {
      Note c2 = Note.Create(Tone.C, Accidental.Natural, 2);

      Assert.AreEqual(Note.Create(Tone.C, Accidental.Sharp, 2), c2 + 1);
      Assert.AreEqual(Note.Create(Tone.B, Accidental.Natural, 1), c2 + -1);
      Assert.AreEqual(Note.Create(Tone.D, Accidental.Natural, 2), c2 + 2);
      Assert.AreEqual(Note.Create(Tone.A, Accidental.Sharp, 1), c2 + -2);
    }

    [TestMethod]
    public void op_IncrementTest()
    {
      Note c2 = Note.Create(Tone.C, Accidental.Natural, 2);

      Assert.AreEqual(Note.Create(Tone.C, Accidental.Sharp, 2), ++c2);
      Assert.AreEqual(Note.Create(Tone.D, Accidental.Natural, 2), ++c2);
    }

    [TestMethod]
    public void op_SubtractionIntTest()
    {
      Note c2 = Note.Create(Tone.C, Accidental.Natural, 2);

      Assert.AreEqual(Note.Create(Tone.B, Accidental.Natural, 1), c2 - 1);
      Assert.AreEqual(Note.Create(Tone.A, Accidental.Sharp, 1), c2 - 2);
    }

    [TestMethod]
    public void op_DecrementTest()
    {
      Note c2 = Note.Create(Tone.C, Accidental.Natural, 2);

      Assert.AreEqual(Note.Create(Tone.B, Accidental.Natural, 1), --c2);
      Assert.AreEqual(Note.Create(Tone.A, Accidental.Sharp, 1), --c2);
    }

    [TestMethod]
    public void op_AdditionIntAccidentalModeTest()
    {
      Note c2 = Note.Create(Tone.C, Accidental.Natural, 2);

      Note.AccidentalMode = AccidentalMode.FavorSharps;

      Note actual = c2 + 1;
      Assert.AreEqual("C#2", actual.ToString());

      Note.AccidentalMode = AccidentalMode.FavorFlats;

      actual = c2 + 1;
      Assert.AreEqual("Db2", actual.ToString());
    }

    [TestMethod]
    public void op_SubtractionIntAccidentalModeTest()
    {
      Note c2 = Note.Create(Tone.C, Accidental.Natural, 2);

      Note.AccidentalMode = AccidentalMode.FavorSharps;

      Note actual = c2 - 2;
      Assert.AreEqual("A#1", actual.ToString());

      Note.AccidentalMode = AccidentalMode.FavorFlats;

      actual = c2 - 2;
      Assert.AreEqual("Bb1", actual.ToString());
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException), "Must be equal to or less than G9")]
    public void MaxNoteTest()
    {
      Assert.IsNotNull(Note.Parse("G9"));
      Note.Parse("A9");
    }

    [TestMethod]
    public void TryParseTest()
    {
      Note actual;
      Assert.IsTrue(Note.TryParse("C", out actual));
      Assert.AreEqual(Note.Create(Tone.C, Accidental.Natural, 4), actual);

      Assert.IsTrue(Note.TryParse("C#", out actual));
      Assert.AreEqual(Note.Create(Tone.C, Accidental.Sharp, 4), actual);

      Assert.IsTrue(Note.TryParse("C##", out actual));
      Assert.AreEqual(Note.Create(Tone.C, Accidental.DoubleSharp, 4), actual);

      Assert.IsTrue(Note.TryParse("Cb", out actual));
      Assert.AreEqual(Note.Create(Tone.C, Accidental.Flat, 4), actual);

      Assert.IsTrue(Note.TryParse("Cbb", out actual));
      Assert.AreEqual(Note.Create(Tone.C, Accidental.DoubleFlat, 4), actual);

      Assert.IsTrue(Note.TryParse("C2", out actual));
      Assert.AreEqual(Note.Create(Tone.C, Accidental.Natural, 2), actual);

      Assert.IsTrue(Note.TryParse("C#2", out actual));
      Assert.AreEqual(Note.Create(Tone.C, Accidental.Sharp, 2), actual);

      Assert.IsTrue(Note.TryParse("C##2", out actual));
      Assert.AreEqual(Note.Create(Tone.C, Accidental.DoubleSharp, 2), actual);

      Assert.IsTrue(Note.TryParse("Cb2", out actual));
      Assert.AreEqual(Note.Create(Tone.C, Accidental.Flat, 2), actual);

      Assert.IsTrue(Note.TryParse("Cbb2", out actual));
      Assert.AreEqual(Note.Create(Tone.C, Accidental.DoubleFlat, 2), actual);

      Assert.IsFalse(Note.TryParse("H", out actual));
      Assert.IsFalse(actual.IsValid);

      Assert.IsFalse(Note.TryParse("C!", out actual));
      Assert.IsFalse(actual.IsValid);

      Assert.IsFalse(Note.TryParse("C#-1", out actual));
      Assert.IsFalse(actual.IsValid);

      Assert.IsFalse(Note.TryParse("C#10", out actual));
      Assert.IsFalse(actual.IsValid);

      Assert.IsFalse(Note.TryParse("C#b2", out actual));
      Assert.IsFalse(actual.IsValid);

      Assert.IsFalse(Note.TryParse("Cb#2", out actual));
      Assert.IsFalse(actual.IsValid);
    }

    [TestMethod]
    public void FrequencyTest()
    {
      Assert.AreEqual(440.0, Math.Round(Note.Parse("A4").Frequency, 2));
      Assert.AreEqual(523.25, Math.Round(Note.Parse("C5").Frequency, 2));
      Assert.AreEqual(349.23, Math.Round(Note.Parse("F4").Frequency, 2));
      Assert.AreEqual(880.0, Math.Round(Note.Parse("A5").Frequency, 2));
    }

    [TestMethod]
    public void MidiTest()
    {
      Assert.AreEqual(12, Note.Parse("C0").Midi);
      Assert.AreEqual(24, Note.Parse("C1").Midi);
      Assert.AreEqual(36, Note.Parse("C2").Midi);
      Assert.AreEqual(48, Note.Parse("C3").Midi);
      Assert.AreEqual(60, Note.Parse("C4").Midi);
      Assert.AreEqual(72, Note.Parse("C5").Midi);
      Assert.AreEqual(84, Note.Parse("C6").Midi);
      Assert.AreEqual(96, Note.Parse("C7").Midi);
      Assert.AreEqual(108, Note.Parse("C8").Midi);
      Assert.AreEqual(120, Note.Parse("C9").Midi);
      Assert.AreEqual(127, Note.Parse("G9").Midi);
    }

    #endregion
  }
}
