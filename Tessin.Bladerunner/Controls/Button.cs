using System;
using System.Collections.Generic;
using System.Text;
using LINQPad.Controls;

namespace Tessin.Bladerunner.Controls
{
    public class Button : LINQPad.Controls.Button
    {
        public Button(string initialText = "", Action<Button> onClick = null) : base(initialText, onClick)
        {
            Button obj = this;
            if (onClick != null)
            {
                Click += delegate
                {
                    onClick(obj);
                };
            }
        }
    }
}
