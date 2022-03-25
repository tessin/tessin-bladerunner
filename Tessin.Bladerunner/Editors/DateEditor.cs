using System;
using System.Globalization;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.Serialization;
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
                value = dto.DateTime;
            }
            else if (_value is DateTime dt)
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
            if ((Nullable.GetUnderlyingType(editorField.Type) is { } ut))
            {
                if (string.IsNullOrEmpty(_dateBox.Text))
                {
                    editorField.SetValue(obj, null);
                    return;
                }
                type = ut;

                DateTime? selectedDate = _dateBox.SelectedDate;
                if (selectedDate == null)
                {
                    editorField.SetValue(obj, null);
                    return;
                }

                if (type == typeof(DateTimeOffset))
                {
                    editorField.SetValue(obj, new DateTimeOffset(selectedDate!.Value));
                }
                else
                {
                    editorField.SetValue(obj, selectedDate!.Value);
                }
                return;
            }
            try
            {
                DateTime? selectedDate = _dateBox.SelectedDate;

                if (type == typeof(DateTimeOffset))
                {
                    editorField.SetValue(obj, new DateTimeOffset(selectedDate!.Value));
                }
                else
                {
                    editorField.SetValue(obj, selectedDate!.Value);
                }
            }
            catch (Exception)
            {
                //ignore
            }
        }

        public bool Validate(T obj, EditorField<T> editorField)
        {
            void SetError(string message)
            {
                _dateBox.Styles["border-color"] = "#aa0000";
                _field.SetError(message);
            }

            void ClearError()
            {
                _dateBox.Styles["border-color"] = null;
                _field.SetError("");
            }

            DateTime val = DateTime.MinValue;

            if (!editorField.Type.IsNullable() && string.IsNullOrEmpty(_dateBox.Text) ||
                !string.IsNullOrEmpty(_dateBox.Text) && !DateTime.TryParseExact(_dateBox.Text, new[] { "yyyy-MM-dd" }, null, DateTimeStyles.None, out val))
            {
                SetError("Invalid date");
                return false;
            }

            (bool, string)? error = editorField.Validators
                .Select(e => e(val))
                .Where(e => e.Item1)
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