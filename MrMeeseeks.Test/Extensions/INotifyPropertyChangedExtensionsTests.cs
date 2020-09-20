using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MrMeeseeks.Reactive.Extensions;
using Xunit;

namespace MrMeeseeks.Test.Extensions
{
    public class INotifyPropertyChangedExtensionsTests
    {
        [Fact]
        public void ObservePropertyChanged_TestClass_Works()
        {
            // Arrange + Act + Assert
            using var _ = new TestClass().ObservePropertyChanged("").Subscribe(__ => {});
        }
        [Fact]
        public void ObservePropertyChanged_ObservableCollection_Works()
        {
            // Arrange + Act + Assert
            using var _ = new ObservableCollection<string>().ObservePropertyChanged("").Subscribe(__ => {});
        }
        
        private class TestClass : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler? PropertyChanged;

            protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}