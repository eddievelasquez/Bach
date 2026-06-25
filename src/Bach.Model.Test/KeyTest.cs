namespace Bach.Model.Test;

public sealed class KeyTest
{
  [Fact]
  public void Constructor_ShouldInitializeCorrectly_ForMajorKey()
  {
    var key = new Key( PitchClass.G, "Major", 1 );

    key.Tonic.Should()
       .Be( PitchClass.G );
    key.Mode.Should()
       .Be( "Major" );
    key.KeySignature.Should()
       .Be( 1 );
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
    var key = new Key( PitchClass.E, "Minor", 4 );

    key.Tonic.Should()
       .Be( PitchClass.E );
    key.Mode.Should()
       .Be( "Minor" );
    key.KeySignature.Should()
       .Be( 4 );
    key.Scale.Root.Should()
       .Be( PitchClass.E );
  }

  [Fact]
  public void Constructor_ShouldThrowArgumentNullException_WhenModeIsNull()
  {
    var act = () => new Key( PitchClass.C, null!, 0 );

    act.Should()
       .Throw<ArgumentNullException>();
  }

  [Fact]
  public void Constructor_ShouldThrowArgumentException_WhenModeIsEmpty()
  {
    var act = () => new Key( PitchClass.C, string.Empty, 0 );

    act.Should()
       .Throw<ArgumentException>();
  }

  [Fact]
  public void ToString_ShouldRenderTonicAndMode()
  {
    var key = new Key( PitchClass.C, "Major", 0 );

    key.ToString()
       .Should()
       .Be( "C Major" );
  }
}
