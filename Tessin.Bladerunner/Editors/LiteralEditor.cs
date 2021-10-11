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

        private IContentFormatter _contentFormatter = new DefaultContentFormatter();

        public object Render(T obj, EditorField<T> editorFieldInfo, Action preview)
        {
            Control valueControl = _contentFormatter.Format(editorFieldInfo.GetValue(obj), emptyContent:"-");

            return _field = new Field(editorFieldInfo.Label, valueControl, editorFieldInfo.Description, editorFieldInfo.Helper);
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
