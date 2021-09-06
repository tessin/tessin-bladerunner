using LINQPad;
using LINQPad.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tessin.Bladerunner.Controls
{
    public class ContentFormatter : IContentFormatter
    {
        public static Control Format(object content, Func<Control, bool, Control> wrapper = null, object emptyContent = null)
        {
            return (new ContentFormatter())._Format(content, wrapper, emptyContent ?? "", false);
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
                return Wrapper(new Literal(content.ToString()));
            }

            return content switch
            {
                null or "" => EmptyFormatter(),
                string strContent => Wrapper(new Literal(strContent)),
                long and 0 => EmptyFormatter(),
                long longContent => Wrapper(new Literal(longContent.ToString("N0"))),
                int and 0 => EmptyFormatter(),
                int intContent => Wrapper(new Literal(intContent.ToString("N0"))),
                double and 0 => EmptyFormatter(),
                double doubleContent => Wrapper(new Literal(doubleContent.ToString("N2"))),
                bool boolContent => Wrapper(new Literal(boolContent.ToString())),
                DateTime dateContent => Wrapper(new Literal(dateContent.ToString("yyyy-MM-dd"))),
                _ => Wrapper(new DumpContainer() { Content = content })
            };
        }

        public Control Format(object content)
        {
            return _Format(content, null, "", false);
        }
    }
}
