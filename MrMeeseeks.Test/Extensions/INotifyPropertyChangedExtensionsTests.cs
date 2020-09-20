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
        
        [Fact]
        public void ObservePropertyChanged_ChangingTestProp_NotificationEmitted()
        {
            // Arrange
            bool wasTriggered = false;
            var sut = new TestClass();
            using var _ = sut.ObservePropertyChanged(nameof(TestClass.TestProp)).Subscribe(__ => wasTriggered = true);
            
            // Act
            sut.TestProp = 69;
            
            // Assert
            Assert.True(wasTriggered);
        }
        
        private class TestClass : INotifyPropertyChanged
        {
            private int _testProp;
            public event PropertyChangedEventHandler? PropertyChanged;

            public int TestProp
            {
                get => _testProp;
                set
                {
                    _testProp = value;
                    OnPropertyChanged();
                }
            }

            private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}