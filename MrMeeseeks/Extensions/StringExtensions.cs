using System.Runtime.CompilerServices;

namespace MrMeeseeks.Extensions
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string? @this) => string.IsNullOrEmpty(@this);
        public static bool IsNullOrWhitespace(this string? @this) => string.IsNullOrWhiteSpace(@this);
        public static bool IsEmpty(this string @this) => string.IsNullOrEmpty(@this);
        public static bool IsWhitespace(this string @this) => string.IsNullOrWhiteSpace(@this);
    }
}