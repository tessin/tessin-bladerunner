using System;
using System.Collections.Generic;
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

        public object Render(T obj, EditorField<T> editorField, Action preview)
        {
            var value = Convert.ToBoolean(editorField.GetValue(obj));

            _checkBox = new CheckBox(editorField.Label, value);

            _checkBox.Click += (sender, args) => preview();

            var _container = new Div(_checkBox);
            _container.SetClass("entity-editor-bool");
            
            _wrapper = new DumpContainer(_container);

            return _wrapper;
        }

        public void Save(T obj, EditorField<T> editorField)
        {
            editorField.SetValue(obj, _checkBox.Checked);
        }

        public bool Validate(T obj, EditorField<T> editorField)
        {
            return true;
        }

        public void SetVisibility(bool value)
        {
            _wrapper.SetVisibility(value);
        }
    }
}