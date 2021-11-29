using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using LINQPad;
using LINQPad.Controls;
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
            //todo:
            //void SetError(string message)
            //{
            //    _fileBox.Styles["border-color"] = "darkred";
            //    _field.SetError(message);
            //}

            //void ClearError()
            //{
            //    _fileBox.Styles["border-color"] = "inherit";
            //    _field.SetError("");
            //}

            //object value = _fileBox.GetType().GetProperty("Text").GetValue(_fileBox);

            //(bool, string)? error = editorFieldInfo.Validators
            //    .Select(e => e(value))
            //    .Where(e => !e.Item1)
            //    .Select(e => ((bool, string)?)e)
            //    .FirstOrDefault();

            //if (error != null && !error.Value.Item1)
            //{
            //    SetError(error.Value.Item2);
            //    return false;
            //}
            //else
            //{
            //    ClearError();
            //}

            return true;
        }

        public void SetVisibility(bool value)
        {
            _field.SetVisibility(value);
        }
    }
}