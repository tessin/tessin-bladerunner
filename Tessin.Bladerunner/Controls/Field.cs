using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using LINQPad;
using LINQPad.Controls;

namespace Tessin.Bladerunner.Controls
{
    public class Field : DumpContainer
    {
        private Div _divError;

        public Field(string label, Control input, string description = "", Func<Control,object> helper = null, bool required = false)
        {
            this.Style = "width:-webkit-fill-available;";

            if (input is not DateBox && input is not SearchBox)
            {
                input.HtmlElement.SetAttribute("required", "required");
            }

            var dcHelper = new DumpContainer();

            Span _description = null;
            if(!string.IsNullOrEmpty(description))
            {
                _description = new Span("");
                _description.HtmlElement.SetAttribute("title", description);
                _description.SetClass("field--header-description");
            }

            Span _label = new Span(label + (required ? "*" : ""));
            _label.SetClass("field--header-label");

            var divHeader = new Div((new Control[] {_label, _description, dcHelper }).Where(e => e != null).ToArray());
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
            _divError.SetClass("error");

            var divContainer = new Div(input, divHeader, _divError);
            divContainer.SetClass("field");

            this.Content = divContainer;
        }

        public void SetError(string message)
        {
            _divError.HtmlElement.InnerText = message; 
        }
    }
}
