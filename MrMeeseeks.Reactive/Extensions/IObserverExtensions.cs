using System;
using System.Reactive;

namespace MrMeeseeks.Reactive.Extensions
{
    // ReSharper disable once InconsistentNaming
    public static class IObserverExtensions
    {
        public static void OnNextUnit(this IObserver<Unit> observer) => observer.OnNext(Unit.Default);
    }
}