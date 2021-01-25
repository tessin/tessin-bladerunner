using System;
using System.ComponentModel;
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
	
        public object Render(bool refresh = false)
        {
            if (_rendered == null || refresh)
            {
                try
                {
                    _rendered = LINQPad.Util.VerticalRun(
                        new Hyperlink("Close", (_) => {
                            Manager.PopTo(Index-1);
                            Manager.Render();
                        }),
                        Renderer.Render(this)
                    );
                }
                catch (Exception e)
                {
                    return e;
                }
               
            }  
            return _rendered;
        }

        public void PushBlade(IBladeRenderer renderer)
        {
            Manager.PopTo(this.Index);
            Manager.PushBlade(renderer);
        }

        public void PopTo()
        {
            Manager.PopTo(this.Index - 1);
        }

        public void PopToAndRefresh()
        {
            Manager.PopTo(this.Index-1);
            Manager.Refresh();
        }

        public void Refresh()
        {
            this.Render(true);
            this.Manager.Render();
        }
    }
}