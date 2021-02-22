using System;
using System.Collections.Generic;
using System.Text;
using LINQPad.Controls;

namespace Tessin.Bladerunner.Controls
{
    public class H1 : Control
    {
        public H1(string text, string @class = null) : base("h1", text)
        {
            if (@class != null)
            {
                this.HtmlElement.SetAttribute("class", @class);
            }
        }
    }
}
