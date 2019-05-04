// Module Name: Scale.cs
// Project:     Bach.Model
// Copyright (c) 2012, 2019  Eddie Velasquez.
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

namespace Bach.Model
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using Internal;

  /// <summary>A scale is a set of pitchClasses defined by a ScaleFormula .</summary>
  public class Scale: IEquatable<Scale>
  {
    #region Constants

    private const string DefaultToStringFormat = "N {S}";

    #endregion

    #region Constructors

    /// <summary>Constructor.</summary>
    /// <param name="root">The root pitchClass of the scale.</param>
    /// <param name="formula">The formula used to generate the scale.</param>
    /// <exception cref="ArgumentNullException">Thrown when the formula is null.</exception>
    public Scale(PitchClass root,
                 ScaleFormula formula)
    {
      Contract.Requires<ArgumentNullException>(formula != null);

      Root = root;
      Formula = formula;
      PitchClasses = new PitchClassCollection(Formula.Generate(Root).Take(Formula.Intervals.Count));

      var buf = new StringBuilder();
      buf.Append(root.NoteName);
      buf.Append(root.Accidental.ToSymbol());

      if( !Comparer.NameComparer.Equals(formula.Name, "Major") )
      {
        buf.Append(' ');
        buf.Append(formula.Name);
      }

      Name = buf.ToString();
      Theoretical = IsTheoretical(this);
    }

    /// <summary>Constructor.</summary>
    /// <param name="root">The root pitchClass of the scale.</param>
    /// <param name="formulaIdOrName">Id or name of the formula as defined in the Registry.</param>
    /// <exception cref="ArgumentNullException">Thrown when the formula name is null.</exception>
    public Scale(PitchClass root,
                 string formulaIdOrName)
      : this(root, Registry.ScaleFormulas[formulaIdOrName])
    {
    }

    #endregion

    #region Properties

    /// <summary>Gets the root pitchClass of the scale.</summary>
    /// <value>The root.</value>
    public PitchClass Root { get; }

    /// <summary>Gets the localized name of the scale.</summary>
    /// <value>The name.</value>
    public string Name { get; }

    /// <summary>Gets the formula for the scale.</summary>
    /// <value>The formula.</value>
    public ScaleFormula Formula { get; }

    /// <summary>Gets the pitchClasses that form this scale.</summary>
    /// <value>A collection of pitchClasses.</value>
    public PitchClassCollection PitchClasses { get; }

    /// <summary>Determines if this scale is theoretical.</summary>
    /// <remarks>
    ///   A theoretical scale is one that contains at least one double flat or double sharp accidental. These
    ///   scales exist in the musical theory realm but are not used in practice due to their complexity. There's always another
    ///   practical scale that contains exactly the same enharmonic pitches in the same order. See
    ///   <see cref="GetEnharmonicScale" /> for a way to obtain said scale.
    /// </remarks>
    /// <returns>True if the scale is theoretical; otherwise, it returns false.</returns>
    public bool Theoretical { get; }

    /// <summary>Returns an enumerable that iterates through the scale in ascending fashion.</summary>
    /// <value>An enumerable that iterates through the scale in ascending fashion.</value>
    public IEnumerable<PitchClass> Ascending
    {
      get
      {
        var index = 0;

        while( true )
        {
          yield return PitchClasses[index];

          index = ArrayExtensions.WrapIndex(PitchClasses.Count, ++index);
        }

        // ReSharper disable once IteratorNeverReturns
      }
    }

    /// <summary>Returns an enumerable that iterates through the scale in descending fashion.</summary>
    /// <value>An enumerable that iterates through the scale in descending fashion.</value>
    public IEnumerable<PitchClass> Descending
    {
      get
      {
        var index = 0;

        while( true )
        {
          yield return PitchClasses[index];

          index = ArrayExtensions.WrapIndex(PitchClasses.Count, --index);
        }

        // ReSharper disable once IteratorNeverReturns
      }
    }

    #endregion

    #region IEquatable<Scale> Members

    /// <inheritdoc />
    public bool Equals(Scale other)
    {
      if( ReferenceEquals(other, this) )
      {
        return true;
      }

      if( ReferenceEquals(other, null) )
      {
        return false;
      }

      return Root.Equals(other.Root) && Formula.Equals(other.Formula);
    }

    #endregion

    #region Public Methods

    /// <summary>Returns a rendered version of the scale starting with the provided pitch.</summary>
    /// <param name="octave">The octave for the first pitch.</param>
    /// <returns>An enumerator for a pitch sequence for this scale.</returns>
    public IEnumerable<Pitch> Render(int octave) => Formula.Generate(Pitch.Create(Root, octave));

    /// <summary>Gets a enharmonic scale for this instance.</summary>
    /// <returns>The enharmonic scale.</returns>
    public Scale GetEnharmonicScale()
    {
      PitchClass expectedPitchClass = Root.Accidental >= Accidental.Natural ? Root + 1 : Root - 1;
      PitchClass? enharmonicRoot = Root.GetEnharmonic(expectedPitchClass.NoteName);
      if( enharmonicRoot == null || enharmonicRoot.Value == Root )
      {
        return this;
      }

      var scale = new Scale(enharmonicRoot.Value, Formula);
      return scale;
    }

    /// <summary>Determines if this instance contains the given pitchClasses.</summary>
    /// <param name="notes">The pitchClasses.</param>
    /// <returns>True if all the pitchClasses are in this scale; otherwise, false.</returns>
    public bool Contains(IEnumerable<PitchClass> notes) => notes.All(note => PitchClasses.IndexOf(note) >= 0);

    /// <summary>Enumerates the scales that contain the given pitchClasses matching exactly the intervals between them.</summary>
    /// <param name="notes">The pitchClasses.</param>
    /// <returns>
    ///   An enumerator to all the scales that contain the pitchClasses.
    /// </returns>
    public static IEnumerable<Scale> ScalesContaining(IEnumerable<PitchClass> notes) => ScalesContaining(IntervalMatch.Exact, notes);

    /// <summary>Enumerates the scales that contain the given pitchClasses.</summary>
    /// <param name="match">Interval matching strategy.</param>
    /// <param name="notes">The pitchClasses.</param>
    /// <returns>
    ///   An enumerator to all the scales that contain the pitchClasses.
    /// </returns>
    public static IEnumerable<Scale> ScalesContaining(IntervalMatch match,
                                                      IEnumerable<PitchClass> notes)
    {
#if BRUTE_FORCE_MATCHING
      foreach( ScaleFormula formula in Registry.ScaleFormulas )
      {
        PitchClass root = PitchClass.C;

        do
        {
          var scale = new Scale(root, formula);
          if( scale.Contains(pitchClasses) )
          {
            yield return scale;
          }

          ++root;
        } while( root != PitchClass.C );
      }
#else
      var rootNotes = new CircularArray<PitchClass>(notes);

      do
      {
        Interval[] intervals = rootNotes.Intervals().ToArray();

        foreach( ScaleFormula formula in Registry.ScaleFormulas )
        {
          if( !formula.Contains(intervals, match) )
          {
            continue;
          }

          var scale = new Scale(rootNotes[0], formula);
          yield return scale;
        }

        ++rootNotes.Head;
      } while( rootNotes.Head != 0 );
#endif
    }

    /// <summary>
    ///   Returns a string representation of the value of this <see cref="Scale" /> instance, according to the
    ///   provided format specifier.
    /// </summary>
    /// <param name="format">A custom format string.</param>
    /// <returns>
    ///   A string representation of the value of the current <see cref="Scale" /> object as specified by
    ///   <paramref name="format" />.
    /// </returns>
    /// <remarks>
    ///   <para>"N": Name pattern. e.g. "C Major".</para>
    ///   <para>"R": Root pattern. e.g. "C".</para>
    ///   <para>"F": Formula name pattern. e.g. "Major".</para>
    ///   <para>"S": PitchClasses pattern. e.g. "C,E,G".</para>
    ///   <para>"I": Intervals pattern. e.g. "P1,M3,P5".</para>
    /// </remarks>
    public string ToString(string format) => ToString(format, null);

    /// <summary>
    ///   Returns a string representation of the value of this <see cref="Scale" /> instance, according to the
    ///   provided format specifier and format provider.
    /// </summary>
    /// <param name="format">A custom format string.</param>
    /// <param name="provider">The format provider. (Currently unused)</param>
    /// <returns>
    ///   A string representation of the value of the current <see cref="Scale" /> object as specified by
    ///   <paramref name="format" />.
    /// </returns>
    /// <remarks>
    ///   <para>Format specifiers:</para>
    ///   <para>"N": Name pattern. e.g. "C Major".</para>
    ///   <para>"R": Root pattern. e.g. "C".</para>
    ///   <para>"F": Formula name pattern. e.g. "Major".</para>
    ///   <para>"S": PitchClasses pattern. e.g. "C,E,G".</para>
    ///   <para>"I": Intervals pattern. e.g. "P1,M3,P5".</para>
    /// </remarks>
    public string ToString(string format,
                           IFormatProvider provider)
    {
      if( string.IsNullOrEmpty(format) )
      {
        format = DefaultToStringFormat;
      }

      var buf = new StringBuilder();
      foreach( char f in format )
      {
        switch( f )
        {
          case 'F':
            buf.Append(Formula.Name);
            break;

          case 'I':
            buf.Append(Formula.ToString("I"));
            break;

          case 'N':
            buf.Append(Name);
            break;

          case 'R':
            buf.Append(Root);
            break;

          case 'S':
            buf.Append(PitchClasses);
            break;

          default:
            buf.Append(f);
            break;
        }
      }

      return buf.ToString();
    }

    #endregion

    #region Overrides

    /// <inheritdoc />
    public override bool Equals(object other)
    {
      if( ReferenceEquals(other, this) )
      {
        return true;
      }

      if( ReferenceEquals(other, null) || other.GetType() != GetType() )
      {
        return false;
      }

      return Equals((Scale)other);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
      var hashCode = 17;

      unchecked
      {
        hashCode = ( hashCode * 23 ) + Root.GetHashCode();
        hashCode = ( hashCode * 23 ) + Formula.GetHashCode();
      }

      return hashCode;
    }

    /// <inheritdoc />
    public override string ToString() => ToString(DefaultToStringFormat, null);

    #endregion

    #region  Implementation

    private static bool IsTheoretical(Scale scale)
    {
      // Scale is theoretical when it contains at least one double flat or sharp.
      return scale.PitchClasses.Any(note => note.Accidental == Accidental.DoubleFlat || note.Accidental == Accidental.DoubleSharp);
    }

    #endregion
  }
}
