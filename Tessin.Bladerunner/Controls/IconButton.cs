using System;
using System.Collections.Generic;
using System.Text;
using LINQPad.Controls;

// https://materialdesignicons.com/icon/svg

namespace Tessin.Bladerunner.Controls
{
    public class IconButton : Button
    {
        public IconButton(string icon, Action<Button> onClick, string tooltip = "") : base("", onClick)
        {
            this.HtmlElement.SetAttribute("class", "icon-button");
            this.HtmlElement.SetAttribute("title", tooltip);
            this.HtmlElement.InnerHtml = icon;
        }
    }
}
