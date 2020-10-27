using System;
using System.Collections.Generic;

namespace MrMeeseeks.LambdaPattern
{
    public class LambdaComparer<T> : IComparer<T> where T : class
    {
        private readonly Func<T, T, int> _comparingLogic;
        private readonly bool _descending;
        private readonly bool _skipDefaultNullComparison;

        public LambdaComparer(
            Func<T, T, int> comparingLogic,
            bool descending = false,
            bool skipDefaultNullComparison = false)
        {
            _comparingLogic = comparingLogic;
            _descending = descending;
            _skipDefaultNullComparison = skipDefaultNullComparison;
        }

        public int Compare(T x, T y)
        {
            if(!_skipDefaultNullComparison)
            {
                // ReSharper disable ConditionIsAlwaysTrueOrFalse
                if (x is null && y is null)
                    return 0;
                if (x is null)
                    return _descending ? 1 : -1;
                if (y is null)
                    return _descending ? -1 : 1;
                // ReSharper restore ConditionIsAlwaysTrueOrFalse
            }
            return _descending 
                ? -1 * _comparingLogic(x, y)
                : _comparingLogic(x, y);
        }
    }
}
