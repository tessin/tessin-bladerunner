using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Tessin.Bladerunner.Controls;

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

        public IFieldEditor<T> Number(int decimals = 0)
        {
            return new NumberEditor<T>(decimals);
        }

        public IFieldEditor<T> Link(Func<T, string> fetchUrl)
        {
            return new LinkEditor<T>(fetchUrl);
        }

        public IFieldEditor<T> Link(Action<T> onAction)
        {
            return new LinkEditor<T>(onAction);
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

        public IFieldEditor<T> File(string initialCatalog = null)
        {
            return new FileEditor<T>(initialCatalog);
        }

        public IFieldEditor<T> ImageProxy(ImageProxySettings settings, string initialCatalog = null)
        {
            return new ImageProxyEditor<T>(settings, initialCatalog);
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

        public IFieldEditor<T> Select(IEnumerable<Option> options, bool nullable = false)
        {
            return new SelectEditor<T>(options.ToArray(), nullable);
        }

        public IFieldEditor<T> Select(Type enumType, Type idType = null)
        {
            if (!enumType.IsEnum) throw new ArgumentException("Not an enum.");
            
            idType ??= typeof(string);

            Func<int, string, string, Option> optionBuilder = null;
            
            if (idType == typeof(int))
            {
                optionBuilder = (id, name, desc) => new Option(desc ?? name, id);
            }
            else if(idType == typeof(string))
            {
                optionBuilder = (_, name, desc) => new Option(desc ?? name, name);
            }
            else
            {
                throw new Exception();
            }

            var enumValues = enumType.GetEnumValues().Cast<object>()
                .Select(e => Convert.ChangeType(e, enumType)).Select(e => ((int)e, e.ToString()));

            var options = enumValues.Zip(
                enumType.GetFields().Where(e => e.IsStatic).Select(field =>
                    Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute
                        ? attribute.Description
                        : null
                ),
                (x, desc) => optionBuilder(x.Item1, x.Item2, desc)
            );

            return new SelectEditor<T>(options.ToArray());
        }
    }
}