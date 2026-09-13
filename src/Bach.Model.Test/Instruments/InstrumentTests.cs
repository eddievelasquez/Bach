// Module Name: InstrumentTest.cs
// Project:     Bach.Model.Test
// Copyright (c) 2012, 2026  Eddie Velasquez.
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

namespace Bach.Model.Instruments.Test;

public sealed class InstrumentTests
{
  #region Nested Types

  private sealed class TestInstrument: Instrument
  {
    #region Constructors

    public TestInstrument(
      InstrumentDefinition? instrumentDefinition )
      : base( instrumentDefinition! )
    {
    }

    #endregion
  }

  #endregion

  #region Public Methods

  [Fact]
  public void Constructor_ShouldThrowArgumentNullException_WhenDefinitionIsNull()
  {
    var act = () => new TestInstrument( null! );

    act.Should()
       .Throw<ArgumentNullException>();
  }

  [Fact]
  public void EqualsContractTest()
  {
    var definition = Registry.StringedInstrumentDefinitions["guitar"];

    object x = new StringedInstrument( definition, 22, (Tuning?)null );
    object y = new StringedInstrument( definition, 22, (Tuning?)null );
    object z = new StringedInstrument( definition, 22, (Tuning?)null );

    // ReSharper disable once EqualExpressionComparison
    x.Equals( x )
     .Should()
     .BeTrue(); // Reflexive

    x.Equals( y )
     .Should()
     .BeTrue(); // Symmetric

    y.Equals( x )
     .Should()
     .BeTrue();

    y.Equals( z )
     .Should()
     .BeTrue(); // Transitive

    x.Equals( z )
     .Should()
     .BeTrue();

    x.Equals( null )
     .Should()
     .BeFalse(); // Never equal to null
  }

  [Fact]
  public void EqualsFailsWithDifferentTypeTest()
  {
    var definition = Registry.StringedInstrumentDefinitions["guitar"];
    object a = new StringedInstrument( definition, 22, (Tuning?)null );
    object b = new StringedInstrument( Registry.StringedInstrumentDefinitions["bass"], 22, (Tuning?)null );

    a.Equals( b )
     .Should()
     .BeFalse();

    b.Equals( a )
     .Should()
     .BeFalse();

    Equals( a, b )
      .Should()
      .BeFalse();

    Equals( b, a )
      .Should()
      .BeFalse();
  }

  [Fact]
  public void EqualsFailsWithNullTest()
  {
    var definition = Registry.StringedInstrumentDefinitions["guitar"];
    object actual = new StringedInstrument( definition, 22, (Tuning?)null );

    actual.Equals( null )
          .Should()
          .BeFalse();
  }

  [Fact]
  public void EqualsSucceedsWithSameObjectTest()
  {
    var definition = Registry.StringedInstrumentDefinitions["guitar"];
    Instrument actual = new StringedInstrument( definition, 22, (Tuning?)null );

    actual.Equals( actual )
          .Should()
          .BeTrue();
  }

  [Fact]
  public void GetHashcodeTest()
  {
    var definition = Registry.StringedInstrumentDefinitions["guitar"];
    Instrument actual = new StringedInstrument( definition, 22, (Tuning?)null );
    Instrument expected = new StringedInstrument( definition, 22, (Tuning?)null );

    expected.Equals( actual )
            .Should()
            .BeTrue();

    actual.GetHashCode()
          .Should()
          .Be( expected.GetHashCode() );
  }

  [Fact]
  public void TypeSafeEqualsContractTest()
  {
    var definition = Registry.StringedInstrumentDefinitions["guitar"];

    Instrument x = new StringedInstrument( definition, 22, (Tuning?)null );
    Instrument y = new StringedInstrument( definition, 22, (Tuning?)null );
    Instrument z = new StringedInstrument( definition, 22, (Tuning?)null );

    x.Equals( x )
     .Should()
     .BeTrue(); // Reflexive

    x.Equals( y )
     .Should()
     .BeTrue(); // Symmetric

    y.Equals( x )
     .Should()
     .BeTrue();

    y.Equals( z )
     .Should()
     .BeTrue(); // Transitive

    x.Equals( z )
     .Should()
     .BeTrue();

    x.Equals( null )
     .Should()
     .BeFalse(); // Never equal to null
  }

  [Fact]
  public void TypeSafeEqualsFailsWithDifferentTypeTest()
  {
    var definition = Registry.StringedInstrumentDefinitions["guitar"];
    Instrument a = new StringedInstrument( definition, 22, (Tuning?)null );
    Instrument b = new StringedInstrument( Registry.StringedInstrumentDefinitions["bass"], 22, (Tuning?)null );

    a.Equals( b )
     .Should()
     .BeFalse();

    b.Equals( a )
     .Should()
     .BeFalse();

    Equals( a, b )
      .Should()
      .BeFalse();

    Equals( b, a )
      .Should()
      .BeFalse();
  }

  [Fact]
  public void TypeSafeEqualsFailsWithNullTest()
  {
    var definition = Registry.StringedInstrumentDefinitions["guitar"];
    Instrument actual = new StringedInstrument( definition, 22, (Tuning?)null );

    actual.Equals( null )
          .Should()
          .BeFalse();
  }

  #endregion
}
