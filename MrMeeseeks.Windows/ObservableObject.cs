using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using MrMeeseeks.Reactive.Extensions;

namespace MrMeeseeks.Windows
{
    public interface IObservableObject : INotifyPropertyChanged {}
    
    public class ObservableObject
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected IDisposable EscalateNotifications(
            IObservableObject observableObject,
            params (string Source, string Target)[] propertyNameMapping) =>
            propertyNameMapping
                .Select(pair => observableObject.ObservePropertyChanged(pair.Source).Select(_ => pair.Target))
                .Merge()
                .Subscribe(OnPropertyChanged);

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        

        protected void OnPropertyChanged(params string[] propertyNames)
        {
            foreach (string propertyName in propertyNames)
                OnPropertyChanged(propertyName);
        }

        protected void SetIfChangedAndRaise<T>(ref T field, T value,[CallerMemberName] string? propertyName = null)
        {
            if (ReferenceEquals(field, value)) return;
            field = value;
            OnPropertyChanged(propertyName);
        }
    }
}