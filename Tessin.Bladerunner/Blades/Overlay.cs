using System;
using System.Collections.Generic;
using System.Text;
using LINQPad.Controls;

namespace Tessin.Bladerunner.Blades
{
    public class Overlay : Control
    {
        private readonly Control _divOverlay;

        public Overlay()
        {
            _divOverlay = new Div().SetClass("overlay");
            VisualTree.Add(_divOverlay);
            Hide();
        }

        public void Hide()
        {
            _divOverlay.SetVisibility(false);
        }

        public void Show()
        {
            _divOverlay.SetVisibility(true);
        }
    }
}
