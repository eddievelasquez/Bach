// Module Name: DisplayScalesContainingCommand.cs
// Project:     Bach.Cli
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

using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using Bach.Model;

namespace Bach.Cli;

internal sealed class DisplayScalesContainingCommand: BachCommand
{
#region Constructors

  /// <inheritdoc />
  public DisplayScalesContainingCommand()
  {
    var notesArg = CreateMultiArgument<string>( "notes", "The notes that will be searched for" );

    var command = CreateCommand( "scales-containing", arguments: new Argument[] { notesArg } );
    command.SetHandler( Execute, notesArg );
    Command = command;
  }

#endregion

#region Properties

  /// <inheritdoc />
  public override Command Command { get; }

#endregion

#region Implementation

  private static void Execute( IEnumerable<string> notes )
  {
    var pitchClasses = PitchClassCollection.Parse( string.Join( ",", notes ) );

    WriteList( "Scales containing: ", pitchClasses );
    WriteLine();

    foreach( var scale in Scale.ScalesContaining( pitchClasses ) )
    {
      WriteList( $"  {scale.Name}: ", scale.PitchClasses );
      WriteList( "    => ", scale.Formula.Intervals.Select( interval => interval.ToString( "Sq" ) ) );
      WriteLine();
    }
  }

#endregion
}
