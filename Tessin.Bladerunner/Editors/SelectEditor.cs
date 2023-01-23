using LINQPad;
using System;
using System.Linq;
using Tessin.Bladerunner.Controls;

namespace Tessin.Bladerunner.Editors
{
    public class SelectEditor<T> : IFieldEditor<T>
    {
        private Controls.SelectBox _selectBox;
        private Field _field;
        private readonly bool _nullable;

        private readonly Option[] _options;

        public SelectEditor(Option[] options, bool nullable = false)
        {
            _options = options;
            _nullable = nullable;
        }

        public void Update(object value)
        {
            throw new NotImplementedException();
        }

        public object Render(T obj, EditorField<T> editorFieldInfo, Action preview)
        {
            object value = editorFieldInfo.GetValue(obj);

            //todo: not sure this is the correct solution
            if (value.GetType().IsEnum)
            {
                value = Convert.ToInt32(value);
            }

            var options = GetOptions(editorFieldInfo);

            var selectedOption = options.Where(e => Equals(e.Value,value)).Select(e => e.Label).FirstOrDefault();

            _selectBox = new Controls.SelectBox(options.Select(e => e.Label).ToArray())
            {
                SelectedOption = selectedOption
            };

            _selectBox.SelectionChanged += (sender, args) => preview();

            _selectBox.Enabled = editorFieldInfo.Enabled;

            return _field = new Field(
                editorFieldInfo.Label,
                _selectBox,
                editorFieldInfo.Description,
                editorFieldInfo.Helper,
                editorFieldInfo.Required
            );
        }

        private Option[] GetOptions(EditorField<T> editorFieldInfo)
        {
            var isNullable = editorFieldInfo.Type.IsNullable();
            if (_nullable || isNullable)
            {
                return (new[] { new Option("", null) }).Concat(_options).ToArray();
            }
            else
            {
                return _options;
            }
        }

        public void Save(T obj, EditorField<T> editorFieldInfo)
        {
            var options = GetOptions(editorFieldInfo);
            editorFieldInfo.SetValue(obj, options[_selectBox.SelectedIndex].Value);
        }

        public bool Validate(T obj, EditorField<T> editorFieldInfo)
        {
            void SetError(string message)
            {
                _selectBox.Styles["border-color"] = "#aa0000";
                _field.SetError(message);
            }

            void ClearError()
            {
                _selectBox.Styles["border-color"] = null;
                _field.SetError("");
            }

            var options = GetOptions(editorFieldInfo);
            object value = options[_selectBox.SelectedIndex].Value;

            (bool, string)? error = editorFieldInfo.Validators
                .Select(e => e(value))
                .Where(e => !e.Item1)
                .Select(e => ((bool, string)?)e)
                .FirstOrDefault();

            if (error is { Item1: false })
            {
                SetError(error.Value.Item2);
                return false;
            }
            else
            {
                ClearError();
            }

            return true;
        }

        public void SetVisibility(bool value)
        {
            _field.SetVisibility(value);
        }
    }
}