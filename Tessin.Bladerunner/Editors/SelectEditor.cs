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

            var options = GetOptions(editorFieldInfo);

            var selectedOption = options.Where(e => e.Value ==  value).Select(e => e.Label).FirstOrDefault();

            _selectBox = new Controls.SelectBox(options.Select(e => e.Label).ToArray())
            {
                SelectedOption = selectedOption
            };

            _selectBox.SelectionChanged += (sender, args) => preview();

            _selectBox.Enabled = editorFieldInfo.Enabled;

            return _field = new Field(editorFieldInfo.Label, _selectBox, editorFieldInfo.Description, editorFieldInfo.Helper, editorFieldInfo.Required);
        }

        private Option[] GetOptions(EditorField<T> editorFieldInfo)
        {
            var isNullable = editorFieldInfo.Type.IsNullable();
            if (isNullable)
            {
                return (new[] { new Option("", null) }).Concat(_options).ToArray();
            }
            else
            {
                return _options;
            }
        }

        public void Save(T obj, EditorField<T> editorFieldInfo)
        {
            var options = GetOptions(editorFieldInfo);
            editorFieldInfo.SetValue(obj, options[_selectBox.SelectedIndex].Value);
        }

        public bool Validate(T obj, EditorField<T> editorFieldInfo)
        {
            void SetError(string message)
            {
                _selectBox.Styles["border-color"] = "darkred";
                _field.SetError(message);
            }

            void ClearError()
            {
                _selectBox.Styles["border-color"] = "inherit";
                _field.SetError("");
            }
            
            var options = GetOptions(editorFieldInfo);
            object value = options[_selectBox.SelectedIndex].Value;

            (bool, string)? error = editorFieldInfo.Validators
                .Select(e => e(value))
                .Where(e => !e.Item1)
                .Select(e => ((bool, string)?)e)
                .FirstOrDefault();

            if (error is { Item1: false })
            {
                SetError(error.Value.Item2);
                return false;
            }
            else
            {
                ClearError();
            }

            return true;
        }

        public void SetVisibility(bool value)
        {
            _field.SetVisibility(value);
        }
    }
}