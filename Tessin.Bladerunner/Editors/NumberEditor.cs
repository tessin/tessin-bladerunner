﻿using System;
using System.Collections.Generic;
using System.Linq;
using LINQPad.Controls;
using Tessin.Bladerunner.Controls;

namespace Tessin.Bladerunner.Editors
{
    public class NumberEditor<T> : IFieldEditor<T>
    {
        private Field _field;
        private TextBox _textBox;

        public NumberEditor()
        {
        }

        public object Render(T obj, EditorField<T> editorFieldInfo, Action preview)
        {
            _textBox = new TextBox(editorFieldInfo.GetValue(obj)?.ToString()??"") { };

            _textBox.TextInput += (sender, args) => preview();

            return _field = new Field(editorFieldInfo.Label, _textBox, editorFieldInfo.Description, editorFieldInfo.Helper);
        }

        public void Save(T obj, EditorField<T> editorFieldInfo)
        {
            var type = editorFieldInfo.Type;
            if (Nullable.GetUnderlyingType(editorFieldInfo.Type) is Type t)
            {
                if (string.IsNullOrEmpty(_textBox.Text))
                {
                    editorFieldInfo.SetValue(obj, null);
                    return;
                }
                type = t;
            }
            try
            {
                editorFieldInfo.SetValue(obj, Convert.ChangeType(double.Parse(_textBox.Text), type));
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
                _textBox.Styles["border-color"] = "tomato";
                _field.SetError(message);
            }

            if (!editorFieldInfo.Type.IsNullable() && string.IsNullOrEmpty(_textBox.Text) || !string.IsNullOrEmpty(_textBox.Text) && !double.TryParse(_textBox.Text, out double _))
            {
                SetError("Invalid number.");
                return false;
            }

            (bool, string)? error = editorFieldInfo.Validators
                .Select(e => e(Convert.ChangeType(_textBox.Text, editorFieldInfo.Type)))
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