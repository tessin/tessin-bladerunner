using LINQPad;
using LINQPad.Controls;
using System;
using System.Linq;

namespace Tessin.Bladerunner.Controls
{
    public class Field : DumpContainer //todo: why is this a DumpContainer? this.Content is never updated
    {
        private Div _divError;

        public Field(string label, Control input, string description = "", Func<Control, object> helper = null, bool required = false)
        {
            this.Style = "width:-webkit-fill-available;";

            if (input is not DateBox && input is not SearchBox && input is not FileBox)
            {
                input.HtmlElement.SetAttribute("required", "required");
            }

            var dcHelper = new DumpContainer();

            Span _description = null;
            if (!string.IsNullOrEmpty(description))
            {
                _description = new Span("");
                _description.HtmlElement.SetAttribute("title", description);
                _description.SetClass("field--header-description");
            }

            Span _label = new Span(label + (required ? "*" : ""));
            _label.SetClass("field--header-label");

            var divHeader = new Div((new Control[] { _label, _description, dcHelper }).Where(e => e != null).ToArray());
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

            var divContainer = new Div(input, divHeader, _divError);
            divContainer.AddClass("field");

            this.Content = divContainer;
        }

        public void SetError(string message)
        {
            _divError.HtmlElement.InnerText = message;
        }
    }
}
