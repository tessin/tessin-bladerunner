using System;
using System.Collections.Generic;
using System.Reflection;

namespace Tessin.Bladerunner.Editors
{
    public interface IFieldEditor<T>
    {

        object Render(T obj, EditorField<T> instruction, Action updated);

        void Save(T obj, EditorField<T> instruction);

        void Update(object value);

        bool Validate(T obj, EditorField<T> instruction);

        void SetVisibility(bool value);

    }
}