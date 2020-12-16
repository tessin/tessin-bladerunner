using System;
using System.Collections.Generic;
using System.Text;
using LINQPad;

namespace Tessin.Bladerunner.Blades
{
    public class DisplayBlade : IBladeRenderer
    {
        private readonly object[] _show;

        public DisplayBlade(params object[] show)
        {
            _show = show;
        }

        public object Render(Blade blade)
        {
            return
                LINQPad.Util.VerticalRun(
                  _show
                );
        }
    }
}
