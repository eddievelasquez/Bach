// Module Name: TonalAnalysisResult.cs
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
///   Base type for tonal analysis results.
/// </summary>
/// <remarks>
///   Results use an ordered, duration-free <see cref="PartEventScope"/> that retains measure locations.
/// </remarks>
public abstract record TonalAnalysisResult
{
  #region Constructors

  /// <summary>
  ///   Initializes a tonal analysis result with an ordered event scope.
  /// </summary>
  /// <param name="scope">The immutable part and range examined by the analysis.</param>
  /// <exception cref="ArgumentNullException">Thrown when <paramref name="scope"/> is null.</exception>
  protected TonalAnalysisResult(
    PartEventScope scope )
  {
    Scope = scope ?? throw new ArgumentNullException( nameof( scope ) );
  }

  #endregion

  #region Properties

  /// <summary>
  ///   Gets the ordered scope used for the analysis.
  /// </summary>
  /// <remarks>
  ///   The scope contains an immutable source part and a standard .NET event range.
  /// </remarks>
  public PartEventScope Scope { get; init; }

  #endregion
}
