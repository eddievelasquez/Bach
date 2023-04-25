// Module Name: BachCommand.cs
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
using Spectre.Console;

namespace Bach.Cli;

internal abstract class BachCommand
{
#region Properties

  public abstract Command Command { get; }

#endregion

#region Implementation

  protected static Argument<T> CreateArgument<T>(
    string name,
    string? description = null )
  {
    return new Argument<T>( name, description );
  }

  protected static Argument<IEnumerable<T>> CreateMultiArgument<T>(
    string name,
    string? description = null,
    ArgumentArity? arity = null )
  {
    var argument = new Argument<IEnumerable<T>>( name, description );
    if( arity != null )
    {
      argument.Arity = arity.Value;
    }

    return argument;
  }

  protected static Command CreateCommand(
    string name,
    string? description = null,
    Argument[]? arguments = null,
    BachCommand[]? subCommands = null )
  {
    var command = new Command( name, description );
    if( arguments != null )
    {
      foreach( var argument in arguments )
      {
        command.AddArgument( argument );
      }
    }

    if( subCommands != null )
    {
      foreach( var subCommand in subCommands )
      {
        command.AddCommand( subCommand.Command );
      }
    }

    return command;
  }

  protected static void WriteTitle( string title )
  {
    AnsiConsole.Write( new FigletText( title ).Color( Color.CadetBlue ) );
  }

  protected static void WriteLine()
  {
    AnsiConsole.WriteLine();
  }

  protected static void WriteLine<T>(
    string prompt,
    T? value )
  {
    if( value == null )
    {
      return;
    }

    AnsiConsole.MarkupLine( $"{prompt}[yellow]{value}[/]" );
  }

  protected static void Write<T>( T value )
  {
    AnsiConsole.Markup( $"[yellow]{value}[/]" );
  }

  protected static void WriteLine<T>( T value )
  {
    AnsiConsole.MarkupLine( $"[yellow]{value}[/]" );
  }

  protected static void WriteList<T>( IEnumerable<T> values )
  {
    var needsSep = false;
    foreach( var value in values )
    {
      if( needsSep )
      {
        AnsiConsole.Write( ", " );
      }
      else
      {
        needsSep = true;
      }

      AnsiConsole.Markup( $"[yellow]{value}[/]" );
    }
  }

  protected static void WriteList<T>(
    string prompt,
    IEnumerable<T> values )
  {
    // ReSharper disable once PossibleMultipleEnumeration
    if( !values.Any() )
    {
      return;
    }

    AnsiConsole.Write( prompt );

    // ReSharper disable once PossibleMultipleEnumeration
    WriteList( values );
    AnsiConsole.WriteLine();
  }

#endregion
}
