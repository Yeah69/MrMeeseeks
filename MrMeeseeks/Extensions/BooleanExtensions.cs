namespace MrMeeseeks.Extensions
{
    public static class BooleanExtensions
    {
        public static bool Not(this bool b) => !b;
        public static bool And(this bool a, bool b) => a && b;
        public static bool Or(this bool a, bool b) => a || b;
        public static bool Xor(this bool a, bool b) => a && !b || b && !a;
    }
}
