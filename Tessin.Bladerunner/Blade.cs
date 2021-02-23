using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LINQPad;
using LINQPad.Controls;
using Tessin.Bladerunner.Controls;

namespace Tessin.Bladerunner
{
    public class Blade
    {
        public int Index { get; set; }

        public IBladeRenderer Renderer { get; set; }
	
        public BladeManager Manager { get; set;}

        public DumpContainer Panel { get; set; }

        public string Title { get; set;  }

        public Blade(BladeManager manager, IBladeRenderer renderer, int index, DumpContainer panel, string title)
        {
            Manager = manager;
            Index = index;
            Renderer = renderer;
            Panel = panel;
            Title = title;
        }
	
        public object Render()
        {
            try
            {
                var dc = new DumpContainer();

                var buttons = new List<Control>()
                {
                    new IconButton(Icons.Refresh, (_) => {
                        Manager.PopTo(Index, true);
                    })
                };

                if (Index == 0 && Manager.ShowDebugButton)
                {
                    buttons.Add(new IconButton(Icons.Duck, (_) =>
                    {
                        Manager.DebugHtml();
                    }));
                }

                if (Index != 0)
                {
                    buttons.Add(new IconButton(Icons.Close, (_) =>
                    {
                        Manager.PopTo(Index - 1, false);
                    }));
                }

                var div = Div("blade-panel", 
                    Div("blade-header",
                        Element("h1", null, Title??""),
                        Element("aside", null, buttons.ToArray())
                    ),
                    Div("blade-content", dc)
                );

                dc.Content = Renderer.Render(this);

                return div;
            }
            catch (Exception e)
            {
                return e;
            }
        }

        public void PushBlade(IBladeRenderer renderer, string title = null)
        {
            Manager.PopTo(this.Index, false);
            Manager.PushBlade(renderer, title);
        }

        public void PopToPrevious(bool refresh = true)
        {
            Manager.PopTo(this.Index - 1, refresh);
        }

        public void Refresh()
        {
            Manager.PopTo(this.Index, false);
            Panel.Content = Element("div", "loading", "Loading...");
            Task.Run(() =>
            {
                Panel.Content = this.Render();
            });
        }

        public void Clear()
        {
            Panel.Content = "";
        }

        private static Control Div(string @class, params Control[] children)
        {
            return Element("div", @class, children);
        }

        private static Control Element(string name, string @class, params Control[] children)
        {
            var el = new Control(name, children);
            el.HtmlElement.SetAttribute("class", @class);
            return el;
        }

        private static Control Element(string name, string @class, string content)
        {
            var el = new Control(name);
            el.HtmlElement.InnerText = content;
            if (@class != null)
            {
                el.HtmlElement.SetAttribute("class", @class);
            }
            return el;
        }
    }
}