using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Tessin.Bladerunner.Blades
{
    public static class BladeFactory
    {
        public static IBladeRenderer Make(Func<Blade, object> render)
        {
            return new FuncBlade((blade) => Task.Run(() => render(blade)));
        }

        public static IBladeRenderer Make(Func<Blade, Task<object>> render)
        {
            return new FuncBlade(render);
        }
    }

    public class FuncBlade : IBladeRenderer
    {
        private readonly Func<Blade, Task<object>> _render;

        public FuncBlade(Func<Blade, Task<object>> render)
        {
            _render = render;
        }

        public object Render(Blade blade)
        {
            return _render(blade);
        }
    }

}
