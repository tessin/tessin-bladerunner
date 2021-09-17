using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LINQPad.Controls;

namespace Tessin.Bladerunner.Controls
{
    public class UrlBox : LINQPad.Controls.TextBox
    {
        public UrlBox(string initialValue = null, Action<LINQPad.Controls.TextBox> onTextInput = null) : base(initialValue.ToString(), onTextInput:onTextInput)
        {
            this.HtmlElement.SetAttribute("type", "url");
        }
    }
}
