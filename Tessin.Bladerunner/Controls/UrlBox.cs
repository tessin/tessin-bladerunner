using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LINQPad.Controls;

namespace Tessin.Bladerunner.Controls
{
    public class UrlBox : TextBox
    {
        public UrlBox(string initialValue = null, Action<TextBox> onTextInput = null) : base(initialValue.ToString(), onTextInput:onTextInput)
        {
            this.HtmlElement.SetAttribute("type", "url");
        }
    }
}
