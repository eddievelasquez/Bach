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
  using System.Collections;
  using System.Linq;
  using Xunit;

  public class ScaleTest
  {
    #region Public Methods

    [Fact]
    public void ConstructorTest()
    {
      var actual = new Scale(Note.C, ScaleFormula.Major);
      Assert.Equal("C Major", actual.Name);
      Assert.Equal(Note.C, actual.Root);
      Assert.Equal(ScaleFormula.Major, actual.Formula);
    }

    [Fact]
    public void GetEnumeratorTest()
    {
      var scale = new Scale(Note.C, ScaleFormula.Major);
      var enumerator = ((IEnumerable) scale).GetEnumerator();
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
      TestScale("C,D,E,F,G,A,B", root, ScaleFormula.Major);
      TestScale("C,D,Eb,F,G,Ab,Bb", root, ScaleFormula.NaturalMinor);
      TestScale("C,D,Eb,F,G,Ab,B", root, ScaleFormula.HarmonicMinor);
      TestScale("C,D,Eb,F,G,A,B", root, ScaleFormula.MelodicMinor);
      TestScale("C,D,Eb,F,Gb,G#,A,B", root, ScaleFormula.Diminished);
      TestScale("C,Db,Eb,E,F#,G,A,Bb", root, ScaleFormula.Polytonal);
      TestScale("C,D,E,F#,G#,A#", root, ScaleFormula.WholeTone);
      TestScale("C,D,E,G,A", root, ScaleFormula.Pentatonic);
      TestScale("C,Eb,F,G,Bb", root, ScaleFormula.MinorPentatonic);
      TestScale("C,Eb,F,Gb,G,Bb", root, ScaleFormula.Blues);
      TestScale("C,D,Eb,E,G,A", root, ScaleFormula.Gospel);
    }

    [Fact]
    public void EqualsContractTest()
    {
      object x = new Scale(Note.C, ScaleFormula.Major);
      object y = new Scale(Note.C, ScaleFormula.Major);
      object z = new Scale(Note.C, ScaleFormula.Major);

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
      var x = new Scale(Note.C, ScaleFormula.Major);
      var y = new Scale(Note.C, ScaleFormula.Major);
      var z = new Scale(Note.C, ScaleFormula.Major);

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
      object actual = new Scale(Note.C, ScaleFormula.Major);
      Assert.False(actual.Equals(int.MinValue));
    }

    [Fact]
    public void TypeSafeEqualsFailsWithDifferentTypeTest()
    {
      var actual = new Scale(Note.C, ScaleFormula.Major);
      Assert.False(actual.Equals(int.MinValue));
    }

    [Fact]
    public void EqualsFailsWithNullTest()
    {
      object actual = new Scale(Note.C, ScaleFormula.Major);
      Assert.False(actual.Equals(null));
    }

    [Fact]
    public void TypeSafeEqualsFailsWithNullTest()
    {
      var actual = new Scale(Note.C, ScaleFormula.Major);
      Assert.False(actual.Equals(null));
    }

    [Fact]
    public void EqualsSucceedsWithSameObjectTest()
    {
      var actual = new Scale(Note.C, ScaleFormula.Major);
      Assert.True(actual.Equals(actual));
    }

    [Fact]
    public void GetHashcodeTest()
    {
      var actual = new Scale(Note.C, ScaleFormula.Major);
      var expected = new Scale(Note.C, ScaleFormula.Major);
      Assert.True(expected.Equals(actual));
      Assert.Equal(expected.GetHashCode(), actual.GetHashCode());
    }

    #endregion

    #region Implementation

    private static void TestScale(string expectedNotes, Note root, ScaleFormula formula)
    {
      NoteCollection expected = NoteCollection.Parse(expectedNotes);
      var scale = new Scale(root, formula);
      var actual = scale.Take(expected.Count).ToArray();
      Assert.Equal(expected, actual);
      Assert.Equal(expectedNotes, scale.ToString());
    }

    #endregion
  }
}
