using System;
using System.Collections.Generic;
using System.Globalization;
using LINQPad.Controls;
using Tessin.Bladerunner.Controls;

namespace Tessin.Bladerunner.Editors
{
    public class DateEditor<T> : IFieldEditor<T>
    {
        private DateBox _dateBox;
        private Field _field;

        public DateEditor()
        {
        }

        public void Update(object value)
        {
            throw new NotImplementedException();
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

            _dateBox = new DateBox(value) { };
            //_dateBox.HtmlElement.SetAttribute("placeholder", "YYYY-MM-DD");

            _dateBox.TextInput += (sender, args) => preview();

            _dateBox.Enabled = editorField.Enabled;

            return _field = new Field(editorField.Label, _dateBox, editorField.Description, editorField.Helper, editorField.Required);
        }

        public void Save(T obj, EditorField<T> editorField)
        {
            var type = editorField.Type;
            if ((Nullable.GetUnderlyingType(editorField.Type) is Type ut))
            {
                if (string.IsNullOrEmpty(_dateBox.Text))
                {
                    editorField.SetValue(obj, null);
                    return;
                }
                type = ut;
            }

            if (type == typeof(DateTimeOffset))
            {
                editorField.SetValue(obj, DateTimeOffset.Parse(_dateBox.Text));
            }
            else
            {
                editorField.SetValue(obj, DateTime.Parse(_dateBox.Text));
            }
        }

        public bool Validate(T obj, EditorField<T> editorField)
        {
            if (!editorField.Type.IsNullable() && string.IsNullOrEmpty(_dateBox.Text) ||
                !string.IsNullOrEmpty(_dateBox.Text) && !DateTime.TryParseExact(_dateBox.Text, new[] {"yyyy-MM-dd"}, null, DateTimeStyles.None, out DateTime _))
            {
                _dateBox.Styles["border-color"] = "tomato";
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