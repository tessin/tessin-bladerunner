using System;

namespace Tessin.Bladerunner.Controls
{
    public sealed class Hyperlink : LINQPad.Controls.Hyperlink
    {
        public Hyperlink(string text = "", Action<Hyperlink> onClick = null) : base(text)
        {
            Hyperlink obj = this;
            if (onClick != null)
            {
                Click += delegate
                {
                    onClick(obj);
                };
            }
        }

        public Hyperlink(string text, string href) : base(text, href)
        {
        }
    }
}
