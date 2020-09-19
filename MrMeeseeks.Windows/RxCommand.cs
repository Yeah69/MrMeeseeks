using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Windows.Input;

namespace MrMeeseeks.Windows
{
    public interface IRxCommand : ICommand, IDisposable
    {
        IObservable<object> Observe { get; }

        IObservable<T> ObserveOfType<T>() => Observe.OfType<T>();

        IObservable<T> ObserveCast<T>() => Observe.Cast<T>();
    }

    public class RxCommand : IRxCommand
    {
        private readonly Subject<object> _commandTriggering = new Subject<object>();
        private readonly IDisposable _canExecuteSubscription;

        private bool _canExecute;
        
        public RxCommand(
            bool initialCanExecute = true)
        {
            _canExecuteSubscription = Disposable.Empty;
            _canExecute = initialCanExecute;
        }
        
        public RxCommand(
            IObservable<bool> observeCanExecute,
            bool initialCanExecute = true)
        {
            _canExecuteSubscription = observeCanExecute
                .Subscribe(b =>
                {
                    _canExecute = b;
                    CanExecuteChanged?.Invoke(this, EventArgs.Empty);
                });
            _canExecute = initialCanExecute;
        }

        public bool CanExecute(object parameter) => _canExecute;

        public void Execute(object parameter) => _commandTriggering.OnNext(parameter);

        public event EventHandler? CanExecuteChanged;
        public void Dispose()
        {
            _commandTriggering.Dispose();
            _canExecuteSubscription.Dispose();
        }

        public IObservable<object> Observe => _commandTriggering.AsObservable();
    }
}