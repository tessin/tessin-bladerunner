using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LINQPad;
using LINQPad.Controls;
using Tessin.Bladerunner.Blades;
using Tessin.Bladerunner.Controls;

namespace Tessin.Bladerunner
{
    public static class ControlExtensions
    {
        public static T SetClass<T>(this T control, string @class) where T : Control
        {
            if (control.IsRendered)
            {
                control.HtmlElement["className"] = @class;
            }
            else
            {
                control.HtmlElement.SetAttribute("class", @class);
            }
            return control;
        }

        public static string GetClass<T>(this T control) where T : Control
        {
            if (control.IsRendered)
            {
                return control.HtmlElement["className"];
            }
            else
            {
                return control.HtmlElement.GetAttribute("class");
            }
        }

        public static T AddClass<T>(this T control, string @class) where T : Control
        {
            var current = GetClass(control);
            if (string.IsNullOrEmpty(current) || current.Split(' ').All(e => e != @class))
            {
                control.SetClass($"{current} {@class}".Trim());
            }
            return control;
        }

        public static T RemoveClass<T>(this T control, string @class) where T : Control
        {
            var current = GetClass(control);
            if(current != null)
            {
                control.SetClass(string.Join(" ", current.Split(' ').Where(e => e != @class).ToArray()));
            }
            return control;
        }

        public static T SetVisibility<T>(this T control, bool value) where T : Control
        {
            control.Styles["display"] = (value ? "block" : "none");
            return control;
        }

        public static DumpContainer Show(this DumpContainer control)
        {
            control.SetVisibility(true);
            return control;
        }

        public static DumpContainer Hide(this DumpContainer control)
        {
            control.SetVisibility(false);
            return control;
        }

        public static T Show<T>(this T control) where T : Control
        {
            control.SetVisibility(true);
            return control;
        }

        public static T Hide<T>(this T control) where T : Control
        {
            control.SetVisibility(false);
            return control;
        }

        public static void SetVisibility(this DumpContainer control, bool value)
        {
            control.Style = "display:" + (value ? "block" : "none");
        }

        public static void ClearStyles(this Controls.Table table)
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

        public static void AddPadding(DumpContainer dc, object content)
        {
            DumpContainer Wrap(object c)
            {
                var inner = new DumpContainer
                {
                    Content = c
                };
                if (c is not INoContainerPadding noPadding)
                {
                    dc.Style = "padding:10px;width:100%;box-sizing:border-box;";
                }
                return inner;
            }

            if (content is Task<object> contentTask)
            {
                content = Task.Run(async () =>
                {
                    object result = await contentTask;
                    return Wrap(result);
                });
            }
            else
            {
                content = Wrap(content);
            }

            dc.Content = content;
        }

        public static void OnUpdate(this Control control, Action onUpdate)
        {
            if (control is LINQPad.Controls.TextBox textBox)
            {
                textBox.TextInput += (_, __) =>
                {
                    onUpdate();
                };
            }
            if (control is Controls.CheckBox checkBox)
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
            if (control is Controls.TextArea textArea)
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
