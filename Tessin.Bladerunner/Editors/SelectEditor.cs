using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LINQPad;
using LINQPad.Controls;
using Tessin.Bladerunner.Controls;

namespace Tessin.Bladerunner.Editors
{
    public class SelectEditor<T> : IFieldEditor<T>
    {

        private SelectBox _selectBox;

        private readonly Option[] _options;

        public SelectEditor(Option[] options)
        {
            _options = options;
        }

        public object Render(T obj, FieldInfo fieldInfo)
        {
            object value = fieldInfo.GetValue(obj);

            var selectedOption = _options.Where(e => e.Value.Equals(value)).Select(e => e.Label).FirstOrDefault();

            _selectBox = new SelectBox(_options.Select(e => e.Label).ToArray())
            {
                SelectedOption = selectedOption
            };

            var label = new FieldLabel(fieldInfo.Name);

            return Util.VerticalRun(
                label,
                _selectBox
            );
        }

        public void Save(T obj, FieldInfo fieldInfo)
        {
            fieldInfo.SetValue(obj, _options[_selectBox.SelectedIndex].Value);
        }

        public bool Validate(T obj, FieldInfo fieldInfo)
        {
            return true;
        }
    }
}