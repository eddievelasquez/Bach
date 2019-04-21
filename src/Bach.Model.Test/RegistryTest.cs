//
// Module Name: RegistryTest.cs
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
// PARTICULAR PURPOSE AND NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE
// OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

namespace Bach.Model.Test
{
  using System.Linq;
  using Model.Instruments;
  using Xunit;

  public class RegistryTest
  {
    [Fact]
    public void ScaleFormulasTest()
    {
      ScaleFormula[] scaleFormulas = Registry.ScaleFormulas.ToArray();
      Assert.NotNull(scaleFormulas);
      Assert.NotEmpty(scaleFormulas);

      foreach( ScaleFormula expected in scaleFormulas )
      {
        ScaleFormula actual = Registry.ScaleFormulas[expected.Id];
        Assert.Equal(expected, actual);
      }
    }

    [Fact]
    public void ChordFormulasTest()
    {
      ChordFormula[] chordFormulas = Registry.ChordFormulas.ToArray();
      Assert.NotNull(chordFormulas);
      Assert.NotEmpty(chordFormulas);

      foreach( ChordFormula expected in chordFormulas )
      {
        ChordFormula actual = Registry.ChordFormulas[expected.Id];
        Assert.Equal(expected, actual);
      }
    }

    [Fact]
    public void StringedInstrumentDefinitionsTest()
    {
      StringedInstrumentDefinition[] instrumentDefinitions = Registry.StringedInstrumentDefinitions.ToArray();
      Assert.NotNull(instrumentDefinitions);
      Assert.NotEmpty(instrumentDefinitions);

      foreach( StringedInstrumentDefinition expected in instrumentDefinitions )
      {
        InstrumentDefinition actual = Registry.StringedInstrumentDefinitions[expected.Id];
        Assert.Equal(expected, actual);
      }
    }
  }
}
