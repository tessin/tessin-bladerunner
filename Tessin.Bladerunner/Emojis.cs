using LINQPad;

namespace Tessin.Bladerunner
{
    public static class Emojis
    {
        public static void Setup()
        {
            LINQPad.Util.RawHtml(@"<link href=""https://emoji-css.afeld.me/emoji.css"" rel=""stylesheet"">").Dump();
        }

        public static object Get(Emoji emoji, string tooltip = "")
        {
            return LINQPad.Util.RawHtml($@"<i class=""em em-{emoji.ToString().ToLower()}"" title=""{tooltip}""></i>");
        }
    }

    public enum Emoji
    {
        Warning,
        X,
        Turtle,
        Heavy_Check_Mary,
    }

}
