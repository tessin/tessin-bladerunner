using LINQPad.Controls;
using System;
using System.Runtime.Versioning;

namespace Tessin.Bladerunner.Controls
{
    public class CheckBox : LINQPad.Controls.CheckBox
    {
        public CheckBox(string text = "", bool isChecked = false, Action<LINQPad.Controls.CheckBox> onClick = null) : base(text, isChecked, onClick)
        {
            
        }

        public CheckBox(Control captionControl, bool isChecked = false, Action<LINQPad.Controls.CheckBox> onClick = null) : base(captionControl, isChecked, onClick)
        {
        }

        protected CheckBox(string type, Control captionControl, bool isChecked, Action<LINQPad.Controls.CheckBox> onClick) : base(type, captionControl, isChecked, onClick)
        {
        }
    }
}
