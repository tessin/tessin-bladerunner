using System;
using System.Collections.Generic;
using System.Text;
using LINQPad.Controls;

namespace Tessin.Bladerunner
{
    public static class ControlExtensions
    {
        public static void SetClass(this Control control, string @class)
        {
            control.HtmlElement.SetAttribute("class", @class);
        }
    }
}
