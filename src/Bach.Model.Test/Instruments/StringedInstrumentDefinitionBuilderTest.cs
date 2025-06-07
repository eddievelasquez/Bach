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

namespace Bach.Model.Test.Instruments;

using System.Linq;
using Model.Instruments;

public sealed class StringedInstrumentDefinitionBuilderTest
{
  #region Constants

  private const string INSTRUMENT_ID = "Id";
  private const string INSTRUMENT_NAME = "Name";
  private const int INSTRUMENT_STRING_COUNT = 3;
  private const string TUNING_ID = "Standard";
  private const string TUNING_NAME = "Standard";

  #endregion

  #region Public Methods

  [Fact]
  public void AddTuningThrowsOnStringMismatchTest()
  {
    var builder = new StringedInstrumentDefinitionBuilder( INSTRUMENT_ID, INSTRUMENT_NAME, INSTRUMENT_STRING_COUNT );
    var act = () => builder.AddTuning( TUNING_ID, TUNING_NAME, "C4,D4,E4,F4" );
    act.Should()
       .Throw<ArgumentException>();
  }

  [Fact]
  public void AddTuningWithNoteArrayTest()
  {
    var pitches = PitchCollection.Parse( "C4,D4,E4" )
                                 .ToArray();
    var builder = new StringedInstrumentDefinitionBuilder( INSTRUMENT_ID, INSTRUMENT_NAME, INSTRUMENT_STRING_COUNT );
    builder.AddTuning( TUNING_ID, TUNING_NAME, pitches );

    var definition = builder.Build();
    definition.Should()
              .NotBeNull();
    definition.Name.Should()
              .Be( INSTRUMENT_NAME );
    definition.StringCount.Should()
              .Be( INSTRUMENT_STRING_COUNT );
    definition.Tunings.Should()
              .NotBeNull();
    definition.Tunings.Should()
              .ContainSingle();
    definition.Tunings.Standard.Name.Should()
              .Be( TUNING_NAME );
    definition.Tunings.Standard.Name.Should()
              .Be( TUNING_NAME );
    definition.Tunings.Standard.Pitches.Should()
              .BeEquivalentTo( pitches );
  }

  [Fact]
  public void AddTuningWithNoteArrayThrowsOnBuilderReuseTest()
  {
    var pitches = PitchCollection.Parse( "C4,D4,E4" )
                                 .ToArray();
    var builder = new StringedInstrumentDefinitionBuilder( INSTRUMENT_ID, INSTRUMENT_NAME, INSTRUMENT_STRING_COUNT );
    builder.AddTuning( TUNING_ID, TUNING_NAME, pitches );
    builder.Build();

    var act = () => builder.AddTuning( "ATuning", "A tuning", pitches );
    act.Should()
       .Throw<InvalidOperationException>();
  }

  [Fact]
  public void AddTuningWithNoteArrayThrowsOnEmptyKeyTest()
  {
    var pitches = PitchCollection.Parse( "C4,D4,E4" )
                                 .ToArray();
    var builder = new StringedInstrumentDefinitionBuilder( INSTRUMENT_ID, INSTRUMENT_NAME, INSTRUMENT_STRING_COUNT );

    var act = () => builder.AddTuning( "", TUNING_NAME, pitches );
    act.Should()
       .Throw<ArgumentException>();
  }

  [Fact]
  public void AddTuningWithNoteArrayThrowsOnEmptyNameTest()
  {
    var pitches = PitchCollection.Parse( "C4,D4,E4" )
                                 .ToArray();
    var builder = new StringedInstrumentDefinitionBuilder( INSTRUMENT_ID, INSTRUMENT_NAME, INSTRUMENT_STRING_COUNT );

    var act = () => builder.AddTuning( TUNING_ID, "", pitches );
    act.Should()
       .Throw<ArgumentException>();
  }

  [Fact]
  public void AddTuningWithNoteArrayThrowsOnMismatchedNoteCountTest()
  {
    var pitches = PitchCollection.Parse( "C4,D4,E4,F4" )
                                 .ToArray();
    var builder = new StringedInstrumentDefinitionBuilder( INSTRUMENT_ID, INSTRUMENT_NAME, INSTRUMENT_STRING_COUNT );

    var act = () => builder.AddTuning( TUNING_ID, TUNING_NAME, pitches );
    act.Should()
       .Throw<ArgumentException>();
  }

  [Fact]
  public void AddTuningWithNoteArrayThrowsOnNullKeyTest()
  {
    var pitches = PitchCollection.Parse( "C4,D4,E4" )
                                 .ToArray();
    var builder = new StringedInstrumentDefinitionBuilder( INSTRUMENT_ID, INSTRUMENT_NAME, INSTRUMENT_STRING_COUNT );

    var act = () => builder.AddTuning( null!, TUNING_NAME, pitches );
    act.Should()
       .Throw<ArgumentNullException>();
  }

  [Fact]
  public void AddTuningWithNoteArrayThrowsOnNullNameTest()
  {
    var pitches = PitchCollection.Parse( "C4,D4,E4" )
                                 .ToArray();
    var builder = new StringedInstrumentDefinitionBuilder( INSTRUMENT_ID, INSTRUMENT_NAME, INSTRUMENT_STRING_COUNT );

    var act = () => builder.AddTuning( TUNING_ID, null!, pitches );
    act.Should()
       .Throw<ArgumentNullException>();
  }

  [Fact]
  public void AddTuningWithNoteCollectionThrowsOnBuilderReuseTest()
  {
    var pitches = PitchCollection.Parse( "C4,D4,E4" );
    var builder = new StringedInstrumentDefinitionBuilder( INSTRUMENT_ID, INSTRUMENT_NAME, INSTRUMENT_STRING_COUNT );
    builder.AddTuning( TUNING_ID, TUNING_NAME, pitches );
    builder.Build();

    var act = () => builder.AddTuning( "ATuning", "A tuning", pitches );
    act.Should()
       .Throw<InvalidOperationException>();
  }

  [Fact]
  public void AddTuningWithNoteCollectionThrowsOnEmptyKeyTest()
  {
    var pitches = PitchCollection.Parse( "C4,D4,E4" );
    var builder = new StringedInstrumentDefinitionBuilder( INSTRUMENT_ID, INSTRUMENT_NAME, INSTRUMENT_STRING_COUNT );

    var act = () => builder.AddTuning( "", TUNING_NAME, pitches );
    act.Should()
       .Throw<ArgumentException>();
  }

  [Fact]
  public void AddTuningWithNoteCollectionThrowsOnEmptyNameTest()
  {
    var pitches = PitchCollection.Parse( "C4,D4,E4" );
    var builder = new StringedInstrumentDefinitionBuilder( INSTRUMENT_ID, INSTRUMENT_NAME, INSTRUMENT_STRING_COUNT );

    var act = () => builder.AddTuning( TUNING_ID, "", pitches );
    act.Should()
       .Throw<ArgumentException>();
  }

  [Fact]
  public void AddTuningWithNoteCollectionThrowsOnMismatchedNoteCountTest()
  {
    var pitches = PitchCollection.Parse( "C4,D4,E4,F4" );
    var builder = new StringedInstrumentDefinitionBuilder( INSTRUMENT_ID, INSTRUMENT_NAME, INSTRUMENT_STRING_COUNT );

    var act = () => builder.AddTuning( TUNING_ID, TUNING_NAME, pitches );
    act.Should()
       .Throw<ArgumentException>();
  }

  [Fact]
  public void AddTuningWithNoteCollectionThrowsOnNullKeyTest()
  {
    var pitches = PitchCollection.Parse( "C4,D4,E4" );
    var builder = new StringedInstrumentDefinitionBuilder( INSTRUMENT_ID, INSTRUMENT_NAME, INSTRUMENT_STRING_COUNT );

    var act = () => builder.AddTuning( null!, TUNING_NAME, pitches );
    act.Should()
       .Throw<ArgumentNullException>();
  }

  [Fact]
  public void AddTuningWithNoteCollectionThrowsOnNullNameTest()
  {
    var pitches = PitchCollection.Parse( "C4,D4,E4" );
    var builder = new StringedInstrumentDefinitionBuilder( INSTRUMENT_ID, INSTRUMENT_NAME, INSTRUMENT_STRING_COUNT );

    var act = () => builder.AddTuning( TUNING_ID, null!, pitches );
    act.Should()
       .Throw<ArgumentNullException>();
  }

  [Fact]
  public void BuildTest()
  {
    var pitches = PitchCollection.Parse( "C4,D4,E4" );
    var builder = new StringedInstrumentDefinitionBuilder( INSTRUMENT_ID, INSTRUMENT_NAME, INSTRUMENT_STRING_COUNT );
    builder.AddTuning( TUNING_ID, TUNING_NAME, pitches );

    var definition = builder.Build();
    definition.Should()
              .NotBeNull();
    definition.Name.Should()
              .Be( INSTRUMENT_NAME );
    definition.StringCount.Should()
              .Be( INSTRUMENT_STRING_COUNT );
    definition.Tunings.Should()
              .NotBeNull();
    definition.Tunings.Should()
              .ContainSingle();
    definition.Tunings.Standard.Name.Should()
              .Be( TUNING_NAME );
    definition.Tunings.Standard.Name.Should()
              .Be( TUNING_NAME );
    definition.Tunings.Standard.Pitches.Should()
              .BeEquivalentTo( pitches );
  }

  [Fact]
  public void BuildThrowOnEmptyTuningsTest()
  {
    var builder = new StringedInstrumentDefinitionBuilder( INSTRUMENT_ID, INSTRUMENT_NAME, INSTRUMENT_STRING_COUNT );
    var act = () => builder.Build();
    act.Should()
       .Throw<InvalidOperationException>();
  }

  [Fact]
  public void BuildThrowOnMissingStandardTuningTest()
  {
    var pitches = PitchCollection.Parse( "C4,D4,E4" )
                                 .ToArray();
    var builder = new StringedInstrumentDefinitionBuilder( INSTRUMENT_ID, INSTRUMENT_NAME, INSTRUMENT_STRING_COUNT );
    builder.AddTuning( "ATuning", "A tuning", pitches );

    var act = () => builder.Build();
    act.Should()
       .Throw<InvalidOperationException>();
  }

  [Fact]
  public void BuildThrowsOnBuilderReuseTest()
  {
    var pitches = PitchCollection.Parse( "C4,D4,E4" );
    var builder = new StringedInstrumentDefinitionBuilder( INSTRUMENT_ID, INSTRUMENT_NAME, INSTRUMENT_STRING_COUNT );
    builder.AddTuning( TUNING_ID, TUNING_NAME, pitches );
    builder.Build();

    var act = () => builder.Build();
    act.Should()
       .Throw<InvalidOperationException>();
  }

  #endregion
}
