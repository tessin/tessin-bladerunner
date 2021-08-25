using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Tessin.Bladerunner.Controls
{
    public static class PropertyListBuilder
    {
        public static _PropertyListBuilder<T> Create<T>(T obj)
        {
            return new _PropertyListBuilder<T>(obj);
        }
    }

    public class _PropertyListBuilder<T>
    {
        private object _obj;

        private Dictionary<string, Property> _properties;

        internal _PropertyListBuilder(object obj, string width = null)
        {
            _obj = obj;
            Scaffold();
        }

        private void Scaffold()
        {
            _properties = new Dictionary<string, Property>();

            var type = typeof(T);

            var fields = type
                .GetFields()
                .Select(e => new { e.Name, Type = e.FieldType, FieldInfo = e, PropertyInfo = (PropertyInfo)null })
                .Union(
                    type
                        .GetProperties()
                        .Select(e => new { e.Name, Type = e.PropertyType, FieldInfo = (FieldInfo)null, PropertyInfo = e })
                )
                //.OrderBy(e => e.Name)
                .ToList();


            foreach (var field in fields)
            {
                _properties[field.Name] =
                    new Property(Regex.Replace(field.Name, "(\\B[A-Z])", " $1"), field.FieldInfo?.GetValue(_obj) ?? field.PropertyInfo?.GetValue(_obj));
            }
        }

        public PropertyList Render()
        {
            return new PropertyList(_properties.Values.ToArray());
        }

    }
}
