using LINQPad;
using LINQPad.Controls;
using System;

namespace Tessin.Bladerunner.Controls
{
    public class EmptySpan : Span
    {
        public EmptySpan(string text) : base(text)
        {
        }
    }

    public class DefaultContentFormatter : IContentFormatter
    {
        public string IntegerFormat { get; set; } = "N0";

        public string DecimalFormat { get; set; } = "#,#.#####";

        public string DateFormat { get; set; } = "yyyy-MM-dd";

        public Control Format(object content, Func<Control, bool, Control> wrapper = null, object emptyContent = null)
        {
            return (new DefaultContentFormatter())._Format(content, wrapper, emptyContent ?? "", false);
        }

        private Control _Format(object content, Func<Control, bool, Control> wrapper, object emptyContent, bool formattingEmpty)
        {
            wrapper ??= ((c, e) => c);

            Control Wrapper(Control inner)
            {
                return wrapper(inner, formattingEmpty);
            }

            Control EmptyFormatter()
            {
                if (!formattingEmpty)
                {
                    return _Format(emptyContent, wrapper, null, true);
                }
                return Wrapper(new EmptySpan(content.ToString()));
            }

            return content switch
            {
                null or "" => EmptyFormatter(),
                string strContent => Wrapper(new Span(strContent)),
                long and 0 => EmptyFormatter(),
                long longContent => Wrapper(new Span(longContent.ToString(IntegerFormat))),
                int and 0 => EmptyFormatter(),
                int intContent => Wrapper(new Span(intContent.ToString(IntegerFormat))),
                double and 0 => EmptyFormatter(),
                double doubleContent => Wrapper(new Span(doubleContent.ToString(DecimalFormat).TrimEnd(".00"))),
                decimal and 0 => EmptyFormatter(),
                decimal decimalContent => Wrapper(new Span(decimalContent.ToString(DecimalFormat).TrimEnd(".00"))),
                bool boolContent => Wrapper(boolContent ? new Icon(Icons.CheckBold, theme: Theme.Success) : Icon.Empty()),
                DateTime dateContent => Wrapper(new Span(dateContent.ToString(DateFormat))),
                DateTimeOffset dateTimeOffset => Wrapper(new Span(dateTimeOffset.ToString(DateFormat))),
                Guid guid => guid == Guid.Empty ? EmptyFormatter() : Wrapper(new Span(guid.ToString())),
                Control control => Wrapper(control),
                DumpContainer dumpContainer => Wrapper(dumpContainer),
                _ => Wrapper(new DumpContainer() { Content = content })
            };
        }

        public Control Format(object content)
        {
            return _Format(content, null, "", false);
        }
    }
}
