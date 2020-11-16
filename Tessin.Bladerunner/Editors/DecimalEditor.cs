using System;
using System.Reflection;
using LINQPad;
using LINQPad.Controls;
using Tessin.Bladerunner.Controls;

namespace Tessin.Bladerunner.Editors
{
    public class DecimalEditor<T> : IFieldEditor<T>
    {
        private TextBox _textBox;

        public DecimalEditor()
        {
        }

        public object Render(T obj, FieldInfo fieldInfo)
        {
            var value = Convert.ToDecimal(fieldInfo.GetValue(obj));

            _textBox = new TextBox(value.ToString());

            var label = new FieldLabel(fieldInfo.Name);

            return Util.VerticalRun(
                label,
                _textBox
            );
        }

        public void Save(T obj)
        {
            
        }
    }
}