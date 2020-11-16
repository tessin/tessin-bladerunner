using System;
using System.Collections.Generic;

namespace Tessin.Bladerunner.Editors
{
    public class EditorFactory<T>
    {
        
        public IFieldEditor<T> Text()
        {
            return new TextEditor<T>();
        }

        public IFieldEditor<T> Literal()
        {
            return new LiteralEditor<T>();
        }

        public IFieldEditor<T> Date()
        {
            return new DateEditor<T>();
        }

        public IFieldEditor<T> Hidden()
        {
            return new HiddenEditor<T>();
        }

        public IFieldEditor<T> Decimal()
        {
            return new DecimalEditor<T>();
        }

        public IFieldEditor<T> Link(Func<T,string> func, Action<T> onAction = null)
        {
            return new LinkEditor<T>(func, onAction);
        }

        public IFieldEditor<T> DataList(IEnumerable<Option> options)
        {
            return new DataListEditor<T>(options);
        }

    }
}