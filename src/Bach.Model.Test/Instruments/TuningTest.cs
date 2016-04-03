//  
// Module Name: TuningTest.cs
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

namespace Bach.Model.Test.Instruments
{
  using System;
  using System.Linq;
  using Bach.Model.Instruments;
  using Xunit;

  public class TuningTest
  {
    #region Public Methods

    [Fact]
    public void TestConstructor()
    {
      var guitar = new GuitarDefinition();
      var actual = new Tuning(guitar, "Drop D", NoteCollection.Parse("E4,B3,G3,D3,A2,D2"));
      Assert.Equal(guitar, actual.InstrumentDefinition);
      Assert.Equal("Drop D", actual.Name);
      Assert.NotNull(actual.Notes);
      Assert.Equal(6, actual.Notes.Length);
    }

    [Fact]
    public void ConstructorFailsWithNullInstrumentTest()
    {
      Assert.Throws<ArgumentNullException>(() => new Tuning(null, "Drop D", NoteCollection.Parse("E4,B3,G3,D3,A2,D2")));
    }

    [Fact]
    public void ConstructorFailsWithNullNameTest()
    {
      Assert.Throws<ArgumentNullException>(() =>
                                           {
                                             var guitar = new GuitarDefinition();
                                             new Tuning(guitar, null, NoteCollection.Parse("E4,B3,G3,D3,A2,D2"));
                                           });
    }

    [Fact]
    public void ConstructorFailsWithEmptyNameTest()
    {
      Assert.Throws<ArgumentException>(() =>
                                       {
                                         var guitar = new GuitarDefinition();
                                         new Tuning(guitar, "", NoteCollection.Parse("E4,B3,G3,D3,A2,D2"));
                                       });
    }

    [Fact]
    public void ConstructorFailsWithNullNoteCollectionTest()
    {
      Assert.Throws<ArgumentException>(() =>
                                       {
                                         var guitar = new GuitarDefinition();
                                         new Tuning(guitar, "", (NoteCollection) null);
                                       });
    }

    [Fact]
    public void ConstructorFailsWithNullNoteArrayTest()
    {
      Assert.Throws<ArgumentException>(() =>
                                       {
                                         var guitar = new GuitarDefinition();
                                         new Tuning(guitar, "", (Note[]) null);
                                       });
    }

    [Fact]
    public void ConstructorFailsWithInvalidNoteCollectionCountTest()
    {
      Assert.Throws<ArgumentOutOfRangeException>(() =>
                                                 {
                                                   var guitar = new GuitarDefinition();
                                                   new Tuning(guitar, "Drop D", NoteCollection.Parse("E4,B3,G3,D3,A2"));
                                                 });
    }

    [Fact]
    public void ConstructorFailsWithInvalidNoteArrayLengthTest()
    {
      Assert.Throws<ArgumentOutOfRangeException>(() =>
                                                 {
                                                   var guitar = new GuitarDefinition();
                                                   new Tuning(guitar, "Drop D",
                                                              NoteCollection.Parse("E4,B3,G3,D3,A2").ToArray());
                                                 });
    }

    [Fact]
    public void EqualsContractTest()
    {
      object x = new Tuning(new GuitarDefinition(), "Drop D", NoteCollection.Parse("E4,B3,G3,D3,A2,D2"));
      object y = new Tuning(new GuitarDefinition(), "Drop D", NoteCollection.Parse("E4,B3,G3,D3,A2,D2"));
      object z = new Tuning(new GuitarDefinition(), "Drop D", NoteCollection.Parse("E4,B3,G3,D3,A2,D2"));

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
      var x = new Tuning(new GuitarDefinition(), "Drop D", NoteCollection.Parse("E4,B3,G3,D3,A2,D2"));
      var y = new Tuning(new GuitarDefinition(), "Drop D", NoteCollection.Parse("E4,B3,G3,D3,A2,D2"));
      var z = new Tuning(new GuitarDefinition(), "Drop D", NoteCollection.Parse("E4,B3,G3,D3,A2,D2"));

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
      object a = new Tuning(new GuitarDefinition(), "Drop D", NoteCollection.Parse("E4,B3,G3,D3,A2,D2"));
      object b = new GuitarDefinition();
      Assert.False(a.Equals(b));
      Assert.False(b.Equals(a));
      Assert.False(Equals(a, b));
      Assert.False(Equals(b, a));
    }

    [Fact]
    public void TypeSafeEqualsFailsWithDifferentTypeTest()
    {
      var a = new Tuning(new GuitarDefinition(), "Drop D", NoteCollection.Parse("E4,B3,G3,D3,A2,D2"));
      var b = new GuitarDefinition();
      Assert.False(a.Equals(b));
      Assert.False(b.Equals(a));
      Assert.False(Equals(a, b));
      Assert.False(Equals(b, a));
    }

    [Fact]
    public void EqualsFailsWithNullTest()
    {
      object actual = new Tuning(new GuitarDefinition(), "Drop D", NoteCollection.Parse("E4,B3,G3,D3,A2,D2"));
      Assert.False(actual.Equals(null));
    }

    [Fact]
    public void TypeSafeEqualsFailsWithNullTest()
    {
      var actual = new Tuning(new GuitarDefinition(), "Drop D", NoteCollection.Parse("E4,B3,G3,D3,A2,D2"));
      Assert.False(actual.Equals(null));
    }

    [Fact]
    public void EqualsSucceedsWithSameObjectTest()
    {
      var actual = new Tuning(new GuitarDefinition(), "Drop D", NoteCollection.Parse("E4,B3,G3,D3,A2,D2"));
      Assert.True(actual.Equals(actual));
    }

    [Fact]
    public void GetHashcodeTest()
    {
      var actual = new Tuning(new GuitarDefinition(), "Drop D", NoteCollection.Parse("E4,B3,G3,D3,A2,D2"));
      var expected = new Tuning(new GuitarDefinition(), "Drop D", NoteCollection.Parse("E4,B3,G3,D3,A2,D2"));
      Assert.True(expected.Equals(actual));
      Assert.Equal(expected.GetHashCode(), actual.GetHashCode());
    }

    #endregion
  }
}
