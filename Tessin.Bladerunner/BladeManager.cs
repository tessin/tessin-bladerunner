using System.Collections.Generic;
using System.Linq;
using LINQPad;
using LINQPad.Controls;

namespace Tessin.Bladerunner
{
    public class BladeManager
    {
        private readonly Stack<Blade> _stack;

        private readonly DumpContainer[] _panels;

        private readonly int _maxDepth;

        public BladeManager(int maxDepth = 10)
        {
            _maxDepth = maxDepth;
            _stack = new Stack<Blade>();
            _panels = Enumerable.Range(0, _maxDepth).Select((e, i) => new DumpContainer()).ToArray();

            Styles.Setup();
        }
	
        public void PushBlade(IBladeRenderer renderer, string title = "")
        {
            var blade = new Blade(this, renderer, _stack.Count(), _panels[_stack.Count()], title);
            _stack.Push(blade);
            blade.Refresh();
        }
	
        public void PopTo(int index, bool refresh)
        {
            while(_stack.Count()-1 > index)
            {
                var blade =_stack.Pop();
                blade.Clear();
            }
            if (refresh)
            {
                _stack.Peek().Refresh();
            }
        }

        object ToDump()
        {
            return BladeWrapper(_panels.Select(Blade).ToArray());
        }

        object BladeWrapper(params Control[] blades)
        {
            var div = new Div(blades);
            div.HtmlElement.SetAttribute("class", "blade-wrapper");
            return div;
        }

        Control Blade(DumpContainer dc)
        {
            var innerDiv = new Div(dc);
            innerDiv.HtmlElement.SetAttribute("class", "blade-container");

            var outerDiv = new Div(innerDiv);
            outerDiv.HtmlElement.SetAttribute("class", "blade");

            return outerDiv;
        }

    }
}