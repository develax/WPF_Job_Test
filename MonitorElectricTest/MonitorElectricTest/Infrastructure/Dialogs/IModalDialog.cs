using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitorElectricTest.Infrastructure.Dialogs
{
    public interface IModalDialog<T>
    {
        void ShowModal();
        void Close();
    }
}
