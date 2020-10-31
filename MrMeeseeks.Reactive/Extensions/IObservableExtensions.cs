using System;
using System.Reactive;
using System.Reactive.Linq;

namespace MrMeeseeks.Reactive.Extensions
{
    // ReSharper disable once InconsistentNaming
    public static class IObservableExtensions
    {
        public static IObservable<Unit> SelectUnit<T>(this IObservable<T> observable) =>
            observable.Select(_ => Unit.Default);
        
        public static IDisposable SubscribeUnit<T>(this IObservable<T> observable, Action onNextUnit) =>
            observable.Subscribe(_ => onNextUnit());
    }
}