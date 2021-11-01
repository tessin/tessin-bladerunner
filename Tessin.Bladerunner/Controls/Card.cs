using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LINQPad;
using LINQPad.Controls;

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
            var dc = new DumpContainer {Content = content};

            if(!string.IsNullOrEmpty(title))
            {
                var titleDiv = new Div(new Literal(title));
                titleDiv.SetClass("card--title");
                this.VisualTree.Add(titleDiv);
            }

            this.VisualTree.Add(dc);
        }
    }
}
