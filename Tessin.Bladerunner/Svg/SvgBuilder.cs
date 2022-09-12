using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Xml;
using Svg;
using Svg.Transforms;

namespace Tessin.Bladerunner.Svg
{
    public class SvgBuilder
    {
        public SvgElement RootElement { get; set; }

        private readonly SvgElement _layer;

        private readonly SvgBuilder _parent;

        private readonly HashSet<string> _insertedScripts = new();

        private SvgElement _last;

        public static SvgUnit Px(float value)
        {
            return new SvgUnit(SvgUnitType.Pixel, value);
        }

        public SvgBuilder(SvgElement doc, SvgBuilder parent = null)
        {
            _layer = doc;

            if (parent == null)
            {
                RootElement = doc;
            }
            else
            {
                _parent = parent;
            }
        }

        public static SvgBuilder New()
        {
            return new SvgBuilder(new SvgDocument() { Width = SvgUnit.None, Height = SvgUnit.None});
        }

        public static SvgElement Make(Func<SvgBuilder, SvgBuilder> builder)
        {
            var group = new SvgGroup();
            builder(new SvgBuilder(group));
            return group;
        }

        public SvgBuilder Place(float x, float y, SvgElement el)
        {
            el.Transforms = el.Transforms ?? new SvgTransformCollection();
            el.Transforms.Add(new SvgTranslate(x, y));
            _last = el;
            _layer.Children.Add(el);
            return this;
        }

        public SvgBuilder Group(float x, float y, Func<SvgBuilder, SvgBuilder> group,
            Action<SvgAttributeBuilder> attr = null)
        {
            _last = new SvgGroup()
            {
                Transforms = new SvgTransformCollection()
                {
                    new SvgTranslate(x, y)
                }
            };
            attr?.Invoke(new SvgAttributeBuilder(_last));
            group(new SvgBuilder(_last));
            _layer.Children.Add(_last);
            return this;
        }

        public SvgElement Last()
        {
            return _last;
        }

        public SvgBuilder Line(float x0, float y0, float x1, float y1, Action<SvgAttributeBuilder> attr = null)
        {
            _last = new SvgLine()
            {
                StartX = Px(x0),
                StartY = Px(y0),
                EndX = Px(x1),
                EndY = Px(y1),
            };
            attr?.Invoke(new SvgAttributeBuilder(_last));
            _layer.Children.Add(_last);
            return this;
        }

        public SvgBuilder Rect(float x, float y, float width, float height, Action<SvgAttributeBuilder> attr = null)
        {
            _last = new SvgRectangle()
            {
                Width = Px(width),
                Height = Px(height),
                X = Px(x),
                Y = Px(y)
            };
            attr?.Invoke(new SvgAttributeBuilder(_last));
            _layer.Children.Add(_last);
            return this;
        }

        public SvgBuilder Text(float x, float y, string text, Action<SvgAttributeBuilder> attr = null)
        {
            _last = new SvgText(text)
            {
                X = new SvgUnitCollection()
                {
                    Px(x)
                },
                Y = new SvgUnitCollection()
                {
                    Px(y)
                },
                Fill = new SvgColourServer(Color.Black),
                FontSize = new SvgUnit(SvgUnitType.Em, 1f),
                FontFamily = "Segoe UI",
                FontStyle = SvgFontStyle.Normal,
                FontWeight = SvgFontWeight.Normal,
                TextAnchor = SvgTextAnchor.Start
            };
            attr?.Invoke(new SvgAttributeBuilder(_last));
            _layer.Children.Add(_last);
            return this;
        }

        public SvgBuilder Icon(string iconSvg, float x = 0f, float y = 0f, Color color = new(),
            Action<SvgAttributeBuilder> attr = null)
        {
            SvgDocument icon = SvgDocument.FromSvg<SvgDocument>(iconSvg);
            icon.X = Px(x);
            icon.Y = Px(y);
            icon.Fill = new SvgColourServer(color);
            attr?.Invoke(new SvgAttributeBuilder(icon));
            _layer.Children.Add(icon);
            _last = icon;
            return this;
        }

        public SvgBuilder Attributes(Action<SvgAttributeBuilder> attr)
        {
            attr?.Invoke(new SvgAttributeBuilder(_last));
            return this;
        }

        public SvgBuilder Tooltip(float x, float y, Func<SvgBuilder, SvgBuilder> elementBuilder,
            Func<SvgBuilder, SvgBuilder> tooltipBuilder)
        {
            GetRootBuilder().InsertScript("Tooltip");

            this.Group(x, y, group =>
            {
                return tooltipBuilder(elementBuilder(@group).Attributes(a => a.Name("tooltip-element")))
                    .Attributes(a => a.Name("tooltip-tooltip").Visible(false));
            }, a => a.Name("tooltip"));

            return this;
        }

        public SvgBuilder Overlay(float x, float y, float width, float height, Func<SvgBuilder, SvgBuilder> overlay)
        {
            this.Group(x, y, group => overlay(@group
                .Rect(5, 5, width, height, a => a.Opacity(0.5f).Fill(Color.LightGray))
                .Rect(0, 0, width, height, a => a.Fill(Color.Aquamarine))), a => a.Name("overlay"));

            return this;
        }

        private void InsertScript(string key)
        {
            if (_insertedScripts.Contains(key)) return;
            var script = new SvgScript()
            {
                Content = SvgScripts.ResourceManager.GetString(key)
            };
            RootElement.Children.Add(script);
            _insertedScripts.Add(key);
        }

        private SvgBuilder GetRootBuilder()
        {
            var iterator = this;
            while (iterator._parent != null)
            {
                iterator = iterator._parent;
            }

            return iterator;
        }

        public override string ToString()
        {
            using var stringWriter = new StringWriter();
            using var writer = new XmlTextWriter(stringWriter);
            this.RootElement.Write(writer);
            return stringWriter.ToString();
        }
    }
}