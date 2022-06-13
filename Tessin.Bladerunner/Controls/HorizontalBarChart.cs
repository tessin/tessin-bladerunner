using System.Linq;
using LINQPad.Controls;
using Tessin.Bladerunner.Svg;

namespace Tessin.Bladerunner.Controls
{
    public class HorizontalBarChart : Control
    {
        public HorizontalBarChart(Point[] points, float width = 200, float barHeight = 20) : base("div")
        {
            var max = points.Select(e => e.Value).Max();
            
            var builder = SvgBuilder.New();

            float y = 0;
            foreach (var point in points)
            {
                // builder.Tooltip(
                //     0, y,
                //     e => e.Rect(0, 0, (float)(point.Value / max * width), barHeight, x => x.Fill(point.Color)),
                //     e => e.Overlay(25, 0, 300, 50, overlay => overlay.Text(10, 20, point.Label))
                // );
                builder.Rect(0, y, (float)(point.Value / max * width), barHeight, x => x.Fill(point.Color));
                y += barHeight;
            }
            
            this.HtmlElement.InnerHtml = builder.ToString();
        }
        
        public class Point
        {
            public double Value { get; }
            public string Label { get; }
            public string Color { get; }

            public Point(double value, string label, string color)
            {
                Value = value;
                Label = label;
                Color = color;
            }
        }
    }
}
