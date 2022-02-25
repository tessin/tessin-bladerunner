using System;

namespace Tessin.Bladerunner.Controls
{
    public class EmailBox : Controls.TextBox
    {
        public EmailBox(string initialValue = null, Action<LINQPad.Controls.TextBox> onTextInput = null) : base(initialValue.ToString(), onTextInput: onTextInput)
        {
            this.HtmlElement.SetAttribute("type", "email");
        }
    }
}
