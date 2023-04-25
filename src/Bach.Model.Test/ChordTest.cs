// Module Name: ChordTest.cs
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

using System;
using System.Collections;
using System.Linq;
using Xunit;

namespace Bach.Model.Test;

public sealed class ChordTest
{
#region Public Methods

  [Fact]
  public void ChordIsExtendedTest()
  {
    ChordIsExtended( PitchClass.C, "Major", false );
    ChordIsExtended( PitchClass.C, "Major7", false );
    ChordIsExtended( PitchClass.C, "Major9", true );
    ChordIsExtended( PitchClass.C, "Major11", true );
    ChordIsExtended( PitchClass.C, "Major13", true );
    ChordIsExtended( PitchClass.C, "Minor", false );
    ChordIsExtended( PitchClass.C, "Minor7", false );
    ChordIsExtended( PitchClass.C, "Minor9", true );
    ChordIsExtended( PitchClass.C, "Minor11", true );
    ChordIsExtended( PitchClass.C, "Minor13", true );
    ChordIsExtended( PitchClass.C, "Dominant7", false );
    ChordIsExtended( PitchClass.C, "Dominant9", true );
    ChordIsExtended( PitchClass.C, "Dominant11", true );
    ChordIsExtended( PitchClass.C, "Dominant13", true );
    ChordIsExtended( PitchClass.C, "SixNine", true );
    ChordIsExtended( PitchClass.C, "AddNine", true );
    ChordIsExtended( PitchClass.C, "Diminished", false );
    ChordIsExtended( PitchClass.C, "Diminished7", false );
    ChordIsExtended( PitchClass.C, "HalfDiminished", false );
    ChordIsExtended( PitchClass.C, "Augmented", false );
  }

  [Fact]
  public void ChordsTest()
  {
    ChordTestImpl( "C,E,G", PitchClass.C, "Major" );
    ChordTestImpl( "C,E,G,B", PitchClass.C, "Major7" );
    ChordTestImpl( "C,E,G,B,D", PitchClass.C, "Major9" );
    ChordTestImpl( "C,E,G,B,D,F", PitchClass.C, "Major11" );
    ChordTestImpl( "C,E,G,B,D,F,A", PitchClass.C, "Major13" );
    ChordTestImpl( "C,Eb,G", PitchClass.C, "Minor" );
    ChordTestImpl( "C,Eb,G,Bb", PitchClass.C, "Minor7" );
    ChordTestImpl( "C,Eb,G,Bb,D", PitchClass.C, "Minor9" );
    ChordTestImpl( "C,Eb,G,Bb,D,F", PitchClass.C, "Minor11" );
    ChordTestImpl( "C,Eb,G,Bb,D,F,A", PitchClass.C, "Minor13" );
    ChordTestImpl( "C,E,G,Bb", PitchClass.C, "Dominant7" );
    ChordTestImpl( "C,E,G,Bb,D", PitchClass.C, "Dominant9" );
    ChordTestImpl( "C,E,G,Bb,D,F", PitchClass.C, "Dominant11" );
    ChordTestImpl( "C,E,G,Bb,D,F,A", PitchClass.C, "Dominant13" );
    ChordTestImpl( "C,E,G,A,D", PitchClass.C, "SixNine" );
    ChordTestImpl( "C,E,G,D", PitchClass.C, "AddNine" );
    ChordTestImpl( "C,Eb,Gb", PitchClass.C, "Diminished" );
    ChordTestImpl( "C,Eb,Gb,A", PitchClass.C, "Diminished7" );
    ChordTestImpl( "C,Eb,Gb,Bb", PitchClass.C, "HalfDiminished" );
    ChordTestImpl( "C,E,G#", PitchClass.C, "Augmented" );
  }

  [Fact]
  public void EnumeratorTest()
  {
    var cMajor = new Chord( PitchClass.C, "Major" );
    var enumerator = ( (IEnumerable) cMajor ).GetEnumerator();
    Assert.NotNull( enumerator );
    Assert.True( enumerator.MoveNext() );
    Assert.Equal( PitchClass.C, enumerator.Current );
    Assert.True( enumerator.MoveNext() );
    Assert.Equal( PitchClass.E, enumerator.Current );
    Assert.True( enumerator.MoveNext() );
    Assert.Equal( PitchClass.G, enumerator.Current );
    Assert.False( enumerator.MoveNext() );
  }

  [Fact]
  public void EqualsContractTest()
  {
    object x = new Chord( PitchClass.C, "Major" );
    object y = new Chord( PitchClass.C, "Major" );
    object z = new Chord( PitchClass.C, "Major" );

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
    object actual = new Chord( PitchClass.C, "Major" );
    Assert.False( actual.Equals( int.MinValue ) );
  }

  [Fact]
  public void EqualsFailsWithNullTest()
  {
    object actual = new Chord( PitchClass.C, "Major" );
    Assert.False( actual.Equals( null ) );
  }

  [Fact]
  public void EqualsSucceedsWithSameObjectTest()
  {
    var actual = new Chord( PitchClass.C, "Major" );
    Assert.True( actual.Equals( actual ) );
  }

  [Fact]
  public void FormulaConstructorTest()
  {
    var formula = Registry.ChordFormulas["Minor"];
    var target = new Chord( PitchClass.C, formula );
    Assert.Equal( PitchClass.C, target.Root );
    Assert.Equal( Registry.ChordFormulas["Minor"], target.Formula );
    Assert.Equal( "Cm", target.Name );
    Assert.Equal( PitchClassCollection.Parse( "C,Eb,G" ), target.PitchClasses );
    Assert.Equal( target.Name, target.ToString() );
  }

  [Fact]
  public void FormulaConstructorThrowsOnNullFormulaTest()
  {
    Assert.Throws<ArgumentNullException>( () => new Chord( PitchClass.C, (ChordFormula) null ) );
  }

  [Fact]
  public void GetHashcodeTest()
  {
    var actual = new Chord( PitchClass.C, "Major" );
    var expected = new Chord( PitchClass.C, "Major" );
    Assert.True( expected.Equals( actual ) );
    Assert.Equal( expected.GetHashCode(), actual.GetHashCode() );
  }

  [Fact]
  public void GetInversionTest()
  {
    var cMajor = new Chord( PitchClass.C, "Major" );
    var firstInversion = cMajor.GetInversion( 1 );
    Assert.NotNull( firstInversion );
    Assert.Equal( "C/E", firstInversion.Name );
    Assert.Equal( PitchClassCollection.Parse( "E,G,C" ), firstInversion.PitchClasses );

    var secondInversion = cMajor.GetInversion( 2 );
    Assert.NotNull( secondInversion );
    Assert.Equal( "C/G", secondInversion.Name );
    Assert.Equal( PitchClassCollection.Parse( "G,C,E" ), secondInversion.PitchClasses );

    Assert.Throws<ArgumentOutOfRangeException>( () => cMajor.GetInversion( 3 ) );
  }

  [Fact]
  public void StringConstructorTest()
  {
    var target = new Chord( PitchClass.C, "Minor" );
    Assert.Equal( PitchClass.C, target.Root );
    Assert.Equal( Registry.ChordFormulas["Minor"], target.Formula );
    Assert.Equal( "Cm", target.Name );
    Assert.Equal( PitchClassCollection.Parse( "C,Eb,G" ), target.PitchClasses );
    Assert.Equal( target.Name, target.ToString() );
  }

  [Fact]
  public void StringConstructorThrowsOnEmptyFormulaNameTest()
  {
    Assert.Throws<ArgumentException>( () => new Chord( PitchClass.C, "" ) );
  }

  [Fact]
  public void StringConstructorThrowsOnNullFormulaNameTest()
  {
    Assert.Throws<ArgumentNullException>( () => new Chord( PitchClass.C, (string) null ) );
  }

  [Fact]
  public void TypeSafeEqualsContractTest()
  {
    var x = new Chord( PitchClass.C, "Major" );
    var y = new Chord( PitchClass.C, "Major" );
    var z = new Chord( PitchClass.C, "Major" );

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
    var actual = new Chord( PitchClass.C, "Major" );

    // ReSharper disable once SuspiciousTypeConversion.Global
    Assert.False( actual.Equals( int.MinValue ) );
  }

  [Fact]
  public void TypeSafeEqualsFailsWithNullTest()
  {
    var actual = new Chord( PitchClass.C, "Major" );
    Assert.False( actual.Equals( null ) );
  }

#endregion

#region Implementation

  private static void ChordTestImpl(
    string expectedNotes,
    PitchClass root,
    string formulaName )
  {
    var chord = new Chord( root, formulaName );
    var actualNotes = chord.Take( PitchClassCollection.Parse( expectedNotes ).Count );
    Assert.Equal( PitchClassCollection.Parse( expectedNotes ), actualNotes );
  }

  private static void ChordIsExtended(
    PitchClass root,
    string formulaName,
    bool expected )
  {
    var chord = new Chord( root, formulaName );
    Assert.Equal( expected, chord.IsExtended );
  }

#endregion
}
