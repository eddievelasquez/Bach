// Module Name: TriadTest.cs
// Project:     Bach.Model.Test
// Copyright (c) 2012, 2023  Eddie Velasquez.
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

using System;
using Xunit;

namespace Bach.Model.Test;

public sealed class TriadTests
{
#region Public Methods

  [Fact]
  public void AugmentedTriadTest()
  {
    var triad = new Triad( PitchClass.C, TriadQuality.Augmented );

    // ReSharper disable once StringLiteralTypo
    Assert.Equal( "Caug", triad.Name );
    Assert.Equal( TriadQuality.Augmented, triad.Quality );
    Assert.Equal( PitchClass.C, triad.Root );
    Assert.Equal( PitchClass.C, triad.Bass );
    Assert.Equal( 0, triad.Inversion );
    Assert.Equal( 3, triad.PitchClasses.Count );
    Assert.Equal( PitchClass.C, triad.PitchClasses[0] );
    Assert.Equal( PitchClass.E, triad.PitchClasses[1] );
    Assert.Equal( PitchClass.GSharp, triad.PitchClasses[2] );
  }

  [Fact]
  public void DiminishedTriadTest()
  {
    var triad = new Triad( PitchClass.C, TriadQuality.Diminished );

    // ReSharper disable once StringLiteralTypo
    Assert.Equal( "Cdim", triad.Name );
    Assert.Equal( TriadQuality.Diminished, triad.Quality );
    Assert.Equal( PitchClass.C, triad.Root );
    Assert.Equal( PitchClass.C, triad.Bass );
    Assert.Equal( 0, triad.Inversion );
    Assert.Equal( 3, triad.PitchClasses.Count );
    Assert.Equal( PitchClass.C, triad.PitchClasses[0] );
    Assert.Equal( PitchClass.EFlat, triad.PitchClasses[1] );
    Assert.Equal( PitchClass.GFlat, triad.PitchClasses[2] );
  }

  [Fact]
  public void FirstInversionTest()
  {
    var triad = new Triad( PitchClass.G, TriadQuality.Major );
    var inversion = triad.GetInversion( 1 );
    Assert.Equal( "G/B", inversion.Name );
    Assert.Equal( TriadQuality.Major, inversion.Quality );
    Assert.Equal( PitchClass.G, inversion.Root );
    Assert.Equal( PitchClass.B, inversion.Bass );
    Assert.Equal( 1, inversion.Inversion );
    Assert.Equal( 3, inversion.PitchClasses.Count );
    Assert.Equal( PitchClass.B, inversion.PitchClasses[0] );
    Assert.Equal( PitchClass.D, inversion.PitchClasses[1] );
    Assert.Equal( PitchClass.G, inversion.PitchClasses[2] );
  }

  [Fact]
  public void InvertThrowsWithInvalidInversionNumberTest()
  {
    var triad = new Triad( PitchClass.G, TriadQuality.Major );
    Assert.Throws<ArgumentOutOfRangeException>( () => triad.GetInversion( -1 ) );
    Assert.Throws<ArgumentOutOfRangeException>( () => triad.GetInversion( 3 ) );
  }

  [Fact]
  public void MajorTriadTest()
  {
    var triad = new Triad( PitchClass.C, TriadQuality.Major );
    Assert.Equal( "C", triad.Name );
    Assert.Equal( TriadQuality.Major, triad.Quality );
    Assert.Equal( PitchClass.C, triad.Root );
    Assert.Equal( PitchClass.C, triad.Bass );
    Assert.Equal( 0, triad.Inversion );
    Assert.Equal( 3, triad.PitchClasses.Count );
    Assert.Equal( PitchClass.C, triad.PitchClasses[0] );
    Assert.Equal( PitchClass.E, triad.PitchClasses[1] );
    Assert.Equal( PitchClass.G, triad.PitchClasses[2] );
  }

  [Fact]
  public void MinorTriadTest()
  {
    var triad = new Triad( PitchClass.C, TriadQuality.Minor );
    Assert.Equal( "Cm", triad.Name );
    Assert.Equal( TriadQuality.Minor, triad.Quality );
    Assert.Equal( PitchClass.C, triad.Root );
    Assert.Equal( PitchClass.C, triad.Bass );
    Assert.Equal( 0, triad.Inversion );
    Assert.Equal( 3, triad.PitchClasses.Count );
    Assert.Equal( PitchClass.C, triad.PitchClasses[0] );
    Assert.Equal( PitchClass.EFlat, triad.PitchClasses[1] );
    Assert.Equal( PitchClass.G, triad.PitchClasses[2] );
  }

  [Fact]
  public void SecondInversionTest()
  {
    var triad = new Triad( PitchClass.G, TriadQuality.Major );
    var inversion = triad.GetInversion( 2 );
    Assert.Equal( "G/D", inversion.Name );
    Assert.Equal( TriadQuality.Major, inversion.Quality );
    Assert.Equal( PitchClass.G, inversion.Root );
    Assert.Equal( PitchClass.D, inversion.Bass );
    Assert.Equal( 2, inversion.Inversion );
    Assert.Equal( 3, inversion.PitchClasses.Count );
    Assert.Equal( PitchClass.D, inversion.PitchClasses[0] );
    Assert.Equal( PitchClass.G, inversion.PitchClasses[1] );
    Assert.Equal( PitchClass.B, inversion.PitchClasses[2] );
  }

#endregion
}
