---
applyTo: "src/Bach.Model.Test/**/*.cs"
---

# Test rules

- Use xUnit v3 and FluentAssertions.
- Put each test in the folder that matches the production namespace relative to `src/Bach.Model.Test`, and use the production namespace with `.Test` appended. For example, tests for `Bach.Model.Pitch` use `Bach.Model.Test` in the project root, and tests for `Bach.Model.Analysis.AlteredDegree` use `Bach.Model.Analysis.Test` in the `Analysis` folder.
- Name test classes {ClassUnderTest}Tests.
- Name test methods {MethodUnderTest}_ShouldReturn{ExpectedValue}_When{ConditionOccurs} for returned values. Otherwise use {MethodUnderTest}_Should{ExpectedResult}_When{ConditionOccurs}.
- Use [Fact] for one case. Use [Theory] for parameterized cases. [InlineData] is acceptable for ten or fewer cases that use only primitive values, such as int or string. Use public TheoryData properties for larger or complex parameter sets, and put each property before its theory.
- Test observable behavior and edge cases. Do not test private implementation details or change production visibility only for tests.
- Keep tests independent and deterministic. Do not use shared mutable state, timing, network access, or machine-specific paths.
- When a change affects music-theory semantics, add at least one regression test for the affected behavior.
