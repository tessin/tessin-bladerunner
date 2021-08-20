using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using LINQPad;
using LINQPad.Controls;

namespace Tessin.Bladerunner.Controls
{
    public class MenuButton : Div
    {
        private class InternalButton : Div
        {
            public InternalButton(Action<Div> onClick, object label, string svgIcon, string tooltip)
            {
                this.SetClass("button");

                if (tooltip != null)
                {
                    this.HtmlElement.SetAttribute("title", tooltip);
                }

                if(!string.IsNullOrEmpty(svgIcon))
                {
                    var divIcon = new Div();
                    divIcon.HtmlElement.InnerHtml = svgIcon;
                    this.VisualTree.Add(divIcon);
                }

                if(label is string stringLabel)
                {
                    this.VisualTree.Add(new Span(stringLabel));
                }
                else
                {
                    var dc = new DumpContainer();
                    dc.Content = label;
                    this.VisualTree.Add(new Div(dc).SetClass("dc"));
                }
            }
        }

        public MenuButton(
            object label,
            Action<Div> onClick,
            string svgIcon = null,
            string tooltip = null,
            object pill = null,
            AnyTask pillTask = null,
            IconButton[] actions = null)
        {
            this.SetClass("menu-button");

            var button = new InternalButton(onClick, label, svgIcon, tooltip);

            this.VisualTree.Add(button);

            Control pillContainer = null;

            if (pillTask != null || pill != null)
            {
                var _pillContainer = new DumpContainer();
                pillContainer = _pillContainer.ToControl();
                this.VisualTree.Add(pillContainer);

                if (pillTask != null)
                {
                    Task.Run(() => pillTask.Result).ContinueWith(e =>
                    {
                        if (e.Result != null)
                        {
                            var content = e.Result.ToString();
                            if (content != "")
                            {
                                var span = new Span(content);
                                span.SetClass("menu-button--pill");
                                _pillContainer.Content = span;
                            }
                            else
                            {
                                _pillContainer.Content = "";
                            }
                        }
                        else
                        {
                            _pillContainer.Content = "";
                        }
                    }).ConfigureAwait(false);
                }
                else
                {
                    var content = pill.ToString();
                    if (content != "")
                    {
                        var span = new Span(content);
                        span.SetClass("menu-button--pill");
                        _pillContainer.Content = span;
                    }
                }
            }

            Div divActions = null;

            if (actions != null)
            {
                divActions = new Div(
                    actions.Cast<Control>()
                );
                divActions.SetClass("menu-button--actions");

                this.VisualTree.Add(divActions);
            }

            if (actions != null)
            {
                JavascriptHelpers.ShowOnMouseOver(this, divActions, pillContainer);
            }
        }

    }
}