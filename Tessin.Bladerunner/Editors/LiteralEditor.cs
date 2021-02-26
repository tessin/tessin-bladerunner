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
        private Field _field;

        public object Render(T obj, Field<T> fieldInfo, Action preview)
        {
            var value = Convert.ToString(fieldInfo.GetValue(obj));

            var valueLabel = new Label(value ?? "null");

            valueLabel.HtmlElement.SetAttribute("class", "entity-editor-literal");

            return _field = new Field(fieldInfo.Label, valueLabel, fieldInfo.Description, fieldInfo.Helper);
        }

        public void Save(T obj, Field<T> fieldInfo)
        {
            //ignore
        }

        public bool Validate(T obj, Field<T> instruction)
        {
            return true;
        }

        public void SetVisibility(bool value)
        {
            _field.SetVisibility(value);
        }
    }
}
