using LINQPad;
using LINQPad.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tessin.Bladerunner.Controls
{
    public class DefaultContentFormatter : IContentFormatter
    {
        public static Control Format(object content, Func<Control, bool, Control> wrapper = null, object emptyContent = null)
        {
            return (new DefaultContentFormatter())._Format(content, wrapper, emptyContent ?? "", false);
        }

        private Control _Format(object content, Func<Control, bool, Control> wrapper, object emptyContent, bool formattingEmpty)
        {
            wrapper = wrapper ?? ((c, e) => c);

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
                return Wrapper(new Span(content.ToString()));
            }

            return content switch
            {
                null or "" => EmptyFormatter(),
                string strContent => Wrapper(new Span(strContent)),
                long and 0 => EmptyFormatter(),
                long longContent => Wrapper(new Span(longContent.ToString("N0"))),
                int and 0 => EmptyFormatter(),
                int intContent => Wrapper(new Span(intContent.ToString("N0"))),
                double and 0 => EmptyFormatter(),
                double doubleContent => Wrapper(new Span(doubleContent.ToString("N2").TrimEnd(".00"))),
                decimal and 0 => EmptyFormatter(),
                decimal decimalContent => Wrapper(new Span(decimalContent.ToString("N2").TrimEnd(".00"))),
                bool boolContent => Wrapper(new Span(boolContent.ToString())),
                DateTime dateContent => Wrapper(new Span(dateContent.ToString("yyyy-MM-dd"))),
                _ => Wrapper(new DumpContainer() { Content = content })
            };
        }

        public Control Format(object content)
        {
            return _Format(content, null, "", false);
        }
    }
}
