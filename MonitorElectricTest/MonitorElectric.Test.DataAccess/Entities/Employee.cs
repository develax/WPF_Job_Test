using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitorElectric.Test.Data.Entities
{
    public class Employee : Entity
    {
        public int CityId { get; set; }
        public string FullName { get; set; }
        public Gender Gender { get; set; }
    }
}
