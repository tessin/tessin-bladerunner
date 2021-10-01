using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LINQPad;
using LINQPad.Controls;

// https://materialdesignicons.com/icon/svg

namespace Tessin.Bladerunner.Controls
{
    public class IconPlaceholder : Div
    {
        public IconPlaceholder()
        {
            this.Styles["width"] = "24px";
            this.Styles["height"] = "24px";
        }
    }

    public class Icon : Div
    {
        public static Icon Empty()
        {
            return new Icon("");
        }

        public Icon(string icon, string tooltip = "", Color color = Color.Black) : base()
        {
            this.HtmlElement.SetAttribute("title", tooltip);
            this.HtmlElement.SetAttribute("class", $"icon {color.ToString().ToLower()}");
            this.HtmlElement.InnerHtml = icon;
        }
    }
}