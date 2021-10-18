using LINQPad;
using LINQPad.Controls;
using System;
using System.Collections.Generic;
using System.Text;
using Tessin.Bladerunner.Controls;

namespace Tessin.Bladerunner.Blades
{
    public enum ToasterType
    {
        Normal,
        Error
    }


    public class Toaster : Div
    {
        static Toaster() {
            Util.HtmlHead.AddScript(Javascript.Toaster);
        }

        private DumpContainer _dc;
        private string _lastWrapperId;
        private int _timeout;
        private bool _cleared;

        public Toaster()
        {
            _dc = new DumpContainer();
            _dc.ContentChanged += _ContentChanged;
            this.SetClass("toaster");
            this.VisualTree.Add(_dc);
        }

        private void _ContentChanged(object sender, EventArgs e)
        {
            if (!_cleared)
            {
                Util.InvokeScript(false, "ToasterSetPosition", _lastWrapperId, _timeout);
            }
        }

        public void Show(Control content, int timeout = 3000, ToasterType type = ToasterType.Normal)
        {
            _cleared = false;
            _timeout = timeout;
            var closeBtn = new IconButton(Icons.Close, (_) =>
            {
                Clear();
            });
            closeBtn.AddClass("toaster-close");
            var wrapper = new Div(content, closeBtn);
            wrapper.SetClass($"toaster-wrapper toaster-{type.ToString().ToLower()}");
            _lastWrapperId = wrapper.HtmlElement.ID;
            _dc.Content = wrapper;
        }

        public void Clear()
        {
            _cleared = true;
            _dc.ClearContent();
        }
    }
}
