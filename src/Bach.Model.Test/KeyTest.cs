namespace Bach.Model.Test;

public sealed class KeyTest
{
  [Fact]
  public void Constructor_ShouldInitializeCorrectly_ForMajorKey()
  {
    var key = new Key( PitchClass.G, ModeType.Major );

    key.Tonic.Should()
       .Be( PitchClass.G );
    key.Mode.Should()
       .Be( ModeType.Major );
    key.KeySignature.AccidentalCount.Should()
       .Be( 1 );
    key.KeySignature.Accidental.Should()
       .Be( Accidental.Sharp );
    key.Scale.Should()
       .NotBeNull();
    key.Scale.Root.Should()
       .Be( PitchClass.G );
    key.Scale.Formula.Name.Should()
       .Be( "Major" );
  }

  [Fact]
  public void Constructor_ShouldInitializeCorrectly_ForMinorKey()
  {
    var key = new Key( PitchClass.E, ModeType.Minor );

    key.Tonic.Should()
       .Be( PitchClass.E );
    key.Mode.Should()
       .Be( ModeType.Minor );
    key.KeySignature.AccidentalCount.Should()
       .Be( 1 );
    key.KeySignature.Accidental.Should()
       .Be( Accidental.Sharp );
    key.Scale.Root.Should()
       .Be( PitchClass.E );
  }

  [Fact]
  public void ToString_ShouldRenderTonicAndMode()
  {
    var key = new Key( PitchClass.C, ModeType.Major );

    key.ToString()
       .Should()
       .Be( "C Major" );
  }
}
