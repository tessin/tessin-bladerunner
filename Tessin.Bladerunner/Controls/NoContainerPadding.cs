using LINQPad.Controls;
using System;
using System.Collections.Generic;
using System.Text;
using Tessin.Bladerunner.Blades;

namespace Tessin.Bladerunner.Controls
{
    public class NoContainerPadding : Div, INoContainerPadding
    {
        public NoContainerPadding(params Control[] children) : base(children)
        {
        }
    }
}
