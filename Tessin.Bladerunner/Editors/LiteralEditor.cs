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

        public object Render(T obj, EditorField<T> editorFieldInfo, Action preview)
        {
            var value = Convert.ToString(editorFieldInfo.GetValue(obj));

            var valueLabel = new Label(value ?? "null");

            valueLabel.HtmlElement.SetAttribute("class", "entity-editor-literal");

            return _field = new Field(editorFieldInfo.Label, valueLabel, editorFieldInfo.Description, editorFieldInfo.Helper);
        }

        public void Save(T obj, EditorField<T> editorFieldInfo)
        {
            //ignore
        }

        public bool Validate(T obj, EditorField<T> instruction)
        {
            return true;
        }

        public void SetVisibility(bool value)
        {
            _field.SetVisibility(value);
        }
    }
}
