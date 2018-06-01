using System;
using System.Linq;
using System.Reflection;

namespace WebApp.Filter
{
    public class ExcludePropertyAttribute : Attribute
    {
    }
    public static class TypeExtensions
    {
        public static PropertyInfo[] GetFilteredProperties(this Type type)
        {
            return type.GetProperties().Where(pi => !Attribute.IsDefined(pi, typeof(ExcludePropertyAttribute))).ToArray();
        }
    }

}