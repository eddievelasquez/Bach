//
// Module Name: ToneTest.cs
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

  public class ToneTest
  {
    #region Public Methods

    [Fact]
    public void ConstructorTest()
    {
      ConstructorTestImpl(ToneName.C, Accidental.DoubleFlat, 10);
      ConstructorTestImpl(ToneName.C, Accidental.Flat, 11);
      ConstructorTestImpl(ToneName.C, Accidental.Natural, 0);
      ConstructorTestImpl(ToneName.C, Accidental.Sharp, 1);
      ConstructorTestImpl(ToneName.C, Accidental.DoubleSharp, 2);

      ConstructorTestImpl(ToneName.D, Accidental.DoubleFlat, 0);
      ConstructorTestImpl(ToneName.D, Accidental.Flat, 1);
      ConstructorTestImpl(ToneName.D, Accidental.Natural, 2);
      ConstructorTestImpl(ToneName.D, Accidental.Sharp, 3);
      ConstructorTestImpl(ToneName.D, Accidental.DoubleSharp, 4);

      ConstructorTestImpl(ToneName.E, Accidental.DoubleFlat, 2);
      ConstructorTestImpl(ToneName.E, Accidental.Flat, 3);
      ConstructorTestImpl(ToneName.E, Accidental.Natural, 4);
      ConstructorTestImpl(ToneName.E, Accidental.Sharp, 5);
      ConstructorTestImpl(ToneName.E, Accidental.DoubleSharp, 6);

      ConstructorTestImpl(ToneName.F, Accidental.DoubleFlat, 3);
      ConstructorTestImpl(ToneName.F, Accidental.Flat, 4);
      ConstructorTestImpl(ToneName.F, Accidental.Natural, 5);
      ConstructorTestImpl(ToneName.F, Accidental.Sharp, 6);
      ConstructorTestImpl(ToneName.F, Accidental.DoubleSharp, 7);

      ConstructorTestImpl(ToneName.G, Accidental.DoubleFlat, 5);
      ConstructorTestImpl(ToneName.G, Accidental.Flat, 6);
      ConstructorTestImpl(ToneName.G, Accidental.Natural, 7);
      ConstructorTestImpl(ToneName.G, Accidental.Sharp, 8);
      ConstructorTestImpl(ToneName.G, Accidental.DoubleSharp, 9);

      ConstructorTestImpl(ToneName.A, Accidental.DoubleFlat, 7);
      ConstructorTestImpl(ToneName.A, Accidental.Flat, 8);
      ConstructorTestImpl(ToneName.A, Accidental.Natural, 9);
      ConstructorTestImpl(ToneName.A, Accidental.Sharp, 10);
      ConstructorTestImpl(ToneName.A, Accidental.DoubleSharp, 11);

      ConstructorTestImpl(ToneName.B, Accidental.DoubleFlat, 9);
      ConstructorTestImpl(ToneName.B, Accidental.Flat, 10);
      ConstructorTestImpl(ToneName.B, Accidental.Natural, 11);
      ConstructorTestImpl(ToneName.B, Accidental.Sharp, 0);
      ConstructorTestImpl(ToneName.B, Accidental.DoubleSharp, 1);
    }

    [Fact]
    public void PredefinedNoteTest()
    {
      ToneMemberTestImpl(Tone.C, ToneName.C, Accidental.Natural, 0);
      ToneMemberTestImpl(Tone.CSharp, ToneName.C, Accidental.Sharp, 1);
      ToneMemberTestImpl(Tone.DFlat, ToneName.D, Accidental.Flat, 1);
      ToneMemberTestImpl(Tone.D, ToneName.D, Accidental.Natural, 2);
      ToneMemberTestImpl(Tone.DSharp, ToneName.D, Accidental.Sharp, 3);
      ToneMemberTestImpl(Tone.EFlat, ToneName.E, Accidental.Flat, 3);
      ToneMemberTestImpl(Tone.E, ToneName.E, Accidental.Natural, 4);
      ToneMemberTestImpl(Tone.F, ToneName.F, Accidental.Natural, 5);
      ToneMemberTestImpl(Tone.FSharp, ToneName.F, Accidental.Sharp, 6);
      ToneMemberTestImpl(Tone.GFlat, ToneName.G, Accidental.Flat, 6);
      ToneMemberTestImpl(Tone.G, ToneName.G, Accidental.Natural, 7);
      ToneMemberTestImpl(Tone.GSharp, ToneName.G, Accidental.Sharp, 8);
      ToneMemberTestImpl(Tone.AFlat, ToneName.A, Accidental.Flat, 8);
      ToneMemberTestImpl(Tone.A, ToneName.A, Accidental.Natural, 9);
      ToneMemberTestImpl(Tone.ASharp, ToneName.A, Accidental.Sharp, 10);
      ToneMemberTestImpl(Tone.BFlat, ToneName.B, Accidental.Flat, 10);
      ToneMemberTestImpl(Tone.B, ToneName.B, Accidental.Natural, 11);
    }

    [Fact]
    public void NextTest()
    {
      NextTestImpl(Tone.C, Tone.CSharp, Tone.DFlat);
      NextTestImpl(Tone.CSharp, Tone.D);
      NextTestImpl(Tone.DFlat, Tone.D);
      NextTestImpl(Tone.D, Tone.DSharp, Tone.EFlat);
      NextTestImpl(Tone.DSharp, Tone.E);
      NextTestImpl(Tone.EFlat, Tone.E);
      NextTestImpl(Tone.E, Tone.F);
      NextTestImpl(Tone.F, Tone.FSharp, Tone.GFlat);
      NextTestImpl(Tone.FSharp, Tone.G);
      NextTestImpl(Tone.GFlat, Tone.G);
      NextTestImpl(Tone.G, Tone.GSharp, Tone.AFlat);
      NextTestImpl(Tone.GSharp, Tone.A);
      NextTestImpl(Tone.AFlat, Tone.A);
      NextTestImpl(Tone.A, Tone.ASharp, Tone.BFlat);
      NextTestImpl(Tone.ASharp, Tone.B);
      NextTestImpl(Tone.BFlat, Tone.B);
      NextTestImpl(Tone.B, Tone.C);
    }

    [Fact]
    public void AddTest()
    {
      AddTestImpl(Tone.C, 1, Tone.CSharp, Tone.DFlat);
      AddTestImpl(Tone.C, 2, Tone.D);
      AddTestImpl(Tone.C, 3, Tone.DSharp, Tone.EFlat);
      AddTestImpl(Tone.C, 4, Tone.E);
      AddTestImpl(Tone.C, 5, Tone.F);
      AddTestImpl(Tone.C, 6, Tone.FSharp, Tone.GFlat);
      AddTestImpl(Tone.C, 7, Tone.G);
      AddTestImpl(Tone.C, 8, Tone.GSharp, Tone.AFlat);
      AddTestImpl(Tone.C, 9, Tone.A);
      AddTestImpl(Tone.C, 10, Tone.ASharp, Tone.BFlat);
      AddTestImpl(Tone.C, 11, Tone.B);
      AddTestImpl(Tone.C, 12, Tone.C);
    }

    [Fact]
    public void PreviousTest()
    {
      PreviousTestImpl(Tone.C, Tone.B);
      PreviousTestImpl(Tone.CSharp, Tone.C);
      PreviousTestImpl(Tone.DFlat, Tone.C);
      PreviousTestImpl(Tone.D, Tone.CSharp, Tone.DFlat);
      PreviousTestImpl(Tone.DSharp, Tone.D);
      PreviousTestImpl(Tone.EFlat, Tone.D);
      PreviousTestImpl(Tone.E, Tone.DSharp, Tone.EFlat);
      PreviousTestImpl(Tone.F, Tone.E);
      PreviousTestImpl(Tone.FSharp, Tone.F);
      PreviousTestImpl(Tone.GFlat, Tone.F);
      PreviousTestImpl(Tone.G, Tone.FSharp, Tone.GFlat);
      PreviousTestImpl(Tone.GSharp, Tone.G);
      PreviousTestImpl(Tone.AFlat, Tone.G);
      PreviousTestImpl(Tone.A, Tone.GSharp, Tone.AFlat);
      PreviousTestImpl(Tone.ASharp, Tone.A);
      PreviousTestImpl(Tone.BFlat, Tone.A);
      PreviousTestImpl(Tone.B, Tone.ASharp, Tone.BFlat);
    }

    [Fact]
    public void SubtractTest()
    {
      SubtractTestImpl(Tone.B, 1, Tone.BFlat, Tone.ASharp);
      SubtractTestImpl(Tone.B, 2, Tone.A);
      SubtractTestImpl(Tone.B, 3, Tone.GSharp, Tone.AFlat);
      SubtractTestImpl(Tone.B, 4, Tone.G);
      SubtractTestImpl(Tone.B, 5, Tone.FSharp, Tone.GFlat);
      SubtractTestImpl(Tone.B, 6, Tone.F);
      SubtractTestImpl(Tone.B, 7, Tone.E);
      SubtractTestImpl(Tone.B, 8, Tone.DSharp, Tone.EFlat);
      SubtractTestImpl(Tone.B, 9, Tone.D);
      SubtractTestImpl(Tone.B, 10, Tone.CSharp, Tone.DFlat);
      SubtractTestImpl(Tone.B, 11, Tone.C);
      SubtractTestImpl(Tone.B, 12, Tone.B);
    }

    [Fact]
    public void CompareToTest()
    {
      Assert.True(Tone.C.CompareTo(Tone.C) == 0);
      Assert.True(Tone.C.CompareTo(Tone.D) < 0);
      Assert.True(Tone.D.CompareTo(Tone.C) > 0);
      Assert.True(Tone.C.CompareTo(Tone.B) < 0);
      Assert.True(Tone.B.CompareTo(Tone.C) > 0);
    }

    [Fact]
    public void TryParseTest()
    {
      TryParseTestImpl("C", Tone.C);
      TryParseTestImpl("C#", Tone.CSharp);
      TryParseTestImpl("C##", Tone.D);
      TryParseTestImpl("Cb", Tone.B);
      TryParseTestImpl("Cbb", Tone.BFlat);
      TryParseTestImpl("B#", Tone.C);
      TryParseTestImpl("B##", Tone.CSharp);
      TryParseTestImpl("Bb", Tone.BFlat);
      TryParseTestImpl("Bbb", Tone.A);
    }

    [Fact]
    public void TryParseRejectsInvalidStringsTest()
    {
      Tone tone;
      Assert.False(Tone.TryParse(null, out tone));
      Assert.False(Tone.TryParse("", out tone));
      Assert.False(Tone.TryParse("J", out tone));
      Assert.False(Tone.TryParse("C$", out tone));
    }

    [Fact]
    public void ParseTest()
    {
      Assert.Equal(new Tone(ToneName.C, Accidental.DoubleFlat) , Tone.Parse("Cbb"));
      Assert.Equal(new Tone(ToneName.C, Accidental.Flat) , Tone.Parse("CB"));
      Assert.Equal(Tone.C, Tone.Parse("C"));
      Assert.Equal(Tone.CSharp, Tone.Parse("c#"));
      Assert.Equal(new Tone(ToneName.C, Accidental.DoubleSharp), Tone.Parse("c##"));
    }

    [Fact]
    public void ParseRejectsInvalidStringsTest()
    {
      Assert.Throws<ArgumentNullException>(() => Tone.Parse(null));
      Assert.Throws<ArgumentException>(() => Tone.Parse(""));
      Assert.Throws<FormatException>(() => Tone.Parse("J"));
      Assert.Throws<FormatException>(() => Tone.Parse("C$"));
    }

    [Fact]
    public void EqualsTest()
    {
      object actual = new Tone(ToneName.C);
      Assert.True(Tone.C.Equals(actual));
      Assert.False(Tone.C.Equals(null));
    }

    [Fact]
    public void AccidentalModeTest()
    {
      Tone.AccidentalMode = AccidentalMode.FavorFlats;
      Assert.Equal(AccidentalMode.FavorFlats, Tone.AccidentalMode);
      Tone.AccidentalMode = AccidentalMode.FavorSharps;
      Assert.Equal(AccidentalMode.FavorSharps, Tone.AccidentalMode);
    }

    [Fact]
    public void LogicalOperatorsTest()
    {
      Assert.True(Tone.C == new Tone(ToneName.B, Accidental.Sharp));
      Assert.True(Tone.C != Tone.B);
      Assert.True(Tone.C < Tone.B);
      Assert.True(Tone.C <= Tone.B);
      Assert.True(Tone.D > Tone.C);
      Assert.True(Tone.D >= Tone.C);
    }

    [Fact]
    public void ArithmeticOperatorsTest()
    {
      Assert.Equal(Tone.C, Tone.B + 1);
      Assert.Equal(Tone.C, Tone.C + 12);
      Assert.Equal(Tone.B, Tone.C - 1);
      Assert.Equal(Tone.C, Tone.C - 12);

      Tone tone = Tone.B;
      Assert.Equal(Tone.B, tone++);
      Assert.Equal(Tone.C, tone);
      Assert.Equal(Tone.CSharp, ++tone);

      tone = Tone.C;
      Assert.Equal(Tone.C, tone--);
      Assert.Equal(Tone.B, tone);
      Assert.Equal(Tone.BFlat, --tone);
    }

    [Fact]
    public void ToStringTest()
    {
      Assert.Equal("Cbb", new Tone(ToneName.C, Accidental.DoubleFlat).ToString());
      Assert.Equal("Cb", new Tone(ToneName.C, Accidental.Flat).ToString());
      Assert.Equal("C", new Tone(ToneName.C).ToString());
      Assert.Equal("C#", new Tone(ToneName.C, Accidental.Sharp).ToString());
      Assert.Equal("C##", new Tone(ToneName.C, Accidental.DoubleSharp).ToString());
    }

    #endregion

    #region Implementation

    private static void ConstructorTestImpl(ToneName toneName, Accidental accidental, int interval)
    {
      var note = new Tone(toneName, accidental);
      ToneMemberTestImpl(note, toneName, accidental, interval);
    }

    private static void ToneMemberTestImpl(Tone tone, ToneName toneName, Accidental accidental, int interval)
    {
      Assert.Equal(interval, tone.Interval);
      Assert.Equal(toneName, tone.ToneName);
      Assert.Equal(accidental, tone.Accidental);
    }

    private static void TryParseTestImpl(string value, Tone expected)
    {
      Tone actual;
      Assert.True(Tone.TryParse(value, out actual));
      Assert.Equal(expected, actual);
    }

    private static void NextTestImpl(Tone tone, Tone expectedSharp, Tone? expectedFlat = null)
    {
      AddTestImpl(tone, 1, expectedSharp, expectedFlat);
    }

    private static void PreviousTestImpl(Tone tone, Tone expectedSharp, Tone? expectedFlat = null)
    {
      SubtractTestImpl(tone, 1, expectedSharp, expectedFlat);
    }

    private static void AddTestImpl(Tone tone, int interval, Tone expectedSharp, Tone? expectedFlat = null)
    {
      Assert.Equal(expectedSharp, tone.Add(interval, AccidentalMode.FavorSharps));
      Assert.Equal(expectedFlat ?? expectedSharp, tone.Add(interval, AccidentalMode.FavorFlats));
    }

    private static void SubtractTestImpl(Tone tone, int interval, Tone expectedSharp, Tone? expectedFlat = null)
    {
      Assert.Equal(expectedSharp, tone.Subtract(interval, AccidentalMode.FavorSharps));
      Assert.Equal(expectedFlat ?? expectedSharp, tone.Subtract(interval, AccidentalMode.FavorFlats));
    }

    #endregion
  }
}
