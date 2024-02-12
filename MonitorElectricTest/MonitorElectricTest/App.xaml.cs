using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MonitorElectric.Test.Data.Entities;
using MonitorElectric.Test.Data.Repositories;
using MonitorElectricTest.Infrastructure.Dialogs;
using MonitorElectricTest.ViewModels;
using MonitorElectricTest.ViewModels.Dialogs;
using MonitorElectricTest.Views.Dialogs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace MonitorElectricTest
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ServiceProvider _serviceProvider;

        public static IHost AppHost { get; private set; }

        public App()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("ru-RU");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("ru-RU");
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            _SetupDI();
            await _Init_Async();

            MainWindow mainWindow = new MainWindow();
            mainWindow.DataContext = _serviceProvider.GetService<MainWindow_VM>();
            mainWindow.Show();
        }

        private async Task _Init_Async()
        {
            await _GenerateTempData_Async();
        }

        private void _SetupDI()
        {
            var services = new ServiceCollection();

            // Repositories:
            services.AddSingleton<IEmployeesRepository, EmployeesRepository>();
            services.AddSingleton<ICitiesRepository, CitiesRepository>();

            // View-Models:
            services.AddTransient<MainWindow_VM>();
            services.AddTransient<EmployeesTable_VM>();

            // Windows:
            services.AddSingleton<ConfirmDialog>();

            // Confirm Dialog:
            services.AddSingleton<IConfirmDialog, ConfirmDialog>();

            // Modal Dialogs:
            AddEmployeeDialog addEmployeeDialog = new AddEmployeeDialog();
            services.AddSingleton<IModalDialog<AddEmployeeDialog_VM>>(addEmployeeDialog);
            services.AddSingleton<AddEmployeeDialog_VM>();

            _serviceProvider = services.BuildServiceProvider();
            
            var addEmployee = _serviceProvider.GetService<AddEmployeeDialog_VM>();
            addEmployeeDialog.DataContext = addEmployee;
        }

        private async Task _GenerateTempData_Async()
        {
            ICitiesRepository citiesRepository = _serviceProvider.GetService<ICitiesRepository>();
            City city0 = await citiesRepository.Add_Async(new City { Name = "Ставрополь" });
            City city1 = await citiesRepository.Add_Async(new City { Name = "Пятигорск" });
            City city2 = await citiesRepository.Add_Async(new City { Name = "Воронеж" });
            City city3 = await citiesRepository.Add_Async(new City { Name = "Смоленск" });

            IEmployeesRepository employeesRepository = _serviceProvider.GetService<IEmployeesRepository>();
            await employeesRepository.Add_Async(new Employee { FullName = "Иванов Иван Иванович", Gender = Gender.Male, CityId = city0.Id });
            await employeesRepository.Add_Async(new Employee { FullName = "Ткачёва Людмила Ивановна", Gender = Gender.Female, CityId = city1.Id });
            await employeesRepository.Add_Async(new Employee { FullName = "Ивлева Татьяна Петровна", Gender = Gender.Female, CityId = city2.Id });
            await employeesRepository.Add_Async(new Employee { FullName = "Пётр Сергеевич Иванов", Gender = Gender.Male, CityId = city3.Id });
        }
    }
}
