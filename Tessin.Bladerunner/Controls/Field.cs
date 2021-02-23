using System;
using System.Collections.Generic;
using System.Text;
using LINQPad;
using LINQPad.Controls;

namespace Tessin.Bladerunner.Controls
{
    public class Field : DumpContainer
    {   
        public Field(string label, Control input, string description, Func<Control,object> helper = null)
        {
            var dcHelper = new DumpContainer();
            var divHeader = new Div(new Span(label), dcHelper);
            divHeader.SetClass("field--header");

            if (helper != null)
            {
                input.OnUpdate(() =>
                {
                    dcHelper.Content = helper(input);
                });
                dcHelper.Content = helper(input);
            }

            var divContainer = new Div(divHeader, input);
            divContainer.SetClass("field");

            this.Content = divContainer;
        }
    }
}
