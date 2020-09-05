using System;
using System.Collections.Generic;

namespace MrMeeseeks.LambdaPattern
{
    public class LambdaStructComparer<T> : IComparer<T> where T : struct
    {
        private readonly Func<T, T, int> comparingLogic;
        private readonly bool descending;

        public LambdaStructComparer(
            Func<T, T, int> comparingLogic,
            bool descending = false)
        {
            this.comparingLogic = comparingLogic;
            this.descending = descending;
        }

        public int Compare(T x, T y)
        {
            return descending
                ? -1 * comparingLogic(x, y)
                : comparingLogic(x, y);
        }
    }
}