﻿using LINQPad;
using LINQPad.Controls;
using Tessin.Bladerunner.Blades;

namespace Tessin.Bladerunner.Controls
{
    public class HeaderPanel : Div, INoContainerPadding
    {
        public HeaderPanel(object header, object body, string width = null)
        {
            this.SetClass("header-panel");

            if (width != null)
            {
                this.Styles["width"] = width;
            }

            var dcHeader = new DumpContainer { Content = header.Render() };
            var divHeader = new Div(dcHeader);
            divHeader.SetClass("header-panel--header");

            var dcBody = new DumpContainer();

            if (body != null)
            {
                if (body is RefreshPanel)
                {
                    dcBody.Content = body;
                }
                else
                {
                    ControlExtensions.AddPadding(dcBody, body.Render()).GetAwaiter().GetResult();
                }
            }

            var divBody = new Div(dcBody);
            divBody.SetClass("header-panel--body");

            var divContainer = new Div(divHeader, divBody);
            divContainer.SetClass("header-panel--container");

            this.VisualTree.Add(divContainer);
        }
    }
}
