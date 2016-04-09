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
    public void ConstructorTest()
    {
      ConstructorTestImpl(Tone.C, Accidental.DoubleFlat, 10);
      ConstructorTestImpl(Tone.C, Accidental.Flat, 11);
      ConstructorTestImpl(Tone.C, Accidental.Natural, 0);
      ConstructorTestImpl(Tone.C, Accidental.Sharp, 1);
      ConstructorTestImpl(Tone.C, Accidental.DoubleSharp, 2);

      ConstructorTestImpl(Tone.D, Accidental.DoubleFlat, 0);
      ConstructorTestImpl(Tone.D, Accidental.Flat, 1);
      ConstructorTestImpl(Tone.D, Accidental.Natural, 2);
      ConstructorTestImpl(Tone.D, Accidental.Sharp, 3);
      ConstructorTestImpl(Tone.D, Accidental.DoubleSharp, 4);

      ConstructorTestImpl(Tone.E, Accidental.DoubleFlat, 2);
      ConstructorTestImpl(Tone.E, Accidental.Flat, 3);
      ConstructorTestImpl(Tone.E, Accidental.Natural, 4);
      ConstructorTestImpl(Tone.E, Accidental.Sharp, 5);
      ConstructorTestImpl(Tone.E, Accidental.DoubleSharp, 6);

      ConstructorTestImpl(Tone.F, Accidental.DoubleFlat, 3);
      ConstructorTestImpl(Tone.F, Accidental.Flat, 4);
      ConstructorTestImpl(Tone.F, Accidental.Natural, 5);
      ConstructorTestImpl(Tone.F, Accidental.Sharp, 6);
      ConstructorTestImpl(Tone.F, Accidental.DoubleSharp, 7);

      ConstructorTestImpl(Tone.G, Accidental.DoubleFlat, 5);
      ConstructorTestImpl(Tone.G, Accidental.Flat, 6);
      ConstructorTestImpl(Tone.G, Accidental.Natural, 7);
      ConstructorTestImpl(Tone.G, Accidental.Sharp, 8);
      ConstructorTestImpl(Tone.G, Accidental.DoubleSharp, 9);

      ConstructorTestImpl(Tone.A, Accidental.DoubleFlat, 7);
      ConstructorTestImpl(Tone.A, Accidental.Flat, 8);
      ConstructorTestImpl(Tone.A, Accidental.Natural, 9);
      ConstructorTestImpl(Tone.A, Accidental.Sharp, 10);
      ConstructorTestImpl(Tone.A, Accidental.DoubleSharp, 11);

      ConstructorTestImpl(Tone.B, Accidental.DoubleFlat, 9);
      ConstructorTestImpl(Tone.B, Accidental.Flat, 10);
      ConstructorTestImpl(Tone.B, Accidental.Natural, 11);
      ConstructorTestImpl(Tone.B, Accidental.Sharp, 0);
      ConstructorTestImpl(Tone.B, Accidental.DoubleSharp, 1);
    }

    [Fact]
    public void PredefinedNoteTest()
    {
      NoteMemberTestImpl(Note.C, Tone.C, Accidental.Natural, 0);
      NoteMemberTestImpl(Note.CSharp, Tone.C, Accidental.Sharp, 1);
      NoteMemberTestImpl(Note.DFlat, Tone.D, Accidental.Flat, 1);
      NoteMemberTestImpl(Note.D, Tone.D, Accidental.Natural, 2);
      NoteMemberTestImpl(Note.DSharp, Tone.D, Accidental.Sharp, 3);
      NoteMemberTestImpl(Note.EFlat, Tone.E, Accidental.Flat, 3);
      NoteMemberTestImpl(Note.E, Tone.E, Accidental.Natural, 4);
      NoteMemberTestImpl(Note.F, Tone.F, Accidental.Natural, 5);
      NoteMemberTestImpl(Note.FSharp, Tone.F, Accidental.Sharp, 6);
      NoteMemberTestImpl(Note.GFlat, Tone.G, Accidental.Flat, 6);
      NoteMemberTestImpl(Note.G, Tone.G, Accidental.Natural, 7);
      NoteMemberTestImpl(Note.GSharp, Tone.G, Accidental.Sharp, 8);
      NoteMemberTestImpl(Note.AFlat, Tone.A, Accidental.Flat, 8);
      NoteMemberTestImpl(Note.A, Tone.A, Accidental.Natural, 9);
      NoteMemberTestImpl(Note.ASharp, Tone.A, Accidental.Sharp, 10);
      NoteMemberTestImpl(Note.BFlat, Tone.B, Accidental.Flat, 10);
      NoteMemberTestImpl(Note.B, Tone.B, Accidental.Natural, 11);
    }

    [Fact]
    public void NextTest()
    {
      NextTestImpl(Note.C, Note.CSharp, Note.DFlat);
      NextTestImpl(Note.CSharp, Note.D);
      NextTestImpl(Note.DFlat, Note.D);
      NextTestImpl(Note.D, Note.DSharp, Note.EFlat);
      NextTestImpl(Note.DSharp, Note.E);
      NextTestImpl(Note.EFlat, Note.E);
      NextTestImpl(Note.E, Note.F);
      NextTestImpl(Note.F, Note.FSharp, Note.GFlat);
      NextTestImpl(Note.FSharp, Note.G);
      NextTestImpl(Note.GFlat, Note.G);
      NextTestImpl(Note.G, Note.GSharp, Note.AFlat);
      NextTestImpl(Note.GSharp, Note.A);
      NextTestImpl(Note.AFlat, Note.A);
      NextTestImpl(Note.A, Note.ASharp, Note.BFlat);
      NextTestImpl(Note.ASharp, Note.B);
      NextTestImpl(Note.BFlat, Note.B);
      NextTestImpl(Note.B, Note.C);
    }

    [Fact]
    public void AddTest()
    {
      AddTestImpl(Note.C, 1, Note.CSharp, Note.DFlat);
      AddTestImpl(Note.C, 2, Note.D);
      AddTestImpl(Note.C, 3, Note.DSharp, Note.EFlat);
      AddTestImpl(Note.C, 4, Note.E);
      AddTestImpl(Note.C, 5, Note.F);
      AddTestImpl(Note.C, 6, Note.FSharp, Note.GFlat);
      AddTestImpl(Note.C, 7, Note.G);
      AddTestImpl(Note.C, 8, Note.GSharp, Note.AFlat);
      AddTestImpl(Note.C, 9, Note.A);
      AddTestImpl(Note.C, 10, Note.ASharp, Note.BFlat);
      AddTestImpl(Note.C, 11, Note.B);
      AddTestImpl(Note.C, 12, Note.C);
    }

    [Fact]
    public void PreviousTest()
    {
      PreviousTestImpl(Note.C, Note.B);
      PreviousTestImpl(Note.CSharp, Note.C);
      PreviousTestImpl(Note.DFlat, Note.C);
      PreviousTestImpl(Note.D, Note.CSharp, Note.DFlat);
      PreviousTestImpl(Note.DSharp, Note.D);
      PreviousTestImpl(Note.EFlat, Note.D);
      PreviousTestImpl(Note.E, Note.DSharp, Note.EFlat);
      PreviousTestImpl(Note.F, Note.E);
      PreviousTestImpl(Note.FSharp, Note.F);
      PreviousTestImpl(Note.GFlat, Note.F);
      PreviousTestImpl(Note.G, Note.FSharp, Note.GFlat);
      PreviousTestImpl(Note.GSharp, Note.G);
      PreviousTestImpl(Note.AFlat, Note.G);
      PreviousTestImpl(Note.A, Note.GSharp, Note.AFlat);
      PreviousTestImpl(Note.ASharp, Note.A);
      PreviousTestImpl(Note.BFlat, Note.A);
      PreviousTestImpl(Note.B, Note.ASharp, Note.BFlat);
    }

    [Fact]
    public void SubtractTest()
    {
      SubtractTestImpl(Note.B, 1, Note.BFlat, Note.ASharp);
      SubtractTestImpl(Note.B, 2, Note.A);
      SubtractTestImpl(Note.B, 3, Note.GSharp, Note.AFlat);
      SubtractTestImpl(Note.B, 4, Note.G);
      SubtractTestImpl(Note.B, 5, Note.FSharp, Note.GFlat);
      SubtractTestImpl(Note.B, 6, Note.F);
      SubtractTestImpl(Note.B, 7, Note.E);
      SubtractTestImpl(Note.B, 8, Note.DSharp, Note.EFlat);
      SubtractTestImpl(Note.B, 9, Note.D);
      SubtractTestImpl(Note.B, 10, Note.CSharp, Note.DFlat);
      SubtractTestImpl(Note.B, 11, Note.C);
      SubtractTestImpl(Note.B, 12, Note.B);
    }

    [Fact]
    public void CompareToTest()
    {
      Assert.True(Note.C.CompareTo(Note.C) == 0);
      Assert.True(Note.C.CompareTo(Note.D) < 0);
      Assert.True(Note.D.CompareTo(Note.C) > 0);
      Assert.True(Note.C.CompareTo(Note.B) < 0);
      Assert.True(Note.B.CompareTo(Note.C) > 0);
    }

    [Fact]
    public void TryParseTest()
    {
      TryParseTestImpl("C", Note.C);
      TryParseTestImpl("C#", Note.CSharp);
      TryParseTestImpl("C##", Note.D);
      TryParseTestImpl("Cb", Note.B);
      TryParseTestImpl("Cbb", Note.BFlat);
      TryParseTestImpl("B#", Note.C);
      TryParseTestImpl("B##", Note.CSharp);
      TryParseTestImpl("Bb", Note.BFlat);
      TryParseTestImpl("Bbb", Note.A);
    }

    [Fact]
    public void TryParseRejectsInvalidStringsTest()
    {
      Note note;
      Assert.False(Note.TryParse(null, out note));
      Assert.False(Note.TryParse("", out note));
      Assert.False(Note.TryParse("J", out note));
      Assert.False(Note.TryParse("C$", out note));
    }

    [Fact]
    public void ParseTest()
    {
      Assert.Equal(new Note(Tone.C, Accidental.DoubleFlat) , Note.Parse("Cbb"));
      Assert.Equal(new Note(Tone.C, Accidental.Flat) , Note.Parse("CB"));
      Assert.Equal(Note.C, Note.Parse("C"));
      Assert.Equal(Note.CSharp, Note.Parse("c#"));
      Assert.Equal(new Note(Tone.C, Accidental.DoubleSharp), Note.Parse("c##"));
    }

    [Fact]
    public void ParseRejectsInvalidStringsTest()
    {
      Assert.Throws<ArgumentNullException>(() => Note.Parse(null));
      Assert.Throws<ArgumentException>(() => Note.Parse(""));
      Assert.Throws<FormatException>(() => Note.Parse("J"));
      Assert.Throws<FormatException>(() => Note.Parse("C$"));
    }

    [Fact]
    public void EqualsTest()
    {
      object actual = new Note(Tone.C);
      Assert.True(Note.C.Equals(actual));
      Assert.False(Note.C.Equals(null));
    }

    [Fact]
    public void AccidentalModeTest()
    {
      Note.AccidentalMode = AccidentalMode.FavorFlats;
      Assert.Equal(AccidentalMode.FavorFlats, Note.AccidentalMode);
      Note.AccidentalMode = AccidentalMode.FavorSharps;
      Assert.Equal(AccidentalMode.FavorSharps, Note.AccidentalMode);
    }

    [Fact]
    public void LogicalOperatorsTest()
    {
      Assert.True(Note.C == new Note(Tone.B, Accidental.Sharp));
      Assert.True(Note.C != Note.B);
      Assert.True(Note.C < Note.B);
      Assert.True(Note.C <= Note.B);
      Assert.True(Note.D > Note.C);
      Assert.True(Note.D >= Note.C);
    }

    [Fact]
    public void ArithmeticOperatorsTest()
    {
      Assert.Equal(Note.C, Note.B + 1);
      Assert.Equal(Note.C, Note.C + 12);
      Assert.Equal(Note.B, Note.C - 1);
      Assert.Equal(Note.C, Note.C - 12);

      Note note = Note.B;
      Assert.Equal(Note.B, note++);
      Assert.Equal(Note.C, note);
      Assert.Equal(Note.CSharp, ++note);

      note = Note.C;
      Assert.Equal(Note.C, note--);
      Assert.Equal(Note.B, note);
      Assert.Equal(Note.BFlat, --note);
    }

    [Fact]
    public void ToStringTest()
    {
      Assert.Equal("Cbb", new Note(Tone.C, Accidental.DoubleFlat).ToString());
      Assert.Equal("Cb", new Note(Tone.C, Accidental.Flat).ToString());
      Assert.Equal("C", new Note(Tone.C).ToString());
      Assert.Equal("C#", new Note(Tone.C, Accidental.Sharp).ToString());
      Assert.Equal("C##", new Note(Tone.C, Accidental.DoubleSharp).ToString());
    }

    #endregion

    #region Implementation

    private static void ConstructorTestImpl(Tone tone, Accidental accidental, int interval)
    {
      var note = new Note(tone, accidental);
      NoteMemberTestImpl(note, tone, accidental, interval);
    }

    private static void NoteMemberTestImpl(Note note, Tone tone, Accidental accidental, int interval)
    {
      Assert.Equal(interval, note.Interval);
      Assert.Equal(tone, note.Tone);
      Assert.Equal(accidental, note.Accidental);
    }

    private static void TryParseTestImpl(string value, Note expected)
    {
      Note actual;
      Assert.True(Note.TryParse(value, out actual));
      Assert.Equal(expected, actual);
    }

    private static void NextTestImpl(Note note, Note expectedSharp, Note? expectedFlat = null)
    {
      AddTestImpl(note, 1, expectedSharp, expectedFlat);
    }

    private static void PreviousTestImpl(Note note, Note expectedSharp, Note? expectedFlat = null)
    {
      SubtractTestImpl(note, 1, expectedSharp, expectedFlat);
    }

    private static void AddTestImpl(Note note, int interval, Note expectedSharp, Note? expectedFlat = null)
    {
      Assert.Equal(expectedSharp, note.Add(interval, AccidentalMode.FavorSharps));
      Assert.Equal(expectedFlat ?? expectedSharp, note.Add(interval, AccidentalMode.FavorFlats));
    }

    private static void SubtractTestImpl(Note note, int interval, Note expectedSharp, Note? expectedFlat = null)
    {
      Assert.Equal(expectedSharp, note.Subtract(interval, AccidentalMode.FavorSharps));
      Assert.Equal(expectedFlat ?? expectedSharp, note.Subtract(interval, AccidentalMode.FavorFlats));
    }

    #endregion
  }
}
