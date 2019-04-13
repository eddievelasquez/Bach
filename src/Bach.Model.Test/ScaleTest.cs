// Module Name: ScaleTest.cs
// Project:     Bach.Model.Test
// Copyright (c) 2012, 2019  Eddie Velasquez.
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
// portions of the Software.
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
  using System.Collections;
  using System.Collections.Generic;
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
      Assert.Throws<ArgumentNullException>(() => new Scale(Note.C, (string)null));
    }

    [Fact]
    public void StringConstructorThrowsOnEmptyFormulaNameTest()
    {
      Assert.Throws<KeyNotFoundException>(() => new Scale(Note.C, ""));
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
      Assert.Throws<ArgumentNullException>(() => new Scale(Note.C, (ScaleFormula)null));
    }

    [Fact]
    public void GetAscendingEnumeratorTest()
    {
      var scale = new Scale(Note.C, "Major");
      IEnumerator enumerator = scale.Ascending.GetEnumerator();
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
      Assert.True(enumerator.MoveNext()); // Scale enumerator wraps around infinitely
      Assert.Equal(Note.C, enumerator.Current);
    }

    [Fact]
    public void GetDescendingEnumeratorTest()
    {
      var scale = new Scale(Note.C, "Major");
      IEnumerator enumerator = scale.Descending.GetEnumerator();
      Assert.True(enumerator.MoveNext());
      Assert.Equal(Note.C, enumerator.Current);
      Assert.True(enumerator.MoveNext());
      Assert.Equal(Note.B, enumerator.Current);
      Assert.True(enumerator.MoveNext());
      Assert.Equal(Note.A, enumerator.Current);
      Assert.True(enumerator.MoveNext());
      Assert.Equal(Note.G, enumerator.Current);
      Assert.True(enumerator.MoveNext());
      Assert.Equal(Note.F, enumerator.Current);
      Assert.True(enumerator.MoveNext());
      Assert.Equal(Note.E, enumerator.Current);
      Assert.True(enumerator.MoveNext());
      Assert.Equal(Note.D, enumerator.Current);
      Assert.True(enumerator.MoveNext()); // Scale enumerator wraps around infinitely
      Assert.Equal(Note.C, enumerator.Current);
    }

    [Fact]
    public void ScaleAscendingTest()
    {
      Note root = Note.C;
      TestScaleAscending("C,D,E,F,G,A,B", root, "Major");
      TestScaleAscending("C,D,Eb,F,G,Ab,Bb", root, "NaturalMinor");
      TestScaleAscending("C,D,Eb,F,G,Ab,B", root, "HarmonicMinor");
      TestScaleAscending("C,D,Eb,F,G,A,B", root, "MelodicMinor");
      TestScaleAscending("C,D,Eb,F,Gb,G#,A,B", root, "Diminished");
      TestScaleAscending("C,Db,Eb,E,F#,G,A,Bb", root, "Polytonal");
      TestScaleAscending("C,D,E,F#,G#,A#", root, "WholeTone");
      TestScaleAscending("C,D,E,G,A", root, "Pentatonic");
      TestScaleAscending("C,Eb,F,G,Bb", root, "MinorPentatonic");
      TestScaleAscending("C,Eb,F,Gb,G,Bb", root, "MinorBlues");
      TestScaleAscending("C,D,Eb,E,G,A", root, "MajorBlues");
    }

    [Fact]
    public void ScaleDescendingTest()
    {
      Note root = Note.C;
      TestScaleDescending("C,B,A,G,F,E,D", root, "Major");
      TestScaleDescending("C,Bb,Ab,G,F,Eb,D", root, "NaturalMinor");
      TestScaleDescending("C,B,Ab,G,F,Eb,D", root, "HarmonicMinor");
      TestScaleDescending("C,B,A,G,F,Eb,D", root, "MelodicMinor");
      TestScaleDescending("C,B,A,G#,Gb,F,Eb,D", root, "Diminished");
      TestScaleDescending("C,Bb,A,G,F#,E,Eb,Db", root, "Polytonal");
      TestScaleDescending("C,A#,G#,F#,E,D", root, "WholeTone");
      TestScaleDescending("C,A,G,E,D", root, "Pentatonic");
      TestScaleDescending("C,Bb,G,F,Eb", root, "MinorPentatonic");
      TestScaleDescending("C,Bb,G,Gb,F,Eb", root, "MinorBlues");
      TestScaleDescending("C,A,G,E,Eb,D", root, "MajorBlues");
    }

    [Fact]
    public void IsDiatonicTest()
    {
      TestPredicate(Note.C, "Major", scale => scale.Formula.Categories.Contains("Diatonic"));
      TestPredicate(Note.C, "LeadingTone", scale => scale.Formula.Categories.Contains("Diatonic"));
      TestPredicate(Note.C, "LydianDominant", scale => scale.Formula.Categories.Contains("Diatonic"));
      TestPredicate(Note.C, "Hindu", scale => scale.Formula.Categories.Contains("Diatonic"));
      TestPredicate(Note.C, "Arabian", scale => scale.Formula.Categories.Contains("Diatonic"));
      TestPredicate(Note.C, "NaturalMinor", scale => scale.Formula.Categories.Contains("Diatonic"));
      TestPredicate(Note.C, "Javanese", scale => scale.Formula.Categories.Contains("Diatonic"));
      TestPredicate(Note.C, "NeapolitanMajor", scale => scale.Formula.Categories.Contains("Diatonic"));

      TestPredicate(Note.C, "HungarianFolk", scale => !scale.Formula.Categories.Contains("Diatonic"));
      TestPredicate(Note.C, "Gypsy", scale => !scale.Formula.Categories.Contains("Diatonic"));
      TestPredicate(Note.C, "EnigmaticMajor", scale => !scale.Formula.Categories.Contains("Diatonic"));
      TestPredicate(Note.C, "Persian", scale => !scale.Formula.Categories.Contains("Diatonic"));
      TestPredicate(Note.C, "Mongolian", scale => !scale.Formula.Categories.Contains("Diatonic"));
    }

    [Fact]
    public void IsMajorTest()
    {
      TestPredicate(Note.C, "Major", scale => scale.Formula.Categories.Contains("Major"));
      TestPredicate(Note.C, "HungarianFolk", scale => scale.Formula.Categories.Contains("Major"));
      TestPredicate(Note.C, "Gypsy", scale => scale.Formula.Categories.Contains("Major"));
      TestPredicate(Note.C, "Mongolian", scale => scale.Formula.Categories.Contains("Major"));
    }

    [Fact]
    public void IsMinorTest()
    {
      TestPredicate(Note.C, "NaturalMinor", scale => scale.Formula.Categories.Contains("Minor"));
      TestPredicate(Note.C, "GypsyMinor", scale => scale.Formula.Categories.Contains("Minor"));
      TestPredicate(Note.C, "Javanese", scale => scale.Formula.Categories.Contains("Minor"));
      TestPredicate(Note.C, "NeapolitanMinor", scale => scale.Formula.Categories.Contains("Minor"));
      TestPredicate(Note.C, "NeapolitanMajor", scale => scale.Formula.Categories.Contains("Minor"));
      TestPredicate(Note.C, "HungarianGypsy", scale => scale.Formula.Categories.Contains("Minor"));
      TestPredicate(Note.C, "Yo", scale => scale.Formula.Categories.Contains("Minor"));
      TestPredicate(Note.C, "Hirajoshi", scale => scale.Formula.Categories.Contains("Minor"));
      TestPredicate(Note.C, "Balinese", scale => scale.Formula.Categories.Contains("Minor"));
    }

    [Fact]
    public void EqualsContractTest()
    {
      object x = new Scale(Note.C, "Major");
      object y = new Scale(Note.C, "Major");
      object z = new Scale(Note.C, "Major");

      Assert.True(x.Equals(x)); // Reflexive
      Assert.True(x.Equals(y)); // Symmetric
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
      Assert.True(x.Equals(y)); // Symmetric
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
    public void RenderTest()
    {
      TestRender(new Scale(Note.C, "MinorPentatonic"), 1, "C1,Eb1,F1,G1,Bb1");
      TestRender(new Scale(Note.D, "MinorPentatonic"), 1, "D1,F1,G1,A1,C2");
      TestRender(new Scale(Note.B, "MinorPentatonic"), 1, "B1,D2,E2,F#2,A2");
    }

    [Fact]
    public void RenderUsesChoosesAppropriateAccidentalForMajorScaleTest()
    {
      TestScaleAscending("C,D,E,F,G,A,B", Note.C, "Major");
      TestScaleAscending("C#,D#,E#,F#,G#,A#,B#", Note.CSharp, "Major");
      TestScaleAscending("Db,Eb,F,Gb,Ab,Bb,C", Note.DFlat, "Major");
      TestScaleAscending("D,E,F#,G,A,B,C#", Note.D, "Major");
      TestScaleAscending("D#,E#,F##,G#,A#,B#,C##", Note.DSharp, "Major");
      TestScaleAscending("Eb,F,G,Ab,Bb,C,D", Note.EFlat, "Major");
      TestScaleAscending("E,F#,G#,A,B,C#,D#", Note.E, "Major");
      TestScaleAscending("E#,F##,G##,A#,B#,C##,D##", Note.Parse("E#"), "Major");
      TestScaleAscending("Fb,Gb,Ab,Bbb,Cb,Db,Eb", Note.Parse("Fb"), "Major");
      TestScaleAscending("F,G,A,Bb,C,D,E", Note.F, "Major");
      TestScaleAscending("F#,G#,A#,B,C#,D#,E#", Note.FSharp, "Major");
      TestScaleAscending("Gb,Ab,Bb,Cb,Db,Eb,F", Note.GFlat, "Major");
      TestScaleAscending("G,A,B,C,D,E,F#", Note.G, "Major");
      TestScaleAscending("G#,A#,B#,C#,D#,E#,F##", Note.GSharp, "Major");
      TestScaleAscending("Ab,Bb,C,Db,Eb,F,G", Note.AFlat, "Major");
      TestScaleAscending("A,B,C#,D,E,F#,G#", Note.A, "Major");
      TestScaleAscending("A#,B#,C##,D#,E#,F##,G##", Note.ASharp, "Major");
      TestScaleAscending("Bb,C,D,Eb,F,G,A", Note.BFlat, "Major");
      TestScaleAscending("B,C#,D#,E,F#,G#,A#", Note.B, "Major");
      TestScaleAscending("B#,C##,D##,E#,F##,G##,A##", Note.Parse("B#"), "Major");
      TestScaleAscending("Cb,Db,Eb,Fb,Gb,Ab,Bb", Note.Parse("Cb"), "Major");
    }

    [Fact]
    public void RenderUsesChoosesAppropriateAccidentalForNaturalMinorScaleTest()
    {
      TestScaleAscending("C,D,Eb,F,G,Ab,Bb", Note.C, "NaturalMinor");
      TestScaleAscending("C#,D#,E,F#,G#,A,B", Note.CSharp, "NaturalMinor");
      TestScaleAscending("Db,Eb,Fb,Gb,Ab,Bbb,Cb", Note.DFlat, "NaturalMinor");
      TestScaleAscending("D,E,F,G,A,Bb,C", Note.D, "NaturalMinor");
      TestScaleAscending("D#,E#,F#,G#,A#,B,C#", Note.DSharp, "NaturalMinor");
      TestScaleAscending("EB,F,Gb,Ab,Bb,Cb,Db", Note.EFlat, "NaturalMinor");
      TestScaleAscending("E,F#,G,A,B,C,D", Note.E, "NaturalMinor");
      TestScaleAscending("E#,F##,G#,A#,B#,C#,D#", Note.Parse("E#"), "NaturalMinor");
      TestScaleAscending("Fb,Gb,Abb,Bbb,Cb,Dbb,Ebb", Note.Parse("Fb"), "NaturalMinor");
      TestScaleAscending("F,G,Ab,Bb,C,Db,Eb", Note.F, "NaturalMinor");
      TestScaleAscending("F#,G#,A,B,C#,D,E", Note.FSharp, "NaturalMinor");
      TestScaleAscending("Gb,Ab,Bbb,Cb,Db,Ebb,Fb", Note.GFlat, "NaturalMinor");
      TestScaleAscending("G,A,Bb,C,D,Eb,F", Note.G, "NaturalMinor");
      TestScaleAscending("G#,A#,B,C#,D#,E,F#", Note.GSharp, "NaturalMinor");
      TestScaleAscending("Ab,Bb,Cb,Db,Eb,Fb,Gb", Note.AFlat, "NaturalMinor");
      TestScaleAscending("A,B,C,D,E,F,G", Note.A, "NaturalMinor");
      TestScaleAscending("A#,B#,C#,D#,E#,F#,G#", Note.ASharp, "NaturalMinor");
      TestScaleAscending("Bb,C,Db,Eb,F,Gb,Ab", Note.BFlat, "NaturalMinor");
      TestScaleAscending("B,C#,D,E,F#,G,A", Note.B, "NaturalMinor");
      TestScaleAscending("B#,C##,D#,E#,F##,G#,A#", Note.Parse("B#"), "NaturalMinor");
      TestScaleAscending("Cb,Db,Ebb,Fb,Gb,Abb,Bbb", Note.Parse("Cb"), "NaturalMinor");
    }

    [Fact]
    public void NoteCountTest()
    {
      var scale = new Scale(Note.C, "MinorPentatonic");
      Assert.Equal(Registry.ScaleFormulas["MinorPentatonic"].Intervals.Count, scale.Notes.Count);
    }

    [Fact]
    public void IsTheoreticalTest()
    {
      Assert.False(new Scale(Note.C, "major").Theoretical);
      Assert.True(new Scale(Note.DSharp, "major").Theoretical);
      Assert.True(new Scale(Note.Parse("E#"), "major").Theoretical);
      Assert.True(new Scale(Note.Parse("Fb"), "major").Theoretical);
      Assert.True(new Scale(Note.GSharp, "major").Theoretical);
      Assert.True(new Scale(Note.ASharp, "major").Theoretical);
      Assert.True(new Scale(Note.Parse("B#"), "major").Theoretical);
    }

    [Fact]
    public void EnharmonicScaleTest()
    {
      TestEnharmonic(Note.C, Note.C, "major");
      TestEnharmonic(Note.CSharp, Note.DFlat, "major");
      TestEnharmonic(Note.DSharp, Note.EFlat, "major");
      TestEnharmonic(Note.Parse("E#"), Note.F, "major");
      TestEnharmonic(Note.Parse("Fb"), Note.E, "major");
      TestEnharmonic(Note.GSharp, Note.AFlat, "major");
      TestEnharmonic(Note.ASharp, Note.BFlat, "major");
      TestEnharmonic(Note.Parse("B#"), Note.C, "major");
    }

    [Fact]
    public void ToStringTest()
    {
      var scale = new Scale(Note.C, "MinorPentatonic");
      Assert.Equal("C Minor Pentatonic {C,Eb,F,G,Bb}", scale.ToString());
    }

    [Fact]
    public void NotesTest()
    {
      var scale = new Scale(Note.C, "MinorPentatonic");
      Assert.Equal(Note.C, scale.Notes[0]);
      Assert.Equal(Note.EFlat, scale.Notes[1]);
      Assert.Equal(Note.F, scale.Notes[2]);
      Assert.Equal(Note.G, scale.Notes[3]);
      Assert.Equal(Note.BFlat, scale.Notes[4]);
    }

    [Fact]
    public void ContainsTest()
    {
      var scale = new Scale(Note.C, "major");
      Assert.True(scale.Contains(new[] { Note.C }));
      Assert.True(scale.Contains(new[] { Note.C, Note.E, Note.G }));
      Assert.False(scale.Contains(new[] { Note.C, Note.E, Note.GFlat }));
    }

    [Fact]
    public void ScalesContainingTest()
    {
      IDictionary<string, Scale> scales = Scale.ScalesContaining(new[] { Note.C, Note.E, Note.G }).ToDictionary(scale => scale.Name);
      Assert.Contains("C", scales);
      Assert.Contains("C Pentatonic", scales);
      Assert.Contains("E Natural Minor", scales);
      Assert.Contains("E Harmonic Minor", scales);
      Assert.Contains("G", scales);
      Assert.Contains("G Melodic Minor", scales);
      Assert.Contains("G Diminished", scales);
    }

    #endregion

    #region  Implementation

    private static void TestEnharmonic(Note root,
                                       Note enharmonicRoot,
                                       string scaleKey)
    {
      var scale = new Scale(root, scaleKey);
      Scale actual = scale.GetEnharmonicScale();
      var expected = new Scale(enharmonicRoot, scaleKey);
      Assert.Equal(expected, actual);
    }

    private static void TestRender(Scale scale,
                                   int octave,
                                   string expectedNotes)
    {
      Pitch[] actual = scale.Render(octave).Take(scale.Formula.Intervals.Count).ToArray();
      Assert.Equal(PitchCollection.Parse(expectedNotes), actual);
    }

    private static void TestScaleAscending(string expectedNotes,
                                           Note root,
                                           string formulaName)
    {
      NoteCollection expected = NoteCollection.Parse(expectedNotes);
      var scale = new Scale(root, formulaName);
      IEnumerable<Note> actual = scale.Ascending.Take(expected.Count);
      Assert.Equal(expected, actual);
    }

    private static void TestScaleDescending(string expectedNotes,
                                            Note root,
                                            string formulaName)
    {
      NoteCollection expected = NoteCollection.Parse(expectedNotes);
      var scale = new Scale(root, formulaName);
      IEnumerable<Note> actual = scale.Descending.Take(expected.Count);
      Assert.Equal(expected, actual);
    }

    private static void TestPredicate(Note root,
                                      string formulaName,
                                      Predicate<Scale> pred)
    {
      var scale = new Scale(root, formulaName);
      Assert.True(pred(scale));
    }

    #endregion
  }
}
