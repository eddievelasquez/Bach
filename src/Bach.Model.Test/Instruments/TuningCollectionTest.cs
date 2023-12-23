// Module Name: TuningCollectionTest.cs
// Project:     Bach.Model.Test
// Copyright (c) 2012, 2023  Eddie Velasquez.
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

namespace Bach.Model.Test.Instruments;

using System.Linq;

public sealed class TuningCollectionTest
{
  #region Public Methods

  [Fact]
  public void ContainsKeyTest()
  {
    var definition = Registry.StringedInstrumentDefinitions["guitar"];
    definition.Tunings.ContainsKey( "standard" )
              .Should()
              .BeTrue();
    definition.Tunings.ContainsKey( "DoesNotExist!!!" )
              .Should()
              .BeFalse();
  }

  [Fact]
  public void ContainsKeyThrowsOnNullKeyTest()
  {
    var definition = Registry.StringedInstrumentDefinitions["guitar"];
    var act = () => definition.Tunings.ContainsKey( null! );
    act.Should()
       .Throw<ArgumentNullException>();
  }

  [Fact]
  public void GetEnumeratorOfObjectTest()
  {
    var definition = Registry.StringedInstrumentDefinitions["guitar"];
    var tunings = definition.Tunings.ToArray();

    using var enumerator = definition.Tunings.GetEnumerator();

    // ReSharper disable once ForCanBeConvertedToForeach
    for( var i = 0; i < tunings.Length; i++ )
    {
      enumerator.MoveNext()
                .Should()
                .BeTrue();
      enumerator.Current.Should()
                .Be( tunings[i] );
    }

    enumerator.MoveNext()
              .Should()
              .BeFalse();
  }

  [Fact]
  public void GetEnumeratorTest()
  {
    var definition = Registry.StringedInstrumentDefinitions["guitar"];
    var tunings = definition.Tunings.ToArray();

    using var enumerator = definition.Tunings.GetEnumerator();

    // ReSharper disable once ForCanBeConvertedToForeach
    for( var i = 0; i < tunings.Length; i++ )
    {
      enumerator.MoveNext()
                .Should()
                .BeTrue();
      enumerator.Current.Should()
                .Be( tunings[i] );
    }

    enumerator.MoveNext()
              .Should()
              .BeFalse();
  }

  [Fact]
  public void KeysTest()
  {
    var definition = Registry.StringedInstrumentDefinitions["guitar"];
    definition.Tunings.Keys.Count()
              .Should()
              .Be( definition.Tunings.Count );
  }

  [Fact]
  public void TryGetValueTest()
  {
    var definition = Registry.StringedInstrumentDefinitions["guitar"];

    definition.Tunings.TryGetValue( "standard", out var tuning )
              .Should()
              .BeTrue();
    tuning.Should()
          .Be( definition.Tunings.Standard );
  }

  [Fact]
  public void TryGetValueThrowsOnNullKeyTest()
  {
    var definition = Registry.StringedInstrumentDefinitions["guitar"];
    var act = () => definition.Tunings.TryGetValue( null!, out _ );
    act.Should()
       .Throw<ArgumentNullException>();
  }

  [Fact]
  public void ValuesTest()
  {
    var definition = Registry.StringedInstrumentDefinitions["guitar"];
    definition.Tunings.Values.Count()
              .Should()
              .Be( definition.Tunings.Count );
  }

  #endregion
}
