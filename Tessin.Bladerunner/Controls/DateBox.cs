using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LINQPad.Controls;

namespace Tessin.Bladerunner.Controls
{
    public class DateBox : TextBox
    {
        public DateBox(DateTime? intitialValue = null, string width = "10em", Action<TextBox> onTextInput = null) : base("", width, onTextInput)
        {
            //todo: handle initialValue
            this.HtmlElement.SetAttribute("type","date");
        }
    }
}
