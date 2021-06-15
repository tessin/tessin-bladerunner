using System;
using System.Collections.Generic;
using System.Text;
using LINQPad;
using LINQPad.Controls;

namespace Tessin.Bladerunner.Controls
{
    public class Field : DumpContainer
    {
        private Div _divError;

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

            _divError = new Div();
            _divError.SetClass("field--error");

            var divContainer = new Div(divHeader, input, _divError);
            divContainer.SetClass("field");

            this.Content = divContainer;
        }

        public void SetError(string message)
        {
            _divError.HtmlElement.InnerText = message; 
        }
    }
}
