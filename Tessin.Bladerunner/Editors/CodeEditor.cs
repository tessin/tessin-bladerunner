using System;
using System.Collections.Generic;
using System.Linq;
using Tessin.Bladerunner.Controls;

namespace Tessin.Bladerunner.Editors
{
    public class CodeEditor<T> : IFieldEditor<T>
    {
        private Field _field;
        private Controls.CodeEditor _codeBox;
        private string _language;

        public CodeEditor(string language)
        {
            _language = language;
        }

        public void Update(object value)
        {
            throw new NotImplementedException();
        }

        public object Render(T obj, EditorField<T> editorFieldInfo, Action updated)
        {
            var value = Convert.ToString(editorFieldInfo.GetValue(obj));

            _codeBox = new Controls.CodeEditor(value, _language);
            _codeBox.TextInput += (sender, args) => updated();

            return _field = new Field(
                editorFieldInfo.Label,
                _codeBox,
                editorFieldInfo.Description,
                editorFieldInfo.Helper,
                editorFieldInfo.Required
            );
        }

        public void Save(T obj, EditorField<T> editorFieldInfo)
        {
            string value = (string)_codeBox.GetType().GetProperty("Text").GetValue(_codeBox);
            editorFieldInfo.SetValue(obj, value);
        }

        public bool Validate(T obj, EditorField<T> editorFieldInfo)
        {
            void SetError(string message)
            {
                _codeBox.Styles["border-color"] = "#aa0000";
                _field.SetError(message);
            }

            void ClearError()
            {
                _codeBox.Styles["border-color"] = null;
                _field.SetError("");
            }

            object value = (string)_codeBox.GetType().GetProperty("Text").GetValue(_codeBox);

            var validators = new List<Func<object, (bool, string)>>(editorFieldInfo.Validators);
            
            (bool, string)? error = validators
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