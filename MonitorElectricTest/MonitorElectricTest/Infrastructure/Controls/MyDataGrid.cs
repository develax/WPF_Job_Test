using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MonitorElectricTest.Infrastructure.Controls
{
    public class MyDataGrid : DataGrid
    {
        public static readonly DependencyProperty SelectedItems1 = DependencyProperty.Register(
            nameof(SelectedItems1),
            typeof(MyDataGrid), 
            typeof(DataGrid)
            );
    }
}
