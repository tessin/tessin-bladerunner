using System;
using System.Reflection;
using LINQPad;
using LINQPad.Controls;

namespace Tessin.Bladerunner.Editors
{
    public class BoolEditor<T> : IFieldEditor<T>
    {
        private CheckBox _checkBox;

        public BoolEditor()
        {
        }

        public object Render(T obj, FieldInfo fieldInfo)
        {
            var value = Convert.ToBoolean(fieldInfo.GetValue(obj));

            _checkBox = new CheckBox(fieldInfo.Name, value);

            return Util.VerticalRun(
                _checkBox
            );
        }

        public void Save(T obj, FieldInfo fieldInfo)
        {
            fieldInfo.SetValue(obj, _checkBox.Checked);
        }

        public bool Validate(T obj, FieldInfo fieldInfo)
        {
            return true;
        }

    }
}