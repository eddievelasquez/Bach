// Module Name: AppliedFunction.cs
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

namespace Bach.Model.Analysis;

/// <summary>
///   Represents an applied harmonic function assigned to a musical event.
/// </summary>
public sealed record AppliedFunction
{
  #region Constructors

  /// <summary>
  ///   Initializes an applied function.
  /// </summary>
  /// <param name="target">The event or pitch that expresses the applied function.</param>
  /// <param name="targetDegree">The degree toward which the function points.</param>
  /// <param name="function">The type of applied function.</param>
  /// <param name="evidence">The reason that supports the classification.</param>
  public AppliedFunction(
    AnalysisTarget target,
    ScaleDegree targetDegree,
    AppliedTriadFunction function,
    EvidenceReason evidence )
  {
    Target = target ?? throw new ArgumentNullException( nameof( target ) );
    Evidence = evidence ?? throw new ArgumentNullException( nameof( evidence ) );

    if( targetDegree.Degree is < 1 or > 7 )
    {
      throw new ArgumentOutOfRangeException( nameof( targetDegree ), "The target degree must be between 1 and 7." );
    }

    TargetDegree = targetDegree;
    Function = function;
  }

  #endregion

  #region Properties

  /// <summary>
  ///   Gets the event or pitch that expresses the applied function.
  /// </summary>
  public AnalysisTarget Target { get; }

  /// <summary>
  ///   Gets the degree toward which the function points.
  /// </summary>
  public ScaleDegree TargetDegree { get; }

  /// <summary>
  ///   Gets the type of applied function.
  /// </summary>
  public AppliedTriadFunction Function { get; }

  /// <summary>
  ///   Gets the reason that supports the classification.
  /// </summary>
  public EvidenceReason Evidence { get; }

  /// <summary>
  ///   Gets the conventional applied-function label.
  /// </summary>
  public string Label => Function switch
  {
    AppliedTriadFunction.Dominant => $"V/{TargetDegree.Symbol}",
    AppliedTriadFunction.LeadingTone => $"vii°/{TargetDegree.Symbol}",
    _ => throw new InvalidOperationException( $"The applied function '{Function}' is not supported." )
  };

  #endregion

  #region Public Methods

  /// <summary>
  ///   Returns the conventional applied-function label.
  /// </summary>
  /// <returns>The conventional applied-function label.</returns>
  public override string ToString()
  {
    return Label;
  }

  #endregion
}
