namespace Intercode.MusicLib.Test
{
  using System;
  using Microsoft.VisualStudio.TestTools.UnitTesting;

  /// <summary>
  ///     Summary description for NoteTests
  /// </summary>
  [ TestClass ]
  public class NoteTests
  {
    [ TestMethod ]
    public void EqualsContractTest()
    {
      object x = Note.AFlat(1);
      object y = Note.GSharp(1);
      object z = Note.AFlat(1);

      Assert.IsTrue(x.Equals(x)); // Reflexive
      Assert.IsTrue(x.Equals(y)); // Symetric
      Assert.IsTrue(y.Equals(x));
      Assert.IsTrue(y.Equals(z)); // Transitive
      Assert.IsTrue(x.Equals(z));
      Assert.IsFalse(x.Equals(null)); // Never equal to null
    }

    [ TestMethod ]
    public void TypeSafeEqualsContractTest()
    {
      var x = Note.AFlat(1);
      var y = Note.GSharp(1);
      var z = Note.AFlat(1);

      Assert.IsTrue(x.Equals(x)); // Reflexive
      Assert.IsTrue(x.Equals(y)); // Symetric
      Assert.IsTrue(y.Equals(x));
      Assert.IsTrue(y.Equals(z)); // Transitive
      Assert.IsTrue(x.Equals(z));
      Assert.IsFalse(x.Equals(null)); // Never equal to null
    }

    [ TestMethod ]
    public void EqualsFailsWithDifferentTypeTest()
    {
      object actual = Note.A(1);
      Assert.IsFalse(actual.Equals(Int32.MinValue));
    }

    [ TestMethod ]
    public void TypeSafeEqualsFailsWithDifferentTypeTest()
    {
      var actual = Note.A(1);
      Assert.IsFalse(actual.Equals(Int32.MinValue));
    }

    [ TestMethod ]
    public void EqualsFailsWithNullTest()
    {
      object actual = Note.A(1);
      Assert.IsFalse(actual.Equals(null));
    }

    [ TestMethod ]
    public void TypeSafeEqualsFailsWithNullTest()
    {
      var actual = Note.A(1);
      Assert.IsFalse(actual.Equals(null));
    }

    [ TestMethod ]
    public void EqualsSucceedsWithSameObjectTest()
    {
      var actual = Note.A(1);
      Assert.IsTrue(actual.Equals(actual));
    }

    [ TestMethod ]
    public void GetHashcodeTest()
    {
      var actual = Note.A(1);
      var expected = Note.A(1);
      Assert.IsTrue(expected.Equals(actual));
      Assert.AreEqual(expected.GetHashCode(), actual.GetHashCode());
    }

    [ TestMethod ]
    public void EqualitySucceedsWithTwoObjectsTest()
    {
      var lhs = Note.A(1);
      var rhs = Note.A(1);
      Assert.IsTrue(lhs == rhs);
    }

    [ TestMethod ]
    public void EqualitySucceedsWithSameObjectTest()
    {
#pragma warning disable 1718
      var lhs = Note.A(1);
      Assert.IsTrue(lhs == lhs);
#pragma warning restore 1718
    }

    [ TestMethod ]
    public void EqualityFailsWithNullTest()
    {
      var lhs = Note.A(1);
      Assert.IsFalse(lhs == null);
    }

    [ TestMethod ]
    public void InequalitySucceedsWithTwoObjectsTest()
    {
      var lhs = Note.A(1);
      var rhs = Note.A(2);
      Assert.IsTrue(lhs != rhs);
    }

    [ TestMethod ]
    public void InequalityFailsWithSameObjectTest()
    {
#pragma warning disable 1718
      var lhs = Note.A(1);
      Assert.IsFalse(lhs != lhs);
#pragma warning restore 1718
    }

    [ TestMethod ]
    public void PropertiesTest()
    {
      var note = Note.A(1);
      Assert.AreEqual(Tone.A, note.Tone);
      Assert.AreEqual(Accidental.None, note.Accidental);
      Assert.AreEqual(1, note.Octave);

      note = Note.ASharp(2);
      Assert.AreEqual(Tone.ASharp, note.Tone);
      Assert.AreEqual(Accidental.Sharp, note.Accidental);
      Assert.AreEqual(2, note.Octave);

      note = Note.BFlat(3);
      Assert.AreEqual(Tone.BFlat, note.Tone);
      Assert.AreEqual(Accidental.Flat, note.Accidental);
      Assert.AreEqual(3, note.Octave);
    }

    [ TestMethod ]
    public void ToStringTests()
    {
      Assert.AreEqual("A1", Note.A(1).ToString());
      Assert.AreEqual("A#1", Note.ASharp(1).ToString());
      Assert.AreEqual("Bb1", Note.BFlat(1).ToString());
      Assert.AreEqual("C2", Note.C(2).ToString());
      Assert.AreEqual("C#2", Note.CSharp(2).ToString());
      Assert.AreEqual("Db2", Note.DFlat(2).ToString());
      Assert.AreEqual("D3", Note.D(3).ToString());
      Assert.AreEqual("D#3", Note.DSharp(3).ToString());
      Assert.AreEqual("Eb3", Note.EFlat(3).ToString());
      Assert.AreEqual("E4", Note.E(4).ToString());
      Assert.AreEqual("F4", Note.F(4).ToString());
      Assert.AreEqual("F#5", Note.FSharp(5).ToString());
      Assert.AreEqual("Gb5", Note.GFlat(5).ToString());
      Assert.AreEqual("G6", Note.G(6).ToString());
      Assert.AreEqual("G#7", Note.GSharp(7).ToString());
      Assert.AreEqual("Ab8", Note.AFlat(8).ToString());
    }

    [ TestMethod ]
    public void NextTest()
    {
      Assert.AreEqual(Note.C(1), Note.C(1).Next(0));

      Assert.AreEqual(Note.ASharp(1), Note.A(1).Next(1));
      Assert.AreEqual(Note.BFlat(1), Note.A(1).Next(1));

      Assert.AreEqual(Note.C(2), Note.B(1).Next(1));
      Assert.AreEqual(Note.C(3), Note.B(2).Next(1));
      Assert.AreEqual(Note.C(4), Note.B(3).Next(1));
      Assert.AreEqual(Note.C(5), Note.B(4).Next(1));
      Assert.AreEqual(Note.C(6), Note.B(5).Next(1));
      Assert.AreEqual(Note.C(7), Note.B(6).Next(1));
      Assert.AreEqual(Note.C(8), Note.B(7).Next(1));

      Assert.AreEqual(Note.C(2), Note.C(1).Next(12));
      Assert.AreEqual(Note.CSharp(2), Note.C(1).Next(13));
      Assert.AreEqual(Note.DFlat(2), Note.C(1).Next(13));
    }

    [ TestMethod ]
    public void PreviousTest()
    {
      Assert.AreEqual(Note.C(1), Note.C(1).Previous(0));

      Assert.AreEqual(Note.C(1), Note.CSharp(1).Previous(1));
      Assert.AreEqual(Note.C(1), Note.DFlat(1).Previous(1));

      Assert.AreEqual(Note.B(7), Note.C(8).Previous(1));
      Assert.AreEqual(Note.B(6), Note.C(7).Previous(1));
      Assert.AreEqual(Note.B(5), Note.C(6).Previous(1));
      Assert.AreEqual(Note.B(4), Note.C(5).Previous(1));
      Assert.AreEqual(Note.B(3), Note.C(4).Previous(1));
      Assert.AreEqual(Note.B(2), Note.C(3).Previous(1));
      Assert.AreEqual(Note.B(1), Note.C(2).Previous(1));

      Assert.AreEqual(Note.C(3), Note.C(4).Previous(12));
      Assert.AreEqual(Note.B(2), Note.C(4).Previous(13));
    }

    [ TestMethod ]
    public void AsSharpTest()
    {
      Assert.AreEqual(Note.A(4), Note.A(4).AsSharp());
      Assert.AreEqual(Note.ASharp(4), Note.ASharp(4).AsSharp());
      Assert.AreEqual(Note.ASharp(4), Note.BFlat(4).AsSharp());
    }

    [ TestMethod ]
    public void AsFlatTest()
    {
      Assert.AreEqual(Note.A(4), Note.A(4).AsFlat());
      Assert.AreEqual(Note.AFlat(4), Note.AFlat(4).AsFlat());
      Assert.AreEqual(Note.AFlat(4), Note.GSharp(4).AsFlat());
    }
  }
}
