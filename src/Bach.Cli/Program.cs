// Module Name: Program.cs
// Project:     Bach.Cli
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

namespace Bach.Cli
{
  using System;
  using System.Linq;
  using McMaster.Extensions.CommandLineUtils;
  using Model;

  internal class Program
  {
    #region Public Methods

    public static int Main(string[] args)
    {
      var app = new CommandLineApplication { Name = "bach", Description = "CLI for Bach.Model" };

      try
      {
        app.HelpOption(true);

        app.OnExecute(() =>
        {
          app.ShowHelp();
          return 1;
        });

        app.Command("scale", DisplayScale);
        app.Command("chord", DisplayChord);
        app.Command("intervals", DisplayIntervals);
        app.Command("scales-containing", DisplayScalesForNotes);
        app.Command("list",
                    cmd =>
                    {
                      cmd.Command("scales", ListScales);
                      cmd.Command("chords", ListChords);
                    });

        return app.Execute(args);
      }
      catch( Exception ex )
      {
        Console.WriteLine($"ERROR: {ex.Message}");
        app.ShowHelp();
        return 1;
      }
    }

    #endregion

    #region  Implementation

    private static void ListScales(CommandLineApplication app)
    {
      app.OnExecute(() =>
      {
        Console.WriteLine("Scales:\n");

        foreach( ScaleFormula formula in Registry.ScaleFormulas )
        {
          Console.WriteLine($"{formula.Key} - \"{formula.Name}\"");
          Console.WriteLine($"  Formula: {formula.Intervals}");

          if( formula.Categories.Count > 0 )
          {
            Console.WriteLine("  Categories: " + string.Join(", ", formula.Categories));
          }

          if( formula.Aliases.Count > 0 )
          {
            Console.WriteLine("  Aliases: " + string.Join(", ", formula.Aliases));
          }

          Console.WriteLine();
        }
      });
    }

    private static void ListChords(CommandLineApplication app)
    {
      app.OnExecute(() =>
      {
        Console.WriteLine("Chords:\n");

        foreach( ChordFormula formula in Registry.ChordFormulas )
        {
          Console.WriteLine($"{formula.Key} - \"{formula.Name}\"");

          if( !string.IsNullOrEmpty(formula.Symbol) )
          {
            Console.WriteLine($"  Symbol: {formula.Symbol}");
          }

          Console.WriteLine($"  Formula: {formula.Intervals}");
          Console.WriteLine();
        }
      });
    }

    private static void DisplayScale(CommandLineApplication app)
    {
      CommandArgument formulaArg = app.Argument("scaleKey", "Required. The key of scale formula").IsRequired();
      CommandArgument rootArg = app.Argument("root", "Required. The Scale root", true).IsRequired();

      app.OnExecute(() =>
      {
        ScaleFormula formula = Registry.ScaleFormulas[formulaArg.Value];

        Console.WriteLine($"{formula.Name} scale => {formula.Intervals}");
        Console.WriteLine();

        foreach( string rootValue in rootArg.Values )
        {
          Note root = Note.Parse(rootValue);
          var scale = new Scale(root, formula);
          Console.WriteLine($"  {root} {formula.Name} {{{scale.Notes}}}");
        }

        Console.WriteLine();
      });
    }

    private static void DisplayChord(CommandLineApplication app)
    {
      CommandArgument formulaArg = app.Argument("chordKey", "The key of chord formula").IsRequired();
      CommandArgument rootArg = app.Argument("root", "Chord root", true).IsRequired();

      app.OnExecute(() =>
      {
        ChordFormula formula = Registry.ChordFormulas[formulaArg.Value];

        Console.WriteLine($"{formula.Name} chord => {formula.Intervals}");
        Console.WriteLine();

        foreach( string rootValue in rootArg.Values )
        {
          Note root = Note.Parse(rootValue);
          var chord = new Chord(root, formula);

          Console.WriteLine($"  {chord} {{{chord.Notes}}}");
        }

        Console.WriteLine();
      });
    }

    private static void DisplayIntervals(CommandLineApplication app)
    {
      CommandArgument notesArg = app.Argument("Notes", "Notes", true).IsRequired();

      app.OnExecute(() =>
      {
        Note[] notes = notesArg.Values.Select(Note.Parse).ToArray();
        Interval[] intervals = notes.Intervals().ToArray();

        Console.WriteLine($"Notes: {{{string.Join(",", notes)}}}");
        Console.WriteLine($"    => {{{string.Join(",", intervals.Select(interval => interval.ToString("Sq")))}}}");
        Console.WriteLine();
      });
    }

    private static void DisplayScalesForNotes(CommandLineApplication app)
    {
      CommandArgument notesArg = app.Argument("Notes", "Notes", true).IsRequired();

      app.OnExecute(() =>
      {
        Note[] notes = notesArg.Values.Select(Note.Parse).ToArray();

        Console.WriteLine($"Scales containing: {{{string.Join(",", notes)}}}");
        Console.WriteLine();

        foreach( Scale scale in Scale.ScalesContaining(notes) )
        {
          Console.WriteLine($"  {scale.Name} {{{string.Join(",", scale.Notes)}}}");
          Console.WriteLine($"    => {{{string.Join(",", scale.Formula.Intervals.Select(interval => interval.ToString("Sq")))}}}");
          Console.WriteLine();
        }
      });
    }

    #endregion
  }
}
