using LINQPad.Controls;
using System;

namespace Tessin.Bladerunner
{
    public static partial class Extensions
    {
        public static void Remember(this Control control, string id)
        {
            var key = (LINQPad.Util.CurrentQueryPath + ":" + id).GetHashCode().ToString();
            var previous = LINQPad.Util.LoadString(key);

            if (control is TextBox textBox)
            {
                if (previous != null)
                {
                    textBox.Text = previous;
                }
                textBox.TextInput += (_, __) =>
                {
                    LINQPad.Util.SaveString(key, textBox.Text);
                };
            }
            if (control is CheckBox checkBox)
            {
                if (previous != null)
                {
                    checkBox.Checked = Convert.ToBoolean(previous);
                }
                checkBox.Click += (_, __) =>
                {
                    LINQPad.Util.SaveString(key, checkBox.Checked.ToString());
                };
            }
            if (control is DataListBox dataListBox)
            {
                if (previous != null)
                {
                    dataListBox.Text = previous;
                }
                dataListBox.TextInput += (_, __) =>
                {
                    LINQPad.Util.SaveString(key, dataListBox.Text);
                };
            }
            if (control is TextArea textArea)
            {
                if (previous != null)
                {
                    textArea.Text = previous;
                }

                textArea.TextInput += (_, __) =>
                {
                    LINQPad.Util.SaveString(key, textArea.Text);
                };
            }
            if (control is FilePicker filePicker)
            {
                if (previous != null)
                {
                    filePicker.Text = previous;
                }

                filePicker.TextInput += (_, __) =>
                {
                    LINQPad.Util.SaveString(key, filePicker.Text);
                };
            }
        }
    }
}
