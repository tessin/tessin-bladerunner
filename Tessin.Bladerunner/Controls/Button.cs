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
        public Func<bool> Validate { get; set; }

        public Button(string text = "", Action<Button> onClick = null, ButtonStyle style = ButtonStyle.Primary) : base(text)
        {
            Button obj = this;

            this.AddClass(style.ToString().ToLower());

            if (onClick != null)
            {
                Click += delegate
                {
                    if(Validate==null || Validate())
                    {
                        onClick(obj);
                    }
                };
            }
        }
    }
}
