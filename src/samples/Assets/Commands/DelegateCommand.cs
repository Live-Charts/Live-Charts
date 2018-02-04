using System;
using System.Windows.Input;

namespace Assets.Commands
{
    public class DelegateCommand : ICommand
    {
        public Action<object> Action { get; set; }

        public DelegateCommand(Action<object> action)
        {
            Action = action;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Action(parameter);
        }

        public event EventHandler CanExecuteChanged;
    }
}
