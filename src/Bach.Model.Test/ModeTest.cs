//  
// Module Name: ModeTest.cs
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

  public class ModeTest
  {
    #region Public Methods

    [Fact]
    public void ModeConstructorTest()
    {
      Note root = Note.Parse("C4");
      ModeFormula formula = ModeFormula.Phrygian;
      var target = new Mode(root, formula);

      Assert.Equal(root, target.Root);
      Assert.Equal(formula, target.Formula);
      Assert.Equal("C Phrygian", target.Name);
      Assert.Equal(target.ToArray(), NoteCollection.Parse("E4,F4,G4,A4,B4,C5,D5"));
    }

    [Fact]
    public void ModesTest()
    {
      Note root = Note.Parse("C4");
      TestMode("C4,D4,E4,F4,G4,A4,B4", root, ModeFormula.Ionian);
      TestMode("D4,E4,F4,G4,A4,B4,C5", root, ModeFormula.Dorian);
      TestMode("E4,F4,G4,A4,B4,C5,D5", root, ModeFormula.Phrygian);
      TestMode("F4,G4,A4,B4,C5,D5,E5", root, ModeFormula.Lydian);
      TestMode("G4,A4,B4,C5,D5,E5,F5", root, ModeFormula.Mixolydian);
      TestMode("A4,B4,C5,D5,E5,F5,G5", root, ModeFormula.Aeolian);
      TestMode("B4,C5,D5,E5,F5,G5,A5", root, ModeFormula.Locrian);
    }

    [Fact]
    public void EqualsContractTest()
    {
      object x = new Mode(Note.Parse("C4"), ModeFormula.Dorian);
      object y = new Mode(Note.Parse("C4"), ModeFormula.Dorian);
      object z = new Mode(Note.Parse("C4"), ModeFormula.Dorian);

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
      var x = new Mode(Note.Parse("C4"), ModeFormula.Dorian);
      var y = new Mode(Note.Parse("C4"), ModeFormula.Dorian);
      var z = new Mode(Note.Parse("C4"), ModeFormula.Dorian);

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
      object actual = new Mode(Note.Parse("C4"), ModeFormula.Dorian);
      Assert.False(actual.Equals(int.MinValue));
    }

    [Fact]
    public void TypeSafeEqualsFailsWithDifferentTypeTest()
    {
      var actual = new Mode(Note.Parse("C4"), ModeFormula.Dorian);
      Assert.False(actual.Equals(int.MinValue));
    }

    [Fact]
    public void EqualsFailsWithNullTest()
    {
      object actual = new Mode(Note.Parse("C4"), ModeFormula.Dorian);
      Assert.False(actual.Equals(null));
    }

    [Fact]
    public void TypeSafeEqualsFailsWithNullTest()
    {
      var actual = new Mode(Note.Parse("C4"), ModeFormula.Dorian);
      Assert.False(actual.Equals(null));
    }

    [Fact]
    public void EqualsSucceedsWithSameObjectTest()
    {
      var actual = new Mode(Note.Parse("C4"), ModeFormula.Dorian);
      Assert.True(actual.Equals(actual));
    }

    [Fact]
    public void GetHashcodeTest()
    {
      var actual = new Mode(Note.Parse("C4"), ModeFormula.Dorian);
      var expected = new Mode(Note.Parse("C4"), ModeFormula.Dorian);
      Assert.True(expected.Equals(actual));
      Assert.Equal(expected.GetHashCode(), actual.GetHashCode());
    }

    #endregion

    #region Implementation

    private static void TestMode(string expectedNotes, Note root, ModeFormula formula)
    {
      NoteCollection expected = NoteCollection.Parse(expectedNotes);
      Assert.Equal(expected, new Mode(root, formula).Take(expected.Count).ToArray());
    }

    #endregion
  }
}
