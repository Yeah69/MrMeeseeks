using System;
using System.Collections.Generic;
using System.Linq;
using MrMeeseeks.Utility;

namespace MrMeeseeks.Extensions
{
    public static class GenericExtensions
    {
        public static bool IsNull<T>(this T? element) where T : class => element is null;

        public static bool IsNull<T>(this T? element) where T : struct => element is null;

        public static bool IsNotNull<T>(this T? element) where T : class => element is {};

        public static bool IsNotNull<T>(this T? element) where T : struct => element is {};

        public static T AddTo<T, TItem>(this T element, ICollection<TItem> collection) where T : TItem
        {
            collection.Add(element);
            return element;
        }

        // ReSharper disable once RedundantAssignment
        public static T SetTo<T, TSet>(this T element, out TSet reference) where T : TSet
        {
            reference = element;
            return element;
        }

        // ReSharper disable once RedundantAssignment
        public static T SetTo<T, TSet>(this T element, Action<TSet> setAction) where T : TSet
        {
            setAction(element);
            return element;
        }

        public static IEnumerable<T> ToEnumerable<T>(this T element)
        {
            yield return element;
        }
        
        public static IDisposable AsDisposable(this object element) => element as IDisposable ?? EmptyDisposable.Instance;
        
        /// <summary>
        /// !!! Copied from System.Reactive (https://github.com/dotnet/reactive), because I didn't want to depend on it in this project !!!
        /// Represents a disposable that does nothing on disposal.
        /// </summary>
        private sealed class EmptyDisposable : IDisposable
        {
            /// <summary>
            /// Singleton default disposable.
            /// </summary>
            public static readonly EmptyDisposable Instance = new EmptyDisposable();

            private EmptyDisposable()
            {
            }

            /// <summary>
            /// Does nothing.
            /// </summary>
            public void Dispose()
            {
                // no op
            }
        }

        public static IEnumerable<T> IterateTreeBreadthFirst<T>(this T root, Func<T, IEnumerable<T>> childrenSelector)
        {
            var queue = new Queue<T>();
            queue.Enqueue(root);

            while (queue.Any())
            {
                var current = queue.Dequeue();
                yield return current;
                foreach (var child in childrenSelector(current) ?? Enumerable.Empty<T>())
                    queue.Enqueue(child);
            }
        }

        public enum DepthFirstIterationPattern
        {
            Prefix,
            Infix,
            Postfix
        }

        public static IEnumerable<T> IterateTreeDepthFirst<T>(
            this T root,
            Func<T, IEnumerable<T>> childrenSelector)
        {
            return root.IterateTreeDepthFirst(childrenSelector, DepthFirstIterationPattern.Prefix, Basic.Zero);
        }

        public static IEnumerable<T> IterateTreeDepthFirst<T>(
            this T root,
            Func<T, IEnumerable<T>> childrenSelector,
            DepthFirstIterationPattern pattern)
        {
            return pattern switch
                {
                DepthFirstIterationPattern.Infix =>
                root.IterateTreeDepthFirst(childrenSelector, pattern, DefaultInfixPattern),
                _ => root.IterateTreeDepthFirst(childrenSelector, pattern, Basic.Zero)
                };

            int DefaultInfixPattern(IEnumerable<T> _) => 1;
        }

        public static IEnumerable<T> IterateTreeDepthFirst<T>(
            this T root,
            Func<T, IEnumerable<T>> childrenSelector,
            Func<IEnumerable<T>, int> infixSplitFunc)
        {
            return root.IterateTreeDepthFirst(childrenSelector, DepthFirstIterationPattern.Infix, infixSplitFunc);
        }

        internal static IEnumerable<T> IterateTreeDepthFirst<T>(
            this T root,
            Func<T, IEnumerable<T>> childrenSelector,
            DepthFirstIterationPattern pattern,
            Func<IEnumerable<T>, int> infixSplitFunc)
        {
            return pattern switch
            {
                DepthFirstIterationPattern.Prefix => InnerPrefix(),
                DepthFirstIterationPattern.Infix => InnerInfix(),
                DepthFirstIterationPattern.Postfix => InnerPostfix(),
                _ => throw new ArgumentException("Unknown iteration pattern.")
            };

            IEnumerable<T> InnerPrefix()
            {
                var stack = new Stack<T>();
                stack.Push(root);

                while (stack.Any())
                {
                    var current = stack.Pop();
                    yield return current;
                    foreach (var child in (childrenSelector(current) ?? Enumerable.Empty<T>()).Reverse())
                        stack.Push(child);
                }
            }

            IEnumerable<T> InnerPostfix()
            {
                IEnumerable<T> Inner(T node)
                {
                    foreach (var child in childrenSelector(node) ?? Enumerable.Empty<T>())
                    {
                        foreach (var yield in Inner(child))
                        {
                            yield return yield;
                        }
                    }
                    yield return node;
                }

                return Inner(root);
            }

            IEnumerable<T> InnerInfix()
            {
                IEnumerable<T> Inner(T node)
                {
                    var children = (childrenSelector(node) ?? Enumerable.Empty<T>()).ToList();
                    var split = Math.Max(0, Math.Min(children.Count, infixSplitFunc(children)));
                    foreach (var child in children.Take(split))
                    {
                        foreach (var yield in Inner(child))
                        {
                            yield return yield;
                        }
                    }

                    yield return node;

                    foreach (var child in children.Take(split))
                    {
                        foreach (var yield in Inner(child))
                        {
                            yield return yield;
                        }
                    }
                }

                return Inner(root);
            }
        }

        public static TResult SelectTree<TSource, TResult>(
            this TSource root,
            Func<TSource, IEnumerable<TSource>> childrenSelector,
            Func<TSource, IEnumerable<TResult>, TResult> resultFactory) =>
            root.SelectTree(childrenSelector, Basic.False, resultFactory);

        public static TResult SelectTree<TSource, TResult>(
            this TSource root,
            Func<TSource, IEnumerable<TSource>> childrenSelector,
            Func<TSource, bool> leafPredicate,
            Func<TSource, IEnumerable<TResult>, TResult> resultFactory) =>
            resultFactory(
                root, 
                leafPredicate(root) 
                    ? Enumerable.Empty<TResult>() 
                    : (childrenSelector(root) 
                       ?? Enumerable.Empty<TSource>())
                    .Select(element => 
                        SelectTree(
                            element, 
                            childrenSelector, 
                            leafPredicate, 
                            resultFactory)));
    }
}
