﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using LINQPad.Controls;

namespace Tessin.Bladerunner.Editors
{
    public class EditorField<T>
    {
        public EditorField(
            string name, 
            string label, 
            int order, 
            IFieldEditor<T> editor, 
            FieldInfo fieldInfo = null, 
            PropertyInfo propertyInfo = null)
        {
            Name = name;
            Label = Regex.Replace(label, "(\\B[A-Z])", " $1");
            Order = order;
            Editor = editor;
            FieldInfo = fieldInfo;
            PropertyInfo = propertyInfo;
            Column = 1;
            ShowIf = (_) => true;
        }

        public Func<Control, object> Helper { get; set; }

        public Func<T, bool> ShowIf { get; set; }

        public string Name { get; set; }

        public FieldInfo FieldInfo { get; set; }

        public PropertyInfo PropertyInfo { get; set;  }

        public Type Type => FieldInfo?.FieldType ?? PropertyInfo?.PropertyType;

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

        public int Order { get; set; }

        public int Column { get; set; }

        public string Label { get; set; }

        public string Description { get; set; }

        public IFieldEditor<T> Editor { get; set; }

        public bool Removed { get; set; }

        public List<Func<object, (bool, string)>> Validators { get; set; } = new List<Func<object, (bool, string)>>();
    }
}