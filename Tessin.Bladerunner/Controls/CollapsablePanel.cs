﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using LINQPad;
using LINQPad.Controls;

namespace Tessin.Bladerunner.Controls
{
    public class CollapsablePanel : Div
    {
        private bool _isOpen = false;

        public CollapsablePanel(string header, object body)
        {
            this.SetClass("collapsable-panel");

            var dcBody = new DumpContainer { Content = body };
            var divBody = new Div(dcBody);
            divBody.SetClass("collapsable-panel--body");

            var divHeader = new Button(header, btn =>
            {

                if(_isOpen) {
                    this.AddClass("collapsable-panel-open");
                } else {
                    this.RemoveClass("collapsable-panel-open");
                }
                _isOpen = !_isOpen;
            });

            divHeader.SetClass("collapsable-panel--header");

            var divContainer = new Div(divHeader, divBody);
            divContainer.SetClass("collapsable-panel--container");

            this.VisualTree.Add(divContainer);
        }
    }
}
