// Module Name: TonalEvaluationOptions.cs
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

using System.Runtime.CompilerServices;

namespace Bach.Model.Analysis;

/// <summary>
///   Controls how the evaluator selects ranked tonal candidates for its result.
/// </summary>
public sealed record TonalEvaluationOptions
{
  #region Properties

  /// <summary>
  ///   Gets options that return only the highest-ranked candidate.
  /// </summary>
  public static TonalEvaluationOptions Default => new();

  /// <summary>
  ///   Gets options that return every candidate that matches at least one observed pitch class.
  /// </summary>
  public static TonalEvaluationOptions AllCandidates => new() { IncludeAllCandidates = true };

  /// <summary>
  ///   Gets or initializes a value indicating whether to return all scored candidates.
  /// </summary>
  public bool IncludeAllCandidates { get; init; }

  /// <summary>
  ///   Gets or initializes the maximum number of candidates to return when all candidates are not included.
  /// </summary>
  public int MaximumCandidates { get; init; } = 1;

  /// <summary>
  ///   Gets or initializes the maximum confidence difference from the highest-ranked candidate.
  /// </summary>
  public double ConfidenceDelta { get; init; }

  /// <summary>
  ///   Weight for tonic root recurrence scoring. Applied as a maximum contribution when many tonic roots are observed.
  /// </summary>
  public double TonicRootRecurrenceWeight { get; init; } = 0.06;

  /// <summary>
  ///   Weight for tonic bass recurrence scoring.
  /// </summary>
  public double TonicBassRecurrenceWeight { get; init; } = 0.05;

  /// <summary>
  ///   Weight when the first event emphasizes the tonic.
  /// </summary>
  public double OpeningEmphasisWeight { get; init; } = 0.04;

  /// <summary>
  ///   Weight when the last event emphasizes the tonic.
  /// </summary>
  public double ClosingEmphasisWeight { get; init; } = 0.06;

  /// <summary>
  ///   Weight for detected dominant-to-tonic motion.
  /// </summary>
  public double DominantToTonicWeight { get; init; } = 0.06;

  /// <summary>
  ///   Weight for detected leading-tone resolution to tonic.
  /// </summary>
  public double LeadingToneResolutionWeight { get; init; } = 0.05;

  #endregion

  #region Implementation

  internal void Validate()
  {
    if( MaximumCandidates < 1 )
    {
      throw new ArgumentOutOfRangeException( nameof( MaximumCandidates ), "MaximumCandidates must be at least one." );
    }

    ValidateDouble( ConfidenceDelta, 0.0 );
    ValidateDouble( TonicRootRecurrenceWeight, 0.0 );
    ValidateDouble( TonicBassRecurrenceWeight, 0.0 );
    ValidateDouble( OpeningEmphasisWeight, 0.0 );
    ValidateDouble( ClosingEmphasisWeight, 0.0 );
    ValidateDouble( DominantToTonicWeight, 0.0 );
    ValidateDouble( LeadingToneResolutionWeight, 0.0 );

    return;

    static void ValidateDouble(
      double value,
      double minimum,
      [CallerArgumentExpression( nameof( value ) )] string? paramName = null )
    {
      if( !( !double.IsNaN( value ) && !double.IsInfinity( value ) && value >= minimum && value <= 1.0 ) )
      {
        throw new ArgumentOutOfRangeException( paramName, $"Value must be finite and between {minimum} and 1.0." );
      }
    }
  }

  #endregion
}
