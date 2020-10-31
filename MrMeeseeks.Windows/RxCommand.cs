using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Windows.Input;

namespace MrMeeseeks.Windows
{
    public interface IDisposableRxCommand : IRxCommand, IDisposable
    {
    }
    
    public interface IRxCommand : ICommand
    {
        IObservable<object> Observe { get; }

        IObservable<T> ObserveOfType<T>() => Observe.OfType<T>();

        IObservable<T> ObserveCast<T>() => Observe.Cast<T>();
    }

    public static class RxCommand
    {
        public static IRxCommand CanNeverExecute() => RxCommandCanNeverExecute.Instance;
        
        public static IDisposableRxCommand CanAlwaysExecute() => new RxCommandCanAlwaysExecute();
        
        public static IRxCommand CanAlwaysExecuteNeverEmits() => RxCommandCanAlwaysExecuteNeverEmits.Instance;

        public static IDisposableRxCommand CallerDeterminedCanExecute(IObservable<bool> observeCanExecute, bool initialCanExecute) =>
            new RxCommandCallerDeterminedCanExecute(observeCanExecute, initialCanExecute);
        
        public static IDisposableRxCommand CallerDeterminedCanExecute(IObservable<bool> observeCanExecute) =>
            new RxCommandCallerDeterminedCanExecute(observeCanExecute, false);

        public static IDisposableRxCommand CallerDeterminedCanExecuteNeverEmits(IObservable<bool> observeCanExecute, bool initialCanExecute) =>
            new RxCommandCallerDeterminedCanExecuteNeverEmits(observeCanExecute, initialCanExecute);
        
        public static IDisposableRxCommand CallerDeterminedCanExecuteNeverEmits(IObservable<bool> observeCanExecute) =>
            new RxCommandCallerDeterminedCanExecuteNeverEmits(observeCanExecute, false);
    }

    public static class ObservableRxCommandExtensions
    {
        public static IDisposableRxCommand AsCanExecuteForRxCommand(
            this IObservable<bool> observeCanExecute,
            bool initialCanExecute) =>
            RxCommand.CallerDeterminedCanExecute(observeCanExecute, initialCanExecute);
        
        public static IDisposableRxCommand AsCanExecuteForRxCommand(this IObservable<bool> observeCanExecute) =>
            RxCommand.CallerDeterminedCanExecute(observeCanExecute);
        
        public static IDisposableRxCommand AsCanExecuteForRxCommandThatNeverEmits(
            this IObservable<bool> observeCanExecute,
            bool initialCanExecute) =>
            RxCommand.CallerDeterminedCanExecuteNeverEmits(observeCanExecute, initialCanExecute);
        
        public static IDisposableRxCommand AsCanExecuteForRxCommandThatNeverEmits(this IObservable<bool> observeCanExecute) =>
            RxCommand.CallerDeterminedCanExecuteNeverEmits(observeCanExecute);
    }

    internal class RxCommandCanNeverExecute : IRxCommand
    {
        static RxCommandCanNeverExecute()
        {
            Instance = new RxCommandCanNeverExecute();
        }
        
        internal static IRxCommand Instance { get; }
        
        private RxCommandCanNeverExecute()
        {}
        
        public bool CanExecute(object _) => false;

        public void Execute(object _)
        {}
        
#pragma warning disable 67 // CanExecuteChange never used, because CanExecute never changes
        public event EventHandler? CanExecuteChanged;
#pragma warning restore 67

        public IObservable<object> Observe => Observable.Never<object>();
    }

    
    internal class RxCommandCanAlwaysExecute : IDisposableRxCommand
    {
        private readonly Subject<object> _commandTriggering = new Subject<object>();

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter) => _commandTriggering.OnNext(parameter);
        
#pragma warning disable 67 // CanExecuteChange never used, because CanExecute never changes
        public event EventHandler? CanExecuteChanged;
#pragma warning restore 67
        public void Dispose() => _commandTriggering.Dispose();

        public IObservable<object> Observe => _commandTriggering.AsObservable();
    }
    
    internal class RxCommandCanAlwaysExecuteNeverEmits : IRxCommand
    {
        static RxCommandCanAlwaysExecuteNeverEmits()
        {
            Instance = new RxCommandCanAlwaysExecuteNeverEmits();
        }
        
        internal static IRxCommand Instance { get; }
        
        private RxCommandCanAlwaysExecuteNeverEmits()
        {}
        
        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter) {}
        
#pragma warning disable 67 // CanExecuteChange never used, because CanExecute never changes
        public event EventHandler? CanExecuteChanged;
#pragma warning restore 67

        public IObservable<object> Observe => Observable.Never<object>();
    }
    
    internal class RxCommandCallerDeterminedCanExecute : IDisposableRxCommand
    {
        private readonly Subject<object> _commandTriggering = new Subject<object>();
        private readonly IDisposable _canExecuteSubscription;

        private bool _canExecute;
        
        internal RxCommandCallerDeterminedCanExecute(
            IObservable<bool> observeCanExecute,
            bool initialCanExecute)
        {
            _canExecute = initialCanExecute;
            _canExecuteSubscription = observeCanExecute
                .Subscribe(b =>
                {
                    _canExecute = b;
                    CanExecuteChanged?.Invoke(this, EventArgs.Empty);
                });
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
    
    internal class RxCommandCallerDeterminedCanExecuteNeverEmits : IDisposableRxCommand
    {
        private readonly IDisposable _canExecuteSubscription;

        private bool _canExecute;
        
        internal RxCommandCallerDeterminedCanExecuteNeverEmits(
            IObservable<bool> observeCanExecute,
            bool initialCanExecute)
        {
            _canExecute = initialCanExecute;
            _canExecuteSubscription = observeCanExecute
                .Subscribe(b =>
                {
                    _canExecute = b;
                    CanExecuteChanged?.Invoke(this, EventArgs.Empty);
                });
        }

        public bool CanExecute(object parameter) => _canExecute;

        public void Execute(object parameter) {}

        public event EventHandler? CanExecuteChanged;
        public void Dispose() => _canExecuteSubscription.Dispose();

        public IObservable<object> Observe => Observable.Never<object>();
    }
}