using System;
using System.Collections.Generic;
using System.Text;
using LINQPad;
using LINQPad.Controls;

namespace Tessin.Bladerunner.Controls
{
    public class FilterPanel : Div  
    {
        public FilterPanel(object header, object body)
        {
            this.SetClass("filter-panel");

            var dcHeader = new DumpContainer {Content = header};
            var divHeader = new Div(dcHeader);
            divHeader.SetClass("filter-panel--header");

            var dcBody = new DumpContainer {Content = body};
            var divBody = new Div(dcBody);
            divBody.SetClass("filter-panel--body");

            var divContainer = new Div(divHeader, divBody);
            divContainer.SetClass("filter-panel--container");

            this.VisualTree.Add(divContainer);
        }
    }
}
