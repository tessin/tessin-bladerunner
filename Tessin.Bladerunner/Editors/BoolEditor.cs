using System;
using System.Reflection;
using LINQPad;
using LINQPad.Controls;

namespace Tessin.Bladerunner.Editors
{
    public class BoolEditor<T> : IFieldEditor<T>
    {
        private CheckBox _checkBox;
        private DumpContainer _wrapper;

        public BoolEditor()
        {
        }

        public object Render(T obj, Field<T> field, Action preview)
        {

            var value = Convert.ToBoolean(field.GetValue(obj));

            _checkBox = new CheckBox(field.Label, value);

            _checkBox.Click += (sender, args) => preview();

            var _container = new Div(_checkBox);
            _container.SetClass("entity-editor-bool");
            
            _wrapper = new DumpContainer(_container);

            return _wrapper;
        }

        public void Save(T obj, Field<T> field)
        {
            field.SetValue(obj, _checkBox.Checked);
        }

        public bool Validate(T obj, Field<T> field)
        {
            return true;
        }

        public void SetVisibility(bool value)
        {
            _wrapper.SetVisibility(value);
        }
    }
}