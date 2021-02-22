using System;
using System.Globalization;
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

        public object Render(T obj, Field<T> field, Action preview)
        {
            object _value = field.GetValue(obj);
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

            _textBox.TextInput += (sender, args) => preview();

            var label = new FieldLabel(field.Label);

            return LINQPad.Util.VerticalRun(
                label,
                _textBox
            );
        }

        public void Save(T obj, Field<T> field)
        {
            var type = field.Type;
            if ((Nullable.GetUnderlyingType(field.Type) is Type ut))
            {
                if (string.IsNullOrEmpty(_textBox.Text))
                {
                    field.SetValue(obj, null);
                    return;
                }
                type = ut;
            }
            field.SetValue(obj,
                type == typeof(DateTimeOffset)
                    ? DateTimeOffset.Parse(_textBox.Text)
                    : DateTime.Parse(_textBox.Text));
        }

        public bool Validate(T obj, Field<T> field)
        {
            if (!field.Type.IsNullable() && string.IsNullOrEmpty(_textBox.Text) ||
                !string.IsNullOrEmpty(_textBox.Text) && !DateTime.TryParseExact(_textBox.Text, new[] {"yyyy-MM-dd"}, null, DateTimeStyles.None, out DateTime _))
            {
                _textBox.Styles["border-color"] = "tomato";
                return false;
            }
            return true;
        }

    }
}