using System.IO;
using System.Text.RegularExpressions;

namespace MrMeeseeks.Extensions
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string? @this) => string.IsNullOrEmpty(@this);
        public static bool IsNullOrWhitespace(this string? @this) => string.IsNullOrWhiteSpace(@this);
        public static bool IsEmpty(this string @this) => string.IsNullOrEmpty(@this);
        public static bool IsWhitespace(this string @this) => string.IsNullOrWhiteSpace(@this);
    }
    
    public static class StringRemoveIllegalFilePathCharactersExtension
    {
        static StringRemoveIllegalFilePathCharactersExtension()
        {
            MatchAllIllegalFilePathCharacters = new Regex(
                $"[{Regex.Escape(new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars()))}]");
        }

        private static readonly Regex MatchAllIllegalFilePathCharacters;

        public static string RemoveIllegalFilePathCharacters(this string @this)
        {
            return MatchAllIllegalFilePathCharacters.Replace(@this, "");
        }
    }
}