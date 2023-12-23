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

namespace Bach.Model.Test;

public sealed class TriadTests
{
  #region Public Methods

  [Fact]
  public void AugmentedTriadTest()
  {
    var triad = new Triad( PitchClass.C, TriadQuality.Augmented );

    // ReSharper disable once StringLiteralTypo
    triad.Name.Should()
         .Be( "Caug" );
    triad.Quality.Should()
         .Be( TriadQuality.Augmented );
    triad.Root.Should()
         .Be( PitchClass.C );
    triad.Bass.Should()
         .Be( PitchClass.C );
    triad.Inversion.Should()
         .Be( 0 );
    triad.PitchClasses.Count.Should()
         .Be( 3 );
    triad.PitchClasses[0]
         .Should()
         .Be( PitchClass.C );
    triad.PitchClasses[1]
         .Should()
         .Be( PitchClass.E );
    triad.PitchClasses[2]
         .Should()
         .Be( PitchClass.GSharp );
  }

  [Fact]
  public void DiminishedTriadTest()
  {
    var triad = new Triad( PitchClass.C, TriadQuality.Diminished );

    // ReSharper disable once StringLiteralTypo
    triad.Name.Should()
         .Be( "Cdim" );
    triad.Quality.Should()
         .Be( TriadQuality.Diminished );
    triad.Root.Should()
         .Be( PitchClass.C );
    triad.Bass.Should()
         .Be( PitchClass.C );
    triad.Inversion.Should()
         .Be( 0 );
    triad.PitchClasses.Count.Should()
         .Be( 3 );
    triad.PitchClasses[0]
         .Should()
         .Be( PitchClass.C );
    triad.PitchClasses[1]
         .Should()
         .Be( PitchClass.EFlat );
    triad.PitchClasses[2]
         .Should()
         .Be( PitchClass.GFlat );
  }

  [Fact]
  public void FirstInversionTest()
  {
    var triad = new Triad( PitchClass.G, TriadQuality.Major );
    var inversion = triad.GetInversion( 1 );
    inversion.Name.Should()
             .Be( "G/B" );
    inversion.Quality.Should()
             .Be( TriadQuality.Major );
    inversion.Root.Should()
             .Be( PitchClass.G );
    inversion.Bass.Should()
             .Be( PitchClass.B );
    inversion.Inversion.Should()
             .Be( 1 );
    inversion.PitchClasses.Count.Should()
             .Be( 3 );
    inversion.PitchClasses[0]
             .Should()
             .Be( PitchClass.B );
    inversion.PitchClasses[1]
             .Should()
             .Be( PitchClass.D );
    inversion.PitchClasses[2]
             .Should()
             .Be( PitchClass.G );
  }

  [Fact]
  public void InvertThrowsWithInvalidInversionNumberTest()
  {
    var triad = new Triad( PitchClass.G, TriadQuality.Major );
    var act1 = () => triad.GetInversion( -1 );
    act1.Should()
        .Throw<ArgumentOutOfRangeException>();
    var act2 = () => triad.GetInversion( 3 );
    act2.Should()
        .Throw<ArgumentOutOfRangeException>();
  }

  [Fact]
  public void MajorTriadTest()
  {
    var triad = new Triad( PitchClass.C, TriadQuality.Major );
    triad.Name.Should()
         .Be( "C" );
    triad.Quality.Should()
         .Be( TriadQuality.Major );
    triad.Root.Should()
         .Be( PitchClass.C );
    triad.Bass.Should()
         .Be( PitchClass.C );
    triad.Inversion.Should()
         .Be( 0 );
    triad.PitchClasses.Count.Should()
         .Be( 3 );
    triad.PitchClasses[0]
         .Should()
         .Be( PitchClass.C );
    triad.PitchClasses[1]
         .Should()
         .Be( PitchClass.E );
    triad.PitchClasses[2]
         .Should()
         .Be( PitchClass.G );
  }

  [Fact]
  public void MinorTriadTest()
  {
    var triad = new Triad( PitchClass.C, TriadQuality.Minor );
    triad.Name.Should()
         .Be( "Cm" );
    triad.Quality.Should()
         .Be( TriadQuality.Minor );
    triad.Root.Should()
         .Be( PitchClass.C );
    triad.Bass.Should()
         .Be( PitchClass.C );
    triad.Inversion.Should()
         .Be( 0 );
    triad.PitchClasses.Count.Should()
         .Be( 3 );
    triad.PitchClasses[0]
         .Should()
         .Be( PitchClass.C );
    triad.PitchClasses[1]
         .Should()
         .Be( PitchClass.EFlat );
    triad.PitchClasses[2]
         .Should()
         .Be( PitchClass.G );
  }

  [Fact]
  public void SecondInversionTest()
  {
    var triad = new Triad( PitchClass.G, TriadQuality.Major );
    var inversion = triad.GetInversion( 2 );
    inversion.Name.Should()
             .Be( "G/D" );
    inversion.Quality.Should()
             .Be( TriadQuality.Major );
    inversion.Root.Should()
             .Be( PitchClass.G );
    inversion.Bass.Should()
             .Be( PitchClass.D );
    inversion.Inversion.Should()
             .Be( 2 );
    inversion.PitchClasses.Count.Should()
             .Be( 3 );
    inversion.PitchClasses[0]
             .Should()
             .Be( PitchClass.D );
    inversion.PitchClasses[1]
             .Should()
             .Be( PitchClass.G );
    inversion.PitchClasses[2]
             .Should()
             .Be( PitchClass.B );
  }

  #endregion
}
