using LINQPad.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tessin.Bladerunner.Controls
{
    public interface IContentFormatter
    {
        Control Format(object content);
    }
}
