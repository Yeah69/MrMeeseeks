using System;
using System.Collections.Generic;
using System.Linq;
using MrMeeseeks.Extensions;

namespace MrMeeseeks.DataStructures
{
    public class Forest<T> : TreeBase<T>
    {
        public IReadOnlyList<Tree<T>> Trees { get; }

        public Forest()
        {
            Trees = Children.ToReadOnlyList();
        }

        public Forest<TResult> Select<TResult>(Func<T, IEnumerable<TResult>, TResult> resultSelector)
        {
            var forest = new Forest<TResult>();
            foreach (var tree in Trees.Select(t => t.Select(resultSelector)))
            {
                forest.Children.Add(tree);
            }
            return forest;
        }
    }
}