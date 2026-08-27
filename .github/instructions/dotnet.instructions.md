---
applyTo: "**/*.cs"
---

# C# rules

- Target .NET 10 and use the language version from the project settings.
- Keep nullable reference types enabled. Express contracts with types and guard clauses. Do not use the null-forgiving operator unless there is a valid reason.
- Use file-scoped namespaces and PascalCase type names. Match the surrounding code for member order and formatting.
- Default to the least visible access level. Use `internal` for code that is not public API.
- Prefer simple, direct code. Add abstractions, async code, pooling, spans, or `ValueTask` only when they solve a real need.
- Use precise exception types. Do not catch `Exception` unless the code can add useful context and rethrow.
- Add XML documentation for new or changed public APIs. Comments explain reasons, not code syntax.
