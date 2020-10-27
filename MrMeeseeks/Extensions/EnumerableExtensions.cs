using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using MrMeeseeks.Utility;

namespace MrMeeseeks.Extensions
{
    public static class EnumerableExtensions
    {
        public static bool None<T>(this IEnumerable<T> enumerable) => !enumerable.Any();
        public static bool None<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate) => 
            !enumerable.Any(predicate);

        public static async Task<IEnumerable<T>> ToAwaitableEnumerable<T>(this IEnumerable<Task<T>> enumerable) =>
            await Task.WhenAll(enumerable).ConfigureAwait(false);

        public static IReadOnlyCollection<T> ToReadOnlyCollection<T>(this IEnumerable<T> enumerable) =>
            new ReadOnlyCollection<T>(enumerable.ToList());

        public static IReadOnlyList<T> ToReadOnlyList<T>(this IEnumerable<T> enumerable) =>
            new ReadOnlyCollection<T>(enumerable.ToList());

        public static IEnumerable<T> WhereNotNullRef<T>(this IEnumerable<T?> enumerable) where T : class =>
            // ReSharper disable once RedundantEnumerableCastCall
            enumerable.Where(t => !(t is null)).OfType<T>();

        public static IEnumerable<T> TakeWhileNotNullRef<T>(this IEnumerable<T?> enumerable) where T : class =>
            // ReSharper disable once RedundantEnumerableCastCall
            enumerable.TakeWhile(t => !(t is null)).OfType<T>();

        public static IEnumerable<T> WhereNotNullable<T>(this IEnumerable<T?> enumerable) where T : struct =>
            enumerable.Where(t => !(t is null)).Select(t => t!.Value);

        public static IEnumerable<T> TakeWhileNotNullable<T>(this IEnumerable<T?> enumerable) where T : struct =>
            enumerable.TakeWhile(t => !(t is null)).Select(t => t!.Value);


        public static IEnumerable<T> IterateTreeBreadthFirst<T>(this IEnumerable<T> roots, Func<T, IEnumerable<T>> childrenSelector)
        {
            var queue = new Queue<T>();
            foreach (var root in roots)
            {
                queue.Enqueue(root);
            }

            while (queue.Any())
            {
                var current = queue.Dequeue();
                yield return current;
                foreach (var child in childrenSelector(current))
                    queue.Enqueue(child);
            }
        }

        public static IEnumerable<T> IterateTreeDepthFirst<T>(
            this IEnumerable<T> roots,
            Func<T, IEnumerable<T>> childrenSelector)
        {
            return roots.IterateTreeDepthFirst(childrenSelector, GenericExtensions.DepthFirstIterationPattern.Prefix, Basic.Zero);
        }

        public static IEnumerable<T> IterateTreeDepthFirst<T>(
            this IEnumerable<T> roots,
            Func<T, IEnumerable<T>> childrenSelector,
            GenericExtensions.DepthFirstIterationPattern pattern)
        {
            return pattern switch
            {
                GenericExtensions.DepthFirstIterationPattern.Infix =>
                roots.IterateTreeDepthFirst(childrenSelector, pattern, DefaultInfixPattern),
                _ => roots.IterateTreeDepthFirst(childrenSelector, pattern, Basic.Zero)
            };

            int DefaultInfixPattern(IEnumerable<T> _) => 1;
        }

        public static IEnumerable<T> IterateTreeDepthFirst<T>(
            this IEnumerable<T> roots,
            Func<T, IEnumerable<T>> childrenSelector,
            Func<IEnumerable<T>, int> infixSplitFunc)
        {
            return roots.IterateTreeDepthFirst(childrenSelector, GenericExtensions.DepthFirstIterationPattern.Infix, infixSplitFunc);
        }

        private static IEnumerable<T> IterateTreeDepthFirst<T>(
            this IEnumerable<T> roots,
            Func<T, IEnumerable<T>> childrenSelector,
            GenericExtensions.DepthFirstIterationPattern pattern,
            Func<IEnumerable<T>, int> infixSplitFunc)
        {
            return roots.SelectMany(root => root.IterateTreeDepthFirst(childrenSelector, pattern, infixSplitFunc));
        }

        public static IEnumerable<TResult> SelectTree<TSource, TResult>(
            this IEnumerable<TSource> roots,
            Func<TSource, IEnumerable<TSource>> childrenSelector,
            Func<TSource, IEnumerable<TResult>, TResult> resultFactory) =>
            roots.SelectTree(childrenSelector, Basic.False, resultFactory);

        public static IEnumerable<TResult> SelectTree<TSource, TResult>(
            this IEnumerable<TSource> roots,
            Func<TSource, IEnumerable<TSource>> childrenSelector,
            Func<TSource, bool> leafPredicate,
            Func<TSource, IEnumerable<TResult>, TResult> resultFactory) =>
            roots.Select(e => e.SelectTree(childrenSelector, leafPredicate, resultFactory));
    }
}
