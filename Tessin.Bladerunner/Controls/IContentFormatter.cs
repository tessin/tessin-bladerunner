using LINQPad.Controls;
using System;

namespace Tessin.Bladerunner.Controls
{
    public interface IContentFormatter
    {
        Control Format(object content, Func<Control, bool, Control> wrapper = null, object emptyContent = null);
    }
}
