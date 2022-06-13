using System.Drawing;
using Svg;

namespace Tessin.Bladerunner.Svg
{
   public class SvgAttributeBuilder
    {

        private readonly SvgElement _element;

        public SvgAttributeBuilder(SvgElement element)
        {
            _element = element;
        }

        public SvgAttributeBuilder Fill(Color color)
        {
            _element.Fill = new SvgColourServer(color);
            return this;
        }
        
        public SvgAttributeBuilder Fill(string color)
        {
            var converter = new SvgColourConverter();

            if (converter.ConvertFrom(color) is Color _color)
            {
                _element.Fill = new SvgColourServer(_color);
            }
            
            return this;
        }

        public SvgAttributeBuilder Id(string id)
        {
            _element.ID = id;
            return this;
        }

        public SvgAttributeBuilder Name(string name)
        {
            _element.CustomAttributes["name"] = name;
            return this;
        }

        public SvgAttributeBuilder Visible(bool value)
        {
            _element.Visibility = value ? "visible" : "hidden";
            return this;
        }

        public SvgAttributeBuilder Anchor(SvgTextAnchor anchor)
        {
            _element.TextAnchor = anchor;
            return this;
        }

        public SvgAttributeBuilder Stroke(Color color, float width)
        {
            _element.Stroke = new SvgColourServer(color);
            _element.StrokeWidth = SvgBuilder.Px(width);
            return this;
        }

        public SvgAttributeBuilder Opacity(float opacity)
        {
            _element.Opacity = opacity;
            return this;
        }

        public SvgAttributeBuilder FontSize(float size)
        {
            _element.FontSize = SvgBuilder.Px(size);
            return this;
        }

        public SvgAttributeBuilder FillWithHorizontalFadeOut(Color backgroundColor, Color strokeColor, float strokeWidth)
        {
            _element.Fill = new SvgLinearGradientServer()
            {
                ID = "FadeOutGradient",
                X1 = 0f,
                X2 = 1f,
                Y1 = 0f,
                Y2 = 0f,
                Stops =
                {
                    new SvgGradientStop(new SvgUnit(SvgUnitType.Percentage, 0f), backgroundColor),
                    new SvgGradientStop(new SvgUnit(SvgUnitType.Percentage, 100f), Color.White)
                    {
                        Opacity = 0f
                    }
                }
            };
            return this;
        }

    }
}