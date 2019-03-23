// Module Name: TriadTest.cs
// Project:     Bach.Model.Test
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

namespace Bach.Model.Test
{
  using System;
  using System.Diagnostics;
  using Xunit;

  public class TriadTests
  {
    #region Public Methods

    [Fact]
    public void MajorTriadTest()
    {
      var triad = new Triad(Note.C, TriadQuality.Major);
      Assert.Equal("C", triad.Name);
      Assert.Equal(TriadQuality.Major, triad.Quality);
      Assert.Equal(Note.C, triad.Root);
      Assert.Equal(Note.C, triad.Bass);
      Assert.Equal(0, triad.Inversion);
      Assert.Equal(3, triad.Notes.Length);
      Assert.Equal(Note.C, triad.Notes[0]);
      Assert.Equal(Note.E, triad.Notes[1]);
      Assert.Equal(Note.G, triad.Notes[2]);
    }

    [Fact]
    public void MinorTriadTest()
    {
      var triad = new Triad(Note.C, TriadQuality.Minor);
      Assert.Equal("Cm", triad.Name);
      Assert.Equal(TriadQuality.Minor, triad.Quality);
      Assert.Equal(Note.C, triad.Root);
      Assert.Equal(Note.C, triad.Bass);
      Assert.Equal(0, triad.Inversion);
      Assert.Equal(3, triad.Notes.Length);
      Assert.Equal(Note.C, triad.Notes[0]);
      Assert.Equal(Note.EFlat, triad.Notes[1]);
      Assert.Equal(Note.G, triad.Notes[2]);
    }

    [Fact]
    public void DiminishedTriadTest()
    {
      var triad = new Triad(Note.C, TriadQuality.Diminished);
      Assert.Equal("Cdim", triad.Name);
      Assert.Equal(TriadQuality.Diminished, triad.Quality);
      Assert.Equal(Note.C, triad.Root);
      Assert.Equal(Note.C, triad.Bass);
      Assert.Equal(0, triad.Inversion);
      Assert.Equal(3, triad.Notes.Length);
      Assert.Equal(Note.C, triad.Notes[0]);
      Assert.Equal(Note.EFlat, triad.Notes[1]);
      Assert.Equal(Note.GFlat, triad.Notes[2]);
    }

    [Fact]
    public void AugmentedTriadTest()
    {
      var triad = new Triad(Note.C, TriadQuality.Augmented);
      Assert.Equal("Caug", triad.Name);
      Assert.Equal(TriadQuality.Augmented, triad.Quality);
      Assert.Equal(Note.C, triad.Root);
      Assert.Equal(Note.C, triad.Bass);
      Assert.Equal(0, triad.Inversion);
      Assert.Equal(3, triad.Notes.Length);
      Assert.Equal(Note.C, triad.Notes[0]);
      Assert.Equal(Note.E, triad.Notes[1]);
      Assert.Equal(Note.GSharp, triad.Notes[2]);
    }

    [Fact]
    public void FirstInversionTest()
    {
      var triad = new Triad(Note.G, TriadQuality.Major);
      var inversion = triad.GetInversion(1);
      Assert.Equal("G/B", inversion.Name);
      Assert.Equal(TriadQuality.Major, inversion.Quality);
      Assert.Equal(Note.G, inversion.Root);
      Assert.Equal(Note.B, inversion.Bass);
      Assert.Equal(1, inversion.Inversion);
      Assert.Equal(3, inversion.Notes.Length);
      Assert.Equal(Note.B, inversion.Notes[0]);
      Assert.Equal(Note.D, inversion.Notes[1]);
      Assert.Equal(Note.G, inversion.Notes[2]);
    }

    [Fact]
    public void SecondInversionTest()
    {
      var triad = new Triad(Note.G, TriadQuality.Major);
      var inversion = triad.GetInversion(2);
      Assert.Equal("G/D", inversion.Name);
      Assert.Equal(TriadQuality.Major, inversion.Quality);
      Assert.Equal(Note.G, inversion.Root);
      Assert.Equal(Note.D, inversion.Bass);
      Assert.Equal(2, inversion.Inversion);
      Assert.Equal(3, inversion.Notes.Length);
      Assert.Equal(Note.D, inversion.Notes[0]);
      Assert.Equal(Note.G, inversion.Notes[1]);
      Assert.Equal(Note.B, inversion.Notes[2]);
    }

    [Fact]
    public void InvertThrowsWithInvalidInversionNumberTest()
    {
      var triad = new Triad(Note.G, TriadQuality.Major);
      Assert.Throws<ArgumentOutOfRangeException>(() => triad.GetInversion(-1));
      Assert.Throws<ArgumentOutOfRangeException>(() => triad.GetInversion(3));
    }

    #endregion

    #region  Implementation

    #endregion
  }
}
