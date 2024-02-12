using MonitorElectric.Test.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitorElectricTest.ViewModels
{
    public abstract class EmployeeBase_VM : ViewModelBase
    {
        private string _fullName;
        private string _genter;
        private string _city;

        public Employee entity;

        public string FullName
        {
            get => _fullName;
            set => _SetPropertyValue(ref _fullName, value);
        }

        public string Gender
        {
            get => _genter;
            set => _SetPropertyValue(ref _genter, value);
        }

        public string City
        {
            get => _city;
            set => _SetPropertyValue(ref _city, value);
        }
    }
}
