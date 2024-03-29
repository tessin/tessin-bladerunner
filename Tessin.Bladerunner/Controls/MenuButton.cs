﻿using LINQPad;
using LINQPad.Controls;
using System;
using System.Linq;

namespace Tessin.Bladerunner.Controls
{
    public class MenuButton : Div
    {
        private class InternalButton : Div
        {
            public InternalButton(Action<Div> onClick, object label, string svgIcon, string tooltip)
            {
                DateTime? lastClick = null;

                Action<Div> _onClick =
                (div) =>
                {
                    if (lastClick != null && (DateTime.Now - lastClick.Value) < TimeSpan.FromSeconds(1))
                    {
                        lastClick = DateTime.Now;
                        return;
                    }
                    lastClick = DateTime.Now;
                    onClick?.Invoke(div);
                };

                this.SetClass("button");
                this.Click += (_, _) =>
                {
                    _onClick(this);
                };

                if (tooltip != null)
                {
                    this.HtmlElement.SetAttribute("title", tooltip);
                }

                if (!string.IsNullOrEmpty(svgIcon))
                {
                    var divIcon = new Div();
                    divIcon.SetClass("menu-button--icon");
                    divIcon.HtmlElement.InnerHtml = svgIcon;
                    this.VisualTree.Add(divIcon);
                }

                if (label is string stringLabel)
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
            Action<Div> onClick = null,
            string svgIcon = null,
            string tooltip = null,
            object pill = null,
            AnyTask pillTask = null,
            Theme? pillTheme = Theme.Empty,
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

                var formatter = new DefaultContentFormatter();

                Control Format(object content)
                {
                    return formatter.Format(content, (c, e) =>
                    {
                        if (e) return c;
                        var span = new Span(c);
                        span.AddClass("menu-button--pill");
                        if(pillTheme != null)
                        {
                            span.AddClass($"theme-{Utils.SplitCamelCase(pillTheme.ToString()).Replace(" ", "-").ToLower()}");
                        }
                        return span;
                    });
                }

                if (pillTask != null)
                {
                    pillTask.OnResult(result => { _pillContainer.Content = Format(result); });
                }
                else
                {
                    _pillContainer.Content = Format(pill);
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