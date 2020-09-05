using System;
using System.Collections.Generic;
using System.Linq;
using MrMeeseeks.Extensions;

namespace MrMeeseeks.DataStructures
{
    public class Tree<T> : TreeBase<T>
    {
        public T Value { get; }

        public IReadOnlyList<Tree<T>> Branches { get; }

        public Tree(T value)
        {
            Value = value;
            Branches = Children.ToReadOnlyList();
        }

        public Tree(T value, IEnumerable<Tree<T>> children)
        {
            Value = value;
            Branches = children.ToReadOnlyList();
        }

        public Tree<TResult> Select<TResult>(Func<T, IEnumerable<TResult>, TResult> resultSelector)
        {
            var branches = Branches.Select(b => b.Select(resultSelector)).ToList();
            var valuesOfBranches = branches.Select(b => b.Value);
            var value = resultSelector(Value, valuesOfBranches);
            return new Tree<TResult>(value, branches);
        }
    }
}
