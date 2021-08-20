using System;
using System.Collections.Generic;
using System.Text;
using LINQPad.Controls;

namespace Tessin.Bladerunner.Controls
{
    [Obsolete]
    public class H2 : Control
    {
        public H2(string text, string @class = null) : base("h2", text)
        {
            if (@class != null)
            {
                this.HtmlElement.SetAttribute("class", @class);
            }
        }
    }
}
