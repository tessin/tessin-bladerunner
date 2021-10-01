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
        public IconButton(string icon, Action<Button> onClick = null, string tooltip = "", Color color = Color.Black)
        {
            this.AddClass($"icon-button {color.ToString().ToLower()}");
            this.HtmlElement.SetAttribute("title", tooltip);
            this.HtmlElement.InnerHtml = icon;

            IconButton obj = this;
            if (onClick != null)
            {
                Click += delegate
                {
                    onClick(obj);
                };
            }
        }
    }
}
