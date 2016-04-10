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
      var guitar = InstrumentDefinitionRegistry.Lookup<StringedInstrumentDefinition>("guitar");
      var actual = new Tuning(guitar, "Drop D", AbsoluteNoteCollection.Parse("E4,B3,G3,D3,A2,D2"));
      Assert.Equal(guitar, actual.InstrumentDefinition);
      Assert.Equal("Drop D", actual.Name);
      Assert.NotNull(actual.Notes);
      Assert.Equal(6, actual.Notes.Length);
    }

    [Fact]
    public void ConstructorFailsWithNullInstrumentTest()
    {
      Assert.Throws<ArgumentNullException>(
                                           () =>
                                             new Tuning(null, "Drop D",
                                                        AbsoluteNoteCollection.Parse("E4,B3,G3,D3,A2,D2")));
    }

    [Fact]
    public void ConstructorFailsWithNullNameTest()
    {
      Assert.Throws<ArgumentNullException>(() =>
                                           {
                                             var guitar =
                                               InstrumentDefinitionRegistry.Lookup<StringedInstrumentDefinition>(
                                                                                                                 "guitar");
                                             new Tuning(guitar, null, AbsoluteNoteCollection.Parse("E4,B3,G3,D3,A2,D2"));
                                           });
    }

    [Fact]
    public void ConstructorFailsWithEmptyNameTest()
    {
      Assert.Throws<ArgumentException>(() =>
                                       {
                                         var guitar =
                                           InstrumentDefinitionRegistry.Lookup<StringedInstrumentDefinition>("guitar");
                                         new Tuning(guitar, "", AbsoluteNoteCollection.Parse("E4,B3,G3,D3,A2,D2"));
                                       });
    }

    [Fact]
    public void ConstructorFailsWithNullNoteCollectionTest()
    {
      Assert.Throws<ArgumentException>(() =>
                                       {
                                         var guitar =
                                           InstrumentDefinitionRegistry.Lookup<StringedInstrumentDefinition>("guitar");
                                         new Tuning(guitar, "", (AbsoluteNoteCollection) null);
                                       });
    }

    [Fact]
    public void ConstructorFailsWithInvalidNoteCollectionCountTest()
    {
      Assert.Throws<ArgumentOutOfRangeException>(() =>
                                                 {
                                                   var guitar =
                                                     InstrumentDefinitionRegistry.Lookup<StringedInstrumentDefinition>(
                                                                                                                       "guitar");
                                                   new Tuning(guitar, "Drop D",
                                                              AbsoluteNoteCollection.Parse("E4,B3,G3,D3,A2"));
                                                 });
    }

    [Fact]
    public void EqualsContractTest()
    {
      var guitar = InstrumentDefinitionRegistry.Lookup<StringedInstrumentDefinition>("guitar");
      object x = new Tuning(guitar, "Drop D", AbsoluteNoteCollection.Parse("E4,B3,G3,D3,A2,D2"));
      object y = new Tuning(guitar, "Drop D", AbsoluteNoteCollection.Parse("E4,B3,G3,D3,A2,D2"));
      object z = new Tuning(guitar, "Drop D", AbsoluteNoteCollection.Parse("E4,B3,G3,D3,A2,D2"));

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
      var guitar = InstrumentDefinitionRegistry.Lookup<StringedInstrumentDefinition>("guitar");
      var x = new Tuning(guitar, "Drop D", AbsoluteNoteCollection.Parse("E4,B3,G3,D3,A2,D2"));
      var y = new Tuning(guitar, "Drop D", AbsoluteNoteCollection.Parse("E4,B3,G3,D3,A2,D2"));
      var z = new Tuning(guitar, "Drop D", AbsoluteNoteCollection.Parse("E4,B3,G3,D3,A2,D2"));

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
      var guitar = InstrumentDefinitionRegistry.Lookup<StringedInstrumentDefinition>("guitar");
      object a = new Tuning(guitar, "Drop D", AbsoluteNoteCollection.Parse("E4,B3,G3,D3,A2,D2"));
      object b = guitar;
      Assert.False(a.Equals(b));
      Assert.False(b.Equals(a));
      Assert.False(Equals(a, b));
      Assert.False(Equals(b, a));
    }

    [Fact]
    public void TypeSafeEqualsFailsWithDifferentTypeTest()
    {
      var guitar = InstrumentDefinitionRegistry.Lookup<StringedInstrumentDefinition>("guitar");
      var a = new Tuning(guitar, "Drop D", AbsoluteNoteCollection.Parse("E4,B3,G3,D3,A2,D2"));
      StringedInstrumentDefinition b = guitar;
      Assert.False(a.Equals(b));
      Assert.False(b.Equals(a));
      Assert.False(Equals(a, b));
      Assert.False(Equals(b, a));
    }

    [Fact]
    public void EqualsFailsWithNullTest()
    {
      var guitar = InstrumentDefinitionRegistry.Lookup<StringedInstrumentDefinition>("guitar");
      object actual = new Tuning(guitar, "Drop D", AbsoluteNoteCollection.Parse("E4,B3,G3,D3,A2,D2"));
      Assert.False(actual.Equals(null));
    }

    [Fact]
    public void TypeSafeEqualsFailsWithNullTest()
    {
      var guitar = InstrumentDefinitionRegistry.Lookup<StringedInstrumentDefinition>("guitar");
      var actual = new Tuning(guitar, "Drop D", AbsoluteNoteCollection.Parse("E4,B3,G3,D3,A2,D2"));
      Assert.False(actual.Equals(null));
    }

    [Fact]
    public void EqualsSucceedsWithSameObjectTest()
    {
      var guitar = InstrumentDefinitionRegistry.Lookup<StringedInstrumentDefinition>("guitar");
      var actual = new Tuning(guitar, "Drop D", AbsoluteNoteCollection.Parse("E4,B3,G3,D3,A2,D2"));
      Assert.True(actual.Equals(actual));
    }

    [Fact]
    public void GetHashcodeTest()
    {
      var guitar = InstrumentDefinitionRegistry.Lookup<StringedInstrumentDefinition>("guitar");
      var actual = new Tuning(guitar, "Drop D", AbsoluteNoteCollection.Parse("E4,B3,G3,D3,A2,D2"));
      var expected = new Tuning(guitar, "Drop D", AbsoluteNoteCollection.Parse("E4,B3,G3,D3,A2,D2"));
      Assert.True(expected.Equals(actual));
      Assert.Equal(expected.GetHashCode(), actual.GetHashCode());
    }

    #endregion
  }
}
