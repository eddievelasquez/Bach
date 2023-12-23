// Module Name: TuningTest.cs
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

using System.Diagnostics.CodeAnalysis;
using Model.Instruments;

public sealed class TuningTest
{
  #region Constants

  private const string InstrumentKey = "guitar";
  private const string TuningKey = "DropD";
  private const string TuningName = "Drop D";

  #endregion

  #region Public Methods

  [Fact]
  public void ConstructorFailsWithEmptyKeyTest()
  {
    var act = () =>
      {
        var guitar = Registry.StringedInstrumentDefinitions[InstrumentKey];

        _ = new Tuning(
          guitar,
          "",
          TuningName,
          PitchCollection.Parse( "E4,B3,G3,D3,A2,D2" )
        );
      }
      ;

    act.Should()
       .Throw<ArgumentException>();
  }

  [Fact]
  public void ConstructorFailsWithEmptyNameTest()
  {
    var act = () =>
      {
        var guitar = Registry.StringedInstrumentDefinitions[InstrumentKey];

        _ = new Tuning(
          guitar,
          TuningKey,
          "",
          PitchCollection.Parse( "E4,B3,G3,D3,A2,D2" )
        );
      }
      ;

    act.Should()
       .Throw<ArgumentException>();
  }

  [Fact]
  public void ConstructorFailsWithInvalidNoteCollectionCountTest()
  {
    var act = () =>
    {
      var guitar = Registry.StringedInstrumentDefinitions[InstrumentKey];

      _ = new Tuning(
        guitar,
        TuningKey,
        TuningName,
        PitchCollection.Parse( "E4,B3,G3,D3,A2" )
      );
    };

    act.Should()
       .Throw<ArgumentException>();
  }

  [Fact]
  public void EqualsContractTest()
  {
    var guitar = Registry.StringedInstrumentDefinitions[InstrumentKey];
    object x = new Tuning( guitar, TuningKey, TuningName, PitchCollection.Parse( "E4,B3,G3,D3,A2,D2" ) );
    object y = new Tuning( guitar, TuningKey, TuningName, PitchCollection.Parse( "E4,B3,G3,D3,A2,D2" ) );
    object z = new Tuning( guitar, TuningKey, TuningName, PitchCollection.Parse( "E4,B3,G3,D3,A2,D2" ) );

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
    var guitar = Registry.StringedInstrumentDefinitions[InstrumentKey];
    object a = new Tuning( guitar, TuningKey, TuningName, PitchCollection.Parse( "E4,B3,G3,D3,A2,D2" ) );
    object b = guitar;

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
    var guitar = Registry.StringedInstrumentDefinitions[InstrumentKey];
    object actual = new Tuning( guitar, TuningKey, TuningName, PitchCollection.Parse( "E4,B3,G3,D3,A2,D2" ) );

    actual.Equals( null )
          .Should()
          .BeFalse();
  }

  [Fact]
  public void EqualsSucceedsWithSameObjectTest()
  {
    var guitar = Registry.StringedInstrumentDefinitions[InstrumentKey];
    var actual = new Tuning( guitar, TuningKey, TuningName, PitchCollection.Parse( "E4,B3,G3,D3,A2,D2" ) );

    actual.Equals( actual )
          .Should()
          .BeTrue();
  }

  [Fact]
  public void GetHashcodeTest()
  {
    var guitar = Registry.StringedInstrumentDefinitions[InstrumentKey];
    var actual = new Tuning( guitar, TuningKey, TuningName, PitchCollection.Parse( "E4,B3,G3,D3,A2,D2" ) );
    var expected = new Tuning( guitar, TuningKey, TuningName, PitchCollection.Parse( "E4,B3,G3,D3,A2,D2" ) );

    expected.Equals( actual )
            .Should()
            .BeTrue();
    actual.GetHashCode()
          .Should()
          .Be( expected.GetHashCode() );
  }

  [Fact]
  public void TestConstructor()
  {
    var guitar = Registry.StringedInstrumentDefinitions[InstrumentKey];
    var actual = new Tuning( guitar, TuningKey, TuningName, PitchCollection.Parse( "E4,B3,G3,D3,A2,D2" ) );

    actual.InstrumentDefinition.Should()
          .Be( guitar );
    actual.Name.Should()
          .Be( TuningName );
    actual.Pitches.Should()
          .NotBeNull();
    actual.Pitches.Count.Should()
          .Be( 6 );
  }

  [Fact]
  public void TypeSafeEqualsContractTest()
  {
    var guitar = Registry.StringedInstrumentDefinitions[InstrumentKey];
    var x = new Tuning( guitar, TuningKey, TuningName, PitchCollection.Parse( "E4,B3,G3,D3,A2,D2" ) );
    var y = new Tuning( guitar, TuningKey, TuningName, PitchCollection.Parse( "E4,B3,G3,D3,A2,D2" ) );
    var z = new Tuning( guitar, TuningKey, TuningName, PitchCollection.Parse( "E4,B3,G3,D3,A2,D2" ) );

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
  [SuppressMessage( "ReSharper", "SuspiciousTypeConversion.Global" )]
  public void TypeSafeEqualsFailsWithDifferentTypeTest()
  {
    var guitar = Registry.StringedInstrumentDefinitions[InstrumentKey];
    var a = new Tuning( guitar, TuningKey, TuningName, PitchCollection.Parse( "E4,B3,G3,D3,A2,D2" ) );
    var b = guitar;

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
    var guitar = Registry.StringedInstrumentDefinitions[InstrumentKey];
    var actual = new Tuning( guitar, TuningKey, TuningName, PitchCollection.Parse( "E4,B3,G3,D3,A2,D2" ) );

    actual.Equals( null )
          .Should()
          .BeFalse();
  }

  #endregion
}
