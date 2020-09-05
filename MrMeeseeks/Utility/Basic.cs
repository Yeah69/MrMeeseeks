namespace MrMeeseeks.Utility
{
    public static class Basic
    {
        public static T Identity<T>(T element) => element;

        public static bool True<T>(T _) => true;

        public static bool False<T>(T _) => false;

        public static bool True<T1, T2>(T1 _, T2 __) => true;

        public static bool False<T1, T2>(T1 _, T2 __) => false;

        public static int Zero<T>(T _) => 0;
    }
}
