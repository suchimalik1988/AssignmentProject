namespace CSharpProblemSolving.Solutions
{
    /// <summary>
    /// Section 3: C# Problem-Solving Task.
    ///
    /// Determines whether a string contains only unique characters
    /// (case-insensitive), without using any auxiliary data structure
    /// such as HashSet or Dictionary.
    ///
    /// Approach: bit-vector (bitmask) using two <see cref="ulong"/> values
    /// as a 128-bit lookup table covering the full extended-ASCII range
    /// (character codes 0-127, which covers all English letters, digits,
    /// punctuation and whitespace). Each bit answers "have I seen this
    /// character before?" — checking and setting a bit is O(1), so the
    /// whole scan is O(n) time and O(1) *extra* space: exactly two fixed
    /// 64-bit integers, regardless of input length. This satisfies both
    /// constraints — no collection types, and optimal time/space
    /// complexity — and is the standard interview-grade solution to this
    /// exact problem.
    ///
    /// A pigeonhole-principle short-circuit is also applied: the ASCII
    /// character set only has 128 distinct values, so any string longer
    /// than 128 characters is guaranteed to contain a duplicate and can
    /// be rejected in O(1) without scanning it at all.
    /// </summary>
    public static class UniqueCharacterChecker
    {
        /// <summary>
        /// Number of distinct values a single extended-ASCII character can hold.
        /// </summary>
        private const int AsciiRange = 128;

        /// <summary>
        /// Returns true if <paramref name="input"/> contains no repeated
        /// characters when compared case-insensitively; otherwise false.
        /// An empty string is considered to have all-unique characters
        /// (vacuously true) — there are no duplicates to find.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="input"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if <paramref name="input"/> contains a character outside
        /// the extended-ASCII range (code point 0-127). This solution is
        /// scoped to ASCII, matching the task's examples; supporting full
        /// Unicode would require a larger lookup table.
        /// </exception>
        public static bool HasOnlyUniqueCharacters(string input)
        {
            ArgumentNullException.ThrowIfNull(input);

            if (input.Length == 0)
            {
                return true;
            }

            // Pigeonhole principle: more characters than the alphabet size
            // guarantees a repeat, so bail out without scanning.
            if (input.Length > AsciiRange)
            {
                return false;
            }

            ulong lowMask = 0;  // tracks characters with code points 0-63
            ulong highMask = 0; // tracks characters with code points 64-127

            foreach (char c in input)
            {
                int code = char.ToLowerInvariant(c);

                if (code >= AsciiRange)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(input),
                        $"Character '{c}' (code {code}) is outside the supported ASCII range (0-127).");
                }

                bool isHighRange = code >= 64;
                int bitPosition = isHighRange ? code - 64 : code;
                ulong bit = 1UL << bitPosition;

                if (isHighRange)
                {
                    if ((highMask & bit) != 0)
                    {
                        return false; // already seen this character
                    }
                    highMask |= bit;
                }
                else
                {
                    if ((lowMask & bit) != 0)
                    {
                        return false; // already seen this character
                    }
                    lowMask |= bit;
                }
            }

            return true;
        }
    }
}
