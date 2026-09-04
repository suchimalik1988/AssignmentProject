using CSharpProblemSolving.Solutions;
using Xunit;

namespace CSharpProblemSolving.Tests
{
    /// <summary>
    /// Test suite for <see cref="UniqueCharacterChecker.HasOnlyUniqueCharacters"/>.
    /// Covers the exact examples from the task brief plus the edge cases a
    /// reviewer would expect: empty input, casing, null, and the boundary
    /// around the pigeonhole-principle short-circuit.
    /// </summary>
    public class UniqueCharacterCheckerTests
    {
        // ---- Examples given directly in the task ----

        [Theory]
        [Trait("Category", "Given Examples")]
        [InlineData("hello", false)]
        [InlineData("world", true)]
        [InlineData("Adam", false)] // 'a'/'A' and 'a' collide once case-insensitive
        public void HasOnlyUniqueCharacters_MatchesTaskExamples(string input, bool expected)
        {
            Assert.Equal(expected, UniqueCharacterChecker.HasOnlyUniqueCharacters(input));
        }

        // ---- Case-insensitivity ----

        [Theory]
        [Trait("Category", "Case Insensitivity")]
        [InlineData("Aa", false)]     // same letter, different case
        [InlineData("aA", false)]
        [InlineData("AaBb", false)]   // multiple case-duplicated pairs
        [InlineData("AbCdEf", true)]  // all distinct even with mixed case
        public void HasOnlyUniqueCharacters_IsCaseInsensitive(string input, bool expected)
        {
            Assert.Equal(expected, UniqueCharacterChecker.HasOnlyUniqueCharacters(input));
        }

        // ---- Boundary / edge cases ----

        [Fact]
        [Trait("Category", "Edge Cases")]
        public void HasOnlyUniqueCharacters_EmptyString_ReturnsTrue()
        {
            Assert.True(UniqueCharacterChecker.HasOnlyUniqueCharacters(string.Empty));
        }

        [Fact]
        [Trait("Category", "Edge Cases")]
        public void HasOnlyUniqueCharacters_SingleCharacter_ReturnsTrue()
        {
            Assert.True(UniqueCharacterChecker.HasOnlyUniqueCharacters("x"));
        }

        [Fact]
        [Trait("Category", "Edge Cases")]
        public void HasOnlyUniqueCharacters_NullInput_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                UniqueCharacterChecker.HasOnlyUniqueCharacters(null!));
        }

        [Fact]
        [Trait("Category", "Edge Cases")]
        public void HasOnlyUniqueCharacters_RepeatedSameCharacter_ReturnsFalse()
        {
            Assert.False(UniqueCharacterChecker.HasOnlyUniqueCharacters("aaaa"));
        }

        [Fact]
        [Trait("Category", "Edge Cases")]
        public void HasOnlyUniqueCharacters_WithSpacesAndPunctuation_AllUnique_ReturnsTrue()
        {
            // Space, comma and letters are all distinct ASCII code points here.
            Assert.True(UniqueCharacterChecker.HasOnlyUniqueCharacters("ab, cd"));
        }

        [Fact]
        [Trait("Category", "Edge Cases")]
        public void HasOnlyUniqueCharacters_RepeatedSpace_ReturnsFalse()
        {
            Assert.False(UniqueCharacterChecker.HasOnlyUniqueCharacters("a b c"));
        }

        // ---- Pigeonhole-principle short-circuit ----

        [Fact]
        [Trait("Category", "Performance")]
        public void HasOnlyUniqueCharacters_LongerThan128Chars_ShortCircuitsToFalse()
        {
            // 129 characters cannot all be unique within a 128-value alphabet,
            // regardless of content — this exercises the O(1) fast-reject path.
            string input = new string('x', 129);
            Assert.False(UniqueCharacterChecker.HasOnlyUniqueCharacters(input));
        }

        [Fact]
        [Trait("Category", "Performance")]
        public void HasOnlyUniqueCharacters_LongStringAllDistinctAfterCaseFolding_ReturnsTrue()
        {
            // Build a string from every ASCII code point 0-127, excluding
            // uppercase 'A'-'Z' (65-90) — those would case-fold onto the
            // lowercase codes already in the set and create a false
            // collision. What remains (102 characters) is genuinely unique
            // post-normalization, proving long-but-under-the-threshold
            // inputs are still evaluated correctly rather than just relying
            // on the >128 short-circuit.
            var chars = new List<char>();
            for (int i = 0; i < 128; i++)
            {
                bool isUppercaseLetter = i is >= 'A' and <= 'Z';
                if (!isUppercaseLetter)
                {
                    chars.Add((char)i);
                }
            }
            string input = new string(chars.ToArray());

            Assert.Equal(128 - 26, input.Length);
            Assert.True(UniqueCharacterChecker.HasOnlyUniqueCharacters(input));
        }
    }
}
