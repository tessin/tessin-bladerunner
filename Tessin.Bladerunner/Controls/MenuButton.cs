using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using LINQPad;
using LINQPad.Controls;

namespace Tessin.Bladerunner.Controls
{
    public class MenuButton : DumpContainer
    {
        public MenuButton(
            string label, 
            Action<Button> onClick, 
            string svgIcon = null,
            string tooltip = null,
            object pill = null,
            AnyTask pillTask = null,
            IconButton[] actions = null)
        {
            //todo: can be made into a Control using VisualTree
            
            List<Control> children = new List<Control>();

            var button = new Button(null, onClick);
            if (tooltip != null)
            {
                button.HtmlElement.SetAttribute("title", tooltip);
            }
            button.HtmlElement.InnerHtml = $@"{svgIcon}<span>{label}</span>";
            children.Add(button);

            Control pillContainer = null;

            if (pillTask != null || pill != null)
            {
                var _pillContainer = new DumpContainer();
                pillContainer = _pillContainer.ToControl();
                children.Add(pillContainer);

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

                children.Add(divActions);
            }

            var divContainer = new Div(children);
            divContainer.SetClass("menu-button");

            if (actions != null)
            { 
                JavascriptHelpers.ShowOnMouseOver(divContainer, divActions, pillContainer);
            }
            
            this.Content = new Div(divContainer);
        }
    }
}