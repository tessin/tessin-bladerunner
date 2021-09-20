using System;
using System.Collections.Generic;
using System.Linq;
using LINQPad.Controls;
using Tessin.Bladerunner.Controls;

namespace Tessin.Bladerunner.Editors
{
    public class NumberEditor<T> : IFieldEditor<T>
    {
        private Field _field;
        private NumberBox _numberBox;

        public NumberEditor()
        {
        }

        public object Render(T obj, EditorField<T> editorFieldInfo, Action preview)
        {
            _numberBox = new NumberBox(Convert.ToDouble(editorFieldInfo.GetValue(obj))) { };

            _numberBox.TextInput += (sender, args) => preview();

            return _field = new Field(editorFieldInfo.Label, _numberBox, editorFieldInfo.Description, editorFieldInfo.Helper, editorFieldInfo.Required);
        }

        public void Save(T obj, EditorField<T> editorFieldInfo)
        {
            var type = editorFieldInfo.Type;
            if (Nullable.GetUnderlyingType(editorFieldInfo.Type) is Type t)
            {
                if (string.IsNullOrEmpty(_numberBox.Text))
                {
                    editorFieldInfo.SetValue(obj, null);
                    return;
                }
                type = t;
            }
            try
            {
                editorFieldInfo.SetValue(obj, Convert.ChangeType(double.Parse(_numberBox.Text), type));
            }
            catch (Exception)
            {
                editorFieldInfo.SetValue(obj, Convert.ChangeType(0, type));
            }
        }

        public bool Validate(T obj, EditorField<T> editorFieldInfo)
        {
            void SetError(string message)
            {
                _numberBox.Styles["border-color"] = "tomato";
                _field.SetError(message);
            }

            if (!editorFieldInfo.Type.IsNullable() && string.IsNullOrEmpty(_numberBox.Text) || !string.IsNullOrEmpty(_numberBox.Text) && !double.TryParse(_numberBox.Text, out double _))
            {
                SetError("Invalid number.");
                return false;
            }

            (bool, string)? error = editorFieldInfo.Validators
                .Select(e => e(Convert.ChangeType(_numberBox.Text, editorFieldInfo.Type)))
                .Where(e => e.Item1)
                .Select(e => ((bool, string)?)e)
                .FirstOrDefault();

            if (error != null)
            {
                SetError(error.Value.Item2);
                return false;
            }

            return true;
        }

        public void SetVisibility(bool value)
        {
            _field.SetVisibility(value);
        }
    }
}