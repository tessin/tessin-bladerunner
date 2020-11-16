namespace Tessin.Bladerunner.Editors
{
    public class Option
    {
        public Option(string label, object value)
        {
            Label = label;
            Value = value;
        }

        public string Label { get; set; }

        public object Value { get; set; }

    }
}