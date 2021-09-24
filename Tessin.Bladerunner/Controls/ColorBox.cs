using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LINQPad.Controls;

namespace Tessin.Bladerunner.Controls
{
    public class ColorBox : LINQPad.Controls.TextBox
    {
        public ColorBox(string initialValue = null, string width = "38px", Action<LINQPad.Controls.TextBox> onTextInput = null) : base(initialValue ?? "#000000", width, onTextInput)
        {
            this.HtmlElement.SetAttribute("type", "color");
        }
    }
}