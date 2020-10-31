using System;
using System.Reactive.Disposables;

namespace MrMeeseeks.Reactive.Extensions
{
    public static class DisposableExtensions
    {
        public static T SerializeDisposalWith<T>(this T @this, SerialDisposable serialDisposable) where T : IDisposable
        {
            serialDisposable.Disposable = @this;
            return @this;
        } 

        public static T CompositeDisposalWith<T>(this T element, CompositeDisposable compositeDisposable) where T : IDisposable
        {
            compositeDisposable.Add(element);
            return element;
        }
    }
}
