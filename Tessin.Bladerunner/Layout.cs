using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using LINQPad;
using LINQPad.Controls;
using Tessin.Bladerunner.Controls;

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
        internal int _gap = 1;
        internal string _padding = null;
        internal bool _fill = false;
        internal string _width = "max-content";
        internal string _height = "max-content"; 
        internal Orientation _orientation = Orientation.Vertical;
        internal HorizontalAlignment _hAlignment = HorizontalAlignment.Left;
        internal VerticalAlignment _vAlignment = VerticalAlignment.Top;
        internal readonly List<Element> _elements = new();

        public LayoutBuilder Padding(string padding = "10px")
        {
            _padding = "10px";
            return this;
        }

        public LayoutBuilder Gap(bool gap = true)
        {
            _gap = gap ? 1 : 0;
            return this;
        }

        public LayoutBuilder Fill()
        {
            _fill = true;
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

        private Div Render()
        {
            return new LayoutDiv(this);
        }
    }

    internal class LayoutDiv : Div
    {
        internal LayoutDiv(LayoutBuilder builder)
        {
            this.Styles["display"] = "grid";

            if (builder._debug)
            {
                this.Styles["background-color"] = "red";
            }

            var template = string.Join(" ", builder._elements.Select(e => e._space));

            this.Styles["justify-items"] = builder._hAlignment switch
            {
                HorizontalAlignment.Right => "end",
                HorizontalAlignment.Left => "start",
                HorizontalAlignment.Center => "center",
                _ => "stretch"
            };
            this.Styles["align-items"] = builder._vAlignment switch
            {
                VerticalAlignment.Bottom => "end",
                VerticalAlignment.Top => "start",
                VerticalAlignment.Middle => "center",
                _ => "stretch"
            };

            if (builder._orientation == Orientation.Horizontal)
            {
                this.Styles["grid-template-columns"] = template;
                this.Styles["grid-template-rows"] = "auto";
                if (builder._gap != 0)
                {
                    this.Styles["column-gap"] = (builder._gap*0.4) + "em";
                }
                if (builder._fill)
                {
                    builder._height = "100%";
                }
            }
            else
            {
                //Vertical:
                this.Styles["grid-template-columns"] = "auto";
                this.Styles["grid-template-rows"] = template;
                if (builder._gap != 0)
                {
                    this.Styles["row-gap"] = (builder._gap * 0.4) + "em";
                }
                if (builder._fill)
                {
                    builder._width = "100%";
                }
            }

            if (builder._gap != 0)
            {
                this.Styles["grid-template-rows"] = template;
            }

            if (!string.IsNullOrEmpty(builder._padding))
            {
                this.Styles["padding"] = builder._padding;
            }

            this.Styles["height"] = builder._height;
            this.Styles["width"] = builder._width;

            foreach (var element in builder._elements)
            {
                if (element._space != "auto" && element._content is Control control)
                {
                    control.Styles["width"] = "inherit";
                }
                this.VisualTree.Add(builder._formatter.Format(element._content));
            }
        }
    }

    public static class Layout2
    {
        public static LayoutBuilder Gap(bool gap = true)
        {
            return (new LayoutBuilder()).Gap(gap);
        }

        public static LayoutBuilder Top()
        {
            return (new LayoutBuilder()).Align(VerticalAlignment.Top);
        }

        public static LayoutBuilder Debug()
        {
            return (new LayoutBuilder()).Debug();
        }

        public static LayoutBuilder Padding(string padding = "10px")
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

        public static Div Vertical(params object[] elements)
        {
            return (new LayoutBuilder()).Vertical(elements);
        }

        public static Div Horizontal(params object[] elements)
        {
            return (new LayoutBuilder()).Horizontal(elements);
        }
    }

    public static class Layout
    {
        private static object _Vertical(bool withGaps, IEnumerable<object> elements, HorizontalAlignment alignment)
        {
            var @class = $"vrun vrun-{alignment.ToString().ToLower()}";
            elements = elements.Where(e => e != null);
            if (!withGaps)
            {
                return WithClass(LINQPad.Util.VerticalRun(elements), @class);
            }
            return WithClass(LINQPad.Util.VerticalRun(elements.Select(e => LINQPad.Util.WithStyle(e, "margin-bottom:0.4em;display:block;"))), @class);
        }

        public static object Vertical(bool withGaps, IEnumerable<object> elements, HorizontalAlignment alignment = HorizontalAlignment.Left)
        {
            return _Vertical(withGaps, elements, alignment);
        }

        public static object Vertical(bool withGaps, params object[] elements)
        {
            return _Vertical(withGaps, elements, HorizontalAlignment.Left);
        }

        public static object WithClass(object data, string @class)
        {
            Type t = typeof(Util)
                .Assembly.GetType("LINQPad.ObjectGraph.Highlight");
            var ctor = t.GetConstructors()[0];
            return ctor.Invoke(new object[] { data, @class, null });
        }

        private static object _Horizontal(bool withGaps, IEnumerable<object> elements)
        {
            elements = elements.Where(e => e != null);
            if (!withGaps)
            {
                return WithClass(Util.VerticalRun(elements), "hrun");
            }
            return WithClass(LINQPad.Util.VerticalRun(elements.Select(e => LINQPad.Util.WithStyle(e, "margin-right:0.4em;display:block;"))), "hrun");
        }

        public static object Horizontal(bool withGaps, IEnumerable<object> elements)
        {
            return _Horizontal(withGaps, elements);
        }


        public static object Horizontal(bool withGaps, params object[] elements)
        {
            return _Horizontal(withGaps, elements);
        }

        //[Obsolete]
        //public static object DumpMatrix<T, T1, T2>(IQueryable<T> raw, Expression<Func<T, T1>> verticalExpr, Expression<Func<T, T2>> horizontalExpr, Func<IEnumerable<T>, object> cellRenderer) where T1 : IComparable where T2 : IComparable
        //{
        //    var verticalKeys = raw.GroupBy(verticalExpr).Select(e => e.Key).Distinct().ToList();
        //    var horizontalKeys = raw.GroupBy(horizontalExpr).Select(e => e.Key).Distinct().ToList();

        //    var verticalFunc = verticalExpr.Compile();
        //    var horizontalFunc = horizontalExpr.Compile();

        //    var table = new Table();

        //    table.Rows.Add(new TableRow(true, horizontalKeys.Select(e => new TableCell(true, new Literal(e.ToString()))).ToArray().Prepend(new TableCell(true, new Literal("")))));

        //    foreach (var verticalKey in verticalKeys)
        //    {
        //        table.Rows.Add(new TableRow(false,
        //            horizontalKeys.Select(horizontalKey => cellRenderer(raw.Where(e => horizontalFunc(e).Equals(horizontalKey) && verticalFunc(e).Equals(verticalKey)))).Select(e => new TableCell(false, new Literal(e.ToString()))).ToArray()
        //                .Prepend(new TableCell(true, new Literal(verticalKey.ToString())))
        //        ));
        //    }

        //    return table;
        //}
	}
}
