//
// Module Name: PitchTest.cs
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
// PARTICULAR PURPOSE AND NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE
// OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

namespace Bach.Model.Test
{
  using System;
  using Xunit;

  public class PitchTest
  {
    [Fact]
    public void CreateWithToneAndAccidentalTest()
    {
      Pitch target = Pitch.Create(NoteName.A, Accidental.Natural, 1);
      Assert.Equal(NoteName.A, target.Note.NoteName);
      Assert.Equal(Accidental.Natural, target.Note.Accidental);
      Assert.Equal(1, target.Octave);

      Assert.Throws<ArgumentOutOfRangeException>(() => Pitch.Create(NoteName.C, Accidental.Flat, Pitch.MinOctave));
      Assert.Throws<ArgumentOutOfRangeException>(() => Pitch.Create(NoteName.C, Accidental.DoubleFlat, Pitch.MinOctave));
      Assert.Throws<ArgumentOutOfRangeException>(() => Pitch.Create(NoteName.B, Accidental.Sharp, Pitch.MaxOctave));
      Assert.Throws<ArgumentOutOfRangeException>(() => Pitch.Create(NoteName.B, Accidental.DoubleSharp, Pitch.MaxOctave));
    }

    [Fact]
    public void CreateWithNoteTest()
    {
      Pitch target = Pitch.Create(Note.A, 1);
      Assert.Equal(Note.A, target.Note);
      Assert.Equal(1, target.Octave);

      Assert.Throws<ArgumentOutOfRangeException>(() => Pitch.Create(Note.Create(NoteName.C, Accidental.Flat), Pitch.MinOctave));
      Assert.Throws<ArgumentOutOfRangeException>(() => Pitch.Create(Note.Create(NoteName.C, Accidental.DoubleFlat), Pitch.MinOctave));
      Assert.Throws<ArgumentOutOfRangeException>(() => Pitch.Create(Note.Create(NoteName.B, Accidental.Sharp), Pitch.MaxOctave));
      Assert.Throws<ArgumentOutOfRangeException>(() => Pitch.Create(Note.Create(NoteName.B, Accidental.DoubleSharp), Pitch.MaxOctave));
    }

    [Fact]
    public void EqualsContractTest()
    {
      object x = Pitch.Create(NoteName.A, Accidental.Natural, 1);
      object y = Pitch.Create(NoteName.A, Accidental.Natural, 1);
      object z = Pitch.Create(NoteName.A, Accidental.Natural, 1);

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
      Pitch x = Pitch.Create(NoteName.A, Accidental.Natural, 1);
      Pitch y = Pitch.Create(NoteName.A, Accidental.Natural, 1);
      Pitch z = Pitch.Create(NoteName.A, Accidental.Natural, 1);

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
      object actual = Pitch.Create(NoteName.A, Accidental.Natural, 1);
      Assert.False(actual.Equals(int.MinValue));
    }

    [Fact]
    public void TypeSafeEqualsFailsWithDifferentTypeTest()
    {
      Pitch actual = Pitch.Create(NoteName.A, Accidental.Natural, 1);
      Assert.False(actual.Equals(int.MinValue));
    }

    [Fact]
    public void EqualsFailsWithNullTest()
    {
      object actual = Pitch.Create(NoteName.A, Accidental.Natural, 1);
      Assert.False(actual.Equals(null));
    }

    [Fact]
    public void TypeSafeEqualsFailsWithNullTest()
    {
      Pitch actual = Pitch.Create(NoteName.A, Accidental.Natural, 1);
      Assert.False(actual.Equals(null));
    }

    [Fact]
    public void EqualsSucceedsWithSameObjectTest()
    {
      Pitch actual = Pitch.Create(NoteName.A, Accidental.Natural, 1);
      Assert.True(actual.Equals(actual));
    }

    [Fact]
    public void GetHashcodeTest()
    {
      Pitch actual = Pitch.Create(NoteName.A, Accidental.Natural, 1);
      Pitch expected = Pitch.Create(NoteName.A, Accidental.Natural, 1);
      Assert.True(expected.Equals(actual));
      Assert.Equal(expected.GetHashCode(), actual.GetHashCode());
    }

    [Fact]
    public void CompareToContractTest()
    {
      {
        Pitch a = Pitch.Create(NoteName.A, Accidental.Natural, 1);
        Assert.True(a.CompareTo(a) == 0);

        Pitch b = Pitch.Create(NoteName.A, Accidental.Natural, 1);
        Assert.True(a.CompareTo(b) == 0);
        Assert.True(b.CompareTo(a) == 0);

        Pitch c = Pitch.Create(NoteName.A, Accidental.Natural, 1);
        Assert.True(b.CompareTo(c) == 0);
        Assert.True(a.CompareTo(c) == 0);
      }
      {
        Pitch a = Pitch.Create(NoteName.C, Accidental.Natural, 1);
        Pitch b = Pitch.Create(NoteName.D, Accidental.Natural, 1);

        Assert.Equal(a.CompareTo(b), -b.CompareTo(a));

        Pitch c = Pitch.Create(NoteName.E, Accidental.Natural, 1);
        Assert.True(a.CompareTo(b) < 0);
        Assert.True(b.CompareTo(c) < 0);
        Assert.True(a.CompareTo(c) < 0);
      }
    }

    [Fact]
    public void CompareToTest()
    {
      Pitch a1 = Pitch.Create(NoteName.A, Accidental.Natural, 1);
      Pitch aSharp1 = Pitch.Create(NoteName.A, Accidental.Sharp, 1);
      Pitch aFlat1 = Pitch.Create(NoteName.A, Accidental.Flat, 1);
      Pitch a2 = Pitch.Create(NoteName.A, Accidental.Natural, 2);
      Pitch aSharp2 = Pitch.Create(NoteName.A, Accidental.Sharp, 2);
      Pitch aFlat2 = Pitch.Create(NoteName.A, Accidental.Flat, 2);

      Assert.True(a1.CompareTo(a1) == 0);
      Assert.True(a1.CompareTo(aSharp1) < 0);
      Assert.True(a1.CompareTo(aFlat1) > 0);
      Assert.True(a1.CompareTo(a2) < 0);
      Assert.True(a1.CompareTo(aFlat2) < 0);
      Assert.True(a1.CompareTo(aSharp2) < 0);

      Pitch c1 = Pitch.Create(NoteName.C, Accidental.Natural, 1);
      Assert.True(a1.CompareTo(c1) > 0);
      Assert.True(c1.CompareTo(a1) < 0);
    }

    [Fact]
    public void ToStringTest()
    {
      Pitch target = Pitch.Create(NoteName.A, Accidental.DoubleFlat, 1);
      Assert.Equal("Abb1", target.ToString());

      target = Pitch.Create(NoteName.A, Accidental.Flat, 1);
      Assert.Equal("Ab1", target.ToString());

      target = Pitch.Create(NoteName.A, Accidental.Natural, 1);
      Assert.Equal("A1", target.ToString());

      target = Pitch.Create(NoteName.A, Accidental.Sharp, 1);
      Assert.Equal("A#1", target.ToString());

      target = Pitch.Create(NoteName.A, Accidental.DoubleSharp, 1);
      Assert.Equal("A##1", target.ToString());
    }

    [Fact]
    public void op_EqualityTest()
    {
      Pitch a = Pitch.Create(NoteName.A, Accidental.Natural, 1);
      Pitch b = Pitch.Create(NoteName.A, Accidental.Natural, 1);
      Pitch c = Pitch.Create(NoteName.B, Accidental.Natural, 1);

      Assert.True(a == b);
      Assert.False(a == c);
      Assert.False(b == c);
    }

    [Fact]
    public void op_InequalityTest()
    {
      Pitch a = Pitch.Create(NoteName.A, Accidental.Natural, 1);
      Pitch b = Pitch.Create(NoteName.A, Accidental.Natural, 1);
      Pitch c = Pitch.Create(NoteName.B, Accidental.Natural, 1);

      Assert.True(a != c);
      Assert.True(b != c);
      Assert.False(a != b);
    }

    [Fact]
    public void ComparisonOperatorsTest()
    {
      Pitch a = Pitch.Create(NoteName.A, Accidental.Natural, 1);
      Pitch b = Pitch.Create(NoteName.B, Accidental.Natural, 1);

      Assert.True(b > a);
      Assert.True(b >= a);
      Assert.False(b < a);
      Assert.False(b <= a);
    }

    [Fact]
    public void op_SubtractionNoteTest()
    {
      Pitch cDoubleFlat2 = Pitch.Create(NoteName.C, Accidental.DoubleFlat, 2);
      Pitch cFlat2 = Pitch.Create(NoteName.C, Accidental.Flat, 2);
      Pitch c2 = Pitch.Create(NoteName.C, Accidental.Natural, 2);
      Pitch cSharp2 = Pitch.Create(NoteName.C, Accidental.Sharp, 2);
      Pitch cDoubleSharp2 = Pitch.Create(NoteName.C, Accidental.DoubleSharp, 2);

      // Test interval with same pitches in the same octave with different accidentals
      Assert.Equal(0, cDoubleFlat2 - cDoubleFlat2);
      Assert.Equal(-1, cDoubleFlat2 - cFlat2);
      Assert.Equal(-2, cDoubleFlat2 - c2);
      Assert.Equal(-3, cDoubleFlat2 - cSharp2);
      Assert.Equal(-4, cDoubleFlat2 - cDoubleSharp2);
      Assert.Equal(1, cFlat2 - cDoubleFlat2);
      Assert.Equal(2, c2 - cDoubleFlat2);
      Assert.Equal(3, cSharp2 - cDoubleFlat2);
      Assert.Equal(4, cDoubleSharp2 - cDoubleFlat2);

      Pitch c3 = Pitch.Create(NoteName.C, Accidental.Natural, 3);
      Assert.Equal(-12, c2 - c3);
      Assert.Equal(12, c3 - c2);
    }

    [Fact]
    public void op_AdditionIntTest()
    {
      Pitch c2 = Pitch.Create(NoteName.C, Accidental.Natural, 2);

      Assert.Equal(Pitch.Create(NoteName.C, Accidental.Sharp, 2), c2 + 1);
      Assert.Equal(Pitch.Create(NoteName.B, Accidental.Natural, 1), c2 + -1);
      Assert.Equal(Pitch.Create(NoteName.D, Accidental.Natural, 2), c2 + 2);
      Assert.Equal(Pitch.Create(NoteName.A, Accidental.Sharp, 1), c2 + -2);
    }

    [Fact]
    public void op_IncrementTest()
    {
      Pitch c2 = Pitch.Create(NoteName.C, Accidental.Natural, 2);

      Assert.Equal(Pitch.Create(NoteName.C, Accidental.Sharp, 2), ++c2);
      Assert.Equal(Pitch.Create(NoteName.D, Accidental.Natural, 2), ++c2);
    }

    [Fact]
    public void op_SubtractionIntTest()
    {
      Pitch c2 = Pitch.Create(NoteName.C, Accidental.Natural, 2);

      Assert.Equal(Pitch.Create(NoteName.B, Accidental.Natural, 1), c2 - 1);
      Assert.Equal(Pitch.Create(NoteName.A, Accidental.Sharp, 1), c2 - 2);
    }

    [Fact]
    public void op_DecrementTest()
    {
      Pitch c2 = Pitch.Create(NoteName.C, Accidental.Natural, 2);

      Assert.Equal(Pitch.Create(NoteName.B, Accidental.Natural, 1), --c2);
      Assert.Equal(Pitch.Create(NoteName.A, Accidental.Sharp, 1), --c2);
    }

    [Fact]
    public void op_AdditionIntAccidentalModeTest()
    {
      Pitch c2 = Pitch.Create(NoteName.C, Accidental.Natural, 2);

      Pitch.AccidentalMode = AccidentalMode.FavorSharps;

      Pitch actual = c2 + 1;
      Assert.Equal("C#2", actual.ToString());

      Pitch.AccidentalMode = AccidentalMode.FavorFlats;

      actual = c2 + 1;
      Assert.Equal("Db2", actual.ToString());
    }

    [Fact]
    public void op_SubtractionIntAccidentalModeTest()
    {
      Pitch c2 = Pitch.Create(NoteName.C, Accidental.Natural, 2);

      Pitch.AccidentalMode = AccidentalMode.FavorSharps;

      Pitch actual = c2 - 2;
      Assert.Equal("A#1", actual.ToString());

      Pitch.AccidentalMode = AccidentalMode.FavorFlats;

      actual = c2 - 2;
      Assert.Equal("Bb1", actual.ToString());
    }

    [Fact]
    public void TryParseTest()
    {
      Assert.True(Pitch.TryParse("C4", out Pitch actual));
      Assert.Equal(Pitch.Create(NoteName.C, Accidental.Natural, 4), actual);

      Assert.True(Pitch.TryParse("C#4", out actual));
      Assert.Equal(Pitch.Create(NoteName.C, Accidental.Sharp, 4), actual);

      Assert.True(Pitch.TryParse("C##4", out actual));
      Assert.Equal(Pitch.Create(NoteName.C, Accidental.DoubleSharp, 4), actual);

      Assert.True(Pitch.TryParse("Cb4", out actual));
      Assert.Equal(Pitch.Create(NoteName.C, Accidental.Flat, 4), actual);

      Assert.True(Pitch.TryParse("Cbb4", out actual));
      Assert.Equal(Pitch.Create(NoteName.C, Accidental.DoubleFlat, 4), actual);

      Assert.True(Pitch.TryParse("C2", out actual));
      Assert.Equal(Pitch.Create(NoteName.C, Accidental.Natural, 2), actual);

      Assert.True(Pitch.TryParse("C#2", out actual));
      Assert.Equal(Pitch.Create(NoteName.C, Accidental.Sharp, 2), actual);

      Assert.True(Pitch.TryParse("C##2", out actual));
      Assert.Equal(Pitch.Create(NoteName.C, Accidental.DoubleSharp, 2), actual);

      Assert.True(Pitch.TryParse("Cb2", out actual));
      Assert.Equal(Pitch.Create(NoteName.C, Accidental.Flat, 2), actual);

      Assert.True(Pitch.TryParse("Cbb2", out actual));
      Assert.Equal(Pitch.Create(NoteName.C, Accidental.DoubleFlat, 2), actual);

      Assert.True(Pitch.TryParse("60", out actual));
      Assert.Equal(Pitch.Create(NoteName.C, Accidental.Natural, 4), actual);

      Assert.False(Pitch.TryParse("H", out actual));
      Assert.False(actual.IsValid);

      Assert.False(Pitch.TryParse("C!", out actual));
      Assert.False(actual.IsValid);

      Assert.False(Pitch.TryParse("C#-1", out actual));
      Assert.False(actual.IsValid);

      Assert.False(Pitch.TryParse("C#10", out actual));
      Assert.False(actual.IsValid);

      Assert.False(Pitch.TryParse("C#b2", out actual));
      Assert.False(actual.IsValid);

      Assert.False(Pitch.TryParse("Cb#2", out actual));
      Assert.False(actual.IsValid);

      Assert.False(Pitch.TryParse(null, out actual));
      Assert.False(Pitch.TryParse("", out actual));
      Assert.False(Pitch.TryParse("256", out actual));
      Assert.False(Pitch.TryParse("-1", out actual));
      Assert.False(Pitch.TryParse("1X", out actual));
    }

    [Fact]
    public void ParseTest()
    {
      Assert.Throws<FormatException>(() => Pitch.Parse("C$4"));
      Assert.Throws<ArgumentOutOfRangeException>(() => { Pitch.Parse("A9"); });
    }

    [Fact]
    public void FrequencyTest()
    {
      Assert.Equal(440.0, Math.Round(Pitch.Parse("A4").Frequency, 2));
      Assert.Equal(523.25, Math.Round(Pitch.Parse("C5").Frequency, 2));
      Assert.Equal(349.23, Math.Round(Pitch.Parse("F4").Frequency, 2));
      Assert.Equal(880.0, Math.Round(Pitch.Parse("A5").Frequency, 2));
    }

    [Fact]
    public void MidiTest()
    {
      Assert.Equal(12, Pitch.Parse("C0").Midi);
      Assert.Equal(24, Pitch.Parse("C1").Midi);
      Assert.Equal(36, Pitch.Parse("C2").Midi);
      Assert.Equal(48, Pitch.Parse("C3").Midi);
      Assert.Equal(60, Pitch.Parse("C4").Midi);
      Assert.Equal(72, Pitch.Parse("C5").Midi);
      Assert.Equal(84, Pitch.Parse("C6").Midi);
      Assert.Equal(96, Pitch.Parse("C7").Midi);
      Assert.Equal(108, Pitch.Parse("C8").Midi);
      Assert.Equal(120, Pitch.Parse("C9").Midi);
      Assert.Equal(127, Pitch.Parse("G9").Midi);
      Assert.Throws<ArgumentOutOfRangeException>(() => Pitch.CreateFromMidi(11));
    }

    [Fact]
    public void PitchIntervalAdditionTest()
    {
      Assert.Equal(Pitch.Parse("E4"), Pitch.Parse("C4") + Interval.MajorThird);
      Assert.Equal(Pitch.Parse("E4"), Pitch.Parse("C#4") + Interval.MinorThird);
      Assert.Equal(Pitch.Parse("F4"), Pitch.Parse("D4") + Interval.MinorThird);
      Assert.Equal(Pitch.Parse("G4"), Pitch.Parse("D4") + Interval.Fourth);
      Assert.Equal(Pitch.Parse("A4"), Pitch.Parse("E4") + Interval.Fourth);
      Assert.Equal(Pitch.Parse("Ab4"), Pitch.Parse("Eb4") + Interval.Fourth);
      Assert.Equal(Pitch.Parse("G#4"), Pitch.Parse("Eb4") + Interval.AugmentedThird);
      Assert.Equal(Pitch.Parse("D5"), Pitch.Parse("F4") + Interval.MajorSixth);
      Assert.Equal(Pitch.Parse("D5"), Pitch.Parse("G4") + Interval.Fifth);
      Assert.Equal(Pitch.Parse("C5"), Pitch.Parse("F4") + Interval.Fifth);
      Assert.Equal(Pitch.Parse("E5"), Pitch.Parse("A4") + Interval.Fifth);
      Assert.Equal(Pitch.Parse("Eb5"), Pitch.Parse("Ab4") + Interval.Fifth);
      Assert.Equal(Pitch.Parse("Eb5"), Pitch.Parse("G#4") + Interval.DiminishedSixth);
      Assert.Equal(Pitch.Parse("C5"), Pitch.Parse("F#4") + Interval.AugmentedFourth);
      Assert.Equal(Pitch.Parse("C5"), Pitch.Parse("Gb4") + Interval.DiminishedFifth);
      Assert.Equal(Pitch.Parse("D#4"), Pitch.Parse("C4") + Interval.AugmentedSecond);
      Assert.Equal(Pitch.Parse("F#4"), Pitch.Parse("C4") + Interval.DiminishedFifth);
      Assert.Equal(Pitch.Parse("Gb4"), Pitch.Parse("C4") + Interval.AugmentedFourth);
      Assert.Equal(Pitch.Parse("C5"), Pitch.Parse("D#4") + Interval.DiminishedSeventh);
      Assert.Equal(Pitch.Parse("F4"), Pitch.Parse("D#4") + Interval.DiminishedThird);
      Assert.Equal(Pitch.Parse("G#4"), Pitch.Parse("D##4") + Interval.DiminishedFourth);
    }
  }
}
