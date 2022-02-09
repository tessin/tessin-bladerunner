using System;
using System.Collections.Generic;
using System.Text;
using LINQPad.Controls;
using LINQPad.Controls.Core;

namespace Tessin.Bladerunner.Controls
{
    public class FileBox : LINQPad.Controls.Div, ITextControl
    {
        private readonly LINQPad.Controls.TextBox _textBox;

        public FileBox(string initialValue = null, string initialCatalog = null, string width = "-webkit-fill-available",
            Action<LINQPad.Controls.TextBox> onTextInput = null)
        {
            this.SetClass("file-box");

            _textBox = new LINQPad.Controls.TextBox(initialValue, width, onTextInput);

            _textBox.TextInput += (sender, args) =>
            {
                TextInput?.Invoke(sender, args);
            };

            this.VisualTree.Add(_textBox);

            var btnSelectFile = new IconButton(Icons.FolderOpen, (_) =>
            {
                var path = ShowOpenFileDialog(initialCatalog);
                _textBox.Text = path ?? _textBox.Text;
                TextInput?.Invoke(_textBox, null!);
            });

            this.VisualTree.Add(btnSelectFile);
        }

        public string ShowOpenFileDialog(string initialCatalog)
        {
            // You can't hide from me Joe!
            Type tBridge = typeof(LINQPad.Util).Assembly.GetType("LINQPad.ExecutionModel.UIBridge");
            Type tService = typeof(LINQPad.Util).Assembly.GetType("LINQPad.ExecutionModel.IRuntimeUIServices");
            var prop = tBridge.GetProperty("UIServices");
            var oService = prop.GetValue(null, null);
            if (oService != null)
            {
                var method = tService.GetMethod("ShowOpenFileDialog");
                var result = method.Invoke(oService, new[] { initialCatalog });
                return result?.ToString();
            }
            return null;
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

        public LINQPad.Controls.TextBox TextBox => _textBox;
    }
}
