using LINQPad.Controls;

// https://materialdesignicons.com/icon/svg

namespace Tessin.Bladerunner.Controls
{
    public class EmptyIcon : Icon
    {
        public EmptyIcon() : base("")
        {
        }
    }

    public class Icon : Div
    {
        public static Icon Empty()
        {
            return new EmptyIcon();
        }

        public Icon(string icon, string tooltip = "", Theme theme = Theme.Secondary) : base()
        {
            this.HtmlElement.SetAttribute("title", tooltip);
            this.HtmlElement.SetAttribute("class", $"icon theme-{Utils.SplitCamelCase(theme.ToString()).Replace(" ", "-").ToLower()}");
            this.HtmlElement.InnerHtml = icon;
        }
    }
}