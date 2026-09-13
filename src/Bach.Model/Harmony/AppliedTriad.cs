// Module Name: AppliedTriad.cs
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

namespace Bach.Model.Harmony;

/// <summary>
///   Represents a triad applied to a target scale degree.
/// </summary>
public sealed class AppliedTriad
{
  #region Constructors

  /// <summary>
  ///   Initializes an applied triad.
  /// </summary>
  /// <param name="triad">
  ///   The applied triad.
  /// </param>
  /// <param name="targetDegree">
  ///   The degree receiving the applied function.
  /// </param>
  /// <param name="function">
  ///   The applied function.
  /// </param>
  public AppliedTriad(
    Triad triad,
    ScaleDegree targetDegree,
    AppliedTriadFunction function )
  {
    Triad = triad ?? throw new ArgumentNullException( nameof( triad ) );

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
  ///   Gets the applied triad.
  /// </summary>
  public Triad Triad { get; }

  /// <summary>
  ///   Gets the target degree.
  /// </summary>
  public ScaleDegree TargetDegree { get; }

  /// <summary>
  ///   Gets the applied function.
  /// </summary>
  public AppliedTriadFunction Function { get; }

  #endregion
}
