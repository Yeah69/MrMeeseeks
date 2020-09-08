using System;
using System.Collections.Generic;

namespace MrMeeseeks.LambdaPattern
{
    public class LambdaStructComparer<T> : IComparer<T> where T : struct
    {
        private readonly Func<T, T, int> _comparingLogic;
        private readonly bool _descending;

        public LambdaStructComparer(
            Func<T, T, int> comparingLogic,
            bool descending = false)
        {
            _comparingLogic = comparingLogic;
            _descending = descending;
        }

        public int Compare(T x, T y)
        {
            return _descending
                ? -1 * _comparingLogic(x, y)
                : _comparingLogic(x, y);
        }
    }
}