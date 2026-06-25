# Bach
[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)](https://opensource.org/licenses/MIT)
[![.NET](https://github.com/eddievelasquez/bach/actions/workflows/BuildAndTest.yml/badge.svg)](https://github.com/eddievelasquez/bach/actions/workflows/BuildAndTest.yml)

A Music Theory library for .NET
## Introduction
Bach is a .NET Core library that enables expressing Western tonal musical concepts in C#: 
Pitches, Intervals, Scales, Triads, Chords, Modes, etc.

This library has been a pet project of mine while learning a bit of music theory and, at the same time, 
keeping my C# and .NET skills up to date while I've been off doing C++ for quite a while.

## Usage
Scales and chords are created from root pitches and formulas that describe them; these formulas are 
maintained in an extensible registry.

For example, to list all the scale formulas in the registry
```csharp
foreach( var formula in Registry.ScaleFormulas )
{
  Console.WriteLine( $"Scale name '{formula.Name}', Intervals: '{formuula.Intervals}'" );
}
```

To display the intervals between a set of pitch classes:
```csharp
var pitches = PitchClassCollection.Parse( "C,D,F,A" );
var intervals = pitches.Intervals().Select( interval => interval.ToString( "Sq" ) );
Console.WriteLine( $"Intervals: {string.Join( ",", intervals )}" );
```

To obtain the list of scales that contain a set of pitch classes:
```csharp
var pitches = PitchClassCollection.Parse( "C,D,F#,A" );
foreach( var scale in Scale.ScalesContaining( pitches ) )
{
  Console.WriteLine( $"Scale name: '{scale.Name}'");
}
```
The included command line application demonstrates some of the available functionality:
* Dump the list of scales or chords in the registry,
* List the pitches and intervals in a scale or chord for a root pitch.
* Infer the intervals between a set of pitches.
* Infer the list of scales that contain a given set of pitches.

## Tonal context and harmonic motion
Bach also supports a few higher-level abstractions for tonal analysis and simple progression building:

- `Key` captures a tonic, mode, key signature, and the implied scale.
- `ScaleDegree` resolves degrees such as tonic, dominant, or subdominant within a key.
- `ChordProgression` collects a sequence of chords for analysis or composition.

These concepts work together naturally. For example, you can resolve the important scale degrees of a key and use them to build a chord progression:

```csharp
var key = new Key( PitchClass.C, ModeType.Major );

var tonic = ScaleDegree.Tonic.Resolve( key );
var subdominant = ScaleDegree.Subdominant.Resolve( key );
var dominant = ScaleDegree.Dominant.Resolve( key );

var progression = new ChordProgression(
[
  new Chord( tonic, "Major" ),
  new Chord( subdominant, "Major" ),
  new Chord( dominant, "Major" )
] );

Console.WriteLine( key );
Console.WriteLine( progression );
```

This produces a simple tonal progression anchored in the selected key while staying consistent with the library's existing pitch and scale abstractions.

## Contribution
* Feel free to make pull requests or fix any existing bugs.
* Feel free to open issues if you find music theory mistakes, bugs, or unexpected behavior.
