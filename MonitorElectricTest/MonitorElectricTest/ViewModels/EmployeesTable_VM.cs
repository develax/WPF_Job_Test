using MonitorElectric.Test.Data.Entities;
using MonitorElectric.Test.Data.Repositories;
using MonitorElectricTest.Infrastructure.Commands;
using MonitorElectricTest.Infrastructure.Dialogs;
using MonitorElectricTest.Infrastructure.Helpers;
using MonitorElectricTest.ViewModels.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Threading;

namespace MonitorElectricTest.ViewModels
{
    public class EmployeesTable_VM : ViewModelBase
    {
        private readonly IEmployeesRepository _employeesRepository;
        private readonly ICitiesRepository _citiesRepository;
        private readonly IConfirmDialog _confirmDialog;
        private readonly List<EmployeeTableItem_VM> _items;
        private string _filter;
        private DispatcherTimer _timer;
        private ObservableCollection<EmployeeTableItem_VM> _filteredItems;

        public EmployeesTable_VM(IEmployeesRepository employeesRepository, ICitiesRepository citiesRepository, IConfirmDialog confirmDialog, AddEmployeeDialog_VM addEmployeeDialog_VM)
        {
            _employeesRepository = employeesRepository;
            _citiesRepository = citiesRepository;
            _confirmDialog = confirmDialog;

            _items = new List<EmployeeTableItem_VM>();
            FilteredItems = new ObservableCollection<EmployeeTableItem_VM>();

            _UpdateFiteredItems();

            AddCommand = new Add_Command(addEmployeeDialog_VM);
            DeleteCommand = new Delete_Command(this);
        }


        public ObservableCollection<EmployeeTableItem_VM> FilteredItems { get => _filteredItems; private set => _filteredItems = value; }

        public string Filter
        {
            get => _filter;
            set
            {
                string v = value.TrimStart().Replace("  ", " ").Replace("\t", " ");

                if (_filter == v)
                    return;

                _SetPropertyValue(ref _filter, v);
                _UpdateFiteredItems(1);
            }
        }

        public ICommand AddCommand { get; set; }

        public ICommand DeleteCommand { get; set; }

        #region Helpers

        private event Action _SelectedItemsChanged;

        protected override async Task _OnLoaded_Async()
        {
            ObservableCollection<Employee> employees = await _employeesRepository.GetAll_Async();

            foreach (var employee in employees)
            {
                EmployeeTableItem_VM employeeTableItem = await _CreateItemVM_Async(employee);
                _items.Add(employeeTableItem);
                employeeTableItem.PropertyChanged += _OnEmployeeTableItem_PropertyChanged;
                employees.CollectionChanged += _OnEmployees_CollectionChanged;
            }

            _UpdateFiteredItems();
        }

        private async void _OnEmployees_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            int changesCount = 0;

            if (e.OldItems != null)
            {
                foreach (var newItem in e.OldItems)
                {
                    var id = ((Employee)newItem).Id;
                    int index = _items.GetElemementIndex(m => m.entity.Id == id);

                    if (index == -1)
                        continue;
                    //throw new InvalidOperationException($"Employee Id = {id} is not found in the {nameof(Items)}.");

                    _items.RemoveAt(index);
                    changesCount++;
                }
            }

            if (e.NewItems != null)
            {
                foreach (var newItem in e.NewItems)
                {
                    Employee employ = (Employee)newItem;
                    var id = employ.Id;
                    EmployeeTableItem_VM newItemVM = await _CreateItemVM_Async(employ);
                    EmployeeTableItem_VM foundItem = _items.FirstOrDefault(m => m.entity.Id == id);

                    if (foundItem != null)
                        continue;
                    //throw new InvalidOperationException($"Employee Id = {id} already exists in the {nameof(Items)}.");

                    _items.Add(newItemVM);
                    changesCount++;
                }
            }

            if (changesCount > 0)
                _UpdateFiteredItems();
        }

        private void _OnEmployeeTableItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(EmployeeTableItem_VM.IsSelected))
                return;

            _OnSelectedItemsChanged();
        }

        private void _OnSelectedItemsChanged()
        {
            _SelectedItemsChanged?.Invoke();
        }

        private async Task<EmployeeTableItem_VM> _CreateItemVM_Async(Employee employee)
        {
            City city = await _citiesRepository.Get_Async(employee.CityId);

            return new EmployeeTableItem_VM
            {
                entity = employee,
                City = city.Name,
                FullName = employee.FullName,
                Gender = employee.Gender.TryGetAttribute<DescriptionAttribute>()?.Description ?? employee.Gender.ToString(),
            };
        }

        private void _UpdateFiteredItems(double secondsDelay)
        {
            if (_timer != null)
                _timer.Stop();

            _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            _timer.Start();
            _timer.Tick += (sender, args) =>
            {
                _timer.Stop();
                _timer = null;
                _UpdateFiteredItems();
            };
        }

        private void _UpdateFiteredItems()
        {
            FilteredItems.Clear();
            string filterStr = Filter?.Trim().ToUpper().Replace("Ё", "Е");

            // TODO: фильтрация должна выполняться на уровне базы данных
            var filterLinq =
                string.IsNullOrWhiteSpace(filterStr)
                ?
                _items
                :
                _items.Where(m => m.FullName.ToUpper().Replace("Ё", "Е").IndexOf(filterStr, StringComparison.CurrentCulture) > -1);

            foreach (var item in filterLinq)
                FilteredItems.Add(item);

            _OnSelectedItemsChanged();
        }

        #endregion

        #region Commands

        private class Add_Command : CommandBase
        {
            private AddEmployeeDialog_VM _addEmployeeDialog_VM;

            public Add_Command(AddEmployeeDialog_VM addEmployeeDialog_VM)
            {
                _addEmployeeDialog_VM = addEmployeeDialog_VM;
            }

            public override void Execute(object parameter)
            {
                _addEmployeeDialog_VM.Show();
            }
        }

        private class Delete_Command : CommandBaseAsync
        {
            private EmployeesTable_VM _viewModel;

            public Delete_Command(EmployeesTable_VM viewModel)
            {
                _viewModel = viewModel;
                _viewModel._SelectedItemsChanged += _InvokeCanExecuteChanged;
            }

            public override async Task Execute_Async(object parameter)
            {
                var deletingItems = _viewModel.FilteredItems.Where(i => i.IsSelected).ToArray();
                int count = deletingItems.Length;
                bool confirmed = _viewModel._confirmDialog.Show("Удаление данных", $"Удалить {count} сотрудников из базы?");

                if (!confirmed)
                    return;

                foreach (var item in deletingItems)
                {
                    await _viewModel._employeesRepository.Delete_Async(item.entity);
                }
            }

            public override bool CanExecute(object parameter)
            {
                return _viewModel.FilteredItems.Any(i => i.IsSelected);
            }
        }

        #endregion
    }
}
