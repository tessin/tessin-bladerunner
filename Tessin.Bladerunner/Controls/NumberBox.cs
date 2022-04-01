using LINQPad;
using System;

namespace Tessin.Bladerunner.Controls
{
    public class NumberBox : LINQPad.Controls.TextBox
    {
        private readonly double _min;
        private readonly double _max;

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

            return TrimDecimalZeros(initialValue?.ToString("N" + decimals) ?? "");
        }

        public NumberBox(double? initialValue = null, 
            int decimals = 0, string width = "-webkit-fill-available", Action<LINQPad.Controls.TextBox> onTextInput = null, double min = 0, double max = double.MaxValue)
            : base(FormatNumber(initialValue, decimals), width, onTextInput)
        {
            _min = min;
            _max = max;
            this.HtmlElement.SetAttribute("onfocusout", $"NumberBoxOnFocusOut(event,{decimals},{min},{max})");
            /*
            this.HtmlElement.SetAttribute("type", "number");
            if (decimals > 0)
            {
                this.HtmlElement.SetAttribute("step", (1/Math.Pow(10,decimals)).ToString()); 
            }
            */
        }

        public double? Value {
            get
            {
                if (string.IsNullOrEmpty(this.Text)) return null;
                if (double.TryParse(this.Text, out var result))
                {
                    return Math.Max(Math.Min(_max, result), _min);
                }
                return null;
            }
        }
    }
}
