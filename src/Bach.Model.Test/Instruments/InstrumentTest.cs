//  
// Module Name: InstrumentTest.cs
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
  using Bach.Model.Instruments;
  using Xunit;

  public class InstrumentTest
  {
    #region Public Methods

    [Fact]
    public void EqualsContractTest()
    {
      var definition = InstrumentDefinitionRegistry.Get<StringedInstrumentDefinition>("guitar");

      object x = StringedInstrument.Create(definition, 22);
      object y = StringedInstrument.Create(definition, 22);
      object z = StringedInstrument.Create(definition, 22);

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
      var definition = InstrumentDefinitionRegistry.Get<StringedInstrumentDefinition>("guitar");

      Instrument x = StringedInstrument.Create(definition, 22);
      Instrument y = StringedInstrument.Create(definition, 22);
      Instrument z = StringedInstrument.Create(definition, 22);

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
      var definition = InstrumentDefinitionRegistry.Get<StringedInstrumentDefinition>("guitar");
      object a = StringedInstrument.Create(definition, 22);
      object b = StringedInstrument.Create(InstrumentDefinitionRegistry.Get<StringedInstrumentDefinition>("bass"), 22);

      Assert.False(a.Equals(b));
      Assert.False(b.Equals(a));
      Assert.False(Equals(a, b));
      Assert.False(Equals(b, a));
    }

    [Fact]
    public void TypeSafeEqualsFailsWithDifferentTypeTest()
    {
      var definition = InstrumentDefinitionRegistry.Get<StringedInstrumentDefinition>("guitar");
      Instrument a = StringedInstrument.Create(definition, 22);
      Instrument b =
        StringedInstrument.Create(InstrumentDefinitionRegistry.Get<StringedInstrumentDefinition>("bass"), 22);

      Assert.False(a.Equals(b));
      Assert.False(b.Equals(a));
      Assert.False(Equals(a, b));
      Assert.False(Equals(b, a));
    }

    [Fact]
    public void EqualsFailsWithNullTest()
    {
      var definition = InstrumentDefinitionRegistry.Get<StringedInstrumentDefinition>("guitar");
      object actual = StringedInstrument.Create(definition, 22);
      Assert.False(actual.Equals(null));
    }

    [Fact]
    public void TypeSafeEqualsFailsWithNullTest()
    {
      var definition = InstrumentDefinitionRegistry.Get<StringedInstrumentDefinition>("guitar");
      Instrument actual = StringedInstrument.Create(definition, 22);
      Assert.False(actual.Equals(null));
    }

    [Fact]
    public void EqualsSucceedsWithSameObjectTest()
    {
      var definition = InstrumentDefinitionRegistry.Get<StringedInstrumentDefinition>("guitar");
      Instrument actual = StringedInstrument.Create(definition, 22);
      Assert.True(actual.Equals(actual));
    }

    [Fact]
    public void GetHashcodeTest()
    {
      var definition = InstrumentDefinitionRegistry.Get<StringedInstrumentDefinition>("guitar");
      Instrument actual = StringedInstrument.Create(definition, 22);
      Instrument expected = StringedInstrument.Create(definition, 22);
      Assert.True(expected.Equals(actual));
      Assert.Equal(expected.GetHashCode(), actual.GetHashCode());
    }

    #endregion
  }
}