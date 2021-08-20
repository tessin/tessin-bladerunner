using System;
using System.Collections.Generic;
using System.Text;
using LINQPad;
using LINQPad.Controls;

namespace Tessin.Bladerunner.Controls
{
    public class HeaderPanel : Div  
    {
        public HeaderPanel(object header, object body)
        {
            this.SetClass("header-panel");

            var dcHeader = new DumpContainer {Content = header};
            var divHeader = new Div(dcHeader);
            divHeader.SetClass("header-panel--header");

            var dcBody = new DumpContainer {Content = body};
            var divBody = new Div(dcBody);
            divBody.SetClass("header-panel--body");

            var divContainer = new Div(divHeader, divBody);
            divContainer.SetClass("header-panel--container");

            this.VisualTree.Add(divContainer);
        }
    }
}
