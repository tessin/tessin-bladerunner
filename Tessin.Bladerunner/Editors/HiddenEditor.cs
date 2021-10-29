using System;
using System.Collections.Generic;
using System.Reflection;

namespace Tessin.Bladerunner.Editors
{
    public class HiddenEditor<T> : IFieldEditor<T>
    {
        public object Render(T obj, EditorField<T> editorFieldInfo, Action preview)
        {
            return null;
        }

        public void Update(object value)
        {

        }

        public void Save(T obj, EditorField<T> editorFieldInfo)
        {
            // ignore
        }

        public bool Validate(T obj, EditorField<T> editorFieldInfo)
        {
            return true;
        }

        public void SetVisibility(bool value)
        {
            // ignore
        }
    }
}