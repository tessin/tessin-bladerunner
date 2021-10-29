using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using LINQPad;

namespace Tessin.Bladerunner.Controls
{
    public class NumberBox : LINQPad.Controls.TextBox
    {
        static NumberBox()
        {
            Util.HtmlHead.AddScript(Javascript.NumberBox);
        }

        public static string FormatNumber(double? initialValue, int decimals)
        {
            return initialValue?.ToString("N"+decimals).TrimEnd('0') ?? "";
        }

        public NumberBox(double? initialValue = null, int decimals = 0, string width = "-webkit-fill-available", Action<LINQPad.Controls.TextBox> onTextInput = null) 
            : base(FormatNumber(initialValue, decimals), width, onTextInput)
        {
            this.HtmlElement.SetAttribute("onkeyup", $"NumberBoxOnChange(event,{decimals})");
            /*
            this.HtmlElement.SetAttribute("type", "number");
            if (decimals > 0)
            {
                this.HtmlElement.SetAttribute("step", (1/Math.Pow(10,decimals)).ToString()); 
            }
            */
        }
    }
}
