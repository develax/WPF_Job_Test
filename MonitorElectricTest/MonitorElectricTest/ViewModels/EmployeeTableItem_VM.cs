using MonitorElectric.Test.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitorElectricTest.ViewModels
{
    public class EmployeeTableItem_VM : EmployeeBase_VM
    {
        private bool _isSelected;

        public bool IsSelected
        {
            get => _isSelected;
            set => _SetPropertyValue(ref _isSelected, value);
        }
    }
}
