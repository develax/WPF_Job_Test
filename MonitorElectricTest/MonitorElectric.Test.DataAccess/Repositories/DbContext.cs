using MonitorElectric.Test.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitorElectric.Test.Data.Repositories
{
    internal static class DbContext
    {
        internal static Dictionary<int, City> Cities { get; }
        internal static Dictionary<int, Employee> Employees { get; }

        static DbContext()
        {
            Cities = new Dictionary<int, City>();
            Employees = new Dictionary<int, Employee>();
        }
    }
}
