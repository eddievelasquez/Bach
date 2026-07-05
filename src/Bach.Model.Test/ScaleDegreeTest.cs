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
  public void Parse_ShouldThrowArgumentException_WhenValueIsNullOrWhitespace()
  {
    var act = () => ScaleDegree.Parse( "   " );

    act.Should()
       .Throw<ArgumentException>();
  }

  [Fact]
  public void Parse_ShouldThrowFormatException_WhenValueIsInvalid()
  {
    var act = () => ScaleDegree.Parse( "X" );

    act.Should()
       .Throw<FormatException>()
       .WithMessage( "The value 'X' is not a valid scale degree." );
  }

  [Fact]
  public void ResolveDiatonicTriad_ShouldReturnExpectedQuality_ForMajorMode()
  {
    var degree = ScaleDegree.Tonic;
    var key = new Key( PitchClass.C, ModeType.Major );

    var triad = degree.ResolveDiatonicTriad( key );

    triad.Root.Should()
         .Be( PitchClass.C );

    triad.Quality.Should()
         .Be( TriadQuality.Major );
  }

  [Fact]
  public void TryParse_ShouldReturnExpectedValue_ForNashvilleAndRomanNumeralInput()
  {
    ScaleDegree.TryParse( "5", out var nashville ).Should().BeTrue();
    nashville.Should().Be( ScaleDegree.Dominant );

    ScaleDegree.TryParse( "vi", out var roman ).Should().BeTrue();
    roman.Should().Be( ScaleDegree.Submediant );
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
