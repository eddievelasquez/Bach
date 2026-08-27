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
- Preserve music-theory terms and behavior.
- Add or update tests for observable behavior changes.
- Use existing validation and error-handling patterns. Do not hide errors with broad catches or silent fallback values.

## Build and test commands

- Restore, build, and test with "dotnet test Bach.slnx".
- Use "dotnet build Bach.slnx" when tests are not applicable.
- Prefer the smallest project or test scope that gives useful feedback first.

## C# conventions

- Target .NET 10 and follow the project settings.
- Keep nullable reference types enabled.
- Use file-scoped namespaces and PascalCase type names.
- Prefer the least visible access level that fits the code.
- Add XML documentation for new or changed public APIs.

## Documentation and instructions

- Write prose, documentation, comments, and commit messages in ASD-STE100 Simplified Technical English.
- Scoped rules for C#, tests, and README API references live in .github/instructions/.
- Use the bach-test-engineer agent when focused test design or test changes are needed.
