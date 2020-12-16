using System;
using System.Collections.Generic;

namespace Tessin.Bladerunner.Editors
{
    public class EditorFactory<T>
    {
        
        public IFieldEditor<T> Text(bool isMultiLine = false)
        {
            return new TextEditor<T>(isMultiLine);
        }

        public IFieldEditor<T> Literal()
        {
            return new LiteralEditor<T>();
        }

        public IFieldEditor<T> Date()
        {
            return new DateEditor<T>();
        }

        public IFieldEditor<T> Bool()
        {
            return new BoolEditor<T>();
        }

        public IFieldEditor<T> Hidden()
        {
            return new HiddenEditor<T>();
        }

        public IFieldEditor<T> Number()
        {
            return new NumberEditor<T>();
        }

        public IFieldEditor<T> Link(Func<T,string> func, Action<T> onAction = null)
        {
            return new LinkEditor<T>(func, onAction);
        }

        public IFieldEditor<T> DataList(Option[] options)
        {
            return new DataListEditor<T>(options);
        }

        public IFieldEditor<T> Select(Option[] options)
        {
            return new SelectEditor<T>(options);
        }

    }
}