namespace Tessin.Bladerunner
{
    public class Option
    {
        public Option()
        {

        }

        public Option(string label, object value, object tag = null)
        {
            Label = label;
            Value = value;
            Tag = tag;
        }

        public Option(string label) : this(label, label)
        {
        }

        public string Label { get; set; }

        public object Value { get; set; }

        public object Tag { get; set; }

        public override string ToString()
        {
            return Label;
        }
    }
}