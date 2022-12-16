using LINQPad.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using Tessin.Bladerunner.Blades;

namespace Tessin.Bladerunner.Controls
{
    public class ContextMenu : Control
    {
        private Item[] _items;
        private BladeManager _bladeManager;
        private Button _target;

        public event EventHandler<Item> ItemSelected;

        public ContextMenu(BladeManager bladeManager, Button target, params Item[] items)
        {
            _items = items;
            _bladeManager = bladeManager;
            _target = target;

            target.Click += (_, __) =>
            {
                Show();
            };

            this.VisualTree.Add(target);
        }

        private void Show()
        {
            Popover portal = null;

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

                var control = new Control("li", children)
                {
                    Enabled = item.Enabled
                };
                if (item.Enabled)
                {
                    control.Click += (_, __) =>
                    {
                        portal?.Clear();
                        if (item.OnClick != null)
                        {
                            item.OnClick?.Invoke(item);
                        }
                        else
                        {
                            ItemSelected?.Invoke(this, item);
                        }
                    };
                }
                return control;
            }

            var menu = new Div(new Control("ul",
                _items.Where(e => e != null).Select(RenderItem).ToArray())).SetClass("context-menu");

            portal = _bladeManager.ShowPopover(menu, _target.HtmlElement.ID);
        }

        public class Item
        {
            public string Label { get; set; }

            public string Tag { get; set; }

            public Action<Item> OnClick { get; set; }

            public string Icon { get; set; }

            public bool Enabled { get; set; }

            public Item(string label, Action<Item> onClick = null, string icon = null, string tag = null, bool enabled = true)
            {
                Label = label;
                OnClick = onClick;
                Tag = tag;
                Icon = icon;
                Enabled = enabled;
            }
        }
    }
}




