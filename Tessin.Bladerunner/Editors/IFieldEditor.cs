using System;
using System.Reflection;

namespace Tessin.Bladerunner.Editors
{
    public interface IFieldEditor<T>
    {

        object Render(T obj, Field<T> instruction, Action updated);

        void Save(T obj, Field<T> instruction);

        bool Validate(T obj, Field<T> instruction);

    }
}