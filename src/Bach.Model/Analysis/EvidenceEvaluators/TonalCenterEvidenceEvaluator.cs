// Module Name: TonalCenterEvidenceEvaluator.cs
// Project:     Bach.Model
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
using System.Linq;

namespace Bach.Model.Analysis.EvidenceEvaluators;

/// <summary>
///   Provides ordered-event tonal-center evidence and its numeric score contribution.
/// </summary>
internal sealed class TonalCenterEvidenceEvaluator: TonalEvidenceEvaluator
{
  #region Constructors

  /// <summary>
  ///   Initializes a new instance of the <see cref="TonalCenterEvidenceEvaluator"/> class.
  /// </summary>
  public TonalCenterEvidenceEvaluator()
  {
  }

  #endregion

  #region Properties

  /// <inheritdoc/>
  public override int Priority => 40;

  #endregion

  #region Public Methods

  /// <inheritdoc/>
  public override IEnumerable<TonalEvidence> Evaluate(
    TonalEvidenceContext context )
  {
    ArgumentNullException.ThrowIfNull( context );

    var tonic = context.CandidateKey.Tonic;
    var scale = context.CandidateKey.Scale;
    var events = context.Events;

    var chordEvents = events.OfType<IChordEvent>()
                            .ToArray();
    var totalEvents = events.Count;
    var tonicOccurrences = events.Count( e => e.Any( tonic ) );
    var tonicRootOccurrences = chordEvents.Count( c => c.Root.PitchClass == tonic );
    var tonicBassOccurrences = chordEvents.Count( c => c.Bass.PitchClass == tonic );
    var reasons = new List<(EvidenceReason Reason, double Contribution)>();

    // Add reasons for tonic occurrences, tonic root occurrences, and tonic bass occurrences
    AddReasonIfPositive(
      tonicOccurrences,
      EvidenceReasonCategory.TonalCenter,
      $"Tonic {tonic} occurs {tonicOccurrences} times."
    );

    AddReasonIfPositive(
      tonicRootOccurrences,
      EvidenceReasonCategory.TonalCenter,
      $"Tonic root {tonic} occurs {tonicRootOccurrences} times."
    );

    AddReasonIfPositive(
      tonicBassOccurrences,
      EvidenceReasonCategory.TonalCenter,
      $"Tonic bass {tonic} occurs {tonicBassOccurrences} times."
    );

    // Add reasons for opening and closing emphasis on the tonic
    if( totalEvents > 0 )
    {
      if( events[0]
         .Any( tonic ) )
      {
        AddReason(
          EvidenceReasonCategory.TonalCenterOpeningEmphasis,
          $"First event emphasizes tonic {tonic}.",
          context.Options.OpeningEmphasisWeight
        );
      }

      if( events[^1]
         .Any( tonic ) )
      {
        AddReason(
          EvidenceReasonCategory.TonalCenterClosingEmphasis,
          $"Last event emphasizes tonic {tonic}.",
          context.Options.ClosingEmphasisWeight
        );
      }
    }

    var degrees = GetDegrees( scale );
    var dominant = degrees.Length >= 5 ? degrees[4] : (PitchClass?) null;
    var leading = degrees.Length >= 7 ? degrees[6] : (PitchClass?) null;
    var dominantToTonic = 0;
    var measureBoundaryDominantToTonic = 0;
    var leadingToTonic = 0;
    var locations = context.Scope.Locations.ToArray();

    // Count dominant-to-tonic and leading-tone-to-tonic motions
    for( var index = 0; index + 1 < totalEvents; ++index )
    {
      var currentEvent = events[index];
      var nextEvent = events[index + 1];

      if( dominant is not null && currentEvent.Any( dominant.Value ) && nextEvent.Any( tonic ) )
      {
        ++dominantToTonic;

        if( locations[index].MeasureIndex != locations[index + 1].MeasureIndex )
        {
          ++measureBoundaryDominantToTonic;
        }
      }

      if( leading is not null && currentEvent.Any( leading.Value ) && nextEvent.Any( tonic ) )
      {
        ++leadingToTonic;
      }
    }

    // Add reasons for dominant-to-tonic and leading-tone-to-tonic motions
    AddReasonIfPositive(
      dominantToTonic,
      EvidenceReasonCategory.DominantToTonicMotion,
      $"Detected {dominantToTonic} dominant-to-tonic motion(s)."
    );

    AddReasonIfPositive(
      measureBoundaryDominantToTonic,
      EvidenceReasonCategory.DominantToTonicMotion,
      $"Detected {measureBoundaryDominantToTonic} dominant-to-tonic motion(s) across measure boundaries."
    );

    AddReasonIfPositive(
      leadingToTonic,
      EvidenceReasonCategory.LeadingToneResolution,
      $"Detected {leadingToTonic} leading-tone resolution(s) to tonic."
    );

    var score = 0.0;

    // Calculate score contributions based on tonic occurrences and motions
    if( chordEvents.Length > 0 )
    {
      score += (double) tonicRootOccurrences / chordEvents.Length * context.Options.TonicRootRecurrenceWeight;
      score += (double) tonicBassOccurrences / chordEvents.Length * context.Options.TonicBassRecurrenceWeight;
    }

    score += (double) tonicOccurrences / Math.Max( 1, totalEvents ) * ( context.Options.TonicRootRecurrenceWeight * 0.5 );

    // Add score contributions for opening and closing emphasis on the tonic
    if( totalEvents > 0 )
    {
      score += events[0]
        .Any( tonic )
        ? context.Options.OpeningEmphasisWeight
        : 0.0;

      score += events[^1]
        .Any( tonic )
        ? context.Options.ClosingEmphasisWeight
        : 0.0;
    }

    // Add score contributions for dominant-to-tonic and leading-tone-to-tonic motions
    if( totalEvents > 1 )
    {
      score += (double) dominantToTonic / ( totalEvents - 1 ) * context.Options.DominantToTonicWeight;
      score += (double) measureBoundaryDominantToTonic / ( totalEvents - 1 ) * context.Options.DominantToTonicWeight * 0.5;
      score += (double) leadingToTonic / ( totalEvents - 1 ) * context.Options.LeadingToneResolutionWeight;
    }

    score = Math.Min( score, 0.35 );

    // Yield the reasons and their contributions as TonalEvidence
    for( var index = 0; index < reasons.Count; ++index )
    {
      var (reason, contribution) = reasons[index];
      yield return new TonalEvidence( reason, true, index == 0 ? score : contribution );
    }

    yield break;

    void AddReasonIfPositive(
      int value,
      EvidenceReasonCategory category,
      string explanation,
      double contribution = 0.0 )
    {
      if( value > 0 )
      {
        AddReason( category, explanation, contribution );
      }
    }

    void AddReason(
      EvidenceReasonCategory category,
      string explanation,
      double contribution = 0.0 )
    {
      reasons.Add( ( new EvidenceReason( category, explanation ), contribution ) );
    }
  }

  #endregion
}
