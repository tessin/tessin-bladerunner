using System;
using System.Reflection;
using LINQPad;
using LINQPad.Controls;
using Tessin.Bladerunner.Controls;

namespace Tessin.Bladerunner.Editors
{
    public class NumberEditor<T> : IFieldEditor<T>
    {
        private TextBox _textBox;

        public NumberEditor()
        {
        }

        public object Render(T obj, FieldInfo fieldInfo)
        {
            _textBox = new TextBox(fieldInfo.GetValue(obj)?.ToString()??"") {Width = "90px"};

            var label = new FieldLabel(fieldInfo.Name);

            return Util.VerticalRun(
                label,
                _textBox
            );
        }

        public void Save(T obj, FieldInfo fieldInfo)
        {
            var type = fieldInfo.FieldType;
            if (Nullable.GetUnderlyingType(fieldInfo.FieldType) is Type t)
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

        public bool Validate(T obj, FieldInfo fieldInfo)
        {
            if (!fieldInfo.FieldType.IsNullable() && string.IsNullOrEmpty(_textBox.Text) || !string.IsNullOrEmpty(_textBox.Text) && !double.TryParse(_textBox.Text, out double _))
            {
                _textBox.Styles["border-color"] = "tomato";
                return false;
            }
            return true;
        }
    }
}