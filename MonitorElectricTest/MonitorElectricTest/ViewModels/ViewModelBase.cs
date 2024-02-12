using MonitorElectricTest.Infrastructure.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MonitorElectricTest.ViewModels
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public ViewModelBase()
        {
            LoadedCommand = new Loaded_CommandAsync(this);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand LoadedCommand { get; }

        public virtual void Dispose() { }

        #region Helpers

        protected virtual Task _OnLoaded_Async() => Task.CompletedTask;

        protected virtual void _OnPropertyChanged(string propertyName = null)
        {
            _VeryfyPropertyName(propertyName);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void _SetPropertyValue<T>(ref T target, T newValue, [CallerMemberName]string propertyName = null)
            where T : IComparable
        {
            if (target == null)
            {
                if (newValue == null)
                    return;
            }
            else if (target.CompareTo(newValue) == 0)
            {
                return;
            }

            target = newValue;
            _OnPropertyChanged(propertyName);
        }

        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        private void _VeryfyPropertyName(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
                return;

            if (TypeDescriptor.GetProperties(this)[propertyName] != null)
                return;

            throw new Exception($"Invalid property name: {propertyName}");
        }

        #endregion

        #region CloseCommand

        private class Loaded_CommandAsync : CommandBaseAsync
        {
            private ViewModelBase _viewModel;

            public Loaded_CommandAsync(ViewModelBase viewModel)
            {
                _viewModel = viewModel;
            }

            public override Task Execute_Async(object parameter)
            {
                return _viewModel._OnLoaded_Async();
            }
        }

        #endregion
    }
}
