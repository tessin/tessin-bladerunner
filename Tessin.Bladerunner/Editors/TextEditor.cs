using System;
using System.Net.Mime;
using System.Reflection;
using LINQPad;
using LINQPad.Controls;
using Tessin.Bladerunner.Controls;

namespace Tessin.Bladerunner.Editors
{
    public class TextEditor<T> : IFieldEditor<T>
    {
        private Control _textBox;

        private readonly bool _isMultiLine;

        public TextEditor(bool isMultiLine = false)
        {
            _isMultiLine = isMultiLine;
        }

        public object Render(T obj, FieldInfo fieldInfo)
        {
            var value = Convert.ToString(fieldInfo.GetValue(obj));

            if (_isMultiLine)
            {
                _textBox = new TextArea(value, columns: 40);
            }
            else
            {
                _textBox = new TextBox(value);
            }

            var label = new FieldLabel(fieldInfo.Name);

            return Util.VerticalRun(
                label,
                _textBox
            );
        }

        public void Save(T obj, FieldInfo fieldInfo)
        {
            fieldInfo.SetValue(obj, _textBox.GetType().GetProperty("Text").GetValue(_textBox));
        }

        public bool Validate(T obj, FieldInfo fieldInfo)
        {
            return true;
        }
    }
}