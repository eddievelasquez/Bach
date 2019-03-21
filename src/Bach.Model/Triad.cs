﻿// Module Name: Triad.cs
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

  public class Triad: Chord
  {
    #region Constants

    private static readonly ChordFormula s_majorTriad =
      new ChordFormula("MajorTriad", "MajorTriad", "", new[] { Interval.Unison, Interval.MajorThird, Interval.Fifth });

    private static readonly ChordFormula s_minorTriad =
      new ChordFormula("MinorTriad", "MinorTriad", "m", new[] { Interval.Unison, Interval.MinorThird, Interval.Fifth });

    private static readonly ChordFormula s_diminishedTriad =
      new ChordFormula("DiminishedTriad", "DiminishedTriad", "dim", new[] { Interval.Unison, Interval.MinorThird, Interval.DiminishedFifth });

    private static readonly ChordFormula s_augmentedTriad =
      new ChordFormula("AugmentedTriad", "AugmentedTriad", "aug", new[] { Interval.Unison, Interval.MajorThird, Interval.AugmentedFifth });

    #endregion

    #region Constructors

    public Triad(Note root,
                 TriadQuality quality)
      : base(root, GetFormula(quality))
    {
      Quality = quality;
    }

    private Triad(Note root,
                  ChordFormula formula,
                  int inversion)
      : base(root, formula, inversion)
    {
    }

    #endregion

    #region Properties

    public TriadQuality Quality { get; }

    #endregion

    #region Public Methods

    public new Triad Invert(int inversion = 1)
    {
      var result = new Triad(Root, Formula, inversion);
      return result;
    }

    #endregion

    #region  Implementation

    private static ChordFormula GetFormula(TriadQuality quality)
    {
      switch( quality )
      {
        case TriadQuality.Major:
          return s_majorTriad;

        case TriadQuality.Minor:
          return s_minorTriad;

        case TriadQuality.Diminished:
          return s_diminishedTriad;

        case TriadQuality.Augmented:
          return s_augmentedTriad;

        default:
          throw new ArgumentOutOfRangeException(nameof(quality), quality, null);
      }
    }

    #endregion
  }
}