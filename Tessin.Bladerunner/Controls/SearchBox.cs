using LINQPad.Controls;
using System;

namespace Tessin.Bladerunner.Controls
{
    public class SearchBox : Div, ITextControl
    {
        private readonly LINQPad.Controls.TextBox _textBox;

        public SearchBox(string initialText = "", string placeholder = "Search", ContextMenu contextMenu = null)
        {
            _textBox = new TextBox(initialText);
            
            if (!string.IsNullOrEmpty(placeholder))
            {
                _textBox.HtmlElement.SetAttribute("placeholder", placeholder);
            }

            _textBox.TextInput += (sender, args) =>
            {
                TextInput?.Invoke(sender, args);
            };

            this.SetClass("search-box");

            this.VisualTree.Add(_textBox);

            this.VisualTree.Add(new Icon(Icons.Magnify));

            if (contextMenu != null)
            {
                VisualTree.Add(contextMenu);
            }
        }

        public void SelectAll()
        {
            _textBox.SelectAll();
        }

        public string Text
        {
            get => _textBox.Text;
            set {
                if(_textBox.Text != value)
                {
                    _textBox.Text = value;
                    TextInput?.Invoke(_textBox, null);
                }
            }
        }

        public event EventHandler TextInput;
    }
}
