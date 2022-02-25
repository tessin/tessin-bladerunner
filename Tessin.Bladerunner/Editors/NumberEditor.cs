using System;
using System.Linq;
using Tessin.Bladerunner.Controls;

namespace Tessin.Bladerunner.Editors
{
    public class NumberEditor<T> : IFieldEditor<T>
    {
        private Field _field;
        private NumberBox _numberBox;
        private int _decimals;

        public NumberEditor(int decimals)
        {
            _decimals = decimals;
        }

        public void Update(object value)
        {
            if (_numberBox != null)
            {
                _numberBox.Text = NumberBox.FormatNumber(Convert.ToDouble(value), _decimals);
            }
        }

        public object Render(T obj, EditorField<T> editorFieldInfo, Action preview)
        {
            _numberBox = new NumberBox(Convert.ToDouble(editorFieldInfo.GetValue(obj)), _decimals) { };

            _numberBox.TextInput += (sender, args) => preview();

            _numberBox.Enabled = editorFieldInfo.Enabled;

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
                _numberBox.Styles["border-color"] = "#aa0000";
                _field.SetError(message);
            }

            void ClearError()
            {
                _numberBox.Styles["border-color"] = null;
                _field.SetError("");
            }

            double val = 0;

            if (!editorFieldInfo.Type.IsNullable() && string.IsNullOrEmpty(_numberBox.Text)
                || !string.IsNullOrEmpty(_numberBox.Text) && !double.TryParse(_numberBox.Text, out val))
            {
                SetError("Invalid number");
                return false;
            }

            (bool, string)? error = editorFieldInfo.Validators
                .Select(e => e(Convert.ChangeType(val, editorFieldInfo.Type)))
                .Where(e => e.Item1)
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