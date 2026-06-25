namespace Bach.Model.Test;

public sealed class ChordProgressionTest
{
  [Fact]
  public void Constructor_ShouldInitializeWithChordSequence()
  {
    var progression = new ChordProgression(
      [
        new Chord( PitchClass.C, "Major" ),
        new Chord( PitchClass.G, "Major" )
      ] );

    progression.Chords.Should()
               .HaveCount( 2 );
    progression.Chords[0].Name.Should()
               .Be( "C" );
    progression.Chords[1].Name.Should()
               .Be( "G" );
  }

  [Fact]
  public void Constructor_ShouldThrowArgumentNullException_WhenChordsIsNull()
  {
    var act = () => new ChordProgression( null! );

    act.Should()
       .Throw<ArgumentNullException>();
  }

  [Fact]
  public void ToString_ShouldJoinChordNames()
  {
    var progression = new ChordProgression(
      [
        new Chord( PitchClass.C, "Major" ),
        new Chord( PitchClass.F, "Major" ),
        new Chord( PitchClass.G, "Major" )
      ] );

    progression.ToString()
               .Should()
               .Be( "C, F, G" );
  }
}
