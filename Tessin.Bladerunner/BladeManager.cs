using System.Collections.Generic;
using System.Linq;
using LINQPad;

namespace Tessin.Bladerunner
{
    public class BladeManager
    {
        readonly Stack<Blade> _stack;

        readonly DumpContainer _dc;
	
        public BladeManager()
        {
            _stack = new Stack<Blade>();
            _dc = new DumpContainer();
        }
	
        public void PushBlade(IBladeRenderer renderer)
        {
            _stack.Push(new Blade(this, renderer, _stack.Count()));
            Render();
        }
	
        public void PopTo(int index)
        {
            while(_stack.Count()-1 > index)
            {
                _stack.Pop();
            }
        }
	
        internal void Render()
        {
            var blades = _stack.Select(e => e.Render()).Reverse().ToList();
            _dc.Content = LINQPad.Util.HorizontalRun(true, blades);
        }
	
        object ToDump()
        {
            return _dc;
        }
    }
}