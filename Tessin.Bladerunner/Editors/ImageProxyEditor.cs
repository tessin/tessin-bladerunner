using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tessin.Bladerunner.Controls;

namespace Tessin.Bladerunner.Editors
{
    public class ImageProxyEditor<T> : IFieldEditor<T>
    {
        ImageProxySettings _settings;
        private readonly string _initialCatalog;
        private Field _field;
        private ImageProxyBox _imageProxyBox;

        public ImageProxyEditor(ImageProxySettings settings, string initialCatalog = null)
        {
            _settings = settings;
            _initialCatalog = initialCatalog;
        }

        public object Render(T obj, EditorField<T> editorFieldInfo, Action updated)
        {
            var value = Convert.ToString(editorFieldInfo.GetValue(obj));

            _imageProxyBox = new ImageProxyBox(_settings, value, _initialCatalog);
            _imageProxyBox.TextInput += (sender, args) => updated();

            if (editorFieldInfo.Required)
            {
                _imageProxyBox.TextBox.HtmlElement.SetAttribute("required", "required");
            }

            return _field = new Field(
                editorFieldInfo.Label,
                _imageProxyBox,
                editorFieldInfo.Description,
                editorFieldInfo.Helper,
                editorFieldInfo.Required
            );
        }

        public void Save(T obj, EditorField<T> editorFieldInfo)
        {
            string value = (string)_imageProxyBox.GetType().GetProperty("Text").GetValue(_imageProxyBox);
            editorFieldInfo.SetValue(obj, value);
        }

        public void SetVisibility(bool value)
        {
            _field.SetVisibility(value);
        }

        public void Update(object value)
        {
            throw new NotImplementedException();
        }

        public bool Validate(T obj, EditorField<T> editorFieldInfo)
        {
            void SetError(string message)
            {
                //_imageProxyBox.TextBox.Styles["border-color"] = "#aa0000";
                _field.SetError(message);
            }

            void ClearError()
            {
                //_imageProxyBox.TextBox.Styles["border-color"] = null;
                _field.SetError("");
            }

            if (_imageProxyBox.IsUploading)
            {
                return false;
            }

            object value = _imageProxyBox.GetType().GetProperty("Text").GetValue(_imageProxyBox);

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

            ClearError();

            return true;
        }
    }
}
