using System;
using System.Collections.Generic;
using System.Text;
using LINQPad.Controls;

namespace Tessin.Bladerunner
{
    public static class Typography
    {
        public static Span Small(string content)
        {
            var span = new Span(content);
            span.SetClass("small");
            return span;
        }

        public static Span Error(string content)
        {
            var span = new Span(content);
            span.SetClass("error");
            return span;
        }

        public static Control H1(string content)
        {
            var control = new Control("h1", content);
            control.SetClass("default");
            return control;
        }

        public static Control H2(string content)
        {
            var control = new Control("h2", content);
            control.SetClass("default");
            return control;
        }

        public static Control P(string content)
        {
            var control = new Control("p", content);
            control.SetClass("default");
            return control;
        }
    }
}
