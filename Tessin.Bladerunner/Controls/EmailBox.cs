using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LINQPad.Controls;

namespace Tessin.Bladerunner.Controls
{
    public class EmailBox : TextBox
    {
        public EmailBox(string initialValue = null, Action<TextBox> onTextInput = null) : base(initialValue.ToString(), onTextInput:onTextInput)
        {
            this.HtmlElement.SetAttribute("type", "email");
        }
    }
}
