using System;
using System.Collections.Generic;
using System.Text;
using LINQPad.Controls;

namespace Tessin.Bladerunner.Blades
{
    public class Overlay : Control
    {
        private readonly Control _divOverlay;

        private int stack = 0;

        public Overlay()
        {
            _divOverlay = new Div().SetClass("overlay");
            Hide();
            VisualTree.Add(_divOverlay);
        }

        public void Hide()
        {
            stack = Math.Max(stack - 1, 0);
            if (stack == 0)
            {
                _divOverlay.SetVisibility(false);
            }
        }

        public void Show()
        {
            stack++;
            _divOverlay.SetVisibility(true);
        }
    }
}
