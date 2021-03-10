using System;
using System.Collections.Generic;
using System.Text;
using LINQPad.Controls;

// https://materialdesignicons.com/icon/svg

namespace Tessin.Bladerunner.Controls
{
    public class Icon : Div
    {
        public Icon(string icon, string tooltip = "") : base()
        {
            this.HtmlElement.SetAttribute("title", tooltip);
            this.HtmlElement.SetAttribute("class", "icon");
            this.HtmlElement.InnerHtml = icon;
        }
    }
}