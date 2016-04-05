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
  using Xunit;

  public class NoteTest
  {
    #region Public Methods

    [Fact]
    public void NextTest()
    {
      TestNext(Note.C, Note.CSharp, Note.DFlat);
      TestNext(Note.CSharp, Note.D);
      TestNext(Note.DFlat, Note.D);
      TestNext(Note.D, Note.DSharp, Note.EFlat);
      TestNext(Note.DSharp, Note.E);
      TestNext(Note.EFlat, Note.E);
      TestNext(Note.E, Note.F);
      TestNext(Note.F, Note.FSharp, Note.GFlat);
      TestNext(Note.FSharp, Note.G);
      TestNext(Note.GFlat, Note.G);
      TestNext(Note.G, Note.GSharp, Note.AFlat);
      TestNext(Note.GSharp, Note.A);
      TestNext(Note.AFlat, Note.A);
      TestNext(Note.A, Note.ASharp, Note.BFlat);
      TestNext(Note.ASharp, Note.B);
      TestNext(Note.BFlat, Note.B);
      TestNext(Note.B, Note.C);
    }

    [Fact]
    public void AddTest()
    {
      TestAdd(Note.C, 1, Note.CSharp, Note.DFlat);
      TestAdd(Note.C, 2, Note.D);
      TestAdd(Note.C, 3, Note.DSharp, Note.EFlat);
      TestAdd(Note.C, 4, Note.E);
      TestAdd(Note.C, 5, Note.F);
      TestAdd(Note.C, 6, Note.FSharp, Note.GFlat);
      TestAdd(Note.C, 7, Note.G);
      TestAdd(Note.C, 8, Note.GSharp, Note.AFlat);
      TestAdd(Note.C, 9, Note.A);
      TestAdd(Note.C, 10, Note.ASharp, Note.BFlat);
      TestAdd(Note.C, 11, Note.B);
      TestAdd(Note.C, 12, Note.C);
    }

    [Fact]
    public void PreviousTest()
    {
      TestPrevious(Note.C, Note.B);
      TestPrevious(Note.CSharp, Note.C);
      TestPrevious(Note.DFlat, Note.C);
      TestPrevious(Note.D, Note.CSharp, Note.DFlat);
      TestPrevious(Note.DSharp, Note.D);
      TestPrevious(Note.EFlat, Note.D);
      TestPrevious(Note.E, Note.DSharp, Note.EFlat);
      TestPrevious(Note.F, Note.E);
      TestPrevious(Note.FSharp, Note.F);
      TestPrevious(Note.GFlat, Note.F);
      TestPrevious(Note.G, Note.FSharp, Note.GFlat);
      TestPrevious(Note.GSharp, Note.G);
      TestPrevious(Note.AFlat, Note.G);
      TestPrevious(Note.A, Note.GSharp, Note.AFlat);
      TestPrevious(Note.ASharp, Note.A);
      TestPrevious(Note.BFlat, Note.A);
      TestPrevious(Note.B, Note.ASharp, Note.BFlat);
    }

    [Fact]
    public void SubtractTest()
    {
      TestSubtract(Note.B, 1, Note.BFlat, Note.ASharp);
      TestSubtract(Note.B, 2, Note.A);
      TestSubtract(Note.B, 3, Note.GSharp, Note.AFlat);
      TestSubtract(Note.B, 4, Note.G);
      TestSubtract(Note.B, 5, Note.FSharp, Note.GFlat);
      TestSubtract(Note.B, 6, Note.F);
      TestSubtract(Note.B, 7, Note.E);
      TestSubtract(Note.B, 8, Note.DSharp, Note.EFlat);
      TestSubtract(Note.B, 9, Note.D);
      TestSubtract(Note.B, 10, Note.CSharp, Note.DFlat);
      TestSubtract(Note.B, 11, Note.C);
      TestSubtract(Note.B, 12, Note.B);
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
      TestTryParse("C", Note.C);
      TestTryParse("C#", Note.CSharp);
      TestTryParse("C##", Note.D);
      TestTryParse("Cb", Note.B);
      TestTryParse("Cbb", Note.BFlat);
      TestTryParse("B#", Note.C);
      TestTryParse("B##", Note.CSharp);
      TestTryParse("Bb", Note.BFlat);
      TestTryParse("Bbb", Note.A);
    }

    [Fact]
    public void TryParseRejectsInvalidStrings()
    {
      Note note;
      Assert.False(Note.TryParse(null, out note));
      Assert.False(Note.TryParse("", out note));
      Assert.False(Note.TryParse("J", out note));
      Assert.False(Note.TryParse("C$", out note));
    }

    #endregion

    #region Implementation

    private static void TestTryParse(string value, Note expected)
    {
      Note actual;
      Assert.True(Note.TryParse(value, out actual));
      Assert.Equal(expected, actual);
    }

    private static void TestNext(Note note, Note expectedSharp, Note? expectedFlat = null)
    {
      TestAdd(note, 1, expectedSharp, expectedFlat);
    }

    private static void TestPrevious(Note note, Note expectedSharp, Note? expectedFlat = null)
    {
      TestSubtract(note, 1, expectedSharp, expectedFlat);
    }

    private static void TestAdd(Note note, int interval, Note expectedSharp, Note? expectedFlat = null)
    {
      Assert.Equal(expectedSharp, note.Add(interval, AccidentalMode.FavorSharps));
      Assert.Equal(expectedFlat ?? expectedSharp, note.Add(interval, AccidentalMode.FavorFlats));
    }

    private static void TestSubtract(Note note, int interval, Note expectedSharp, Note? expectedFlat = null)
    {
      Assert.Equal(expectedSharp, note.Subtract(interval, AccidentalMode.FavorSharps));
      Assert.Equal(expectedFlat ?? expectedSharp, note.Subtract(interval, AccidentalMode.FavorFlats));
    }

    #endregion
  }
}
