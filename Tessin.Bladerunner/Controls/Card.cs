using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LINQPad;
using LINQPad.Controls;

namespace Tessin.Bladerunner.Controls
{
    public class Card : Div
    {
        public Card(object content) : base()
        {
            this.SetClass("card");
            var dc = new DumpContainer {Content = content};
            this.VisualTree.Add(dc);
        }
    }
}
