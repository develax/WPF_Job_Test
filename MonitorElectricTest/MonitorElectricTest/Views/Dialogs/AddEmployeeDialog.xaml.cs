using MonitorElectricTest.Infrastructure.Dialogs;
using MonitorElectricTest.ViewModels.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;

namespace MonitorElectricTest.Views.Dialogs
{
    /// <summary>
    /// Interaction logic for AddEmploeeDialog.xaml
    /// </summary>
    public partial class AddEmployeeDialog : Window, IModalDialog<AddEmployeeDialog_VM>
    {
        public AddEmployeeDialog()
        {
            InitializeComponent();
        }

        public void ShowModal()
        {
            ShowDialog();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            Visibility = Visibility.Hidden;
            e.Cancel = true;
        }
    }
}
