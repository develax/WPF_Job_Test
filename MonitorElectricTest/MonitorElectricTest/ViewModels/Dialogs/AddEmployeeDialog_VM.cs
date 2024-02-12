using MonitorElectric.Test.Data.Entities;
using MonitorElectric.Test.Data.Repositories;
using MonitorElectricTest.Infrastructure.Commands;
using MonitorElectricTest.Infrastructure.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MonitorElectricTest.ViewModels.Dialogs
{
    public class AddEmployeeDialog_VM : ViewModelBase
    {
        private readonly IModalDialog<AddEmployeeDialog_VM> _modalDialog;
        private readonly IEmployeesRepository _employeesRepository;

        public AddEmployeeDialog_VM(IModalDialog<AddEmployeeDialog_VM> modalDialog, ICitiesRepository citiesRepository, IEmployeesRepository employeesRepository)
        {
            _modalDialog = modalDialog;
            _employeesRepository = employeesRepository;

            Form = new AddEmployeeForm_VM(citiesRepository);

            SumbitCommand = new Submit_Command(this);
            CancelCommand = new Cancel_Command(this); 
        }

        public AddEmployeeForm_VM Form { get; }

        public ICommand SumbitCommand { get; }

        public ICommand CancelCommand { get; }

        public void Show()
        {
            _ = Form.Reset_Async();
            _modalDialog.ShowModal();
        }

        #region Commands

        private abstract class Base_Command : CommandBaseAsync
        {
            protected readonly AddEmployeeDialog_VM _vm;

            public Base_Command(AddEmployeeDialog_VM vm)
            {
                _vm = vm;
            }
        }

        private class Submit_Command : Base_Command
        {
            public Submit_Command(AddEmployeeDialog_VM vm)
                :base(vm)
            {
                vm.Form.PropertyChanged += _OnForm_PropertyChanged;
            }

            public override async Task Execute_Async(object parameter)
            {
                var form = _vm.Form;

                Employee employee = new Employee
                {
                    CityId = form.City.Id,
                    FullName = form.FullName,
                    Gender = form.Gender.Id
                };

                try
                {
                    await _vm._employeesRepository.Add_Async(employee);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка добавления в базу");
                }
                
                _vm._modalDialog.Close();
            }

            public override bool CanExecute(object parameter)
            {
                return _vm.Form.IsFormValid;
            }

            private void _OnForm_PropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                if (e.PropertyName != nameof(_vm.Form.IsFormValid))
                    return;

                _InvokeCanExecuteChanged();
            }
        }

        private class Cancel_Command : Base_Command
        {
            public Cancel_Command(AddEmployeeDialog_VM vm)
                : base(vm)
            {
            }

            public override Task Execute_Async(object parameter)
            {
                _vm._modalDialog.Close();
                return Task.CompletedTask;
            }
        }

        #endregion
    }
}
