//  
// Module Name: NoteValueTest.cs
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

  public class NoteValueTest
  {
    #region Public Methods

    [Fact]
    public void NextTest()
    {
      TestNext(NoteValue.C, NoteValue.CSharp, NoteValue.DFlat);
      TestNext(NoteValue.CSharp, NoteValue.D);
      TestNext(NoteValue.DFlat, NoteValue.D);
      TestNext(NoteValue.D, NoteValue.DSharp, NoteValue.EFlat);
      TestNext(NoteValue.DSharp, NoteValue.E);
      TestNext(NoteValue.EFlat, NoteValue.E);
      TestNext(NoteValue.E, NoteValue.F);
      TestNext(NoteValue.F, NoteValue.FSharp, NoteValue.GFlat);
      TestNext(NoteValue.FSharp, NoteValue.G);
      TestNext(NoteValue.GFlat, NoteValue.G);
      TestNext(NoteValue.G, NoteValue.GSharp, NoteValue.AFlat);
      TestNext(NoteValue.GSharp, NoteValue.A);
      TestNext(NoteValue.AFlat, NoteValue.A);
      TestNext(NoteValue.A, NoteValue.ASharp, NoteValue.BFlat);
      TestNext(NoteValue.ASharp, NoteValue.B);
      TestNext(NoteValue.BFlat, NoteValue.B);
      TestNext(NoteValue.B, NoteValue.C);
    }

    [Fact]
    public void AddTest()
    {
      TestAdd(NoteValue.C, 1, NoteValue.CSharp, NoteValue.DFlat);
      TestAdd(NoteValue.C, 2, NoteValue.D);
      TestAdd(NoteValue.C, 3, NoteValue.DSharp, NoteValue.EFlat);
      TestAdd(NoteValue.C, 4, NoteValue.E);
      TestAdd(NoteValue.C, 5, NoteValue.F);
      TestAdd(NoteValue.C, 6, NoteValue.FSharp, NoteValue.GFlat);
      TestAdd(NoteValue.C, 7, NoteValue.G);
      TestAdd(NoteValue.C, 8, NoteValue.GSharp, NoteValue.AFlat);
      TestAdd(NoteValue.C, 9, NoteValue.A);
      TestAdd(NoteValue.C, 10, NoteValue.ASharp, NoteValue.BFlat);
      TestAdd(NoteValue.C, 11, NoteValue.B);
      TestAdd(NoteValue.C, 12, NoteValue.C);
    }

    [Fact]
    public void PreviousTest()
    {
      TestPrevious(NoteValue.C, NoteValue.B);
      TestPrevious(NoteValue.CSharp, NoteValue.C);
      TestPrevious(NoteValue.DFlat, NoteValue.C);
      TestPrevious(NoteValue.D, NoteValue.CSharp, NoteValue.DFlat);
      TestPrevious(NoteValue.DSharp, NoteValue.D);
      TestPrevious(NoteValue.EFlat, NoteValue.D);
      TestPrevious(NoteValue.E, NoteValue.DSharp, NoteValue.EFlat);
      TestPrevious(NoteValue.F, NoteValue.E);
      TestPrevious(NoteValue.FSharp, NoteValue.F);
      TestPrevious(NoteValue.GFlat, NoteValue.F);
      TestPrevious(NoteValue.G, NoteValue.FSharp, NoteValue.GFlat);
      TestPrevious(NoteValue.GSharp, NoteValue.G);
      TestPrevious(NoteValue.AFlat, NoteValue.G);
      TestPrevious(NoteValue.A, NoteValue.GSharp, NoteValue.AFlat);
      TestPrevious(NoteValue.ASharp, NoteValue.A);
      TestPrevious(NoteValue.BFlat, NoteValue.A);
      TestPrevious(NoteValue.B, NoteValue.ASharp, NoteValue.BFlat);
    }

    [Fact]
    public void SubtractTest()
    {
      TestSubtract(NoteValue.B, 1, NoteValue.BFlat, NoteValue.ASharp);
      TestSubtract(NoteValue.B, 2, NoteValue.A);
      TestSubtract(NoteValue.B, 3, NoteValue.GSharp, NoteValue.AFlat);
      TestSubtract(NoteValue.B, 4, NoteValue.G);
      TestSubtract(NoteValue.B, 5, NoteValue.FSharp, NoteValue.GFlat);
      TestSubtract(NoteValue.B, 6, NoteValue.F);
      TestSubtract(NoteValue.B, 7, NoteValue.E);
      TestSubtract(NoteValue.B, 8, NoteValue.DSharp, NoteValue.EFlat);
      TestSubtract(NoteValue.B, 9, NoteValue.D);
      TestSubtract(NoteValue.B, 10, NoteValue.CSharp, NoteValue.DFlat);
      TestSubtract(NoteValue.B, 11, NoteValue.C);
      TestSubtract(NoteValue.B, 12, NoteValue.B);
    }

    [Fact]
    public void CompareToTest()
    {
      Assert.True(NoteValue.C.CompareTo(NoteValue.C) == 0);  
      Assert.True(NoteValue.C.CompareTo(NoteValue.D) < 0);  
      Assert.True(NoteValue.D.CompareTo(NoteValue.C) > 0);  
      Assert.True(NoteValue.C.CompareTo(NoteValue.B) < 0);
      Assert.True(NoteValue.B.CompareTo(NoteValue.C) > 0);
    }

    [Fact]
    public void TryParseTest()
    {
      TestTryParse("C", NoteValue.C);
      TestTryParse("C#", NoteValue.CSharp);
      TestTryParse("C##", NoteValue.D);
      TestTryParse("Cb", NoteValue.B);
      TestTryParse("Cbb", NoteValue.BFlat);
      TestTryParse("B#", NoteValue.C);
      TestTryParse("B##", NoteValue.CSharp);
      TestTryParse("Bb", NoteValue.BFlat);
      TestTryParse("Bbb", NoteValue.A);
    }

    [Fact]
    public void TryParseRejectsInvalidStrings()
    {
      NoteValue note;
      Assert.False(NoteValue.TryParse(null, out note));
      Assert.False(NoteValue.TryParse("", out note));
      Assert.False(NoteValue.TryParse("J", out note));
      Assert.False(NoteValue.TryParse("C$", out note));
    }

    #endregion

    #region Implementation

    private static void TestTryParse(string value, NoteValue expected)
    {
      NoteValue actual;
      Assert.True(NoteValue.TryParse(value, out actual));
      Assert.Equal(expected, actual);
    }

    private static void TestNext(NoteValue note, NoteValue expectedSharp, NoteValue expectedFlat = null)
    {
      TestAdd(note, 1, expectedSharp, expectedFlat);
    }
    private static void TestPrevious(NoteValue note, NoteValue expectedSharp, NoteValue expectedFlat = null)
    {
      TestSubtract(note, 1, expectedSharp, expectedFlat);
    }
    private static void TestAdd(NoteValue note, int interval, NoteValue expectedSharp, NoteValue expectedFlat = null)
    {
      Assert.Equal(expectedSharp, note.Add(interval, AccidentalMode.FavorSharps));
      Assert.Equal(expectedFlat ?? expectedSharp, note.Add(interval, AccidentalMode.FavorFlats));
    }

    private static void TestSubtract(NoteValue note, int interval, NoteValue expectedSharp, NoteValue expectedFlat = null)
    {
      Assert.Equal(expectedSharp, note.Subtract(interval, AccidentalMode.FavorSharps));
      Assert.Equal(expectedFlat ?? expectedSharp, note.Subtract(interval, AccidentalMode.FavorFlats));
    }

    #endregion
  }
}
