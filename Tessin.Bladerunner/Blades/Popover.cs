using LINQPad;
using LINQPad.Controls;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tessin.Bladerunner.Blades
{
    public class Popover : Div
    {
        static Popover() {
            Util.HtmlHead.AddScript(Javascript.Popover);
        }

        private DumpContainer _dc;
        private string _lastTargetId;
        private string _lastWrapperId;

        public Popover()
        {
            _dc = new DumpContainer();
            _dc.ContentChanged += _ContentChanged;
            this.SetClass("popover");
            this.VisualTree.Add(_dc);
        }

        private void _ContentChanged(object sender, EventArgs e)
        {
            Util.InvokeScript(false, "PopoverSetPosition", _lastWrapperId, _lastTargetId);
        }

        public void Show(Control content, string targetId)
        {
            var wrapper = new Div(content);
            wrapper.SetClass("popover-wrapper");
            _lastWrapperId = wrapper.HtmlElement.ID;
            _lastTargetId = targetId;
            _dc.Content = wrapper;
        }

        public void Clear()
        {
            _dc.ClearContent();
        }
    }
}
