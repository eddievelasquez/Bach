namespace Bach.Model.Test.Analysis;

public sealed class ChordEventTests
{
  [Fact]
  public void PitchChord_ShouldExposeChordMetadata_ViaIChordEvent()
  {
    var root = Pitch.Create( PitchClass.C, 4 );
    var chord = PitchChord.Create( root, ChordFormula.Major, inversion: 1 );

    (chord as IChordEvent).Should().NotBeNull();

    var chordEvent = (IChordEvent) chord;

    chordEvent.Root.Should().Be( root );
    chordEvent.Inversion.Should().Be( 1 );
    chordEvent.Formula.Should().Be( ChordFormula.Major );
    chordEvent.Bass.Should().Be( chord.Bass );
  }

  [Fact]
  public void InvertedChord_ShouldReportDistinctBass()
  {
    var root = Pitch.Create( PitchClass.G, 3 );
    var chord = PitchChord.Create( root, ChordFormula.Major, inversion: 2 );

    var chordEvent = (IChordEvent) chord;

    chordEvent.Bass.Should().Be( Pitch.Create( PitchClass.D, 4 ) );
  }

  [Fact]
  public void Part_ShouldAllowPatternMatching_ForChordEvents()
  {
    var part = Part.Parse( "C4,C" );

    IChordEvent? found = null;

    foreach( var ev in part )
    {
      if( ev is IChordEvent ce )
      {
        found = ce;
        break;
      }
    }

    found.Should().NotBeNull();
    found!.Formula.Should().Be( ChordFormula.Major );
  }
}
