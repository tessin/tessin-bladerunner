using System;
using System.Collections.Generic;
using System.Text;
using LINQPad.Controls;

// https://materialdesignicons.com/icon/svg

namespace Tessin.Bladerunner.Controls
{
    public class Icon : Div
    {
        public Icon(string icon, string tooltip = "", Color color = Color.Black) : base()
        {
            this.HtmlElement.SetAttribute("title", tooltip);
            this.HtmlElement.SetAttribute("class", $"icon {color.ToString().ToLower()}");
            this.HtmlElement.InnerHtml = icon;
        }
    }
}