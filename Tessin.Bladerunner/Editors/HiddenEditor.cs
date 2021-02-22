using System;
using System.Reflection;

namespace Tessin.Bladerunner.Editors
{
    public class HiddenEditor<T> : IFieldEditor<T>
    {
        public object Render(T obj, Field<T> fieldInfo, Action preview)
        {
            return null;
        }

        public void Save(T obj, Field<T> fieldInfo)
        {
            // ignore
        }

        public bool Validate(T obj, Field<T> fieldInfo)
        {
            return true;
        }
    }
}