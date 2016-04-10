//  
// Module Name: StringedInstrumentTest.cs
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

  public class StringedInstrumentTest
  {
    #region Public Methods

    [Fact]
    public void CreateWithDefinitionTest()
    {
      var definition = InstrumentDefinitionRegistry.Get<StringedInstrumentDefinition>("guitar");
      Tuning expectedTuning = definition.Tunings.Standard;

      StringedInstrument instrument = StringedInstrument.Create(definition, 22, expectedTuning);
      Assert.NotNull(instrument);
      Assert.Equal(definition, instrument.Definition);
      Assert.Equal(22, instrument.FretCount);
      Assert.Equal(expectedTuning, instrument.Tuning);
    }

    [Fact]
    public void CreateWithDefinitionDefaultTuningTest()
    {
      var definition = InstrumentDefinitionRegistry.Get<StringedInstrumentDefinition>("guitar");
      Tuning expectedTuning = definition.Tunings.Standard;

      StringedInstrument instrument = StringedInstrument.Create(definition, 22);
      Assert.NotNull(instrument);
      Assert.Equal(definition, instrument.Definition);
      Assert.Equal(22, instrument.FretCount);
      Assert.Equal(expectedTuning, instrument.Tuning);
    }

    [Fact]
    public void CreateWithDefinitionThrowsOnNullDefinitionTest()
    {
      var definition = InstrumentDefinitionRegistry.Get<StringedInstrumentDefinition>("guitar");
      Tuning expectedTuning = definition.Tunings.Standard;
      Assert.Throws<ArgumentNullException>(() => StringedInstrument.Create(null, 22, expectedTuning));
    }

    [Fact]
    public void CreateWithDefinitionThrowsOnMismatchedStringCountTest()
    {
      var definition = InstrumentDefinitionRegistry.Get<StringedInstrumentDefinition>("guitar");
      Assert.Throws<ArgumentOutOfRangeException>(() => StringedInstrument.Create(definition, 0));
    }

    [Fact]
    public void TestFactoryDefaultTuning()
    {
      var definition = InstrumentDefinitionRegistry.Get<StringedInstrumentDefinition>("guitar");
      StringedInstrument instrument = StringedInstrument.Create(definition, 22);
      Assert.Equal(definition.Tunings.Standard, instrument.Tuning);
    }

    [Fact]
    public void TestFactoryNullDefinition()
    {
      Assert.Throws<ArgumentNullException>(() => { StringedInstrument.Create((StringedInstrumentDefinition) null, 22); });
    }

    [Fact]
    public void TestFactoryInvalidFretCount()
    {
      Assert.Throws<ArgumentOutOfRangeException>(
                                                 () =>
                                                 {
                                                   StringedInstrument.Create(
                                                                             InstrumentDefinitionRegistry
                                                                               .Get<StringedInstrumentDefinition>("bass"),
                                                                             0);
                                                 });
    }

    [Fact]
    public void CreateWithNamesTest()
    {
      var definition = InstrumentDefinitionRegistry.Get<StringedInstrumentDefinition>("guitar");
      Tuning expectedTuning = definition.Tunings.Standard;

      StringedInstrument instrument = StringedInstrument.Create("guitar", 22, "standard");
      Assert.NotNull(instrument);
      Assert.Equal(definition, instrument.Definition);
      Assert.Equal(22, instrument.FretCount);
      Assert.Equal(expectedTuning, instrument.Tuning);
    }

    [Fact]
    public void CreateWithNamesDefaultTuningTest()
    {
      var definition = InstrumentDefinitionRegistry.Get<StringedInstrumentDefinition>("guitar");
      Tuning expectedTuning = definition.Tunings.Standard;

      StringedInstrument instrument = StringedInstrument.Create("guitar", 22);
      Assert.NotNull(instrument);
      Assert.Equal(definition, instrument.Definition);
      Assert.Equal(22, instrument.FretCount);
      Assert.Equal(expectedTuning, instrument.Tuning);
    }

    [Fact]
    public void CreateWithNamesThrowsOnNullDefinitionNameTest()
    {
      Assert.Throws<ArgumentNullException>(() => StringedInstrument.Create(null, 22, "standard"));
    }

    [Fact]
    public void CreateWithNamesThrowsOnInvalidFretCountTest()
    {
      Assert.Throws<ArgumentOutOfRangeException>(() => StringedInstrument.Create("guitar", 0));
    }

    [Fact]
    public void EqualsContractTest()
    {
      var definition = InstrumentDefinitionRegistry.Lookup<StringedInstrumentDefinition>("guitar");

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
      var definition = InstrumentDefinitionRegistry.Lookup<StringedInstrumentDefinition>("guitar");

      StringedInstrument x = StringedInstrument.Create(definition, 22);
      StringedInstrument y = StringedInstrument.Create(definition, 22);
      StringedInstrument z = StringedInstrument.Create(definition, 22);

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
      var definition = InstrumentDefinitionRegistry.Lookup<StringedInstrumentDefinition>("guitar");
      object a = StringedInstrument.Create(definition, 22);
      object b = StringedInstrument.Create(InstrumentDefinitionRegistry.Lookup<StringedInstrumentDefinition>("bass"), 22);

      Assert.False(a.Equals(b));
      Assert.False(b.Equals(a));
      Assert.False(Equals(a, b));
      Assert.False(Equals(b, a));
    }

    [Fact]
    public void TypeSafeEqualsFailsWithDifferentTypeTest()
    {
      var definition = InstrumentDefinitionRegistry.Lookup<StringedInstrumentDefinition>("guitar");
      StringedInstrument a = StringedInstrument.Create(definition, 22);
      StringedInstrument b =
        StringedInstrument.Create(InstrumentDefinitionRegistry.Lookup<StringedInstrumentDefinition>("bass"), 22);

      Assert.False(a.Equals(b));
      Assert.False(b.Equals(a));
      Assert.False(Equals(a, b));
      Assert.False(Equals(b, a));
    }

    [Fact]
    public void EqualsFailsWithNullTest()
    {
      var definition = InstrumentDefinitionRegistry.Lookup<StringedInstrumentDefinition>("guitar");
      object actual = StringedInstrument.Create(definition, 22);
      Assert.False(actual.Equals(null));
    }

    [Fact]
    public void TypeSafeEqualsFailsWithNullTest()
    {
      var definition = InstrumentDefinitionRegistry.Lookup<StringedInstrumentDefinition>("guitar");
      StringedInstrument actual = StringedInstrument.Create(definition, 22);
      Assert.False(actual.Equals(null));
    }

    [Fact]
    public void EqualsSucceedsWithSameObjectTest()
    {
      var definition = InstrumentDefinitionRegistry.Lookup<StringedInstrumentDefinition>("guitar");
      StringedInstrument actual = StringedInstrument.Create(definition, 22);
      Assert.True(actual.Equals(actual));
    }

    [Fact]
    public void GetHashcodeTest()
    {
      var definition = InstrumentDefinitionRegistry.Lookup<StringedInstrumentDefinition>("guitar");
      StringedInstrument actual = StringedInstrument.Create(definition, 22);
      StringedInstrument expected = StringedInstrument.Create(definition, 22);
      Assert.True(expected.Equals(actual));
      Assert.Equal(expected.GetHashCode(), actual.GetHashCode());
    }

    #endregion
  }
}
