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
        public ProgressBar(int value, string width = "100px") : base("progress")
        {
            this.Styles["width"] = width;
            this.HtmlElement.SetAttribute("value", value.ToString());
            this.HtmlElement.SetAttribute("max", "100");
        }
    }
}
