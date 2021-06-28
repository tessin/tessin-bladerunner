using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using LINQPad;
using LINQPad.Controls;

namespace Tessin.Bladerunner.Controls
{
    public class ContextMenu : Control
    {
        private Control _menu;

        public ContextMenu(Button target, params Item[] items)
        {
            Control RenderItem(Item item)
            {
                var children = new List<Control>();

                if (item.Icon != null)
                {
                    var icon = new Control();
                    icon.HtmlElement.InnerHtml = item.Icon;
                    children.Add(icon);
                }

                children.Add(new Span(item.Label));

                var control = new Control("li", children);
                control.Click += (_,__) =>
                {
                    Close();
                    item.OnClick?.Invoke(item);
                };
                return control;
            }

            target.Click += (_, __) =>
            {
                Show();
            };

            _menu = new Control("ul", items.Select(RenderItem).ToArray());
 
            VisualTree.Add(new Div(target, _menu, new Literal(_menu.HtmlElement.ID)).SetClass("context-menu"));

            //todo: this is broken :( works when we only have one blade

            var script = new Literal(
$@"<script>
    document.addEventListener('click', function(e) {{
        external.log('{_menu.HtmlElement.ID}');
        var menu = document.getElementById('{_menu.HtmlElement.ID}');
        if(!!menu && menu.style.visibility == 'visible')
        {{
            var el = e.target;
            var closest = false;
            do {{
                if (el.msMatchesSelector('#{_menu.HtmlElement.ID}')) {{
                    closest = true;
                    break;
                }}
                el = el.parentElement || el.parentNode;
            }} while (el !== null && el.nodeType === 1);
            if(!closest)
            {{
                menu.style.visibility = 'hidden';
            }}
        }}
    }});
</script>"
            );

            VisualTree.Add(script);
        }

        private void Show()
        {
            _menu.Styles["visibility"] = "visible";
        }

        private void Close()
        {
            _menu.Styles["visibility"] = "hidden";
        }

        public class Item
        {
            public string Label { get; set; }

            public string Tag { get; set; }

            public Action<Item> OnClick { get; set; }

            public string Icon { get; set; }

            public Item(string label, Action<Item> onClick = null, string icon = null, string tag = null)
            {
                Label = label;
                OnClick = onClick;
                Tag = tag;
                Icon = icon;
            }
        }
    }
}




