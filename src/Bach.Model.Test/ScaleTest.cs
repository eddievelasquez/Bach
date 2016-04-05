//  
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
  using System.Linq;
  using Xunit;

  public class ScaleTest
  {
    #region Public Methods

    [Fact]
    public void ConstructorTest()
    {
      var major = new ScaleFormula("Major", Interval.Perfect1, Interval.Major2, Interval.Major3, Interval.Perfect4,
                                   Interval.Perfect5, Interval.Major6, Interval.Major7);
      Assert.Equal("Major", major.Name);
      Assert.Equal(7, major.Count);
    }

    [Fact]
    public void GenerateScaleTest()
    {
      AbsoluteNote root = AbsoluteNote.Parse("C4");
      TestScale("C4,D4,E4,F4,G4,A4,B4", root, ScaleFormula.Major);
      TestScale("C4,D4,Eb4,F4,G4,Ab4,Bb4", root, ScaleFormula.NaturalMinor);
      TestScale("C4,D4,Eb4,F4,G4,Ab4,B4", root, ScaleFormula.HarmonicMinor);
      TestScale("C4,D4,Eb4,F4,G4,A4,B4", root, ScaleFormula.MelodicMinor);
      TestScale("C4,D4,Eb4,F4,Gb4,G#4,A4,B4", root, ScaleFormula.Diminished);
      TestScale("C4,Db4,Eb4,Fb4,F#4,G4,A4,Bb4", root, ScaleFormula.Polytonal);
      TestScale("C4,D4,E4,F#4,G#4", root, ScaleFormula.WholeTone);
      TestScale("C4,D4,E4,G4,A4", root, ScaleFormula.Pentatonic);
      TestScale("C4,Eb4,F4,G4,Bb4", root, ScaleFormula.MinorPentatonic);
      TestScale("C4,Eb4,F4,Gb4,G4,Bb4", root, ScaleFormula.Blues);
      TestScale("C4,D4,Eb4,E4,G4,A4", root, ScaleFormula.Gospel);
    }

    [Fact]
    public void EqualsContractTest()
    {
      object x = new Scale(AbsoluteNote.Parse("C4"), ScaleFormula.Major);
      object y = new Scale(AbsoluteNote.Parse("C4"), ScaleFormula.Major);
      object z = new Scale(AbsoluteNote.Parse("C4"), ScaleFormula.Major);

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
      var x = new Scale(AbsoluteNote.Parse("C4"), ScaleFormula.Major);
      var y = new Scale(AbsoluteNote.Parse("C4"), ScaleFormula.Major);
      var z = new Scale(AbsoluteNote.Parse("C4"), ScaleFormula.Major);

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
      object actual = new Scale(AbsoluteNote.Parse("C4"), ScaleFormula.Major);
      Assert.False(actual.Equals(int.MinValue));
    }

    [Fact]
    public void TypeSafeEqualsFailsWithDifferentTypeTest()
    {
      var actual = new Scale(AbsoluteNote.Parse("C4"), ScaleFormula.Major);
      Assert.False(actual.Equals(int.MinValue));
    }

    [Fact]
    public void EqualsFailsWithNullTest()
    {
      object actual = new Scale(AbsoluteNote.Parse("C4"), ScaleFormula.Major);
      Assert.False(actual.Equals(null));
    }

    [Fact]
    public void TypeSafeEqualsFailsWithNullTest()
    {
      var actual = new Scale(AbsoluteNote.Parse("C4"), ScaleFormula.Major);
      Assert.False(actual.Equals(null));
    }

    [Fact]
    public void EqualsSucceedsWithSameObjectTest()
    {
      var actual = new Scale(AbsoluteNote.Parse("C4"), ScaleFormula.Major);
      Assert.True(actual.Equals(actual));
    }

    [Fact]
    public void GetHashcodeTest()
    {
      var actual = new Scale(AbsoluteNote.Parse("C4"), ScaleFormula.Major);
      var expected = new Scale(AbsoluteNote.Parse("C4"), ScaleFormula.Major);
      Assert.True(expected.Equals(actual));
      Assert.Equal(expected.GetHashCode(), actual.GetHashCode());
    }

    #endregion

    #region Implementation

    private static void TestScale(string expectedNotes, AbsoluteNote root, ScaleFormula formula)
    {
      AbsoluteNoteCollection expected = AbsoluteNoteCollection.Parse(expectedNotes);
      var actual = new AbsoluteNoteCollection(new Scale(root, formula).Take(expected.Count).ToArray());
      Assert.Equal(expected, actual);
    }

    #endregion
  }
}
