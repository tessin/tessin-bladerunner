using System;
using System.Collections.Generic;
using System.Text;
using LINQPad.Controls;

namespace Tessin.Bladerunner.Controls
{
    public class Menu : Div
    {
        public Menu(params Control[] children) : base(children)
        {
            this.SetClass("menu");
        }
    }
}
