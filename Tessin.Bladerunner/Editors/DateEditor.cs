using System;
using System.Reflection;
using LINQPad;
using LINQPad.Controls;
using Tessin.Bladerunner.Controls;

namespace Tessin.Bladerunner.Editors
{
    public class DateEditor<T> : IFieldEditor<T>
    {
        private TextBox _textBox;

        public object Render(T obj, FieldInfo fieldInfo)
        {
            object _value = fieldInfo.GetValue(obj);
            DateTime value;

            if (_value is DateTimeOffset dto)
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

            _textBox = new TextBox(value.ToString("yyyy-MM-dd"));

            var label = new FieldLabel(fieldInfo.Name);

            return Util.VerticalRun(
                label,
                _textBox
            );
        }

        public void Save(T obj, FieldInfo fieldInfo)
        {
            fieldInfo.SetValue(obj,
                fieldInfo.FieldType == typeof(DateTimeOffset)
                    ? DateTimeOffset.Parse(_textBox.Text)
                    : DateTime.Parse(_textBox.Text));
        }
    }
}