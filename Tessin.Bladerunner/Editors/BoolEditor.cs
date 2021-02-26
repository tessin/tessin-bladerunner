using System;
using System.Reflection;
using LINQPad;
using LINQPad.Controls;

namespace Tessin.Bladerunner.Editors
{
    public class BoolEditor<T> : IFieldEditor<T>
    {
        private CheckBox _checkBox;

        public BoolEditor()
        {
        }

        public object Render(T obj, Field<T> field, Action preview)
        {
            var value = Convert.ToBoolean(field.GetValue(obj));

            _checkBox = new CheckBox(field.Label, value);

            _checkBox.Click += (sender, args) => preview();

            return _checkBox;
        }

        public void Save(T obj, Field<T> field)
        {
            field.SetValue(obj, _checkBox.Checked);
        }

        public bool Validate(T obj, Field<T> field)
        {
            return true;
        }

        public void SetVisibility(bool value)
        {
            _checkBox.SetVisibility(value);
        }
    }
}