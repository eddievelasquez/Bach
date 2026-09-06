using Bach.Model.Analysis;

namespace Bach.Model.Test.Analysis;

public class ModalInterchangeTests
{
  [Fact]
  public void Ctor_Should_CreateRecord()
  {
    var target = AnalysisTarget.ForPitch( Pitch.Create( PitchClass.EFlat, 4 ) );
    var degree = ScaleDegree.Mediant;
    var evidence = new EvidenceReason( EvidenceReasonCategory.ScaleContext, "borrowed mediant" );

    var mi = new ModalInterchange( target, "aeolian", degree, evidence );

    mi.Target.Should().Be( target );
    mi.SourceScaleId.Should().Be( "aeolian" );
    mi.TargetDegree.Should().Be( degree );
    mi.Evidence.Should().Be( evidence );
  }

  [Fact]
  public void Ctor_Throws_OnInvalidSourceId()
  {
    var target = AnalysisTarget.ForPitch( Pitch.Create( PitchClass.EFlat, 4 ) );
    var degree = ScaleDegree.Mediant;
    var evidence = new EvidenceReason( EvidenceReasonCategory.AnalystObservation, "x" );

    Action a = () => new ModalInterchange( target, "", degree, evidence );
    a.Should().Throw<ArgumentException>();
  }
}
