using System;
using System.Collections.Generic;
using System.Text;
using LINQPad.Controls;

namespace Tessin.Bladerunner.Controls
{
    public class FieldLabel : Label
    {
        public FieldLabel(string text) : base(text)
        {
            this.Styles["font-weight"] = "bold";
        }
    }
}
