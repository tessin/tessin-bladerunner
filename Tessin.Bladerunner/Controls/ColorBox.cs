using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LINQPad.Controls;

namespace Tessin.Bladerunner.Controls
{
    public class ColorBox : TextBox
    {
        public ColorBox(string intitialValue = null, string width = "30px", Action<TextBox> onTextInput = null) : base(intitialValue??"#000000", width, onTextInput)
        {
            this.HtmlElement.SetAttribute("type", "color");
        }
    }
}