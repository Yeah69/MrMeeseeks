using System;
using System.Collections.Generic;

namespace MrMeeseeks.LambdaPattern
{
    public class LambdaEqualityComparer<T> : IEqualityComparer<T> where T : class
    {
        private readonly Func<T?, T?, bool>? _compareLogic;
        private readonly Func<T, int>? _getHashCodeLogic;
        private readonly bool _skipDefaultNullComparison;

        public LambdaEqualityComparer(
            Func<T?, T?, bool>? compareLogic = null,
            Func<T, int>? getHashCodeLogic = null,
            bool skipDefaultNullComparison = false)
        {
            _compareLogic = compareLogic;
            _getHashCodeLogic = getHashCodeLogic;
            _skipDefaultNullComparison = skipDefaultNullComparison;
        }

        public bool Equals(T? x, T? y)
        {
            if (_skipDefaultNullComparison) return _compareLogic?.Invoke(x, y) ?? object.Equals(x, y);
            
            if (x is null && y is null)
                return true;
            if (x is null || y is null)
                return false;
            return _compareLogic?.Invoke(x, y) ?? object.Equals(x, y);
        }

        public int GetHashCode(T obj)
        {
            return _getHashCodeLogic is null
                ? obj.GetHashCode()
                : _getHashCodeLogic(obj);
        }
    }
}
