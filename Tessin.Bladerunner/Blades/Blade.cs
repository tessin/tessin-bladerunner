using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LINQPad;
using LINQPad.Controls;
using Tessin.Bladerunner.Controls;

namespace Tessin.Bladerunner.Blades
{
    public class Blade
    {
        public int Index { get; set; }

        public IBladeRenderer Renderer { get; set; }
	
        public BladeManager Manager { get; set;}

        public DumpContainer Panel { get; set; }

        public string Title { get; set;  }

        public Div Container { get; set; }

        public Blade(BladeManager manager, IBladeRenderer renderer, int index, DumpContainer panel, string title, Div container)
        {
            Manager = manager;
            Index = index;
            Renderer = renderer;
            Panel = panel;
            Title = title;
            Container = container;
        }
	
        public async Task<object> Render()
        {
            Container?.RemoveClass("blade-hidden");
            try
            {
                var dc = new DumpContainer();

                var buttons = new List<Control>();

                if (Index != -1)
                {
                    buttons.Add(new IconButton(Icons.Refresh, (_) =>
                    {
                        Manager.PopTo(Index, true);
                    }, theme: Theme.PrimaryAlternate));
                }

                if (Index == 0 && Manager.ShowDebugButton)
                {
                    buttons.Add(new IconButton(Icons.Duck, (_) =>
                    {
                        Manager.DebugHtml();
                    }, theme:Theme.PrimaryAlternate));
                }

                if (Index != 0)
                {
                    buttons.Add(new IconButton(Icons.Close, (_) =>
                    {
                        if (Index == -1)
                        {
                            Manager.CloseSideBlade(false);
                        }
                        else
                        {
                            Manager.PopTo(Index - 1, false);
                        }
                    }, theme: Theme.PrimaryAlternate));
                }

                var div = Div("blade-panel", 
                    Div("blade-header",
                        Element("h1", null, string.IsNullOrEmpty(Title) ? " " : Title),
                        Element("aside", null, buttons.ToArray())
                    ),
                    Div("blade-content", dc)
                );

                await ControlExtensions.AddPadding(dc, Renderer.Render(this));

                return div;
            }
            catch (Exception ex)
            {
                return Layout.Padding(true).Vertical(
                    Typography.H2(ex.GetType().Name), 
                    Typography.P(ex.Message), 
                    new CollapsablePanel("Stack Trace", Typography.Code(ex.StackTrace)));
            }
        }

        public void PushBlade(IBladeRenderer renderer, string title = null)
        {
            Manager.PopTo(this.Index, false);
            Manager.Push(renderer, title);
        }

        public void PopToPrevious(bool refresh = true)
        {
            if (Index == -1)
            {
                Manager.CloseSideBlade(refresh);
            }
            else
            {
                Manager.PopTo(this.Index - 1, refresh);
            }
        }

        public void PopToMe(bool refresh = false)
        {
            if (Index != -1)
            {
                Manager.PopTo(this.Index, refresh);
            }
        }

        public void Refresh()
        {
            if (Index != -1)
            {
                Manager.PopTo(this.Index, false);
                Panel.Content = Element("div", "loading", "Loading...");
            }
            Task.Run(async () =>
            {
                Panel.Content = await this.Render();
                Util.InvokeScript(false, "ScrollTo", Container.HtmlElement.ID);
            });
        }

        public void Clear()
        {
            Container?.AddClass("blade-hidden");
            Panel.ClearContent();
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

        public Action<T> Catch<T>(Func<T,Task> action)
        {
            return async (btn) =>
            {
                try
                {
                    await action(btn);
                }
                catch (Exception ex)
                {
                    Manager.ShowUnhandledException(ex);
                }
            };
        }

        public Action<T> Catch<T>(Action<T> action)
        {
            return (btn) =>
            {
                try
                {
                    action(btn);
                }
                catch (Exception ex)
                {
                    Manager.ShowUnhandledException(ex);
                }
            };
        }

    }
}