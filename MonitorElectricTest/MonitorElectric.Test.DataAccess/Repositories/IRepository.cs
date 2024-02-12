using MonitorElectric.Test.Data.Entities;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System;

namespace MonitorElectric.Test.Data.Repositories
{
    public interface IRepository<T>
        where T : Entity
    {
        Task<T> Get_Async(int id);

        Task<T> Add_Async(T item);

        Task<T> Update_Async(T item);

        Task Delete_Async(T item);

        Task<ObservableCollection<T>> GetAll_Async();
    }
}