using MonitorElectric.Test.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitorElectricTest.ViewModels
{
    internal class MainWindow_VM
    {
        public EmployeesTable_VM EmployeesTable { get; }

        public MainWindow_VM(EmployeesTable_VM employeesTable)
        {
            EmployeesTable = employeesTable;
        }
    }
}
