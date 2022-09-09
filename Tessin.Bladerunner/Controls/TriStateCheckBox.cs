using System;
using LINQPad.Controls;

namespace Tessin.Bladerunner.Controls
{
    public sealed class TriStateCheckBox  : Control, IRefreshable
    {
        public event RefreshEvent Updated;

        private readonly Control InputControl;
        private readonly Control CaptionControl;

        public TriStateCheckBox(string text = "", bool? isChecked = false, Action<TriStateCheckBox> onClick = null)
        : this(new Span(text), isChecked, onClick)
        {
        }
        
        public TriStateCheckBox(Control captionControl, bool? isChecked = false, Action<TriStateCheckBox> onClick = null)
            : this("checkbox", captionControl, isChecked, onClick)
        {
        }

        private bool? _state;

        private TriStateCheckBox(
            string type,
            Control captionControl,
            bool? isChecked,
            Action<TriStateCheckBox> onClick)
            : base("label")
        {
            TriStateCheckBox checkBox = this;
            this.HtmlElement["class"] = "tri-state-checkbox checkbox-label";
            this.VisualTree.Add(this.InputControl = new Control("input", Array.Empty<Control>()));
            this.VisualTree.Add(this.CaptionControl = captionControl);
            this.InputControl.HtmlElement[nameof (type)] = type;
            this.InputControl.HtmlElement["class"] = "checkbox-label";

            this.Checked = isChecked;

            if (onClick != null)
            {
                this.Click += (EventHandler) ((sender, args) => onClick(checkBox));
            }
                
            this.Click += (EventHandler) ((sender, args) =>
            {
                if (Updated != null) Updated(Checked);
            });
            
            this.Click += (EventHandler) ((sender, args) =>
            {
                Checked = _state switch
                {
                    true => null,
                    false => true,
                    null => false
                };
            });
        }
        
        public bool? Checked
        {
            get
            {
                if (this.InputControl.HasClass("indeterminate")) return null;
                return this.InputControl.HtmlElement["checked"]?.ToLowerInvariant() == "true";
            }
            set
            {
                this.InputControl.HtmlElement["checked"] = (value==null || value.Value) ? "true" : (string) null;

                if (value == null)
                {
                    this.InputControl.AddClass("indeterminate");
                }
                else
                {
                    this.InputControl.RemoveClass("indeterminate");
                }

                _state = value;
            }
        }

        public string Text
        {
            get => this.CaptionControl.HtmlElement.InnerText ?? "";
            set => this.CaptionControl.HtmlElement.InnerText = value ?? "";
        }
        
    }
}