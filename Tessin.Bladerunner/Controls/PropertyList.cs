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
        public Property(string label, object value)
        {
            Label = label;
            Value = value;
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
                var label = new Span(prop.Label);
                var dcValue = new DumpContainer { Content = prop.Value };
                var divProp = new Div(label, dcValue);
                this.VisualTree.Add(divProp);
            }
        }
    }

}
