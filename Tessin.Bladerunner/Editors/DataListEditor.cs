using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LINQPad;
using LINQPad.Controls;
using Tessin.Bladerunner.Controls;

namespace Tessin.Bladerunner.Editors
{
    public class DataListEditor<T> : IFieldEditor<T>
    {

        private DataListBox _dataListBox;

        private readonly string[] _options;

        public DataListEditor(string[] options)
        {
            _options = options;
        }

        public object Render(T obj, Field<T> field, Action preview)
        {
            object value = field.GetValue(obj);

            var labelText = value?.ToString() ?? "";

            _dataListBox = new DataListBox(_options)
            {
                Text = labelText
            };

            _dataListBox.TextInput += (sender, args) => preview();

            var label = new FieldLabel(field.Label);

            return LINQPad.Util.VerticalRun(
                label,
                _dataListBox
            );
        }

        public void Save(T obj, Field<T> field)
        {
            field.SetValue(obj, _dataListBox.Text);
        }

        public bool Validate(T obj, Field<T> field)
        {
            return true;
        }
    }
}