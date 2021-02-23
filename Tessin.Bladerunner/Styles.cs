using System.Collections.Generic;
using LINQPad;
using LINQPad.Controls;
using Tessin.Bladerunner.Themes;

namespace Tessin.Bladerunner
{
    public static class Styles
    {
        public static Div Generate()
        {
            var divStyles = new Div();
            divStyles.HtmlElement.InnerHtml = $@"
                <link rel=""preconnect"" href=""https://fonts.gstatic.com"" />
                <link href=""https://fonts.googleapis.com/css2?family=Roboto:wght@100;300;400&display=swap"" rel=""stylesheet"" />
                <style>{Themes.Themes.Default}</style>
            ";

            return divStyles;
        }
    }
}