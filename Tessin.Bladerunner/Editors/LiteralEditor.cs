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

        public object Render(T obj, Field<T> fieldInfo, Action preview)
        {
            var value = Convert.ToString(fieldInfo.GetValue(obj));

            var valueLabel = new Label(value ?? "null");

            valueLabel.HtmlElement.SetAttribute("class", "entity-editor-literal");

            var fieldLabel = new FieldLabel(fieldInfo.Label);

            return LINQPad.Util.VerticalRun(
                fieldLabel,
                valueLabel
            );
        }

        public void Save(T obj, Field<T> fieldInfo)
        {
            //ignore
        }

        public bool Validate(T obj, Field<T> fieldInfo)
        {
            return true;
        }
    }
}
