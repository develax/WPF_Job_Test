using MonitorElectricTest.Infrastructure.Commands;
using MonitorElectricTest.Infrastructure.Dialogs;
using MonitorElectricTest.ViewModels;
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
    /// Interaction logic for ConfirmDialog.xaml
    /// </summary>
    public partial class ConfirmDialog : Window, IConfirmDialog
    {
        private readonly ViewModel _viewModel;
        private bool _result;

        public ConfirmDialog()
        {
            DataContext = _viewModel = new ViewModel();
            InitializeComponent();
        }

        public bool Show(string title, string text)
        {
            _viewModel.Title = title;
            _viewModel.Text = text;
            _result = false;
            ShowDialog();
            return _result;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            Visibility = Visibility.Hidden;
            e.Cancel = true;
        }

        private void _OnConfirm(object sender, RoutedEventArgs e)
        {
            _result = true;
            Close();
        }

        private void _OnReject(object sender, RoutedEventArgs e)
        {
            _result = false;
            Close();
        }

        private void _OnShown(object sender, DependencyPropertyChangedEventArgs e)
        {
            noButton.Focus();
        }

        #region DataContext

        private class ViewModel : ViewModelBase
        {
            private string _title = "Test";
            private string _text = "Test";

            public string Title
            {
                get => _title;
                set => _SetPropertyValue(ref _title, value);
            }

            public string Text
            {
                get => _text;
                set => _SetPropertyValue(ref _text, value);
            }
        }

        #endregion
    }
}
