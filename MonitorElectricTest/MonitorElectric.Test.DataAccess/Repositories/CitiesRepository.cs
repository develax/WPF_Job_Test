using MonitorElectric.Test.Data.Entities;
using MonitorElectric.Test.Data.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace MonitorElectric.Test.Data.Repositories
{
    public class CitiesRepository : BaseRepository<City>, ICitiesRepository
    {
        public override async Task<City> Add_Async(City item)
        {
            await Task.Delay(10);
            int maxId = _DbItems.Keys.DefaultIfEmpty(0).Max();

            if (_DbItems.Values.Any(c => string.Compare(c.Name, item.Name, true) == 0))
                throw new ArgumentException($"City '{item.Name}' already exists.");

            item.Id = maxId + 1;
            _DbItems.Add(item.Id, item);
            _observableCollection.Add(item);
            return item;
        }

        public override async Task<City> Update_Async(City item)
        {
            await Task.Delay(1000);

            if (!_DbItems.TryGetValue(item.Id, out City found))
                throw new ArgumentException($"City Id = '{item.Id}' not found.");

            found.Name = item.Name;
            _OnItemUpdated(item);
            return found;
        }

        public override async Task Delete_Async(City item)
        {
            await Task.Delay(10);

            if (!_DbItems.TryGetValue(item.Id, out City found))
                throw new ArgumentException($"City Id = {item.Id} not found.");

            if (DbContext.Employees.Values.Any(e=>e.CityId == item.Id))
                throw new ArgumentException($"Can't delete city Id = {item.Id}, some employee is bound to it.");

            _DbItems.Remove(found.Id);
            _observableCollection.Remove(found);
        }

        public override async Task<ObservableCollection<City>> GetAll_Async()
        {
            await Task.Delay(1000);
            return _observableCollection;
        }

        #region Helpers

        protected override Dictionary<int, City> _DbItems => DbContext.Cities;

        #endregion
    }
}
