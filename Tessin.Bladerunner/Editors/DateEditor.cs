using System;
using System.Globalization;
using System.Reflection;
using LINQPad;
using LINQPad.Controls;
using Tessin.Bladerunner.Controls;

namespace Tessin.Bladerunner.Editors
{
    public class DateEditor<T> : IFieldEditor<T>
    {
        private TextBox _textBox;

        public DateEditor()
        {
        }

        public object Render(T obj, FieldInfo fieldInfo)
        {
            object _value = fieldInfo.GetValue(obj);
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

            _textBox = new TextBox(value?.ToString("yyyy-MM-dd") ?? "") {Width = "90px"};

            var label = new FieldLabel(fieldInfo.Name);

            return Util.VerticalRun(
                label,
                _textBox
            );
        }

        public void Save(T obj, FieldInfo fieldInfo)
        {
            var type = fieldInfo.FieldType;
            if ((Nullable.GetUnderlyingType(fieldInfo.FieldType) is Type ut))
            {
                if (string.IsNullOrEmpty(_textBox.Text))
                {
                    fieldInfo.SetValue(obj, null);
                    return;
                }
                type = ut;
            }
            fieldInfo.SetValue(obj,
                type == typeof(DateTimeOffset)
                    ? DateTimeOffset.Parse(_textBox.Text)
                    : DateTime.Parse(_textBox.Text));
        }

        public bool Validate(T obj, FieldInfo fieldInfo)
        {
            if (!fieldInfo.FieldType.IsNullable() && string.IsNullOrEmpty(_textBox.Text) ||
                !string.IsNullOrEmpty(_textBox.Text) && !DateTime.TryParseExact(_textBox.Text, new[] {"yyyy-MM-dd"}, null, DateTimeStyles.None, out DateTime _))
            {
                _textBox.Styles["border-color"] = "tomato";
                return false;
            }
            return true;
        }

    }
}