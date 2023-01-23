﻿using System;

namespace Tessin.Bladerunner.Controls
{
    public class DateBox : LINQPad.Controls.TextBox
    {
        public DateBox(DateTime? initialValue = null, bool showTime = false, string width = "-webkit-fill-available", Action<LINQPad.Controls.TextBox> onTextInput = null) : base("", width, onTextInput)
        {
            if (initialValue != null)
            {
                this.Text = initialValue.Value.ToString(showTime ? "yyyy-MM-ddTHH:mm" : "yyyy-MM-dd");
            }
            this.HtmlElement.SetAttribute("type", showTime ? "datetime-local" : "date");
        }

        public DateTime? SelectedDate
        {
            get
            {
                if (string.IsNullOrEmpty(this.Text)) return null;
                if (DateTime.TryParse(this.Text, out var result))
                {
                    return DateTime.SpecifyKind(result, DateTimeKind.Utc);
                }
                return null;
            }
        }
    }
}
