// Module Name: Notation.cs
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

namespace Bach.Model;

/// <summary>
///   Provides notation symbols and names for musical intervals.
/// </summary>
/// <remarks>
///   This class contains static properties that represent various musical interval qualities,
///   including their symbols, short names, and long names. These properties can be used
///   to standardize the representation of interval qualities in musical applications.
/// </remarks>
public static class Notation
{
  #region Properties

  /// <summary>
  ///   Gets or sets the symbol representing a perfect interval quality.
  /// </summary>
  /// <value>
  ///   A <see cref="char"/> that represents the perfect interval quality. The default value is 'P'.
  /// </value>
  /// <remarks>
  ///   This property is used to standardize the representation of perfect interval qualities
  ///   in musical notation. It is commonly used in conjunction with other interval quality symbols
  ///   to construct interval representations.
  /// </remarks>
  public static char PerfectSymbol { get; set; } = 'P';

  /// <summary>
  ///   Gets or sets the symbol representing a major interval quality.
  /// </summary>
  /// <value>
  ///   A <see cref="char"/> that represents the major interval quality.
  ///   The default value is 'M'.
  /// </value>
  /// <remarks>
  ///   This property is used to standardize the representation of major interval qualities
  ///   in musical notation. It is commonly used in conjunction with other interval symbols
  ///   to describe musical intervals.
  /// </remarks>
  public static char MajorSymbol { get; set; } = 'M';

  /// <summary>
  ///   Gets or sets the symbol representing a minor interval quality.
  /// </summary>
  /// <value>
  ///   A <see cref="char"/> that represents the minor interval quality. The default value is 'm'.
  /// </value>
  /// <remarks>
  ///   This property is used to standardize the representation of minor interval qualities
  ///   in musical notation. It is commonly used in conjunction with other interval quality symbols
  ///   to describe musical intervals.
  /// </remarks>
  public static char MinorSymbol { get; set; } = 'm';

  /// <summary>
  ///   Gets or sets the symbol representing an augmented interval quality.
  /// </summary>
  /// <value>
  ///   A character that denotes an augmented interval. The default value is 'A'.
  /// </value>
  /// <remarks>
  ///   This property is used to standardize the representation of augmented interval qualities
  ///   in musical notation. It is commonly used in conjunction with other properties in the
  ///   <see cref="Notation"/> class to represent interval qualities.
  /// </remarks>
  public static char AugmentedSymbol { get; set; } = 'A';

  /// <summary>
  ///   Gets or sets the symbol representing a diminished interval quality.
  /// </summary>
  /// <value>
  ///   A <see cref="char"/> that represents the diminished interval quality.
  ///   The default value is 'd'.
  /// </value>
  /// <remarks>
  ///   This property is used to standardize the representation of diminished interval qualities
  ///   in musical notation. It is commonly used in conjunction with other interval quality symbols
  ///   to describe musical intervals.
  /// </remarks>
  public static char DiminishedSymbol { get; set; } = 'd';

  /// <summary>
  ///   Gets or sets the short name representation for an augmented interval quality.
  /// </summary>
  /// <value>
  ///   A string representing the short name for augmented intervals. The default value is "Aug".
  /// </value>
  /// <remarks>
  ///   This property is used to standardize the abbreviated representation of augmented intervals
  ///   in musical notation. It can be utilized in conjunction with other properties in the
  ///   <see cref="Notation"/> class to provide consistent naming conventions.
  /// </remarks>
  public static string AugmentedShortName { get; set; } = "Aug";

  /// <summary>
  ///   Gets or sets the short name representation for a diminished musical interval.
  /// </summary>
  /// <value>
  ///   A <see cref="string"/> representing the short name for a diminished interval.
  ///   The default value is "dim".
  /// </value>
  /// <remarks>
  ///   This property is used to standardize the representation of diminished intervals
  ///   in musical applications. It can be utilized in conjunction with other properties
  ///   in the <see cref="Notation"/> class to provide consistent naming conventions.
  /// </remarks>
  public static string DiminishedShortName { get; set; } = "dim";

  /// <summary>
  ///   Gets or sets the short name for the "Major" interval quality.
  /// </summary>
  /// <value>
  ///   A string representing the short name for the "Major" interval quality, typically "Maj".
  /// </value>
  /// <remarks>
  ///   This property is used to standardize the representation of the "Major" interval quality
  ///   in musical applications. It is commonly used in conjunction with other interval quality
  ///   properties to provide consistent naming conventions.
  /// </remarks>
  public static string MajorShortName { get; set; } = "Maj";

  /// <summary>
  ///   Gets or sets the short name representation for a minor interval quality.
  /// </summary>
  /// <value>
  ///   A string representing the short name for a minor interval, typically "min".
  /// </value>
  /// <remarks>
  ///   This property is used to standardize the representation of minor interval qualities
  ///   in musical notation.
  /// </remarks>
  public static string MinorShortName { get; set; } = "min";

  /// <summary>
  ///   Gets or sets the short name representation for the "Perfect" interval quality.
  /// </summary>
  /// <remarks>
  ///   This property provides a standardized short name for the "Perfect" interval quality,
  ///   which is commonly used in musical notation. The default value is "Perf".
  /// </remarks>
  public static string PerfectShortName { get; set; } = "Perf";

  /// <summary>
  ///   Gets or sets the long name representation of the "Major" interval quality.
  /// </summary>
  /// <value>
  ///   A <see cref="string"/> representing the full name of the "Major" interval quality.
  /// </value>
  /// <remarks>
  ///   This property is used to provide a standardized long name for the "Major" interval quality,
  ///   which can be utilized in musical applications for clarity and consistency.
  /// </remarks>
  public static string MajorLongName { get; set; } = "Major";

  /// <summary>
  ///   Gets or sets the long name representation of the "Perfect" interval quality.
  /// </summary>
  /// <value>
  ///   A <see cref="string"/> that represents the full name of the "Perfect" interval quality.
  ///   The default value is "Perfect".
  /// </value>
  /// <remarks>
  ///   This property is used to standardize the representation of the "Perfect" interval quality
  ///   in musical applications. It is particularly useful when generating descriptive names
  ///   for musical intervals.
  /// </remarks>
  public static string PerfectLongName { get; set; } = "Perfect";

  /// <summary>
  ///   Gets or sets the long name representation for an augmented interval quality.
  /// </summary>
  /// <value>
  ///   A <see cref="string"/> representing the long name for an augmented interval.
  ///   The default value is "Augmented".
  /// </value>
  /// <remarks>
  ///   This property is used to standardize the textual representation of augmented interval qualities
  ///   in musical applications.
  /// </remarks>
  public static string AugmentedLongName { get; set; } = "Augmented";

  /// <summary>
  ///   Gets or sets the full name representation of a diminished interval quality.
  /// </summary>
  /// <value>
  ///   A <see cref="string"/> representing the long name for a diminished interval quality.
  ///   The default value is "Diminished".
  /// </value>
  /// <remarks>
  ///   This property is used to standardize the representation of diminished interval qualities
  ///   in musical applications. It is typically used in conjunction with other interval quality
  ///   properties in the <see cref="Notation"/> class.
  /// </remarks>
  public static string DiminishedLongName { get; set; } = "Diminished";

  /// <summary>
  ///   Gets or sets the long name representation of the "Minor" interval quality.
  /// </summary>
  /// <value>
  ///   A <see cref="string"/> that represents the full name of the "Minor" interval quality.
  ///   The default value is "Minor".
  /// </value>
  /// <remarks>
  ///   This property is used to standardize the representation of the "Minor" interval quality
  ///   in musical applications. It is typically used in conjunction with other interval quality
  ///   properties in the <see cref="Notation"/> class.
  /// </remarks>
  public static string MinorLongName { get; set; } = "Minor";

  /// <summary>
  ///   Gets or sets the long name representation for a doubly augmented musical interval.
  /// </summary>
  /// <value>
  ///   A <see cref="string"/> representing the long name "Doubly Augmented".
  /// </value>
  /// <remarks>
  ///   This property is used to standardize the naming of doubly augmented intervals
  ///   in musical notation.
  /// </remarks>
  public static string DoublyAugmentedLongName { get; set; } = "Doubly Augmented";

  /// <summary>
  ///   Gets or sets the long name representation for a triply augmented interval.
  /// </summary>
  /// <value>
  ///   A <see cref="string"/> that represents the long name "Triply Augmented".
  /// </value>
  /// <remarks>
  ///   This property is used to standardize the naming of triply augmented intervals
  ///   in musical notation.
  /// </remarks>
  public static string TriplyAugmentedLongName { get; set; } = "Triply Augmented";

  /// <summary>
  ///   Gets or sets the long name representation for a doubly diminished interval quality.
  /// </summary>
  /// <value>
  ///   A <see cref="string"/> that represents the long name "Doubly Diminished".
  /// </value>
  /// <remarks>
  ///   This property is used to standardize the representation of doubly diminished interval qualities
  ///   in musical applications. It is primarily utilized in methods that require descriptive names
  ///   for interval qualities.
  /// </remarks>
  public static string DoublyDiminishedLongName { get; set; } = "Doubly Diminished";

  /// <summary>
  ///   Gets or sets the long name representation for a triply diminished musical interval.
  /// </summary>
  /// <value>
  ///   A <see cref="string"/> that represents the long name "Triply Diminished".
  /// </value>
  /// <remarks>
  ///   This property is used to standardize the naming of triply diminished intervals
  ///   in musical applications.
  /// </remarks>
  public static string TriplyDiminishedLongName { get; set; } = "Triply Diminished";

  #endregion
}
