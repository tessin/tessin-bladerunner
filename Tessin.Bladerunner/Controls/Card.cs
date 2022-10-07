using LINQPad;
using LINQPad.Controls;
using System.Collections.Generic;

namespace Tessin.Bladerunner.Controls
{
    public enum CardStyle
    {
        Info,
        Warning
    }

    public class Card : Div
    {
        public Card(object content, string title = null, CardStyle style = CardStyle.Info) : base()
        {
            this.SetClass($"card card-{style.ToString().ToLower()}");

            List<Control> controls = new();

            if (!string.IsNullOrEmpty(title))
            {
                var titleDiv = new Div(new Literal(title));
                titleDiv.SetClass("card--title");
                controls.Add(titleDiv);
            }

            var dc = new DumpContainer
            {
                Content = content.Render()
            };
            controls.Add(dc);

            this.VisualTree.Add(new Div(controls));
        }
    }
}
