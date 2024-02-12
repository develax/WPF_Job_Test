using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitorElectricTest.Infrastructure.Helpers
{
    internal static class CollectionHelper
    {
        public static int GetElemementIndex<T>(this IList<T> list,  Func<T, bool> predicate)
        {
            for(int i = 0; i < list.Count; i++)
            {
                T item = list[i];

                if (predicate(item))
                    return i;
            }

            return -1;
        }
    }
}
