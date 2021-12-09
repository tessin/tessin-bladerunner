using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LINQPad.Controls;

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

        private bool _removeEmpty = false;

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
                    new Property(Utils.SplitCamelCase(field.Name), field.FieldInfo?.GetValue(_obj) ?? field.PropertyInfo?.GetValue(_obj));
            }
        }

        public _PropertyListBuilder<T> RemoveEmpty()
        {
            _removeEmpty = true;
            return this;
        }

        public _PropertyListBuilder<T> MultiLine(params Expression<Func<T, object>>[] fields)
        {
            foreach (var expr in fields)
            {
                var prop = GetField(expr);
                prop.IsMultiLine = true;
            }
            return this;
        }

        public PropertyList Render()
        {
            var props = _properties.Values.ToArray();

            if (_removeEmpty)
            {
                props = props.Where(e => e.Value != null || e.Value is "").ToArray();
            }

            return new PropertyList(props);
        }

        private Property GetField<TU>(Expression<Func<T, TU>> field)
        {
            var name = Utils.GetNameFromMemberExpression(field.Body);
            return _properties[name];
        }

    }
}
