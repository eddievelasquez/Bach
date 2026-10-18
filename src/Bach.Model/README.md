# Bach.Model

Bach.Model is a .NET 10+ library for expressing Western tonal music concepts in C#.

## Install

```powershell
dotnet add package Bach.Model
```

## Quick start

```csharp
using Bach.Model.Harmony;
using Bach.Model.Pitches;

var chord = Chord.Parse( "Cmaj7" );

Console.WriteLine( $"Chord: {chord}" );
Console.WriteLine( $"Root: {chord.Root}" );
```

## Features

- Pitches, pitch classes, note names, accidentals, and intervals.
- Scales, modes, scale formulas, and scale-degree information.
- Triads, chords, chord progressions, keys, and applied functions.
- Immutable parts, measures, pitch events, and pitch chords.
- Tonal-center evaluation with ranked candidates and supporting evidence.
- Stringed-instrument definitions, tunings, and fingerings.
- JSON serialization support for model events.

Predefined scale, chord, and stringed-instrument definitions are stored in an immutable registry embedded in the assembly. The package does not require a separate registry file at runtime.

## Example

```csharp
using Bach.Model.Analysis;
using Bach.Model.Harmony;
using Bach.Model.Pitches;
using Bach.Model.Structure;

var key = new Key( PitchClass.C, ScaleDefinition.Major );
var part = Part.Parse( "C4,G4,B4,C5|G4,C5" );
var result = new TonalEvaluator().Evaluate( new PartEventScope( part ), [key] );

Console.WriteLine( result );
```

Tonal analysis uses ordered pitch events and pitch content. It does not currently use duration, rhythm, meter, voice, ties, rests, or written-versus-sounding notation context.

## API documentation

The full API reference and examples are available in the [Bach repository](https://github.com/eddievelasquez/bach):

- [API reference](https://github.com/eddievelasquez/bach/blob/main/docs/REFERENCE.md)
- [Repository README](https://github.com/eddievelasquez/bach#readme)

## Supported frameworks

Bach.Model explicitly supports .NET 10 and later.

## License

Bach.Model is released under the [MIT License](https://github.com/eddievelasquez/bach/blob/main/LICENSE).
