using System;
using System.Threading.Tasks;

namespace MrMeeseeks.Extensions
{
    public static class ObjectExtensions
    {
        public static bool IsInstanceOfType(this object @this, Type type) => type.IsInstanceOfType(@this);

        public static Task<T> TaskFromResult<T>(this T result) => Task.FromResult(result);
    }
}
