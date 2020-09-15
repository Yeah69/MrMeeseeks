using System;
using System.ComponentModel;
using System.Reactive.Linq;
using MoreLinq;
using MrMeeseeks.Extensions;

namespace MrMeeseeks.Reactive.Extensions
{
    public static class INotifyPropertyChangedExtensions
    {
        public static IObservable<string> ObserveProperty(
            this INotifyPropertyChanged notifyPropertyChanged,
            params string[] propertyNames)
        {
            var propertyNamesHashSet = propertyNames.WhereNotNull().ToHashSet();
            return Observable
                .FromEventPattern<PropertyChangedEventArgs>(
                    notifyPropertyChanged,
                    nameof(notifyPropertyChanged.PropertyChanged))
                .Where(e => propertyNamesHashSet.Contains(e.EventArgs.PropertyName))
                .Select(e => e.EventArgs.PropertyName);
        }
    }
}