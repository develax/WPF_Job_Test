using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MonitorElectricTest.Infrastructure.Helpers
{
    internal static class EnumHelper
    {
        public static T TryGetAttribute<T>(this Enum val) 
            where T : Attribute
        {
            Type attrType = typeof(T);
            Type enumType = val.GetType();
            MemberInfo[] memInfo =  enumType.GetMember(val.ToString());
            object[] attributes = memInfo[0].GetCustomAttributes(typeof(T), false);

            if (attributes.Length > 1)
                throw new ArgumentException($"Too many attributes of type = '{attrType.Name}' on '{val}' enum. There should be only 1.");

            return (T)attributes.FirstOrDefault();
        }
    }
}
