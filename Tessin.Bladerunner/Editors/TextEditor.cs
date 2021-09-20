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
        private string _type;
        private bool _trim;

        public TextEditor(bool multiLine = false, bool fixedFont = false, string type = null, bool trim = true)
        {
            _multiLine = multiLine;
            _fixedFont = fixedFont;
            _type = type;
            _trim = trim;
        }

        public object Render(T obj, EditorField<T> editorFieldInfo, Action updated)
        {
            var value = Convert.ToString(editorFieldInfo.GetValue(obj));

            if (_multiLine)
            {
                _textBox = new Controls.TextArea(value);
                _textBox.Styles["width"] = "-webkit-fill-available";
                ((Controls.TextArea) _textBox).TextInput += (sender, args) => updated();
            }
            else
            {
                _textBox = _type switch
                {
                    "url" => new UrlBox(value),
                    "email" =>  new EmailBox(value),
                    _ => new Controls.TextBox(value)
                };
                ((Controls.TextBox)_textBox).TextInput += (sender, args) => updated();
            }

            if (_fixedFont)
            {
                _textBox.HtmlElement.SetAttribute("class", "fixed-font");
            }

            return _field = new Field(
                editorFieldInfo.Label, 
                _textBox, 
                editorFieldInfo.Description, 
                editorFieldInfo.Helper,
                editorFieldInfo.Required
            );
        }

        public void Save(T obj, EditorField<T> editorFieldInfo)
        {
            string value = (string)_textBox.GetType().GetProperty("Text").GetValue(_textBox);

            if(_trim)
            {
                value = value.Trim();
            }

            editorFieldInfo.SetValue(obj, value);
        }

        public bool Validate(T obj, EditorField<T> editorFieldInfo)
        {
            void SetError(string message)
            {
                _textBox.Styles["border-color"] = "darkred";
                _field.SetError(message);
            }

            void ClearError()
            {
                _textBox.Styles["border-color"] = "inherit";
                _field.SetError("");
            }

            object value = _textBox.GetType().GetProperty("Text").GetValue(_textBox);
            
            (bool, string)? error = editorFieldInfo.Validators
                .Select(e => e(value))
                .Where(e => !e.Item1)
                .Select(e => ((bool, string)?)e)
                .FirstOrDefault();

            if (error != null && !error.Value.Item1)
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