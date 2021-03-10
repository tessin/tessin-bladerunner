using System;
using System.Collections.Generic;
using System.Text;
using LINQPad.Controls;

namespace Tessin.Bladerunner.Controls
{
    public class Spacer : Div
    {

        public Spacer(string width, string height)
        {
            this.HtmlElement.SetAttribute("style", $"width:{width};height:{height};");
        }

    }
}
