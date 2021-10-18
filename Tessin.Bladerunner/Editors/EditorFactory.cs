using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Tessin.Bladerunner.Editors
{
    public class EditorFactory<T>
    {
        
        public IFieldEditor<T> Text(bool multiLine = false, bool fixedFont = false)
        {
            return new TextEditor<T>(multiLine, fixedFont);
        }

        public IFieldEditor<T> Code(string language)
        {
            return new CodeEditor<T>(language);
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

        //public IFieldEditor<T> Hidden()
        //{
        //    return new HiddenEditor<T>();
        //}

        public IFieldEditor<T> Number()
        {
            return new NumberEditor<T>();
        }

        public IFieldEditor<T> Link(Func<T,string> fetchUrl)
        {
            return new LinkEditor<T>(fetchUrl);
        }

        public IFieldEditor<T> Link(Action<T> onAction)
        {
            return new LinkEditor<T>( onAction);
        }

        public IFieldEditor<T> Link()
        {
            return new LinkEditor<T>();
        }

        public IFieldEditor<T> Url()
        {
            return new TextEditor<T>(type: "url");
        }

        public IFieldEditor<T> Email()
        {
            return new TextEditor<T>(type: "email");
        }

        public IFieldEditor<T> AsyncDataList(Func<string, Task<IEnumerable<Option>>> queryOptions, Func<object, Task<Option>> findOption = null)
        {
            return new AsyncDataListEditor<T>(queryOptions, findOption);
        }

        public IFieldEditor<T> DataList(IEnumerable<string> options)
        {
            return new DataListEditor<T>(options.ToArray());
        }

        public IFieldEditor<T> MultiSelect(IEnumerable<Option> options)
        {
            return new MultiSelectEditor<T>(options.ToArray());
        }

        public IFieldEditor<T> Select(IEnumerable<Option> options)
        {
            return new SelectEditor<T>(options.ToArray());
        }

        public IFieldEditor<T> Select(Type enumType)
        {
            if (!enumType.IsEnum) throw new ArgumentException("Not an enum.");

            var options = enumType.GetEnumNames().Zip(
                enumType.GetFields().Where(e => e.IsStatic).Select(field =>
                    Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute
                        ? attribute.Description
                        : null
                ),
                (name, descr) => new Option(descr??name, name));

            return new SelectEditor<T>(options.ToArray());
        }

    }
}