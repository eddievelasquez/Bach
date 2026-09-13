# Bach
[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)](https://opensource.org/licenses/MIT)
[![.NET](https://github.com/eddievelasquez/bach/actions/workflows/BuildAndTest.yml/badge.svg)](https://github.com/eddievelasquez/bach/actions/workflows/BuildAndTest.yml)

A music theory library for .NET
## Introduction
Bach is a .NET library for expressing Western tonal music concepts in C#:
pitches, intervals, scales, triads, chords, and modes.

This library is a personal project for learning music theory and keeping C# and .NET skills current
while working with C++.

## Usage
Scales and chords are created from root pitches and formulas that describe them; these formulas are 
maintained in an extensible registry.

For example, to list all scale formulas in the registry:
```csharp
foreach( var formula in Registry.ScaleFormulas )
{
  Console.WriteLine( $"Scale name '{formula.Name}', Intervals: '{formula.Intervals}'" );
}
```

To display the intervals between a set of pitch classes:
```csharp
var pitches = new[] { PitchClass.C, PitchClass.D, PitchClass.F, PitchClass.A };
var intervals = pitches.Intervals().Select( interval => interval.ToString( "Sq" ) );
Console.WriteLine( $"Intervals: {string.Join( ",", intervals )}" );
```

To parse a chord and inspect its pitch classes:
```csharp
var chord = Chord.Parse( "Cmaj7" );
Console.WriteLine( $"Chord: {chord}, Root: {chord.Root}" );
```

To create an immutable part from ordered events:
```csharp
var part = new PartBuilder()
  .AddMeasure( measure => measure.Add( Pitch.Parse( "C4" ) )
                            .Add( PitchChord.Parse( "G7" ) ) )
  .Build();
```

To create a registered stringed instrument:
```csharp
var guitar = StringedInstrument.Create( "guitar", 22 );
Console.WriteLine( $"Strings: {guitar.Tuning.Count}, Positions: {guitar.PositionCount}" );
```

To get the list of scales or modes whose pitch content contains a set of pitch classes:
```csharp
var pitches = new[] { PitchClass.C, PitchClass.D, PitchClass.FSharp, PitchClass.A };
foreach( var scale in Scale.ScalesContaining( pitches ) )
{
  Console.WriteLine( $"Scale name: '{scale.Name}'");
}
```
`Scale.ScalesContaining` performs scale or mode matching only. It does not infer a tonal center. Use
`TonalEvaluator` for ranked tonal candidates and evidence.
The command-line application demonstrates these functions:
* List the scales or chords in the registry.
* List the pitches and intervals in a scale or chord for a root pitch.
* Find the intervals between a set of pitches.
* Find the scales that contain a set of pitches.

## Tonal context and harmonic motion
Bach supports higher-level abstractions for keys and duration-free tonal analysis. A `Key` contains a
spelled tonic, a governing scale or mode, a key signature, and the implied scale. The evaluator returns
ranked candidates with supporting and conflicting evidence:

```csharp
using Bach.Model.Analysis;
using Bach.Model.Harmony;
using Bach.Model.Pitches;
using Bach.Model.Structure;
using Bach.Model.Analysis;

var key = new Key( PitchClass.C, ScaleDefinition.Major );
var part = Part.Parse( "C4,G4,B4,C5|G4,C5" );
var scope = new PartEventScope( part );
var result = new TonalEvaluator().Evaluate( scope, [key] );

Console.WriteLine( result );
```

The analysis uses the ordered pitch events in the scope. It does not use duration or rhythmic evidence.

## Tonal analysis contract
The tonal-analysis model uses 12-tone equal temperament (12-TET) for pitch-class and semitone
relationships. Pitch spelling remains significant. For example, C-sharp and D-flat can sound at the
same 12-TET pitch but remain different spellings in analysis.

Analysis input is an immutable, ordered `Part`. A `PartEventScope` selects all events or a standard
.NET `Range` of events. The scope preserves event order and pitch-class duplicates. `Pitch` and
`PitchChord` provide the pitch content. Chord roots, basses, inversions, and formulas are available
when the input contains a chord event.

`TonalEvaluator` compares the input with candidate keys. It uses scale or mode membership, pitch
spelling, harmonic relationships, tonal-center patterns, and an optional `RepertoireProfile`. The
result is a ranked set of candidates. Each candidate has a key, rank, confidence, supporting evidence,
and conflicting evidence. The evaluator can also copy supplied `AppliedFunction` annotations into the
candidate evidence. It does not infer applied functions from unlabeled input.

An analysis can return `InconclusiveTonalAnalysisResult` when the input has no usable pitch content or
when no candidate matches. An inconclusive result keeps the analyzed scope and explains the outcome.

The current contract excludes duration, rhythm, meter, voice, transposition, ties, rests, and written
or sounding notation context. These features are planned for later modeling and do not affect the
current evidence score.

`Scale.ScalesContaining` performs scale or mode matching. It finds formulas whose pitch content contains
the supplied set. It is a set-discovery method, not tonal inference. Use `TonalEvaluator` when the goal
is to rank likely tonal centers.

## Reference

- [API reference](docs/REFERENCE.md)
- [Namespace mapping](docs/namespace-mapping.md)

## Contribution
* Submit pull requests for improvements or bug fixes.
* Open an issue for music theory errors, bugs, or unexpected behavior.
