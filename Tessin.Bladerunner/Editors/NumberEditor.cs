using System;
using LINQPad.Controls;
using Tessin.Bladerunner.Controls;

namespace Tessin.Bladerunner.Editors
{
    public class NumberEditor<T> : IFieldEditor<T>
    {
        private Field _field;
        private TextBox _textBox;

        public NumberEditor()
        {
        }

        public object Render(T obj, Field<T> fieldInfo, Action preview)
        {
            _textBox = new TextBox(fieldInfo.GetValue(obj)?.ToString()??"") { };

            _textBox.TextInput += (sender, args) => preview();

            return _field = new Field(fieldInfo.Label, _textBox, fieldInfo.Description, fieldInfo.Helper);
        }

        public void Save(T obj, Field<T> fieldInfo)
        {
            var type = fieldInfo.Type;
            if (Nullable.GetUnderlyingType(fieldInfo.Type) is Type t)
            {
                if (string.IsNullOrEmpty(_textBox.Text))
                {
                    fieldInfo.SetValue(obj, null);
                    return;
                }
                type = t;
            }
            fieldInfo.SetValue(obj, Convert.ChangeType(double.Parse(_textBox.Text), type));
        }

        public bool Validate(T obj, Field<T> fieldInfo)
        {
            if (!fieldInfo.Type.IsNullable() && string.IsNullOrEmpty(_textBox.Text) || !string.IsNullOrEmpty(_textBox.Text) && !double.TryParse(_textBox.Text, out double _))
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