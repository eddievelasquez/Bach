// Module Name: ModeFormulaTest.cs
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

namespace Bach.Model.Test;

public sealed class ModeFormulaTest
{
  #region Public Methods

  [Fact]
  public void Equals_ShouldSatisfyEquivalenceRelation_ObjectVariant()
  {
    object x = ModeFormula.Dorian;
    object y = ModeFormula.Dorian;
    object z = ModeFormula.Dorian;

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
  public void Equals_ShouldReturnFalse_WhenComparedWithDifferentType()
  {
    object actual = ModeFormula.Dorian;
    actual.Equals( int.MinValue )
          .Should()
          .BeFalse();
  }

  [Fact]
  public void Equals_ShouldReturnFalse_WhenComparedWithNull()
  {
    object actual = ModeFormula.Dorian;
    actual.Equals( null )
          .Should()
          .BeFalse();
  }

  [Fact]
  public void Equals_ShouldReturnTrue_WhenComparedWithSameObject()
  {
    var actual = ModeFormula.Dorian;
    actual.Equals( actual )
          .Should()
          .BeTrue();
  }

  [Fact]
  public void GetHashCode_ShouldReturnEqualValue_WhenObjectsAreEqual()
  {
    var actual = ModeFormula.Dorian;
    var expected = ModeFormula.Dorian;
    expected.Equals( actual )
            .Should()
            .BeTrue();
    actual.GetHashCode()
          .Should()
          .Be( expected.GetHashCode() );
  }

  [Theory]
  [MemberData( nameof( ModeNames ) )]
  public void ToString_ShouldReturnExpectedValue( ModeFormula formula, string expected )
  {
    formula.ToString()
           .Should()
           .Be( expected );
  }

  [Fact]
  public void Equals_ShouldReturnFalse_WhenTypeSafeComparedWithDifferentType()
  {
    var actual = ModeFormula.Dorian;

    // ReSharper disable once SuspiciousTypeConversion.Global
    actual.Equals( int.MinValue )
          .Should()
          .BeFalse();
  }

  [Fact]
  public void Equals_ShouldReturnFalse_WhenTypeSafeComparedWithNull()
  {
    var actual = ModeFormula.Dorian;
    actual.Equals( null )
          .Should()
          .BeFalse();
  }

  [Fact]
  public void Equals_ShouldReturnFalse_WhenComparedWithDifferentType_ObjectVariant()
  {
    object actual = ModeFormula.Dorian;
    actual.Equals( int.MinValue )
          .Should()
          .BeFalse();
  }

  [Fact]
  public void Equals_ShouldReturnFalse_WhenComparedWithNull_ObjectVariant()
  {
    object actual = ModeFormula.Dorian;
    actual.Equals( null )
          .Should()
          .BeFalse();
  }

  [Fact]
  public void Equals_ShouldReturnTrue_WhenComparedWithSameObject_TypeSafeVariant()
  {
    var actual = ModeFormula.Dorian;
    actual.Equals( actual )
          .Should()
          .BeTrue();
  }

  [Fact]
  public void GetHashCode_ShouldReturnEqualValue_WhenObjectsAreEqual_TypeSafeVariant()
  {
    var actual = ModeFormula.Dorian;
    var expected = ModeFormula.Dorian;
    expected.Equals( actual )
            .Should()
            .BeTrue();
    actual.GetHashCode()
          .Should()
          .Be( expected.GetHashCode() );
  }

  [Fact]
  public void Equals_ShouldSatisfyEquivalenceRelation_TypeSafeVariant()
  {
    var x = ModeFormula.Dorian;
    var y = ModeFormula.Dorian;
    var z = ModeFormula.Dorian;

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
  public void Equals_ShouldReturnFalse_WhenTypeSafeComparedWithDifferentType_TypeSafeVariant()
  {
    var actual = ModeFormula.Dorian;

    // ReSharper disable once SuspiciousTypeConversion.Global
    actual.Equals( int.MinValue )
          .Should()
          .BeFalse();
  }

  [Fact]
  public void Equals_ShouldReturnFalse_WhenTypeSafeComparedWithNull_TypeSafeVariant()
  {
    var actual = ModeFormula.Dorian;
    actual.Equals( null )
          .Should()
          .BeFalse();
  }

  public static TheoryData<ModeFormula, string> ModeNames =>
    new()
    {
      { ModeFormula.Ionian, "Ionian" },
      { ModeFormula.Dorian, "Dorian" },
      { ModeFormula.Phrygian, "Phrygian" },
      { ModeFormula.Lydian, "Lydian" },
      { ModeFormula.Mixolydian, "Mixolydian" },
      { ModeFormula.Aeolian, "Aeolian" },
      { ModeFormula.Locrian, "Locrian" }
    };

  #endregion
}
