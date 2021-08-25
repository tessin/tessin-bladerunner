using System;
using System.Collections.Generic;
using System.Text;
using LINQPad.Controls;

namespace Tessin.Bladerunner.Controls
{
    public class Menu : Div
    {
        
        internal Menu(IEnumerable<Control> children) : base(children)
        { 
            this.SetClass("menu");
        }

        public Menu(IEnumerable<MenuButton> children) : this((IEnumerable<Control>)children)
        {
        }

        public Menu(params MenuButton[] children) : this((IEnumerable<Control>)children)
        {
        }
    }
}
