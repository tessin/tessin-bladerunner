using System;
using System.Collections.Generic;
using System.Text;

namespace Tessin.Bladerunner.Controls
{
    public class Hyperlink : LINQPad.Controls.Hyperlink
    {
        public Hyperlink(string text = "", Action<LINQPad.Controls.Hyperlink> onClick = null) : base(text, onClick)
        {
        }

        public Hyperlink(string text, string href) : base(text, href)
        {
        }
    }
}
