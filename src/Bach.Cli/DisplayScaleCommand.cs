// Module Name: DisplayScaleCommand.cs
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
using Bach.Model;

namespace Bach.Cli;

internal sealed class DisplayScaleCommand: BachCommand
{
#region Constructors

  /// <inheritdoc />
  public DisplayScaleCommand()
  {
    var nameArg = CreateArgument<string>( "name", "The name of the scale" );
    var rootsArg = CreateMultiArgument<string>( "roots", "The scale roots" );

    var command = CreateCommand( "scale", arguments: new Argument[] { nameArg, rootsArg } );
    command.SetHandler( Execute, nameArg, rootsArg );
    Command = command;
  }

#endregion

#region Properties

  /// <inheritdoc />
  public override Command Command { get; }

#endregion

#region Implementation

  private static void Execute(
    string scaleName,
    IEnumerable<string> roots )
  {
    var formula = Registry.ScaleFormulas[scaleName];

    WriteList( $"{formula.Name} scale => ", formula.Intervals );

    foreach( var rootValue in roots )
    {
      var root = PitchClass.Parse( rootValue );
      var scale = new Scale( root, formula );
      WriteList( $"  {root} {formula.Name}: ", scale.PitchClasses );
    }

    WriteLine();
  }

#endregion
}
