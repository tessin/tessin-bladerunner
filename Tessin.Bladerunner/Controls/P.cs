using System;
using System.Collections.Generic;
using System.Text;
using LINQPad.Controls;

namespace Tessin.Bladerunner.Controls
{
    public class P : Control
    {
        public P(string text, string @class = null) : base("p", text)
        {
            if (@class != null)
            {
                this.SetClass(@class);
            }
        }
    }
}
