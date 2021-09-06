using LINQPad.Controls;
using System;
using System.Collections.Generic;
using System.Text;
using Tessin.Bladerunner.Blades;

namespace Tessin.Bladerunner.Controls
{
    public class NoPadding : Div, INoPadding
    {
        public NoPadding(params Control[] children) : base(children)
        {
        }
    }
}
