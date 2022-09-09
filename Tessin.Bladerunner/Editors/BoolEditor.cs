using LINQPad;
using LINQPad.Controls;
using System;
using System.Runtime.InteropServices.ComTypes;
using Tessin.Bladerunner.Controls;
using CheckBox = LINQPad.Controls.CheckBox;

namespace Tessin.Bladerunner.Editors
{
    public class BoolEditor<T> : IFieldEditor<T>
    {
        private Control _checkBox;
        private DumpContainer _wrapper;
        private bool _isNullable;
        
        public BoolEditor()
        {
        }

        public void Update(object value)
        {
            if (_checkBox is CheckBox checkBox)
            {
                checkBox.Checked = Convert.ToBoolean(value);
            }
            else if (_checkBox is TriStateCheckBox triStateCheckBox)
            {
                triStateCheckBox.Checked = (bool?)value;
            }
        }

        public object Render(T obj, EditorField<T> editorField, Action preview)
        {
            var val = editorField.GetValue(obj);

            _isNullable = editorField.Type.IsNullable();
            
            if (_isNullable)
            {
                _checkBox = new TriStateCheckBox(editorField.Label, (bool?)val);
                _checkBox.Click += (_, _) => preview();
            }
            else
            {
                _checkBox = new CheckBox(editorField.Label, Convert.ToBoolean(val));
                _checkBox.Click += (_, _) => preview();
            }
            
            var container = new Div(_checkBox);
            container.SetClass("entity-editor-bool");

            _wrapper = new DumpContainer(container);

            _checkBox.Enabled = editorField.Enabled;

            return _wrapper;
        }

        public void Save(T obj, EditorField<T> editorField)
        {
            object val = _checkBox switch
            {
                CheckBox checkBox => checkBox.Checked,
                TriStateCheckBox triStateCheckBox => triStateCheckBox.Checked,
                _ => throw new ArgumentOutOfRangeException()
            };
            editorField.SetValue(obj, val);
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