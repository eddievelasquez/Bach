// Module Name: PartTest.cs
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

using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Bach.Model.Test;

public sealed class PartTests
{
  #region Public Methods

  [Fact]
  public void AddAndEnumerate_ShouldStoreEventsAndPreserveChordsAsSingleEvents()
  {
    var part = Part.Parse( "C4,C" );

    var events = part.ToArray();

    events.Should()
          .HaveCount( 2 );

    events[0]
      .Should()
      .Be( PitchClass.C[4] );

    events[1]
      .Should()
      .Be( PitchChord.Parse( "C" ) );
  }

  [Fact]
  public void Add_ShouldThrowArgumentNullException_WhenPartEventIsNull()
  {
    // Arrange
    var part = new Part();

    // Act
    var act = () => part.Add( null! );

    // Assert
    act.Should()
       .Throw<ArgumentNullException>();
  }

  [Fact]
  public void Clear_ShouldDoNothing_WhenPartIsEmpty()
  {
    // Arrange
    var part = new Part();

    // Act
    part.Clear();

    // Assert
    part.Count.Should()
        .Be( 0 );
  }

  [Fact]
  public void Clear_ShouldRemoveAllEvents_WhenCalled()
  {
    // Arrange
    var part = Part.Parse( "C4,D4" );

    // Act
    part.Clear();

    // Assert
    part.Count.Should()
        .Be( 0 );
  }

  [Fact]
  public void Contains_ShouldReturnFalse_WhenEventDoesNotExist()
  {
    // Arrange
    var part = Part.Parse( "C4" );

    // Act
    var result = part.Contains( PitchClass.D[4] );

    // Assert
    result.Should()
          .BeFalse();
  }

  [Fact]
  public void Contains_ShouldReturnFalse_WhenPartIsEmpty()
  {
    // Arrange
    var part = new Part();

    // Act
    var result = part.Contains( PitchClass.C[4] );

    // Assert
    result.Should()
          .BeFalse();
  }

  [Fact]
  public void Contains_ShouldReturnTrue_WhenEventExists()
  {
    // Arrange
    var part = Part.Parse( "C4" );

    // Act
    var result = part.Contains( PitchClass.C[4] );

    // Assert
    result.Should()
          .BeTrue();
  }

  [Fact]
  public void Contains_ShouldThrowArgumentNullException_WhenItemIsNull()
  {
    // Arrange
    var part = new Part();

    // Act
    var act = () => part.Contains( null! );

    // Assert
    act.Should()
       .Throw<ArgumentNullException>();
  }

  [Fact]
  public void CopyTo_ShouldCopyEventsToArrayAtIndex_WhenArrayIndexIsProvided()
  {
    // Arrange
    var part = Part.Parse( "C4,D4" );
    var array = new IPartEvent[4];

    // Act
    part.CopyTo( array, 2 );

    // Assert
    array[0]
      .Should()
      .BeNull();

    array[1]
      .Should()
      .BeNull();

    array[2]
      .Should()
      .Be( PitchClass.C[4] );

    array[3]
      .Should()
      .Be( PitchClass.D[4] );
  }

  [Fact]
  public void CopyTo_ShouldCopyEventsToArray_WhenCalled()
  {
    // Arrange
    var part = Part.Parse( "C4,D4" );
    var array = new IPartEvent[2];

    // Act
    part.CopyTo( array, 0 );

    // Assert
    array.Should()
         .HaveCount( 2 );

    array[0]
      .Should()
      .Be( PitchClass.C[4] );

    array[1]
      .Should()
      .Be( PitchClass.D[4] );
  }

  [Fact]
  public void CopyTo_ShouldThrowArgumentNullException_WhenArrayIsNull()
  {
    // Arrange
    var part = new Part();

    // Act
    var act = () => part.CopyTo( null!, 0 );

    // Assert
    act.Should()
       .Throw<ArgumentNullException>();
  }

  [Fact]
  public void Count_ShouldReturnCorrectValue_WhenEventsAreAdded()
  {
    // Arrange
    var part = new Part();
    var pitch = Pitch.Create( PitchClass.C, 4 );

    // Act
    part.Add( pitch );
    var count = part.Count;

    // Assert
    count.Should()
         .Be( 1 );
  }

  [Fact]
  public void Count_ShouldReturnZero_WhenPartIsEmpty()
  {
    // Arrange
    var part = new Part();

    // Act
    var count = part.Count;

    // Assert
    count.Should()
         .Be( 0 );
  }

  [Fact]
  public void GetEnumerator_ShouldReturnEmptyEnumerator_WhenPartIsEmpty()
  {
    // Arrange
    var part = new Part();

    // Act
    var enumerator = part.GetEnumerator();

    // Assert
    enumerator.Should()
              .NotBeNull();

    enumerator.MoveNext()
              .Should()
              .BeFalse();
  }

  [Fact]
  public void GetEnumerator_ShouldReturnEnumerator_WhenCalled()
  {
    // Arrange
    var part = Part.Parse( "C4,D4" );

    // Act
    var enumerator = part.GetEnumerator();

    // Assert
    enumerator.Should()
              .NotBeNull();
    var events = new List<IPartEvent>();

    while( enumerator.MoveNext() )
    {
      events.Add( enumerator.Current );
    }

    events.Should()
          .HaveCount( 2 );

    events[0]
      .Should()
      .Be( PitchClass.C[4] );

    events[1]
      .Should()
      .Be( PitchClass.D[4] );
  }

  [Fact]
  public void IEnumerableGetEnumerator_ShouldReturnEnumerator_WhenCalled()
  {
    // Arrange
    var part = Part.Parse( "C4,D4" );

    // Act
    var enumerator = ( (IEnumerable) part ).GetEnumerator();

    // Assert
    enumerator.Should()
              .NotBeNull();
    var events = new List<IPartEvent>();

    while( enumerator.MoveNext() )
    {
      events.Add( (IPartEvent) enumerator.Current );
    }

    events.Should()
          .HaveCount( 2 );

    events[0]
      .Should()
      .Be( PitchClass.C[4] );

    events[1]
      .Should()
      .Be( PitchClass.D[4] );
  }

  [Fact]
  public void IndexOf_ShouldReturnCorrectIndex_WhenEventExists()
  {
    // Arrange
    var part = Part.Parse( "C4,D4,E4" );

    // Act
    var index = part.IndexOf( PitchClass.D[4] );

    // Assert
    index.Should()
         .Be( 1 );
  }

  [Fact]
  public void IndexOf_ShouldReturnMinusOne_WhenEventDoesNotExist()
  {
    // Arrange
    var part = Part.Parse( "C4" );

    // Act
    var index = part.IndexOf( PitchClass.D[4] );

    // Assert
    index.Should()
         .Be( -1 );
  }

  [Fact]
  public void IndexOf_ShouldReturnMinusOne_WhenPartIsEmpty()
  {
    // Arrange
    var part = new Part();

    // Act
    var index = part.IndexOf( PitchClass.C[4] );

    // Assert
    index.Should()
         .Be( -1 );
  }

  [Fact]
  public void IndexOf_ShouldThrowArgumentNullException_WhenItemIsNull()
  {
    // Arrange
    var part = new Part();

    // Act
    var act = () => part.IndexOf( null! );

    // Assert
    act.Should()
       .Throw<ArgumentNullException>();
  }

  [Fact]
  public void Insert_ShouldInsertEventAtBeginning_WhenIndexIsZero()
  {
    // Arrange
    var part = Part.Parse( "C4" );
    var pitch = Pitch.Create( PitchClass.D, 4 );

    // Act
    part.Insert( 0, pitch );

    // Assert
    part.Count.Should()
        .Be( 2 );

    part[0]
      .Should()
      .Be( pitch );

    part[1]
      .Should()
      .Be( PitchClass.C[4] );
  }

  [Fact]
  public void Insert_ShouldInsertEventAtEnd_WhenIndexIsCount()
  {
    // Arrange
    var part = Part.Parse( "C4" );

    // Act
    part.Insert( part.Count, PitchClass.D[4] );

    // Assert
    part.Count.Should()
        .Be( 2 );

    part[0]
      .Should()
      .Be( PitchClass.C[4] );

    part[1]
      .Should()
      .Be( PitchClass.D[4] );
  }

  [Fact]
  public void Insert_ShouldInsertEventAtIndex_WhenCalled()
  {
    // Arrange
    var part = Part.Parse( "C4,E4" );

    // Act
    part.Insert( 1, PitchClass.D[4] );

    // Assert
    part.Count.Should()
        .Be( 3 );

    part[0]
      .Should()
      .Be( PitchClass.C[4] );

    part[1]
      .Should()
      .Be( PitchClass.D[4] );

    part[2]
      .Should()
      .Be( PitchClass.E[4] );
  }

  [Fact]
  public void Insert_ShouldThrowArgumentNullException_WhenItemIsNull()
  {
    // Arrange
    var part = new Part();

    // Act
    var act = () => part.Insert( 0, null! );

    // Assert
    act.Should()
       .Throw<ArgumentNullException>();
  }

  [Fact]
  public void IsReadOnly_ShouldReturnFalse()
  {
    // Arrange
    var part = new Part();

    // Act
    var isReadOnly = part.IsReadOnly;

    // Assert
    isReadOnly.Should()
              .BeFalse();
  }

  [Fact]
  public void Parse_ShouldThrowFormatExceptionForInvalidInput()
  {
    var act = () => Part.Parse( "invalid".AsSpan(), null );

    act.Should()
       .Throw<FormatException>();
  }

  [Fact]
  public void Part_ShouldInitializeEmptyList_WhenDefaultConstructorIsUsed()
  {
    // Arrange & Act
    var part = new Part();

    // Assert
    part.Count.Should()
        .Be( 0 );

    part.IsReadOnly.Should()
        .BeFalse();
  }

  [Fact]
  public void Part_ShouldInitializeWithCapacity_WhenCapacityIsProvided()
  {
    // Arrange
    var part = new Part( 10 );

    // Act & Assert
    part.Count.Should()
        .Be( 0 );

    part.IsReadOnly.Should()
        .BeFalse();
  }

  [Fact]
  public void Part_ShouldInitializeWithEvents_WhenEventsAreProvided()
  {
    // Arrange
    var pitch1 = Pitch.Create( PitchClass.C, 4 );
    var pitch2 = Pitch.Create( PitchClass.D, 4 );

    // Act
    var part = new Part( [pitch1, pitch2] );

    // Assert
    part.Count.Should()
        .Be( 2 );

    part[0]
      .Should()
      .Be( pitch1 );

    part[1]
      .Should()
      .Be( pitch2 );
  }

  [Fact]
  public void Part_ShouldThrowArgumentNullException_WhenEventsIsNull()
  {
    // Arrange
    IEnumerable<IPartEvent>? events = null;

    // Act
    var act = () => new Part( events! );

    // Assert
    act.Should()
       .Throw<ArgumentNullException>();
  }

  [Fact]
  public void RemoveAt_ShouldRemoveEventAtIndex_WhenCalled()
  {
    // Arrange
    var part = Part.Parse( "C4,D4,E4" );

    // Act
    part.RemoveAt( 1 );

    // Assert
    part.Count.Should()
        .Be( 2 );

    part[0]
      .Should()
      .Be( PitchClass.C[4] );

    part[1]
      .Should()
      .Be( PitchClass.E[4] );
  }

  [Fact]
  public void RemoveAt_ShouldRemoveFirstElement_WhenIndexIsZero()
  {
    // Arrange
    var part = Part.Parse( "C4,D4" );

    // Act
    part.RemoveAt( 0 );

    // Assert
    part.Count.Should()
        .Be( 1 );

    part[0]
      .Should()
      .Be( PitchClass.D[4] );
  }

  [Fact]
  public void RemoveAt_ShouldRemoveLastElement_WhenIndexIsCountMinusOne()
  {
    // Arrange
    var part = Part.Parse( "C4,D4" );

    // Act
    part.RemoveAt( part.Count - 1 );

    // Assert
    part.Count.Should()
        .Be( 1 );

    part[0]
      .Should()
      .Be( PitchClass.C[4] );
  }

  [Fact]
  public void Remove_ShouldReturnFalse_WhenEventDoesNotExist()
  {
    // Arrange
    var part = Part.Parse( "C4" );

    // Act
    var result = part.Remove( PitchClass.D[4] );

    // Assert
    result.Should()
          .BeFalse();

    part.Count.Should()
        .Be( 1 );
  }

  [Fact]
  public void Remove_ShouldReturnFalse_WhenPartIsEmpty()
  {
    // Arrange
    var part = new Part();

    // Act
    var result = part.Remove( PitchClass.C[4] );

    // Assert
    result.Should()
          .BeFalse();

    part.Count.Should()
        .Be( 0 );
  }

  [Fact]
  public void Remove_ShouldReturnTrueAndRemoveEvent_WhenEventExists()
  {
    // Arrange
    var part = Part.Parse( "C4,D4,E4" );

    // Act
    var result = part.Remove( PitchClass.D[4] );

    // Assert
    result.Should()
          .BeTrue();

    part.Count.Should()
        .Be( 2 );

    part[0]
      .Should()
      .Be( PitchClass.C[4] );

    part[1]
      .Should()
      .Be( PitchClass.E[4] );
  }

  [Fact]
  public void Remove_ShouldThrowArgumentNullException_WhenItemIsNull()
  {
    // Arrange
    var part = new Part();

    // Act
    var act = () => part.Remove( null! );

    // Assert
    act.Should()
       .Throw<ArgumentNullException>();
  }

  [Fact]
  public void TryParse_EmptySegments_ShouldBeIgnored()
  {
    var input = "C4, , , E4".AsSpan();
    var result = Part.TryParse( input, null, out var part, out var tail );

    result.Should()
          .BeTrue();

    part.Should()
        .HaveCount( 2 );

    part![0]
      .Should()
      .Be( Pitch.Parse( "C4" ) );

    part![1]
      .Should()
      .Be( Pitch.Parse( "E4" ) );

    tail.IsEmpty.Should()
        .BeTrue();
  }

  [Fact]
  public void TryParse_PrefersPitchChordOverPitch()
  {
    var input = "C, E, G".AsSpan();
    var result = Part.TryParse( input, null, out var part, out var tail );

    result.Should()
          .BeTrue();

    part![0]
      .Should()
      .BeOfType<PitchChord>();
  }

  [Fact]
  public void TryParse_ShouldHandlePitchAndPitchChord()
  {
    var input = "C4, Cmaj7, E4, Am".AsSpan();
    var result = Part.TryParse( input, null, out var part, out var tail );

    result.Should()
          .BeTrue();

    part.Should()
        .HaveCount( 4 );

    part![0]
      .Should()
      .Be( Pitch.Parse( "C4" ) );

    part![1]
      .Should()
      .Be( PitchChord.Parse( "Cmaj7" ) );

    part![2]
      .Should()
      .Be( Pitch.Parse( "E4" ) );

    part![3]
      .Should()
      .Be( PitchChord.Parse( "Am" ) );
  }

  [Fact]
  public void TryParse_ShouldHandlePitchChordWithBassNote()
  {
    var input = "Cmaj7/E, G4".AsSpan();
    var result = Part.TryParse( input, null, out var part, out var tail );

    result.Should()
          .BeTrue();

    part.Should()
        .HaveCount( 2 );
    var chord = (PitchChord) part![0];

    chord.Root.PitchClass.Should()
         .Be( PitchClass.C );

    chord.Inversion.Should()
         .Be( 1 ); // E is the 3rd of Cmaj7
  }

  [Theory]
  [InlineData( "C4", 1, "" )]
  [InlineData( "C4, E4, G4", 3, "" )]
  [InlineData( "Cmaj7", 1, "" )]
  [InlineData( "C4, Am, G5", 3, "" )]
  [InlineData( "", 0, "" )]
  [InlineData( "  ", 0, "" )]
  [InlineData( " C4 , E4 ", 2, " " )]
  [InlineData( "C4,,E4", 2, "" )]
  [InlineData( "C4, E4, ", 2, ", " )]
  public void TryParse_ShouldParseValidInputs(
    string input,
    int expectedCount,
    string expectedTail )
  {
    var result = Part.TryParse( input.AsSpan(), null, out var part, out var tail );

    result.Should()
          .BeTrue();

    part.Should()
        .NotBeNull();

    part!.Count.Should()
         .Be( expectedCount );

    tail.ToString()
        .Should()
        .Be( expectedTail );
  }

  [Theory]
  [InlineData( "invalid" )]
  [InlineData( "C4, invalid" )]
  public void TryParse_ShouldReturnFalseForInvalidInputs(
    string input )
  {
    var result = Part.TryParse( input.AsSpan(), null, out var part, out var tail );

    result.Should()
          .BeFalse();
  }

  [Fact]
  public void TryParse_WithOnlyCommas_ShouldReturnTrueAndEmptyPart()
  {
    var input = ", , ,".AsSpan();
    var result = Part.TryParse( input, null, out var part, out var tail );

    result.Should()
          .BeTrue();

    part.Should()
        .BeEmpty();

    // Since no events were parsed, tail should be the trimmed input.
    tail.ToString()
        .Should()
        .Be( ", , ," );
  }

  #endregion
}
