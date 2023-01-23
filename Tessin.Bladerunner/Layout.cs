﻿using LINQPad.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using Tessin.Bladerunner.Blades;
using Tessin.Bladerunner.Controls;

//todo: make simple Vertical layouts more simple

namespace Tessin.Bladerunner
{
    public enum HorizontalAlignment
    {
        Left,
        Center,
        Right,
        Stretch
    }

    public enum VerticalAlignment
    {
        Top,
        Middle,
        Bottom,
        Stretch
    }

    internal enum Orientation
    {
        Vertical,
        Horizontal
    }

    public class LayoutBuilder
    {
        internal LayoutBuilder()
        {
        }

        public class Element
        {
            internal string _space;
            internal object _content;

            public Element(object content, string space = "auto")
            {
                _content = content;
                _space = space;
            }
        }

        internal IContentFormatter _formatter = new DefaultContentFormatter();
        internal bool _debug = false;
        internal string _class = null;
        internal bool _gap = true;
        internal bool _padding = false;
        internal bool? _fill = null;
        internal string _width = null;
        internal string _height = "max-content";
        internal Orientation _orientation = Orientation.Vertical;
        internal HorizontalAlignment? _hAlignment = null;
        internal VerticalAlignment? _vAlignment = null;
        internal readonly List<Element> _elements = new();
        internal bool _containerPadding = true;
        
        public LayoutBuilder Padding(bool padding)
        {
            _padding = padding;
            return this;
        }

        public LayoutBuilder ContainerPadding(bool containerPadding = true)
        {
            _containerPadding = containerPadding;
            return this;
        }

        public LayoutBuilder Gap(bool gap = true)
        {
            _gap = gap;
            return this;
        }

        public LayoutBuilder Fill(bool fill = true)
        {
            _fill = fill;
            return this;
        }

        public LayoutBuilder Class(string @class)
        {
            _class = @class;
            return this;
        }

        public LayoutBuilder Add(object content, string space = "auto")
        {
            _elements.Add(new(content, space));
            return this;
        }

        public LayoutBuilder Add(object[] content, string space = "auto")
        {
            _elements.AddRange(content.Select(e => new Element(e, space)));
            return this;
        }

        public LayoutBuilder Debug()
        {
            _debug = true;
            return this;
        }

        public LayoutBuilder Width(string width)
        {
            _width = width;
            return this;
        }

        public LayoutBuilder Height(string height)
        {
            _height = height;
            return this;
        }

        public LayoutBuilder Align(HorizontalAlignment hAlign)
        {
            _hAlignment = hAlign;
            return this;
        }

        public LayoutBuilder Align(VerticalAlignment vAlign)
        {
            _vAlignment = vAlign;
            return this;
        }

        public LayoutBuilder Top()
        {
            return Align(VerticalAlignment.Top);
        }

        public LayoutBuilder Middle()
        {
            return Align(VerticalAlignment.Middle);
        }

        public LayoutBuilder Bottom()
        {
            return Align(VerticalAlignment.Bottom);
        }

        public LayoutBuilder Right()
        {
            return Align(HorizontalAlignment.Right);
        }

        public LayoutBuilder Center()
        {
            return Align(HorizontalAlignment.Center);
        }

        public LayoutBuilder Left()
        {
            return Align(HorizontalAlignment.Left);
        }

        public LayoutBuilder Stretch()
        {
            return Align(HorizontalAlignment.Stretch).Align(VerticalAlignment.Stretch);
        }

        public Div Vertical(params object[] elements)
        {
            _orientation = Orientation.Vertical;
            Add(elements);
            return Render();
        }

        public Div Horizontal(params object[] elements)
        {
            _orientation = Orientation.Horizontal;
            Add(elements);
            return Render();
        }

        public Div Vertical(IEnumerable<object> elements)
        {
            return Vertical(elements.ToArray());
        }

        public Div Horizontal(IEnumerable<object> elements)
        {
            return Horizontal(elements.ToArray());
        }

        private Div Render()
        {
            return !_containerPadding ? new LayoutDivWithoutContainerContainerPadding(this) : new LayoutDiv(this);
        }
    }

    internal class LayoutDivWithoutContainerContainerPadding : LayoutDiv, INoContainerPadding
    {
        internal LayoutDivWithoutContainerContainerPadding(LayoutBuilder builder) : base(builder)
        {

        }
    }

    internal class LayoutDiv : Div
    {
        internal LayoutDiv(LayoutBuilder builder)
        {
            Control BuildElement(LayoutBuilder.Element element)
            {
                if (element._space != "auto" && element._content is Control control)
                {
                    control.Styles["width"] = "-webkit-fill-available";
                }

                var formatted = builder._formatter.Format(element._content.Render()); //todo: this will result in a lot of wrapped 

                return formatted;
            }

            var children = builder._elements.Where(e => e._content != null).ToArray();
            var cn = children.Count();

            var fill = (builder._fill, builder._orientation) switch
            {
                (null, Orientation.Horizontal) => false,
                (null, Orientation.Vertical) => true,
                (not null, _) => builder._fill.Value,
                _ => throw new ArgumentOutOfRangeException()
            };

            var hAlignment = (builder._hAlignment, builder._orientation) switch
            {
                (null, Orientation.Horizontal) => HorizontalAlignment.Left,
                (null, Orientation.Vertical) => HorizontalAlignment.Stretch,
                (not null, _) => builder._hAlignment.Value,
                _ => throw new ArgumentOutOfRangeException()
            };

            var vAlignment = (builder._vAlignment, builder._orientation) switch
            {
                (null, Orientation.Horizontal) => VerticalAlignment.Stretch,
                (null, Orientation.Vertical) => VerticalAlignment.Top,
                (not null, _) => builder._vAlignment.Value,
                _ => throw new ArgumentOutOfRangeException()
            };

            if (builder._debug)
            {
                this.Styles["background-color"] = "red";
            }

            this.Styles["box-sizing"] = "border-box";

            if (cn == 0) return;

            if (cn > 1)
            {
                this.Styles["display"] = "grid";
            }

            var template = string.Join(" ", children.Select(e => e._space));

            this.Styles["justify-items"] = hAlignment switch
            {
                HorizontalAlignment.Right => "end",
                HorizontalAlignment.Left => "start",
                HorizontalAlignment.Center => "center",
                _ => "stretch"
            };
            this.Styles["align-items"] = vAlignment switch
            {
                VerticalAlignment.Bottom => "end",
                VerticalAlignment.Top => "start",
                VerticalAlignment.Middle => "center",
                _ => "stretch"
            };

            if (builder._orientation == Orientation.Horizontal)
            {
                this.Styles["grid-template-columns"] = template;
                if (builder._gap)
                {
                    this.Styles["column-gap"] = "10px";
                }
                if (fill)
                {
                    builder._width ??= "100%";
                    this.Styles["justify-items"] = "stretch";
                }
            }
            else
            {
                this.Styles["grid-template-rows"] = template;
                if (builder._gap)
                {
                    this.Styles["row-gap"] = "10px";
                }
                if (fill)
                {
                    builder._width ??= "100%";
                }
            }

            if (builder._padding)
            {
                this.Styles["padding"] = "10px";
            }

            this.Styles["height"] = builder._height;
            this.Styles["width"] = builder._width ?? "max-content";

            if (!string.IsNullOrEmpty(builder._class))
            {
                this.SetClass(builder._class);
            }

            foreach (var element in children)
            {
                this.VisualTree.Add(BuildElement(element));
            }
        }
    }

    public static class Layout
    {

        public static LayoutBuilder Add(object content, string space = "auto")
        {
            return (new LayoutBuilder()).Add(content, space);
        }

        public static LayoutBuilder Add(object[] content, string space = "auto")
        {
            return (new LayoutBuilder()).Add(content, space);
        }

        public static LayoutBuilder Gap(bool gap = true)
        {
            return (new LayoutBuilder()).Gap(gap);
        }

        public static LayoutBuilder ContainerPadding(bool containerPadding = true)
        {
            return (new LayoutBuilder()).ContainerPadding(containerPadding);
        }

        public static LayoutBuilder Top()
        {
            return (new LayoutBuilder()).Align(VerticalAlignment.Top);
        }

        public static LayoutBuilder Debug()
        {
            return (new LayoutBuilder()).Debug();
        }

        public static LayoutBuilder Padding(bool padding = true)
        {
            return (new LayoutBuilder()).Padding(padding);
        }

        public static LayoutBuilder Middle()
        {
            return (new LayoutBuilder()).Align(VerticalAlignment.Middle);
        }

        public static LayoutBuilder Bottom()
        {
            return (new LayoutBuilder()).Align(VerticalAlignment.Bottom);
        }

        public static LayoutBuilder Right()
        {
            return (new LayoutBuilder()).Align(HorizontalAlignment.Right);
        }

        public static LayoutBuilder Center()
        {
            return (new LayoutBuilder()).Align(HorizontalAlignment.Center);
        }

        public static LayoutBuilder Width(string width)
        {
            return (new LayoutBuilder()).Width(width);
        }

        public static LayoutBuilder Height(string height)
        {
            return (new LayoutBuilder()).Height(height);
        }

        public static LayoutBuilder Fill()
        {
            return (new LayoutBuilder()).Fill();
        }

        public static LayoutBuilder Left()
        {
            return (new LayoutBuilder()).Align(HorizontalAlignment.Left);
        }

        public static LayoutBuilder Class(string @class)
        {
            return (new LayoutBuilder()).Class(@class);
        }

        public static Div Vertical(params object[] elements)
        {
            return (new LayoutBuilder()).Vertical(elements);
        }

        public static Div Horizontal(params object[] elements)
        {
            return (new LayoutBuilder()).Horizontal(elements);
        }

        public static Div Vertical(IEnumerable<object> elements)
        {
            return (new LayoutBuilder()).Vertical(elements.ToArray());
        }

        public static Div Horizontal(IEnumerable<object> elements)
        {
            return (new LayoutBuilder()).Horizontal(elements.ToArray());
        }
    }
}
