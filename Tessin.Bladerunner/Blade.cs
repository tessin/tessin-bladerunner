using LINQPad.Controls;

namespace Tessin.Bladerunner
{
    public class Blade
    {
        public int Index { get; set; }

        public IBladeRenderer Renderer { get; set; }
	
        public BladeManager Manager { get; set;}

        object _rendered = null;
	
        public Blade(BladeManager manager, IBladeRenderer renderer, int index)
        {
            Manager = manager;
            Index = index;
            Renderer = renderer;
        }
	
        public object Render()
        {
            if (_rendered == null)
            {
                _rendered = LINQPad.Util.VerticalRun(
                    new Hyperlink("Close", (_) => {
                        Manager.PopTo(Index-1);
                        Manager.Render();
                    }),
                    Renderer.Render(this)
                );
            }
            return _rendered;
        }

        public void PushBlade(IBladeRenderer renderer)
        {
            Manager.PopTo(this.Index);
            Manager.PushBlade(renderer);
        }
    }
}