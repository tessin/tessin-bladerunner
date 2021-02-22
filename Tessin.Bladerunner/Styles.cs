using LINQPad;
using Tessin.Bladerunner.Themes;

namespace Tessin.Bladerunner
{
    public static class Styles
    {
        public static void Setup()
        {
            var settings = new ThemeSettings()
            {
                GENERAL_BG = "white",
                GENERAL_FG = "#00000"
            };

            LINQPad.Util.RawHtml($@"<link rel=""preconnect"" href=""https://fonts.gstatic.com"" />").Dump();
            LINQPad.Util.RawHtml($@"<link href=""https://fonts.googleapis.com/css2?family=Roboto:wght@100;300;400&display=swap"" rel=""stylesheet"" />").Dump();
            LINQPad.Util.RawHtml($@"<style>{Themes.Themes.Default}</style>").Dump();
        }
    }
}