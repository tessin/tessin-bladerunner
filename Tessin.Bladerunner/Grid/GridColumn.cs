using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using LINQPad;
using LINQPad.Controls;

namespace Tessin.Bladerunner.Grid
{
    public class GridColumn<T>
    {
        public GridColumn(
            string name,
            string label,
            int order,
            ICellRenderer<T> cellRenderer,
            CellAlignment cellAlignment = CellAlignment.Left,
            FieldInfo fieldInfo = null,
            PropertyInfo propertyInfo = null,
            bool removed = false)
        {
            Name = name;
            Label = Utils.SplitCamelCase(name);
            Order = order;
            CellRenderer = cellRenderer;
            CellAlignment = cellAlignment;
            FieldInfo = fieldInfo;
            PropertyInfo = propertyInfo;
            Removed = removed;
        }

        public string Name { get; set; }

        public FieldInfo FieldInfo { get; set; }

        public PropertyInfo PropertyInfo { get; set; }

        public ICellRenderer<T> CellRenderer { get; set; }

        public Type Type => FieldInfo?.FieldType ?? PropertyInfo?.PropertyType;

        public int Order { get; set; }

        public string Label { get; set; }

        public string Description { get; set; }

        public bool Removed { get; set; }

        public string Width { get; set; } = "0%";

        public CellAlignment CellAlignment { get; set;  }

        public Func<IEnumerable<T>,object> SummaryMethod { get; set; } 

        public object GetValue(T obj)
        {
            if (FieldInfo != null)
            {
                return FieldInfo.GetValue(obj);
            }
            return PropertyInfo.GetValue(obj);
        }

        public void SetValue(T obj, object value)
        {
            if (FieldInfo != null)
            {
                FieldInfo.SetValue(obj, value);
            }
            else
            {
                PropertyInfo.SetValue(obj, value);
            }
        }
    }

    public enum CellAlignment
    {
        Left,
        Center,
        Right
    }

}
