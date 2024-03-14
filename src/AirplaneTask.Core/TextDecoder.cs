using System.Text;

namespace AirplaneTask.Core
{
    public class TextDecoder
    {
        private const char TokenStart = '[';
        private const char TokenEnd = ']';

        /// <exception cref="ArgumentNullException"></exception>
        public string Decode(string sourceText)
        {
            if (sourceText == null)
                throw new ArgumentNullException(nameof(sourceText));

            StringBuilder builder = DecodeInternal(sourceText, idxStart: 0, isTokenStartExist: false, out int _, out bool _);
            return builder.ToString();
        }

        private StringBuilder DecodeInternal(string sourceText, int idxStart, bool isTokenStartExist,
            out int nextIdx, out bool isTokenEndExist)
        {
            var builder = new StringBuilder();
            int nextIdxInternal = idxStart;

            isTokenEndExist = false;

            for (int idx = idxStart; idx < sourceText.Length; idx++)
            {
                int idxNext = idx + 1;
                char currentChar = sourceText[idx];
                char? nextChar = idxNext < sourceText.Length ? sourceText[idxNext] : null;

                bool isMultiplierExist = IsMultiplier(currentChar, out int multiplier);
                bool isTokenStartWithMultiplier = isMultiplierExist && nextChar.HasValue && IsTokenStart(nextChar.Value);

                if (IsTokenStart(currentChar) || isTokenStartWithMultiplier)
                {
                    int startIdxInner = isMultiplierExist ? idx + 2 : idx + 1;

                    StringBuilder innerBuilder = DecodeInternal(sourceText, startIdxInner, isTokenStartExist: true,
                        out nextIdxInternal, out bool isTokenEndExistInternal);

                    if (isMultiplierExist && !isTokenEndExistInternal)
                    {
                        builder.Append(multiplier);
                    }
                    if (!isMultiplierExist || !isTokenEndExistInternal)
                    {
                        multiplier = 1;
                    }

                    for (int i = 0; i < multiplier; i++)
                    {
                        builder.Append(innerBuilder);
                    }

                    idx = nextIdxInternal;
                    continue;
                }

                nextIdxInternal = idx;
                if (IsTokenEnd(currentChar) && isTokenStartExist)
                {
                    isTokenEndExist = true;
                    break;
                }
                builder.Append(currentChar);
            }

            if (!isTokenEndExist && isTokenStartExist)
            {
                builder.Insert(0, TokenStart);
            }

            nextIdx = nextIdxInternal;
            return builder;
        }

        private static bool IsMultiplier(char c, out int multiplier)
        {
            multiplier = 1;
            bool isDigit = char.IsDigit(c);
            if (isDigit)
            {
                multiplier = c - '0';
            }
            return isDigit;
        }

        private static bool IsTokenStart(char c) => c == TokenStart;

        private static bool IsTokenEnd(char c) => c == TokenEnd;
    }
}