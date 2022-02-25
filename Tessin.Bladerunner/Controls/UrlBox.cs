using System;

namespace Tessin.Bladerunner.Controls
{
    [Obsolete]
    public class UrlBox : Controls.TextBox
    {
        public UrlBox(string initialValue = null, Action<LINQPad.Controls.TextBox> onTextInput = null) : base(initialValue.ToString(), onTextInput: onTextInput)
        {
            this.HtmlElement.SetAttribute("type", "url");
        }
    }
}
