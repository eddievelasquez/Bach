---
applyTo: "README.md"
---

# Bach.Model API reference

Use these rules only when you create or update the README Reference section. Document public Bach.Model APIs. Do not document internal APIs or generated documentation output.

## Reference structure

- Add one ### TypeName section for each documented type.
- Keep the following subsections in this order when they apply: Summary, Syntax, Constructors, Fields, Properties, Methods, Usage Example, and Remarks.
- Do not add an empty subsection. Use #### headings for subsections.
- Use a csharp fenced code block for declarations and examples.
- Use a Markdown table with Property, Type, and Description columns for properties.
- Use Markdown list items for constructors, fields, methods, and remarks.
- Show signatures in backticks. State behavior, defaults, validation, and thrown exceptions when they are relevant.
- Make each code example complete enough to show normal use of the API.
- Use ASD-STE100 Simplified Technical English.

## Template

````markdown
## Reference

### TypeName

#### Summary
Briefly describe the type, its purpose, and its thread-safety or immutability when relevant.

#### Syntax
```csharp
public sealed class TypeName
```

#### Constructors

- TypeName() - Initializes a new instance with default settings.

#### Fields

- TypeName Default - Describes the field purpose.

#### Properties

| Property | Type | Description |
| --- | --- | --- |
| Name | string | Describes the property behavior. |

#### Methods

- MethodName() - Describes the method behavior.

#### Usage Example

```csharp
var value = new TypeName();
```

#### Remarks

- State valid values, constraints, edge cases, and performance effects when they are relevant.

````
