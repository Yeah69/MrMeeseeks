using System;
using System.Collections.Generic;

namespace MrMeeseeks.LambdaPattern
{
    public class LambdaStructEqualityComparer<T> : IEqualityComparer<T> where T : struct
    {
        private readonly Func<T, T, bool>? _compareLogic;
        private readonly Func<T, int>? _getHashCodeLogic;

        public LambdaStructEqualityComparer(
            Func<T, T, bool>? compareLogic = null,
            Func<T, int>? getHashCodeLogic = null)
        {
            _compareLogic = compareLogic;
            _getHashCodeLogic = getHashCodeLogic;
        }

        public bool Equals(T x, T y)
        {
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