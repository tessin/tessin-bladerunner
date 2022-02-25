using System;
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
            return true;
        }

        public void SetVisibility(bool value)
        {
            _field.SetVisibility(value);
        }
    }
}