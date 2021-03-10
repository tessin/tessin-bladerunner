using LINQPad.Controls;

namespace Tessin.Bladerunner
{
    public static class JavascriptHelpers
    {

        public static void ShowOnMouseOver(Control parent, Control child, Control replaces = null)
        { 
            child?.HtmlElement?.SetAttribute("style", "visibility:hidden;");
            parent?.HtmlElement?.SetAttribute("onMouseOver", 
                $"document.getElementById('{child?.HtmlElement?.ID}').style.visibility = 'visible';{(replaces != null ? $"document.getElementById('{replaces?.HtmlElement?.ID}').style.visibility = 'hidden';" : "")}");
            parent?.HtmlElement?.SetAttribute("onMouseOut", 
                $"document.getElementById('{child?.HtmlElement?.ID}').style.visibility = 'hidden';{(replaces != null ? $"document.getElementById('{replaces?.HtmlElement?.ID}').style.visibility = 'visible';" : "")}");
        }

    }
}
