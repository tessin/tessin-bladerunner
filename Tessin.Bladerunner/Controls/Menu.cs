using LINQPad.Controls;
using System.Collections.Generic;
using Tessin.Bladerunner.Blades;

namespace Tessin.Bladerunner.Controls
{
    public class Menu : Div, INoContainerPadding
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
