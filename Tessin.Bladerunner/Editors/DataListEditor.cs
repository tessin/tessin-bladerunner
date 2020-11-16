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

        private readonly IEnumerable<Option> _options;

        public DataListEditor(IEnumerable<Option> options)
        {
            _options = options;
        }

        public object Render(T obj, FieldInfo fieldInfo)
        {
            object value = fieldInfo.GetValue(obj);

            var labelText = _options.Where(e => e.Value.Equals(value)).Select(e => e.Label).FirstOrDefault();

            _dataListBox = new DataListBox(_options.Select(e => e.Label))
            {
                Text = labelText
            };

            var label = new FieldLabel(fieldInfo.Name);

            return Util.VerticalRun(
                label,
                _dataListBox
            );
        }

        public void Save(T obj, FieldInfo fieldInfo)
        {
            fieldInfo.SetValue(obj, _options.Where(e => e.Label == _dataListBox.Text).Select(e => e.Value).FirstOrDefault());
        }
        
    }
}