// Module Name: InstrumentDefinitionTest.cs
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

using Bach.Model.Instruments;
using Xunit;

namespace Bach.Model.Test.Instruments;

public sealed class InstrumentDefinitionTest
{
#region Public Methods

  [Fact]
  public void EqualsContractTest()
  {
    object x = Registry.StringedInstrumentDefinitions["guitar"];
    object y = Registry.StringedInstrumentDefinitions["guitar"];
    object z = Registry.StringedInstrumentDefinitions["guitar"];

    // ReSharper disable once EqualExpressionComparison
    Assert.True( x.Equals( x ) ); // Reflexive
    Assert.True( x.Equals( y ) ); // Symmetric
    Assert.True( y.Equals( x ) );
    Assert.True( y.Equals( z ) ); // Transitive
    Assert.True( x.Equals( z ) );
    Assert.False( x.Equals( null ) ); // Never equal to null
  }

  [Fact]
  public void EqualsFailsWithDifferentTypeTest()
  {
    object actual = Registry.StringedInstrumentDefinitions["guitar"];
    Assert.False( actual.Equals( int.MinValue ) );
  }

  [Fact]
  public void EqualsFailsWithNullTest()
  {
    object actual = Registry.StringedInstrumentDefinitions["guitar"];
    Assert.False( actual.Equals( null ) );
  }

  [Fact]
  public void EqualsSucceedsWithSameObjectTest()
  {
    InstrumentDefinition actual = Registry.StringedInstrumentDefinitions["guitar"];
    Assert.True( actual.Equals( actual ) );
  }

  [Fact]
  public void EqualsTest()
  {
    var builder = new StringedInstrumentDefinitionBuilder( "GuitarX", "GuitarX", 6 );
    builder.AddTuning( "Standard", "Standard", PitchCollection.Parse( "E4,B3,G3,D3,A2,E2" ) );

    object a = builder.Build();
    object b = Registry.StringedInstrumentDefinitions["guitar"];
    Assert.False( a.Equals( b ) );
    Assert.False( b.Equals( a ) );
  }

  [Fact]
  public void GetHashcodeTest()
  {
    InstrumentDefinition actual = Registry.StringedInstrumentDefinitions["guitar"];
    InstrumentDefinition expected = Registry.StringedInstrumentDefinitions["guitar"];
    Assert.True( expected.Equals( actual ) );
    Assert.Equal( expected.GetHashCode(), actual.GetHashCode() );
  }

  [Fact]
  public void TypeSafeEqualsContractTest()
  {
    InstrumentDefinition x = Registry.StringedInstrumentDefinitions["guitar"];
    InstrumentDefinition y = Registry.StringedInstrumentDefinitions["guitar"];
    InstrumentDefinition z = Registry.StringedInstrumentDefinitions["guitar"];

    Assert.True( x.Equals( x ) ); // Reflexive
    Assert.True( x.Equals( y ) ); // Symmetric
    Assert.True( y.Equals( x ) );
    Assert.True( y.Equals( z ) ); // Transitive
    Assert.True( x.Equals( z ) );
    Assert.False( x.Equals( null ) ); // Never equal to null
  }

  [Fact]
  public void TypeSafeEqualsFailsWithDifferentTypeTest()
  {
    InstrumentDefinition actual = Registry.StringedInstrumentDefinitions["guitar"];

    // ReSharper disable once SuspiciousTypeConversion.Global
    Assert.False( actual.Equals( int.MinValue ) );
  }

  [Fact]
  public void TypeSafeEqualsFailsWithNullTest()
  {
    InstrumentDefinition actual = Registry.StringedInstrumentDefinitions["guitar"];
    Assert.False( actual.Equals( null ) );
  }

#endregion
}
