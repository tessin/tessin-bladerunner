using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LINQPad.Controls;

namespace Tessin.Bladerunner.Controls
{
    public class NumberBox : LINQPad.Controls.TextBox
    {
        public NumberBox(double? initialValue = null, int decimals = 0, string width = "-webkit-fill-available", Action<LINQPad.Controls.TextBox> onTextInput = null) : base(initialValue.ToString(), width, onTextInput)
        {
            this.HtmlElement.SetAttribute("type", "number");
            if (decimals > 0)
            {
                this.HtmlElement.SetAttribute("step", (1/Math.Pow(10,decimals)).ToString()); 
            }
        }
    }
}
