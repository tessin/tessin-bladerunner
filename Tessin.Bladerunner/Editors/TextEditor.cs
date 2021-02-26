using System;
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

        public object Render(T obj, Field<T> fieldInfo, Action updated)
        {
            var value = Convert.ToString(fieldInfo.GetValue(obj));

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

            return _field = new Field(fieldInfo.Label, _textBox, fieldInfo.Description, fieldInfo.Helper);
        }

        public void Save(T obj, Field<T> fieldInfo)
        {
            fieldInfo.SetValue(obj, _textBox.GetType().GetProperty("Text").GetValue(_textBox));
        }

        public bool Validate(T obj, Field<T> fieldInfo)
        {
            return true;
        }

        public void SetVisibility(bool value)
        {
            _field.SetVisibility(value);
        }
    }
}