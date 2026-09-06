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
var pitches = PitchClassCollection.Parse( "C,D,F,A" );
var intervals = pitches.Intervals().Select( interval => interval.ToString( "Sq" ) );
Console.WriteLine( $"Intervals: {string.Join( ",", intervals )}" );
```

To get the list of scales that contain a set of pitch classes:
```csharp
var pitches = PitchClassCollection.Parse( "C,D,F#,A" );
foreach( var scale in Scale.ScalesContaining( pitches ) )
{
  Console.WriteLine( $"Scale name: '{scale.Name}'");
}
```
The command-line application demonstrates these functions:
* List the scales or chords in the registry.
* List the pitches and intervals in a scale or chord for a root pitch.
* Find the intervals between a set of pitches.
* Find the scales that contain a set of pitches.

## Tonal context and harmonic motion
Bach also supports higher-level abstractions for tonal analysis and progression building:

- `Key` captures a tonic, mode, key signature, and the implied scale.
- `ScaleDegree` resolves degrees such as tonic, dominant, or subdominant within a key.
- `ChordProgression` collects a sequence of chords for analysis or composition.

These concepts work together. For example, you can resolve the main scale degrees of a key and use them to build a chord progression:

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

This produces a tonal progression anchored in the selected key and consistent with the library's pitch and scale abstractions.

## Contribution
* Submit pull requests for improvements or bug fixes.
* Open an issue for music theory errors, bugs, or unexpected behavior.
