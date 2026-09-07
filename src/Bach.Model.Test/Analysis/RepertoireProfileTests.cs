// Module Name: RepertoireProfileTests.cs
// Project:     Bach.Model.Test
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

namespace Bach.Model.Analysis.Test;

public sealed class RepertoireProfileTests
{
  #region Public Methods

  [Theory]
  [InlineData( ScaleTag.Tonal, 1.0 )]
  [InlineData( ScaleTag.CommonPractice, 1.0 )]
  [InlineData( ScaleTag.Modal, 0.8 )]
  [InlineData( ScaleTag.Jazz, 0.5 )]
  [InlineData( ScaleTag.Blues, 0.5 )]
  public void Default_ShouldReturnConfiguredWeight_WhenTagIsEnabled(
    ScaleTag tag,
    double expectedWeight )
  {
    RepertoireProfile.Default.GetWeight( tag )
                     .Should()
                     .Be( expectedWeight );
  }

  [Fact]
  public void CommonPractice_ShouldEnableOnlyCommonPracticeTags()
  {
    var profile = RepertoireProfile.CommonPractice;

    profile.EnabledTags.Should()
           .BeEquivalentTo( [ScaleTag.Tonal, ScaleTag.CommonPractice] );

    profile.GetWeight( ScaleTag.Modal )
           .Should()
           .Be( 0.0 );
  }

  [Fact]
  public void Modal_ShouldEnableModalAndTonalTags()
  {
    var profile = RepertoireProfile.Modal;

    profile.EnabledTags.Should()
           .BeEquivalentTo( [ScaleTag.Modal, ScaleTag.Tonal] );

    profile.GetWeight( ScaleTag.Modal )
           .Should()
           .Be( 1.0 );

    profile.GetWeight( ScaleTag.Tonal )
           .Should()
           .Be( 0.5 );
  }

  [Fact]
  public void GetWeight_ShouldReturnZero_WhenTagIsNotEnabled()
  {
    var profile = new RepertoireProfile( [( ScaleTag.Jazz, 0.75 )] );

    profile.GetWeight( ScaleTag.Blues )
           .Should()
           .Be( 0.0 );
  }

  #endregion
}
