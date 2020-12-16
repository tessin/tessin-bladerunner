using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using LINQPad;
using LINQPad.Controls;
using Tessin.Bladerunner.Controls;

namespace Tessin.Bladerunner.Editors
{
    public class LiteralEditor<T> : IFieldEditor<T>
    {

        public object Render(T obj, FieldInfo fieldInfo)
        {
            var value = Convert.ToString(fieldInfo.GetValue(obj));

            var valueLabel = new Label(value);

            var fieldLabel = new FieldLabel(fieldInfo.Name);

            return Util.VerticalRun(
                fieldLabel,
                valueLabel
            );
        }

        public void Save(T obj, FieldInfo fieldInfo)
        {
            //ignore
        }

        public bool Validate(T obj, FieldInfo fieldInfo)
        {
            return true;
        }
    }
}
