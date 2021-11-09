
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Text;
using LINQPad.Controls;

namespace Tessin.Bladerunner.Controls
{
    public class ToggleButton : Div, IRefreshable
    {
        public event RefreshEvent Updated;

        CheckBox _checkBox;

        public ToggleButton(bool state, Func<ToggleButton,Task> onUpdate, Action<Exception> onError = null, string onLabel = "", string offLabel = "")
        {
            this.SetClass("toggle-button");
            _checkBox = new CheckBox("", isChecked: state, async (_) =>
            {
                _checkBox.Enabled = false;
                this.AddClass("toggle-button--loading");
                try
                {
                    await onUpdate(this);
                    Updated?.Invoke(_checkBox.Checked);
                }
                catch (Exception ex)
                {
                    _checkBox.Checked = !_checkBox.Checked;
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
            });
            this.VisualTree.Add(_checkBox);
            
            var sliderSpan = new Span();
            sliderSpan.SetClass("toggle-button--slider");
            this.VisualTree.Add(sliderSpan);
            
            var onSpan = new Span(onLabel);
            onSpan.SetClass("toggle-button--on");
            this.VisualTree.Add(onSpan);
            
            var offSpan = new Span(offLabel);
            offSpan.SetClass("toggle-button--off");
            this.VisualTree.Add(offSpan);
        }

        public bool State => _checkBox.Checked;
    }
}
