using System.Reflection;

namespace Tessin.Bladerunner.Editors
{
    public interface IFieldEditor<T>
    {

        object Render(T obj, FieldInfo fieldInfo);

        void Save(T obj);

    }
}