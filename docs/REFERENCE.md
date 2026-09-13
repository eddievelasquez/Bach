# Reference

## Accidental

### Summary
Represents a natural, flat, sharp, double-flat, or double-sharp alteration.

### Syntax
```csharp
public readonly struct Accidental
```

## NoteName

### Summary
Represents one of the seven diatonic note names, from C through B.

### Syntax
```csharp
public readonly struct NoteName
```

## PitchClass

### Summary
Represents a spelled pitch class with a note name and accidental.

### Syntax
```csharp
public readonly struct PitchClass
```

### Remarks

- Equality and ordering are spelling-sensitive. C-sharp and D-flat are not equal.
- Use `EnharmonicEquals`, `EnharmonicCompareTo`, and `EnharmonicComparer` for sounding-pitch comparisons.

## Pitch

### Summary
Represents a pitch class at a supported scientific-pitch-notation octave.

### Syntax
```csharp
public readonly struct Pitch : IPartEvent
```

### Methods

- `Parse(string)` - Parses a pitch such as `C4`.
- `TryParse(string?, out Pitch)` - Attempts to parse a pitch without throwing for invalid input.

### Remarks

- Supported octaves range from 0 through 9.
- A pitch provides its pitch class as event pitch content.

## IPitch<TPitch>

### Summary
Defines the common operations for pitch and pitch-class value types.

### Syntax
```csharp
public interface IPitch<TPitch>
```

## Interval

### Summary
Represents a spelled musical interval with quantity, quality, direction, and semitone count.

### Syntax
```csharp
public readonly struct Interval
```

### Methods

- `Parse(string)` - Parses a formatted interval.
- `TryParse(string?, out Interval)` - Attempts to parse a formatted interval.

### Remarks

- Spelled intervals preserve distinctions such as augmented second and minor third.

## IntervalQuantity

### Summary
Identifies the diatonic quantity of an interval.

### Syntax
```csharp
public enum IntervalQuantity
```

## IntervalQuality

### Summary
Identifies the quality of an interval, such as perfect, major, minor, augmented, or diminished.

### Syntax
```csharp
public enum IntervalQuality
```

## IntervalMatch

### Summary
Selects exact or semitone-based interval matching.

### Syntax
```csharp
public enum IntervalMatch
```

## IntervalCollection

### Summary
Stores an immutable ordered collection of intervals used by formulas.

### Syntax
```csharp
public sealed class IntervalCollection
```

## IntervalExtensions

### Summary
Provides interval sequence operations, including semitone-step calculation.

### Syntax
```csharp
public static class IntervalExtensions
```

## IntervalQuantityExtensions

### Summary
Provides formatting and calculation operations for interval quantities.

### Syntax
```csharp
public static class IntervalQuantityExtensions
```

## IntervalQualityExtensions

### Summary
Provides formatting and classification operations for interval qualities.

### Syntax
```csharp
public static class IntervalQualityExtensions
```

## PitchCollection<TPitch>

### Summary
Provides an immutable indexed collection of pitch values.

### Syntax
```csharp
public abstract class PitchCollection<TPitch> : IReadOnlyList<TPitch>
```

### Remarks

- The collection preserves order and exposes `Count`, an indexer, enumeration, and `IndexOf`.

## Formula

### Summary
Defines a named, identified sequence of intervals used to construct musical content.

### Syntax
```csharp
public abstract class Formula
```

### Properties

| Property | Type | Description |
| --- | --- | --- |
| `Id` | `string` | Gets the language-neutral formula identifier. |
| `Name` | `string` | Gets the formula name. |
| `Intervals` | `IntervalCollection` | Gets the intervals in the formula. |

### Methods

- `Contains(IEnumerable<Interval>, IntervalMatch)` - Tests whether the formula contains the supplied intervals.

## ScaleFormula

### Summary
Defines the spelled degrees and classification of a scale or mode.

### Syntax
```csharp
public class ScaleFormula : Formula
```

### Properties

| Property | Type | Description |
| --- | --- | --- |
| `AscendingDegrees` | `IReadOnlyList<ScaleDegreeStep>` | Gets the authoritative ascending degrees. |
| `DescendingDegrees` | `IReadOnlyList<ScaleDegreeStep>` | Gets the authoritative descending degrees. |
| `Classification` | `ScaleClassification` | Gets structural and repertoire metadata. |
| `Categories` | `IReadOnlySet<string>` | Gets the calculated and repertoire category names. |
| `Aliases` | `IReadOnlySet<string>` | Gets optional registry aliases. |

### Methods

- `GetSemitoneSteps()` - Calculates the ascending semitone steps from the spelled degrees.

## ScaleFormulaBuilder

### Summary
Builds validated `ScaleFormula` instances from spelled degree data.

### Syntax
```csharp
public sealed class ScaleFormulaBuilder
```

### Remarks

- The builder validates required degree data, ordering, cardinality, and supplied direction data.
- Registry loading is the normal public source of scale formulas.

## ScaleDegreeStep

### Summary
Associates a one-based scale-degree ordinal with its spelled tonic-relative interval.

### Syntax
```csharp
public readonly struct ScaleDegreeStep
```

## Scale

### Summary
Represents the ordered pitch classes generated from a root and scale formula.

### Syntax
```csharp
public sealed class Scale : PitchCollection<PitchClass>
```

### Constructors

- `Scale(PitchClass root, ScaleFormula formula)` - Creates a scale from a formula.
- `Scale(PitchClass root, string formulaIdOrName)` - Creates a scale from a registry formula identifier or name.

### Properties

| Property | Type | Description |
| --- | --- | --- |
| `Root` | `PitchClass` | Gets the scale root. |
| `Name` | `string` | Gets the generated scale name. |
| `Formula` | `ScaleFormula` | Gets the governing formula. |
| `Theoretical` | `bool` | Indicates whether the spelling uses a double accidental. |

### Methods

- `ScalesContaining(IEnumerable<PitchClass>)` - Finds scales or modes whose pitch content contains the supplied set. This is matching, not tonal inference.
- `GetAscending()` - Enumerates the scale in ascending direction.
- `GetDescending()` - Enumerates the scale in descending direction.

## Mode

### Summary
Represents a named rotation or mode of a scale formula.

### Syntax
```csharp
public sealed class Mode
```

## ModeFormula

### Summary
Represents the formula data for a mode and its modal rotation.

### Syntax
```csharp
public sealed class ModeFormula : IEquatable<ModeFormula>
```

## ScaleDefinition

### Summary
Identifies the governing scale or mode definition used by a `Key`.

### Syntax
```csharp
public sealed class ScaleDefinition
```

### Remarks

- Predefined definitions include major, natural minor, harmonic minor, melodic minor, and church modes.
- A definition identifies its registry formula and supports directional formula data.

## ScaleCategory

### Summary
Identifies calculated structural categories for a scale formula.

### Syntax
```csharp
public enum ScaleCategory
```

## ScaleTag

### Summary
Identifies a repertoire classification tag for a scale formula.

### Syntax
```csharp
public enum ScaleTag
```

## ScaleClassification

### Summary
Stores structural classification, modal relationship, repertoire tags, and key-candidate metadata.

### Syntax
```csharp
public sealed record ScaleClassification
```

## ScaleClassificationBuilder

### Summary
Builds scale classification metadata for registry formulas.

### Syntax
```csharp
public sealed class ScaleClassificationBuilder
```

## ScaleDegree

### Summary
Represents a diatonic scale degree and its conventional symbol.

### Syntax
```csharp
public readonly struct ScaleDegree : IParsable<ScaleDegree>
```

### Methods

- `Resolve(Key)` - Resolves the degree in the key's governing scale or mode.
- `ResolveDiatonicTriad(Key, MelodicDirection)` - Resolves the scale-degree triad for the selected direction.

## DegreeAlteration

### Summary
Represents a local alteration applied to a scale degree in a key.

### Syntax
```csharp
public readonly struct DegreeAlteration
```

## KeySignature

### Summary
Represents the conventional signature associated with a key.

### Syntax
```csharp
public readonly struct KeySignature
```

## Key

### Summary
Represents a spelled tonic, governing scale or mode, derived key signature, scale, and local degree alterations.

### Syntax
```csharp
public sealed class Key
```

### Constructors

- `Key(PitchClass pitchClass, ScaleDefinition scaleDefinition, IEnumerable<DegreeAlteration>? alterations = null)` - Creates a key from a tonic and governing scale or mode.

### Properties

| Property | Type | Description |
| --- | --- | --- |
| `Tonic` | `PitchClass` | Gets the spelled tonic. |
| `ScaleDefinition` | `ScaleDefinition` | Gets the governing scale or mode. |
| `KeySignature` | `KeySignature` | Gets the derived key signature. |
| `Scale` | `Scale` | Gets the scale implied by the key. |
| `DegreeAlterations` | `IReadOnlyList<DegreeAlteration>` | Gets local degree alterations. |

## TriadQuality

### Summary
Identifies major, minor, diminished, or augmented triad quality.

### Syntax
```csharp
public enum TriadQuality
```

## Triad

### Summary
Represents a three-note chord generated from a root and triad quality.

### Syntax
```csharp
public sealed class Triad : Chord
```

## AppliedTriadFunction

### Summary
Identifies an applied dominant or applied leading-tone triad function.

### Syntax
```csharp
public enum AppliedTriadFunction
```

## AppliedTriad

### Summary
Stores an applied triad, its target degree, and its applied function.

### Syntax
```csharp
public sealed class AppliedTriad
```

## ChordFormula

### Summary
Defines the intervals and symbol for a chord.

### Syntax
```csharp
public sealed class ChordFormula : Formula
```

## IChord<TChord, TPitch>

### Summary
Defines common root, formula, inversion, and pitch-content access for chord types.

### Syntax
```csharp
public interface IChord<TChord, out TPitch>
```

## IChordFactory<TChord, TPitch>

### Summary
Defines a factory contract for creating typed chords.

### Syntax
```csharp
public interface IChordFactory<out TChord, in TPitch>
```

## IChordParser<TChord>

### Summary
Defines a parsing contract for typed chord values.

### Syntax
```csharp
public interface IChordParser<TChord>
```

## IChordEvent

### Summary
Exposes chord metadata for a part event without duplicating chord state.

### Syntax
```csharp
public interface IChordEvent
```

## Chord<TSelf, TPitch>

### Summary
Provides the generic base implementation for typed chords.

### Syntax
```csharp
public abstract class Chord<TSelf, TPitch>
```

## Chord

### Summary
Represents a chord of pitch classes generated from a root, formula, and inversion.

### Syntax
```csharp
public class Chord : Chord<Chord, PitchClass>
```

### Methods

- `Parse(string)` - Parses a chord symbol.
- `TryParse(string?, out Chord)` - Attempts to parse a chord symbol.

## PitchChord

### Summary
Represents a chord whose content contains octave-specific pitches and part-event metadata.

### Syntax
```csharp
public class PitchChord : Chord<PitchChord, Pitch>
```

## ChordProgression

### Summary
Stores an ordered immutable sequence of chords.

### Syntax
```csharp
public sealed class ChordProgression
```

## ChordChart

### Summary
Represents a named collection of chord formulas or chord symbols.

### Syntax
```csharp
public sealed class ChordChart
```

## IPartEvent

### Summary
Defines pitch content for an ordered part event.

### Syntax
```csharp
public interface IPartEvent
```

## PartEventLocation

### Summary
Identifies a zero-based measure and event position in a part.

### Syntax
```csharp
public readonly record struct PartEventLocation
```

## Measure

### Summary
Stores an immutable ordered collection of part events for one measure.

### Syntax
```csharp
public sealed class Measure : IReadOnlyList<IPartEvent>
```

## MeasureBuilder

### Summary
Builds immutable measures from ordered part events.

### Syntax
```csharp
public sealed class MeasureBuilder
```

## Part

### Summary
Stores an immutable ordered sequence of measures and exposes flattened event and pitch-class views.

### Syntax
```csharp
public sealed class Part : IReadOnlyList<Measure>
```

### Methods

- `Parse(string)` - Parses measures and events from the part text format.
- `TryParse(string?, out Part)` - Attempts to parse a part without throwing for invalid input.

## PartBuilder

### Summary
Builds immutable parts from ordered measures.

### Syntax
```csharp
public sealed class PartBuilder
```

### Methods

- `AddMeasure(Action<MeasureBuilder>)` - Adds a configured measure.
- `Build()` - Creates an immutable `Part`.

## StepCollection

### Summary
Parses a supported string of scale steps into step characters.

### Syntax
```csharp
public static class StepCollection
```

### Remarks

- Parsing checks the supported step count and step characters.
- Parsing does not construct a scale or validate octave closure and other formula rules.

## Registry

### Summary
Provides access to the predefined scale formulas, chord formulas, and stringed-instrument definitions.

### Syntax
```csharp
public static class Registry
```

### Properties

| Property | Type | Description |
| --- | --- | --- |
| `ScaleFormulas` | `NamedObjectCollection<ScaleFormula>` | Gets the registry scale formulas. |
| `ChordFormulas` | `NamedObjectCollection<ChordFormula>` | Gets the registry chord formulas. |
| `StringedInstrumentDefinitions` | `NamedObjectCollection<StringedInstrumentDefinition>` | Gets the registry instrument definitions. |

### Methods

- `TryGetScaleFormula(string, out ScaleFormula)` - Finds a scale formula by identifier or name.
- `TryGetChordFormula(string, out ChordFormula)` - Finds a chord formula by identifier or name.
- `TryGetChordFormulaBySymbol(string, out ChordFormula)` - Finds a chord formula by symbol.

### Remarks

- Registry data is loaded from `Bach.Model.Library.json`.
- Registry collections are read-only views for consumers.

## InstrumentDefinition

### Summary
Defines the name and metadata for a registered instrument.

### Syntax
```csharp
public abstract class InstrumentDefinition
```

## Instrument

### Summary
Represents an instrument created from an instrument definition.

### Syntax
```csharp
public abstract class Instrument
```

## StringedInstrumentDefinition

### Summary
Defines a stringed instrument and its available tunings.

### Syntax
```csharp
public sealed class StringedInstrumentDefinition
```

## StringedInstrumentDefinitionBuilder

### Summary
Builds a stringed-instrument definition with strings and tunings.

### Syntax
```csharp
public sealed class StringedInstrumentDefinitionBuilder
```

## StringedInstrument

### Summary
Represents a stringed instrument with a tuning and a position count.

### Syntax
```csharp
public sealed class StringedInstrument : Instrument
```

### Methods

- `Create(StringedInstrumentDefinition, int, Tuning?)` - Creates an instrument with the specified position count and optional tuning.

### Remarks

- Position count must be at least one.
- When tuning is omitted, the definition's standard tuning is used.

## Tuning

### Summary
Represents the ordered open-string pitches of a stringed instrument.

### Syntax
```csharp
public sealed class Tuning
```

## TuningCollection

### Summary
Provides a read-only name-to-tuning collection.

### Syntax
```csharp
public sealed class TuningCollection : IReadOnlyDictionary<string, Tuning>
```

## Fingering

### Summary
Represents a stringed-instrument fingering position.

### Syntax
```csharp
public readonly struct Fingering
```

## JsonSerializerOptionsExtensions

### Summary
Provides JSON configuration extensions for Bach.Model types.

### Syntax
```csharp
public static class JsonSerializerOptionsExtensions
```

### Methods

- The extensions register the supported part-event JSON converters with `System.Text.Json` options.

### Remarks

- JSON support is optional. The model does not require a JSON serializer for normal construction or analysis.

## PartEventScope

### Summary
Selects an ordered range of events from an immutable `Part`. The scope also stores optional applied-function
annotations for events in the selected range.

### Syntax
```csharp
public sealed class PartEventScope
```

### Constructors

- `PartEventScope(Part source)` - Selects all events in the source part.
- `PartEventScope(Part source, Range range, IEnumerable<AppliedFunction>? appliedFunctions = null)` - Selects the start-inclusive and end-exclusive range. Each annotation target must refer to an event in the range.

### Properties

| Property | Type | Description |
| --- | --- | --- |
| `Source` | `Part` | Gets the immutable source part. |
| `Range` | `Range` | Gets the event range represented by the scope. |
| `AppliedFunctions` | `IReadOnlyList<AppliedFunction>` | Gets the copied applied-function annotations. |
| `Events` | `IEnumerable<IPartEvent>` | Gets the events selected by the range, in order. |
| `Locations` | `IEnumerable<PartEventLocation>` | Gets the source location for each selected event. |

### Remarks

- The constructor throws when the source is null or the range is invalid.
- The constructor throws when an annotation is null or targets an event outside the scope.
- The scope does not add duration, rhythm, meter, voice, rest, tie, or notation state.

## TonalEvaluator

### Summary
Evaluates an event scope and returns ranked tonal candidates. The evaluator is duration-free.

### Syntax
```csharp
public sealed class TonalEvaluator
```

### Constructors

- `TonalEvaluator(RepertoireProfile? profile = null, TonalEvidenceEvaluatorPipeline? pipeline = null)` - Uses the default repertoire profile and evidence pipeline when they are not supplied.

### Methods

- `Evaluate(PartEventScope scope, IEnumerable<Key>? additionalCandidates = null, TonalEvaluationOptions? options = null)` - Scores the scope and returns a `TonalAnalysisResult`. Additional keys are considered with the registry candidates.

### Remarks

- The default candidate source uses registry formulas marked as key candidates and supported spelled tonic values.
- The evaluator records supporting and conflicting evidence. It does not infer supplied applied functions.
- The evaluator returns an inconclusive result for an empty scope, missing pitch content, or no matching candidate.

## TonalAnalysisResult

### Summary
Provides the common result scope for a conclusive or inconclusive tonal analysis.

### Syntax
```csharp
public abstract record TonalAnalysisResult
```

### Properties

| Property | Type | Description |
| --- | --- | --- |
| `Scope` | `PartEventScope` | Gets the immutable ordered scope examined by the analysis. |

### Remarks

- Inspect the runtime type to distinguish `TonalAnalysisResultSet` from `InconclusiveTonalAnalysisResult`.

## TonalAnalysisResultSet

### Summary
Contains an immutable ordered list of ranked tonal candidates.

### Syntax
```csharp
public sealed record class TonalAnalysisResultSet : TonalAnalysisResult
```

### Constructors

- `TonalAnalysisResultSet(IEnumerable<RankedTonalCandidateResult> candidates, PartEventScope scope)` - Copies candidates in their existing rank order.

### Properties

| Property | Type | Description |
| --- | --- | --- |
| `Candidates` | `IReadOnlyList<RankedTonalCandidateResult>` | Gets candidates in rank order. |
| `Count` | `int` | Gets the number of candidates. |

### Remarks

- The constructor rejects a null candidate sequence or null candidate item.
- An empty result set is not the evaluator's inconclusive result type. Use `InconclusiveTonalAnalysisResult` for an inconclusive outcome.

## RankedTonalCandidateResult

### Summary
Represents one ranked key candidate and its evidence.

### Syntax
```csharp
public sealed record RankedTonalCandidateResult : TonalAnalysisResult
```

### Constructors

- `RankedTonalCandidateResult(int rank, PitchClass tonic, Key key, double confidence, IReadOnlyList<EvidenceReason>? supportingEvidence, IReadOnlyList<EvidenceReason>? conflictingEvidence, PartEventScope scope, TonalCandidateStatus status = TonalCandidateStatus.Accepted)` - Creates a candidate with a one-based rank and confidence from 0.0 through 1.0.

### Properties

| Property | Type | Description |
| --- | --- | --- |
| `Rank` | `int` | Gets the one-based rank. Lower values indicate stronger candidates. |
| `Tonic` | `PitchClass` | Gets the spelled candidate tonic. |
| `Key` | `Key` | Gets the candidate key and its governing scale or mode. |
| `Confidence` | `double` | Gets the confidence score from 0.0 through 1.0. |
| `SupportingEvidence` | `IReadOnlyList<EvidenceReason>` | Gets evidence that supports the candidate. |
| `ConflictingEvidence` | `IReadOnlyList<EvidenceReason>` | Gets evidence that conflicts with the candidate. |
| `Status` | `TonalCandidateStatus` | Gets the candidate status. |

### Remarks

- The constructor rejects a rank below one, a confidence outside the inclusive range, a null key or scope, or a tonic that does not match the key tonic.
- Evidence collections are copied during construction.

## InconclusiveTonalAnalysisResult

### Summary
Represents an analysis with no accepted tonic or key.

### Syntax
```csharp
public sealed record InconclusiveTonalAnalysisResult : TonalAnalysisResult
```

### Constructors

- `InconclusiveTonalAnalysisResult(PartEventScope scope, string? reason = null, IReadOnlyList<EvidenceReason>? conflictingEvidence = null)` - Stores the analyzed scope, an optional explanation, and evidence that prevents a conclusive result.

### Properties

| Property | Type | Description |
| --- | --- | --- |
| `Reason` | `string?` | Gets the optional explanation. |
| `ConflictingEvidence` | `IReadOnlyList<EvidenceReason>` | Gets evidence that prevents a conclusive result. |

### Remarks

- Evidence is copied during construction.
- This type is the explicit result for unsupported, empty, or unmatched input.

## TonalCandidateSource

### Summary
Discovers candidate keys from registry classification metadata.

### Syntax
```csharp
public static class TonalCandidateSource
```

### Methods

- `GetDefaultCandidates(RepertoireProfile? profile = null)` - Enumerates keys for registry formulas marked as key candidates. A profile filters formulas by repertoire tags.

### Remarks

- The source expands each eligible formula across supported spelled tonic values.
- Use `TonalEvaluator` to score candidates. This type does not infer tonal centers.

## RepertoireProfile

### Summary
Provides immutable repertoire tags and weights for candidate discovery and scoring.

### Syntax
```csharp
public sealed class RepertoireProfile
```

### Constructors

- `RepertoireProfile(IEnumerable<(ScaleTag Tag, double Weight)> tagWeights)` - Creates a profile with finite, non-negative weights. At least one tag is required.

### Properties

| Property | Type | Description |
| --- | --- | --- |
| `EnabledTags` | `IReadOnlyList<ScaleTag>` | Gets the tags enabled by the profile. |
| `Weights` | `IReadOnlyDictionary<ScaleTag, double>` | Gets the immutable tag weights. |
| `Default` | `RepertoireProfile` | Gets a profile for general tonal, modal, jazz, and blues candidates. |
| `CommonPractice` | `RepertoireProfile` | Gets a profile for common-practice tonal analysis. |
| `Modal` | `RepertoireProfile` | Gets a profile for modal analysis. |

### Methods

- `GetWeight(ScaleTag tag)` - Returns the configured weight, or zero when the tag is not enabled.

### Remarks

- The constructor rejects null input, an empty sequence, and invalid weights.
- The evaluator uses `RepertoireProfile.Default` when no profile is supplied.

## AnalysisTarget

### Summary
Identifies the event or pitch that an analysis record annotates.

### Syntax
```csharp
public sealed record AnalysisTarget
```

## EvidenceReason

### Summary
Stores an evidence category and a specific explanation for an analysis decision.

### Syntax
```csharp
public sealed record EvidenceReason
```

### Remarks

- The explanation must not be null, empty, or white space.

## EvidenceReasonCategory

### Summary
Identifies the category of evidence used by tonal and harmonic analysis.

### Syntax
```csharp
public enum EvidenceReasonCategory
```

## AlteredDegreeKind

### Summary
Identifies the type of alteration applied to a scale degree.

### Syntax
```csharp
public enum AlteredDegreeKind
```

## AlteredDegree

### Summary
Records an altered scale degree and the evidence for its interpretation.

### Syntax
```csharp
public sealed record AlteredDegree
```

## AppliedFunction

### Summary
Records an applied harmonic function, its target degree, label, and supporting evidence.

### Syntax
```csharp
public sealed record AppliedFunction
```

### Remarks

- Supported labels include `V/target` and `vii°/target`.
- The record stores supplied interpretation data. It does not infer an applied function.

## NonChordTone

### Summary
Provides the common target and evidence data for non-chord-tone interpretations.

### Syntax
```csharp
public abstract record NonChordTone
```

## PassingTone

### Summary
Records a passing-tone interpretation with typed targets and evidence.

### Syntax
```csharp
public sealed record PassingTone : NonChordTone
```

## NeighborToneDirection

### Summary
Identifies the upper or lower direction of a neighbor tone.

### Syntax
```csharp
public enum NeighborToneDirection
```

## NeighborTone

### Summary
Records a neighbor-tone interpretation with its target and evidence.

### Syntax
```csharp
public sealed record NeighborTone : NonChordTone
```

## Suspension

### Summary
Records a suspension interpretation with its prepared, suspended, and resolving targets.

### Syntax
```csharp
public sealed record Suspension : NonChordTone
```

## Anticipation

### Summary
Records an anticipation interpretation with its target and evidence.

### Syntax
```csharp
public sealed record Anticipation : NonChordTone
```

## PedalTone

### Summary
Records a pedal-tone interpretation with its sustained pitch target and evidence.

### Syntax
```csharp
public sealed record PedalTone : NonChordTone
```

## MelodicDirection

### Summary
Selects ascending or descending scale-degree resolution.

### Syntax
```csharp
public enum MelodicDirection
```

## CadenceKind

### Summary
Identifies the type of cadential pattern.

### Syntax
```csharp
public enum CadenceKind
```

## CadentialPattern

### Summary
Records a cadential pattern and the evidence for its interpretation.

### Syntax
```csharp
public sealed record CadentialPattern
```

## ModalInterchange

### Summary
Records a borrowed scale or mode interpretation and its evidence.

### Syntax
```csharp
public sealed record ModalInterchange
```

## BluesInflectionKind

### Summary
Identifies a blues-related pitch inflection.

### Syntax
```csharp
public enum BluesInflectionKind
```

## BluesInflection

### Summary
Records a blues inflection and its supporting evidence.

### Syntax
```csharp
public sealed record BluesInflection
```

## RootlessJazzVoicing

### Summary
Records a rootless jazz voicing with an explicit implied root and confidence evidence.

### Syntax
```csharp
public sealed record RootlessJazzVoicing
```

## InterpretationKinds

### Summary
Provides shared interpretation labels for analysis records.

### Syntax
```csharp
public static class InterpretationKinds
```

## TonalCandidateStatus

### Summary
Identifies the status assigned to a ranked tonal candidate.

### Syntax
```csharp
public enum TonalCandidateStatus
```

## TonalEvaluationOptions

### Summary
Controls the number and selection of candidates returned by `TonalEvaluator`.

### Syntax
```csharp
public sealed record TonalEvaluationOptions
```

### Remarks

- The default options return the evaluator's small ranked result set.
- Options can request all scored candidates.

## TonalEvidence

### Summary
Stores one scored observation for a candidate during tonal evaluation.

### Syntax
```csharp
public sealed record TonalEvidence
```

## TonalEvidenceContext

### Summary
Provides the ordered event and candidate context used by an evidence evaluator.

### Syntax
```csharp
public sealed class TonalEvidenceContext
```

## ITonalEvidenceEvaluator

### Summary
Defines a provider that evaluates one type of tonal evidence.

### Syntax
```csharp
public interface ITonalEvidenceEvaluator
```

## TonalEvidenceEvaluator

### Summary
Provides the base contract for a tonal evidence evaluator.

### Syntax
```csharp
public abstract class TonalEvidenceEvaluator : ITonalEvidenceEvaluator
```

## PitchClassEvidenceEvaluator

### Summary
Evaluates pitch-class coverage evidence for a candidate.

### Syntax
```csharp
public sealed class PitchClassEvidenceEvaluator : TonalEvidenceEvaluator
```

## ITonalEvidenceEvaluatorProvider

### Summary
Defines a provider for creating or exposing tonal evidence evaluators.

### Syntax
```csharp
public interface ITonalEvidenceEvaluatorProvider
```

## TonalEvidenceEvaluatorPipeline

### Summary
Runs configured tonal evidence evaluators in priority order.

### Syntax
```csharp
public class TonalEvidenceEvaluatorPipeline
```

## TonalEvidenceEvaluatorPipelineBuilder

### Summary
Builds a tonal evidence evaluator pipeline and permits custom evaluators.

### Syntax
```csharp
public class TonalEvidenceEvaluatorPipelineBuilder
```

## TonalAnalysisResultBuilder

### Summary
Builds one ranked or inconclusive tonal analysis result without performing inference.

### Syntax
```csharp
public sealed class TonalAnalysisResultBuilder
```

## TonalAnalysisResultSetBuilder

### Summary
Builds an immutable ranked set of tonal analysis results.

### Syntax
```csharp
public sealed class TonalAnalysisResultSetBuilder
```
