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
        public DateBox(DateTime? initialValue = null, bool showTime = false, string width = "12em", Action<TextBox> onTextInput = null) : base("", width, onTextInput)
        {
            if (initialValue != null)
            {
                this.Text = initialValue.Value.ToString("yyyy-MM-dd");
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
                    return result;
                }
                return null;
            }
        }
    }
}
