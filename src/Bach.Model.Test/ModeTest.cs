// Module Name: ModeTest.cs
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

public sealed class ModeTest
{
  #region Public Methods

  [Fact]
  public void EnumeratorTest()
  {
    var scale = new Scale( PitchClass.C, "Major" );
    var mode = new Mode( scale, ModeFormula.Ionian );
    using var enumerator = mode.GetEnumerator();
    enumerator.MoveNext()
              .Should()
              .BeTrue();
    enumerator.Current.Should()
              .Be( PitchClass.C );
    enumerator.MoveNext()
              .Should()
              .BeTrue();
    enumerator.Current.Should()
              .Be( PitchClass.D );
    enumerator.MoveNext()
              .Should()
              .BeTrue();
    enumerator.Current.Should()
              .Be( PitchClass.E );
    enumerator.MoveNext()
              .Should()
              .BeTrue();
    enumerator.Current.Should()
              .Be( PitchClass.F );
    enumerator.MoveNext()
              .Should()
              .BeTrue();
    enumerator.Current.Should()
              .Be( PitchClass.G );
    enumerator.MoveNext()
              .Should()
              .BeTrue();
    enumerator.Current.Should()
              .Be( PitchClass.A );
    enumerator.MoveNext()
              .Should()
              .BeTrue();
    enumerator.Current.Should()
              .Be( PitchClass.B );
    enumerator.MoveNext()
              .Should()
              .BeTrue(); // Mode enumerator wraps around infinitely
    enumerator.Current.Should()
              .Be( PitchClass.C );
  }

  [Fact]
  public void EqualsContractTest()
  {
    var scale = new Scale( PitchClass.C, "Major" );
    object x = new Mode( scale, ModeFormula.Dorian );
    object y = new Mode( scale, ModeFormula.Dorian );
    object z = new Mode( scale, ModeFormula.Dorian );

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
    var scale = new Scale( PitchClass.C, "Major" );
    object actual = new Mode( scale, ModeFormula.Dorian );
    actual.Equals( int.MinValue )
          .Should()
          .BeFalse();
  }

  [Fact]
  public void EqualsFailsWithNullTest()
  {
    var scale = new Scale( PitchClass.C, "Major" );
    object actual = new Mode( scale, ModeFormula.Dorian );
    actual.Equals( null )
          .Should()
          .BeFalse();
  }

  [Fact]
  public void EqualsSucceedsWithSameObjectTest()
  {
    var scale = new Scale( PitchClass.C, "Major" );
    var actual = new Mode( scale, ModeFormula.Dorian );
    actual.Equals( actual )
          .Should()
          .BeTrue();
  }

  [Fact]
  public void GetHashcodeTest()
  {
    var scale = new Scale( PitchClass.C, "Major" );
    var actual = new Mode( scale, ModeFormula.Dorian );
    var expected = new Mode( scale, ModeFormula.Dorian );
    expected.Equals( actual )
            .Should()
            .BeTrue();
    actual.GetHashCode()
          .Should()
          .Be( expected.GetHashCode() );
  }

  [Fact]
  public void ModeConstructorTest()
  {
    var scale = new Scale( PitchClass.C, "Major" );
    var formula = ModeFormula.Phrygian;
    var target = new Mode( scale, formula );

    target.Scale.Should()
          .Be( scale );
    target.Formula.Should()
          .Be( formula );
    target.Name.Should()
          .Be( "C Phrygian" );
    PitchClassCollection.Parse( "E,F,G,A,B,C,D" )
                        .Should()
                        .BeEquivalentTo( target.PitchClasses );
  }

  [Fact]
  public void ModesTest()
  {
    var scale = new Scale( PitchClass.C, "Major" );
    TestMode( "C,D,E,F,G,A,B", scale, ModeFormula.Ionian );
    TestMode( "D,E,F,G,A,B,C", scale, ModeFormula.Dorian );
    TestMode( "E,F,G,A,B,C,D", scale, ModeFormula.Phrygian );
    TestMode( "F,G,A,B,C,D,E", scale, ModeFormula.Lydian );
    TestMode( "G,A,B,C,D,E,F", scale, ModeFormula.Mixolydian );
    TestMode( "A,B,C,D,E,F,G", scale, ModeFormula.Aeolian );
    TestMode( "B,C,D,E,F,G,A", scale, ModeFormula.Locrian );
  }

  [Fact]
  public void TypeSafeEqualsContractTest()
  {
    var scale = new Scale( PitchClass.C, "Major" );
    var x = new Mode( scale, ModeFormula.Dorian );
    var y = new Mode( scale, ModeFormula.Dorian );
    var z = new Mode( scale, ModeFormula.Dorian );

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
    var scale = new Scale( PitchClass.C, "Major" );
    var actual = new Mode( scale, ModeFormula.Dorian );

    // ReSharper disable once SuspiciousTypeConversion.Global
    actual.Equals( int.MinValue )
          .Should()
          .BeFalse();
  }

  [Fact]
  public void TypeSafeEqualsFailsWithNullTest()
  {
    var scale = new Scale( PitchClass.C, "Major" );
    var actual = new Mode( scale, ModeFormula.Dorian );
    actual.Equals( null )
          .Should()
          .BeFalse();
  }

  #endregion

  #region Implementation

  private static void TestMode(
    string expectedNotes,
    Scale root,
    ModeFormula formula )
  {
    var expected = PitchClassCollection.Parse( expectedNotes );
    var mode = new Mode( root, formula );
    mode.PitchClasses.Should()
        .BeEquivalentTo( expected );
    mode.ToString()
        .Should()
        .Be( expectedNotes );
  }

  #endregion
}
