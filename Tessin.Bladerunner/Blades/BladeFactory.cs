using System;
using System.Collections.Generic;
using System.Text;

namespace Tessin.Bladerunner.Blades
{
    public static class BladeFactory
    {
        public static IBladeRenderer Make(Func<Blade, object> render)
        {
            return new FuncBlade(render);
        }
    }

    public class FuncBlade : IBladeRenderer
    {
        private readonly Func<Blade, object> _render;

        public FuncBlade(Func<Blade, object> render)
        {
            _render = render;
        }

        public object Render(Blade blade)
        {
            return _render(blade);
        }
    }

}
