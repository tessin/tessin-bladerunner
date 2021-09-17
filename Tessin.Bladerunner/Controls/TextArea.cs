using System;
using System.Collections.Generic;
using System.Text;

namespace Tessin.Bladerunner.Controls
{
    public class TextArea : LINQPad.Controls.TextArea
    {
        public TextArea(string initialText = "", int columns = 40, Action<LINQPad.Controls.TextArea> onTextInput = null) : base(initialText, columns, onTextInput)
        {
        }
    }
}
