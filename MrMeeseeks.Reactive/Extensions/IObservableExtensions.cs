using System;
using System.Reactive;
using System.Reactive.Linq;

namespace MrMeeseeks.Reactive.Extensions
{
    public static class IObservableExtensions
    {
        public static IObservable<Unit> SelectUnit<T>(this IObservable<T> observable) =>
            observable.Select(_ => Unit.Default);
    }
}