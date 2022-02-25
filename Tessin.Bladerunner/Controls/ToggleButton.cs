using LINQPad.Controls;
using System;
using System.Threading.Tasks;

namespace Tessin.Bladerunner.Controls
{
    public class ToggleButton : Div, IRefreshable
    {
        public event RefreshEvent Updated;

        Control _checkBox;

        public ToggleButton(bool state, Func<ToggleButton, Task> onUpdate, Action<Exception> onError = null, string onLabel = "", string offLabel = "")
        {
            this.SetClass("toggle-button");
            _checkBox = new Control("input");
            _checkBox.HtmlElement.SetAttribute("type", "checkbox");

            Checked = state;

            _checkBox.Click += async (sender, args) =>
            {
                _checkBox.Enabled = false;
                this.AddClass("toggle-button--loading");
                try
                {
                    await onUpdate(this);
                    Updated?.Invoke(Checked);
                }
                catch (Exception ex)
                {
                    Checked = !Checked;
                    if (onError != null)
                    {
                        onError(ex);
                    }
                }
                finally
                {
                    _checkBox.Enabled = true;
                    this.RemoveClass("toggle-button--loading");
                }
            };

            this.VisualTree.Add(_checkBox);

            var sliderSpan = new Span();
            sliderSpan.SetClass("toggle-button--slider");

            var onSpan = new Span(onLabel);
            onSpan.SetClass("toggle-button--on");

            var offSpan = new Span(offLabel);
            offSpan.SetClass("toggle-button--off");

            var label = new Control("label", sliderSpan, onSpan, offSpan);
            label.HtmlElement.SetAttribute("for", _checkBox.HtmlElement.ID);

            this.VisualTree.Add(label);
        }

        public bool Checked
        {
            get => this._checkBox.HtmlElement["checked"]?.ToLowerInvariant() == "true";
            set => this._checkBox.HtmlElement["checked"] = value ? "true" : (string)null;
        }
    }
}
