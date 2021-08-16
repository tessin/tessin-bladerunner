namespace Tessin.Bladerunner
{
    public class Option
    {
        public Option(string label, object value)
        {
            Label = label;
            Value = value;
        }

        public Option(string label) : this(label, label)
        {
        }

        public string Label { get; set; }

        public object Value { get; set; }

        public override string ToString()
        {
            return Label;
        }
    }
}