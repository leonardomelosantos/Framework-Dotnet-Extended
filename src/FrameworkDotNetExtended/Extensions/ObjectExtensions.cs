using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;

namespace FrameworkDotNetExtended.Extensions
{
    public static class ObjectExtentions
    {

        public static dynamic Merge(this object item1, object item2)
        {
            if (item1 == null || item2 == null)
                return item1 ?? item2 ?? new ExpandoObject();

            dynamic expando = new ExpandoObject();
            var result = expando as IDictionary<string, object>;
            foreach (PropertyInfo fi in item1.GetType().GetProperties().Where(p => p.CanRead))
            {
                result[fi.Name] = fi.GetValue(item1, null);
            }
            foreach (PropertyInfo fi in item2.GetType().GetProperties().Where(p => p.CanRead))
            {
                result[fi.Name] = fi.GetValue(item2, null);
            }
            return result;
        }
    }
}
