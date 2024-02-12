using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitorElectric.Test.Data.Entities
{
    public enum Gender
    {
        [Description("Муж.")]
        Male = 0,

        [Description("Жен.")]
        Female = 1
    }
}
