using System;
using System.Collections.Generic;
using System.Text;
using LINQPad.Controls;
using LINQPad.Controls.Core;

namespace Tessin.Bladerunner.Controls
{
    public class SearchBox : Control, ITextControl
    {
        private readonly Div _container;

        private readonly TextBox _textBox;

        private Action<SearchBox> _externalOpenAction;

        private Action _externalClearAction;

        private Hyperlink _externalOpen;

        public SearchBox(string initialText = "", string placeHolder = "Search", ContextMenu contextMenu = null)
        {
            var children = new List<Control>();

            _textBox = new TextBox(initialText, width: "150px");
            _textBox.HtmlElement.SetAttribute("placeholder", placeHolder);
            _textBox.TextInput += (sender, args) =>
            {
                TextInput?.Invoke(sender, args);
            };

            children.Add(_textBox);

            if (contextMenu != null)
            {
                children.Add(contextMenu);
            }

            var externalClear = new IconButton(Icons.Close, (_) =>
            {
                _container.CssChildRules[".external-open", "display"] = "none";
                _container.CssChildRules[".external-clear", "display"] = "none";
                _container.CssChildRules[".flexbox INPUT:-ms-input-placeholder", "color"] = "#ccc !important";
                _externalClearAction?.Invoke();
                _textBox.Enabled = true;
            });

            externalClear.AddClass("external-clear");
            children.Add(externalClear);

            var flexBox = new Div(children);
            flexBox.SetClass("flexbox");
            
            _externalOpen = new Hyperlink("Query", (_) => _externalOpenAction?.Invoke(this));
            _externalOpen.AddClass("external-open");

            _container = new Div(flexBox, _externalOpen);
            _container.SetClass("search-box");

            VisualTree.Add(_container);
        }

        public void SetExternal(string title, Action<SearchBox> onClick)
        {
            _externalOpen.Text = title;
            _container.CssChildRules[".external-open", "display"] = "block";
            _container.CssChildRules[".external-clear", "display"] = "block";
            _container.CssChildRules[".flexbox INPUT:-ms-input-placeholder", "color"] = "transparent !important";
            _externalOpenAction = onClick;
            _textBox.Enabled = false;
        }

        public void SelectAll()
        {
            _textBox.SelectAll();
        }

        public string Text
        {
            get => _textBox.Text;
            set => _textBox.Text = value;
        }
        
        public event EventHandler TextInput;
    }
}
