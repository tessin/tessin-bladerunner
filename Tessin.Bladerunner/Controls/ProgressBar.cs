using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LINQPad.Controls;

namespace Tessin.Bladerunner.Controls
{
    public class ProgressBar : Control
    {
        public ProgressBar(decimal value, decimal max, string width = "100px") : this(Convert.ToInt32(Math.Round(value/max*100.0m, 0)), width)
        {
        }

        public ProgressBar(int percentage, string width = "100px") : base("progress")
        {
            this.Styles["width"] = width;
            this.HtmlElement.SetAttribute("value", percentage.ToString());
            this.HtmlElement.SetAttribute("max", "100");
        }
    }
}
