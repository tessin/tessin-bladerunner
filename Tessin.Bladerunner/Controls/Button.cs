using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Mapping;
using System.Text;
using LINQPad.Controls;

namespace Tessin.Bladerunner.Controls
{

    public class Button : LINQPad.Controls.Button
    {
        public Func<bool> Validate { get; set; }

        public Button(string text = "", Action<Button> onClick = null, Theme theme = Theme.Primary, bool enabled = true) : base(text)
        {
            Button obj = this;

            this.Enabled = enabled;

            this.AddClass("theme-"+Utils.SplitCamelCase(theme.ToString()).Replace(" ", "-").ToLower());

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
