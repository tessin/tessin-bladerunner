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
        private Field _field;

        private readonly string[] _options;

        //todo: change interface to Options[]

        public DataListEditor(string[] options)
        {
            _options = options;
        }

        public object Render(T obj, EditorField<T> editorField, Action preview)
        {
            object value = editorField.GetValue(obj);

            var labelText = value?.ToString() ?? "";

            _dataListBox = new DataListBox(_options)
            {
                Text = labelText
            };

            _dataListBox.TextInput += (sender, args) => preview();

            return _field = new Field(editorField.Label, _dataListBox, editorField.Description, editorField.Helper, editorField.Required);
        }

        public void Save(T obj, EditorField<T> editorField)
        {
            editorField.SetValue(obj, _dataListBox.Text);
        }

        public bool Validate(T obj, EditorField<T> editorField)
        {
            return true;
        }

        public void SetVisibility(bool value)
        {
            _field.SetVisibility(value);
        }
    }
}