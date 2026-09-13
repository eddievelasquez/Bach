// Module Name: Scale.cs
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
using System.Text;
using Bach.Model.Internal;

namespace Bach.Model.Scales;

/// <summary>A scale is a set of pitchClasses defined by a ScaleFormula .</summary>
public sealed class Scale
  : PitchCollection<PitchClass>,
    IEquatable<Scale>,
    IFormattable
{
  #region Constants

  private const string DEFAULT_TO_STRING_FORMAT = "N {S}";

  #endregion

  #region Constructors

  /// <summary>Constructor.</summary>
  /// <param name="root">The root pitchClass of the scale.</param>
  /// <param name="formula">The formula used to generate the scale.</param>
  /// <exception cref="ArgumentNullException">Thrown when the formula is null.</exception>
  public Scale(
    PitchClass root,
    ScaleFormula formula )
    : this( root, formula, CreatePitchClasses( root, formula ) )
  {
  }

  /// <summary>Constructor.</summary>
  /// <param name="root">The root pitchClass of the scale.</param>
  /// <param name="formulaIdOrName">ID or name of the formula as defined in the Registry.</param>
  /// <exception cref="ArgumentNullException">Thrown when the formula name is null.</exception>
  public Scale(
    PitchClass root,
    string formulaIdOrName )
    : this( root, Registry.ScaleFormulas[formulaIdOrName] )
  {
  }

  /// <summary>
  ///   Constructor.
  /// </summary>
  /// <param name="root">The root pitchClass of the scale.</param>
  /// <param name="formula">The formula used to generate the scale.</param>
  /// <param name="pitchClasses">The collection of pitch classes.</param>
  private Scale(
    PitchClass root,
    ScaleFormula formula,
    PitchClass[] pitchClasses )
    : base( pitchClasses )
  {
    ArgumentNullException.ThrowIfNull( formula );
    ArgumentNullException.ThrowIfNull( pitchClasses );

    Root = root;
    Formula = formula;
    Name = GenerateName( root, formula );
    Theoretical = IsTheoretical( this );
  }

  #endregion

  #region Properties

  /// <summary>Gets the root <see cref="PitchClass"/> of the scale.</summary>
  /// <value>The root.</value>
  public PitchClass Root { get; }

  /// <summary>Gets the localized name of the scale.</summary>
  /// <value>The name.</value>
  public string Name { get; }

  /// <summary>Gets the formula for the scale.</summary>
  /// <value>The <see cref="ScaleFormula"/>.</value>
  public ScaleFormula Formula { get; }

  /// <summary>Determines if this scale is theoretical.</summary>
  /// <remarks>
  ///   A theoretical scale is one that contains at least one double flat or double sharp accidental.
  ///   These scales exist in music theory but are not used in practice because of their complexity.
  ///   There's always another practical scale that contains exactly the same enharmonic pitches in the same order.
  ///   See <see cref="GetEnharmonicScale"/> to get that scale.
  /// </remarks>
  /// <returns>True if the scale is theoretical; otherwise, it returns false.</returns>
  public bool Theoretical { get; }

  #endregion

  #region Public Methods

  /// <summary>Determines if this instance contains the given pitchClasses.</summary>
  /// <param name="notes">The pitchClasses.</param>
  /// <returns>True if all the pitchClasses are in this scale; otherwise, false.</returns>
  public bool Contains(
    IEnumerable<PitchClass> notes )
  {
    return notes.All( note => IndexOf( note ) >= 0 );
  }

  /// <inheritdoc/>
  public bool Equals(
    Scale? other )
  {
    if( ReferenceEquals( other, this ) )
    {
      return true;
    }

    if( other is null )
    {
      return false;
    }

    return Root.Equals( other.Root ) && Formula.Equals( other.Formula );
  }

  /// <inheritdoc/>
  public override bool Equals(
    object? obj )
  {
    if( ReferenceEquals( obj, this ) )
    {
      return true;
    }

    return obj is Scale other && Equals( other );
  }

  /// <summary>Returns an enumerable that iterates through the scale in ascending fashion.</summary>
  /// <returns>An enumerable that iterates through the scale in ascending fashion.</returns>
  public IEnumerable<PitchClass> GetAscending()
  {
    return GeneratePitchClasses( Formula.AscendingDegrees );
  }

  /// <summary>
  /// Returns an enumerable that iterates through the scale in descending fashion.
  /// </summary>
  /// <returns>An enumerable that iterates through the scale in descending fashion.</returns>
  public IEnumerable<PitchClass> GetDescending()
  {
    return GeneratePitchClasses( Formula.DescendingDegrees );
  }

  private IEnumerable<PitchClass> GeneratePitchClasses(
    IReadOnlyList<ScaleDegreeStep> degreeSteps )
  {
    // The root is always the first note in the scale; if the first interval is not Unison,
    // we need to yield the root
    if( degreeSteps[0].Interval != Interval.Unison)
    {
      yield return Root;
    }

    // The rest of the notes in the scale are based on the provided intervals.

    // maxIterationCount provides a way to break out of an otherwise infinite
    // loop, as it doesn't make sense to generate more pitch classes than
    // the number of pitches that are supported.
    var maxIterationCount = Pitch.TotalPitchCount;
    var index = 0;

    while( maxIterationCount-- >= 0 )
    {
      var pitchClass = Root + degreeSteps[index].Interval;
      yield return pitchClass;

      index = this.WrapIndex( index + 1 );
    }
  }

  /// <summary>Gets an enharmonic scale for this instance.</summary>
  /// <returns>The enharmonic scale.</returns>
  /// <remarks>
  ///   An enharmonic scale is a scale that contains notes that are enharmonically equivalent to the notes in this scale.
  ///   For example, a C# major scale is enharmonically equivalent to a Db major scale.
  /// </remarks>
  public Scale GetEnharmonicScale()
  {
    // A sharp-rooted scale re-spells using the next letter name (e.g. C# -> D-something); a flat-rooted
    // scale re-spells using the previous letter name (e.g. Fb -> E-something). A natural root keeps its own
    // letter name, which naturally resolves to itself below. Deriving the target letter name directly (rather
    // than transposing by a semitone) avoids failing for roots whose neighboring pitch class would require an
    // accidental beyond double sharp/flat.
    var noteNameOffset = Math.Sign( (int) Root.Accidental );
    var expectedNoteName = (NoteName) ( (int) Root.NoteName + noteNameOffset ).Wrap( Constants.NoteNameCount );
    var enharmonicRoot = Root.GetEnharmonic( expectedNoteName );

    if( enharmonicRoot == null || enharmonicRoot.Value == Root )
    {
      return this;
    }

    var scale = new Scale( enharmonicRoot.Value, Formula );
    return scale;
  }

  /// <inheritdoc/>
  public override int GetHashCode()
  {
    return HashCode.Combine( Root, Formula );
  }

  /// <summary>Returns a rendered version of the scale starting with the provided pitch.</summary>
  /// <param name="octave">The octave for the first pitch.</param>
  /// <returns>An enumerator for a pitch sequence for this scale.</returns>
  public IEnumerable<Pitch> Render(
    int octave )
  {
    return Formula.Generate( new Pitch( Root, octave ) );
  }

  /// <summary>
  ///   Enumerates the scales that contain the given pitchClasses matching exactly the intervals between them.
  /// </summary>
  /// <param name="notes">The pitchClasses.</param>
  /// <returns>
  ///   An enumerator to all the scales that contain the pitchClasses. NOTE: this method performs collection
  ///   matching only. It is a discovery utility and does not perform tonal inference, scoring, or evidence
  ///   aggregation. Use the tonal evaluator in <c>Bach.Model.Analysis</c> for evidence-based key/candidate
  ///   evaluation.
  /// </returns>
  public static IEnumerable<Scale> ScalesContaining(
    IEnumerable<PitchClass> notes )
  {
    return ScalesContaining( IntervalMatch.Exact, notes );
  }

  /// <summary>Enumerates the scales that contain the given pitchClasses.</summary>
  /// <param name="match">Interval matching strategy.</param>
  /// <param name="pitchClasses">The pitchClasses.</param>
  /// <returns>
  ///   An enumerator to all the scales that contain the pitchClasses.
  /// </returns>
  public static IEnumerable<Scale> ScalesContaining(
    IntervalMatch match,
    IEnumerable<PitchClass> pitchClasses )
  {
#if BRUTE_FORCE_MATCHING
    foreach( var formula in Registry.ScaleFormulas )
    {
      var root = PitchClass.C;

      do
      {
        var scale = new Scale( root, formula );

        if( scale.Contains( pitchClasses ) )
        {
          yield return scale;
        }

        ++root;
      } while( root != PitchClass.C );
    }
#else
    // We calculate the intervals between the given pitch classes and then check which scales contain those intervals.
    var rootNotes = new CircularArray<PitchClass>( pitchClasses );

    do
    {
      // Calculate the intervals between the given pitch classes, starting from the current root note.
      var intervals = rootNotes.CalculateIntervals()
                               .ToArray();

      // Check which scales contain those intervals.
      foreach( var formula in Registry.ScaleFormulas )
      {
        if( !formula.Contains( intervals, match ) )
        {
          continue;
        }

        // If the formula contains the intervals, we create a scale with the current root note and the formula.
        var scale = new Scale( rootNotes[0], formula );
        yield return scale;
      }

      // Move to the next root note in the circular array until we have checked all the pitch classes.
      ++rootNotes.Head;
    } while( rootNotes.Head != 0 );
#endif
  }

  /// <inheritdoc/>
  public override string ToString()
  {
    return ToString( DEFAULT_TO_STRING_FORMAT, null );
  }

  /// <summary>
  ///   Returns a string representation of the value of this <see cref="Scale"/> instance, according to the
  ///   provided format specifier.
  /// </summary>
  /// <param name="format">A custom format string.</param>
  /// <returns>
  ///   A string representation of the value of the current <see cref="Scale"/> object as specified by
  ///   <paramref name="format"/>.
  /// </returns>
  /// <remarks>
  ///   <para>"N": Name pattern. e.g. "C Major".</para>
  ///   <para>"R": Root pattern. e.g. "C".</para>
  ///   <para>"F": Formula name pattern. e.g. "Major".</para>
  ///   <para>"S": PitchClasses pattern. e.g. "C,E,G".</para>
  ///   <para>"I": Intervals pattern. e.g. "P1,M3,P5".</para>
  /// </remarks>
  public string ToString(
    string format )
  {
    return ToString( format, null );
  }

  /// <summary>
  ///   Returns a string representation of the value of this <see cref="Scale"/> instance, according to the
  ///   provided format specifier and format provider.
  /// </summary>
  /// <param name="format">A custom format string.</param>
  /// <param name="provider">The format provider. Not used.</param>
  /// <returns>
  ///   A string representation of the value of the current <see cref="Scale"/> object as specified by
  ///   <paramref name="format"/>.
  /// </returns>
  /// <remarks>
  ///   <para>Format specifiers:</para>
  ///   <para>"N": Name pattern. e.g. "C Major".</para>
  ///   <para>"R": Root pattern. e.g. "C".</para>
  ///   <para>"F": Formula name pattern. e.g. "Major".</para>
  ///   <para>"S": PitchClasses pattern. e.g. "C,E,G".</para>
  ///   <para>"I": Intervals pattern. e.g. "P1,M3,P5".</para>
  /// </remarks>
  public string ToString(
    string? format,
    IFormatProvider? provider )
  {
    if( string.IsNullOrEmpty( format ) )
    {
      format = DEFAULT_TO_STRING_FORMAT;
    }

    var buf = new StringBuilder();

    foreach( var f in format )
    {
      switch( f )
      {
        case 'F':
          buf.Append( Formula.Name );
          break;

        case 'I':
          buf.Append( string.Join(',', Formula.Intervals ) );
          break;

        case 'N':
          buf.Append( Name );
          break;

        case 'R':
          buf.Append( Root );
          break;

        case 'S':
          buf.Append( string.Join( ",", Formula.Generate( Root ).Take( Formula.Intervals.Count ) ) );
          break;

        default:
          buf.Append( f );
          break;
      }
    }

    return buf.ToString();
  }

  #endregion

  #region Implementation

  private static PitchClass[] CreatePitchClasses(
    PitchClass root,
    ScaleFormula formula )
  {
    ArgumentNullException.ThrowIfNull( formula );

    return
    [
      .. formula.Generate( root )
                .Take( formula.Intervals.Count )
    ];
  }

  private static string GenerateName(
    PitchClass root,
    ScaleFormula formula )
  {
    var buf = new StringBuilder();
    buf.Append( root.NoteName );
    buf.Append( root.Accidental.ToSymbol() );

    if( Comparer.NameComparer.Equals( formula.Name, "Major" ) )
    {
      return buf.ToString();
    }

    buf.Append( ' ' );
    buf.Append( formula.Name );

    return buf.ToString();
  }

  private static bool IsTheoretical(
    Scale scale )
  {
    // A scale is theoretical when it contains at least one double flat or sharp.
    return scale.Any( note => note.Accidental == Accidental.DoubleFlat
                              || note.Accidental == Accidental.DoubleSharp
    );
  }

  #endregion
}
