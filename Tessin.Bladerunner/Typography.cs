using System;
using System.Collections.Generic;
using System.Text;
using LINQPad.Controls;

namespace Tessin.Bladerunner
{
    public static class Typography
    {
        public static object Small(string content)
        {
            var span = new Span(content);
            span.SetClass("small");
            return span;
        }
    }
}
