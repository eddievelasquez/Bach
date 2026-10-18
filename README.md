# Bach

[![Build and test](https://github.com/eddievelasquez/bach/actions/workflows/BuildAndTest.yml/badge.svg)](https://github.com/eddievelasquez/bach/actions/workflows/BuildAndTest.yml)
[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)](https://opensource.org/licenses/MIT)

Bach is a .NET 10+ library for expressing Western tonal music concepts in C#.

The repository contains:

- **Bach.Model** - the public music-theory library.
- **Bach.Cli** - a command-line demonstration of the model.
- **Bach.Model.Test** - the xUnit v3 test suite.
- **docs** - DocFX inputs, API reference material, and codebase documentation.

Bach is a personal project for learning music theory and keeping C# and .NET skills current while working with C++.

## Requirements

- .NET SDK 10 or later.
- Windows, Linux, or macOS.

## Repository layout

| Path | Purpose |
| --- | --- |
| `src/Bach.Model` | Public domain model and embedded registry data |
| `src/Bach.Cli` | Demonstration command-line application |
| `src/Bach.Model.Test` | Unit, contract, parsing, serialization, and tonal-analysis tests |
| `docs` | Documentation and DocFX configuration |
| `.github/workflows` | Build, test, coverage, and documentation workflows |

## Build and test

```powershell
dotnet restore Bach.slnx
dotnet build Bach.slnx --configuration Release
dotnet test Bach.slnx --configuration Release
```

The test suite uses xUnit v3, FluentAssertions, and Microsoft Testing Platform. CI collects Cobertura coverage, generates an HTML and GitHub job-summary report, and requires at least 85% total line coverage.

## Validate CI locally

Run the workflow's .NET commands directly for fast local feedback:

```powershell
dotnet restore Bach.slnx
dotnet build Bach.slnx --configuration Release --no-restore
dotnet test Bach.slnx `
  --configuration Release `
  --no-build `
  --coverage `
  --coverage-output "$PWD\.artifacts\coverage\coverage.xml" `
  --coverage-output-format cobertura
```

Check the workflow syntax with [`actionlint`](https://github.com/rhysd/actionlint):

```powershell
actionlint .github\workflows\BuildAndTest.yml
```

For an end-to-end local simulation, install Docker Desktop and [`act`](https://github.com/nektos/act), then run:

```powershell
act pull_request `
  -W .github\workflows\BuildAndTest.yml `
  -j build `
  -P ubuntu-latest=catthehacker/ubuntu:act-latest
```

Direct commands validate the build, tests, coverage, and threshold logic. `actionlint` validates workflow syntax and expressions. `act` is the closest local simulation of GitHub Actions, but it does not exactly reproduce GitHub-hosted runners, permissions, tokens, or artifact storage. A `workflow_dispatch` run or pull request remains the final validation.

## Run the CLI demonstration

```powershell
dotnet run --project src/Bach.Cli/Bach.Cli.csproj -- --help
```

The CLI currently demonstrates model features such as listing registry content, displaying scales and chords, finding intervals, and finding scales or modes that contain pitch classes. A broader CLI effort is planned for the future.

## Architecture

`Bach.Model` is organized by music domain:

- `Pitches` and `Intervals` provide fundamental representations and operations.
- `Scales` and `Harmony` model governing scales or modes, formulas, keys, chords, and progressions.
- `Structure` provides immutable parts, measures, events, and analysis scopes.
- `Analysis` evaluates tonal candidates through composable evidence providers.
- `Library` exposes the bundled registry of formulas and instrument definitions.
- `Serialization` provides JSON converters for model events.

The registry data is embedded in `Bach.Model.dll`. It is immutable at runtime and does not require a separate JSON file beside the assembly.

## Documentation

- [Documentation site configuration](docs/docfx.json)
- [API reference](docs/REFERENCE.md)
- [Namespace mapping](docs/namespace-mapping.md)
- [Codebase architecture notes](docs/codebase/ARCHITECTURE.md)
- [Codebase technology stack](docs/codebase/STACK.md)

To build the documentation locally, use DocFX with `docs/docfx.json`.

## Package

The `Bach.Model` project creates a NuGet package during build or pack:

```powershell
dotnet pack src/Bach.Model/Bach.Model.csproj --configuration Release
```

The package targets .NET 10 and includes the model assembly, XML documentation, and embedded registry data.

## Contributing

Contributions are welcome. Before opening a pull request:

1. Keep changes focused and preserve the public API unless the change requires an API update.
2. Add or update tests for observable behavior changes.
3. Run `dotnet build Bach.slnx --configuration Release`.
4. Run `dotnet test Bach.slnx --configuration Release`.
5. Update directly related documentation.

Open an issue for music-theory errors, bugs, or unexpected behavior. Use the repository instructions under `.github/instructions` for C#, tests, and README API references.

## License

Bach is released under the [MIT License](LICENSE).
