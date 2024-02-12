using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MonitorElectricTest.Infrastructure.Dialogs
{
    public interface IConfirmDialog
    {
        bool Show(string title, string text);
    }
}
