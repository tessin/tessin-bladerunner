using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LINQPad;
using LINQPad.Controls;

namespace Tessin.Bladerunner
{
    public static class ControlExtensions
    {
        public static T SetClass<T>(this T control, string @class) where T : Control
        {
            control.HtmlElement.SetAttribute("class", @class);
            return control;
        }

        public static T AddClass<T>(this T control, string @class) where T : Control
        {
            var current = control.HtmlElement.GetAttribute("class");
            if (current.Split(' ').All(e => e != @class))
            {
                control.HtmlElement.SetAttribute("class", $"{current} {@class}");
            }
            return control;
        }

        public static T SetVisibility<T>(this T control, bool value) where T : Control
        {
            control.Styles["display"] = (value ? "block" : "none");
            return control;
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
