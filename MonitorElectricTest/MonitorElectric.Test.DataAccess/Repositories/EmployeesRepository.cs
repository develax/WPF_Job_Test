using MonitorElectric.Test.Data.Entities;
using MonitorElectric.Test.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitorElectric.Test.Data.Repositories
{
    public class EmployeesRepository : BaseRepository<Employee>, IEmployeesRepository
    {
        protected override Dictionary<int, Employee> _DbItems => DbContext.Employees;

        public async override Task<Employee> Add_Async(Employee item)
        {
            await Task.Delay(10);
            int maxId = _DbItems.Keys.DefaultIfEmpty(0).Max();
            item.FullName = item.FullName.Replace("  ", " ").Replace("\t", " ");

            if (_DbItems.Values.Any(c => string.Compare(c.FullName, item.FullName, true) == 0))
                throw new ArgumentException($"Employee '{item.FullName}' already exists.");

            if (!DbContext.Cities.Values.Any(c=>c.Id == item.CityId))
                throw new ArgumentException($"City.Id = {item.CityId} not found.");

            item.Id = maxId + 1;
            _DbItems.Add(item.Id, item);
            _observableCollection.Add(item);
            return item;
        }

        public async override Task<Employee> Update_Async(Employee item)
        {
            await Task.Delay(1000);

            if (!_DbItems.TryGetValue(item.Id, out Employee found))
                throw new ArgumentException($"Employee Id = '{item.Id}' not found.");

            found.FullName = item.FullName;
            found.Gender = item.Gender;
            _OnItemUpdated(item);
            return found;
        }

        public async override Task Delete_Async(Employee item)
        {
            await Task.Delay(10);

            if (!_DbItems.TryGetValue(item.Id, out Employee found))
                throw new ArgumentException($"Employee Id = {item.Id} not found.");

            _DbItems.Remove(found.Id);
            _observableCollection.Remove(found);
        }

        public async override Task<ObservableCollection<Employee>> GetAll_Async()
        {
            await Task.Delay(1000);
            return _observableCollection;
        }
    }
}
