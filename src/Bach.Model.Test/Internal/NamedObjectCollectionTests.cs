// Module Name: NamedObjectCollectionTest.cs
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

using Bach.Model.Internal;

namespace Bach.Model.Test;

public sealed class NamedObjectCollectionTests
{
  #region Nested Types

  private sealed record FakeNamedObject(
    string Id,
    string Name ): INamedObject;

  #endregion

  #region Public Methods

  [Fact]
  public void RemoveAt_ShouldRemoveExistingEntriesFromIndexes()
  {
    var collection = new NamedObjectCollection<FakeNamedObject>();
    collection.Add( new FakeNamedObject( "alpha", "Alpha" ) );

    collection.RemoveAt( 0 );

    collection.Should()
              .BeEmpty();

    collection.TryGetValue( "Alpha", out _ )
              .Should()
              .BeFalse();
  }

  [Fact]
  public void SetItem_ShouldUpdateIndexes_WhenItemChanges()
  {
    var collection = new NamedObjectCollection<FakeNamedObject>();
    collection.Add( new FakeNamedObject( "alpha", "Alpha" ) );

    collection[0] = new FakeNamedObject( "beta", "Beta" );

    collection.TryGetValue( "alpha", out _ )
              .Should()
              .BeFalse();

    collection.TryGetValue( "Beta", out var updated )
              .Should()
              .BeTrue();

    updated.Should()
           .NotBeNull();

    collection["beta"]
      .Should()
      .BeSameAs( updated );
  }

  [Fact]
  public void TryGetValue_ShouldResolveItemsByIdAndName()
  {
    var collection = new NamedObjectCollection<FakeNamedObject>();
    var item = new FakeNamedObject( "alpha", "Alpha" );
    collection.Add( item );

    collection["alpha"]
      .Should()
      .BeSameAs( item );

    collection["Alpha"]
      .Should()
      .BeSameAs( item );

    collection.TryGetValue( "ALPHA", out var result )
              .Should()
              .BeTrue();

    result.Should()
          .BeSameAs( item );
  }

  [Fact]
  public void TryGetValue_ShouldReturnFalse_WhenItemDoesNotExist()
  {
    var collection = new NamedObjectCollection<FakeNamedObject>();

    collection.TryGetValue( "missing", out var item )
              .Should()
              .BeFalse();

    item.Should()
        .BeNull();
  }

  #endregion
}
