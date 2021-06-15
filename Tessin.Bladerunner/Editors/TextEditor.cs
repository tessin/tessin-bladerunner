using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using LINQPad;
using LINQPad.Controls;
using Tessin.Bladerunner.Controls;

namespace Tessin.Bladerunner.Editors
{
    public class TextEditor<T> : IFieldEditor<T>
    {
        private Field _field;
        private Control _textBox;
        private readonly bool _multiLine;
        private readonly bool _fixedFont;

        public TextEditor(bool multiLine = false, bool fixedFont = false)
        {
            _multiLine = multiLine;
            _fixedFont = fixedFont;
        }

        public object Render(T obj, EditorField<T> editorFieldInfo, Action updated)
        {
            var value = Convert.ToString(editorFieldInfo.GetValue(obj));

            if (_multiLine)
            {
                _textBox = new TextArea(value);
                _textBox.SetClass("entity-editor-textarea");
                ((TextArea) _textBox).TextInput += (sender, args) => updated();
            }
            else
            {
                _textBox = new TextBox(value);
                ((TextBox)_textBox).TextInput += (sender, args) => updated();
            }

            if (_fixedFont)
            {
                _textBox.HtmlElement.SetAttribute("class", "fixed-font");
            }

            return _field = new Field(editorFieldInfo.Label, _textBox, editorFieldInfo.Description, editorFieldInfo.Helper);
        }

        public void Save(T obj, EditorField<T> editorFieldInfo)
        {
            editorFieldInfo.SetValue(obj, _textBox.GetType().GetProperty("Text").GetValue(_textBox));
        }

        public bool Validate(T obj, EditorField<T> editorFieldInfo)
        {
            void SetError(string message)
            {
                _textBox.Styles["border-color"] = "tomato";
                _field.SetError(message);
            }

            object value = _textBox.GetType().GetProperty("Text").GetValue(_textBox);
            
            (bool, string)? error = editorFieldInfo.Validators
                .Select(e => e(value))
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