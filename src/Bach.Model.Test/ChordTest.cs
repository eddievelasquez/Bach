//  
// Module Name: ChordTest.cs
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
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;
  using Xunit;

  public class ChordTest
  {
    #region Public Methods

    [Fact]
    public void ChordConstructorTest()
    {
      AbsoluteNote root = AbsoluteNote.Parse("C4");
      ChordFormula formula = ChordFormula.Minor;
      var target = new Chord(root, formula);

      Assert.Equal(root, target.Root);
      Assert.Equal(formula, target.Formula);
      Assert.Equal("Cm", target.Name);
      Assert.Equal(target.Notes, AbsoluteNoteCollection.Parse("C,Eb,G"));
    }

    [Fact]
    public void ChordsTest()
    {
      AbsoluteNote root = AbsoluteNote.Parse("C4");
      ChordTestImpl("C4,E4,G4", root, ChordFormula.Major);
      ChordTestImpl("C4,E4,G4,B4", root, ChordFormula.Major7);
      ChordTestImpl("C4,E4,G4,B4,D5", root, ChordFormula.Major9);
      ChordTestImpl("C4,E4,G4,B4,D5,F5", root, ChordFormula.Major11);
      ChordTestImpl("C4,E4,G4,B4,D5,F5,A5", root, ChordFormula.Major13);
      ChordTestImpl("C4,Eb4,G4", root, ChordFormula.Minor);
      ChordTestImpl("C4,Eb4,G4,Bb4", root, ChordFormula.Minor7);
      ChordTestImpl("C4,Eb4,G4,Bb4,D5", root, ChordFormula.Minor9);
      ChordTestImpl("C4,Eb4,G4,Bb4,D5,F5", root, ChordFormula.Minor11);
      ChordTestImpl("C4,Eb4,G4,Bb4,D5,F5,A5", root, ChordFormula.Minor13);
      ChordTestImpl("C4,E4,G4,Bb4", root, ChordFormula.Dominant7);
      ChordTestImpl("C4,E4,G4,Bb4,D5", root, ChordFormula.Dominant9);
      ChordTestImpl("C4,E4,G4,Bb4,D5,F5", root, ChordFormula.Dominant11);
      ChordTestImpl("C4,E4,G4,Bb4,D5,F5,A5", root, ChordFormula.Dominant13);
      ChordTestImpl("C4,E4,G4,A4,D5", root, ChordFormula.SixNine);
      ChordTestImpl("C4,E4,G4,D5", root, ChordFormula.AddNine);
      ChordTestImpl("C4,Eb4,Gb4", root, ChordFormula.Diminished);
      ChordTestImpl("C4,Eb4,Gb4,A4", root, ChordFormula.Diminished7);
      ChordTestImpl("C4,Eb4,Gb4,Bb4", root, ChordFormula.HalfDiminished);
      ChordTestImpl("C4,E4,G#4", root, ChordFormula.Augmented);
    }

    [Fact]
    public void EqualsContractTest()
    {
      object x = new Chord(AbsoluteNote.Parse("C4"), ChordFormula.Major);
      object y = new Chord(AbsoluteNote.Parse("C4"), ChordFormula.Major);
      object z = new Chord(AbsoluteNote.Parse("C4"), ChordFormula.Major);

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
      var x = new Chord(AbsoluteNote.Parse("C4"), ChordFormula.Major);
      var y = new Chord(AbsoluteNote.Parse("C4"), ChordFormula.Major);
      var z = new Chord(AbsoluteNote.Parse("C4"), ChordFormula.Major);

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
      object actual = new Chord(AbsoluteNote.Parse("C4"), ChordFormula.Major);
      Assert.False(actual.Equals(int.MinValue));
    }

    [Fact]
    public void TypeSafeEqualsFailsWithDifferentTypeTest()
    {
      var actual = new Chord(AbsoluteNote.Parse("C4"), ChordFormula.Major);
      Assert.False(actual.Equals(int.MinValue));
    }

    [Fact]
    public void EqualsFailsWithNullTest()
    {
      object actual = new Chord(AbsoluteNote.Parse("C4"), ChordFormula.Major);
      Assert.False(actual.Equals(null));
    }

    [Fact]
    public void TypeSafeEqualsFailsWithNullTest()
    {
      var actual = new Chord(AbsoluteNote.Parse("C4"), ChordFormula.Major);
      Assert.False(actual.Equals(null));
    }

    [Fact]
    public void EqualsSucceedsWithSameObjectTest()
    {
      var actual = new Chord(AbsoluteNote.Parse("C4"), ChordFormula.Major);
      Assert.True(actual.Equals(actual));
    }

    [Fact]
    public void GetHashcodeTest()
    {
      var actual = new Chord(AbsoluteNote.Parse("C4"), ChordFormula.Major);
      var expected = new Chord(AbsoluteNote.Parse("C4"), ChordFormula.Major);
      Assert.True(expected.Equals(actual));
      Assert.Equal(expected.GetHashCode(), actual.GetHashCode());
    }

    [Fact]
    public void InvertTest()
    {
      var c4 = new Chord(AbsoluteNote.Parse("C4"), ChordFormula.Major);
      AbsoluteNoteCollection firstInversion = AbsoluteNoteCollection.Parse("E4,G4,C5");
      Chord actual = c4.Invert(1);
      Assert.NotNull(actual);
      Assert.Equal(firstInversion, actual.Notes);

      AbsoluteNoteCollection secondInversion = AbsoluteNoteCollection.Parse("G4,C5,E5");
      actual = c4.Invert(2);
      Assert.NotNull(actual);
      Assert.Equal(secondInversion, actual.Notes);

      AbsoluteNoteCollection thirdInversion = AbsoluteNoteCollection.Parse("C5,E5,G5");
      actual = c4.Invert(3);
      Assert.NotNull(actual);
      Assert.Equal(thirdInversion, actual.Notes);
    }


    [Fact]
    public void EnumeratorTest()
    {
      var c4 = new Chord(AbsoluteNote.Parse("C4"), ChordFormula.Major);
      IEnumerator enumerator = ((IEnumerable) c4).GetEnumerator();
      Assert.NotNull(enumerator);
      Assert.True(enumerator.MoveNext());
      Assert.Equal(AbsoluteNote.Parse("C4"), enumerator.Current);
      Assert.True(enumerator.MoveNext());
      Assert.Equal(AbsoluteNote.Parse("E4"), enumerator.Current);
      Assert.True(enumerator.MoveNext());
      Assert.Equal(AbsoluteNote.Parse("G4"), enumerator.Current);
      Assert.False(enumerator.MoveNext());
    }

    #endregion

    #region Implementation

    private static void ChordTestImpl(string expectedNotes, AbsoluteNote root, ChordFormula formula)
    {
      AbsoluteNoteCollection expected = AbsoluteNoteCollection.Parse(expectedNotes);
      var chord = new Chord(root, formula);
      AbsoluteNote[] actualNotes = chord.Take(expected.Count).ToArray();
      Assert.Equal(expected, actualNotes);
      Assert.Equal(expectedNotes, chord.ToString());
    }

    #endregion
  }
}
