using MonitorElectric.Test.Data.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MonitorElectric.Test.Data.Repositories
{
    public abstract class BaseRepository<T> : IRepository<T>
        where T : Entity
    {
        protected ObservableCollection<T> _observableCollection = new ObservableCollection<T>();

        public event Action<T> ItemUpdated;

        public abstract Task<T> Add_Async(T item);

        public abstract Task<ObservableCollection<T>> GetAll_Async();

        public abstract Task Delete_Async(T item);

        public abstract Task<T> Update_Async(T item);

        public virtual async Task<T> Get_Async(int id)
        {
            await Task.Delay(1);

            if (!_DbItems.TryGetValue(id, out var item))
                throw new ArgumentException($"{typeof(T).Name} Id = {id} not found.");

            return item;
        }

        #region Helpers

        protected abstract Dictionary<int, T> _DbItems { get; }

        protected void _OnItemUpdated(T item)
        {
            ItemUpdated?.Invoke(item);
        }

        #endregion
    }
}
