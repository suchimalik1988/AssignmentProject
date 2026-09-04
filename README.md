# CSharpProblemSolving

Section 3: C# Problem-Solving Task — determine whether a string contains
only unique characters, case-insensitive.

## Project structure

```
CSharpProblemSolving/
├── CSharpProblemSolving.csproj
├── Solutions/
│   └── UniqueCharacterChecker.cs   # HasOnlyUniqueCharacters implementation
└── Tests/
    └── UniqueCharacterCheckerTests.cs
```

## How to run

```bash
dotnet restore
dotnet test
```

Or in Visual Studio 2022: open the `.csproj`, build, then **Test Explorer → Run All**.

## Approach

`HasOnlyUniqueCharacters` uses a **bit-vector (bitmask)** built from two
`ulong` values — a 128-bit lookup table covering ASCII code points 0-127.
For each character:

1. Case-fold it (`char.ToLowerInvariant`).
2. Compute its bit position and check whether that bit is already set.
3. If set → a duplicate was found → return `false` immediately.
4. Otherwise set the bit and continue.

This satisfies both constraints from the brief:

- **No `HashSet`/`Dictionary`/other collection** — only two fixed-size
  primitive `ulong` fields are used as the "seen" tracker, so extra space
  is O(1) regardless of input length (as opposed to a `bool[128]` array,
  which is still technically a data structure).
- **Optimised for performance** — O(n) time, O(1) space, and a
  **pigeonhole-principle short-circuit**: since ASCII only has 128 distinct
  values, any input longer than 128 characters is mathematically
  guaranteed to contain a duplicate and is rejected in O(1) without
  scanning it at all.

**Scope note:** the checker supports the full extended-ASCII range
(0-127), which covers English letters, digits, punctuation, and
whitespace — everything in the task's examples. A character outside that
range throws `ArgumentOutOfRangeException` rather than silently producing
a wrong answer; extending to full Unicode would need a larger table and
wasn't asked for here.

## Test coverage

| Category | What's covered |
|---|---|
| Given Examples | The exact three cases from the brief (`hello` → false, `world` → true, `Adam` → false) |
| Case Insensitivity | `Aa`, `aA`, `AaBb`, `AbCdEf` |
| Edge Cases | Empty string, single character, `null` input, all-same-character, spaces/punctuation, repeated whitespace |
| Performance | The >128-character short-circuit, and a 102-character string proving long-but-under-threshold input is still evaluated correctly (not just short-circuited) |
