using System;
using System.Collections.Generic;
using System.Text;
using LINQPad.Controls;

namespace Tessin.Bladerunner.Controls
{
    public class Button : LINQPad.Controls.Button
    {
        public Button(string text = "", Action<Button> onClick = null) : base(text)
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
