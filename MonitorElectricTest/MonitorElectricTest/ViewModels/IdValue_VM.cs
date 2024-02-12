using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitorElectricTest.ViewModels
{
    public class IdValue_VM<TId, TValue> : ViewModelBase
        where TId : IComparable
        where TValue : IComparable
    {
        private TId _id;
        private TValue _value;

        public TId Id
        {
            get => _id;
            set => _SetPropertyValue(ref _id, value);
        }
        public TValue Value
        {
            get => _value;
            set => _SetPropertyValue(ref _value, value);
        }
    }
}
