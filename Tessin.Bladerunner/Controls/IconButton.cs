using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LINQPad;
using LINQPad.Controls;

// https://materialdesignicons.com/icon/svg

namespace Tessin.Bladerunner.Controls
{
    public class IconButton : Button
    {
        public static object HorizontalRun(params IconButton[] buttons)
        {
            return Util.HorizontalRun(false, buttons.Where(f => f != null).ToList());
        }

        public IconButton(string icon, Action<Button> onClick, string tooltip = "", Color color = Color.Black) : base("", onClick)
        {
            this.HtmlElement.SetAttribute("class", $"icon-button {color.ToString().ToLower()}");
            this.HtmlElement.SetAttribute("title", tooltip);
            this.HtmlElement.InnerHtml = icon;
        }
    }
}
