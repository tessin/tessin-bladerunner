using System;
using System.Collections.Generic;
using System.Globalization;
using LINQPad.Controls;
using Tessin.Bladerunner.Controls;

namespace Tessin.Bladerunner.Editors
{
    public class DateEditor<T> : IFieldEditor<T>
    {
        private TextBox _textBox;
        private Field _field;

        public DateEditor()
        {
        }

        public object Render(T obj, EditorField<T> editorField, Action preview)
        {
            object _value = editorField.GetValue(obj);
            DateTime? value;

            if (_value == null)
            {
                value = null;
            }
            else if (_value is DateTimeOffset dto)
            {
                //todo: is this safe?
                value = dto.DateTime;
            }
            else if(_value is DateTime dt)
            {
                value = dt;
            }
            else
            {
                throw new ArgumentException("Not a DateTime or DateTimeOffset.");
            }

            _textBox = new TextBox(value?.ToString("yyyy-MM-dd") ?? "") { };
            _textBox.HtmlElement.SetAttribute("placeholder", "YYYY-MM-DD");

            _textBox.TextInput += (sender, args) => preview();

            return _field = new Field(editorField.Label, _textBox, editorField.Description, editorField.Helper);
        }

        public void Save(T obj, EditorField<T> editorField)
        {
            var type = editorField.Type;
            if ((Nullable.GetUnderlyingType(editorField.Type) is Type ut))
            {
                if (string.IsNullOrEmpty(_textBox.Text))
                {
                    editorField.SetValue(obj, null);
                    return;
                }
                type = ut;
            }

            if (type == typeof(DateTimeOffset))
            {
                editorField.SetValue(obj, DateTimeOffset.Parse(_textBox.Text));
            }
            else
            {
                editorField.SetValue(obj, DateTime.Parse(_textBox.Text));
            }
        }

        public bool Validate(T obj, EditorField<T> editorField)
        {
            if (!editorField.Type.IsNullable() && string.IsNullOrEmpty(_textBox.Text) ||
                !string.IsNullOrEmpty(_textBox.Text) && !DateTime.TryParseExact(_textBox.Text, new[] {"yyyy-MM-dd"}, null, DateTimeStyles.None, out DateTime _))
            {
                _textBox.Styles["border-color"] = "tomato";
                return false;
            }
            return true;
        }

        public void SetVisibility(bool value)
        {
            _field.SetVisibility(value);
        }
    }
}