using System;
using System.Reactive;

namespace MrMeeseeks.Reactive.Extensions
{
    public static class IObserverExtensions
    {
        public static void OnNextUnit(this IObserver<Unit> observer) => observer.OnNext(Unit.Default);
    }
}