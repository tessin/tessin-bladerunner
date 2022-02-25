using LINQPad.Controls;
using System.Collections.Generic;

namespace Tessin.Bladerunner.Controls
{
    public class Label : LINQPad.Controls.Label
    {
        public Label(string text = "") : base(text)
        {
        }

        public Label(params Control[] children) : base(children)
        {
        }

        public Label(IEnumerable<Control> children) : base(children)
        {
        }
    }
}
