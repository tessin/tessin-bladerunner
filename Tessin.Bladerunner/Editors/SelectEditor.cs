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
        private Controls.SelectBox _selectBox;
        private Field _field;

        private readonly Option[] _options;

        public SelectEditor(Option[] options)
        {
            _options = options;
        }

        public void Update(object value)
        {
            throw new NotImplementedException();
        }

        public object Render(T obj, EditorField<T> editorFieldInfo, Action preview)
        {
            object value = editorFieldInfo.GetValue(obj);

            var selectedOption = _options.Where(e => e.Value.Equals(value)).Select(e => e.Label).FirstOrDefault();

            _selectBox = new Controls.SelectBox(_options.Select(e => e.Label).ToArray())
            {
                SelectedOption = selectedOption
            };

            _selectBox.SelectionChanged += (sender, args) => preview();

            _selectBox.Enabled = editorFieldInfo.Enabled;

            return _field = new Field(editorFieldInfo.Label, _selectBox, editorFieldInfo.Description, editorFieldInfo.Helper, editorFieldInfo.Required);
        }

        public void Save(T obj, EditorField<T> editorFieldInfo)
        {
            editorFieldInfo.SetValue(obj, _options[_selectBox.SelectedIndex].Value);
        }

        public bool Validate(T obj, EditorField<T> editorFieldInfo)
        {
            return true;
        }

        public void SetVisibility(bool value)
        {
            _field.SetVisibility(value);
        }
    }
}