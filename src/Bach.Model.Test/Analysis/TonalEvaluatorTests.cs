// Module Name: TonalEvaluatorTests.cs
// Project:     Bach.Model.Test

using FluentAssertions;
using System.Linq;

namespace Bach.Model.Analysis.Test;

public class TonalEvaluatorTests
{
  [Fact]
  public void Evaluate_NoEvents_ReturnsInconclusive()
  {
    var part = new Part();
    var scope = new PartEventScope( part );
    var evaluator = new TonalEvaluator();

    var result = evaluator.Evaluate( scope );

    result.Should().BeOfType<InconclusiveTonalAnalysisResult>();
  }

  [Fact]
  public void CandidateSource_DefaultCandidates_NotEmpty()
  {
    var candidates = TonalCandidateSource.GetDefaultCandidates().Take( 5 ).ToArray();
    candidates.Should().NotBeEmpty();
    candidates.All( k => k is not null ).Should().BeTrue();
  }

  [Fact]
  public void RepertoireProfile_CommonPractice_EnablesCommonPracticeTags()
  {
    var profile = RepertoireProfile.CommonPractice;

    profile.GetWeight( ScaleTag.CommonPractice ).Should().Be( 1.0 );
    profile.GetWeight( ScaleTag.Modal ).Should().Be( 0.0 );
  }

  [Fact]
  public void Evaluate_ChordEvent_IncludesHarmonicEvidence()
  {
    var chord = PitchChord.Create( PitchClass.C, ChordFormula.Major );
    var scope = new PartEventScope( new Part( new IPartEvent[] { chord } ) );

    var result = new TonalEvaluator( RepertoireProfile.CommonPractice ).Evaluate( scope );

    result.Should().BeOfType<TonalAnalysisResultSet>();
    var candidates = (TonalAnalysisResultSet) result;
    candidates.Candidates.Should()
              .Contain( candidate => candidate.SupportingEvidence.Any( e => e.Category == EvidenceReasonCategory.HarmonicContext ) );
  }

  [Fact]
  public void Evaluate_ChordEvent_KeepsConfidenceInRange()
  {
    var chord = PitchChord.Create( PitchClass.C, ChordFormula.Major, inversion: 1 );
    var scope = new PartEventScope( new Part( new IPartEvent[] { chord } ) );

    var result = new TonalEvaluator().Evaluate( scope );

    result.Should().BeOfType<TonalAnalysisResultSet>();
    ((TonalAnalysisResultSet) result).Candidates.Should()
      .OnlyContain( candidate => candidate.Confidence >= 0.0 && candidate.Confidence <= 1.0 );
  }
}
