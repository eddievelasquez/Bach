# Bach.Model Namespace Mapping

## Status

This document records the breaking namespace reorganization implemented for the public
types in `src/Bach.Model`. It is a reference for the completed changes.

## Goals

The implemented layout groups public types by musical domain and separates foundational
concepts, musical structure, analysis, instruments, library data, and serialization.

The implementation removes public domain types from the catch-all `Bach.Model` namespace.
The `Bach.Model` namespace remains available as the library root, but it does not contain
public model types after the refactor.

## Implemented public namespaces

- `Bach.Model.Pitches`
- `Bach.Model.Intervals`
- `Bach.Model.Scales`
- `Bach.Model.Harmony`
- `Bach.Model.Structure`
- `Bach.Model.Analysis`
- `Bach.Model.Instruments`
- `Bach.Model.Library`
- `Bach.Model.Serialization`

Implementation-only namespaces remain nested under their owning domain where appropriate.

## Namespace mapping

The tables below list public types that moved to new namespaces in this version.
Newly added public types are listed separately below.

## Public types moved in this version

| Type | Previous namespace | Implemented namespace |
| --- | --- | --- |
| `Accidental` | `Bach.Model` | `Bach.Model.Pitches` |
| `Chord` | `Bach.Model` | `Bach.Model.Harmony` |
| `ChordFormula` | `Bach.Model` | `Bach.Model.Harmony` |
| `Formula` | `Bach.Model` | `Bach.Model.Scales` |
| `Interval` | `Bach.Model` | `Bach.Model.Intervals` |
| `IntervalCollection` | `Bach.Model` | `Bach.Model.Intervals` |
| `IntervalMatch` | `Bach.Model` | `Bach.Model.Intervals` |
| `IntervalQuality` | `Bach.Model` | `Bach.Model.Intervals` |
| `IntervalQuantity` | `Bach.Model` | `Bach.Model.Intervals` |
| `Mode` | `Bach.Model` | `Bach.Model.Scales` |
| `ModeFormula` | `Bach.Model` | `Bach.Model.Scales` |
| `NoteName` | `Bach.Model` | `Bach.Model.Pitches` |
| `Pitch` | `Bach.Model` | `Bach.Model.Pitches` |
| `PitchClass` | `Bach.Model` | `Bach.Model.Pitches` |
| `PitchCollection<T>` | `Bach.Model` | `Bach.Model.Pitches` |
| `Registry` | `Bach.Model` | `Bach.Model.Library` |
| `Scale` | `Bach.Model` | `Bach.Model.Scales` |
| `ScaleCategory` | `Bach.Model` | `Bach.Model.Scales` |
| `ScaleFormula` | `Bach.Model` | `Bach.Model.Scales` |
| `ScaleFormulaBuilder` | `Bach.Model` | `Bach.Model.Scales` |
| `Triad` | `Bach.Model` | `Bach.Model.Harmony` |
| `TriadQuality` | `Bach.Model` | `Bach.Model.Harmony` |

`Formula` is used by scale and chord formulas. It is placed with scales because scale
formulas are the primary registry and domain use, while harmony can reference it through
that namespace. `PitchCollection<T>` represents pitch content and is placed with pitch
primitives.

## Newly added public types

These types are new in this version. Their addition is not a namespace breaking change
for consumers of the previous release.

| Namespace | Types |
| --- | --- |
| `Bach.Model.Analysis` | `AlteredDegree`, `AlteredDegreeKind`, `AnalysisTarget`, `Anticipation`, `AppliedFunction`, `BluesInflection`, `CadentialPattern`, `EvidenceReason`, `EvidenceReasonCategory`, `InconclusiveTonalAnalysisResult`, `InterpretationKinds`, `ITonalEvidenceEvaluator`, `ITonalEvidenceEvaluatorProvider`, `MelodicDirection`, `ModalInterchange`, `NeighborTone`, `NeighborToneDirection`, `NonChordTone`, `PartEventScope`, `PassingTone`, `PedalTone`, `PitchClassEvidenceEvaluator`, `RankedTonalCandidateResult`, `RepertoireProfile`, `RootlessJazzVoicing`, `Suspension`, `TonalAnalysisResult`, `TonalAnalysisResultBuilder`, `TonalAnalysisResultSet`, `TonalAnalysisResultSetBuilder`, `TonalCandidateSource`, `TonalCandidateStatus`, `TonalEvaluationOptions`, `TonalEvaluator`, `TonalEvidence`, `TonalEvidenceContext`, `TonalEvidenceEvaluator`, `TonalEvidenceEvaluatorPipeline`, `TonalEvidenceEvaluatorPipelineBuilder` |
| `Bach.Model.Harmony` | `AppliedTriad`, `AppliedTriadFunction`, `ChordChart`, `ChordProgression`, `DegreeAlteration`, `IChord`, `IChordEvent`, `IChordFactory`, `IChordParser`, `PitchChord` |
| `Bach.Model.Intervals` | `IntervalExtensions`, `IntervalQualityExtensions`, `IntervalQuantityExtensions` |
| `Bach.Model.Pitches` | `AccidentalExtensions`, `IPitch`, `NoteNameExtensions` |
| `Bach.Model.Scales` | `ScaleClassification`, `ScaleClassificationBuilder`, `ScaleDefinition`, `ScaleDegree`, `ScaleDegreeStep`, `ScaleTag`, `StepCollection` |
| `Bach.Model.Serialization` | `JsonSerializerOptionsExtensions` |
| `Bach.Model.Structure` | `IPartEvent`, `Measure`, `MeasureBuilder`, `Part`, `PartBuilder`, `PartEventLocation` |

## Instrument types

| Type | Previous namespace | Implemented namespace |
| --- | --- | --- |
| `Fingering` | `Bach.Model.Instruments` | `Bach.Model.Instruments` |
| `Instrument` | `Bach.Model.Instruments` | `Bach.Model.Instruments` |
| `InstrumentDefinition` | `Bach.Model.Instruments` | `Bach.Model.Instruments` |
| `StringedInstrument` | `Bach.Model.Instruments` | `Bach.Model.Instruments` |
| `StringedInstrumentDefinition` | `Bach.Model.Instruments` | `Bach.Model.Instruments` |
| `StringedInstrumentDefinitionBuilder` | `Bach.Model.Instruments` | `Bach.Model.Instruments` |
| `Tuning` | `Bach.Model.Instruments` | `Bach.Model.Instruments` |
| `TuningCollection` | `Bach.Model.Instruments` | `Bach.Model.Instruments` |
| `InstrumentDefinitionState` | `Bach.Model.Instruments.Internal` | `Bach.Model.Instruments.Internal` |
| `StringedInstrumentDefinitionState` | `Bach.Model.Instruments.Internal` | `Bach.Model.Instruments.Internal` |

## Library and registry types

| Type | Previous namespace | Implemented namespace |
| --- | --- | --- |
| `Registry` | `Bach.Model` | `Bach.Model.Library` |

`Registry` loads the library data and exposes scale, chord, and instrument definitions. It
is a library/catalog boundary, not a scale, harmony, or instrument type.

## Serialization types

The persistence records and converters are implementation details. The public
`JsonSerializerOptionsExtensions` type is listed under newly added public types above.

## General internal types

| Type | Previous namespace | Implemented namespace |
| --- | --- | --- |
| `ArgumentExceptionExtensions` | `Bach.Model.Internal` | `Bach.Model.Internal` |
| `ArrayExtensions` | `Bach.Model.Internal` | `Bach.Model.Internal` |
| `CircularArray` | `Bach.Model.Internal` | `Bach.Model.Internal` |
| `CollectionExtensions` | `Bach.Model.Internal` | `Bach.Model.Internal` |
| `Comparer` | `Bach.Model.Internal` | `Bach.Model.Internal` |
| `Constants` | `Bach.Model.Internal` | `Bach.Model.Internal` |
| `INamedObject` | `Bach.Model.Internal` | `Bach.Model.Internal` |
| `ISpanConsumingParsable` | `Bach.Model.Internal` | `Bach.Model.Internal` |
| `Lookup` | `Bach.Model.Internal` | `Bach.Model.Internal` |
| `NamedObjectCollection` | `Bach.Model.Internal` | `Bach.Model.Internal` |
| `ParseHelperExtensions` | `Bach.Model.Internal` | `Bach.Model.Internal` |
| `Result` | `Bach.Model.Internal` | `Bach.Model.Internal` |

Internal types are not part of the public namespace design. They should remain hidden
from consumers and may be reorganized separately if implementation dependencies require it.

## Nested and private types

Nested and private types are not independently addressable public API types. They should
move with their containing type and keep the containing type's implemented namespace. This
applies to private implementation types such as:

- `Formula.IntervalComparer`
- `Formula.SemitoneCountIntervalComparer`
- `Pitch.EnharmonicEqualityComparer`
- `PitchClass.EnharmonicEqualityComparer`
- `StringedInstrumentDefinitionBuilder.TuningInfo`
- `TonalEvaluator.CandidateScore`

Private nested types do not require separate consumer migration entries.

## Special placement decisions

### No public types in `Bach.Model`

The root namespace is not used as a catch-all. Every public type receives a domain
namespace. This creates a clear rule for future additions: a type must belong to a domain
before it is added to the public API.

### `Formula`

`Formula` is shared by scale and chord formulas. It is implemented in `Bach.Model.Scales`
because scale formulas are its primary domain use. Harmony references it through the
scales namespace.

### `PitchCollection<T>`

`PitchCollection<T>` represents pitch content and is used by multiple musical domains. It
belongs with pitch primitives rather than remaining in the root namespace.

### Key and harmonic context

`Key`, `KeySignature`, and `DegreeAlteration` are placed in `Bach.Model.Harmony` rather
than a small `Bach.Model.Tonal` namespace. They describe the harmonic context used by
scales, chord resolution, chord charts, and analysis.

### Analysis scope

`PartEventScope` is placed in `Bach.Model.Analysis` because it selects analysis input and
carries analytical annotations. `Part`, `Measure`, and event contracts remain in
`Bach.Model.Structure`.

### Registry

`Registry` is placed in `Bach.Model.Library` because it loads and exposes the bundled
library data across scales, chords, and instruments.

## Dependency direction

The intended dependency direction is:

```text
Pitches and Intervals
        ↓
Scales
        ↓
Harmony and Structure
        ↓
Analysis

Instruments and Library support the domain model.
Serialization supports Library and persistence boundaries.
Internal namespaces support their owning public domains.
```

This is a conceptual direction. The implementation should verify individual references,
especially `Registry`, `PartEventScope`, and formula abstractions, before moving files.

## Breaking-change impact

This is a breaking public API change. Consumers will need to update:

- `using` directives and fully qualified type names.
- Model-project references between the new namespaces.
- Test namespaces and imports. Test namespaces should mirror production namespaces with
  `.Test` appended.
- CLI imports in command files that currently use `Bach.Model`.
- README examples and API reference headings.
- XML documentation links and generated API documentation.
- Any source generators, analyzers, or external annotations that refer to full type names.

The project file, target framework, package versions, and library data file should not need
changes solely because of the namespace reorganization.
