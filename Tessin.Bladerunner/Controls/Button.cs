using System;
using System.Collections.Generic;
using System.Text;
using LINQPad.Controls;

namespace Tessin.Bladerunner.Controls
{
    public enum ButtonStyle
    {
        Primary,
        Secondary,
        Danger
    }

    public class Button : LINQPad.Controls.Button
    {
        public Button(string text = "", Action<Button> onClick = null, ButtonStyle buttonStyle = ButtonStyle.Primary) : base(text)
        {
            Button obj = this;

            this.AddClass(buttonStyle.ToString().ToLower());

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
