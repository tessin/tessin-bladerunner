using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LINQPad.Controls;

namespace Tessin.Bladerunner.Controls
{
    public class TextBox : LINQPad.Controls.TextBox
    {
        public TextBox(string initialText = null, Action<LINQPad.Controls.TextBox> onTextInput = null, string placeholder = null) 
            : base("text", initialText, "-webkit-fill-available", onTextInput)
        {
            if (!string.IsNullOrEmpty(placeholder))
            {
                this.HtmlElement.SetAttribute("placeholder", placeholder);
            }
        }
    }
}
