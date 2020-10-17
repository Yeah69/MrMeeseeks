using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MrMeeseeks.Windows
{
    public interface IObservableObject : INotifyPropertyChanged {}
    
    public class ObservableObject
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void OnPropertyChanged(params string[] propertyNames)
        {
            foreach (string propertyName in propertyNames)
            {
                OnPropertyChanged(propertyName);
            }
        }

        protected void SetIfChangedAndRaise<T>(ref T field, T value,[CallerMemberName] string? propertyName = null)
        {
            if (ReferenceEquals(field, value)) return;
            field = value;
            OnPropertyChanged(propertyName);
        }
    }
}