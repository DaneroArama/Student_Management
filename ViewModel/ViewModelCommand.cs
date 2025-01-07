using System;
using System.Windows.Input;

namespace Student_Management.ViewModel
{
    public class ViewModelCommand : ICommand
    {
        // Fields
        private readonly Action<object> _executeAction;
        private readonly Predicate<object> _canExecutePredicate;

        // Constructors
        public ViewModelCommand(Action<object> executeAction)
        {
            _executeAction = executeAction;
            _canExecutePredicate = null;
        }

        public ViewModelCommand(Action<object> executeAction, Predicate<object> canExecutePredicate)
        {
            _executeAction = executeAction;
            _canExecutePredicate = canExecutePredicate;
        }

        // Event
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        // Methods
        public bool CanExecute(object parameter)
        {
            return _canExecutePredicate == null || _canExecutePredicate(parameter);
        }

        public void Execute(object parameter)
        {
            _executeAction(parameter);
        }
    }
}