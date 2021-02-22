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
        private Control _textBox;

        private readonly bool _multiLine;

        private readonly bool _fixedFont;

        public TextEditor(bool multiLine = false, bool fixedFont = false)
        {
            _multiLine = multiLine;
            _fixedFont = fixedFont;
        }

        public object Render(T obj, Field<T> fieldInfo, Action preview)
        {
            var value = Convert.ToString(fieldInfo.GetValue(obj));

            if (_multiLine)
            {
                _textBox = new TextArea(value);
                _textBox.HtmlElement.SetAttribute("class", "entity-editor-textarea");

                ((TextArea) _textBox).TextInput += (sender, args) => preview();
            }
            else
            {
                _textBox = new TextBox(value);

                ((TextBox)_textBox).TextInput += (sender, args) => preview();
            }

            if (_fixedFont)
            {
                _textBox.HtmlElement.SetAttribute("class", "fixed-font");
            }

            var label = new FieldLabel(fieldInfo.Label);

            return LINQPad.Util.VerticalRun(
                label,
                _textBox
            );
        }

        public void Save(T obj, Field<T> fieldInfo)
        {
            fieldInfo.SetValue(obj, _textBox.GetType().GetProperty("Text").GetValue(_textBox));
        }

        public bool Validate(T obj, Field<T> fieldInfo)
        {
            return true;
        }
    }
}