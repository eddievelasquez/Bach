namespace Bach.Model.Test;

public sealed class ScaleDegreeTest
{
  [Fact]
  public void Resolve_ShouldReturnTonicPitchClass_ForMajorKey()
  {
    var key = new Key( PitchClass.C, ModeType.Major );
    var degree = ScaleDegree.Tonic;

    degree.Resolve( key )
          .Should()
          .Be( PitchClass.C );
  }

  [Fact]
  public void Resolve_ShouldReturnExpectedPitchClass_ForMinorKey()
  {
    var key = new Key( PitchClass.A, ModeType.Minor );
    var degree = ScaleDegree.Mediant;

    degree.Resolve( key )
          .Should()
          .Be( PitchClass.C );
  }

  [Fact]
  public void Resolve_ShouldThrowArgumentNullException_WhenKeyIsNull()
  {
    var degree = ScaleDegree.Dominant;

    var act = () => degree.Resolve( null! );

    act.Should()
       .Throw<ArgumentNullException>();
  }

  [Fact]
  public void ToString_ShouldReturnRomanNumeralSymbol()
  {
    ScaleDegree.Tonic.ToString()
               .Should()
               .Be( "I" );

    ScaleDegree.Dominant.ToString()
               .Should()
               .Be( "V" );
  }
}
