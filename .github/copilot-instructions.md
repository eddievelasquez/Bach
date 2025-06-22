## General
This project uses .NET Core 8.0 and C# 13.0. Best practices should be followed using modern constructs where applicable.

## Unit tests
- Unit tests are written using the `xUnit` library (version 3) and should be placed in the corresponding 'ProjectName.Test' folder. 
- Each test class should be named `ClassNameTest`.
- Test methods have either the `[Fact]` or `[Theory]` attributes.
- Non-parameterized test methods are called facts and are adorned with the `[Fact]` attribute.
- Parameterized test methods are called theories and are adorned with the [Theory] attribute. They also have the `[MemberData]` attribute for test data.
- Theory test data are exposed as public properties of the `TheoryData` type. These properties should be placed just before any theories that use the data instance.
- Test methods should be named `MethodUnderTestName_ShouldExpectedResult_WhenConditionOccurs`; if the method has an expected return value the names should be `MethodUnderTestName_ShouldReturnExpectedValue_WhenConditionOccurs`.
- Assertions should be made using the `FluentAssertions` library.
