// Module Name: FormulaTest.cs
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

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Xunit.Sdk;

namespace Bach.Model.Test;

public sealed class FormulaTests
{
  #region Nested Types

  // Minimal fake pitch-like implementation to exercise the unsupported branch in the generic Generate<TPitch>
  private readonly struct FakePitch: IPitch<FakePitch>
  {
    #region Properties

    public PitchClass PitchClass => PitchClass.C;
    public NoteName NoteName => NoteName.C;
    public Accidental Accidental => Accidental.Natural;

    #endregion

    #region Public Methods

    public int CompareTo(
      FakePitch other ) => 0;

    public bool Equals(
      FakePitch other ) => true;

    public static FakePitch Parse(
      ReadOnlySpan<char> s,
      IFormatProvider? provider )
    {
      return new FakePitch();
    }

    public static FakePitch Parse(
      string s,
      IFormatProvider? provider )
    {
      return new FakePitch();
    }

    public string ToString(
      string? format,
      IFormatProvider? provider ) => string.Empty;

    public FakePitch Transpose(
      int semitoneCount ) => this;

    public FakePitch Transpose(
      Interval interval ) => this;

    // ISpanParsable / ISpanConsumingParsable static abstract implementations
    public static bool TryParse(
      ReadOnlySpan<char> s,
      IFormatProvider? provider,
      out FakePitch result )
    {
      result = new FakePitch();
      return true;
    }

    public static bool TryParse(
      ReadOnlySpan<char> s,
      IFormatProvider? provider,
      out FakePitch result,
      out ReadOnlySpan<char> tail )
    {
      result = new FakePitch();
      tail = ReadOnlySpan<char>.Empty;
      return true;
    }

    public static bool TryParse(
      string? s,
      IFormatProvider? provider,
      out FakePitch result )
    {
      result = new FakePitch();
      return true;
    }

    #endregion
  }

  private sealed class TestFormula(
    string id,
    string name,
    params Interval[] intervals )
    : Formula( id, name, intervals );

  #endregion

  #region Fields

  private readonly Interval _majorSecond = Interval.MajorSecond;
  private readonly Interval _unison = Interval.Unison;

  #endregion

  #region Public Methods

  // Additional tests added by automated test generator

  [Fact]
  public void Constructor_ShouldInitializeProperties_WhenArgumentsAreValid()
  {
    // Arrange
    var id = "test-id";
    var name = "Test";
    var intervals = new[] { Interval.Unison, Interval.MajorThird };

    // Act
    var sut = new TestFormula( id, name, intervals );

    // Assert
    sut.Id.Should()
       .Be( id );

    sut.Name.Should()
       .Be( name );

    sut.Intervals.Should()
       .NotBeNull();

    sut.Intervals.Count.Should()
       .Be( 2 );

    sut.Intervals.SequenceEqual( intervals )
       .Should()
       .BeTrue();
  }

  [Fact]
  public void Constructor_ShouldThrowArgumentException_WhenIdIsNullOrEmpty()
  {
    // Arrange
    var name = "Test";
    var intervals = new[] { Interval.Unison };

    // Act
    Action actNull = () => new TestFormula( null!, name, intervals );
    Action actEmpty = () => new TestFormula( string.Empty, name, intervals );

    // Assert
    actNull.Should()
           .Throw<ArgumentException>();

    actEmpty.Should()
            .Throw<ArgumentException>();
  }

  [Fact]
  public void Constructor_ShouldThrowArgumentException_WhenNameIsNullOrEmpty()
  {
    // Arrange
    var id = "test";
    var intervals = new[] { Interval.Unison };

    // Act
    Action actNull = () => new TestFormula( id, null!, intervals );
    Action actEmpty = () => new TestFormula( id, string.Empty, intervals );

    // Assert
    actNull.Should()
           .Throw<ArgumentException>();

    actEmpty.Should()
            .Throw<ArgumentException>();
  }

  [Fact]
  public void Constructor_ShouldThrowArgumentNullException_WhenIntervalsIsNull()
  {
    // Arrange
    var id = "test";
    var name = "Test";

    // Act
    Action act = () => new TestFormula( id, name, null! );

    // Assert
    act.Should()
       .Throw<ArgumentNullException>();
  }

  [Fact]
  public void Constructor_ShouldThrowArgumentOutOfRangeException_WhenIntervalsIsEmpty()
  {
    // Arrange
    var id = "test";
    var name = "Test";
    var intervals = Array.Empty<Interval>();

    // Act
    Action act = () => new TestFormula( id, name, intervals );

    // Assert
    act.Should()
       .Throw<ArgumentOutOfRangeException>();
  }

  [Fact]
  public void Contains_ShouldReturnFalse_ForExactMatch_WhenSemitoneEqualButDifferentInterval()
  {
    // Arrange
    // MinorSecond and AugmentedFirst have the same semitone count (1) but are different intervals
    var intervals = new[] { Interval.MinorSecond };
    var sut = new TestFormula( "id", "name", intervals );

    // Act
    var exact = sut.Contains( new[] { Interval.AugmentedFirst } );
    var semitone = sut.Contains( new[] { Interval.AugmentedFirst }, IntervalMatch.Enharmonic );

    // Assert
    exact.Should()
         .BeFalse();

    semitone.Should()
            .BeTrue();
  }

  [Fact]
  public void Contains_ShouldReturnTrue_ForExactMatch_WhenIntervalExists()
  {
    // Arrange
    var intervals = new[] { Interval.Unison, Interval.MinorSecond };
    var sut = new TestFormula( "id", "name", intervals );

    // Act
    var result = sut.Contains( new[] { Interval.MinorSecond } );

    // Assert
    result.Should()
          .BeTrue();
  }

  [Fact]
  public void Contains_ShouldUseEnharmonicMatching_WhenRequested()
  {
    var formula = new TestFormula( "test", "Test", Interval.Unison, Interval.AugmentedFourth );

    formula.Contains( new[] { Interval.DiminishedFifth }, IntervalMatch.Enharmonic )
           .Should()
           .BeTrue();
  }

  [Fact]
  public void Equals_DifferentValues_ReturnsFalse()
  {
    // Arrange
    var a = new TestFormula( "id-a", "name", _unison );
    var b = new TestFormula( "id-b", "name", _unison );

    // Act
    var result = a.Equals( (object) b );

    // Assert
    result.Should()
          .BeFalse();
  }

  [Fact]
  public void Equals_NullOrDifferentType_ReturnsFalse()
  {
    // Arrange
    var f = new TestFormula( "id", "name", _unison );

    // Act / Assert
    f.Equals( "not a formula" )
     .Should()
     .BeFalse();
  }

  [Fact]
  public void Equals_SameReference_ReturnsTrue()
  {
    // Arrange
    var f = new TestFormula( "id", "name", _unison );

    // Act
    var result = f.Equals( (object) f );

    // Assert
    result.Should()
          .BeTrue();
  }

  [Fact]
  public void Equals_SameValues_ReturnsTrue()
  {
    // Arrange
    var a = new TestFormula( "id", "name", _unison, _majorSecond );
    var b = new TestFormula( "id", "name", _unison, _majorSecond );

    // Act
    var result = a.Equals( (object) b );

    // Assert
    result.Should()
          .BeTrue();

    a.Equals( b )
     .Should()
     .BeTrue();
  }

  [Fact]
  public void Equals_ShouldReturnFalse_WhenIdDiffers()
  {
    // Arrange
    var a = new TestFormula( "id-a", "name", Interval.Unison );
    var b = new TestFormula( "id-b", "name", Interval.Unison );

    // Act
    var result = a.Equals( b );

    // Assert
    result.Should()
          .BeFalse();
  }

  [Fact]
  public void Equals_ShouldReturnFalse_WhenIntervalsDiffer()
  {
    // Arrange
    var a = new TestFormula( "id", "name", Interval.Unison );
    var b = new TestFormula( "id", "name", Interval.MajorThird );

    // Act
    var result = a.Equals( b );

    // Assert
    result.Should()
          .BeFalse();
  }

  [Fact]
  public void Equals_ShouldReturnFalse_WhenNameDiffers()
  {
    // Arrange
    var a = new TestFormula( "id", "name-a", Interval.Unison );
    var b = new TestFormula( "id", "name-b", Interval.Unison );

    // Act
    var result = a.Equals( b );

    // Assert
    result.Should()
          .BeFalse();
  }

  [Fact]
  public void Equals_ShouldReturnFalse_WhenOtherIsNull()
  {
    // Arrange
    var sut = new TestFormula( "id", "name", Interval.Unison );

    // Act
    var result = sut.Equals( null );

    // Assert
    result.Should()
          .BeFalse();
  }

  [Fact]
  public void Equals_ShouldReturnTrue_WhenIdNameAndIntervalsAreEqual()
  {
    // Arrange
    var a = new TestFormula( "id", "name", Interval.Unison, Interval.MajorThird );
    var b = new TestFormula( "id", "name", Interval.Unison, Interval.MajorThird );

    // Act
    var result = a.Equals( b );

    // Assert
    result.Should()
          .BeTrue();
  }

  [Fact]
  public void Equals_ShouldReturnTrue_WhenSameReference()
  {
    // Arrange
    var sut = new TestFormula( "id", "name", Interval.Unison );

    // Act
    var result = sut.Equals( sut );

    // Assert
    result.Should()
          .BeTrue();
  }

  [Fact]
  public void Generate_Generic_UnsupportedType_ThrowsArgumentException()
  {
    // Arrange
    var formula = new TestFormula( "f", "name", _majorSecond );

    // Create a fake pitch-like type that implements IPitch<T> but is neither Pitch nor PitchClass
    var fake = new FakePitch();

    // Act
    Action act = () => _ = formula.Generate( fake )
                                  .ToArray();

    // Assert
    var ex = act.Should()
                .Throw<ArgumentException>()
                .Which;

    ex.ParamName.Should()
      .Be( "root" );

    ex.Message.Should()
      .Contain( "Unsupported pitch type." );
  }

  [Fact]
  public void Generate_Generic_WithPitchClass_InvokesPitchClassOverload()
  {
    // Arrange
    var formula = new TestFormula( "f", "name", _majorSecond );
    var root = PitchClass.C;

    // Act
    var seq = formula.Generate<PitchClass>( root )
                     .ToArray();

    // Assert: first element equals root + interval
    seq.Should()
       .NotBeEmpty();

    seq[0]
      .Should()
      .Be( root + _majorSecond );
  }

  [Fact]
  public void Generate_PitchClass_ProducesFiniteSequence()
  {
    // Arrange
    var formula = new TestFormula( "f", "name", _unison );
    var root = PitchClass.C;

    // Act
    var seq = formula.Generate( root )
                     .Take( 10 )
                     .ToArray();

    // Assert: should be able to enumerate and produce pitch classes
    seq.Should()
       .HaveCountGreaterOrEqualTo( 1 );
  }

  [Fact]
  public void Generate_Pitch_StartNearMax_ReturnsExpectedAndStops()
  {
    // Arrange: use F9 so that adding a major second yields G9 (MaxValue)
    var formula = new TestFormula( "f", "name", _majorSecond );
    var root = Pitch.Create( PitchClass.F, Pitch.MaxOctave );

    // Act
    var seq = formula.Generate( root )
                     .ToArray();

    // Assert: should produce at least one pitch and the last should be MaxValue
    seq.Should()
       .NotBeEmpty();

    seq[0]
      .Should()
      .Be( Pitch.MaxValue );
  }

  [Fact]
  public void Generate_Static_WithIntervals_Null_Throws()
  {
    // Arrange
    var root = PitchClass.C;

    // Act
    Action act = () => _ = Formula.Generate( root, null! );

    // Assert
    act.Should()
       .Throw<ArgumentNullException>();
  }

  [Fact]
  public void Generate_Static_WithIntervals_ProjectsIntervals()
  {
    // Arrange
    var root = PitchClass.C;
    var intervals = new[] { Interval.Unison, Interval.MajorThird };

    // Act
    var result = Formula.Generate( root, intervals )
                        .ToArray();

    // Assert
    result.Should()
          .HaveCount( intervals.Length );

    for( var i = 0; i < intervals.Length; i++ )
    {
      result[i]
        .Should()
        .Be( root + intervals[i] );
    }
  }

  [Fact]
  public void GetHashCode_ReturnsOrdinalIgnoreCaseHash_WhenCalled()
  {
    // Arrange
    var id = "AbC";
    var f = new TestFormula( id, "name", _unison );

    // Act
    var result = f.GetHashCode();

    // Assert
    result.Should()
          .Be( StringComparer.OrdinalIgnoreCase.GetHashCode( id ) );
  }

  [Fact]
  public void GetRelativeSteps_ReturnsExpectedSteps_ForMultipleIntervals()
  {
    // Arrange
    var f = new TestFormula( "f", "name", Interval.Unison, Interval.MajorThird, Interval.Fifth );

    // Act
    var steps = f.GetRelativeSteps();

    // Assert
    steps.Should()
         .Equal( 4, 3, 5 );
  }

  [Fact]
  public void GetRelativeSteps_ReturnsTwelve_WhenOnlyUnison()
  {
    // Arrange
    var f = new TestFormula( "f", "name", Interval.Unison );

    // Act
    var steps = f.GetRelativeSteps();

    // Assert
    steps.Should()
         .Equal( 12 );
  }

  [Fact]
  public void GetRelativeSteps_ShouldReturnExpectedSteps_WhenMultipleIntervals()
  {
    // Arrange
    var intervals = new List<Interval>
    {
      Interval.Unison,
      Interval.MajorThird,
      Interval.Fifth
    };

    // Act
    var steps = Formula.GetRelativeSteps( intervals );

    // Assert
    steps.Should()
         .Equal( 4, 3, 5 );
  }

  [Fact]
  public void GetRelativeSteps_ShouldReturnSemitoneStepsBetweenIntervals()
  {
    var formula = new TestFormula(
      "test",
      "Test",
      Interval.Unison,
      Interval.MajorSecond,
      Interval.MajorThird,
      Interval.Fourth,
      Interval.Fifth
    );

    formula.GetRelativeSteps()
           .Should()
           .Equal( 2, 2, 1, 2, 5 );
  }

  [Fact]
  public void GetRelativeSteps_WithEmpty_ShouldThrowArgumentOutOfRangeException()
  {
    // Act
    Action act = () => _ = Formula.GetRelativeSteps( new List<Interval>() );

    // Assert
    act.Should()
       .Throw<ArgumentOutOfRangeException>();
  }

  [Fact]
  public void GetRelativeSteps_WithNull_ShouldThrowArgumentNullException()
  {
    // Act
    Action act = () => _ = Formula.GetRelativeSteps( null! );

    // Assert
    act.Should()
       .Throw<ArgumentNullException>();
  }

  [Fact]
  public void ParseIntervals_ShouldParseIntervalList_WhenFormulaUsesCSV()
  {
    var intervals = Formula.ParseIntervals( "R,M2,M3" );

    intervals.Should()
             .Equal( Interval.Unison, Interval.MajorSecond, Interval.MajorThird );
  }

  [Fact]
  public void ParseIntervals_ShouldThrowFormatException_WhenFormulaContainsInvalidInterval()
  {
    var act = () => Formula.ParseIntervals( "R,ZZ" );

    act.Should()
       .Throw<FormatException>()
       .WithMessage( "ZZ is not a valid interval" );
  }

  [Fact]
  public void ParseIntervals_Span_Empty_ReturnsEmptyArray()
  {
    // Act
    var result = Formula.ParseIntervals( ReadOnlySpan<char>.Empty );

    // Assert
    result.Should()
          .NotBeNull()
          .And.BeEmpty();
  }

  [Fact]
  public void ParseIntervals_Span_InvalidToken_ThrowsFormatException()
  {
    // Arrange
    var input = "x".AsSpan();

    // Act
    // Act
    try
    {
      _ = Formula.ParseIntervals( input );
      throw new XunitException( "Expected FormatException was not thrown" );
    }
    catch( FormatException ex )
    {
      // Assert
      ex.Message.Should()
        .Contain( "x is not a valid interval" );
    }
  }

  [Fact]
  public void ParseIntervals_Span_Valid_ReturnsParsedIntervals()
  {
    // Arrange
    var input = "1,3".AsSpan();

    // Act
    var result = Formula.ParseIntervals( input );

    // Assert
    result.Should()
          .Equal( Interval.Unison, Interval.MajorThird );
  }

  [Fact]
  public void ParseIntervals_String_Null_ThrowsArgumentNullException()
  {
    // Act
    Action act = () => _ = Formula.ParseIntervals( null! );

    // Assert
    act.Should()
       .Throw<ArgumentNullException>();
  }

  [Fact]
  public void ScaleFormula_ShouldExposeCategoriesAliasesAndFlags()
  {
    var formula = new ScaleFormula(
      "custom",
      "Custom",
      new[] { Interval.Unison, Interval.MajorSecond },
      new HashSet<string>
      {
        ScaleCategory.Diatonic,
        ScaleCategory.Major
      },
      new HashSet<string> { "custom-name" }
    );

    formula.Categories.Should()
           .Contain( ScaleCategory.Diatonic )
           .And.Contain( ScaleCategory.Major );

    formula.Aliases.Should()
           .Contain( "custom-name" );

    formula.IsDiatonic.Should()
           .BeTrue();

    formula.IsMajor.Should()
           .BeTrue();

    formula.IsMinor.Should()
           .BeFalse();
  }

  [Fact]
  public void ToString_CustomFormat_ShouldIncludeLiteralsAndPlaceholders_WhenCalled()
  {
    // Arrange
    var f = new TestFormula( "id", "MyName", Interval.Unison, Interval.MajorThird );

    // Act
    var s = f.ToString( "N-I:", null );

    // Assert
    var parts = f.ToString()
                 .Split( ':' );

    var expected = parts[0]
                   + "-"
                   + parts[1]
                     .Trim()
                   + ":";

    s.Should()
     .Be( expected );
  }

  [Fact]
  public void ToString_NullFormat_ShouldUseDefaultFormat_WhenCalled()
  {
    // Arrange
    var f = new TestFormula( "id", "MyName", Interval.Unison, Interval.MajorThird );

    // Act
    var s = f.ToString( null, CultureInfo.InvariantCulture );

    // Assert
    s.Should()
     .Be( f.ToString() );
  }

  [Fact]
  public void ToString_ReturnsNameAndIntervals_WhenDefault()
  {
    // Arrange
    var f = new TestFormula( "id", "MyName", Interval.Unison, Interval.MajorThird );

    // Act
    var s = f.ToString();

    // Assert
    s.Should()
     .Be( "MyName: 1,3" );
  }

  [Fact]
  public void ToString_ShouldHonorFormatSpecifier_WhenCustomFormatIsProvided()
  {
    var formula = new TestFormula( "test", "Test", Interval.Unison, Interval.MajorThird );

    formula.ToString( "N:I" )
           .Should()
           .Be( "Test:1,3" );
  }

  [Fact]
  public void ToString_String_ShouldDelegateToTwoArgOverload_WhenCalled()
  {
    // Arrange
    var f = new TestFormula( "id", "MyName", Interval.Unison, Interval.MajorThird );
    var format = "N|I";

    // Act
    var oneArg = f.ToString( format );
    var twoArg = f.ToString( format, null );

    // Assert
    oneArg.Should()
          .Be( twoArg );
  }

  #endregion
}
