using System;
using System.Collections.Generic;
using System.Text;
using LINQPad.Controls;

namespace Tessin.Bladerunner.Controls
{
    public class SelectBox : LINQPad.Controls.SelectBox
    {
        public SelectBox(object[] options = null, int selectedIndex = 0, Action<LINQPad.Controls.SelectBox> onSelectionChanged = null) : base(options, selectedIndex, onSelectionChanged)
        {
            this.Width = "-webkit-fill-available";
        }

        public SelectBox(params string[] options) : base(options)
        {
            this.Width = "-webkit-fill-available";
        }

        public SelectBox(SelectBoxKind kind, params string[] options) : base(kind, options)
        {
            this.Width = "-webkit-fill-available";
        }

        public SelectBox(SelectBoxKind kind, object[] options = null, int selectedIndex = 0, Action<LINQPad.Controls.SelectBox> onSelectionChanged = null) : base(kind, options, selectedIndex, onSelectionChanged)
        {
            this.Width = "-webkit-fill-available";
        }

        protected SelectBox(string type, SelectBoxKind kind, object[] options, int selectedIndex, Action<LINQPad.Controls.SelectBox> onSelectionChanged) : base(type, kind, options, selectedIndex, onSelectionChanged)
        {
            this.Width = "-webkit-fill-available";
        }
    }
}
