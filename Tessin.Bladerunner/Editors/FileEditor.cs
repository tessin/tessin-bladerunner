using LINQPad;
using System;
using System.Linq;
using Tessin.Bladerunner.Controls;

namespace Tessin.Bladerunner.Editors
{
    public class FileEditor<T> : IFieldEditor<T>
    {
        private readonly string _initialCatalog;
        private Field _field;
        private FileBox _fileBox;

        public FileEditor(string initialCatalog = null)
        {
            _initialCatalog = initialCatalog;
        }

        public void Update(object value)
        {
            throw new NotImplementedException();
        }

        public object Render(T obj, EditorField<T> editorFieldInfo, Action updated)
        {
            var value = Convert.ToString(editorFieldInfo.GetValue(obj));

            _fileBox = new FileBox(value, _initialCatalog);
            _fileBox.TextInput += (sender, args) => updated();

            if (editorFieldInfo.Required)
            {
                _fileBox.TextBox.HtmlElement.SetAttribute("required", "required");
            }

            return _field = new Field(
                editorFieldInfo.Label,
                _fileBox,
                editorFieldInfo.Description,
                editorFieldInfo.Helper,
                editorFieldInfo.Required
            );
        }

        public void Save(T obj, EditorField<T> editorFieldInfo)
        {
            string value = (string)_fileBox.GetType().GetProperty("Text").GetValue(_fileBox);
            editorFieldInfo.SetValue(obj, value);
        }

        public bool Validate(T obj, EditorField<T> editorFieldInfo)
        {
            void SetError(string message)
            {
                _fileBox.TextBox.Styles["border-color"] = "#aa0000";
                _field.SetError(message);
            }

            void ClearError()
            {
                _fileBox.TextBox.Styles["border-color"] = null;
                _field.SetError("");
            }

            object value = _fileBox.GetType().GetProperty("Text").GetValue(_fileBox);

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