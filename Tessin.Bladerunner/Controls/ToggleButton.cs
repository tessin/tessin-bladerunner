
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

        public ToggleButton(bool state, Func<bool,Task<bool>> update)
        {
            this.SetClass("toggle-button");
            _checkBox = new CheckBox("", isChecked: state, async (_) =>
            {
                var result = await update(_checkBox.Checked);
            });
            this.VisualTree.Add(_checkBox);
            var span = new Span();
            span.SetClass("toggle-button--track");
            var label = new Control("label", new[] { span });
            label.HtmlElement.SetAttribute("for", _checkBox.HtmlElement.ID);
            this.VisualTree.Add(label);
        }

        public bool State => _checkBox.Checked;
    }
}
