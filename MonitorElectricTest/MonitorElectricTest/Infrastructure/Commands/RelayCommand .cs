using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MonitorElectricTest.Infrastructure.Commands
{
    public class RelayCommand : ICommand
    {
        private readonly Action _action;
        private readonly Func<bool> _canExecute;

        public RelayCommand(Action action, Func<bool> canExecute = null)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            _action = action;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            if (_canExecute == null)
                return true;

            return _canExecute();
        }

        public void Execute(object parameter) => _action();

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
