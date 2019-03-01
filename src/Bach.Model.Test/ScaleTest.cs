﻿//
// Module Name: ScaleTest.cs
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
  using System.Collections;
  using System.Linq;
  using Xunit;

  public class ScaleTest
  {
    #region Public Methods

    [Fact]
    public void StringConstructorTest()
    {
      var actual = new Scale(Note.C, "Major");
      Assert.Equal("C", actual.Name);
      Assert.Equal(Note.C, actual.Root);
      Assert.Equal(Registry.ScaleFormulas["Major"], actual.Formula);
    }

    [Fact]
    public void StringConstructorThrowsOnNullFormulaNameTest()
    {
      Assert.Throws<ArgumentNullException>(() => new Scale(Note.C, (string) null));
    }

    [Fact]
    public void StringConstructorThrowsOnEmptyFormulaNameTest()
    {
      Assert.Throws<System.Collections.Generic.KeyNotFoundException>(() => new Scale(Note.C, ""));
    }

    [Fact]
    public void FormulaConstructorTest()
    {
      ScaleFormula formula = Registry.ScaleFormulas["Major"];
      var actual = new Scale(Note.C, formula);
      Assert.Equal("C", actual.Name);
      Assert.Equal(Note.C, actual.Root);
      Assert.Equal(Registry.ScaleFormulas["Major"], actual.Formula);
    }

    [Fact]
    public void FormulaConstructorThrowsOnNullFormulaTest()
    {
      Assert.Throws<ArgumentNullException>(() => new Scale(Note.C, (ScaleFormula) null));
    }

    [Fact]
    public void GetEnumeratorTest()
    {
      var scale = new Scale(Note.C, "Major");
      IEnumerator enumerator = ((IEnumerable) scale).GetEnumerator();
      Assert.True(enumerator.MoveNext());
      Assert.Equal(Note.C, enumerator.Current);
      Assert.True(enumerator.MoveNext());
      Assert.Equal(Note.D, enumerator.Current);
      Assert.True(enumerator.MoveNext());
      Assert.Equal(Note.E, enumerator.Current);
      Assert.True(enumerator.MoveNext());
      Assert.Equal(Note.F, enumerator.Current);
      Assert.True(enumerator.MoveNext());
      Assert.Equal(Note.G, enumerator.Current);
      Assert.True(enumerator.MoveNext());
      Assert.Equal(Note.A, enumerator.Current);
      Assert.True(enumerator.MoveNext());
      Assert.Equal(Note.B, enumerator.Current);
      Assert.True(enumerator.MoveNext()); // Scale enumerator wraps around infintely
      Assert.Equal(Note.C, enumerator.Current);
    }

    [Fact]
    public void GenerateScaleTest()
    {
      Note root = Note.C;
      TestScale("C,D,E,F,G,A,B", root, "Major");
      TestScale("C,D,Eb,F,G,Ab,Bb", root, "NaturalMinor");
      TestScale("C,D,Eb,F,G,Ab,B", root, "HarmonicMinor");
      TestScale("C,D,Eb,F,G,A,B", root, "MelodicMinor");
      TestScale("C,D,Eb,F,Gb,G#,A,B", root, "Diminished");
      TestScale("C,Db,Eb,E,F#,G,A,Bb", root, "Polytonal");
      TestScale("C,D,E,F#,G#,A#", root, "WholeTone");
      TestScale("C,D,E,G,A", root, "Pentatonic");
      TestScale("C,Eb,F,G,Bb", root, "MinorPentatonic");
      TestScale("C,Eb,F,Gb,G,Bb", root, "Blues");
      TestScale("C,D,Eb,E,G,A", root, "Gospel");
    }

    [Fact]
    public void EqualsContractTest()
    {
      object x = new Scale(Note.C, "Major");
      object y = new Scale(Note.C, "Major");
      object z = new Scale(Note.C, "Major");

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
      var x = new Scale(Note.C, "Major");
      var y = new Scale(Note.C, "Major");
      var z = new Scale(Note.C, "Major");

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
      object actual = new Scale(Note.C, "Major");
      Assert.False(actual.Equals(int.MinValue));
    }

    [Fact]
    public void TypeSafeEqualsFailsWithDifferentTypeTest()
    {
      var actual = new Scale(Note.C, "Major");
      Assert.False(actual.Equals(int.MinValue));
    }

    [Fact]
    public void EqualsFailsWithNullTest()
    {
      object actual = new Scale(Note.C, "Major");
      Assert.False(actual.Equals(null));
    }

    [Fact]
    public void TypeSafeEqualsFailsWithNullTest()
    {
      var actual = new Scale(Note.C, "Major");
      Assert.False(actual.Equals(null));
    }

    [Fact]
    public void EqualsSucceedsWithSameObjectTest()
    {
      var actual = new Scale(Note.C, "Major");
      Assert.True(actual.Equals(actual));
    }

    [Fact]
    public void GetHashcodeTest()
    {
      var actual = new Scale(Note.C, "Major");
      var expected = new Scale(Note.C, "Major");
      Assert.True(expected.Equals(actual));
      Assert.Equal(expected.GetHashCode(), actual.GetHashCode());
    }

    [Fact]
    public void GetNotesTest()
    {
      var scale = new Scale(Note.C, "MinorPentatonic");
      Assert.Equal(new[] { Note.C, Note.EFlat, Note.F, Note.G, Note.BFlat }, scale.GetNotes());
      Assert.Equal(new[] { Note.EFlat, Note.F, Note.G, Note.BFlat, Note.C }, scale.GetNotes(1));
      Assert.Equal(new[] { Note.F, Note.G, Note.BFlat, Note.C, Note.EFlat }, scale.GetNotes(2));
      Assert.Equal(new[] { Note.G, Note.BFlat, Note.C, Note.EFlat, Note.F }, scale.GetNotes(3));
      Assert.Equal(new[] { Note.BFlat, Note.C, Note.EFlat, Note.F, Note.G }, scale.GetNotes(4));
      Assert.Equal(new[] { Note.C, Note.EFlat, Note.F, Note.G, Note.BFlat }, scale.GetNotes(5));
    }

    [Fact]
    public void RenderTest()
    {
      var scale = new Scale(Note.C, "MinorPentatonic");
      TestRender(scale, "C4", "C4,Eb4,F4,G4,Bb4");
      TestRender(scale, "Eb4", "Eb4,F4,G4,Bb4,C5");
      TestRender(scale, "F4", "F4,G4,Bb4,C5,Eb5");
      TestRender(scale, "G4", "G4,Bb4,C5,Eb5,F5");
      TestRender(scale, "Bb4", "Bb4,C5,Eb5,F5,G5");
    }

    [Fact]
    public void RenderReturnsEmptyIfNotNoteInScaleTest()
    {
      var scale = new Scale(Note.C, "MinorPentatonic");
      Assert.Empty(scale.Render(Pitch.Parse("D4")));
    }

    [Fact]
    public void NoteCountTest()
    {
      var scale = new Scale(Note.C, "MinorPentatonic");
      Assert.Equal(Registry.ScaleFormulas["MinorPentatonic"].IntervalCount, scale.ToneCount);
    }

    [Fact]
    public void IndexOfTest()
    {
      var scale = new Scale(Note.C, "MinorPentatonic");
      Assert.Equal(0, scale.IndexOf(Note.C));
      Assert.Equal(1, scale.IndexOf(Note.EFlat));
      Assert.Equal(2, scale.IndexOf(Note.F));
      Assert.Equal(3, scale.IndexOf(Note.G));
      Assert.Equal(4, scale.IndexOf(Note.BFlat));
    }

    [Fact]
    public void ToStringTest()
    {
      var scale = new Scale(Note.C, "MinorPentatonic");
      Assert.Equal("C,Eb,F,G,Bb", scale.ToString());
    }

    [Fact]
    public void IndexerTest()
    {
      var scale = new Scale(Note.C, "MinorPentatonic");
      Assert.Equal(Note.C, scale[0]);
      Assert.Equal(Note.EFlat, scale[1]);
      Assert.Equal(Note.F, scale[2]);
      Assert.Equal(Note.G, scale[3]);
      Assert.Equal(Note.BFlat, scale[4]);
    }

    #endregion

    #region Implementation

    private static void TestRender(Scale scale,
                                   string startNote,
                                   string expectedNotes)
    {
      Pitch[] actual = scale.Render(Pitch.Parse(startNote)).Take(scale.Formula.IntervalCount).ToArray();
      Assert.Equal(PitchCollection.Parse(expectedNotes), actual);
    }

    private static void TestScale(string expectedNotes,
                                  Note root,
                                  string formulaName)
    {
      NoteCollection expected = NoteCollection.Parse(expectedNotes);
      var scale = new Scale(root, formulaName);
      Note[] actual = scale.Take(expected.Count).ToArray();
      Assert.Equal(expected, actual);
    }

    #endregion
  }
}
