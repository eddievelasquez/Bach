// Module Name: StringedInstrumentDefinitionBuilderTest.cs
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
using System.Linq;
using Bach.Model.Instruments;
using Xunit;

namespace Bach.Model.Test.Instruments;

public sealed class StringedInstrumentDefinitionBuilderTest
{
#region Constants

  private const string InstrumentId = "Id";
  private const string InstrumentName = "Name";
  private const int InstrumentStringCount = 3;
  private const string TuningId = "Standard";
  private const string TuningName = "Standard";

#endregion

#region Public Methods

  [Fact]
  public void AddTuningThrowsOnStringMismatchTest()
  {
    var builder = new StringedInstrumentDefinitionBuilder( InstrumentId, InstrumentName, InstrumentStringCount );
    Assert.Throws<ArgumentException>( () => builder.AddTuning( TuningId, TuningName, "C4,D4,E4,F4" ) );
  }

  [Fact]
  public void AddTuningWithNoteArrayTest()
  {
    var pitches = PitchCollection.Parse( "C4,D4,E4" ).ToArray();
    var builder = new StringedInstrumentDefinitionBuilder( InstrumentId, InstrumentName, InstrumentStringCount );
    builder.AddTuning( TuningId, TuningName, pitches );

    var definition = builder.Build();
    Assert.NotNull( definition );
    Assert.Equal( InstrumentName, definition.Name );
    Assert.Equal( InstrumentStringCount, definition.StringCount );
    Assert.NotNull( definition.Tunings );
    Assert.Single( definition.Tunings );
    Assert.Equal( TuningName, definition.Tunings.Standard.Name );
    Assert.Equal( TuningName, definition.Tunings.Standard.Name );
    Assert.Equal( pitches, definition.Tunings.Standard.Pitches );
  }

  [Fact]
  public void AddTuningWithNoteArrayThrowsOnBuilderReuseTest()
  {
    var pitches = PitchCollection.Parse( "C4,D4,E4" ).ToArray();
    var builder = new StringedInstrumentDefinitionBuilder( InstrumentId, InstrumentName, InstrumentStringCount );
    builder.AddTuning( TuningId, TuningName, pitches );
    builder.Build();

    Assert.Throws<InvalidOperationException>( () => builder.AddTuning( "ATuning", "A tuning", pitches ) );
  }

  [Fact]
  public void AddTuningWithNoteArrayThrowsOnEmptyKeyTest()
  {
    var pitches = PitchCollection.Parse( "C4,D4,E4" ).ToArray();
    var builder = new StringedInstrumentDefinitionBuilder( InstrumentId, InstrumentName, InstrumentStringCount );

    Assert.Throws<ArgumentException>( () => builder.AddTuning( "", TuningName, pitches ) );
  }

  [Fact]
  public void AddTuningWithNoteArrayThrowsOnEmptyNameTest()
  {
    var pitches = PitchCollection.Parse( "C4,D4,E4" ).ToArray();
    var builder = new StringedInstrumentDefinitionBuilder( InstrumentId, InstrumentName, InstrumentStringCount );

    Assert.Throws<ArgumentException>( () => builder.AddTuning( TuningId, "", pitches ) );
  }

  [Fact]
  public void AddTuningWithNoteArrayThrowsOnMismatchedNoteCountTest()
  {
    var pitches = PitchCollection.Parse( "C4,D4,E4,F4" ).ToArray();
    var builder = new StringedInstrumentDefinitionBuilder( InstrumentId, InstrumentName, InstrumentStringCount );

    Assert.Throws<ArgumentException>( () => builder.AddTuning( TuningId, TuningName, pitches ) );
  }

  [Fact]
  public void AddTuningWithNoteArrayThrowsOnNullKeyTest()
  {
    var pitches = PitchCollection.Parse( "C4,D4,E4" ).ToArray();
    var builder = new StringedInstrumentDefinitionBuilder( InstrumentId, InstrumentName, InstrumentStringCount );

    Assert.Throws<ArgumentNullException>( () => builder.AddTuning( null, TuningName, pitches ) );
  }

  [Fact]
  public void AddTuningWithNoteArrayThrowsOnNullNameTest()
  {
    var pitches = PitchCollection.Parse( "C4,D4,E4" ).ToArray();
    var builder = new StringedInstrumentDefinitionBuilder( InstrumentId, InstrumentName, InstrumentStringCount );

    Assert.Throws<ArgumentNullException>( () => builder.AddTuning( TuningId, null, pitches ) );
  }

  [Fact]
  public void AddTuningWithNoteCollectionThrowsOnBuilderReuseTest()
  {
    var pitches = PitchCollection.Parse( "C4,D4,E4" );
    var builder = new StringedInstrumentDefinitionBuilder( InstrumentId, InstrumentName, InstrumentStringCount );
    builder.AddTuning( TuningId, TuningName, pitches );
    builder.Build();

    Assert.Throws<InvalidOperationException>( () => builder.AddTuning( "ATuning", "A tuning", pitches ) );
  }

  [Fact]
  public void AddTuningWithNoteCollectionThrowsOnEmptyKeyTest()
  {
    var pitches = PitchCollection.Parse( "C4,D4,E4" );
    var builder = new StringedInstrumentDefinitionBuilder( InstrumentId, InstrumentName, InstrumentStringCount );

    Assert.Throws<ArgumentException>( () => builder.AddTuning( "", TuningName, pitches ) );
  }

  [Fact]
  public void AddTuningWithNoteCollectionThrowsOnEmptyNameTest()
  {
    var pitches = PitchCollection.Parse( "C4,D4,E4" );
    var builder = new StringedInstrumentDefinitionBuilder( InstrumentId, InstrumentName, InstrumentStringCount );

    Assert.Throws<ArgumentException>( () => builder.AddTuning( TuningId, "", pitches ) );
  }

  [Fact]
  public void AddTuningWithNoteCollectionThrowsOnMismatchedNoteCountTest()
  {
    var pitches = PitchCollection.Parse( "C4,D4,E4,F4" );
    var builder = new StringedInstrumentDefinitionBuilder( InstrumentId, InstrumentName, InstrumentStringCount );

    Assert.Throws<ArgumentException>( () => builder.AddTuning( TuningId, TuningName, pitches ) );
  }

  [Fact]
  public void AddTuningWithNoteCollectionThrowsOnNullKeyTest()
  {
    var pitches = PitchCollection.Parse( "C4,D4,E4" );
    var builder = new StringedInstrumentDefinitionBuilder( InstrumentId, InstrumentName, InstrumentStringCount );

    Assert.Throws<ArgumentNullException>( () => builder.AddTuning( null, TuningName, pitches ) );
  }

  [Fact]
  public void AddTuningWithNoteCollectionThrowsOnNullNameTest()
  {
    var pitches = PitchCollection.Parse( "C4,D4,E4" );
    var builder = new StringedInstrumentDefinitionBuilder( InstrumentId, InstrumentName, InstrumentStringCount );

    Assert.Throws<ArgumentNullException>( () => builder.AddTuning( TuningId, null, pitches ) );
  }

  [Fact]
  public void BuildTest()
  {
    var pitches = PitchCollection.Parse( "C4,D4,E4" );
    var builder = new StringedInstrumentDefinitionBuilder( InstrumentId, InstrumentName, InstrumentStringCount );
    builder.AddTuning( TuningId, TuningName, pitches );

    var definition = builder.Build();
    Assert.NotNull( definition );
    Assert.Equal( InstrumentName, definition.Name );
    Assert.Equal( InstrumentStringCount, definition.StringCount );
    Assert.NotNull( definition.Tunings );
    Assert.Single( definition.Tunings );
    Assert.Equal( TuningName, definition.Tunings.Standard.Name );
    Assert.Equal( TuningName, definition.Tunings.Standard.Name );
    Assert.Equal( pitches, definition.Tunings.Standard.Pitches );
  }

  [Fact]
  public void BuildThrowOnEmptyTuningsTest()
  {
    var builder = new StringedInstrumentDefinitionBuilder( InstrumentId, InstrumentName, InstrumentStringCount );
    Assert.Throws<InvalidOperationException>( () => builder.Build() );
  }

  [Fact]
  public void BuildThrowOnMissingStandardTuningTest()
  {
    var pitches = PitchCollection.Parse( "C4,D4,E4" ).ToArray();
    var builder = new StringedInstrumentDefinitionBuilder( InstrumentId, InstrumentName, InstrumentStringCount );
    builder.AddTuning( "ATuning", "A tuning", pitches );

    Assert.Throws<InvalidOperationException>( () => builder.Build() );
  }

  [Fact]
  public void BuildThrowsOnBuilderReuseTest()
  {
    var pitches = PitchCollection.Parse( "C4,D4,E4" );
    var builder = new StringedInstrumentDefinitionBuilder( InstrumentId, InstrumentName, InstrumentStringCount );
    builder.AddTuning( TuningId, TuningName, pitches );
    builder.Build();

    Assert.Throws<InvalidOperationException>( () => builder.Build() );
  }

#endregion
}
