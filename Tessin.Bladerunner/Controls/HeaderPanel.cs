using System;
using System.Collections.Generic;
using System.Text;
using LINQPad;
using LINQPad.Controls;
using Tessin.Bladerunner.Blades;

namespace Tessin.Bladerunner.Controls
{
    public class HeaderPanel : Div, INoPadding
    {
        public HeaderPanel(object header, object body)
        {
            this.SetClass("header-panel");

            var dcHeader = new DumpContainer {Content = header};
            var divHeader = new Div(dcHeader);
            divHeader.SetClass("header-panel--header");

            var dcBody = new DumpContainer();
            ControlExtensions.AddPadding(dcBody, body);
            var divBody = new Div(dcBody);
            divBody.SetClass("header-panel--body");

            var divContainer = new Div(divHeader, divBody);
            divContainer.SetClass("header-panel--container");

            this.VisualTree.Add(divContainer);
        }
    }
}
