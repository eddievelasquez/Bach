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

  public class AbsoluteNoteTest
  {
    #region Public Methods

    [Fact]
    public void CreateWithToneAndAccidentalTest()
    {
      AbsoluteNote target = AbsoluteNote.Create(Tone.A, Accidental.Natural, 1);
      Assert.Equal(Tone.A, target.Tone);
      Assert.Equal(Accidental.Natural, target.Accidental);
      Assert.Equal(1, target.Octave);

      Assert.Throws<ArgumentOutOfRangeException>(
                                                 () =>
                                                   AbsoluteNote.Create(Tone.C, Accidental.Flat, AbsoluteNote.MinOctave));
      Assert.Throws<ArgumentOutOfRangeException>(
                                                 () =>
                                                   AbsoluteNote.Create(Tone.C, Accidental.DoubleFlat,
                                                                       AbsoluteNote.MinOctave));
      Assert.Throws<ArgumentOutOfRangeException>(
                                                 () =>
                                                   AbsoluteNote.Create(Tone.B, Accidental.Sharp, AbsoluteNote.MaxOctave));
      Assert.Throws<ArgumentOutOfRangeException>(
                                                 () =>
                                                   AbsoluteNote.Create(Tone.B, Accidental.DoubleSharp,
                                                                       AbsoluteNote.MaxOctave));
    }

    [Fact]
    public void CreateWithNoteTest()
    {
      AbsoluteNote target = AbsoluteNote.Create(Note.A, 1);
      Assert.Equal(Note.A, target.Note);
      Assert.Equal(1, target.Octave);

      Assert.Throws<ArgumentOutOfRangeException>(
                                                 () =>
                                                   AbsoluteNote.Create(new Note(Tone.C, Accidental.Flat),
                                                                       AbsoluteNote.MinOctave));
      Assert.Throws<ArgumentOutOfRangeException>(
                                                 () =>
                                                   AbsoluteNote.Create(new Note(Tone.C, Accidental.DoubleFlat),
                                                                       AbsoluteNote.MinOctave));
      Assert.Throws<ArgumentOutOfRangeException>(
                                                 () =>
                                                   AbsoluteNote.Create(new Note(Tone.B, Accidental.Sharp),
                                                                       AbsoluteNote.MaxOctave));
      Assert.Throws<ArgumentOutOfRangeException>(
                                                 () =>
                                                   AbsoluteNote.Create(new Note(Tone.B, Accidental.DoubleSharp),
                                                                       AbsoluteNote.MaxOctave));
    }

    [Fact]
    public void EqualsContractTest()
    {
      object x = AbsoluteNote.Create(Tone.A, Accidental.Natural, 1);
      object y = AbsoluteNote.Create(Tone.A, Accidental.Natural, 1);
      object z = AbsoluteNote.Create(Tone.A, Accidental.Natural, 1);

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
      AbsoluteNote x = AbsoluteNote.Create(Tone.A, Accidental.Natural, 1);
      AbsoluteNote y = AbsoluteNote.Create(Tone.A, Accidental.Natural, 1);
      AbsoluteNote z = AbsoluteNote.Create(Tone.A, Accidental.Natural, 1);

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
      object actual = AbsoluteNote.Create(Tone.A, Accidental.Natural, 1);
      Assert.False(actual.Equals(int.MinValue));
    }

    [Fact]
    public void TypeSafeEqualsFailsWithDifferentTypeTest()
    {
      AbsoluteNote actual = AbsoluteNote.Create(Tone.A, Accidental.Natural, 1);
      Assert.False(actual.Equals(int.MinValue));
    }

    [Fact]
    public void EqualsFailsWithNullTest()
    {
      object actual = AbsoluteNote.Create(Tone.A, Accidental.Natural, 1);
      Assert.False(actual.Equals(null));
    }

    [Fact]
    public void TypeSafeEqualsFailsWithNullTest()
    {
      AbsoluteNote actual = AbsoluteNote.Create(Tone.A, Accidental.Natural, 1);
      Assert.False(actual.Equals(null));
    }

    [Fact]
    public void EqualsSucceedsWithSameObjectTest()
    {
      AbsoluteNote actual = AbsoluteNote.Create(Tone.A, Accidental.Natural, 1);
      Assert.True(actual.Equals(actual));
    }

    [Fact]
    public void GetHashcodeTest()
    {
      AbsoluteNote actual = AbsoluteNote.Create(Tone.A, Accidental.Natural, 1);
      AbsoluteNote expected = AbsoluteNote.Create(Tone.A, Accidental.Natural, 1);
      Assert.True(expected.Equals(actual));
      Assert.Equal(expected.GetHashCode(), actual.GetHashCode());
    }

    [Fact]
    public void CompareToContractTest()
    {
      {
        AbsoluteNote a = AbsoluteNote.Create(Tone.A, Accidental.Natural, 1);
        Assert.True(a.CompareTo(a) == 0);

        AbsoluteNote b = AbsoluteNote.Create(Tone.A, Accidental.Natural, 1);
        Assert.True(a.CompareTo(b) == 0);
        Assert.True(b.CompareTo(a) == 0);

        AbsoluteNote c = AbsoluteNote.Create(Tone.A, Accidental.Natural, 1);
        Assert.True(b.CompareTo(c) == 0);
        Assert.True(a.CompareTo(c) == 0);
      }
      {
        AbsoluteNote a = AbsoluteNote.Create(Tone.C, Accidental.Natural, 1);
        AbsoluteNote b = AbsoluteNote.Create(Tone.D, Accidental.Natural, 1);

        Assert.Equal(a.CompareTo(b), -b.CompareTo(a));

        AbsoluteNote c = AbsoluteNote.Create(Tone.E, Accidental.Natural, 1);
        Assert.True(a.CompareTo(b) < 0);
        Assert.True(b.CompareTo(c) < 0);
        Assert.True(a.CompareTo(c) < 0);
      }
    }

    [Fact]
    public void CompareToTest()
    {
      AbsoluteNote a1 = AbsoluteNote.Create(Tone.A, Accidental.Natural, 1);
      AbsoluteNote aSharp1 = AbsoluteNote.Create(Tone.A, Accidental.Sharp, 1);
      AbsoluteNote aFlat1 = AbsoluteNote.Create(Tone.A, Accidental.Flat, 1);
      AbsoluteNote a2 = AbsoluteNote.Create(Tone.A, Accidental.Natural, 2);
      AbsoluteNote aSharp2 = AbsoluteNote.Create(Tone.A, Accidental.Sharp, 2);
      AbsoluteNote aFlat2 = AbsoluteNote.Create(Tone.A, Accidental.Flat, 2);

      Assert.True(a1.CompareTo(a1) == 0);
      Assert.True(a1.CompareTo(aSharp1) < 0);
      Assert.True(a1.CompareTo(aFlat1) > 0);
      Assert.True(a1.CompareTo(a2) < 0);
      Assert.True(a1.CompareTo(aFlat2) < 0);
      Assert.True(a1.CompareTo(aSharp2) < 0);

      AbsoluteNote c1 = AbsoluteNote.Create(Tone.C, Accidental.Natural, 1);
      Assert.True(a1.CompareTo(c1) > 0);
      Assert.True(c1.CompareTo(a1) < 0);
    }

    [Fact]
    public void ToStringTest()
    {
      AbsoluteNote target = AbsoluteNote.Create(Tone.A, Accidental.DoubleFlat, 1);
      Assert.Equal("Abb1", target.ToString());

      target = AbsoluteNote.Create(Tone.A, Accidental.Flat, 1);
      Assert.Equal("Ab1", target.ToString());

      target = AbsoluteNote.Create(Tone.A, Accidental.Natural, 1);
      Assert.Equal("A1", target.ToString());

      target = AbsoluteNote.Create(Tone.A, Accidental.Sharp, 1);
      Assert.Equal("A#1", target.ToString());

      target = AbsoluteNote.Create(Tone.A, Accidental.DoubleSharp, 1);
      Assert.Equal("A##1", target.ToString());
    }

    [Fact]
    public void op_EqualityTest()
    {
      AbsoluteNote a = AbsoluteNote.Create(Tone.A, Accidental.Natural, 1);
      AbsoluteNote b = AbsoluteNote.Create(Tone.A, Accidental.Natural, 1);
      AbsoluteNote c = AbsoluteNote.Create(Tone.B, Accidental.Natural, 1);

      Assert.True(a == b);
      Assert.False(a == c);
      Assert.False(b == c);
    }

    [Fact]
    public void op_InequalityTest()
    {
      AbsoluteNote a = AbsoluteNote.Create(Tone.A, Accidental.Natural, 1);
      AbsoluteNote b = AbsoluteNote.Create(Tone.A, Accidental.Natural, 1);
      AbsoluteNote c = AbsoluteNote.Create(Tone.B, Accidental.Natural, 1);

      Assert.True(a != c);
      Assert.True(b != c);
      Assert.False(a != b);
    }

    [Fact]
    public void ComparisonOperatorsTest()
    {
      AbsoluteNote a = AbsoluteNote.Create(Tone.A, Accidental.Natural, 1);
      AbsoluteNote b = AbsoluteNote.Create(Tone.B, Accidental.Natural, 1);

      Assert.True(b > a);
      Assert.True(b >= a);
      Assert.False(b < a);
      Assert.False(b <= a);
    }

    [Fact]
    public void AbsoluteValueTest()
    {
      Assert.Equal(0, AbsoluteNote.Parse("C0").AbsoluteValue);
      Assert.Equal(1, AbsoluteNote.Parse("C#0").AbsoluteValue);
      Assert.Equal(2, AbsoluteNote.Parse("C##0").AbsoluteValue);
      Assert.Equal(11, AbsoluteNote.Parse("B0").AbsoluteValue);
      Assert.Equal(12, AbsoluteNote.Parse("C1").AbsoluteValue);
      Assert.Equal(AbsoluteNote.Parse("Db1").AbsoluteValue, AbsoluteNote.Parse("C#1").AbsoluteValue);
      Assert.Equal(AbsoluteNote.Parse("C2").AbsoluteValue, AbsoluteNote.Parse("B#1").AbsoluteValue);
    }

    [Fact]
    public void op_SubtractionNoteTest()
    {
      AbsoluteNote cDoubleFlat2 = AbsoluteNote.Create(Tone.C, Accidental.DoubleFlat, 2);
      AbsoluteNote cFlat2 = AbsoluteNote.Create(Tone.C, Accidental.Flat, 2);
      AbsoluteNote c2 = AbsoluteNote.Create(Tone.C, Accidental.Natural, 2);
      AbsoluteNote cSharp2 = AbsoluteNote.Create(Tone.C, Accidental.Sharp, 2);
      AbsoluteNote cDoubleSharp2 = AbsoluteNote.Create(Tone.C, Accidental.DoubleSharp, 2);

      // Test interval with same notes in the same octave with different accidentals
      Assert.Equal(0, cDoubleFlat2 - cDoubleFlat2);
      Assert.Equal(-1, cDoubleFlat2 - cFlat2);
      Assert.Equal(-2, cDoubleFlat2 - c2);
      Assert.Equal(-3, cDoubleFlat2 - cSharp2);
      Assert.Equal(-4, cDoubleFlat2 - cDoubleSharp2);
      Assert.Equal(1, cFlat2 - cDoubleFlat2);
      Assert.Equal(2, c2 - cDoubleFlat2);
      Assert.Equal(3, cSharp2 - cDoubleFlat2);
      Assert.Equal(4, cDoubleSharp2 - cDoubleFlat2);

      AbsoluteNote c3 = AbsoluteNote.Create(Tone.C, Accidental.Natural, 3);
      Assert.Equal(-12, c2 - c3);
      Assert.Equal(12, c3 - c2);
    }

    [Fact]
    public void op_AdditionIntTest()
    {
      AbsoluteNote c2 = AbsoluteNote.Create(Tone.C, Accidental.Natural, 2);

      Assert.Equal(AbsoluteNote.Create(Tone.C, Accidental.Sharp, 2), c2 + 1);
      Assert.Equal(AbsoluteNote.Create(Tone.B, Accidental.Natural, 1), c2 + -1);
      Assert.Equal(AbsoluteNote.Create(Tone.D, Accidental.Natural, 2), c2 + 2);
      Assert.Equal(AbsoluteNote.Create(Tone.A, Accidental.Sharp, 1), c2 + -2);
    }

    [Fact]
    public void op_IncrementTest()
    {
      AbsoluteNote c2 = AbsoluteNote.Create(Tone.C, Accidental.Natural, 2);

      Assert.Equal(AbsoluteNote.Create(Tone.C, Accidental.Sharp, 2), ++c2);
      Assert.Equal(AbsoluteNote.Create(Tone.D, Accidental.Natural, 2), ++c2);
    }

    [Fact]
    public void op_SubtractionIntTest()
    {
      AbsoluteNote c2 = AbsoluteNote.Create(Tone.C, Accidental.Natural, 2);

      Assert.Equal(AbsoluteNote.Create(Tone.B, Accidental.Natural, 1), c2 - 1);
      Assert.Equal(AbsoluteNote.Create(Tone.A, Accidental.Sharp, 1), c2 - 2);
    }

    [Fact]
    public void op_DecrementTest()
    {
      AbsoluteNote c2 = AbsoluteNote.Create(Tone.C, Accidental.Natural, 2);

      Assert.Equal(AbsoluteNote.Create(Tone.B, Accidental.Natural, 1), --c2);
      Assert.Equal(AbsoluteNote.Create(Tone.A, Accidental.Sharp, 1), --c2);
    }

    [Fact]
    public void op_AdditionIntAccidentalModeTest()
    {
      AbsoluteNote c2 = AbsoluteNote.Create(Tone.C, Accidental.Natural, 2);

      AbsoluteNote.AccidentalMode = AccidentalMode.FavorSharps;

      AbsoluteNote actual = c2 + 1;
      Assert.Equal("C#2", actual.ToString());

      AbsoluteNote.AccidentalMode = AccidentalMode.FavorFlats;

      actual = c2 + 1;
      Assert.Equal("Db2", actual.ToString());
    }

    [Fact]
    public void op_SubtractionIntAccidentalModeTest()
    {
      AbsoluteNote c2 = AbsoluteNote.Create(Tone.C, Accidental.Natural, 2);

      AbsoluteNote.AccidentalMode = AccidentalMode.FavorSharps;

      AbsoluteNote actual = c2 - 2;
      Assert.Equal("A#1", actual.ToString());

      AbsoluteNote.AccidentalMode = AccidentalMode.FavorFlats;

      actual = c2 - 2;
      Assert.Equal("Bb1", actual.ToString());
    }

    [Fact]
    public void TryParseTest()
    {
      AbsoluteNote actual;
      Assert.True(AbsoluteNote.TryParse("C", out actual));
      Assert.Equal(AbsoluteNote.Create(Tone.C, Accidental.Natural, 4), actual);

      Assert.True(AbsoluteNote.TryParse("C#", out actual));
      Assert.Equal(AbsoluteNote.Create(Tone.C, Accidental.Sharp, 4), actual);

      Assert.True(AbsoluteNote.TryParse("C##", out actual));
      Assert.Equal(AbsoluteNote.Create(Tone.C, Accidental.DoubleSharp, 4), actual);

      Assert.True(AbsoluteNote.TryParse("Cb", out actual));
      Assert.Equal(AbsoluteNote.Create(Tone.C, Accidental.Flat, 4), actual);

      Assert.True(AbsoluteNote.TryParse("Cbb", out actual));
      Assert.Equal(AbsoluteNote.Create(Tone.C, Accidental.DoubleFlat, 4), actual);

      Assert.True(AbsoluteNote.TryParse("C2", out actual));
      Assert.Equal(AbsoluteNote.Create(Tone.C, Accidental.Natural, 2), actual);

      Assert.True(AbsoluteNote.TryParse("C#2", out actual));
      Assert.Equal(AbsoluteNote.Create(Tone.C, Accidental.Sharp, 2), actual);

      Assert.True(AbsoluteNote.TryParse("C##2", out actual));
      Assert.Equal(AbsoluteNote.Create(Tone.C, Accidental.DoubleSharp, 2), actual);

      Assert.True(AbsoluteNote.TryParse("Cb2", out actual));
      Assert.Equal(AbsoluteNote.Create(Tone.C, Accidental.Flat, 2), actual);

      Assert.True(AbsoluteNote.TryParse("Cbb2", out actual));
      Assert.Equal(AbsoluteNote.Create(Tone.C, Accidental.DoubleFlat, 2), actual);

      Assert.True(AbsoluteNote.TryParse("60", out actual));
      Assert.Equal(AbsoluteNote.Create(Tone.C, Accidental.Natural, 4), actual);

      Assert.False(AbsoluteNote.TryParse("H", out actual));
      Assert.False(actual.IsValid);

      Assert.False(AbsoluteNote.TryParse("C!", out actual));
      Assert.False(actual.IsValid);

      Assert.False(AbsoluteNote.TryParse("C#-1", out actual));
      Assert.False(actual.IsValid);

      Assert.False(AbsoluteNote.TryParse("C#10", out actual));
      Assert.False(actual.IsValid);

      Assert.False(AbsoluteNote.TryParse("C#b2", out actual));
      Assert.False(actual.IsValid);

      Assert.False(AbsoluteNote.TryParse("Cb#2", out actual));
      Assert.False(actual.IsValid);

      Assert.False(AbsoluteNote.TryParse(null, out actual));
      Assert.False(AbsoluteNote.TryParse("", out actual));
      Assert.False(AbsoluteNote.TryParse("256", out actual));
      Assert.False(AbsoluteNote.TryParse("-1", out actual));
      Assert.False(AbsoluteNote.TryParse("1X", out actual));
    }

    [Fact]
    public void ParseTest()
    {
      Assert.NotNull(AbsoluteNote.Parse("G9"));
      Assert.Throws<FormatException>(() => AbsoluteNote.Parse("C$4"));
      Assert.Throws<ArgumentOutOfRangeException>(() => { AbsoluteNote.Parse("A9"); });
    }

    [Fact]
    public void FrequencyTest()
    {
      Assert.Equal(440.0, Math.Round(AbsoluteNote.Parse("A4").Frequency, 2));
      Assert.Equal(523.25, Math.Round(AbsoluteNote.Parse("C5").Frequency, 2));
      Assert.Equal(349.23, Math.Round(AbsoluteNote.Parse("F4").Frequency, 2));
      Assert.Equal(880.0, Math.Round(AbsoluteNote.Parse("A5").Frequency, 2));
    }

    [Fact]
    public void MidiTest()
    {
      Assert.Equal(12, AbsoluteNote.Parse("C0").Midi);
      Assert.Equal(24, AbsoluteNote.Parse("C1").Midi);
      Assert.Equal(36, AbsoluteNote.Parse("C2").Midi);
      Assert.Equal(48, AbsoluteNote.Parse("C3").Midi);
      Assert.Equal(60, AbsoluteNote.Parse("C4").Midi);
      Assert.Equal(72, AbsoluteNote.Parse("C5").Midi);
      Assert.Equal(84, AbsoluteNote.Parse("C6").Midi);
      Assert.Equal(96, AbsoluteNote.Parse("C7").Midi);
      Assert.Equal(108, AbsoluteNote.Parse("C8").Midi);
      Assert.Equal(120, AbsoluteNote.Parse("C9").Midi);
      Assert.Equal(127, AbsoluteNote.Parse("G9").Midi);
      Assert.Throws<ArgumentOutOfRangeException>(() => AbsoluteNote.CreateFromMidi(11));
    }

    [Fact]
    public void ApplyAccidentalTest()
    {
      AbsoluteNote expected = AbsoluteNote.Parse("C4");
      Assert.Equal(AbsoluteNote.Parse("Bb3"), expected.ApplyAccidental(Accidental.DoubleFlat));
      Assert.Equal(AbsoluteNote.Parse("B3"), expected.ApplyAccidental(Accidental.Flat));
      Assert.Equal(AbsoluteNote.Parse("C4"), expected.ApplyAccidental(Accidental.Natural));
      Assert.Equal(AbsoluteNote.Parse("C#4"), expected.ApplyAccidental(Accidental.Sharp));
      Assert.Equal(AbsoluteNote.Parse("C##4"), expected.ApplyAccidental(Accidental.DoubleSharp));
    }

    #endregion
  }
}
