using MonitorElectric.Test.Data.Entities;
using MonitorElectric.Test.Data.Repositories;
using MonitorElectricTest.Infrastructure;
using MonitorElectricTest.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitorElectricTest.ViewModels.Dialogs
{
    public class AddEmployeeForm_VM : ViewModelBase
    {
        private City_IdValue _city;
        private Gender_IdValue _gender;
        private string _fullName;
        private bool _isFormValid;
        private readonly ICitiesRepository _citiesRepository;

        public AddEmployeeForm_VM(ICitiesRepository citiesRepository)
        {
            _citiesRepository = citiesRepository;

            Cities = new ObservableCollection<City_IdValue>();
            Genders = new ObservableCollection<Gender_IdValue>();
        }

        public async Task Reset_Async()
        {
            Cities.Clear();
            Genders.Clear();
            FullName = null;
            City = null;
            Gender = null;

            foreach (Gender gender in Enum.GetValues(typeof(Gender)))
                Genders.Add(new Gender_IdValue { Id = gender, Value = gender.TryGetAttribute<DescriptionAttribute>()?.Description });

            Gender = Genders.First();

            ObservableCollection<City> cities = await _citiesRepository.GetAll_Async();

            foreach (City city in cities)
                Cities.Add(new City_IdValue { Id = city.Id, Value = city.Name });
        }

        public string FullName
        {
            get => _fullName;
            set
            {
                _SetPropertyValue(ref _fullName, value);
                _ValidateForm();
            }
        }

        public ObservableCollection<City_IdValue> Cities { get; }

        public City_IdValue City
        {
            get => _city;
            set
            {
                _city = value;
                _OnPropertyChanged(nameof(City));
                _ValidateForm();
            }
        }

        public ObservableCollection<Gender_IdValue> Genders { get; }

        public Gender_IdValue Gender
        {
            get => _gender;
            set
            {
                _gender = value;
                _OnPropertyChanged(nameof(Gender));
                _ValidateForm();
            }
        }

        public bool IsFormValid
        {
            get => _isFormValid;
            set => _SetPropertyValue(ref _isFormValid, value);
        }

        #region Helpers

        private void _ValidateForm()
        {
            IsFormValid = _IsFormValid();
        }

        private bool _IsFormValid()
        {
            if (string.IsNullOrWhiteSpace(FullName) || FullName.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Length != 3)
                return false;

            if (City == null)
                return false;

            if (Gender == null)
                return false;

            return true;
        }

        #endregion

        #region Property Types

        public class City_IdValue : IdValue_VM<int, string>
        { }

        public class Gender_IdValue : IdValue_VM<Gender, string>
        { }

        #endregion
    }
}
