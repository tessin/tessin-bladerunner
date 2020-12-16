using System;
using System.Reflection;
using LINQPad;
using LINQPad.Controls;
using Tessin.Bladerunner.Controls;

namespace Tessin.Bladerunner.Editors
{
    public class LinkEditor<T> : IFieldEditor<T>
    {

        private Action<T> _onClick;

        private readonly Func<T, string> _fetchValue;

        public LinkEditor(Func<T, string> fetchValue, Action<T> onClick)
        {
            _onClick = onClick;
            _fetchValue = fetchValue;
        }

        public LinkEditor(Func<T, string> fetchValue)
        {
            _fetchValue = fetchValue;
        }

        public LinkEditor(Action<T> onClick)
        {
            _onClick = onClick;
        }

        public object Render(T obj, FieldInfo fieldInfo)
        {
            var fieldLabel = new FieldLabel(fieldInfo.Name);

            var value = _fetchValue(obj);

            var link = _onClick == null
                ? new Label(value) as Control
                : new Hyperlink(value, (_) => _onClick(obj)) as Control;

            return Util.VerticalRun(
                fieldLabel,
                link
            );
        }

        public void Save(T obj, FieldInfo fieldInfo)
        {
            //ignore
        }

        public bool Validate(T obj, FieldInfo fieldInfo)
        {
            return true;
        }
    }
}