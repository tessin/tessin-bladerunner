using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LINQPad;
using LINQPad.Controls;

namespace Tessin.Bladerunner.Controls
{
    public class Property
    {
        internal IContentFormatter _formatter;

        public Property(string label, object value, IContentFormatter formatter = null)
        {
            Label = label;
            Value = value;
            _formatter = formatter ?? new ContentFormatter();
        }

        public Property(string label) : this(label, label)
        {
        }

        public string Label { get; set; }

        public object Value { get; set; }

        public override string ToString()
        {
            return Label;
        }
    }

    public class PropertyList : Div
    {
        public PropertyList(params Property[] properties) : base()
        {
            this.SetClass("property-list");

            foreach (var prop in properties)
            {
                var label = new Span(prop.Label == "_" ? "" : prop.Label);
                var divProp = new Div(label, prop._formatter.Format(prop.Value));
                this.VisualTree.Add(divProp);
            }
        }
    }

}
