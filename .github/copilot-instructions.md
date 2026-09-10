# Bach Copilot Instructions

Bach is a .NET 10 library for Western tonal music theory. The solution contains the Bach.Model package, the Bach.Cli console app, and Bach.Model.Test tests.

## Project shape

- The public domain model lives in src/Bach.Model.
- The console app lives in src/Bach.Cli and should stay focused on demonstrating the model.
- The test project lives in src/Bach.Model.Test and uses xUnit v3 and FluentAssertions.
- Shared SDK and language settings are in Directory.Build.props. Central package versions are in Directory.Packages.props.

## General rules

- Read the related code and tests before you change code.
- Keep changes small and focused.
- Do not change public APIs, target frameworks, package versions, or project files unless the task needs it.
- Do not add dependencies without approval. Prefer packages already listed in Directory.Packages.props.
- Do not edit generated files or build output.
- Preserve music-theory terms and behavior. Use the phrase "governing scale or mode" instead of the unqualified term "collection" in comments and documentation when referring to the key's scale or mode.
- Add or update tests for observable behavior changes.
- Use existing validation and error-handling patterns. Do not hide errors with broad catches or silent fallback values.
- Keep directional scale data on `ScaleFormula.AscendingDegrees` and `ScaleFormula.DescendingDegrees`. Use the existing formula degree lists for scale direction; do not add separate ascending and descending formula references to `Scale`.

## Build and test commands

- Restore, build, and test with "dotnet test Bach.slnx".
- Use "dotnet build Bach.slnx" when tests are not applicable.
- Prefer the smallest project or test scope that gives useful feedback first.

## C# conventions

- Target .NET 10 and follow the project settings.
- Keep nullable reference types enabled.
- Use file-scoped namespaces and PascalCase type names.
- Prefer the least visible access level that fits the code.
- Add XML documentation for new or changed public APIs. The <summary> tag should describe the behavior, not the implementation; the text should be on a separate line from the summary tags.

## Documentation and instructions

- Write prose, documentation, comments, and commit messages in ASD-STE100 Simplified Technical English.
- Scoped rules for C#, tests, and README API references live in .github/instructions/.
- Use the bach-test-engineer agent when focused test design or test changes are needed.

## Music-theory references

- Use Kostka, Payne, and Almén, *Tonal Harmony*, as a reference for tonal function,
  harmonic progression, applied functions, mixture, and cadential practice.
- Use Aldwell and Schachter, *Harmony and Voice Leading*, as a reference for harmonic
  analysis, voice leading, non-chord tones, and chromatic harmony.
- Use Laitz, *The Complete Musician*, as a reference for scale spelling, diatonic
  structure, modal practice, and notational analysis.
- Use Temperley, *The Cognition of Basic Musical Structures*, as a reference for
  computational and cognitive assumptions about tonal inference.
- Use Krumhansl, *Cognitive Foundations of Musical Pitch*, as a reference for pitch
  hierarchy and tonal-center evidence.
- Use these works as conceptual references and sources for test-case design. Cite
  bibliographic facts only. Do not copy source text.
