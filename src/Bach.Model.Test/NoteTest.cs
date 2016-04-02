//  
// Module Name: NoteTest.cs
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

  public class NoteTest
  {
    #region Public Methods

    [Fact]
    public void NoteConstructorTest()
    {
      Note target = Note.Create(Tone.A, Accidental.Natural, 1);
      Assert.Equal(target.Tone, Tone.A);
      Assert.Equal(target.Accidental, Accidental.Natural);
      Assert.Equal(target.Octave, 1);
    }

    [Fact]
    public void EqualsContractTest()
    {
      object x = Note.Create(Tone.A, Accidental.Natural, 1);
      object y = Note.Create(Tone.A, Accidental.Natural, 1);
      object z = Note.Create(Tone.A, Accidental.Natural, 1);

      Assert.True(x.Equals(x)); // Reflexive
      Assert.True(x.Equals(y)); // Symetric
      Assert.True(y.Equals(x));
      Assert.True(y.Equals(z)); // Transitive
      Assert.True(x.Equals(z));
      Assert.False(x.Equals(null)); // Never equal to null
    }

    [Fact]
    public void TypeSafeEqualsContractTest()
    {
      Note x = Note.Create(Tone.A, Accidental.Natural, 1);
      Note y = Note.Create(Tone.A, Accidental.Natural, 1);
      Note z = Note.Create(Tone.A, Accidental.Natural, 1);

      Assert.True(x.Equals(x)); // Reflexive
      Assert.True(x.Equals(y)); // Symetric
      Assert.True(y.Equals(x));
      Assert.True(y.Equals(z)); // Transitive
      Assert.True(x.Equals(z));
      Assert.False(x.Equals(null)); // Never equal to null
    }

    [Fact]
    public void EqualsFailsWithDifferentTypeTest()
    {
      object actual = Note.Create(Tone.A, Accidental.Natural, 1);
      Assert.False(actual.Equals(int.MinValue));
    }

    [Fact]
    public void TypeSafeEqualsFailsWithDifferentTypeTest()
    {
      Note actual = Note.Create(Tone.A, Accidental.Natural, 1);
      Assert.False(actual.Equals(int.MinValue));
    }

    [Fact]
    public void EqualsFailsWithNullTest()
    {
      object actual = Note.Create(Tone.A, Accidental.Natural, 1);
      Assert.False(actual.Equals(null));
    }

    [Fact]
    public void TypeSafeEqualsFailsWithNullTest()
    {
      Note actual = Note.Create(Tone.A, Accidental.Natural, 1);
      Assert.False(actual.Equals(null));
    }

    [Fact]
    public void EqualsSucceedsWithSameObjectTest()
    {
      Note actual = Note.Create(Tone.A, Accidental.Natural, 1);
      Assert.True(actual.Equals(actual));
    }

    [Fact]
    public void GetHashcodeTest()
    {
      Note actual = Note.Create(Tone.A, Accidental.Natural, 1);
      Note expected = Note.Create(Tone.A, Accidental.Natural, 1);
      Assert.True(expected.Equals(actual));
      Assert.Equal(expected.GetHashCode(), actual.GetHashCode());
    }

    [Fact]
    public void CompareToContractTest()
    {
      {
        Note a = Note.Create(Tone.A, Accidental.Natural, 1);
        Assert.True(a.CompareTo(a) == 0);

        Note b = Note.Create(Tone.A, Accidental.Natural, 1);
        Assert.True(a.CompareTo(b) == 0);
        Assert.True(b.CompareTo(a) == 0);

        Note c = Note.Create(Tone.A, Accidental.Natural, 1);
        Assert.True(b.CompareTo(c) == 0);
        Assert.True(a.CompareTo(c) == 0);
      }
      {
        Note a = Note.Create(Tone.C, Accidental.Natural, 1);
        Note b = Note.Create(Tone.D, Accidental.Natural, 1);

        Assert.Equal(a.CompareTo(b), -b.CompareTo(a));

        Note c = Note.Create(Tone.E, Accidental.Natural, 1);
        Assert.True(a.CompareTo(b) < 0);
        Assert.True(b.CompareTo(c) < 0);
        Assert.True(a.CompareTo(c) < 0);
      }
    }

    [Fact]
    public void CompareToTest()
    {
      Note a1 = Note.Create(Tone.A, Accidental.Natural, 1);
      Note aSharp1 = Note.Create(Tone.A, Accidental.Sharp, 1);
      Note aFlat1 = Note.Create(Tone.A, Accidental.Flat, 1);
      Note a2 = Note.Create(Tone.A, Accidental.Natural, 2);
      Note aSharp2 = Note.Create(Tone.A, Accidental.Sharp, 2);
      Note aFlat2 = Note.Create(Tone.A, Accidental.Flat, 2);

      Assert.True(a1.CompareTo(a1) == 0);
      Assert.True(a1.CompareTo(aSharp1) < 0);
      Assert.True(a1.CompareTo(aFlat1) > 0);
      Assert.True(a1.CompareTo(a2) < 0);
      Assert.True(a1.CompareTo(aFlat2) < 0);
      Assert.True(a1.CompareTo(aSharp2) < 0);

      Note c1 = Note.Create(Tone.C, Accidental.Natural, 1);
      Assert.True(a1.CompareTo(c1) > 0);
      Assert.True(c1.CompareTo(a1) < 0);
    }

    [Fact]
    public void ToStringTest()
    {
      Note target = Note.Create(Tone.A, Accidental.DoubleFlat, 1);
      Assert.Equal("Abb1", target.ToString());

      target = Note.Create(Tone.A, Accidental.Flat, 1);
      Assert.Equal("Ab1", target.ToString());

      target = Note.Create(Tone.A, Accidental.Natural, 1);
      Assert.Equal("A1", target.ToString());

      target = Note.Create(Tone.A, Accidental.Sharp, 1);
      Assert.Equal("A#1", target.ToString());

      target = Note.Create(Tone.A, Accidental.DoubleSharp, 1);
      Assert.Equal("A##1", target.ToString());
    }

    [Fact]
    public void op_EqualityTest()
    {
      Note a = Note.Create(Tone.A, Accidental.Natural, 1);
      Note b = Note.Create(Tone.A, Accidental.Natural, 1);
      Note c = Note.Create(Tone.B, Accidental.Natural, 1);

      Assert.True(a == b);
      Assert.False(a == c);
      Assert.False(b == c);
    }

    [Fact]
    public void op_InequalityTest()
    {
      Note a = Note.Create(Tone.A, Accidental.Natural, 1);
      Note b = Note.Create(Tone.A, Accidental.Natural, 1);
      Note c = Note.Create(Tone.B, Accidental.Natural, 1);

      Assert.True(a != c);
      Assert.True(b != c);
      Assert.False(a != b);
    }

    [Fact]
    public void ComparisonOperatorsTest()
    {
      Note a = Note.Create(Tone.A, Accidental.Natural, 1);
      Note b = Note.Create(Tone.B, Accidental.Natural, 1);

      Assert.True(b > a);
      Assert.True(b >= a);
      Assert.False(b < a);
      Assert.False(b <= a);
    }

    [Fact]
    public void C1DoubleFlatThrowsTest()
    {
      Assert.Throws<ArgumentException>(() => { Note.Create(Tone.C, Accidental.DoubleFlat, 0); });
    }

    [Fact]
    public void C1FlatThrowsTest()
    {
      Assert.Throws<ArgumentException>(() => { Note.Create(Tone.C, Accidental.Flat, 0); });
    }

    [Fact]
    public void B8SharpThrowsTest()
    {
      Assert.Throws<ArgumentException>(() => { Note.Create(Tone.B, Accidental.Sharp, 9); });
    }

    [Fact]
    public void B8DoubleSharpThrowsTest()
    {
      Assert.Throws<ArgumentException>(() => { Note.Create(Tone.B, Accidental.DoubleSharp, 9); });
    }

    [Fact]
    public void AbsoluteValueTest()
    {
      Assert.Equal(0, Note.Parse("C0").AbsoluteValue);
      Assert.Equal(1, Note.Parse("C#0").AbsoluteValue);
      Assert.Equal(2, Note.Parse("C##0").AbsoluteValue);
      Assert.Equal(11, Note.Parse("B0").AbsoluteValue);
      Assert.Equal(12, Note.Parse("C1").AbsoluteValue);
      Assert.Equal(Note.Parse("Db1").AbsoluteValue, Note.Parse("C#1").AbsoluteValue);
      Assert.Equal(Note.Parse("C2").AbsoluteValue, Note.Parse("B#1").AbsoluteValue);
    }

    [Fact]
    public void op_SubtractionNoteTest()
    {
      Note cDoubleFlat2 = Note.Create(Tone.C, Accidental.DoubleFlat, 2);
      Note cFlat2 = Note.Create(Tone.C, Accidental.Flat, 2);
      Note c2 = Note.Create(Tone.C, Accidental.Natural, 2);
      Note cSharp2 = Note.Create(Tone.C, Accidental.Sharp, 2);
      Note cDoubleSharp2 = Note.Create(Tone.C, Accidental.DoubleSharp, 2);

      // Test interval with same notes in the same octave with different accidentals
      Assert.Equal(cDoubleFlat2 - cDoubleFlat2, 0);
      Assert.Equal(cDoubleFlat2 - cFlat2, 1);
      Assert.Equal(cDoubleFlat2 - c2, 2);
      Assert.Equal(cDoubleFlat2 - cSharp2, 3);
      Assert.Equal(cDoubleFlat2 - cDoubleSharp2, 4);
      Assert.Equal(cFlat2 - cDoubleFlat2, -1);
      Assert.Equal(c2 - cDoubleFlat2, -2);
      Assert.Equal(cSharp2 - cDoubleFlat2, -3);
      Assert.Equal(cDoubleSharp2 - cDoubleFlat2, -4);

      Note c3 = Note.Create(Tone.C, Accidental.Natural, 3);
      Assert.Equal(c2 - c3, 12);
      Assert.Equal(c3 - c2, -12);
    }

    [Fact]
    public void op_AdditionIntTest()
    {
      Note c2 = Note.Create(Tone.C, Accidental.Natural, 2);

      Assert.Equal(Note.Create(Tone.C, Accidental.Sharp, 2), c2 + 1);
      Assert.Equal(Note.Create(Tone.B, Accidental.Natural, 1), c2 + -1);
      Assert.Equal(Note.Create(Tone.D, Accidental.Natural, 2), c2 + 2);
      Assert.Equal(Note.Create(Tone.A, Accidental.Sharp, 1), c2 + -2);
    }

    [Fact]
    public void op_IncrementTest()
    {
      Note c2 = Note.Create(Tone.C, Accidental.Natural, 2);

      Assert.Equal(Note.Create(Tone.C, Accidental.Sharp, 2), ++c2);
      Assert.Equal(Note.Create(Tone.D, Accidental.Natural, 2), ++c2);
    }

    [Fact]
    public void op_SubtractionIntTest()
    {
      Note c2 = Note.Create(Tone.C, Accidental.Natural, 2);

      Assert.Equal(Note.Create(Tone.B, Accidental.Natural, 1), c2 - 1);
      Assert.Equal(Note.Create(Tone.A, Accidental.Sharp, 1), c2 - 2);
    }

    [Fact]
    public void op_DecrementTest()
    {
      Note c2 = Note.Create(Tone.C, Accidental.Natural, 2);

      Assert.Equal(Note.Create(Tone.B, Accidental.Natural, 1), --c2);
      Assert.Equal(Note.Create(Tone.A, Accidental.Sharp, 1), --c2);
    }

    [Fact]
    public void op_AdditionIntAccidentalModeTest()
    {
      Note c2 = Note.Create(Tone.C, Accidental.Natural, 2);

      Note.AccidentalMode = AccidentalMode.FavorSharps;

      Note actual = c2 + 1;
      Assert.Equal("C#2", actual.ToString());

      Note.AccidentalMode = AccidentalMode.FavorFlats;

      actual = c2 + 1;
      Assert.Equal("Db2", actual.ToString());
    }

    [Fact]
    public void op_SubtractionIntAccidentalModeTest()
    {
      Note c2 = Note.Create(Tone.C, Accidental.Natural, 2);

      Note.AccidentalMode = AccidentalMode.FavorSharps;

      Note actual = c2 - 2;
      Assert.Equal("A#1", actual.ToString());

      Note.AccidentalMode = AccidentalMode.FavorFlats;

      actual = c2 - 2;
      Assert.Equal("Bb1", actual.ToString());
    }

    [Fact]
    public void MaxNoteTest()
    {
      Assert.NotNull(Note.Parse("G9"));
      Assert.Throws<ArgumentException>(() => { Note.Parse("A9"); });
    }

    [Fact]
    public void TryParseTest()
    {
      Note actual;
      Assert.True(Note.TryParse("C", out actual));
      Assert.Equal(Note.Create(Tone.C, Accidental.Natural, 4), actual);

      Assert.True(Note.TryParse("C#", out actual));
      Assert.Equal(Note.Create(Tone.C, Accidental.Sharp, 4), actual);

      Assert.True(Note.TryParse("C##", out actual));
      Assert.Equal(Note.Create(Tone.C, Accidental.DoubleSharp, 4), actual);

      Assert.True(Note.TryParse("Cb", out actual));
      Assert.Equal(Note.Create(Tone.C, Accidental.Flat, 4), actual);

      Assert.True(Note.TryParse("Cbb", out actual));
      Assert.Equal(Note.Create(Tone.C, Accidental.DoubleFlat, 4), actual);

      Assert.True(Note.TryParse("C2", out actual));
      Assert.Equal(Note.Create(Tone.C, Accidental.Natural, 2), actual);

      Assert.True(Note.TryParse("C#2", out actual));
      Assert.Equal(Note.Create(Tone.C, Accidental.Sharp, 2), actual);

      Assert.True(Note.TryParse("C##2", out actual));
      Assert.Equal(Note.Create(Tone.C, Accidental.DoubleSharp, 2), actual);

      Assert.True(Note.TryParse("Cb2", out actual));
      Assert.Equal(Note.Create(Tone.C, Accidental.Flat, 2), actual);

      Assert.True(Note.TryParse("Cbb2", out actual));
      Assert.Equal(Note.Create(Tone.C, Accidental.DoubleFlat, 2), actual);

      Assert.False(Note.TryParse("H", out actual));
      Assert.False(actual.IsValid);

      Assert.False(Note.TryParse("C!", out actual));
      Assert.False(actual.IsValid);

      Assert.False(Note.TryParse("C#-1", out actual));
      Assert.False(actual.IsValid);

      Assert.False(Note.TryParse("C#10", out actual));
      Assert.False(actual.IsValid);

      Assert.False(Note.TryParse("C#b2", out actual));
      Assert.False(actual.IsValid);

      Assert.False(Note.TryParse("Cb#2", out actual));
      Assert.False(actual.IsValid);
    }

    [Fact]
    public void FrequencyTest()
    {
      Assert.Equal(440.0, Math.Round(Note.Parse("A4").Frequency, 2));
      Assert.Equal(523.25, Math.Round(Note.Parse("C5").Frequency, 2));
      Assert.Equal(349.23, Math.Round(Note.Parse("F4").Frequency, 2));
      Assert.Equal(880.0, Math.Round(Note.Parse("A5").Frequency, 2));
    }

    [Fact]
    public void MidiTest()
    {
      Assert.Equal(12, Note.Parse("C0").Midi);
      Assert.Equal(24, Note.Parse("C1").Midi);
      Assert.Equal(36, Note.Parse("C2").Midi);
      Assert.Equal(48, Note.Parse("C3").Midi);
      Assert.Equal(60, Note.Parse("C4").Midi);
      Assert.Equal(72, Note.Parse("C5").Midi);
      Assert.Equal(84, Note.Parse("C6").Midi);
      Assert.Equal(96, Note.Parse("C7").Midi);
      Assert.Equal(108, Note.Parse("C8").Midi);
      Assert.Equal(120, Note.Parse("C9").Midi);
      Assert.Equal(127, Note.Parse("G9").Midi);
    }

    #endregion
  }
}
