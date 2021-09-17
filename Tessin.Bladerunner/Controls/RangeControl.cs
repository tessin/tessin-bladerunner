using System;
using System.Collections.Generic;
using System.Text;

namespace Tessin.Bladerunner.Controls
{
    public class RangeControl : LINQPad.Controls.RangeControl
    {
        public RangeControl(int min, int max, int value) : base(min, max, value)
        {
        }

        public RangeControl(int min, int max) : base(min, max)
        {
        }
    }
}
