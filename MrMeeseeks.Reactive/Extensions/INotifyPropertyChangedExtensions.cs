using System;
using System.ComponentModel;
using System.Reactive.Linq;
using MoreLinq;
using MrMeeseeks.Extensions;

namespace MrMeeseeks.Reactive.Extensions
{
    // ReSharper disable once InconsistentNaming
    public static class INotifyPropertyChangedExtensions
    {
        public static IObservable<string> ObservePropertyChanged(
            this INotifyPropertyChanged notifyPropertyChanged,
            string propertyName) =>
            Observable
                .FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
                    handler => handler.Invoke,
                    h => notifyPropertyChanged.PropertyChanged += h,
                    h => notifyPropertyChanged.PropertyChanged -= h)
                .Where(e =>
                    // if null or empty than all properties potentially could have changed
                    // https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged.propertychanged?view=netcore-3.1#remarks
                    e.EventArgs.PropertyName.IsNullOrEmpty()
                    || propertyName.Equals(e.EventArgs.PropertyName))
                .Select(e => e.EventArgs.PropertyName);

        public static IObservable<string> ObservePropertyChanged(
            this INotifyPropertyChanged notifyPropertyChanged,
            params string[] propertyNames)
        {
            var propertyNamesHashSet = propertyNames.ToHashSet();
            return Observable
                .FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
                    handler => handler.Invoke,
                    h => notifyPropertyChanged.PropertyChanged += h,
                    h => notifyPropertyChanged.PropertyChanged -= h)
                .Where(e =>
                    // if null or empty than all properties potentially could have changed
                    // https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged.propertychanged?view=netcore-3.1#remarks
                    e.EventArgs.PropertyName.IsNullOrEmpty()
                    || propertyNamesHashSet.Contains(e.EventArgs.PropertyName))
                .Select(e => e.EventArgs.PropertyName);
        }
    }
}