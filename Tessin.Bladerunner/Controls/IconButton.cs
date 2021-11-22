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
        public IconButton(string icon, Action<Button> onClick = null, string tooltip = "", Theme theme = Theme.Primary, bool enabled = true) 
            : base(theme:theme)
        {
            this.AddClass($"icon-button");
            this.HtmlElement.SetAttribute("title", tooltip);
            this.HtmlElement.InnerHtml = icon;
            this.Enabled = enabled;
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
