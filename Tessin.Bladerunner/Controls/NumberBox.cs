using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
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
            string TrimDecimalZeros(string input)
            {
                int dotIndex = input.LastIndexOf('.');
                if (dotIndex != -1)
                {
                    return input.Substring(0, dotIndex) + input.Substring(dotIndex, input.Length - dotIndex).TrimEnd('0').TrimEnd('.');
                }

                return input;
            }

            return TrimDecimalZeros(initialValue?.ToString("N"+decimals) ?? "");
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
