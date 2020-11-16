using System;
using System.Reflection;
using LINQPad;
using LINQPad.Controls;
using Tessin.Bladerunner.Controls;

namespace Tessin.Bladerunner.Editors
{
    public class TextEditor<T> : IFieldEditor<T>
    {
        private TextBox _textBox;

        public TextEditor()
        {
        }

        public object Render(T obj, FieldInfo fieldInfo)
        {
            var value = Convert.ToString(fieldInfo.GetValue(obj));

            _textBox = new TextBox(value);

            var label = new FieldLabel(fieldInfo.Name);

            return Util.VerticalRun(
                label,
                _textBox
            );
        }

        public void Save(T obj, FieldInfo fieldInfo)
        {
            fieldInfo.SetValue(obj, _textBox.Text);
        }
    }
}