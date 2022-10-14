using LINQPad.Controls;

namespace Tessin.Bladerunner.Controls
{
    public class Property
    {
        internal IContentFormatter _formatter;

        public Property(string label, object value, IContentFormatter formatter = null, bool isMultiLine = false)
        {
            Label = label;
            Value = value;
            IsMultiLine = isMultiLine;
            _formatter = formatter ?? new DefaultContentFormatter();
        }

        public Property(string label) : this(label, label)
        {
        }

        public string Label { get; set; }

        public object Value { get; set; }

        public bool IsMultiLine { get; set; }

        public bool IsRemoved { get; set; }

        public override string ToString()
        {
            return Label;
        }
    }

    public class PropertyList : Div
    {
        public PropertyList(params Property[] properties) : base()
        {
            this.AddClass("property-list");

            foreach (var prop in properties)
            {
                var label = new Span(prop.Label == "_" ? "" : prop.Label);
                var divProp = new Div(label, prop._formatter.Format(prop.Value));
                if (prop.IsMultiLine)
                {
                    divProp.SetClass("multi-line");
                }
                this.VisualTree.Add(divProp);
            }
        }
    }

}
