using System;
using System.Collections.Generic;

namespace MrMeeseeks.LambdaPattern
{
    public class LambdaComparer<T> : IComparer<T> where T : class
    {
        private readonly Func<T, T, int> comparingLogic;
        private readonly bool descending;
        private readonly bool skipDefaultNullComparison;

        public LambdaComparer(
            Func<T, T, int> comparingLogic,
            bool descending = false,
            bool skipDefaultNullComparison = false)
        {
            this.comparingLogic = comparingLogic;
            this.descending = descending;
            this.skipDefaultNullComparison = skipDefaultNullComparison;
        }

        public int Compare(T x, T y)
        {
            if(!skipDefaultNullComparison)
            {
                if (x is null && y is null)
                    return 0;
                if (x is null)
                    return descending ? 1 : -1;
                if (y is null)
                    return descending ? -1 : 1;
            }
            return descending 
                ? -1 * comparingLogic(x, y)
                : comparingLogic(x, y);
        }
    }
}
