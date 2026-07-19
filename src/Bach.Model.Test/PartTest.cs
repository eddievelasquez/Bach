namespace Bach.Model.Test;

using System.Collections.Generic;
using System.Linq;
using FluentAssertions;

public sealed class PartTest
{
  [Fact]
  public void AddAndEnumerate_ShouldStoreEventsAndPreserveChordsAsSingleEvents()
  {
    var part = new Part();
    var pitch = Pitch.Create( PitchClass.C, 4 );
    var chord = new PitchChord( pitch, ChordFormula.Major );

    part.Add( pitch );
    part.Add( chord );

    part.Count.Should().Be( 2 );

    var events = part.ToArray();
    events.Should().HaveCount( 2 );
    events[0].Should().Be(pitch);
    events[1].Should().Be(chord);
  }

  [Fact]
  public void Part_ShouldInitializeEmptyList_WhenDefaultConstructorIsUsed()
  {
    // Arrange & Act
    var part = new Part();

    // Assert
    part.Count.Should().Be( 0 );
    part.IsReadOnly.Should().BeFalse();
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
    part.Count.Should().Be( 2 );
    part[0].Should().Be( pitch1 );
    part[1].Should().Be( pitch2 );
  }

  [Fact]
  public void Part_ShouldThrowArgumentNullException_WhenEventsIsNull()
  {
    // Arrange
    IEnumerable<IPartEvent>? events = null;

    // Act
    var act = () => new Part( events! );

    // Assert
    act.Should().Throw<ArgumentNullException>();
  }

  [Fact]
  public void Part_ShouldInitializeWithCapacity_WhenCapacityIsProvided()
  {
    // Arrange
    var part = new Part( 10 );

    // Act & Assert
    part.Count.Should().Be( 0 );
    part.IsReadOnly.Should().BeFalse();
  }

  [Fact]
  public void Count_ShouldReturnZero_WhenPartIsEmpty()
  {
    // Arrange
    var part = new Part();

    // Act
    var count = part.Count;

    // Assert
    count.Should().Be( 0 );
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
    count.Should().Be( 1 );
  }

  [Fact]
  public void IsReadOnly_ShouldReturnFalse()
  {
    // Arrange
    var part = new Part();

    // Act
    var isReadOnly = part.IsReadOnly;

    // Assert
    isReadOnly.Should().BeFalse();
  }

  [Fact]
  public void Add_ShouldThrowArgumentNullException_WhenPartEventIsNull()
  {
    // Arrange
    var part = new Part();

    // Act
    var act = () => part.Add( null! );

    // Assert
    act.Should().Throw<ArgumentNullException>();
  }

  [Fact]
  public void Clear_ShouldRemoveAllEvents_WhenCalled()
  {
    // Arrange
    var part = new Part();
    var pitch1 = Pitch.Create( PitchClass.C, 4 );
    var pitch2 = Pitch.Create( PitchClass.D, 4 );
    part.Add( pitch1 );
    part.Add( pitch2 );

    // Act
    part.Clear();

    // Assert
    part.Count.Should().Be( 0 );
  }

  [Fact]
  public void Clear_ShouldDoNothing_WhenPartIsEmpty()
  {
    // Arrange
    var part = new Part();

    // Act
    part.Clear();

    // Assert
    part.Count.Should().Be( 0 );
  }

  [Fact]
  public void Contains_ShouldReturnTrue_WhenEventExists()
  {
    // Arrange
    var part = new Part();
    var pitch = Pitch.Create( PitchClass.C, 4 );
    part.Add( pitch );

    // Act
    var result = part.Contains( pitch );

    // Assert
    result.Should().BeTrue();
  }

  [Fact]
  public void Contains_ShouldReturnFalse_WhenEventDoesNotExist()
  {
    // Arrange
    var part = new Part();
    var pitch1 = Pitch.Create( PitchClass.C, 4 );
    var pitch2 = Pitch.Create( PitchClass.D, 4 );
    part.Add( pitch1 );

    // Act
    var result = part.Contains( pitch2 );

    // Assert
    result.Should().BeFalse();
  }

  [Fact]
  public void Contains_ShouldReturnFalse_WhenPartIsEmpty()
  {
    // Arrange
    var part = new Part();
    var pitch = Pitch.Create( PitchClass.C, 4 );

    // Act
    var result = part.Contains( pitch );

    // Assert
    result.Should().BeFalse();
  }

  [Fact]
  public void Contains_ShouldThrowArgumentNullException_WhenItemIsNull()
  {
    // Arrange
    var part = new Part();

    // Act
    var act = () => part.Contains( null! );

    // Assert
    act.Should().Throw<ArgumentNullException>();
  }

  [Fact]
  public void CopyTo_ShouldCopyEventsToArray_WhenCalled()
  {
    // Arrange
    var part = new Part();
    var pitch1 = Pitch.Create( PitchClass.C, 4 );
    var pitch2 = Pitch.Create( PitchClass.D, 4 );
    part.Add( pitch1 );
    part.Add( pitch2 );
    var array = new IPartEvent[2];

    // Act
    part.CopyTo( array, 0 );

    // Assert
    array.Should().HaveCount( 2 );
    array[0].Should().Be( pitch1 );
    array[1].Should().Be( pitch2 );
  }

  [Fact]
  public void CopyTo_ShouldCopyEventsToArrayAtIndex_WhenArrayIndexIsProvided()
  {
    // Arrange
    var part = new Part();
    var pitch1 = Pitch.Create( PitchClass.C, 4 );
    var pitch2 = Pitch.Create( PitchClass.D, 4 );
    part.Add( pitch1 );
    part.Add( pitch2 );
    var array = new IPartEvent[4];

    // Act
    part.CopyTo( array, 2 );

    // Assert
    array[0].Should().BeNull();
    array[1].Should().BeNull();
    array[2].Should().Be( pitch1 );
    array[3].Should().Be( pitch2 );
  }

  [Fact]
  public void CopyTo_ShouldThrowArgumentNullException_WhenArrayIsNull()
  {
    // Arrange
    var part = new Part();

    // Act
    var act = () => part.CopyTo( null!, 0 );

    // Assert
    act.Should().Throw<ArgumentNullException>();
  }

  [Fact]
  public void GetEnumerator_ShouldReturnEnumerator_WhenCalled()
  {
    // Arrange
    var part = new Part();
    var pitch1 = Pitch.Create( PitchClass.C, 4 );
    var pitch2 = Pitch.Create( PitchClass.D, 4 );
    part.Add( pitch1 );
    part.Add( pitch2 );

    // Act
    var enumerator = part.GetEnumerator();

    // Assert
    enumerator.Should().NotBeNull();
    var events = new List<IPartEvent>();
    while ( enumerator.MoveNext() )
    {
      events.Add( enumerator.Current );
    }
    events.Should().HaveCount( 2 );
    events[0].Should().Be( pitch1 );
    events[1].Should().Be( pitch2 );
  }

  [Fact]
  public void GetEnumerator_ShouldReturnEmptyEnumerator_WhenPartIsEmpty()
  {
    // Arrange
    var part = new Part();

    // Act
    var enumerator = part.GetEnumerator();

    // Assert
    enumerator.Should().NotBeNull();
    enumerator.MoveNext().Should().BeFalse();
  }

  [Fact]
  public void IEnumerableGetEnumerator_ShouldReturnEnumerator_WhenCalled()
  {
    // Arrange
    var part = new Part();
    var pitch1 = Pitch.Create( PitchClass.C, 4 );
    var pitch2 = Pitch.Create( PitchClass.D, 4 );
    part.Add( pitch1 );
    part.Add( pitch2 );

    // Act
    var enumerator = ( (System.Collections.IEnumerable)part ).GetEnumerator();

    // Assert
    enumerator.Should().NotBeNull();
    var events = new List<IPartEvent>();
    while ( enumerator.MoveNext() )
    {
      events.Add( (IPartEvent)enumerator.Current );
    }
    events.Should().HaveCount( 2 );
    events[0].Should().Be( pitch1 );
    events[1].Should().Be( pitch2 );
  }

  [Fact]
  public void IndexOf_ShouldReturnCorrectIndex_WhenEventExists()
  {
    // Arrange
    var part = new Part();
    var pitch1 = Pitch.Create( PitchClass.C, 4 );
    var pitch2 = Pitch.Create( PitchClass.D, 4 );
    var pitch3 = Pitch.Create( PitchClass.E, 4 );
    part.Add( pitch1 );
    part.Add( pitch2 );
    part.Add( pitch3 );

    // Act
    var index = part.IndexOf( pitch2 );

    // Assert
    index.Should().Be( 1 );
  }

  [Fact]
  public void IndexOf_ShouldReturnMinusOne_WhenEventDoesNotExist()
  {
    // Arrange
    var part = new Part();
    var pitch1 = Pitch.Create( PitchClass.C, 4 );
    var pitch2 = Pitch.Create( PitchClass.D, 4 );
    part.Add( pitch1 );

    // Act
    var index = part.IndexOf( pitch2 );

    // Assert
    index.Should().Be( -1 );
  }

  [Fact]
  public void IndexOf_ShouldReturnMinusOne_WhenPartIsEmpty()
  {
    // Arrange
    var part = new Part();
    var pitch = Pitch.Create( PitchClass.C, 4 );

    // Act
    var index = part.IndexOf( pitch );

    // Assert
    index.Should().Be( -1 );
  }

  [Fact]
  public void IndexOf_ShouldThrowArgumentNullException_WhenItemIsNull()
  {
    // Arrange
    var part = new Part();

    // Act
    var act = () => part.IndexOf( null! );

    // Assert
    act.Should().Throw<ArgumentNullException>();
  }

  [Fact]
  public void Insert_ShouldInsertEventAtIndex_WhenCalled()
  {
    // Arrange
    var part = new Part();
    var pitch1 = Pitch.Create( PitchClass.C, 4 );
    var pitch2 = Pitch.Create( PitchClass.E, 4 );
    var pitch3 = Pitch.Create( PitchClass.D, 4 );
    part.Add( pitch1 );
    part.Add( pitch2 );

    // Act
    part.Insert( 1, pitch3 );

    // Assert
    part.Count.Should().Be( 3 );
    part[0].Should().Be( pitch1 );
    part[1].Should().Be( pitch3 );
    part[2].Should().Be( pitch2 );
  }

  [Fact]
  public void Insert_ShouldInsertEventAtBeginning_WhenIndexIsZero()
  {
    // Arrange
    var part = new Part();
    var pitch1 = Pitch.Create( PitchClass.C, 4 );
    var pitch2 = Pitch.Create( PitchClass.D, 4 );
    part.Add( pitch1 );

    // Act
    part.Insert( 0, pitch2 );

    // Assert
    part.Count.Should().Be( 2 );
    part[0].Should().Be( pitch2 );
    part[1].Should().Be( pitch1 );
  }

  [Fact]
  public void Insert_ShouldInsertEventAtEnd_WhenIndexIsCount()
  {
    // Arrange
    var part = new Part();
    var pitch1 = Pitch.Create( PitchClass.C, 4 );
    var pitch2 = Pitch.Create( PitchClass.D, 4 );
    part.Add( pitch1 );

    // Act
    part.Insert( part.Count, pitch2 );

    // Assert
    part.Count.Should().Be( 2 );
    part[0].Should().Be( pitch1 );
    part[1].Should().Be( pitch2 );
  }

  [Fact]
  public void Insert_ShouldThrowArgumentNullException_WhenItemIsNull()
  {
    // Arrange
    var part = new Part();

    // Act
    var act = () => part.Insert( 0, null! );

    // Assert
    act.Should().Throw<ArgumentNullException>();
  }

  [Fact]
  public void Remove_ShouldReturnTrueAndRemoveEvent_WhenEventExists()
  {
    // Arrange
    var part = new Part();
    var pitch1 = Pitch.Create( PitchClass.C, 4 );
    var pitch2 = Pitch.Create( PitchClass.D, 4 );
    var pitch3 = Pitch.Create( PitchClass.E, 4 );
    part.Add( pitch1 );
    part.Add( pitch2 );
    part.Add( pitch3 );

    // Act
    var result = part.Remove( pitch2 );

    // Assert
    result.Should().BeTrue();
    part.Count.Should().Be( 2 );
    part[0].Should().Be( pitch1 );
    part[1].Should().Be( pitch3 );
  }

  [Fact]
  public void Remove_ShouldReturnFalse_WhenEventDoesNotExist()
  {
    // Arrange
    var part = new Part();
    var pitch1 = Pitch.Create( PitchClass.C, 4 );
    var pitch2 = Pitch.Create( PitchClass.D, 4 );
    part.Add( pitch1 );

    // Act
    var result = part.Remove( pitch2 );

    // Assert
    result.Should().BeFalse();
    part.Count.Should().Be( 1 );
  }

  [Fact]
  public void Remove_ShouldReturnFalse_WhenPartIsEmpty()
  {
    // Arrange
    var part = new Part();
    var pitch = Pitch.Create( PitchClass.C, 4 );

    // Act
    var result = part.Remove( pitch );

    // Assert
    result.Should().BeFalse();
    part.Count.Should().Be( 0 );
  }

  [Fact]
  public void Remove_ShouldThrowArgumentNullException_WhenItemIsNull()
  {
    // Arrange
    var part = new Part();

    // Act
    var act = () => part.Remove( null! );

    // Assert
    act.Should().Throw<ArgumentNullException>();
  }

  [Fact]
  public void RemoveAt_ShouldRemoveEventAtIndex_WhenCalled()
  {
    // Arrange
    var part = new Part();
    var pitch1 = Pitch.Create( PitchClass.C, 4 );
    var pitch2 = Pitch.Create( PitchClass.D, 4 );
    var pitch3 = Pitch.Create( PitchClass.E, 4 );
    part.Add( pitch1 );
    part.Add( pitch2 );
    part.Add( pitch3 );

    // Act
    part.RemoveAt( 1 );

    // Assert
    part.Count.Should().Be( 2 );
    part[0].Should().Be( pitch1 );
    part[1].Should().Be( pitch3 );
  }

  [Fact]
  public void RemoveAt_ShouldRemoveFirstElement_WhenIndexIsZero()
  {
    // Arrange
    var part = new Part();
    var pitch1 = Pitch.Create( PitchClass.C, 4 );
    var pitch2 = Pitch.Create( PitchClass.D, 4 );
    part.Add( pitch1 );
    part.Add( pitch2 );

    // Act
    part.RemoveAt( 0 );

    // Assert
    part.Count.Should().Be( 1 );
    part[0].Should().Be( pitch2 );
  }

  [Fact]
  public void RemoveAt_ShouldRemoveLastElement_WhenIndexIsCountMinusOne()
  {
    // Arrange
    var part = new Part();
    var pitch1 = Pitch.Create( PitchClass.C, 4 );
    var pitch2 = Pitch.Create( PitchClass.D, 4 );
    part.Add( pitch1 );
    part.Add( pitch2 );

    // Act
    part.RemoveAt( part.Count - 1 );

    // Assert
    part.Count.Should().Be( 1 );
    part[0].Should().Be( pitch1 );
  }
}
