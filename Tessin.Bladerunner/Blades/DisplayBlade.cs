using System;
using System.Collections.Generic;
using System.Text;
using LINQPad;
using LINQPad.Controls;

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
            if (_show.Length == 1)
            {
                return _show[0];
            }

            return Layout.Vertical(
              _show
            );
        }
    }
}