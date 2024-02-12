using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MonitorElectricTest.Views
{
    /// <summary>
    /// Interaction logic for EmployeesTable.xaml
    /// </summary>
    public partial class EmployeesTable : UserControl
    {
        public EmployeesTable()
        {
            InitializeComponent();
        }

        private void _OnLoaded(object sender, RoutedEventArgs e)
        {
            filter.Focus();
        }
    }
}
