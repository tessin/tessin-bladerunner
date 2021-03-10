using System;
using System.Collections.Generic;
using System.Text;
using LINQPad;
using LINQPad.Controls;

namespace Tessin.Bladerunner
{
    public static class ControlExtensions
    {
        public static void SetClass(this Control control, string @class)
        {
            control.HtmlElement.SetAttribute("class", @class);
        }

        public static void SetVisibility(this Control control, bool value)
        {
            control.HtmlElement.SetAttribute("style", "display:" + (value ? "block" : "none"));
        }

        public static void SetVisibility(this DumpContainer control, bool value)
        {
            control.Style = "display:" + (value ? "block" : "none");
        }

        public static void ClearStyles(this Table table)
        {
            foreach (var style in table.Styles)
            {
                table.Styles[style.Key] = null;
            }
            foreach (var style in table.CellStyles)
            {
                table.CellStyles[style.Key] = null;
            }
            foreach (var style in table.HeaderStyles)
            {
                table.HeaderStyles[style.Key] = null;
            }
            foreach (var style in table.RowStyles)
            {
                table.RowStyles[style.Key] = null;
            }
        }

        public static void OnUpdate(this Control control, Action onUpdate)
        {
            if (control is TextBox textBox)
            {
                textBox.TextInput += (_, __) =>
                {
                    onUpdate();
                };
            }
            if (control is CheckBox checkBox)
            {
                checkBox.Click += (_, __) =>
                {
                    onUpdate();
                };
            }
            if (control is DataListBox dataListBox)
            {
                dataListBox.TextInput += (_, __) =>
                {
                    onUpdate();
                };
            }
            if (control is TextArea textArea)
            {
                textArea.TextInput += (_, __) =>
                {
                    onUpdate();
                };
            }
            if (control is FilePicker filePicker)
            {
                filePicker.TextInput += (_, __) =>
                {
                    onUpdate();
                };
            }
        }
    }
}
