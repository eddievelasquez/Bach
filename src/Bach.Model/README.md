# Bach.Model API Documentation

A .NET 8 library for representing and manipulating Western tonal music theory concepts, including pitches, intervals, scales, chords, and stringed instrument tunings.

---

## Table of Contents

- [Registry](#registry)
- [Scale](#scale)
- [Chord](#chord)
- [PitchClass](#pitchclass)
- [PitchClassCollection](#pitchclasscollection)
- [ScaleFormula](#scaleforumla)
- [ChordFormula](#chordformula)
- [StringedInstrumentDefinition](#stringedinstrumentdefinition)
- [TuningCollection & Tuning](#tuningcollection--tuning)

---

## Registry

> Provides access to all predefined formulas and definitions (scales, chords and stringed instruments) loaded 
> from the `Bach.Model.Library.json` file.

### Public Members

| Member | Type | Description |
|--------|------|-------------|
| `ScaleFormulas` | `NamedObjectCollection<ScaleFormula>` | All available scale formulas. |
| `ChordFormulas` | `NamedObjectCollection<ChordFormula>` | All available chord formulas. |
| `StringedInstrumentDefinitions` | `NamedObjectCollection<StringedInstrumentDefinition>` | All available stringed instrument definitions. |

### Example

```csharp
// List all scale formulas
foreach( var formula in Registry.ScaleFormulas )
{ 
  Console.WriteLine( formula.Name );
}
```

---

## Scale

> Represents a musical scale, constructed from a root pitch and a scale formula.

### Public Members

| Member | Type | Description |
|--------|------|-------------|
| `Scale(PitchClass root, ScaleFormula formula)` | Constructor | Create a scale from a root and formula. |
| `Scale(PitchClass root, string formulaIdOrName)` | Constructor | Create a scale from a root and formula name/id. |
| `Root` | `PitchClass` | The root pitch class of the scale. |
| `Name` | `string` | The localized name of the scale. |
| `Formula` | `ScaleFormula` | The formula for the scale. |
| `PitchClasses` | `PitchClassCollection` | The pitch classes that form this scale. |
| `Theoretical` | `bool` | Whether this scale is theoretical (contains double flats/sharps). |
| `Contains(IEnumerable<PitchClass> notes)` | `bool` | Returns true if all notes are in this scale. |
| `GetAscending()` | `IEnumerable<PitchClass>` | Enumerates the scale in ascending order. |
| `GetDescending()` | `IEnumerable<PitchClass>` | Enumerates the scale in descending order. |
| `GetEnharmonicScale()` | `Scale` | Gets an enharmonic equivalent scale. |
| `Render(int octave)` | `IEnumerable<Pitch>` | Renders the scale as pitches starting at the given octave. |
| `ToString([string format, IFormatProvider provider])` | `string` | String representation with custom formatting. |
| `static ScalesContaining(IEnumerable<PitchClass> notes)` | `IEnumerable<Scale>` | Finds all scales containing the given notes. |

### Example

```csharp
// Create a C Major scale using the predefined formula
var scale = new Scale( PitchClass.C, Registry.ScaleFormulas["Major"] ); 
Console.WriteLine(scale.Name); 

foreach( var note in scale.PitchClasses )
{
  Console.WriteLine(note);
}

// Find scales containing specific notes
var matches = Scale.ScalesContaining( PitchClassCollection.Parse("C,D,E") );
foreach( var s in matches ) 
{
  Console.WriteLine(s.Name);
}
```

---

## Chord

> Represents a musical chord, defined by a root pitch and a chord formula.

### Public Members

| Member | Type | Description |
|--------|------|-------------|
| `Chord(PitchClass root, ChordFormula formula)` | Constructor | Create a chord from a root and formula. |
| `Chord(PitchClass root, string formulaIdOrName)` | Constructor | Create a chord from a root and formula name/id. |
| `Root` | `PitchClass` | The root pitch class for the chord. |
| `Bass` | `PitchClass` | The bass pitch class (first note, may differ from root in inversions). |
| `Inversion` | `int` | The inversion number. |
| `Name` | `string` | The chord's name. |
| `Formula` | `ChordFormula` | The chord's formula. |
| `PitchClasses` | `PitchClassCollection` | The pitch classes that compose the chord. |
| `IsExtended` | `bool` | True if the chord is an extended chord (intervals beyond the octave). |
| `GetInversion(int inversion)` | `Chord` | Generates an inversion of the chord. |
| `Render(int octave)` | `IEnumerable<Pitch>` | Renders the chord as pitches starting at the given octave. |

### Example

```csharp
// Create a C Major chord using the predefined formula
var chord = new Chord( PitchClass.C, Registry.ChordFormulas["Major"] );
Console.WriteLine(chord.Name);

foreach(var note in chord.PitchClasses) 
{
  Console.WriteLine(note);
}

// Display the chord's bass note for the first inversion 
var inv = chord.GetInversion( 1 ); 
Console.WriteLine( inv.Bass );
```

---

## PitchClass

> Represents a pitch class (e.g., C, C#, D, etc.) in the chromatic scale.

### Public Members

| Member | Type | Description |
|--------|------|-------------|
| `C`, `CSharp`, `DFlat`, ... | `PitchClass` | Static properties for all pitch classes. |
| `NoteName` | `NoteName` | The note name (A-G). |
| `Accidental` | `Accidental` | The accidental (natural, sharp, flat, etc.). |
| `Add(int semitones)` | `PitchClass` | Add semitones to this pitch class. |
| `Add(Interval interval)` | `PitchClass` | Add an interval to this pitch class. |
| `Subtract(int semitones)` | `PitchClass` | Subtract semitones. |
| `Subtract(Interval interval)` | `PitchClass` | Subtract an interval. |
| `Subtract(PitchClass other)` | `Interval` | Get the interval between two pitch classes. |
| `GetEnharmonic(NoteName)` | `PitchClass?` | Get enharmonic equivalent. |
| `Parse(string)` | `PitchClass` | Parse from string. |
| `TryParse(string, out PitchClass)` | `bool` | Try to parse from string. |
| Operators: `+`, `-`, `==`, `!=`, `<`, `>`, `<=`, `>=` | | Arithmetic and comparison. |

### Example

```csharp
// Find the note that is three semitones above C (D#)
var c = PitchClass.C; 
var dSharp = c + 3;

Console.WriteLine( dSharp ); // "D#" 

// Determine the interval between two pitch classes
var interval = PitchClass.E - PitchClass.C; // Major third
```

---

## PitchClassCollection

> Represents a collection of pitch classes.

### Public Members

| Member | Type | Description |
|--------|------|-------------|
| `PitchClassCollection(IEnumerable<PitchClass>)` | Constructor | Create from an enumerable. |
| `PitchClassCollection(PitchClass[])` | Constructor | Create from an array. |
| `Count` | `int` | Number of pitch classes. |
| `this[int index]` | `PitchClass` | Indexer. |
| `IndexOf(PitchClass)` | `int` | Index of a pitch class. |
| `Parse(string)` | `PitchClassCollection` | Parse from string. |
| `TryParse(string, out PitchClassCollection)` | `bool` | Try to parse from string. |
| `ToString()` | `string` | String representation. |
| Implements `IReadOnlyList<PitchClass>`, `IEquatable<IEnumerable<PitchClass>>` | | |

### Example

```csharp
// Create and display a collection of pitch classes
var collection = PitchClassCollection.Parse( "C,E,G" );
foreach( var pc in collection )
{
  Console.WriteLine( pc );
}
```

---

## ScaleFormula

> Describes the interval structure and metadata for a scale type.

### Public Members

| Member | Type | Description |
|--------|------|-------------|
| `Categories` | `IReadOnlySet<string>` | Categories for this scale formula. |
| `Aliases` | `IReadOnlySet<string>` | Aliases for this scale formula. |
| `IsDiatonic` | `bool` | True if diatonic. |
| `IsMajor` | `bool` | True if major. |
| `IsMinor` | `bool` | True if minor. |

### Example

```csharp
// Get the Major scale formula and check its properties
var formula = Registry.ScaleFormulas["Major"];
Console.WriteLine(formula.IsDiatonic); // True
Console.WriteLine(formula.IsMajor); // True
Console.WriteLine(formula.IsMinor); // False
```

---

## ChordFormula

> Describes the interval structure and metadata for a chord type.

### Public Members

| Member | Type | Description |
|--------|------|-------------|
| `Symbol` | `string` | The chord symbol (e.g., "m7", "dim"). |

### Example

```csharp
// Get the Minor chord formula and display its symbol
var formula = Registry.ChordFormulas["Minor"]; 
Console.WriteLine( formula.Symbol ); // "m"
```

---

## StringedInstrumentDefinition

> Describes a stringed instrument (e.g., guitar, bass) and its tunings.

### Public Members

| Member | Type | Description |
|--------|------|-------------|
| `StringCount` | `int` | Number of strings. |
| `Tunings` | `TuningCollection` | Available tunings. |

### Example

```csharp
// Get the Guitar definition and display its string count and registered tunings
var guitar = Registry.StringedInstrumentDefinitions["Guitar"]; 
Console.WriteLine(guitar.StringCount); // 6 

foreach( var tuning in guitar.Tunings.Values )
{
  Console.WriteLine(tuning.Name);
}
```


---

## TuningCollection & Tuning

> `TuningCollection` is a read-only dictionary of `Tuning` objects for a stringed instrument.

### TuningCollection Public Members

| Member | Type | Description |
|--------|------|-------------|
| `Standard` | `Tuning` | The standard tuning. |
| `Count` | `int` | Number of tunings. |
| `this[string id]` | `Tuning` | Get tuning by id. |
| `Keys` | `IEnumerable<string>` | All tuning ids. |
| `Values` | `IEnumerable<Tuning>` | All tunings. |
| `ContainsKey(string)` | `bool` | True if tuning exists. |
| `TryGetValue(string, out Tuning)` | `bool` | Try to get tuning. |

### Tuning Public Members

| Member | Type | Description |
|--------|------|-------------|
| `InstrumentDefinition` | `StringedInstrumentDefinition` | The instrument definition. |
| `Id` | `string` | Tuning id. |
| `Name` | `string` | Tuning name. |
| `Pitches` | `PitchCollection` | Pitches for each string. |
| `this[int stringNumber]` | `Pitch` | Get pitch for a string. |

### Example

```csharp
// Get the standard tuning for Guitar and display the pitches
var guitar = Registry.StringedInstrumentDefinitions["Guitar"]; 
var standard = guitar.Tunings.Standard; 
Console.WriteLine( standard.Name ); // "Standard" 

for( int i = 1; i <= guitar.StringCount; i++ )
{
  Console.WriteLine( standard[i] );
}

```


---

## License

MIT License. See [LICENSE](https://opensource.org/licenses/MIT) for details.

---

For more details, see the main project [README](../../README.md).
