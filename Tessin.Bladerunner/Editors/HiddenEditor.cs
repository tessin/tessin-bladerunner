using System.Reflection;

namespace Tessin.Bladerunner.Editors
{
    public class HiddenEditor<T> : IFieldEditor<T>
    {
        public object Render(T obj, FieldInfo fieldInfo)
        {
            return null;
        }

        public void Save(T obj, FieldInfo fieldInfo)
        {
            // ignore
        }

        public bool Validate(T obj, FieldInfo fieldInfo)
        {
            return true;
        }
    }
}