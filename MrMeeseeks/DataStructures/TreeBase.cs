using System;
using System.Collections.Generic;
using System.Linq;
using MrMeeseeks.Extensions;
using MrMeeseeks.Utility;

namespace MrMeeseeks.DataStructures
{
    public abstract class TreeBase<T>
    {
        protected readonly IList<Tree<T>> Children = new List<Tree<T>>();

        public Tree<T> GetOrCreate(IEnumerable<T> path)
        {
            return GetOrCreate(path, (first, second) => Equals(first, second), Basic.Identity);
        }

        public Tree<T> GetOrCreate<TOther>(IEnumerable<TOther> path, Func<T, TOther, bool> predicate, Func<TOther, T> factory)
        {
            if (path is null || path.Any().Not()) throw new ArgumentException("Cannot process empty path.");

            var first = path.First();
            var nextChild = Children.FirstOrDefault(t => predicate(t.Value, first));
            if (nextChild is null)
            {
                nextChild = new Tree<T>(factory(first));
                Children.Add(nextChild);
            }

            var nextPath = path.Skip(1);
            return nextPath.Any()
                ? nextChild.GetOrCreate(nextPath, predicate, factory)
                : nextChild;
        }
    }
}